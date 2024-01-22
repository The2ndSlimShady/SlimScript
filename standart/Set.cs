namespace SlimScript;

internal class Set : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        if (line[1].Text != "index")
        {
            var name = line[1];
            var keyword = line[2];

            if (keyword.Text != "to")
                chunk.Error(
                    $"Unexpected keyword. Expected 'to' got '{keyword.Text}'",
                    ExitCode.DisordantTokenError
                );
			
            IVariable variable = Variable.Create(line.ToArray()[3..], chunk);

            chunk.SetVar(name.Text, variable);

            return chunk.GetVar(name.Text) ?? new Null();
        }
		// set index <idx> of <array_or_word> to <value>
        else
        {	
            var indexTstr = line.ToArray()[2..line.IndexOf(new("of"))];
			//var indexTstr = line.Skip(2).Take(1).ToArray();
            var indexT = Variable.Create(indexTstr, chunk);

            if (indexT.Token.Type != TokenType.Number)
                chunk.Error(
                    $"Cannot convert from type '{indexT}' to type '<Number>'",
                    ExitCode.DisordantTokenError
                );

            Number index = (Number)indexT;

            var arrayT = Identifier.Identify(
                //line.Skip(4).Take(1).ToArray(),
				line.ToArray()[(line.IndexOf(new("of"))+1)..(line.LastIndexOf(new("to")))],
                chunk
            );
			
			if (arrayT.GetType() == typeof(SlimScript.Array))
			{
				var array = (SlimScript.Array)arrayT;

				array.Val[(int)index.Val] = Variable.Create(
					line.ToArray()[(line.IndexOf(new("to")) + 1)..],
					//line.Skip(6).Take(line.Count - 5).ToArray(),
					chunk
				);

				chunk.SetVar(array.Name, array);
				return chunk.GetVar(array.Name) ?? new Null();
			}
			else if (arrayT.GetType() == typeof(SlimScript.Word))
			{
				IVariable newVal = Variable.Create(
					line.Skip(6).Take(line.Count - 5).ToArray(),
					chunk
				);
				
				if (newVal.Type != TokenType.Word)
					chunk.Error(
						$"Cannot use type '{newVal.Type}' in word indexer",
						ExitCode.DisordantTokenError
					);
				
				Word str = (SlimScript.Word)arrayT;
				
				char[] chArr = str.Val.ToCharArray();
				chArr[(int)index.Val] = ((SlimScript.Word)newVal).Val[0];
				str.Val = new(chArr);
				//str.Val = str.Val.Remove((int)index.Val, 1).Insert((int)index.Val, ((SlimScript.Word)newVal).Val);
				
				chunk.SetVar(str.Name, str);
				return chunk.GetVar(str.Name) ?? new Null();
			}

            if (arrayT.GetType() != typeof(SlimScript.Array))
                chunk.Error(
                    $"Cannot find indexer on type '{arrayT.Type}'",
                    ExitCode.DisordantTokenError
                );
			
			return new Null();
        }
    }
}
