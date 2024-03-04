namespace JsonValidator;

internal record DocumentElement
{
    public DocumentElement(string path, string? value, JsonType type)
    {
        Path = path;
        Value = value;
        Type = type;
    }

    public string Path { get; }
    public string? Value { get; }
    public JsonType Type { get; }
}
