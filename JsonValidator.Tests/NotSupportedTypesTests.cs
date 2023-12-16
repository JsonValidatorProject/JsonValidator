using System.Collections;

namespace JsonValidator.Tests;

public class NotSupportedTypesTests
{
    private class TestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "{\"prop1\": 10}", new { prop1 = (ushort)10 }, typeof(ushort) };
            yield return new object[] { "{\"prop1\": [10, 20]}", new { prop1 = new ushort[] { 10, 20 } }, typeof(ushort) };
            yield return new object[] { "{\"prop1\": 10}", new { prop1 = (uint)10 }, typeof(uint) };
            yield return new object[] { "{\"prop1\": [10, 20]}", new { prop1 = new uint[] { 10, 20 } }, typeof(uint) };
            yield return new object[] { "{\"prop1\": 10}", new { prop1 = (ulong)10 }, typeof(ulong) };
            yield return new object[] { "{\"prop1\": [10, 20]}", new { prop1 = new ulong[] { 10, 20 } }, typeof(ulong) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public void TestMismatch(string json, object expectedObject, Type type)
    {
        void Act() => JsonDocument.Parse(json).ValidateMatch(expectedObject);

        var exception = Assert.Throws<NotSupportedException>(Act);
        Assert.Contains(type.Name, exception.Message);
    }
}
