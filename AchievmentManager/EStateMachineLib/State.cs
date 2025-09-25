namespace EStateMachineLib
{
    /// <summary>
    /// Содепжит все состояния, в которых может находиться машина.
    /// </summary>
    internal enum State
    {
        Awakening,
        WaitingForMainKey,
        GettingMainKey,
        WaitingForMainPoints,
        Starting,

        Passing,
        WaitingForInterSectionComma,

        WaitingForKey,
        GettingKey,
        WaitingForPoints,
        WaitingForValue,
        GettingValue,
        WaitingForComma,

        Stopping
    }
}
