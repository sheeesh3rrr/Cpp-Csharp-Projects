namespace EParserLib
{
    /// <summary>
    /// Содержит все состояния, в которых может находиться машина.
    /// </summary>
    internal enum State
    {
        Awakening = 1,
        WaitingForMainKey,
        GettingMainKey,
        WaitingForMainPoints,
        Starting,
        WaitingForFirstObject,
        Passing,
        WaitingForInterSectionComma,
        Reading,
        Stopping
    }
}
