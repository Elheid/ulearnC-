using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public enum NGrams
            {
                Bigrams,
                Trigrams
            }

        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();
            var bigramsDictionary = new Dictionary<string, Dictionary<string, int>>();
            var trigramsDictionary = new Dictionary<string, Dictionary<string, int>>();
            bigramsDictionary = GainNGigramsDictionary(text,bigramsDictionary,NGrams.Bigrams);
            trigramsDictionary = GainNGigramsDictionary(text, trigramsDictionary, NGrams.Trigrams);
            OrganizeDictionary(bigramsDictionary, result);
            OrganizeDictionary(trigramsDictionary, result);
            return result;
        }

        public static void StuffDictionary(Dictionary<string, Dictionary<string, int>> nGramsDictionary, Dictionary<string, int> nGramsKeys,
             List<string> sentences, string sentence, string nSentence)
        {
            var combinationsValues = 1;
            if (!nGramsDictionary.ContainsKey(sentence))
                nGramsDictionary.Add(sentence, nGramsKeys);
            if (!nGramsDictionary[sentence].ContainsKey(nSentence))
                nGramsDictionary[sentence].Add(nSentence, combinationsValues);
            else
                nGramsDictionary[sentence][nSentence] += 1;
        }

        public static Dictionary<string, Dictionary<string, int>> GainNGigramsDictionary(List<List<string>> text,
             Dictionary<string, Dictionary<string, int>> nGramsDictionary,NGrams nGrams)
        {
            foreach (var sentences in text)
            {
                var n = nGrams == NGrams.Bigrams ? 1 : 2;
                for (var i = 0; i < sentences.Count - n; i++)
                {
                    var sentence = sentences[i];
                    var secondSentence = sentences[i + 1];
                    var bigramSentence = sentence + " " + secondSentence;
                    var thirdSentence = nGrams == NGrams.Trigrams ? sentences[i + 2] : "";
                    var bigramsKeys = new Dictionary<string, int>();
                    var trigramsKeys = new Dictionary<string, int>();

                    if (nGrams == NGrams.Bigrams)
                    {
                        StuffDictionary(nGramsDictionary, bigramsKeys, sentences, sentence, secondSentence);
                    }
                    else
                    {
                        StuffDictionary(nGramsDictionary, trigramsKeys, sentences, bigramSentence, thirdSentence);
                    }
                }
            }
            return nGramsDictionary;
        }

        public static void OrganizeDictionary(Dictionary<string, Dictionary<string, int>> nGrams,
             Dictionary<string, string> result)
        {
            foreach (var combinations in nGrams)
            {
                var maximum = 0;
                var frequentCombinations = "";
                foreach (var pairsSentences in combinations.Value)
                {
                    var compareOfCombinations = (string.CompareOrdinal(frequentCombinations, pairsSentences.Key) > 0);
                    if (pairsSentences.Value == maximum && compareOfCombinations)
                        frequentCombinations = pairsSentences.Key;
                    if (pairsSentences.Value > maximum)
                    {
                        frequentCombinations = pairsSentences.Key;
                        maximum = pairsSentences.Value;
                    }
                }
                result.Add(combinations.Key, frequentCombinations);
            }
        }
    }
}


