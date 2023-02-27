using System.Reflection;
using System.Text;

namespace J4JSoftware.Logging;

public static class TemplateExtensions
{
    private record TemplateComponent(string Component, string Template);

    private static readonly Dictionary<string, TemplateComponent> Components;
    private static readonly string? SourceCodeRootPath;

    static TemplateExtensions()
    {
        SourceCodeRootPath = Assembly.GetEntryAssembly()?.GetCustomAttribute<SourceCodeRootPathAttribute>()?.RootPath;

        Components = typeof(TemplateElements).GetMembers()
            .Select(x => new TemplateComponent(x.Name,
                x.GetCustomAttribute<TemplateElementAttribute>(false)?.Template ?? string.Empty))
            .ToDictionary(x => x.Component, x => x, StringComparer.OrdinalIgnoreCase);
    }

    public static string GetTemplate(this TemplateElements enrichers)
    {
        var sb = new StringBuilder();

        enrichers.AppendTemplate(TemplateElements.Timestamp, sb);
        enrichers.AppendTemplate(TemplateElements.Level, sb);
        enrichers.AppendTemplate(TemplateElements.Message, sb);
        enrichers.AppendTemplate(TemplateElements.Exception, sb);
        enrichers.AppendTemplate(TemplateElements.SourceCode, sb);
        enrichers.AppendTemplate(TemplateElements.SendToSms, sb);

        return sb.ToString();
    }

    private static void AppendTemplate(this TemplateElements value, TemplateElements toTest, StringBuilder sb)
    {
        if ((value & toTest) != toTest)
            return;

        var toAppend = GetTemplateElement(toTest);

        if (!string.IsNullOrEmpty(toAppend))
            sb.Append(toAppend);
    }

    private static bool HasSingleFlag(TemplateElements element) => (element & (element - 1)) == 0;

    private static string? GetTemplateElement(TemplateElements enumValue)
    {
        if (!HasSingleFlag(enumValue))
            return null;

        var textValue = enumValue.ToString();

        return Components.ContainsKey(textValue) ? Components[textValue].Template : null;
    }

    internal static string TrimPath(string srcFilePath) =>
        SourceCodeRootPath == null
            ? srcFilePath
            : srcFilePath.Replace(SourceCodeRootPath, string.Empty);
}