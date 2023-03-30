using System;

namespace Pluralize
{
    public static class PluralizeTask
    {
        internal class Program
        {

            public static string PluralizeRubles(int count)
            {
                var endsWith = count % 10;
                var exceptions = count > 10 ? count % 100 : 0;
                if ((exceptions >= 11 && exceptions <= 19) || (endsWith == 0) || (endsWith > 4))
                    return "рублей";
                else if (endsWith >= 2 || endsWith <= 4)
                    return "рубля";
                else
                    return "рубль";
            }
        }
    }
}