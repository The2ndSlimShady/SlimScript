namespace SlimScript;

internal abstract class Operator
{
    public abstract IVariable Apply(Token[] parameters, SourceChunk chunk);

    public static List<IVariable> ReadyParams(
        Token[] parameters,
        SourceChunk chunk,
        int capacity = 2
    )
    {
        List<IVariable> realParams;

        bool infinite = capacity == 0;

        if (capacity != 0)
            realParams = new(capacity);
        else
            realParams = new();

        for (int i = 1; i < parameters.Length; i++)
        {
            if (!infinite && realParams.Count == realParams.Capacity)
                break;

            Token param = parameters[i];

            if (param.Type == TokenType.Number || param.Type == TokenType.String)
                realParams.Add(Variable.Create(new[] { param }, chunk));
            else if (param.Type == TokenType.Identifier)
                realParams.Add(Identifier.Identify(parameters[i..], chunk));
            else if (param.Type == TokenType.Standart)
            {
                realParams.Add(
                    (
                        Variable.CreateType(Parser.IdentifyAndGet(parameters[i..].ToList(), chunk))
                        as Standart
                    ).Run(parameters[i..].ToList(), chunk)
                );

                i = parameters.Length;
            }
            else if (param.Type == TokenType.Operator)
            {
                realParams.Add(
                    (
                        Variable.CreateType(Parser.IdentifyAndGet(parameters[i..].ToList(), chunk))
                        as Operator
                    ).Apply(parameters[i..], chunk)
                );

                i = parameters.Length;
            }
            else
            {
                chunk.Error(
                    $"Cannot use token '{param.Text}' as operator parameter.",
                    ExitCode.DisordantTokenError
                );
            }
        }

        return realParams;
    }
}
