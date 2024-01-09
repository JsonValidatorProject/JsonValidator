using System.Net.Http.Headers;
using System.Text.Json;
using FluentAssertions;
using JsonValidator.FluentAssertions.Json;
using JsonValidator.FluentAssertions.HttpResponse;
using Xunit.Sdk;

namespace JsonValidator.FluentAssertions.Tests;

public class ComplexObjectTests
{
    public class MatchTests
    {
        [Fact]
        public void TestComplexObjectFromJson()
        {
            var json = JsonDocument.Parse(
                """
                {
                  "name": "Jenny Smith",
                  "title": null,
                  "level": 2,
                  "isAvailable": true,
                  "employeeScore": 4.5326,
                  "nextLevels": [ 3, 4, 5 ],
                  "capabilities": ["accounting", "marketing", "sales"],
                  "pollAnswers": [true, false, true, true],
                  "oldEmployeeScores": [4.2563, 4.2311, 4.3956],
                  "personalData":
                  {
                    "age": 41,
                    "nickname": "JenTheMan",
                    "isMarried": false,
                    "creditScore": 789.3698
                  },
                  "shadows": [
                    {
                      "name": "Jack Smith",
                      "level": 0,
                      "employeeScore": 3.256,
                      "isAvailable": true,
                      "title": null
                    },
                    {
                      "name": "Anna Nicholson",
                      "level": 1,
                      "employeeScore": 3.35,
                      "isAvailable": false,
                      "title": null
                    },
                    null
                  ]
                }
                """);

            var expected = new
            {
                name = "Jenny Smith",
                title = null as string,
                level = 2,
                isAvailable = true,
                employeeScore = 4.5326,
                nextLevels = new[] { 3, 4, 5 },
                capabilities = new[] { "accounting", "marketing", "sales" },
                pollAnswers = new[] { true, false, true, true },
                oldEmployeeScores = new[] { 4.2563, 4.2311, 4.3956 },
                personalData = new { age = 41, nickname = "JenTheMan", isMarried = false, creditScore = 789.3698 },
                shadows = new[]
                {
                    new
                    {
                        name = "Jack Smith",
                        level = 0,
                        employeeScore = 3.256,
                        isAvailable = true,
                        title = null as string
                    },
                    new
                    {
                        name = "Anna Nicholson",
                        level = 1,
                        employeeScore = 3.35,
                        isAvailable = false,
                        title = null as string
                    },
                    null
                }
            };

            json.Should().Match(expected, "The two object should match");
        }

        [Fact]
        public void TestComplexObjectFromHttpResponse()
        {
            var jsonString =
                """
                {
                  "name": "Jenny Smith",
                  "title": null,
                  "level": 2,
                  "isAvailable": true,
                  "employeeScore": 4.5326,
                  "nextLevels": [ 3, 4, 5 ],
                  "capabilities": ["accounting", "marketing", "sales"],
                  "pollAnswers": [true, false, true, true],
                  "oldEmployeeScores": [4.2563, 4.2311, 4.3956],
                  "personalData":
                  {
                    "age": 41,
                    "nickname": "JenTheMan",
                    "isMarried": false,
                    "creditScore": 789.3698
                  },
                  "shadows": [
                    {
                      "name": "Jack Smith",
                      "level": 0,
                      "employeeScore": 3.256,
                      "isAvailable": true,
                      "title": null
                    },
                    {
                      "name": "Anna Nicholson",
                      "level": 1,
                      "employeeScore": 3.35,
                      "isAvailable": false,
                      "title": null
                    },
                    null
                  ]
                }
                """;

            var expected = new
            {
                name = "Jenny Smith",
                title = null as string,
                level = 2,
                isAvailable = true,
                employeeScore = 4.5326,
                nextLevels = new[] { 3, 4, 5 },
                capabilities = new[] { "accounting", "marketing", "sales" },
                pollAnswers = new[] { true, false, true, true },
                oldEmployeeScores = new[] { 4.2563, 4.2311, 4.3956 },
                personalData = new { age = 41, nickname = "JenTheMan", isMarried = false, creditScore = 789.3698 },
                shadows = new[]
                {
                    new
                    {
                        name = "Jack Smith",
                        level = 0,
                        employeeScore = 3.256,
                        isAvailable = true,
                        title = null as string
                    },
                    new
                    {
                        name = "Anna Nicholson",
                        level = 1,
                        employeeScore = 3.35,
                        isAvailable = false,
                        title = null as string
                    },
                    null
                }
            };

            var response = new HttpResponseMessage
            {
                Content = new StringContent(jsonString, mediaType: new MediaTypeHeaderValue("application/json"))
            };

            response.Should().HaveJsonBody(expected, "The two object should match");
        }
    }

    public class MismatchTests
    {
        [Fact]
        public void TestComplexObjectFromJson()
        {
            var json = JsonDocument.Parse(
                """
                {
                  "name": "Jenny Smith",
                  "title": null,
                  "level": 2,
                  "isAvailable": true,
                  "employeeScore": 4.5326,
                  "nextLevels": [ 3, 4, 5 ],
                  "capabilities": ["accounting", "marketing", "sales"],
                  "pollAnswers": [true, false, true, true],
                  "oldEmployeeScores": [4.2563, 4.2311, 4.3956],
                  "personalData":
                  {
                    "age": 41,
                    "nickname": "JenTheMan",
                    "isMarried": false,
                    "creditScore": 789.3698
                  },
                  "shadows": [
                    {
                      "name": "Jack Smith",
                      "level": 0,
                      "employeeScore": 3.256,
                      "isAvailable": true,
                      "title": null
                    },
                    {
                      "name": "Anna Nicholson",
                      "level": 1,
                      "employeeScore": 3.35,
                      "isAvailable": false,
                      "title": null
                    },
                    null
                  ]
                }
                """);

            var expected = new
            {
                name = "Johnny Smith",
                title = null as string,
                level = 2,
                isAvailable = true,
                employeeScore = 4.5326,
                nextLevels = new[] { 3, 4, 5 },
                capabilities = new[] { "accounting", "marketing", "development" },
                pollAnswers = new[] { true, false, true, true },
                oldEmployeeScores = new[] { 4.2563, 4.2311, 4.3956 },
                personalData = new { age = 41, nickname = "JenTheMan", isMarried = false, creditScore = 789.3698 },
                shadows = new[]
                {
                    new
                    {
                        name = "Jack Smith",
                        level = 0,
                        employeeScore = 3.256,
                        isAvailable = true,
                        title = null as string
                    },
                    new
                    {
                        name = "Anna Nicholson",
                        level = 1,
                        employeeScore = 3.35,
                        isAvailable = false,
                        title = null as string
                    },
                    null
                }
            };

            Action act = () => json.Should().Match(expected, "The two object should match");

            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected object to match the JSON input, but found differences *");
        }

        [Fact]
        public void TestComplexObjectFromHttpResponse()
        {
            var jsonString =
                """
                {
                  "name": "Jenny Smith",
                  "title": null,
                  "level": 2,
                  "isAvailable": true,
                  "employeeScore": 4.5326,
                  "nextLevels": [ 3, 4, 5 ],
                  "capabilities": ["accounting", "marketing", "sales"],
                  "pollAnswers": [true, false, true, true],
                  "oldEmployeeScores": [4.2563, 4.2311, 4.3956],
                  "personalData":
                  {
                    "age": 41,
                    "nickname": "JenTheMan",
                    "isMarried": false,
                    "creditScore": 789.3698
                  },
                  "shadows": [
                    {
                      "name": "Jack Smith",
                      "level": 0,
                      "employeeScore": 3.256,
                      "isAvailable": true,
                      "title": null
                    },
                    {
                      "name": "Anna Nicholson",
                      "level": 1,
                      "employeeScore": 3.35,
                      "isAvailable": false,
                      "title": null
                    },
                    null
                  ]
                }
                """;

            var expected = new
            {
                name = "Johnny Smith",
                title = null as string,
                level = 2,
                isAvailable = true,
                employeeScore = 4.5326,
                nextLevels = new[] { 3, 4, 5 },
                capabilities = new[] { "accounting", "marketing", "development" },
                pollAnswers = new[] { true, false, true, true },
                oldEmployeeScores = new[] { 4.2563, 4.2311, 4.3956 },
                personalData = new { age = 41, nickname = "JenTheMan", isMarried = false, creditScore = 789.3698 },
                shadows = new[]
                {
                    new
                    {
                        name = "Jack Smith",
                        level = 0,
                        employeeScore = 3.256,
                        isAvailable = true,
                        title = null as string
                    },
                    new
                    {
                        name = "Anna Nicholson",
                        level = 1,
                        employeeScore = 3.35,
                        isAvailable = false,
                        title = null as string
                    },
                    null
                }
            };

            var response = new HttpResponseMessage
            {
                Content = new StringContent(jsonString, mediaType: new MediaTypeHeaderValue("application/json"))
            };

            Action act = () => response.Should().HaveJsonBody(expected, "The two object should match");

            act.Should()
                .Throw<XunitException>()
                .WithMessage("Expected response body to match the JSON input, but found differences *");
        }
    }
}
