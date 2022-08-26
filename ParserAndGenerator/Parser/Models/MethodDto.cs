namespace Parser.Models;

public class MethodDto
{
    public MethodDto()
    {
    }

    public MethodDto(string returnType, string url, string methodName, string httpMethodName)
    {
        ReturnType = returnType;
        Url = url;
        MethodName = methodName;
        HttpMethodName = httpMethodName;
    }

    public string ReturnType { get; set; }
    public string Url { get; set; }
    public string MethodName { get; set; }
    public string HttpMethodName { get; set; }
    public List<ArgumentDto> ArgDeclarations = new List<ArgumentDto>();
}