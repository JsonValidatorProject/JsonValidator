using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace JsonValidator;

public static class JsonElementExtensions
{
    public static void ValidateMatch(
        this JsonDocument jsonDocument,
        object expectedObject,
        ValidationConfiguration? configuration = null)
    {
        if (!TryValidateMatch(jsonDocument, expectedObject, out var errors, configuration))
        {
            throw new ValidationFailedException(errors);
        }
    }

    public static bool TryValidateMatch(
        this JsonDocument jsonDocument,
        object expectedObject,
        out List<string> errors,
        ValidationConfiguration? configuration = null)
    {
        errors = [];

        var expectedJsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(expectedObject));

        var jsonDocumentElements = new Dictionary<string, DocumentElement>();
        var expectedObjectElements = new Dictionary<string, DocumentElement>();
        var jsonDocumentArrayElements = new List<string>();
        var expectedObjectArrayElements = new List<string>();

        TraverseJson(jsonDocument.RootElement, jsonDocumentElements, jsonDocumentArrayElements);
        TraverseJson(expectedJsonDocument.RootElement, expectedObjectElements, expectedObjectArrayElements);

        var problemElements = new List<DocumentElement>();

        foreach (var (key, expectedElement) in expectedObjectElements)
        {
            var isFound = jsonDocumentElements.TryGetValue(key, out var actualElement);
            if (!isFound || actualElement is null)
            {
                errors.Add($"'{key}' not found");
                continue;
            }

            if (expectedElement.Type != actualElement.Type)
            {
                errors.Add($"Type for '{key}' was {actualElement.Type} but should have been {expectedElement.Type}");
                problemElements.Add(actualElement);
                continue;
            }

            if (expectedElement.Value != actualElement.Value)
            {
                errors.Add($"Value for '{key}' was '{actualElement.Value}' but should have been '{expectedElement.Value}'");
                problemElements.Add(actualElement);
            }
        }

        var excessArrayElements = jsonDocumentArrayElements.Except(expectedObjectArrayElements).ToArray();

        errors.AddRange(excessArrayElements.Select(e => $"Excess array elements in the JSON document: '{e}'"));

        if (configuration is { ExactMatch: true })
        {
            var excessElementPaths = jsonDocumentElements
                .Select(kvp => kvp.Value)
                .Except(expectedObjectElements.Values)
                .Except(problemElements)
                .Select(e => e.Path)
                .Except(excessArrayElements)
                .ToArray();

            errors.AddRange(excessElementPaths.Select(p => $"Excess found in the JSON document: '{p}'"));
        }

        return errors.Count == 0;
    }

    private static void TraverseJson(
        JsonElement element,
        IDictionary<string, DocumentElement> elements,
        ICollection<string> arrays,
        string parentPath = "$")
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var property in element.EnumerateObject())
                {
                    TraverseJson(property.Value, elements, arrays, $"{parentPath}.{property.Name}");
                }
                break;

            case JsonValueKind.Array:
                var array = element.EnumerateArray().ToArray();
                for (var i = 0; i < array.Length; i++)
                {
                    var elementPath = $"{parentPath}[{i}]";
                    TraverseJson(array[i], elements, arrays, elementPath);
                    arrays.Add(elementPath);
                }
                break;

            case JsonValueKind.Number:
                elements.Add(parentPath, new DocumentElement(parentPath, element.GetRawText(), JsonType.Number));
                break;

            case JsonValueKind.String:
                elements.Add(parentPath, new DocumentElement(parentPath, element.GetString()!, JsonType.String));
                break;

            case JsonValueKind.True:
            case JsonValueKind.False:
                elements.Add(parentPath, new DocumentElement(parentPath, element.GetBoolean().ToString().ToLower(), JsonType.Boolean));
                break;

            case JsonValueKind.Undefined:
            case JsonValueKind.Null:
                elements.Add(parentPath, new DocumentElement(parentPath, "null", JsonType.Null));
                break;

            default:
                throw new Exception($"Unknown Json Value Kind: '{element.ValueKind}'");
        }
    }
}
