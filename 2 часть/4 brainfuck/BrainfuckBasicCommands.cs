using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
    public enum Command
    {
        Previous,
        Next,
    }
    public enum Sign
    {
        Plus,
        Minus,
    }
    public class BrainfuckBasicCommands
    {
        public static string GetAllKeyButtons()
        {
            var charLower = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (Char)i).ToArray();
            var charNumber = Enumerable.Range('0', '9' - '0' + 1).Select(i => (Char)i).ToArray();
            var charUpper = charLower.Select(x => Char.ToUpper(x)).ToArray();
            var symbolsLower = new string(charLower, 0, charLower.Length);
            var symbolsUpper = new string(charUpper, 0, charUpper.Length);
            var symbolsNumber = new string(charNumber, 0, charNumber.Length);
            return symbolsUpper + symbolsLower + symbolsNumber;
        }
        public static void SaveSymbols(IVirtualMachine vm)
        {
            var symbols = GetAllKeyButtons();
            foreach (var symbol in symbols)
            {

                vm.RegisterCommand(symbol, b => b.Memory[b.MemoryPointer] = Convert.ToByte(symbol));
            }
        }

        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            Action<IVirtualMachine> printValue = b => write(Convert.ToChar(b.Memory[b.MemoryPointer]));
            Action<IVirtualMachine> saveValue = b => b.Memory[b.MemoryPointer] = Convert.ToByte(read());

            vm.RegisterCommand('.', printValue);
            vm.RegisterCommand('+', b => CommandToChangeValue(b, Sign.Plus));
            vm.RegisterCommand('-', b => CommandToChangeValue(b, Sign.Minus));
            vm.RegisterCommand(',', saveValue);
            vm.RegisterCommand('>', b => ChangePointer(b, b.Memory.Length - 1, 0)); 
            vm.RegisterCommand('<', b => ChangePointer(b, 0, b.Memory.Length - 1));
            SaveSymbols(vm);
        }

        public static void CommandToChangeValue(IVirtualMachine b, Sign sign)
        {
            if (sign == Sign.Minus)
                { unchecked { b.Memory[b.MemoryPointer]--; } }
            else
                { unchecked { b.Memory[b.MemoryPointer]++; } };
        }
  
        public static void ChangePointer(IVirtualMachine b, int firstBorder, int secondBorder)
        {
            if (b.MemoryPointer != firstBorder)
            {
                if (firstBorder == 0)
                    b.MemoryPointer--;
                else
                    b.MemoryPointer++;
            }
            else
                b.MemoryPointer = secondBorder;
        }
    }
}