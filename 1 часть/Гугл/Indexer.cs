using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketGoogle
{
    public class IndexererExtensions
    {
        public static readonly char[] separators;
        public static readonly List<int> EmptyList;
        public Dictionary<string, HashSet<int>> DictionaryWordWithIndexes;
        public IndexererExtensions()
        {
            DictionaryWordWithIndexes = new Dictionary<string, HashSet<int>>();
        }

        static IndexererExtensions()
        {
            separators = new char[] { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };
            EmptyList = new List<int>();
        }
    }

    public class Indexer : IIndexer
    {
        //HashSet - колекция не содержащяя повтор элементов 
        //с высокопроизводительными методами, можно вместо List
        private Dictionary<string, HashSet<int>> wordWithDocumentId
            = new Dictionary<string, HashSet<int>>();
        private Dictionary<int, IndexererExtensions> wordByDocumentsWithIndexes
            = new Dictionary<int, IndexererExtensions>();
        private int wordIndex;
        public int WordIndex 
        { 
            get { return wordIndex; } 
            set 
            { 
                if (value < 0) throw new ArgumentException();
                wordIndex = value; 
            } 
        }

        public void Add(int documentID, string documentText)
        {
            var indexes = new IndexererExtensions();
            var separatedWords = documentText.Split(IndexererExtensions.separators).ToList();
            wordByDocumentsWithIndexes.Add(documentID, indexes);
            wordIndex = 0;
            foreach (var word in separatedWords)
            {
                var nextWord = word.Length + 1;
                FillDictionaries(documentID, word, wordIndex);
                wordIndex += nextWord;
            }
        }

        public List<int> GetIds(string word)
        {
            if (wordWithDocumentId.ContainsKey(word))
                return wordWithDocumentId[word].ToList();
            return IndexererExtensions.EmptyList;
        }

        public List<int> GetPositions(int documentID, string word)
        {
            if (wordByDocumentsWithIndexes[documentID].DictionaryWordWithIndexes.ContainsKey(word))
                return wordByDocumentsWithIndexes[documentID].DictionaryWordWithIndexes[word].ToList();
            return IndexererExtensions.EmptyList;
        }

        public void Remove(int documentID)
        {
            var wordsInThisDocument = wordByDocumentsWithIndexes[documentID].DictionaryWordWithIndexes;
            foreach (var word in wordsInThisDocument.Keys)
            {
                wordWithDocumentId[word].Remove(documentID);
            }
            wordByDocumentsWithIndexes.Remove(documentID);
        }

        public void FillDictionaries(int currentID, string currentWord, int wordIndex)
        {
            var notContainCurrentWord = !wordWithDocumentId.ContainsKey(currentWord);
            if (!wordByDocumentsWithIndexes[currentID].DictionaryWordWithIndexes.ContainsKey(currentWord))
            {
                wordByDocumentsWithIndexes[currentID].DictionaryWordWithIndexes.Add(currentWord, new HashSet<int>());
            }
            if (notContainCurrentWord)
                wordWithDocumentId.Add(currentWord, new HashSet<int>());
            var isContaiCurrentID = !wordWithDocumentId[currentWord].Contains(currentID);
            var isIDAdd = isContaiCurrentID ? wordWithDocumentId[currentWord].Add(currentID) : false;
            wordByDocumentsWithIndexes[currentID].DictionaryWordWithIndexes[currentWord].Add(wordIndex);
        }
    }
}



//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PocketGoogle
//{

//    public class Indexer : IIndexer
//    {
//        private readonly Dictionary<int, Dictionary<string, List<int>>> wordsAndPosByIndex = new Dictionary<int, Dictionary<string, List<int>>>();
//        private readonly Dictionary<string, HashSet<int>> indexesByWord = new Dictionary<string, HashSet<int>>();

//        public void Add(int id, string documentText)
//        {
//            var charsForSplit = new char[] { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };
//            string[] words = documentText.Split(charsForSplit);
//            wordsAndPosByIndex.Add(id, new Dictionary<string, List<int>>());

//            int currentPos = 0;
//            foreach (string word in words)
//            {
//                if (!indexesByWord.ContainsKey(word))
//                    indexesByWord[word] = new HashSet<int>();

//                if (!indexesByWord[word].Contains(id))
//                    indexesByWord[word].Add(id);


//                if (!wordsAndPosByIndex[id].ContainsKey(word))
//                    wordsAndPosByIndex[id].Add(word, new List<int>());

//                wordsAndPosByIndex[id][word].Add(currentPos);
//                currentPos += word.Length + 1;
//            }
//        }

//        public List<int> GetIds(string word)
//        {
//            return indexesByWord.ContainsKey(word) ? indexesByWord[word].ToList() : new List<int>();
//        }

//        public List<int> GetPositions(int id, string word)
//        {
//            List<int> positions = new List<int>();

//            if (wordsAndPosByIndex.ContainsKey(id) && wordsAndPosByIndex[id].ContainsKey(word))
//                positions = wordsAndPosByIndex[id][word];

//            return positions;
//        }

//        public void Remove(int id)
//        {
//            string[] words = wordsAndPosByIndex[id].Keys.ToArray();

//            foreach (var word in words)
//                indexesByWord[word].Remove(id);

//            wordsAndPosByIndex.Remove(id);
//        }
//    }
//}