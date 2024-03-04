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
                yield return
                [
                    "[\"Apple\", \"Banana\", \"Cherry\"]",
                    new[] { "Apple", "Banana", "Cherry" }
                ];
                yield return
                [
                    "{\"prop1\": [\"Apple\", \"Banana\", \"Cherry\"]}",
                    new { prop1 = new[] { "Apple", "Banana", "Cherry" } }
                ];
                yield return
                [
                    "{\"prop1\": [true, false, true]}",
                    new { prop1 = new[] { true, false, true } }
                ];
                yield return
                [
                    "{\"prop1\": [256, 512, 1024]}",
                    new { prop1 = new short[] { 256, 512, 1024 } }
                ];
                yield return
                [
                    "{\"prop1\": [256, 512, 1024]}",
                    new { prop1 = new[] { 256, 512, 1024 } }
                ];
                yield return
                [
                    "{\"prop1\": [256, 512, 1024]}",
                    new { prop1 = new long[] { 256, 512, 1024 } }
                ];
                yield return
                [
                    "{\"prop1\": [12.45, 256.589, 3256.597]}",
                    new { prop1 = new[] { 12.45f, 256.589f, 3256.597f } }
                ];
                yield return
                [
                    "{\"prop1\": [12.45, 256.589, 3256.597, 2541.8913]}",
                    new { prop1 = new[] { 12.45, 256.589, 3256.597, 2541.8913 } }
                ];
                yield return
                [
                    "{\"prop1\": [12.45, 256.589, 3256.597, 2541.8913]}",
                    new { prop1 = new[] { 12.45m, 256.589m, 3256.597m, 2541.8913m } }
                ];
                yield return
                [
                    "{\"prop1\": null}",
                    new { prop1 = null as string[] }
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
                    "{\"prop1\": [\"Orange\", \"Banana\", \"Cherry\"]}",
                    new { prop1 = new[] { "Apple", "Banana", "Cherry" } }
                ];
                yield return
                [
                    "{\"prop1\": [true, false, false]}",
                    new { prop1 = new[] { true, false, true } }
                ];
                yield return
                [
                    "{\"prop1\": [128, 512, 1024]}",
                    new { prop1 = new short[] { 256, 512, 1024 } }
                ];
                yield return
                [
                    "{\"prop1\": [128, 512, 1024]}",
                    new { prop1 = new[] { 256, 512, 1024 } }
                ];
                yield return
                [
                    "{\"prop1\": [128, 512, 1024]}",
                    new { prop1 = new long[] { 256, 512, 1024 } }
                ];
                yield return
                [
                    "{\"prop1\": [12.45, 16.529, 2541.8913]}",
                    new { prop1 = new[] { 12.45f, 256.589f, 2541.8913f } }
                ];
                yield return
                [
                    "{\"prop1\": [12.45, 16.529, 2541.8913]}",
                    new { prop1 = new[] { 12.45, 256.589, 2541.8913 } }
                ];
                yield return
                [
                    "{\"prop1\": [12.45, 16.529, 2541.8913]}",
                    new { prop1 = new[] { 12.45m, 256.589m, 2541.8913m } }
                ];
                yield return
                [
                    "{\"prop1\": null}",
                    new { prop1 = new[] { "Apple", "Banana", "Cherry" } }
                ];
                yield return
                [
                    "{\"prop1\": [\"Apple\", \"Banana\", \"Cherry\"]}",
                    new { prop1 = null as string[] }
                ];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class LengthMismatchTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return
                [
                    "{\"prop1\": [\"Orange\", \"Banana\"]}",
                    new { prop1 = new[] { "Orange", "Banana", "Cherry" } }
                ];
                yield return
                [
                    "{\"prop1\": [true, false]}",
                    new { prop1 = new[] { true, false, true } }
                ];
                yield return
                [
                    "{\"prop1\": [256, 512]}",
                    new { prop1 = new short[] { 256, 512, 1024 } }
                ];
                yield return
                [
                    "{\"prop1\": [128, 512]}",
                    new { prop1 = new[] { 128, 512, 1024 } }
                ];
                yield return
                [
                    "{\"prop1\": [256, 512]}",
                    new { prop1 = new long[] { 256, 512, 1024 } }
                ];
                yield return
                [
                    "{\"prop1\": [12.45, 256.529]}",
                    new { prop1 = new[] { 12.45f, 256.529f, 2541.8913f } }
                ];
                yield return
                [
                    "{\"prop1\": [12.45, 256.529]}",
                    new { prop1 = new[] { 12.45, 256.529, 2541.8913 } }
                ];
                yield return
                [
                    "{\"prop1\": [12.45, 256.529]}",
                    new { prop1 = new[] { 12.45m, 256.529m, 2541.8913m } }
                ];
                yield return
                [
                    "{\"prop1\": [\"Apple\", \"Banana\", \"Cherry\"]}",
                    new { prop1 = new[] { "Apple", "Banana" } }
                ];
                yield return
                [
                    "{\"prop1\": [true, false, true]}",
                    new { prop1 = new[] { true, false } }
                ];
                yield return
                [
                    "{\"prop1\": [256, 512, 1024]}",
                    new { prop1 = new short[] { 256, 512 } }
                ];
                yield return
                [
                    "{\"prop1\": [256, 512, 1024]}",
                    new { prop1 = new[] { 256, 512 } }
                ];
                yield return
                [
                    "{\"prop1\": [256, 512, 1024]}",
                    new { prop1 = new long[] { 256, 512 } }
                ];
                yield return
                [
                    "{\"prop1\": [12.45, 256.589, 2541.8913]}",
                    new { prop1 = new[] { 12.45f, 256.589f } }
                ];
                yield return
                [
                    "{\"prop1\": [12.45, 256.589, 2541.8913]}",
                    new { prop1 = new[] { 12.45, 256.589 } }
                ];
                yield return
                [
                    "{\"prop1\": [12.45, 256.589, 2541.8913]}",
                    new { prop1 = new[] { 12.45m, 256.589m } }
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
                    "{\"prop1\": [\"Apple\", \"Banana\", \"Cherry\"]}",
                    new { prop1 = new[] { 1, 2, 3 } }
                ];
                yield return
                [
                    "{\"prop1\": [true, false, true]}",
                    new { prop1 = new[] { "yes", "no", "yes" } }
                ];
                yield return
                [
                    "{\"prop1\": [256, 512, 1024]}",
                    new { prop1 = new[] { "256", "512", "1024" } }
                ];
                yield return
                [
                    "{\"prop1\": [12.45, 256.589, 2541.8913]}",
                    new { prop1 = new[] { "12.45", "256.589", "2541.8913" } }
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
        [ClassData(typeof(LengthMismatchTestData))]
        public void TestLengthMismatch(string json, object expectedObject)
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
                .Parse("{\"prop1\": [\"value1\", \"value2\"]}")
                .ValidateMatch(new { prop2 = new[] { "Orange", "Banana", "Cherry" } });

            Assert.Throws<ValidationFailedException>(Act);
        }
    }
}
