using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Clones;

public class StackItem
{
    private string value;
    private StackItem next;
    private StackItem previous;
    public string Value { get { return value; } }
    public StackItem Next { get { return next; } }
    public StackItem Previous { get { return previous; } }

    public StackItem(string _value, StackItem _next, StackItem _previous)
    {
        value = _value;
        next = _next;
        previous = _previous;
    }

    public static StackItem ChangeNextItem(StackItem item, StackItem newItem)
    {
        item.next = newItem;
        item = newItem;
        return item;
    }
}

public class StackOfProgramms
{
    public StackItem Head { get { return head; } }
    public  StackItem Tail { get { return tail; } }
    private StackItem head;
    private StackItem tail;

    int count;
    public int Count { get { return count; } }

    public bool IsEmpty { get { return Head == null || Tail == null; } }

    public StackOfProgramms DoubleThisStack(StackOfProgramms oldStack)
    {
        tail = oldStack.Tail;
        head = oldStack.Head;
        count = oldStack.count;
        return this;
    }

    public void Push(string programmToPush)
    {
        if (IsEmpty)
        {
            head = new StackItem ( programmToPush, null, null );
            tail = Head;
        }
        else if (count == 1)
        {
            var programm = new StackItem ( programmToPush, null, Head );
            tail = StackItem.ChangeNextItem(tail, programm);
        }
        else
        {
            var programm = new StackItem ( programmToPush, null, Tail );
            tail = StackItem.ChangeNextItem(tail, programm);
        }
        count++;
    }

    public string Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException();
        var removedProgramm = Tail.Value;
        tail = Tail.Previous;
        count--;
        return removedProgramm;
    }
}

public class Clone
{
    public StackOfProgramms RefusedProgramms;
    public StackOfProgramms AcceptedProgramms;

    public Clone()
    {
        RefusedProgramms = new StackOfProgramms();
        AcceptedProgramms = new StackOfProgramms();
    }

    public Clone DoubleThisClone(Clone cloneToCopy)
    {
        var newClone = new Clone();
        var newProgramms = new StackOfProgramms();
        newClone.AcceptedProgramms.DoubleThisStack(cloneToCopy.AcceptedProgramms);
        newClone.RefusedProgramms.DoubleThisStack(cloneToCopy.RefusedProgramms);
        return newClone;
    }
}

public class CloneVersionSystem : ICloneVersionSystem
{
    private List<Clone> clones = new List<Clone>();

    public CloneVersionSystem()
    {
        clones = new List<Clone>();
        clones.Add(new Clone());
    }

    public void RefuseLastCommand(StackOfProgramms stack, string lastCommand)
    {
        stack.Push(lastCommand);
        return;
    }

    public string ReturnLastCommand(StackOfProgramms stack, StackOfProgramms stackToReturn)
    {
        if (stack.IsEmpty) return null;
        RefuseLastCommand(stackToReturn, stack.Pop());
        return null;
    }

    public string ExecuteCommand(string[] wordsOfCommand, int numberOfClone)
    {
        var clone = clones[numberOfClone];
        var commandToExecute = wordsOfCommand[0];
        var cloneCommands = clone.AcceptedProgramms;
        var cancelledCommands = clone.RefusedProgramms;
        switch (commandToExecute)
        {
            case "learn":
                clone.AcceptedProgramms.Push(wordsOfCommand[2]);
                break;
            case "rollback":
                return ReturnLastCommand(cloneCommands, cancelledCommands);
            case "relearn":
                return ReturnLastCommand(cancelledCommands, cloneCommands);
            case "clone":
                var newClone = new Clone();
                clones.Add(newClone.DoubleThisClone(clones[numberOfClone]));
                break;
            case "check":
                return cloneCommands.IsEmpty ? "basic" : cloneCommands.Tail.Value;
        }
        return null;
    }

    public string Execute(string query)
    {
        var splittedCommandString = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var ordinalNumberOfClone = int.Parse(splittedCommandString[1]);
        var numberOfClone = ordinalNumberOfClone - 1;
        return ExecuteCommand(splittedCommandString, numberOfClone);
    }
}



