using System;
using System.Numerics;

namespace Tickets;
public class TicketsTask
{
    public static BigInteger Solve(int halfLen, int totalSum)
    {
        if (totalSum % 2 == 1)
            return 0;
        return CountHappyTickets(halfLen, totalSum);
    }

    public static BigInteger CountHappyTickets(int halfLen, int totalSum)
    {
        var halfSum = totalSum / 2;
        var tickets = InitializeTable(halfLen, halfSum);
        var halfAmountOfTickets = SearchAmountHalfTickets(tickets, halfLen, halfSum);
        return halfAmountOfTickets * halfAmountOfTickets;
    }

    public static BigInteger SearchAmountHalfTickets(BigInteger?[,] tickets, int halfLen, int halfSum)
    {
        var amountOfFirstDigitNum = 10;
        if (tickets[halfLen, halfSum] != null) return (BigInteger)tickets[halfLen, halfSum];
        tickets[halfLen, halfSum] = 0;
        var line = 0;
        while (halfSum >= line && line < amountOfFirstDigitNum)
        {
            var nextLine = halfSum - line;
            var nextColumn = halfLen - 1;
            tickets[halfLen, halfSum] += SearchAmountHalfTickets(tickets, nextColumn, nextLine);
            line++;
        }
        return (BigInteger)tickets[halfLen, halfSum];
    }

    public static BigInteger?[,] InitializeTable(int halfLen, int halfSum)
    {
        var tickets = new BigInteger?[halfLen + 1, halfSum + 1];
        for(var i = 0; i<= halfLen;i++)
        {
            for (var j = 0; j<= halfSum; j++)
            {
                if (j == 0)
                    tickets[i, j] = 1;
                else if (i == 0)
                    tickets[i, j] = 0;
                else
                    tickets[i, j] = null;
            }
        }
        return tickets;
    }
}

