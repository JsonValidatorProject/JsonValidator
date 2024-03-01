using System.Collections;

namespace JsonValidator.Tests;

public class SimplePropertyTests
{
    public class MatchTests
    {
        private class MatchTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return ["{\"prop1\": \"example\"}", new { prop1 = "example" }];
                yield return ["{\"prop1\": true}", new { prop1 = true }];
                yield return ["{\"prop1\": 1024}", new { prop1 = (short)1024 }];
                yield return ["{\"prop1\": 1024}", new { prop1 = 1024 }];
                yield return ["{\"prop1\": 1024}", new { prop1 = (long)1024 }];
                yield return ["{\"prop1\": 975.4527}", new { prop1 = (float)975.4527 }];
                yield return ["{\"prop1\": 975.4527}", new { prop1 = 975.4527 }];
                yield return ["{\"prop1\": 975.4527}", new { prop1 = (decimal)975.4527 }];
                yield return ["{\"prop1\": null}", new { prop1 = null as string }];
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
                yield return ["{\"prop1\": \"wrong\"}", new { prop1 = "example" }, "wrong", "example"];
                yield return ["{\"prop1\": false}", new { prop1 = true }, false, true];
                yield return ["{\"prop1\": 2048}", new { prop1 = (short)1024 }, 2048, 1024];
                yield return ["{\"prop1\": 2048}", new { prop1 = 1024 }, 2048, 1024];
                yield return ["{\"prop1\": 2048}", new { prop1 = (long)1024 }, 2048, 1024];
                yield return ["{\"prop1\": 575.4527}", new { prop1 = (float)975.4527 }, 575.4527, 975.4527];
                yield return ["{\"prop1\": 575.4527}", new { prop1 = 975.4527 }, 575.4527, 975.4527];
                yield return ["{\"prop1\": 575.4527}", new { prop1 = (decimal)975.4527 }, 575.4527, 975.4527];
                yield return ["{\"prop1\": null}", new { prop1 = "notNull" }, "null", "notNull"];
                yield return ["{\"prop1\": \"notNull\"}", new { prop1 = null as string }, "notNull", "null"];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class TypeMismatchTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return
                [
                    "{\"prop1\": \"128\" }", new { prop1 = 128 }, JsonValueKind.Number, JsonValueKind.String
                ];
                yield return
                [
                    "{\"prop1\": 128 }", new { prop1 = "128" }, JsonValueKind.String, JsonValueKind.Number
                ];
                yield return
                [
                    "{\"prop1\": true }", new { prop1 = "true" }, JsonValueKind.String, JsonValueKind.True
                ];
                yield return
                [
                    "{\"prop1\": \"true\" }", new { prop1 = true }, JsonValueKind.True, JsonValueKind.String
                ];
                yield return
                [
                    "{\"prop1\": 1 }", new { prop1 = true }, JsonValueKind.True, JsonValueKind.Number
                ];
                yield return
                [
                    "{\"prop1\": true }", new { prop1 = 1 }, JsonValueKind.Number, JsonValueKind.True
                ];
                yield return
                [
                    "{\"prop1\": 2541.8914 }", new { prop1 = "2541.8914" }, JsonValueKind.String, JsonValueKind.Number
                ];
                yield return
                [
                    "{\"prop1\": \"2541.8914\" }", new { prop1 = 2541.8914 }, JsonValueKind.Number, JsonValueKind.String
                ];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(ValueMismatchTestData))]
        public void TestMismatch(string json, object expectedObject, object jsonValue, object expectedValue)
        {
            void Act() => JsonDocument.Parse(json).ValidateMatch(expectedObject);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(jsonValue.ToString()!, exception.Message);
            Assert.Contains(expectedValue.ToString()!, exception.Message);
        }

        [Theory]
        [ClassData(typeof(TypeMismatchTestData))]
        public void TestTypeMismatch(string json, object expectedObject, JsonValueKind expectedType, JsonValueKind actualType)
        {
            void Act() => JsonDocument.Parse(json).ValidateMatch(expectedObject);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(expectedType.ToString(), exception.Message);
            Assert.Contains(actualType.ToString(), exception.Message);
        }

        [Fact]
        public void TestPropertyNotFound()
        {
            void Act() => JsonDocument
                .Parse("{\"prop1\": \"value1\"}")
                .ValidateMatch(new { prop2 = "value2" });

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains("prop2", exception.Message);
        }
    }
}
