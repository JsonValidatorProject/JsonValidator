using System.Collections;

namespace JsonValidator.Tests;

public class DictionaryTests
{
    public class MatchTests
    {
        private class MatchTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return
                [
                    """
                    {
                      "key.1": "VALUE_1",
                      "key.2": "VALUE_2",
                      "key.3": "VALUE_3",
                      "key.4": "VALUE_4"
                    }
                    """,
                    new Dictionary<string, string>
                    {
                        ["key.1"] = "VALUE_1", ["key.2"] = "VALUE_2", ["key.3"] = "VALUE_3", ["key.4"] = "VALUE_4"
                    }
                ];

                yield return
                [
                    """
                    {
                      "key.1": { "k11": "v11", "k12": "v12" },
                      "key.2": { "k21": "v21", "k22": "v22" }
                    }
                    """,
                    new Dictionary<string, object>
                    {
                        ["key.1"] = new { k11 = "v11", k12 = "v12" }, ["key.2"] = new { k21 = "v21", k22 = "v22" }
                    }
                ];

                yield return
                [
                    """
                    {
                      "key.1": {
                        "key.11": "VALUE_11",
                        "key.12": "VALUE_12"
                      },
                      "key.2": {
                        "key.21": "VALUE_21",
                        "key.22": "VALUE_22"
                      }
                    }
                    """,
                    new Dictionary<string, Dictionary<string, string>>
                    {
                        ["key.1"] = new() { ["key.11"] = "VALUE_11", ["key.12"] = "VALUE_12", },
                        ["key.2"] = new() { ["key.21"] = "VALUE_21", ["key.22"] = "VALUE_22", }
                    }
                ];

                yield return
                [
                    """
                    {
                      "key.1": [ 1, 2, 3 ],
                      "key.2": [ 4, 5, 6 ]
                    }
                    """,
                    new Dictionary<string, int[]> { ["key.1"] = [1, 2, 3], ["key.2"] = [4, 5, 6] }
                ];

                yield return
                [
                    """
                    {
                      "key.1": [ { "o11": "v11", "o12": "v12" }, { "o21": "v21", "o22": "v22" } ],
                      "key.2": [ { "o31": "v31", "o32": "v32" }, { "o41": "v41", "o42": "v42" } ]
                    }
                    """,
                    new Dictionary<string, object[]>
                    {
                        ["key.1"] = [new { o11 = "v11", o12 = "v12" }, new { o21 = "v21", o22 = "v22" }],
                        ["key.2"] = [new { o31 = "v31", o32 = "v32" }, new { o41 = "v41", o42 = "v42" }]
                    }
                ];

                yield return
                [
                    """
                    {
                      "myDictionary": {
                        "key.1": "VALUE_1",
                        "key.2": "VALUE_2",
                        "key.3": "VALUE_3",
                        "key.4": "VALUE_4"
                      }
                    }
                    """,
                    new
                    {
                        myDictionary = new Dictionary<string, string>
                        {
                            ["key.1"] = "VALUE_1", ["key.2"] = "VALUE_2", ["key.3"] = "VALUE_3", ["key.4"] = "VALUE_4",
                        }
                    }
                ];

                yield return
                [
                    """
                    {
                      "myDictionary": {
                          "key.1": { "k11": "v11", "k12": "v12" },
                          "key.2": { "k21": "v21", "k22": "v22" }
                        }
                    }
                    """,
                    new
                    {
                        myDictionary = new Dictionary<string, object>
                        {
                            ["key.1"] = new { k11 = "v11", k12 = "v12" },
                            ["key.2"] = new { k21 = "v21", k22 = "v22" }
                        }
                    }
                ];

                yield return
                [
                    """
                    {
                      "myDictionary": {
                        "key.1": {
                          "key.11": "VALUE_11",
                          "key.12": "VALUE_12"
                        },
                        "key.2": {
                          "key.21": "VALUE_21",
                          "key.22": "VALUE_22"
                        }
                      }
                    }
                    """,
                    new
                    {
                        myDictionary = new Dictionary<string, Dictionary<string, string>>
                        {
                            ["key.1"] = new() { ["key.11"] = "VALUE_11", ["key.12"] = "VALUE_12" },
                            ["key.2"] = new() { ["key.21"] = "VALUE_21", ["key.22"] = "VALUE_22" }
                        }
                    }
                ];

                yield return
                [
                    """
                    {
                      "myDictionary": {
                        "key.1": [ 1, 2, 3 ],
                        "key.2": [ 4, 5, 6 ]
                      }
                    }
                    """,
                    new
                    {
                        myDictionary = new Dictionary<string, int[]>
                        {
                            ["key.1"] = [1, 2, 3], ["key.2"] = [4, 5, 6]
                        }
                    }
                ];

                yield return
                [
                    """
                    {
                      "myDictionary": {
                        "key.1": [ { "o11": "v11", "o12": "v12" }, { "o21": "v21", "o22": "v22" } ],
                        "key.2": [ { "o31": "v31", "o32": "v32" }, { "o41": "v41", "o42": "v42" } ]
                      }
                    }
                    """,
                    new
                    {
                        myDictionary = new Dictionary<string, object[]>
                        {
                            ["key.1"] = [new { o11 = "v11", o12 = "v12" }, new { o21 = "v21", o22 = "v22" }],
                            ["key.2"] = [new { o31 = "v31", o32 = "v32" }, new { o41 = "v41", o42 = "v42" }]
                        }
                    }
                ];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(MatchTestData))]
        public void TestMatch(string json, object expectedObject) =>
            JsonDocument.Parse(json).ValidateMatch(expectedObject);
    }

    public class MismatchTests
    {
        private class ValueMismatchTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return
                [
                    """
                    {
                      "key.1": "VALUE_1",
                      "key.2": "VALUE_2",
                      "key.3": "VALUE_3",
                      "key.4": "VALUE_5"
                    }
                    """,
                    new Dictionary<string, string>
                    {
                        ["key.1"] = "VALUE_1", ["key.2"] = "VALUE_2", ["key.3"] = "VALUE_3", ["key.4"] = "VALUE_4"
                    }
                ];

                yield return
                [
                    """
                    {
                      "key.1": { "k11": "v11", "k12": "v12" },
                      "key.2": { "k21": "v21", "k22": "v42" }
                    }
                    """,
                    new Dictionary<string, object>
                    {
                        ["key.1"] = new { k11 = "v11", k12 = "v12" }, ["key.2"] = new { k21 = "v21", k22 = "v22" }
                    }
                ];

                yield return
                [
                    """
                    {
                      "key.1": {
                        "key.11": "VALUE_11",
                        "key.12": "VALUE_12"
                      },
                      "key.2": {
                        "key.21": "VALUE_21",
                        "key.22": "VALUE_42"
                      }
                    }
                    """,
                    new Dictionary<string, Dictionary<string, string>>
                    {
                        ["key.1"] = new() { ["key.11"] = "VALUE_11", ["key.12"] = "VALUE_12", },
                        ["key.2"] = new() { ["key.21"] = "VALUE_21", ["key.22"] = "VALUE_22", }
                    }
                ];

                yield return
                [
                    """
                    {
                      "key.1": [ 1, 2, 3 ],
                      "key.2": [ 4, 5, 9 ]
                    }
                    """,
                    new Dictionary<string, int[]> { ["key.1"] = [1, 2, 3], ["key.2"] = [4, 5, 6] }
                ];

                yield return
                [
                    """
                    {
                      "key.1": [ { "o11": "v11", "o12": "v12" }, { "o21": "v21", "o22": "v22" } ],
                      "key.2": [ { "o31": "v31", "o32": "v32" }, { "o41": "v41", "o42": "v82" } ]
                    }
                    """,
                    new Dictionary<string, object[]>
                    {
                        ["key.1"] = [new { o11 = "v11", o12 = "v12" }, new { o21 = "v21", o22 = "v22" }],
                        ["key.2"] = [new { o31 = "v31", o32 = "v32" }, new { o41 = "v41", o42 = "v42" }]
                    }
                ];

                yield return
                [
                    """
                    {
                      "myDictionary": {
                        "key.1": "VALUE_1",
                        "key.2": "VALUE_2",
                        "key.3": "VALUE_3",
                        "key.4": "VALUE_5"
                      }
                    }
                    """,
                    new
                    {
                        myDictionary = new Dictionary<string, string>
                        {
                            ["key.1"] = "VALUE_1", ["key.2"] = "VALUE_2", ["key.3"] = "VALUE_3", ["key.4"] = "VALUE_4",
                        }
                    }
                ];

                yield return
                [
                    """
                    {
                      "myDictionary": {
                          "key.1": { "k11": "v11", "k12": "v12" },
                          "key.2": { "k21": "v21", "k22": "v42" }
                        }
                    }
                    """,
                    new
                    {
                        myDictionary = new Dictionary<string, object>
                        {
                            ["key.1"] = new { k11 = "v11", k12 = "v12" },
                            ["key.2"] = new { k21 = "v21", k22 = "v22" }
                        }
                    }
                ];

                yield return
                [
                    """
                    {
                      "myDictionary": {
                        "key.1": {
                          "key.11": "VALUE_11",
                          "key.12": "VALUE_12"
                        },
                        "key.2": {
                          "key.21": "VALUE_21",
                          "key.22": "VALUE_42"
                        }
                      }
                    }
                    """,
                    new
                    {
                        myDictionary = new Dictionary<string, Dictionary<string, string>>
                        {
                            ["key.1"] = new() { ["key.11"] = "VALUE_11", ["key.12"] = "VALUE_12" },
                            ["key.2"] = new() { ["key.21"] = "VALUE_21", ["key.22"] = "VALUE_22" }
                        }
                    }
                ];

                yield return
                [
                    """
                    {
                      "myDictionary": {
                        "key.1": [ 1, 2, 3 ],
                        "key.2": [ 4, 5, 9 ]
                      }
                    }
                    """,
                    new
                    {
                        myDictionary = new Dictionary<string, int[]>
                        {
                            ["key.1"] = [1, 2, 3], ["key.2"] = [4, 5, 6]
                        }
                    }
                ];

                yield return
                [
                    """
                    {
                      "myDictionary": {
                        "key.1": [ { "o11": "v11", "o12": "v12" }, { "o21": "v21", "o22": "v22" } ],
                        "key.2": [ { "o31": "v31", "o32": "v32" }, { "o41": "v41", "o42": "v82" } ]
                      }
                    }
                    """,
                    new
                    {
                        myDictionary = new Dictionary<string, object[]>
                        {
                            ["key.1"] = [new { o11 = "v11", o12 = "v12" }, new { o21 = "v21", o22 = "v22" }],
                            ["key.2"] = [new { o31 = "v31", o32 = "v32" }, new { o41 = "v41", o42 = "v42" }]
                        }
                    }
                ];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class TypeMismatchTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return
                [
                    """
                    {
                      "key.1": "10",
                      "key.2": "11",
                      "key.3": "12",
                      "key.4": "13"
                    }
                    """,
                    new Dictionary<string, int>
                    {
                        ["key.1"] = 10, ["key.2"] = 11, ["key.3"] = 12, ["key.4"] = 13
                    }
                ];

                yield return
                [
                    """
                    {
                      "myDictionary": {
                        "key.1": 20,
                        "key.2": 21,
                        "key.3": 22,
                        "key.4": 23
                      }
                    }
                    """,
                    new
                    {
                        myDictionary = new Dictionary<string, string>
                        {
                            ["key.1"] = "20", ["key.2"] = "21", ["key.3"] = "22", ["key.4"] = "23"
                        }
                    }
                ];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(ValueMismatchTestData))]
        public void TestValueMismatch(string json, object expectedObject)
        {
            void Act() => JsonDocument.Parse(json).ValidateMatch(expectedObject);

            Assert.Throws<ValidationFailedException>(Act);
        }

        [Theory]
        [ClassData(typeof(TypeMismatchTestData))]
        public void TestTypeMismatch(string json, object expectedObject)
        {
            void Act() => JsonDocument.Parse(json).ValidateMatch(expectedObject);

            Assert.Throws<ValidationFailedException>(Act);
        }
    }
}
