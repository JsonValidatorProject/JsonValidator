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
                // yield return
                // [
                //     """
                //     {
                //       "key.1": "VALUE_1",
                //       "key.2": "VALUE_2",
                //       "key.3": "VALUE_3",
                //       "key.4": "VALUE_4"
                //     }
                //     """,
                //     new Dictionary<string, string>
                //     {
                //         ["key.1"] = "VALUE_1",
                //         ["key.2"] = "VALUE_2",
                //         ["key.3"] = "VALUE_3",
                //         ["key.4"] = "VALUE_4"
                //     }
                // ];
                //
                // yield return
                // [
                //     """
                //     {
                //       "key.1": { "k11": "v11", "k12": "v12" },
                //       "key.2": { "k21": "v21", "k22": "v22" }
                //     }
                //     """,
                //     new Dictionary<string, object>
                //     {
                //         ["key.1"] =  new { k11 = "v11", k12 = "v12" },
                //         ["key.2"] = new { k21 = "v21", k22 = "v22" }
                //     }
                // ];
                //
                // yield return
                // [
                //     """
                //     {
                //       "key.1": {
                //         "key.11": "VALUE_11",
                //         "key.12": "VALUE_12"
                //       },
                //       "key.2": {
                //         "key.21": "VALUE_21",
                //         "key.22": "VALUE_22"
                //       }
                //     }
                //     """,
                //     new Dictionary<string, Dictionary<string, string>>
                //     {
                //         ["key.1"] = new()
                //         {
                //             ["key.11"] = "VALUE_11",
                //             ["key.12"] = "VALUE_12",
                //         },
                //         ["key.2"] = new()
                //         {
                //             ["key.21"] = "VALUE_21",
                //             ["key.22"] = "VALUE_22",
                //         }
                //     }
                // ];

                // yield return
                // [
                //     """
                //     {
                //       "key.1": [ 1, 2, 3 ],
                //       "key.2": [ 4, 5, 6 ]
                //     }
                //     """,
                //     new Dictionary<string, int[]>
                //     {
                //         ["key.1"] = [ 1, 2, 3 ],
                //         ["key.2"] = [ 4, 5, 6 ]
                //     }
                // ];
            //
            //     yield return
            //     [
            //         """
            //         {
            //           "key.1": [ { "o11": "v11", "o12": "v12" }, { "o21": "v21", "o22": "v22" } ],
            //           "key.2": [ { "o31": "v31", "o32": "v32" }, { "o41": "v41", "o42": "v42" } ]
            //         }
            //         """,
            //         new Dictionary<string, object[]>
            //         {
            //             ["key.1"] = [new { o11 = "v11", o12 = "v12" }, new { o21 = "v21", o22 = "v22" }],
            //             ["key.2"] = [new { o11 = "v11", o12 = "v12" }, new { o21 = "v21", o22 = "v22" }]
            //         }
            //     ];
            //
            //     yield return
            //     [
            //         """
            //         {
            //           "myDictionary": {
            //             "key.1": "VALUE_1",
            //             "key.2": "VALUE_2",
            //             "key.3": "VALUE_3",
            //             "key.4": "VALUE_4"
            //           }
            //         }
            //         """,
            //         new
            //         {
            //             myDictionary = new Dictionary<string, string>
            //             {
            //                 ["key.1"] = "VALUE_1",
            //                 ["key.2"] = "VALUE_2",
            //                 ["key.3"] = "VALUE_3",
            //                 ["key.4"] = "VALUE_4",
            //             }
            //         }
            //     ];
            //
            //     yield return
            //     [
            //         """
            //         {
            //           "myDictionary": {
            //               "key.1": { "k11": "v11", "k12": "v12" },
            //               "key.2": { "k21": "v21", "k22": "v22" }
            //             }
            //         }
            //         """,
            //         new
            //         {
            //             myDictionary = new Dictionary<string, object>
            //             {
            //                 ["key.1"] =  new { k11 = "v11", k12 = "v12" },
            //                 ["key.2"] = new { k21 = "v21", k22 = "v22" }
            //             }
            //         }
            //     ];
            //
            //     yield return
            //     [
            //         """
            //         {
            //           "myDictionary": {
            //             "key.1": {
            //               "key.11": "VALUE_11",
            //               "key.12": "VALUE_12"
            //             },
            //             "key.2": {
            //               "key.21": "VALUE_21",
            //               "key.22": "VALUE_22"
            //             }
            //           }
            //         }
            //         """,
            //         new
            //         {
            //             myDictionary = new Dictionary<string, Dictionary<string, string>>
            //             {
            //                 ["key.1"] = new()
            //                 {
            //                     ["key.11"] = "VALUE_11",
            //                     ["key.12"] = "VALUE_12"
            //                 },
            //                 ["key.2"] = new()
            //                 {
            //                     ["key.21"] = "VALUE_21",
            //                     ["key.22"] = "VALUE_22"
            //                 }
            //             }
            //         }
            //     ];
            //
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
                        ["key.1"] = [ 1, 2, 3 ],
                        ["key.2"] = [ 4, 5, 6 ]
                    }
                }
            ];
            //
            //     yield return
            //     [
            //         """
            //         {
            //           "myDictionary": {
            //             "key.1": [ { "o11": "v11", "o12": "v12" }, { "o21": "v21", "o22": "v22" } ],
            //             "key.2": [ { "o31": "v31", "o32": "v32" }, { "o41": "v41", "o42": "v42" } ]
            //           }
            //         }
            //         """,
            //         new
            //         {
            //             myDictionary = new Dictionary<string, object[]>
            //             {
            //                 ["key.1"] = [new { o11 = "v11", o12 = "v12" }, new { o21 = "v21", o22 = "v22" }],
            //                 ["key.2"] = [new { o11 = "v11", o12 = "v12" }, new { o21 = "v21", o22 = "v22" }]
            //             }
            //         }
            //     ];
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
        [Fact]
        public void TestValueMismatch()
        {
            void Act() => JsonDocument.Parse(
                    """
                    {
                      "key.1": "VALUE_1",
                      "key.2": "VALUE_2",
                      "key.3": "VALUE_3",
                      "key.NaN": "VALUE_4"
                    }
                    """)
                .ValidateMatch(
                    new Dictionary<string, string>
                    {
                        ["key.1"] = "VALUE_1",
                        ["key.2"] = "VALUE_2",
                        ["key.3"] = "VALUE_3",
                        ["key.4"] = "VALUE_4",
                    });

            var exception = Assert.Throws<ValidationFailedException>(Act);

            var x = exception;
        }
    }


}
