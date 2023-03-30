using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("'a\\\' b'", 0, "a' b", 7)]
        public static void RunTests()
        {
            Test("hello world", 0 , "hello", 5 );
            Test("hello  world", 5, "world", 5);
            Test(String.Empty, 0, String.Empty, 0);
            //Test("'x y'", new[] { "x y" });
            //Test(@"""a 'b' 'c' d""", new[] { "a 'b' 'c' d" });
            //Test(@"'""1""", new[] { @"""1""" });
            //Test(@"a""b c d e""", new[] { "a", "b c d e" });
            //Test(@"""b c d e""f", new[] { "b c d e", "f" });
            //Test(" 1 ", new[] { "1" });
            //Test(@"""a \""c\""""", new[] { @"a ""c""" });
            //Test(@"""\\""", new[] { "\\" });
            //Test(@" '' ", new[] { "" });
            //Test(@"'a\'a\'a'", new[] { "a'a'a" });
            //Test("'x ", new[] { "x " });
        }

        public static void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
        public void MyTests(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(actualToken, new Token(expectedValue, startIndex, expectedLength));
        }
    }

    class QuotedFieldTask
    {
        private static bool NewCharIsEcran(StringBuilder builder, char currentChar, bool flag)
        {
            if (flag)
            {
                builder.Append(currentChar);
                return false;
            }
            return true;
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            var builder = new StringBuilder();
            var flag = false;
            var currentIndex = startIndex;
            var charFirst = line[currentIndex++];

            while (currentIndex < line.Length)
            {
                var currentChar = line[currentIndex++];

                if (currentChar == '\\')
                {
                    flag = NewCharIsEcran(builder, currentChar, flag);
                }

                else if (currentChar == charFirst)
                {
                    if (!flag)
                    {
                        break;
                    }

                    flag = NewCharIsEcran(builder, currentChar, flag);
                }
                else builder.Append(currentChar);
            }
            return new Token(builder.ToString(), startIndex, currentIndex - startIndex);
            //Token token = null;
            //int length = 0;
            //int openedMarker = -1; //маркер открывающей кавычки

            ////пропустить пробелы в начале
            //while (startIndex < line.Length && line[startIndex] == ' ')
            //    startIndex++;

            ////собираем токен (бежим по строке пока не встретим кавычки или пробел)            
            //for (int i = startIndex; i < line.Length; i++)
            //{
            //    //дошел до закрывающей кавычки (и кавычка не экранирована)
            //    if (openedMarker != -1 && line[i] == line[openedMarker])
            //    {
            //        //проверяем экранирование нечетным количеством слэшей перед закрывающей кавычкой
            //        int slashesCount = 0;
            //        for (int j = i - 1; j > openedMarker; j--)
            //        {
            //            if (line[j] == '\\')
            //                slashesCount++;
            //            else
            //                break;
            //        }

            //        if (slashesCount % 2 == 0) //не экранирована
            //        {
            //            if (openedMarker + 1 != i) //если закрывающая кавычка не стояла сразу следом за своей открывающей, то берем токен между ними
            //            {
            //                length = i - openedMarker + 1;
            //                if (length > 0) token = new Token(line.Substring(openedMarker + 1, length - 2), openedMarker, length);
            //                break;
            //            }
            //            else //иначе обнуляем открывающую кавычку, задаем начальный индекс следущим от текущей позиции, пропускаем текущую позицию и идем дальше
            //            {
            //                openedMarker = -1;
            //                startIndex = i + 1;
            //                continue;
            //            }
            //        }
            //    }

            //    //нашел открывающий маркер-кавычку (и кавычка не экранирована)                
            //    if (openedMarker == -1 && (line[i] == '\'' || line[i] == '\"'))
            //    {
            //        if (i == startIndex) //от текущей кавычки до следущей наш токен
            //            openedMarker = i; //запоминаем открывающую кавычку
            //        else //до текущей кавычки наш токен
            //        {
            //            length = i - startIndex;
            //            token = new Token(line.Substring(startIndex, length), startIndex, length);
            //            break;
            //        }
            //    }

            //    //дошел до пробела
            //    if (openedMarker == -1 && line[i] == ' ') //пробелы стопают поиск только если маркер еще не встречал
            //    {
            //        length = i - startIndex;
            //        token = new Token(line.Substring(startIndex, length), startIndex, length);
            //        break;
            //    }

            //    //дошел до конца строки
            //    if (i == line.Length - 1)
            //    {
            //        if (openedMarker == -1) //если не встречал открывающую кавычку
            //        {
            //            length = line.Length - startIndex;
            //            token = new Token(line.Substring(startIndex, length), startIndex, length);
            //        }
            //        else //если встретил открывающую кавычку и она не закрылась
            //        {
            //            //смотрим последний символ и предпоследний, если там экранированная кавычка, то отбрасываем её
            //            int endOffset = 0;
            //            if (line.Length > 2 && line[line.Length - 1 - endOffset] == line[openedMarker] && line[line.Length - 1 - endOffset - 1] == '\\')
            //                endOffset = 1;

            //            length = line.Length - openedMarker;
            //            if (length > 0) token = new Token(line.Substring(openedMarker + 1, length - 1 - endOffset), openedMarker, length);
            //        }
            //    }
            //}

            //return token;
        }
    }
}
