using System.Text;

namespace SlimScript;

internal class Append : Standart
{
    public override IVariable Run(List<Token> line, SourceChunk chunk)
    {
        var varT = line.ToArray()[1..line.LastIndexOf(new("to"))];
        var variable = Variable.Create(varT, chunk);
		
		if (variable.Type == TokenType.Array)
			chunk.Error("Arrays cannot be appended to anything.", ExitCode.GrammarError);
		
		// append <something> to <array_or_word>
        if(line[3].Text != "index")
		{
			var appendant = Variable.Create(line.ToArray()[(line.LastIndexOf(new("to")) + 1)..], chunk);
			
			if (appendant.GetType() == typeof(SlimScript.Array))
			{
				var array = appendant as SlimScript.Array;

				array.Val.Add(variable);

				chunk.SetVar(array.Name, array);
				
				return chunk.GetVar(array.Name) ?? new Null();
			}
			else if (appendant.GetType() == typeof(Word))
			{
				if (variable.Type != TokenType.Word)
					chunk.Error(
						$"Cannot append '{variable.Type}' to a word.",
						ExitCode.DisordantTokenError
					);
				
				var str = (Word)appendant;
				str.Val += (string)variable.Value;
				chunk.SetVar(str.Name, str);
				
				return chunk.GetVar(str.Name) ?? new Null();
			}
			else
				chunk.Error(
					$"Cannot find indexer on type '{appendant.Token}'",
					ExitCode.DisordantTokenError
				);
		}
		// append <something> to index <idx> of <array_or_string>
		else
		{
			var index = Convert.ToInt32(Variable.VarToClr(Variable.Create(line.ToArray()[(line.LastIndexOf(new("index"))+1)..(line.LastIndexOf(new("of")))], chunk)));
			var appendant = Variable.Create(line.ToArray()[(line.LastIndexOf(new("of"))+1)..], chunk);
			
			//Console.WriteLine(string.Concat(index));
			//Console.WriteLine(string.Concat(appendant));
			
			if (appendant.GetType() == typeof(SlimScript.Array))
			{
				var array = appendant as SlimScript.Array;
				array.Val.Insert(index, variable);
				chunk.SetVar(array.Name, array);
				
				return chunk.GetVar(array.Name) ?? new Null();
			}
			else if (appendant.GetType() == typeof(Word))
			{
				if (variable.Type != TokenType.Word)
					chunk.Error(
						$"Cannot append '{variable.Type}' to a word.",
						ExitCode.DisordantTokenError
					);
				
				var str = (Word)appendant;
				
				var sb = new StringBuilder(str.Val);
				sb.Insert(index, (string)variable.Value);
				
				str.Val = sb.ToString();
				chunk.SetVar(str.Name, str);
				
				return chunk.GetVar(str.Name) ?? new Null();
			}
			else
				chunk.Error(
					$"Cannot find indexer on type '{appendant.Token}'",
					ExitCode.DisordantTokenError
				);
		}
		
		return new Null();
    }
}
