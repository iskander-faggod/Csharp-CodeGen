namespace Parser.Models;

public class ArgumentDto
{
    public ArgumentDto()
    {
    }

    public ArgumentDto(string type, string name)
    {
        Type = type;
        Name = name;
    }

    public string Type { get; set; }
    public string Name { get; set; }
}