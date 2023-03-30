using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        static int valueNotFromString = -5;
        static char[] separationSigns = new char[] { '.', '!', '?', ';', ':', '(', ')' };
        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            var sentences = ChangeTextIntoNeededSentences(text);
            foreach (var sentence in sentences)
            {
                var listSymbols = new List<string>();
                var startSymbol = valueNotFromString;
                CompleteListOfWords(sentence, startSymbol, listSymbols);
                if (listSymbols.Count > 0)
                    sentencesList.Add(listSymbols);
            }
            return sentencesList;
        }

        static string[] ChangeTextIntoNeededSentences(string text)
        {
            return text.ToLower().Split(separationSigns, StringSplitOptions.RemoveEmptyEntries);
        }

        static int SearchStartSymbolAndAddWord(bool charIsValidSymbol, int startSymbol, int iteration, string sentence, List<string> words)
        {
            if (charIsValidSymbol && startSymbol == valueNotFromString)
            {
                startSymbol = iteration;
            }
            else if (!charIsValidSymbol && startSymbol != valueNotFromString)
            {
                words.Add(sentence.Substring(startSymbol, iteration - startSymbol));
                startSymbol = valueNotFromString;
            }

            return startSymbol;
        }

        static void CompleteListOfWords(string sentence, int startSymbol, List<string> words)
        {
            for (var i = 0; i < sentence.Length; i++)
            {
                var charIsValidSymbol = (char.IsLetter(sentence[i]) || sentence[i] == ('\''));

                startSymbol = SearchStartSymbolAndAddWord(charIsValidSymbol, startSymbol, i, sentence, words);

                if (i == sentence.Length - 1 && startSymbol != valueNotFromString)
                    words.Add(sentence.Substring(startSymbol, sentence.Length - startSymbol));
            }
        }
    }
}