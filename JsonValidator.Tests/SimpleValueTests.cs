using System.Globalization;

namespace JsonValidator.Tests;

public class SimpleValueTests
{
    public class MatchTests
    {
        [Fact]
        public void TestStringMatch() => JsonDocument.Parse("\"example\"").ValidateMatch("example");

        [Fact]
        public void TestBooleanMatch() => JsonDocument.Parse("true").ValidateMatch(true);

        [Fact]
        public void TestShortMatch() => JsonDocument.Parse("1024").ValidateMatch((short)1024);

        [Fact]
        public void TestIntMatch() => JsonDocument.Parse("1024").ValidateMatch(1024);

        [Fact]
        public void TestLongMatch() => JsonDocument.Parse("1024").ValidateMatch((long)1024);

        [Fact]
        public void TestFloatMatch() => JsonDocument.Parse("975.4527").ValidateMatch((float)975.4527);

        [Fact]
        public void TestDoubleMatch() => JsonDocument.Parse("975.4527").ValidateMatch(975.4527);

        [Fact]
        public void TestDecimalMatch() => JsonDocument.Parse("975.4527").ValidateMatch((decimal)975.4527);
    }

    public class MismatchTests
    {
        [Fact]
        public void TestStringMismatch()
        {
            const string jsonValue = "wrong";
            const string expectedValue = "example";

            void Act() => JsonDocument.Parse($"\"{jsonValue}\"").ValidateMatch(expectedValue);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(jsonValue, exception.Message);
            Assert.Contains(expectedValue, exception.Message);
        }

        [Fact]
        public void TestBooleanMismatch()
        {
            const bool jsonValue = false;
            const bool expectedValue = true;

            void Act() => JsonDocument.Parse(jsonValue.ToString().ToLower()).ValidateMatch(expectedValue);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(jsonValue.ToString(), exception.Message);
            Assert.Contains(expectedValue.ToString(), exception.Message);
        }

        [Fact]
        public void TestShortMismatch()
        {
            const short jsonValue = 2048;
            const short expectedValue = 1024;

            void Act() => JsonDocument.Parse(jsonValue.ToString()).ValidateMatch(expectedValue);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(jsonValue.ToString(), exception.Message);
            Assert.Contains(expectedValue.ToString(), exception.Message);
        }

        [Fact]
        public void TestIntMismatch()
        {
            const int jsonValue = 2048;
            const int expectedValue = 1024;

            void Act() => JsonDocument.Parse(jsonValue.ToString()).ValidateMatch(expectedValue);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(jsonValue.ToString(), exception.Message);
            Assert.Contains(expectedValue.ToString(), exception.Message);
        }

        [Fact]
        public void TestLongMismatch()
        {
            const long jsonValue = 2048;
            const long expectedValue = 1024;

            void Act() => JsonDocument.Parse(jsonValue.ToString()).ValidateMatch(expectedValue);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(jsonValue.ToString(), exception.Message);
            Assert.Contains(expectedValue.ToString(), exception.Message);
        }

        [Fact]
        public void TestFloatMismatch()
        {
            const float jsonValue = 575.4527f;
            const float expectedValue = 975.4527f;

            void Act() =>
                JsonDocument.Parse(jsonValue.ToString(CultureInfo.InvariantCulture)).ValidateMatch(expectedValue);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(jsonValue.ToString(CultureInfo.InvariantCulture), exception.Message);
            Assert.Contains(expectedValue.ToString(CultureInfo.InvariantCulture), exception.Message);
        }

        [Fact]
        public void TestDoubleMismatch()
        {
            const double jsonValue = 575.4527;
            const double expectedValue = 975.4527;

            void Act() =>
                JsonDocument.Parse(jsonValue.ToString(CultureInfo.InvariantCulture)).ValidateMatch(expectedValue);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(jsonValue.ToString(CultureInfo.InvariantCulture), exception.Message);
            Assert.Contains(expectedValue.ToString(CultureInfo.InvariantCulture), exception.Message);
        }

        [Fact]
        public void TestDecimalMismatch()
        {
            const decimal jsonValue = 575.4527m;
            const decimal expectedValue = 975.4527m;

            void Act() =>
                JsonDocument.Parse(jsonValue.ToString(CultureInfo.InvariantCulture)).ValidateMatch(expectedValue);

            var exception = Assert.Throws<ValidationFailedException>(Act);
            Assert.Contains(jsonValue.ToString(CultureInfo.InvariantCulture), exception.Message);
            Assert.Contains(expectedValue.ToString(CultureInfo.InvariantCulture), exception.Message);
        }
    }
}
