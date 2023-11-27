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
                yield return new object[] { "{\"prop1\": \"example\"}", new { prop1 = "example" } };
                yield return new object[] { "{\"prop1\": true}", new { prop1 = true } };
                yield return new object[] { "{\"prop1\": 1024}", new { prop1 = (short)1024 } };
                yield return new object[] { "{\"prop1\": 1024}", new { prop1 = 1024 } };
                yield return new object[] { "{\"prop1\": 1024}", new { prop1 = (long)1024 } };
                yield return new object[] { "{\"prop1\": 975.4527}", new { prop1 = (float)975.4527 } };
                yield return new object[] { "{\"prop1\": 975.4527}", new { prop1 = 975.4527 } };
                yield return new object[] { "{\"prop1\": 975.4527}", new { prop1 = (decimal)975.4527 } };
                yield return new object[] { "{\"prop1\": null}", new { prop1 = null as string } };
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
        private class MismatchTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "{\"prop1\": \"wrong\"}", new { prop1 = "example" }, "wrong", "example" };
                yield return new object[] { "{\"prop1\": false}", new { prop1 = true }, false, true };
                yield return new object[] { "{\"prop1\": 2048}", new { prop1 = (short)1024 }, 2048, 1024 };
                yield return new object[] { "{\"prop1\": 2048}", new { prop1 = 1024 }, 2048, 1024 };
                yield return new object[] { "{\"prop1\": 2048}", new { prop1 = (long)1024 }, 2048, 1024 };
                yield return new object[] { "{\"prop1\": 575.4527}", new { prop1 = (float)975.4527 }, 575.4527, 975.4527 };
                yield return new object[] { "{\"prop1\": 575.4527}", new { prop1 = 975.4527 }, 575.4527, 975.4527 };
                yield return new object[] { "{\"prop1\": 575.4527}", new { prop1 = (decimal)975.4527 }, 575.4527, 975.4527 };
                yield return new object[] { "{\"prop1\": null}", new { prop1 = "notNull" }, "null", "notNull" };
                yield return new object[] { "{\"prop1\": \"notNull\"}", new { prop1 = null as string }, "notNull", "null" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(MismatchTestData))]
        public void TestMismatch(string json, object expectedObject, object jsonValue, object expectedValue)
        {
            void Act() => JsonDocument.Parse(json).ValidateMatch(expectedObject);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(jsonValue.ToString()!, exception.Message);
            Assert.Contains(expectedValue.ToString()!, exception.Message);
        }
    }
}
