using System.Collections;

namespace JsonValidator.Tests;

public class ObjectTests
{
    public class MatchTests
    {
        private class MatchTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return
                [
                    "{\"prop1\": {\"prop11\": \"Pineapple\" }}",
                    new { prop1 = new { prop11 = "Pineapple" } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": true }}",
                    new { prop1 = new { prop11 = true } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 256 }}",
                    new { prop1 = new { prop11 = (short)256 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 256 }}",
                    new { prop1 = new { prop11 = 256 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 256 }}",
                    new { prop1 = new { prop11 = (long)256 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 2541.8914 }}",
                    new { prop1 = new { prop11 = 2541.8914f } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 2541.8914 }}",
                    new { prop1 = new { prop11 = 2541.8914 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 2541.8914 }}",
                    new { prop1 = new { prop11 = 2541.8914m } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": null }}",
                    new { prop1 = new { prop11 = null as string } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": \"Pineapple\", \"prop12\": \"anything\" }}",
                    new { prop1 = new { prop11 = "Pineapple" } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": true, \"prop12\": \"anything\" }}",
                    new { prop1 = new { prop11 = true } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 256, \"prop12\": \"anything\" }}",
                    new { prop1 = new { prop11 = (short)256 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 256, \"prop12\": \"anything\" }}",
                    new { prop1 = new { prop11 = 256 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 256, \"prop12\": \"anything\" }}",
                    new { prop1 = new { prop11 = (long)256 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 2541.891, \"prop12\": \"anything\" }}",
                    new { prop1 = new { prop11 = 2541.891f } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 2541.8913, \"prop12\": \"anything\" }}",
                    new { prop1 = new { prop11 = 2541.8913 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 2541.8913, \"prop12\": \"anything\" }}",
                    new { prop1 = new { prop11 = 2541.8913m } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": null, \"prop12\": \"anything\" }}",
                    new { prop1 = new { prop11 = null as string } }
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
                    "{\"prop1\": {\"prop11\": \"wrong\" }}",
                    new { prop1 = new { prop11 = "Pineapple" } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": true }}",
                    new { prop1 = new { prop11 = false } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 128 }}",
                    new { prop1 = new { prop11 = (short)256 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 128 }}",
                    new { prop1 = new { prop11 = 256 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 128 }}",
                    new { prop1 = new { prop11 = (long)256 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 3569.8914 }}",
                    new { prop1 = new { prop11 = 2541.8914f } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 3569.8914 }}",
                    new { prop1 = new { prop11 = 2541.8914 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 3569.8914 }}",
                    new { prop1 = new { prop11 = 2541.8914m } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": null }}",
                    new { prop1 = new { prop11 = "notNull" } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": \"notNull\" }}",
                    new { prop1 = new { prop11 = null as string } }
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
                    "{\"prop1\": {\"prop11\": \"128\" }}",
                    new { prop1 = new { prop11 = 128 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 128 }}",
                    new { prop1 = new { prop11 = "128" } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": true }}",
                    new { prop1 = new { prop11 = "true" } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": \"true\" }}",
                    new { prop1 = new { prop11 = true } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 1 }}",
                    new { prop1 = new { prop11 = true } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": true }}",
                    new { prop1 = new { prop11 = 1 } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": 2541.8914 }}",
                    new { prop1 = new { prop11 = "2541.8914" } }
                ];
                yield return
                [
                    "{\"prop1\": {\"prop11\": \"2541.8914\" }}",
                    new { prop1 = new { prop11 = 2541.8914 } }
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

        [Fact]
        public void TestPropertyNotFound()
        {
            void Act() => JsonDocument
                .Parse("{\"prop1\": {\"prop11\": \"value1\"}}")
                .ValidateMatch(new { prop1 = new { prop11 = "value1" }, prop2 = new { prop21 = "value2"} });

            Assert.Throws<ValidationFailedException>(Act);
        }

        [Fact]
        public void TestInnerPropertyNotFound()
        {
            void Act() => JsonDocument
                .Parse("{\"prop1\": {\"prop11\": \"value1\", \"prop12\": \"value2\"}}")
                .ValidateMatch(new { prop1 = new { prop11 = "value1", prop13 = "value3" } });

            Assert.Throws<ValidationFailedException>(Act);
        }
    }
}
