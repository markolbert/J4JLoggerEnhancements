namespace J4JSoftware.Logging;

[Flags]
public enum TemplateElements
{
    [TemplateElement("{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}")]
    Timestamp = 1 << 0,

    [TemplateElement("[{Level:u3}]")]
    Level = 1 << 1,

    [TemplateElement("{Message:lj}")]
    Message = 1 << 2,

    [TemplateElement("{Exception:lj}")]
    Exception = 1 << 3,

    [TemplateElement("j{MemberName} {FilePath} ({LineNumber})")]
    SourceCode = 1 << 4,

    [TemplateElement("{SendToSms}")]
    SendToSms = 1 << 5,

    None = 0,
    NetEvent = Level | Message,
    Sms = SendToSms | Level | Message,
    All = Timestamp | Level | Message | SourceCode | SendToSms
}