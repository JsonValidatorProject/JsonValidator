using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace JsonValidator.FluentAssertions.HttpResponse;

public static class HttpResponseMessageAssertionsExtensions
{
    /// <summary>
    /// Checks whether the HTTP response's body as a JSON string matches the input object in both structure and values.
    /// </summary>
    /// <param name="instance">The <see cref="HttpResponseMessageAssertions"/></param>
    /// <param name="expected">The expected value as an anonymous object</param>
    /// <param name="because">The reason the two values should match</param>
    /// <param name="becauseArgs">The parameters for the reason argument</param>
    public static AndConstraint<HttpResponseMessageAssertions> HaveJsonBody(
        this HttpResponseMessageAssertions instance,
        object expected,
        string because = "",
        params object[] becauseArgs)
    {
        var isMatch = System.Text.Json.JsonDocument
            .Parse(instance.Subject.Content.ReadAsStringAsync().GetAwaiter().GetResult())
            .TryValidateMatch(expected, out var errors);

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(isMatch)
            .FailWith("Expected response body to match the JSON input, but found differences {0}.", errors);

        return new AndConstraint<HttpResponseMessageAssertions>(instance);
    }
}
