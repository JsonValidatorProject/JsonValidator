using System.Text.Json;

namespace JsonValidator.FluentAssertions.Json;

public static class JsonDocumentExtensions
{
    public static JsonDocumentAssertions Should(this JsonDocument instance) => new(instance);
}
