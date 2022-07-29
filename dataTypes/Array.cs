using System;
using System.Collections;

namespace SlimScript;

public class Array : IVariable, IEnumerable<IVariable>
{
    private string _name = "";

    public Token Token { get; set; }

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

    public Array(Token[] items, SourceChunk? chunk)
    {
        Val = new();
        Token = new() { Type = TokenType.Array };

        if (items.Length == 0)
            return;

        items = items[1..(items.Length - 1)];

        List<List<Token>> realItems = new() { new() };

        for (int i = 0, j = 0; i < items.Length; i++)
        {
            Token t = items[i];

            if (t.Text == "," || t.Text.Contains(','))
            {
                if (t.Text.Contains(','))
                {
                    string[] values = t.Text.Split(',');

                    foreach (string value in values)
                    {
                        if (!string.IsNullOrEmpty(value) || !string.IsNullOrWhiteSpace(value))
                        {
                            realItems[j].Add(new(value));
                            j++;
                            realItems.Add(new());
                            continue;
                        }
                    }
                }

                j++;
                realItems.Add(new());
                continue;
            }

            realItems[j].Add(t);
        }

        realItems = realItems.Where(i => i.Count != 0).ToList();

        foreach (List<Token> item in realItems)
        {
            var variable = Variable.Create(item.ToArray(), chunk);
            Val?.Add(variable);
        }
    }

    public Array(Array array)
    {
        Token = array.Token;
        Val = array.Val;
        Name = array.Name;
    }

    public Array() : this(System.Array.Empty<Token>(), null) { }

    public string GetString() => $"[ {string.Join(", ", Val.Select(var => var.GetString()))} ]";

    public IEnumerator<IVariable> GetEnumerator() => Val.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Val.GetEnumerator();
}
