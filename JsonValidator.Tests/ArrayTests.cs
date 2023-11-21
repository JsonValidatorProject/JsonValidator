using System.Collections;

namespace JsonValidator.Tests;

public class ArrayTests
{
    public class MatchTests
    {
        private class MatchTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    "{\"prop1\": [\"Apple\", \"Banana\", \"Cherry\"]}",
                    new { prop1 = new[] { "Apple", "Banana", "Cherry" } }
                };
                yield return new object[]
                {
                    "{\"prop1\": [true, false, true]}",
                    new { prop1 = new[] { true, false, true } }
                };
                yield return new object[]
                {
                    "{\"prop1\": [256, 512, 1024]}",
                    new { prop1 = new short[] { 256, 512, 1024 } }
                };
                yield return new object[]
                {
                    "{\"prop1\": [256, 512, 1024]}",
                    new { prop1 = new[] { 256, 512, 1024 } }
                };
                yield return new object[]
                {
                    "{\"prop1\": [256, 512, 1024]}",
                    new { prop1 = new long[] { 256, 512, 1024 } }
                };
                yield return new object[]
                {
                    "{\"prop1\": [12.45, 256.589, 2541.8913]}",
                    new { prop1 = new[] { 12.45f, 256.589f, 2541.8913f } }
                };
                yield return new object[]
                {
                    "{\"prop1\": [12.45, 256.589, 2541.8913]}",
                    new { prop1 = new[] { 12.45, 256.589, 2541.8913 } }
                };
                yield return new object[]
                {
                    "{\"prop1\": [12.45, 256.589, 2541.8913]}",
                    new { prop1 = new[] { 12.45m, 256.589m, 2541.8913m } }
                };
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
                yield return new object[]
                {
                    "{\"prop1\": [\"Orange\", \"Banana\", \"Cherry\"]}",
                    new { prop1 = new[] { "Apple", "Banana", "Cherry" } },
                    "Orange", "Apple"
                };
                yield return new object[]
                {
                    "{\"prop1\": [true, false, false]}",
                    new { prop1 = new[] { true, false, true } },
                    false, true
                };
                yield return new object[]
                {
                    "{\"prop1\": [128, 512, 1024]}",
                    new { prop1 = new short[] { 256, 512, 1024 } },
                    128, 256
                };
                yield return new object[]
                {
                    "{\"prop1\": [128, 512, 1024]}",
                    new { prop1 = new[] { 256, 512, 1024 } },
                    128, 256
                };
                yield return new object[]
                {
                    "{\"prop1\": [128, 512, 1024]}",
                    new { prop1 = new long[] { 256, 512, 1024 } },
                    128, 256
                };
                yield return new object[]
                {
                    "{\"prop1\": [12.45, 16.529, 2541.8913]}",
                    new { prop1 = new[] { 12.45f, 256.589f, 2541.8913f } },
                    16.529, 256.589
                };
                yield return new object[]
                {
                    "{\"prop1\": [12.45, 16.529, 2541.8913]}",
                    new { prop1 = new[] { 12.45, 256.589, 2541.8913 } },
                    16.529, 256.589
                };
                yield return new object[]
                {
                    "{\"prop1\": [12.45, 16.529, 2541.8913]}",
                    new { prop1 = new[] { 12.45m, 256.589m, 2541.8913m } },
                    16.529, 256.589
                };
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
