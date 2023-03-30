using System;
using System.Collections.Generic;
using System.Net;

namespace Passwords
{
    public class CaseAlternatorTask
    {
        //Тесты будут вызывать этот метод
        public static List<string> AlternateCharCases(string lowercaseWord)
        {
            var result = new List<string>();
            AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
            return result;
        }

        static void AlternateCharCases(char[] word, int startIndex, List<string> result)
        {
            if (startIndex == word.Length)
            {
                if (!result.Contains(new string(word)))
                    result.Add(new string(word)); return;
            }
            var symbol = word[startIndex];
            var symbolIsLetter = char.IsLetter(symbol);
            var symbolInLower = ChangeSymbolRegister(symbol,Char.ToLower);
            var symbolInUpper = ChangeSymbolRegister(symbol, Char.ToUpper);
            word[startIndex] = symbolInLower;
            AlternateCharCases(word, startIndex + 1, result);
            word[startIndex] = symbolInUpper;
            AlternateCharCases(word, startIndex + 1, result);
        }   
        static char ChangeSymbolRegister(char symbol, Func<char,char> ChangeRegister)
        {
            var symbolIsLetter = char.IsLetter(symbol);
            return symbolIsLetter ? ChangeRegister(symbol) : symbol;
        }
    }
}