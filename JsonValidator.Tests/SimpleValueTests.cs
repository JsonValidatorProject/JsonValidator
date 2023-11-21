using System.Collections;

namespace JsonValidator.Tests;

public class SimpleValueTests
{
    public class MatchTests
    {
        private class MatchTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "\"example\"", "example" };
                yield return new object[] { "true", true };
                yield return new object[] { "1024", (short)1024 };
                yield return new object[] { "1024", 1024 };
                yield return new object[] { "1024", (long)1024 };
                yield return new object[] { "975.4527", (float)975.4527 };
                yield return new object[] { "975.4527", 975.4527 };
                yield return new object[] { "975.4527", (decimal)975.4527 };
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
                yield return new object[] { "\"wrong\"", "example", "wrong", "example" };
                yield return new object[] { "false", true, false, true };
                yield return new object[] { "2048", (short)1024, 2048, 1024 };
                yield return new object[] { "2048", 1024, 2048, 1024 };
                yield return new object[] { "2048", (long)1024, 2048, 1024 };
                yield return new object[] { "575.4527", (float)975.4527, 575.4527, 975.4527 };
                yield return new object[] { "575.4527", 975.4527, 575.4527, 975.4527 };
                yield return new object[] { "575.4527", (decimal)975.4527, 575.4527, 975.4527 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(MismatchTestData))]
        public void TestStringMismatch(string json, object expectedObject, object jsonValue, object expectedValue)
        {
            void Act() => JsonDocument.Parse(json).ValidateMatch(expectedObject);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(jsonValue.ToString()!, exception.Message);
            Assert.Contains(expectedValue.ToString()!, exception.Message);
        }
    }
}
