namespace JsonValidator;

public record ValidationConfiguration
{
    public ValidationConfiguration(bool exactMatch = false) => ExactMatch = exactMatch;

    public bool ExactMatch { get; }
}
