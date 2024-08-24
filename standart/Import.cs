using System.Text;
using System.IO;

namespace SlimScript;

internal class Import : Standart
{
	public override IVariable Run(List<Token> line, SourceChunk chunk)
	{
		var args = Operator.ReadyParams(line.ToArray(), chunk, 0);

		string file = string.Concat(args.Select(arg => arg.GetString()));

		if (!File.Exists(file))
		{
			file = Path.Combine(GlobalSettings.SystemFilesPath, $"lib/{file}");
			
			if (!File.Exists(file))
				chunk.Error("Given file does not exists.", ExitCode.NoInputFile);
		}
		

		string[] source;
		if (Path.GetExtension(file) == ".csso")
		{
			var tmpStr = Encoding.UTF8.GetString(Program.Decompress(File.ReadAllBytes(file)));
			source = tmpStr.Split(Environment.NewLine, StringSplitOptions.TrimEntries);
		}
		else
			source = File.ReadAllLines(
				new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), file)).FullName
			);

		var chnk = new SourceChunk(source) {Parent = chunk};
		chnk.Run();
		chunk.Stack.AddRange(chnk.Stack);
		

		return Variable.ClrToVar(file);
	}
}
