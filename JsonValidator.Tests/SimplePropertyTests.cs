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
                yield return ["{\"prop1\": \"wrong\"}", new { prop1 = "example" }];
                yield return ["{\"prop1\": false}", new { prop1 = true }];
                yield return ["{\"prop1\": 2048}", new { prop1 = (short)1024 }];
                yield return ["{\"prop1\": 2048}", new { prop1 = 1024 }];
                yield return ["{\"prop1\": 2048}", new { prop1 = (long)1024 }];
                yield return ["{\"prop1\": 575.4527}", new { prop1 = (float)975.4527 }];
                yield return ["{\"prop1\": 575.4527}", new { prop1 = 975.4527 }];
                yield return ["{\"prop1\": 575.4527}", new { prop1 = (decimal)975.4527 }];
                yield return ["{\"prop1\": null}", new { prop1 = "notNull" }];
                yield return ["{\"prop1\": \"notNull\"}", new { prop1 = null as string }];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class TypeMismatchTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return
                [
                    "{\"prop1\": \"128\" }", new { prop1 = 128 }
                ];
                yield return
                [
                    "{\"prop1\": 128 }", new { prop1 = "128" }
                ];
                yield return
                [
                    "{\"prop1\": true }", new { prop1 = "true" }
                ];
                yield return
                [
                    "{\"prop1\": \"true\" }", new { prop1 = true }
                ];
                yield return
                [
                    "{\"prop1\": 1 }", new { prop1 = true }
                ];
                yield return
                [
                    "{\"prop1\": true }", new { prop1 = 1 }
                ];
                yield return
                [
                    "{\"prop1\": 2541.8914 }", new { prop1 = "2541.8914" }
                ];
                yield return
                [
                    "{\"prop1\": \"2541.8914\" }", new { prop1 = 2541.8914 }
                ];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(ValueMismatchTestData))]
        public void TestMismatch(string json, object expectedObject)
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
