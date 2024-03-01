using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace JsonValidator;

public record ValidationConfiguration
{
    public ValidationConfiguration(bool exactMatch = false) => ExactMatch = exactMatch;

    public bool ExactMatch { get; }
}

internal enum JsonType
{
    String,
    Number,
    Boolean,
    Null
}

internal static class Helpers
{
    public static int IndexOfLast(this string input, string subString)
        => input.LastIndexOf(subString, StringComparison.OrdinalIgnoreCase);
}

internal record Item
{
    public Item(string path, string? value, JsonType type)
    {
        Path = path;
        Value = value;
        Type = type;
    }

    public string Path { get; }
    public string? Value { get; }
    public JsonType Type { get; }
}

public static class NewJsonElementExtensions
{
    public static bool TryValidateMatch(
        this JsonDocument jsonDocument,
        object expectedObject,
        out List<string> errors,
        ValidationConfiguration? configuration = null)
    {
        errors = [];

        var expectedJsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(expectedObject));

        var jsonDocumentItems = new Dictionary<string, Item>();
        var expectedObjectItems = new Dictionary<string, Item>();
        var jsonDocumentArrayItems = new List<string>();
        var expectedObjectArrayItems = new List<string>();

        TraverseJson(jsonDocument.RootElement, jsonDocumentItems, jsonDocumentArrayItems);
        TraverseJson(expectedJsonDocument.RootElement, expectedObjectItems, expectedObjectArrayItems);

        foreach (var (key, expectedItem) in expectedObjectItems)
        {
            var isFound = jsonDocumentItems.TryGetValue(key, out var actualItem);
            if (!isFound || actualItem is null)
            {
                errors.Add($"'{key}' not found");
                continue;
            }

            if (expectedItem.Type != actualItem.Type)
            {
                errors.Add($"Type for '{key}' was {actualItem.Type} but should have been {expectedItem.Type}");
                continue;
            }

            if (expectedItem.Value != actualItem.Value)
            {
                errors.Add($"Value for '{key}' was '{actualItem.Value}' but should have been '{expectedItem.Value}'");
            }
        }

        var excessArrayElements = jsonDocumentArrayItems.Except(expectedObjectArrayItems).ToArray();
        errors.AddRange(excessArrayElements.Select(e => $"Excess array elements in the JSON document: '{e}'"));

        if (configuration is { ExactMatch: true })
        {
            var diff = jsonDocumentItems.Except(expectedObjectItems).Select(kvp => kvp.Value).ToArray();
            errors.AddRange(diff.Select(d => $"Excess found in the JSON document: '{d.Path}'"));
        }

        return errors.Count == 0;
    }

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

    private static void TraverseJson(
        JsonElement element,
        Dictionary<string, Item> items,
        List<string> arrays,
        string parentPath = "$")
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var property in element.EnumerateObject())
                {
                    TraverseJson(property.Value, items, arrays, $"{parentPath}.{property.Name}");
                }
                break;

            case JsonValueKind.Array:
                var array = element.EnumerateArray().ToArray();
                for (var i = 0; i < array.Length; i++)
                {
                    var itemPath = $"{parentPath}[{i}]";
                    TraverseJson(array[i], items, arrays, itemPath);
                    arrays.Add(itemPath);
                }
                break;

            case JsonValueKind.Number:
                items.Add(parentPath, new Item(parentPath, element.GetRawText(), JsonType.Number));
                break;

            case JsonValueKind.String:
                items.Add(parentPath, new Item(parentPath, element.GetString()!, JsonType.String));
                break;

            case JsonValueKind.True:
            case JsonValueKind.False:
                items.Add(parentPath, new Item(parentPath, element.GetBoolean().ToString().ToLower(), JsonType.Boolean));
                break;

            case JsonValueKind.Undefined:
            case JsonValueKind.Null:
                items.Add(parentPath, new Item(parentPath, "null", JsonType.Null));
                break;

            default:
                throw new Exception($"Unknown Json Value Kind: '{element.ValueKind}'");
        }
    }
}
