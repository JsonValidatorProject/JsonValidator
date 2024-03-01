using System.Collections;

namespace JsonValidator.Tests;

public class SimpleValueTests
{
    public class MatchTests
    {
        private class MatchTestData : IEnumerable<object?[]>
        {
            public IEnumerator<object?[]> GetEnumerator()
            {
                yield return ["\"example\"", "example"];
                yield return ["true", true];
                yield return ["1024", (short)1024];
                yield return ["1024", 1024];
                yield return ["1024", (long)1024];
                yield return ["975.4527", (float)975.4527];
                yield return ["975.4527", 975.4527];
                yield return ["975.4527", (decimal)975.4527];
                yield return ["null", null];
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
        private class MismatchTestData : IEnumerable<object?[]>
        {
            public IEnumerator<object?[]> GetEnumerator()
            {
                yield return ["\"wrong\"", "example", "wrong", "example"];
                yield return ["false", true, "false", "true"];
                yield return ["2048", (short)1024, 2048, 1024];
                yield return ["2048", 1024, 2048, 1024];
                yield return ["2048", (long)1024, 2048, 1024];
                yield return ["575.4527", (float)975.4527, 575.4527, 975.4527];
                yield return ["575.4527", 975.4527, 575.4527, 975.4527];
                yield return ["575.4527", (decimal)975.4527, 575.4527, 975.4527];
                yield return ["\"notNull\"", null, "String", "Null"];
                yield return ["null", "notNull", "Null", "String"];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(MismatchTestData))]
        public void TestStringMismatch(
            string json, object expectedObject, object jsonValueOrType, object expectedValueType)
        {
            void Act() => JsonDocument.Parse(json).ValidateMatch(expectedObject);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(jsonValueOrType.ToString()!, exception.Message);
            Assert.Contains(expectedValueType.ToString()!, exception.Message);
        }
    }
}
