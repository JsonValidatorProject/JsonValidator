namespace JsonValidator.Tests;

public class ComplexObjectTests
{
    public class MatchTests
    {
        [Fact]
        public void TestComplexObject() =>
            JsonDocument.Parse(
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
                    """)
                .ValidateMatch(
                    new
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
                        personalData = new
                        {
                            age = 41, nickname = "JenTheMan", isMarried = false, creditScore = 789.3698
                        },
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
                    });
    }
}
