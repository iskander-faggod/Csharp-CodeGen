using Parser.Models;

namespace Parser.Methods;

public static class Parser
{
    private static string _pathToController =
        "/home/iskander/Desktop/techSpringCrud/src/main/java/com/example/techspringcrud/controllers/TaskController.java";
    private static StreamReader _reader = new StreamReader(_pathToController);
    
    public static List<MethodDto> GetMethods()
    {
        var methods = new List<MethodDto>();
        var codeLine = "";
        
        // to create Method 
        var temlHttpMethodName = "";
        var tempUrl = "";
        var tempReturnType = "";
        var tempMethodName = "";

        while ((codeLine = _reader.ReadLine()) != null)
        {
            
            if (codeLine.Contains("Mapping") && codeLine.Contains("@") && !codeLine.Contains("Request"))
            {
                var httpType = codeLine.Substring(codeLine.IndexOf('@'));
                var httpTypeValue = httpType.Split("Mapping").First().Trim('@');
                temlHttpMethodName = httpTypeValue;
                var url = codeLine.Substring(codeLine.IndexOf('@') + httpTypeValue.Length + "Mapping".Length + 1);
                var urlValue = url.Trim('(', ')');
                tempUrl = urlValue;
            }
            
            if (codeLine.Contains("public") && !codeLine.Contains("class"))
            {
                var returnType = codeLine.Substring(codeLine.IndexOf("public") + "public ".Length);
                var returnTypeValue = returnType.Split(" ").First();
                tempReturnType = returnTypeValue;
                var methodName =
                    codeLine.Substring(codeLine.IndexOf("public") + "public ".Length + returnTypeValue.Length + 1);
                var methodNameValue = methodName.Split('(').First();
                tempMethodName = methodNameValue;
                
                // arguments

                var methodTemp = new MethodDto();
                    
                methodTemp.Url = tempUrl;
                methodTemp.HttpMethodName = temlHttpMethodName;
                methodTemp.MethodName = tempMethodName;
                methodTemp.ReturnType = tempReturnType;
                
                if (codeLine.Contains("@Request"))
                {
                    var arguments = codeLine.Substring(codeLine.IndexOf('(')).Trim('{');
                    
                    var argumentsValue = arguments
                        .Replace("@RequestBody ", "")
                        .Replace("@RequestParam ", "")
                        .Trim('(', ')');

                    var tokens = argumentsValue.Split(')')[0].Split(' ', StringSplitOptions.TrimEntries);
                    var decorator = tokens.FirstOrDefault(x => x.StartsWith('@'));
                    
                    var argument =  new ArgumentDto() {
                        Type = tokens.First(x => !x.Equals(decorator)),
                        Name = tokens.Last(),
                    };

                    methodTemp.ArgDeclarations.Add(argument);

                    if (arguments == "()")
                    {
                        methodTemp.ArgDeclarations.Add(new ArgumentDto());
                    }
                }
                methods.Add(methodTemp);
            }
        }

        return methods;
    } 
}