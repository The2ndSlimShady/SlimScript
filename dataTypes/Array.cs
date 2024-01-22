using System;
using System.Collections;

namespace SlimScript;

public class Array : IVariable
{
    private string _name = "";

    public Token Token { get; set; }

    public TokenType Type { get; set; } = TokenType.Array;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            Token = new(_name);
        }
    }

    public List<IVariable> Val { get; set; }

    public object Value
    {
        get => Val;
        set => Val = (List<IVariable>)value;
    }

    internal Array(Token[] items, SourceChunk? chunk)
    {
        Val = new();
        Token = new() { Type = TokenType.Array };

        List<List<Token>> realItems = new(){new()};

        for (int i = 0, j = 0; i < items.Length; i++)
        {
            Token t = items[i];
            string text = t.Text.Trim();

            if (t.Text[0] == '[')
                text = text[1..];
            if (t.Text[^1] == ']')
                text = text[..^1];

            if (string.IsNullOrEmpty(text))
                continue;

            if (text.Contains(','))
            {
                string[] values = text.Split(',');

                if (values.All(s => s.Length == 0))
                {
                    j++;
                    realItems.Add(new());
                    continue;
                }
                else
                {
                    for (int valIndex = 0; valIndex < values.Length; valIndex++)
                    {
                        if (values[valIndex].Length == 0)
                        {
                            j++;
                            realItems.Add(new());
                            continue;
                        }

                        realItems[j].Add(new(values[valIndex]));

                        if (valIndex != values.Length - 1)
                        {
                            j++;
                            realItems.Add(new());
                        }
                    }
                    continue;
                }
            }
			
			realItems[j].Add(new(text));
        }

        for (int i = 0; i < realItems.Count; i++)
        {
			if (realItems[i].Count == 0)
				continue;
			
            var varToCreate = realItems[i];

            var variable = Variable.Create(varToCreate.ToArray(), chunk ?? new SourceChunk());
            Val?.Add(variable);
        }
    }

    public Array(Array array)
    {
        Token = array.Token;
        Val = array.Val;
        Name = array.Name;
    }

    //public Array() : this(System.Array.Empty<Token>(), null) { }
	public Array() 
	{
		Val = new();
		Token = new() { Type = TokenType.Array };
	}

    public string GetString() => $"[ {string.Join(", ", Val.Select(var => var.GetString()))} ]";

    public override string ToString() => GetString();
}
