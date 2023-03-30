using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(
            Dictionary<string, string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            var words = new List<string>() { phraseBeginning };
            words = phraseBeginning.Split(' ').ToList();
            var phraseTwoLatters = words.Count >= 2;
            var phraseOneLatters = words.Count >= 1;
            var preLaterIndex = 2;
            for (var i = 0; i < wordsCount; i++)
            {
                var lastWord = words[words.Count - 1];
                if (words.Count >= 2 && nextWords.ContainsKey(words[words.Count - preLaterIndex] + " " + lastWord))
                    words.Add(nextWords[words[words.Count - preLaterIndex] + " " + lastWord]);
                else if (phraseOneLatters && nextWords.ContainsKey(lastWord))
                    words.Add(nextWords[lastWord]);
                else
                    return string.Join(" ", words.ToArray());
            }
            return string.Join(" ", words.ToArray());
        }
    }
}