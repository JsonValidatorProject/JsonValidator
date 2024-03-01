using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace JsonValidator;

public static class JsonElementExtensions
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> TypePropertiesCache = new();

    private static readonly HashSet<Type> PrimitiveTypes =
    [
        // Supported type
        typeof(string),
        typeof(bool),
        typeof(short),
        typeof(int),
        typeof(long),
        typeof(float),
        typeof(double),
        typeof(decimal),

        // Not supported types
        typeof(char),
        typeof(ushort),
        typeof(uint),
        typeof(ulong),
        typeof(byte),
        typeof(sbyte)
    ];

    private static readonly Dictionary<TypeCode, JsonValueKind[]> TypeCodeToJsonValueKind = new()
    {
        [TypeCode.String] = [JsonValueKind.String],
        [TypeCode.Boolean] = [JsonValueKind.True, JsonValueKind.False],
        [TypeCode.Int16] = [JsonValueKind.Number],
        [TypeCode.Int32] = [JsonValueKind.Number],
        [TypeCode.Int64] = [JsonValueKind.Number],
        [TypeCode.Single] = [JsonValueKind.Number],
        [TypeCode.Double] = [JsonValueKind.Number],
        [TypeCode.Decimal] = [JsonValueKind.Number]
    };

    public static bool TryValidateMatchOld(this JsonDocument jsonDocument, object expectedObject, out List<string> errors)
    {
        errors = [];

        jsonDocument.RootElement.ValidatePropertyElement(expectedObject, errors);

        return errors.Count == 0;
    }

    public static void ValidateMatchOld(this JsonDocument jsonDocument, object expectedObject)
    {
        if (!TryValidateMatchOld(jsonDocument, expectedObject, out var errors))
        {
            throw new ValidationFailedException(errors);
        }
    }

    private static void ValidateValueElement(this JsonElement jsonElement, object? expectedObject, List<string> errors)
    {
        // When the value is null, we can handle it as a string.
        var valueType = expectedObject.GetTypeOrString();
        var typeCode = Type.GetTypeCode(valueType);
        var typeInJson = jsonElement.ValueKind;

        if (!TypeCodeToJsonValueKind.ContainsKey(typeCode))
        {
            throw new NotSupportedException($"Type '{valueType.Name}' is not supported");
        }

        if (!TypeCodeToJsonValueKind[typeCode].Contains(typeInJson) && typeInJson != JsonValueKind.Null)
        {
            errors.Add($"type mismatch for value element: expected '{valueType.Name}' but was '{jsonElement.ValueKind}'");
            return;
        }

        switch (typeCode)
        {
            case TypeCode.String:
                ValidateValue((string?)expectedObject, jsonElement.GetString(), errors);
                break;

            case TypeCode.Boolean:
                ValidateValue((bool)expectedObject!, jsonElement.GetBoolean(), errors);
                break;

            case TypeCode.Int16:
                ValidateValue((short)expectedObject!, jsonElement.GetInt16(), errors);
                break;

            case TypeCode.Int32:
                ValidateValue((int)expectedObject!, jsonElement.GetInt32(), errors);
                break;

            case TypeCode.Int64:
                ValidateValue((long)expectedObject!, jsonElement.GetInt64(), errors);
                break;

            case TypeCode.Single:
                ValidateValue((float)expectedObject!, jsonElement.GetSingle(), errors);
                break;

            case TypeCode.Double:
                ValidateValue((double)expectedObject!, jsonElement.GetDouble(), errors);
                break;

            case TypeCode.Decimal:
                ValidateValue((decimal)expectedObject!, jsonElement.GetDecimal(), errors);
                break;
        }
    }

    private static void ValidatePropertyElement(
        this JsonElement jsonElement,
        object? expectedObject,
        List<string> errors)
    {
        // When the value is null, we can handle it as a string.
        var expectedType = expectedObject.GetTypeOrString();

        if (PrimitiveTypes.Contains(expectedType))
        {
            jsonElement.ValidateValueElement(expectedObject, errors);
            return;
        }

        // if (expectedObject is IDictionary dic)
        // {
        //     foreach (DictionaryEntry entry in dic)
        //     {
        //         var (key, value) = entry;
        //
        //         if (key is not string keyString)
        //         {
        //             throw new InvalidOperationException(
        //                 "When providing a dictionary as an input, the key must be of type 'string'");
        //         }
        //
        //         if (!jsonElement.TryGetProperty(keyString, out var childElement))
        //         {
        //             errors.Add($"property '{keyString}' not found");
        //             continue;
        //         }
        //
        //         ValidatePropertyElement(childElement, value, errors);
        //     }
        //
        //     return;
        // }

        var properties = expectedType.GetTypeProperties();
        foreach (var property in properties)
        {
            switch (Type.GetTypeCode(property.PropertyType))
            {
                case TypeCode.Object when typeof(IDictionary).IsAssignableFrom(property.PropertyType):
                    if (!jsonElement.TryGetProperty(property.Name, out var dictionaryElement))
                    {
                        errors.Add($"property '{property.Name}' not found");
                        break;
                    }

                    foreach (DictionaryEntry entry in (property.GetValue(expectedObject) as IDictionary)!)
                    {
                        var (key, value) = entry;

                        if (key is not string keyString)
                        {
                            throw new InvalidOperationException(
                                "When providing a dictionary as an input, the key must be of type 'string'");
                        }

                        if (!dictionaryElement.TryGetProperty(keyString, out var dictionaryChildElement))
                        {
                            errors.Add($"property '{keyString}' not found");
                            continue;
                        }

                        ValidatePropertyElement(dictionaryChildElement, value, errors);
                    }
                    break;

                case TypeCode.Object when property.PropertyType.IsArray:
                    if (!jsonElement.TryGetProperty(property.Name, out var arrayElement))
                    {
                        errors.Add($"property '{property.Name}' not found");
                        break;
                    }

                    var expectedArray = (Array?)property.GetValue(expectedObject);
                    int? jsonArrayLength = arrayElement.ValueKind == JsonValueKind.Null
                        ? null
                        : arrayElement.GetArrayLength();

                    if (expectedArray is null && jsonArrayLength is null)
                    {
                        break;
                    }

                    if (expectedArray is null || jsonArrayLength is null)
                    {
                        errors.Add($"array mismatch for '{property.Name}': expected {expectedArray.ToValueMessage()} but was {arrayElement.ToValueMessage()}");
                        break;
                    }

                    if (jsonArrayLength != expectedArray.Length)
                    {
                        errors.Add($"array length mismatch for '{property.Name}': expected {expectedArray.Length} but was {jsonArrayLength}");
                        break;
                    }

                    for (var i = 0; i < expectedArray.Length; i++)
                    {
                        ValidatePropertyElement(arrayElement[i], expectedArray!.GetValue(i), errors);
                    }
                    break;

                case TypeCode.Object:
                    if (!jsonElement.TryGetProperty(property.Name, out var childElement))
                    {
                        errors.Add($"property '{property.Name}' not found");
                        break;
                    }

                    ValidatePropertyElement(childElement, property.GetValue(expectedObject), errors);
                    break;

                case TypeCode.String:
                    ValidateProperty(jsonElement, expectedObject, property,
                        element => element.GetProperty(property.Name).GetString(), errors);
                    break;

                case TypeCode.Boolean:
                    ValidateProperty(jsonElement, expectedObject, property,
                        element => element.GetProperty(property.Name).GetBoolean(), errors);
                    break;

                case TypeCode.Int16:
                    ValidateProperty(jsonElement, expectedObject, property,
                        element => element.GetProperty(property.Name).GetInt16(), errors);
                    break;

                case TypeCode.Int32:
                    ValidateProperty(jsonElement, expectedObject, property,
                        element => element.GetProperty(property.Name).GetInt32(), errors);
                    break;

                case TypeCode.Int64:
                    ValidateProperty(jsonElement, expectedObject, property,
                        element => element.GetProperty(property.Name).GetInt64(), errors);
                    break;

                case TypeCode.Single:
                    ValidateProperty(jsonElement, expectedObject, property,
                        element => element.GetProperty(property.Name).GetSingle(), errors);
                    break;

                case TypeCode.Double:
                    ValidateProperty(jsonElement, expectedObject, property,
                        element => element.GetProperty(property.Name).GetDouble(), errors);
                    break;

                case TypeCode.Decimal:
                    ValidateProperty(jsonElement, expectedObject, property,
                        element => element.GetProperty(property.Name).GetDecimal(), errors);
                    break;

                default:
                    throw new NotSupportedException($"Type '{property.PropertyType}' is not supported");
            }
        }
    }

    private static void ValidateProperty<T>(
        JsonElement jsonElement,
        object? expectedObject,
        PropertyInfo propInfo,
        Func<JsonElement, T> jsonElementSelector,
        List<string> errors) where T : IComparable<T>?
    {
        var isFound = jsonElement.TryGetProperty(propInfo.Name, out var jsonProperty);
        if (!isFound)
        {
            errors.Add($"property '{propInfo.Name}' not found");
            return;
        }

        var typeInJson = jsonProperty.ValueKind;
        var expected = (T?)propInfo.GetValue(expectedObject);

        var matchingJsonTypes = TypeCodeToJsonValueKind[Type.GetTypeCode(expected.GetTypeOrString())];

        if (!matchingJsonTypes.Contains(typeInJson) && typeInJson != JsonValueKind.Null)
        {
            errors.Add($"type mismatch for '{propInfo.Name}': expected '{string.Join('|', matchingJsonTypes)}' but was '{typeInJson}'");
            return;
        }

        var actual = jsonElementSelector(jsonElement);

        if (actual is null && expected is null)
        {
            return;
        }

        if (expected?.CompareTo(actual) != 0)
        {
            errors.Add(
                $"value mismatch for '{propInfo.Name}': expected {expected.ToValueMessage()} but was {actual.ToValueMessage()}");
        }
    }

    private static void ValidateValue<T>(T? expected, T? actual, List<string> errors)
    {
        if (expected is null && actual is null)
        {
            return;
        }

        if (expected?.Equals(actual) != true)
        {
            errors.Add(
                $"value mismatch for value element: expected {expected.ToValueMessage()} but was {actual.ToValueMessage()}");
        }
    }

    private static PropertyInfo[] GetTypeProperties(this Type type)
    {
        if (!TypePropertiesCache.TryGetValue(type, out var value))
        {
            value = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            TypePropertiesCache[type] = value;
        }

        return value;
    }

    private static Type GetTypeOrString(this object? obj) => obj?.GetType() ?? typeof(string);

    private static string ToValueMessage(this object? obj)
    {
        if (obj is JsonElement jsonElement)
        {
            return $"({jsonElement.ValueKind.ToString().ToLower()})";
        }

        return obj is null ? "(null)" : $"'{obj}'";
    }
}
