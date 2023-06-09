using Avalonia.Controls.Generators;
using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class Command<TItem>
{
	readonly int? index;
	readonly TItem action;
	public int? Index { get { return index; } }
	public TItem Action { get { return action; } }
    public enum CommandName
    {
        AddItem,
        RemoveItem,
        PushItem
    };
	public CommandName CommName;
    public Command(TItem _action, int? _index, CommandName name)
	{
        action = _action;
        index = _index;
		CommName = name;
	}

}

public class ListModel<TItem>
{
	public List<TItem> Items { get; }
	readonly LimitedSizeStack<Command<TItem>> HistoryOfCommand;

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{
	}

	public ListModel(List<TItem> items, int undoLimit)
	{
        HistoryOfCommand = new LimitedSizeStack<Command<TItem>>(undoLimit);
        Items = items;
	}

	public void AddItem(TItem item)
	{
		var newCommand = new Command<TItem> ( item, null, Command<TItem>.CommandName.AddItem);
		Items.Add(item);
		HistoryOfCommand.Push(newCommand);
	}

	public void RemoveItem(int index)
	{
        var newCommand = new Command<TItem>(Items[index], index, Command<TItem>.CommandName.RemoveItem);
        Items.RemoveAt(index);
		HistoryOfCommand.Push(newCommand);
	}

	public void MoveUpItem(int index)
	{
		var itemToPush = Items[index];
		var newCommand = new Command<TItem>(itemToPush, index, Command<TItem>.CommandName.PushItem);
        Items.RemoveAt(index);
        Items.Insert(0, itemToPush);
        HistoryOfCommand.Push(newCommand);
	}

	public bool CanUndo()
	{
		return HistoryOfCommand.Count == 0 ? false : true;
	}

	public void Undo()
	{
		var commandToReturn = HistoryOfCommand.Pop();
		if (commandToReturn.CommName == Command<TItem>.CommandName.AddItem) 
			Items.Remove(commandToReturn.Action);
        else if (commandToReturn.CommName == Command<TItem>.CommandName.RemoveItem)
            Items.Insert((int)commandToReturn.Index,commandToReturn.Action);
		else
		{
            Items.Remove(commandToReturn.Action);
            Items.Insert((int)commandToReturn.Index, commandToReturn.Action);
        }
    }
}