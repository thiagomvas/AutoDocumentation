using System.Reflection;

namespace MarkdownDocumentationFromSummaries
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Type.GetType("BankAccount"));
            IterateFilesAndFolders("C:\\Users\\Thiago\\source\\repos\\MarkdownDocumentationFromSummaries");
        }


        public static void IterateFilesAndFolders(string directoryPath, string currentPath = "")
        {
            List<DocTypeInfo> typesFound = new();
            try 
            {
                // Process files in the current directory
                string[] files = Directory.GetFiles(directoryPath);
                foreach (string file in files)
                {
                    if (file.Substring(file.IndexOf('.') + 1) != "cs") continue;
                    string fileName = Path.GetFileName(file.Substring(0, file.IndexOf('.')));
                    string path = Path.Combine(new string[] { "docs", currentPath, fileName });

                    Type type = Type.GetType(fileName);
                    if(type != null)
                    {
                        typesFound.Add(new(type, path));
                    }
                    
                }

                // Recursively process subdirectories
                string[] subdirectories = Directory.GetDirectories(directoryPath);
                foreach (string subdirectory in subdirectories)
                {
                    string subdirectoryName = Path.GetFileName(subdirectory);

                    // Skip specific folders (e.g., "obj", "bin", ".vs")
                    if (!ShouldSkipFolder(subdirectoryName))
                    {
                        var path = Path.Combine("C:\\Users\\Thiago\\source\\repos\\MarkdownDocumentationFromSummaries\\docs", subdirectoryName);
                        Console.WriteLine(path);
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        IterateFilesAndFolders(subdirectory, Path.Combine(currentPath, subdirectoryName));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            foreach(DocTypeInfo type in typesFound)
            {
                AnalyzeType(type.type, type.relativePathToDocs, typesFound);
            }
        }

        // Method to determine whether to skip a folder
        private static bool ShouldSkipFolder(string folderName)
        {
            string[] foldersToSkip = { "obj", "bin", ".vs" }; // Add more folder names as needed

            foreach (string folderToSkip in foldersToSkip)
            {
                if (string.Equals(folderName, folderToSkip, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static void AnalyzeType(Type type, string fileName, List<DocTypeInfo> typesToLink)
        {
            Console.WriteLine($"Analyzing type: {type.FullName}");

            string documentation = "# " + type.Name + "\n";

            int i = 0;

            // Get all fields
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                DocsMember doc = new();
                doc.Title = $"{field.Name}";
                doc.Signature = $"{(field.IsPublic ? "public" : "private")}{(field.IsStatic ? " static" : "")} {field.FieldType.Name} {field.Name}";

                documentation += doc;
                i++;
            }

            // Get all properties
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (PropertyInfo property in properties)
            {
                Console.WriteLine($"Property: {property.Name}, Type: {property.PropertyType}");
            }

            // Get all methods
            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo method in methods)
            {
                if (method.Name.StartsWith("get_") || method.Name.StartsWith("set_")) continue;
                DocsMember doc = new();
                doc.type = DocsMember.DocType.Method;
                doc.Title = method.Name;
                string text = $"{(method.IsPublic ? "public" : "private")} {method.ReturnType.Name} {method.Name} (";
                var parameters = method.GetParameters();
                foreach (var param in parameters)
                    text += $"{param.ParameterType.Name} {param.Name}, ";
                if(parameters.Length > 0)text = text.Substring(0, text.Length - 2);
                text += ")";

                doc.Signature = text;

                foreach(var param in method.GetParameters())
                {
                    string link = string.Empty;
                    foreach(var t in typesToLink)
                    {
                        if(t.type == param.ParameterType)
                        {
                            link = t.relativePathToDocs;
                            break;
                        }

                    }
                    doc.Parameters.Add(new(param.Name, param.ParameterType, link));
                }

                documentation += doc;

            }
                WriteTextToFile(documentation, fileName);
        }

        static void WriteTextToFile(string text, string fileName)
        {
            string filePath = $"C:\\Users\\Thiago\\source\\repos\\MarkdownDocumentationFromSummaries\\{fileName}.md";
            try
            {
                // Write the text to the specified file.
                File.WriteAllText(filePath, text);

                Console.WriteLine("Text successfully written to the file.");
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }
    }

    public class DocsMember
    {
        public string Title { get; set; }
        public string Summary { get; set; } = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse aliquet euismod risus, vitae iaculis libero placerat vitae. Vestibulum ac feugiat risus, ac egestas justo. ";
        public string Signature { get; set; }
        public List<ParameterInfo> Parameters { get; set; } = new();
        public enum DocType { Field, Property, Method }
        public DocType type = DocType.Field;


        public override string ToString()
        {
            var text =
$@"
## `{type}` {Title}
{Summary}
```csharp
{Signature}
```";
            if (Parameters.Count > 0)
                text += @"
### Parameters
| Parameter Name  | Type      | Description                            |
|------------------|-----------|----------------------------------------|
";
            foreach (ParameterInfo param in Parameters)
            {
                text += $"|{param.Name} | {(!string.IsNullOrWhiteSpace(param.LinkPath) ? $"[{param.Type}]({param.LinkPath})" : param.Type)} | {param.Description} |\n";
            }
            return text;
        }

    }

    public class ParameterInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Type Type { get; set; }
        public string LinkPath { get; set; }

        public ParameterInfo(string name, Type type, string linkPath = "", string description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.")
        {
            Name = name;
            Description = description;
            Type = type;
            LinkPath = linkPath;
        }
    }

    public class DocTypeInfo
    {
        public Type type;
        public string relativePathToDocs;
        public DocTypeInfo(Type type, string relativePathToDocs)
        {
            this.type = type;
            this.relativePathToDocs = relativePathToDocs;
        }
    }
}