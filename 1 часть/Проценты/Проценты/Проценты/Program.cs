using System.Globalization;
using System.Text.RegularExpressions;

class Program
{
    static double Calculate(string userInput)
    {
        string[] value = userInput.Split(' ');
        var startSum = Convert.ToDouble(value[0], CultureInfo.InvariantCulture);
        var percentRate = Convert.ToDouble(value[1]) / 100;
        var depositTime = Convert.ToInt32(value[2]);
        return startSum * (Math.Pow(1 + percentRate / 12, depositTime));
    }
    static void Main(string[] args)
    {
        Console.WriteLine("Пожалуйста введите три числа через пробел: исходную сумму, процентную ставку (в процентах) и срок вклада в месяцах.");

        Console.WriteLine(Calculate(Console.ReadLine()));
    }
}

