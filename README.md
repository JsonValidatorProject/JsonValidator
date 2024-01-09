# `{✓}` JSON Validator
This project provides a simple way to validate JSON objects in dotnet. The main use for the tool is in integration tests where you would — ideally — validate an API JSON response manually instead of using the model classes.

[![Build Status](https://github.com/JsonValidatorProject/JsonValidator/workflows/build-and-test/badge.svg "Build Status")](https://github.com/JsonValidatorProject/JsonValidator/actions?query=workflow%3A%22build-and-test%22)
[![Coverage](https://codecov.io/gh/JsonValidatorProject/JsonValidator/branch/main/graph/badge.svg)](https://codecov.io/gh/JsonValidatorProject/JsonValidator)
[![License: MIT](https://img.shields.io/badge/license-MIT-blueviolet)](https://opensource.org/licenses/MIT)
[![pull requests: welcome](https://img.shields.io/badge/pull%20requests-welcome-brightgreen)](https://github.com/JsonValidatorProject/JsonValidator/fork)

### NuGet packages:
[![Nuget: JsonValidator](https://img.shields.io/nuget/v/JsonValidator?label=JsonValidator&logo=nuget)](https://www.nuget.org/packages/JsonValidator)
[![Nuget: JsonValidator.FluentAssertions](https://img.shields.io/nuget/v/JsonValidator.FluentAssertions?label=JsonValidator.FluentAssertions&logo=nuget)](https://www.nuget.org/packages/JsonValidator.FluentAssertions)

## Usage of `JsonValidator`
- Parse the JSON string to a `System.Text.Json.JsonDocument`
- Define the object with the properties you would like to validate. This can be a regular class or an anonymous class as well. We recommend anonymous classes.
- Call the `ValidateMatch` extension method on the `JsonDocument`.
- The `ValidateMatch` will throw a `ValidationFailedException` if any value of the above defined object is not present or does not match the value in the `JsonDocument`.
- Values that are present in the `JsonDocument` but are not defined in the validation object are ignored. They will not influence the validation result.

### Example
```csharp
JsonDocument.Parse(
        """
        {
          "name": "Anna Smith",
          "title": null,
          "level": 2,
          "isAvailable": true,
          "score": 4.5326,
          "capabilities": ["accounting", "marketing", "sales"],                      
          "personalData":
          {
            "age": 29,
            "nickname": "Anny",
            "isMarried": false
          }
        }
        """)
    .ValidateMatch(
        new
        {
            name = "Anna Smith",
            title = null as string,
            level = 2,
            isAvailable = true,
            score = 4.5326,
            capabilities = new[] { "accounting", "marketing", "sales" },
            personalData = new
            {
                age = 29,
                nickname = "Anny",
                isMarried = false,
            }
        });
```

## Usage of `JsonValidator.FluentAssertions`
- The `JsonValidator.FluentAssertions` package depends on and uses both [FluentAssertions](https://github.com/fluentassertions/fluentassertions) and the basic [JsonValidator](https://github.com/JsonValidatorProject/JsonValidator) package.
- This package provides extensions that can be accessed through FluentAssertions' `Should()` method.
- Available extensions:
  - for `System.Text.Json.JsonDocument`: `Match(...)` checks whether the JSON document matches the input object in both structure and values.
  - for `System.Net.Http.HttpResponseMessage`: `HaveJsonBody(...)` checks whether the HTTP response's body as a JSON string matches the input object in both structure and values.

### Examples
For `JsonDocument`:
```csharp
var json = JsonDocument.Parse(myJsonString);

json.Should().Match(
  new
  {
      name = "Anna Smith",
      level = 2,
  },
  "Because...");
```

For `HttpResponseMessage`:
```csharp
HttpResponseMessage response = await _client.SendAsync(request);

response.Should().HaveJsonBody(
  new
  {
      name = "Anna Smith",
      level = 2,
  },
  "Because...");
```
