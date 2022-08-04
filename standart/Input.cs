namespace SlimScript;

internal class Input : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        if (line.Count > 1)
        {
            var inputStr = (Word)Variable.Create(line.ToArray()[1..], chunk);

            Console.Write(inputStr.Val);
        }

        var input = new Word(new($"\"{Console.ReadLine()}\""));

        return input;
    }
}
