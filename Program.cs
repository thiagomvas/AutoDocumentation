using AutoDocumentation;
using System.Reflection;

namespace MarkdownDocumentationFromSummaries
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AutoDocumentation.AutoDocumentation.GenerateAutoDocumentation();
        }
    }
}