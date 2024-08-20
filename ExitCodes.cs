namespace SlimScript;

public enum ExitCode
{
    Normal = 0b0000_0000,
    NoInputFile = 0b0000_0001,
    UnidentifiedToken = 0b0000_1100,
    LexerError = 0b0000_0100,
    GrammarError = 0b0000_1000,
    NullReferenceError = 0b0000_1110,
    MultipleDeclarationError = 0b0001_1000,
    DisordantTokenError = 0b0000_1010,
    RuntimeError = 0b0000_0010,
    PreProcessorError = 0b0000_0011,
    CodeRuntimeError = 0b0000_0110,
    ParserError = 0b0001_0010
}
