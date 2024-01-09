using System.Text.Json;
using FluentAssertions;
using FluentAssertions.Execution;

namespace JsonValidator.FluentAssertions.Json;

public class JsonDocumentAssertions
{
    private readonly JsonDocument _instance;

    public JsonDocumentAssertions(JsonDocument instance) => _instance = instance;

    /// <summary>
    /// Checks whether the json document matches the input object in both structure and values.
    /// </summary>
    /// <param name="expected">The expected value as an anonymous object</param>
    /// <param name="because">The reason the two values should match</param>
    /// <param name="becauseArgs">The parameters for the reason argument</param>
    public AndConstraint<JsonDocumentAssertions> Match(
        object expected,
        string because = "",
        params object[] becauseArgs)
    {
        var isMatch = _instance.TryValidateMatch(expected, out var errors);

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(isMatch)
            .FailWith("Expected object to match the JSON input, but found differences {0}.", errors);

        return new AndConstraint<JsonDocumentAssertions>(this);
    }
}
