namespace J4JSoftware.Logging;

[AttributeUsage(AttributeTargets.Field)]
public class TemplateElementAttribute : Attribute
{
    public TemplateElementAttribute(
        string template
    )
    {
        Template = template;
    }

    public string Template { get; }
}