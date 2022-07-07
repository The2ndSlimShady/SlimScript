namespace SlimScript;

internal enum ExitCode
{
	NoInputFile = 0b0001,
	Normal = 0b0000,
	UnidentifiedToken = GrammarError | LexerError,
	LexerError = 0b0100,
	GrammarError = 0b1000
}