namespace MarkdownDocumentationFromSummaries.Classes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    sealed class SummaryAttribute : Attribute
    {
        public string Summary { get; }

        public SummaryAttribute(string summary)
        {
            Summary = summary;
        }
    }
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    sealed class SignatureAttribute : Attribute
    {
        public string Signature { get; }

        public SignatureAttribute(string signature)
        {
            Signature = signature;
        }
    }
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    sealed class TitleAttribute : Attribute
    {
        public string Title { get; }

        public TitleAttribute(string summary)
        {
            Title = summary;
        }
    }
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class ParamAttribute : Attribute
    {
        public string Param { get; }
        public string Description { get; }
        public Type Type { get; }
        public ParamAttribute(string param, string description, Type type)
        {
            Param = param;
            Description = description;
            Type = type;
        }
    }
}
