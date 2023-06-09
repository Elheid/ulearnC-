using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        private static Stack<int> indexOpenBracket = new Stack<int>();
        private static Dictionary<int, int> twinBrackets = new Dictionary<int, int>();

        public static void RecordBrackets(string instructions)
        {
            for (var i = 0; i < instructions.Length; i++)
            {
                if (instructions[i] == '[')
                    indexOpenBracket.Push(i);
                if (instructions[i] == ']')
                {
                    var openBracket = indexOpenBracket.Pop();
                    twinBrackets[openBracket] = i;
                    twinBrackets[i] = openBracket;
                }
            }
        }

        public static void RegisterTo(IVirtualMachine vm)
        {
            RecordBrackets(vm.Instructions);
            Action < IVirtualMachine> openCycle = b => 
            {
                if (b.Memory[b.MemoryPointer] == 0) 
                    b.InstructionPointer = twinBrackets[b.InstructionPointer]; 
            };
            Action<IVirtualMachine> endCycle = b =>
            { 
                if (b.Memory[b.MemoryPointer] != 0) 
                    b.InstructionPointer = twinBrackets[b.InstructionPointer]; 
            };
            vm.RegisterCommand('[', openCycle);
            vm.RegisterCommand(']', endCycle);
        }
    }
}