namespace SlimScript;

internal class Input : Standart
{
    public static TextReader StandartInput { get; set; } = Console.In;

    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        if (line.Count > 1)
        {
            var inputStr = (Word)Variable.Create(line.ToArray()[1..], chunk);

            Write.StandartOutput.Write(inputStr.Val);
        }

        var input = new Word(new($"\"{StandartInput.ReadLine()}\""));

        return input;
    }
}
