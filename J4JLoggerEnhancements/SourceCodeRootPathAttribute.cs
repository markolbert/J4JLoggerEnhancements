namespace J4JSoftware.Logging;

[AttributeUsage(AttributeTargets.Assembly)]
public class SourceCodeRootPathAttribute : Attribute
{
    public SourceCodeRootPathAttribute(
        string rootPath
    )
    {
        RootPath = rootPath;
    }

    public string RootPath { get; }
}