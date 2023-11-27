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
        private class ValueMismatchTestData : IEnumerable<object[]>
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

        private class LengthMismatchTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    "{\"prop1\": [\"Orange\", \"Banana\"]}",
                    new { prop1 = new[] { "Orange", "Banana", "Cherry" } },
                    2, 3
                };
                yield return new object[]
                {
                    "{\"prop1\": [true, false]}",
                    new { prop1 = new[] { true, false, true } },
                    2, 3
                };
                yield return new object[]
                {
                    "{\"prop1\": [256, 512]}",
                    new { prop1 = new short[] { 256, 512, 1024 } },
                    2, 3
                };
                yield return new object[]
                {
                    "{\"prop1\": [128, 512]}",
                    new { prop1 = new[] { 128, 512, 1024 } },
                    2, 3
                };
                yield return new object[]
                {
                    "{\"prop1\": [256, 512]}",
                    new { prop1 = new long[] { 256, 512, 1024 } },
                    2, 3
                };
                yield return new object[]
                {
                    "{\"prop1\": [12.45, 256.529]}",
                    new { prop1 = new[] { 12.45f, 256.589f, 2541.8913f } },
                    2, 3
                };
                yield return new object[]
                {
                    "{\"prop1\": [12.45, 256.529]}",
                    new { prop1 = new[] { 12.45, 256.589, 2541.8913 } },
                    2, 3
                };
                yield return new object[]
                {
                    "{\"prop1\": [12.45, 256.529]}",
                    new { prop1 = new[] { 12.45m, 256.589m, 2541.8913m } },
                    2, 3
                };
                yield return new object[]
                {
                    "{\"prop1\": [\"Apple\", \"Banana\", \"Cherry\"]}",
                    new { prop1 = new[] { "Apple", "Banana" } },
                    3, 2
                };
                yield return new object[]
                {
                    "{\"prop1\": [true, false, true]}",
                    new { prop1 = new[] { true, false } },
                    3, 2
                };
                yield return new object[]
                {
                    "{\"prop1\": [256, 512, 1024]}",
                    new { prop1 = new short[] { 256, 512 } },
                    3, 2
                };
                yield return new object[]
                {
                    "{\"prop1\": [256, 512, 1024]}",
                    new { prop1 = new[] { 256, 512 } },
                    3, 2
                };
                yield return new object[]
                {
                    "{\"prop1\": [256, 512, 1024]}",
                    new { prop1 = new long[] { 256, 512 } },
                    3, 2
                };
                yield return new object[]
                {
                    "{\"prop1\": [12.45, 256.589, 2541.8913]}",
                    new { prop1 = new[] { 12.45f, 256.589f } },
                    3, 2
                };
                yield return new object[]
                {
                    "{\"prop1\": [12.45, 256.589, 2541.8913]}",
                    new { prop1 = new[] { 12.45, 256.589 } },
                    3, 2
                };
                yield return new object[]
                {
                    "{\"prop1\": [12.45, 256.589, 2541.8913]}",
                    new { prop1 = new[] { 12.45m, 256.589m } },
                    3, 2
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(ValueMismatchTestData))]
        public void TestValueMismatch(string json, object expectedObject, object jsonValue, object expectedValue)
        {
            void Act() => JsonDocument.Parse(json).ValidateMatch(expectedObject);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(jsonValue.ToString()!, exception.Message);
            Assert.Contains(expectedValue.ToString()!, exception.Message);
        }

        [Theory]
        [ClassData(typeof(LengthMismatchTestData))]
        public void TestLengthMismatch(string json, object expectedObject, int actualCount, int expectedCount)
        {
            void Act() => JsonDocument.Parse(json).ValidateMatch(expectedObject);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(actualCount.ToString(), exception.Message);
            Assert.Contains(expectedCount.ToString(), exception.Message);
        }
    }
}
