using System;
using System.Text;

namespace SlimScript;

internal class SourceChunk
{
    public Dictionary<Token, IVariable> Stack { get; set; }
    private List<List<Token>> Lines { get; set; }

    private bool _inBlock = false;
    private int _line = 0;

    public SourceChunk(string[] source)
    {
        Lines = Lexer.Lex(source);
    }

    private void CreateBlock(string[] source) { }

    public object Run()
    {
        Destructor();

        return 5;
    }

    private void Destructor()
    {
        Stack.Clear();
    }
}
