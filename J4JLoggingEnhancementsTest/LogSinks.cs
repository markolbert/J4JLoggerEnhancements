namespace J4JLoggingEnhancementTests;

[Flags]
public enum LogSinks
{
    Debug = 1 << 0,
    NetEvent = 1 << 1,
    LastEvent = 1 << 2,
    Twilio = 1 << 3,
    InMemory = 1 << 4,

    AllButTwilio = Debug | NetEvent | LastEvent,
    All = Debug | NetEvent | LastEvent | Twilio,
    None = 0
}
