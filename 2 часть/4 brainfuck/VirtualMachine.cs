using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }
		Dictionary<char, Action<IVirtualMachine>> dictionaryOfCommands;

        public VirtualMachine(string program, int memorySize)
		{
			InstructionPointer = 0;
			Memory = new byte[memorySize];
			Instructions = program;
            dictionaryOfCommands = new Dictionary<char, Action<IVirtualMachine>>();
        }

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			if (!dictionaryOfCommands.ContainsKey(symbol))
				dictionaryOfCommands.Add(symbol, execute);
		}

		public void Run()
		{
			while (InstructionPointer < Instructions.Length)
			{
				var instruction = Instructions[InstructionPointer];
				if (dictionaryOfCommands.ContainsKey(instruction))
					dictionaryOfCommands[instruction].Invoke(this);
                InstructionPointer++;
            }
		}
	}
}