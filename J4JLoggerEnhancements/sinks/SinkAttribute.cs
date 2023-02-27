namespace J4JSoftware.Logging
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal class SinkTemplateParameterAttribute : Attribute
    {
        public SinkTemplateParameterAttribute(
            string templateParam
        )
        {
            if (string.IsNullOrEmpty(templateParam))
                throw new NullReferenceException($"Empty or undefined template parameter");

            TemplateParameter = templateParam;
        }

        public string TemplateParameter { get; }
    }
}
