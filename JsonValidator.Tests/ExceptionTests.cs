namespace JsonValidator.Tests;

public class ExceptionTests
{
    public class ValidationFailedExceptionTests
    {
        [Fact]
        public void TestParameterlessConstructor()
        {
            var ex = new ValidationFailedException();

            Assert.NotNull(ex);
            Assert.False(string.IsNullOrWhiteSpace(ex.Message));
        }

        [Fact]
        public void TestMessageConstructor()
        {
            const string message = "My custom message.";

            var ex = new ValidationFailedException(message);

            Assert.NotNull(ex);
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void TestMessageAndInnerExceptionConstructor()
        {
            const string message = "My custom message.";
            var innerEx = new InvalidOperationException();

            var ex = new ValidationFailedException(message, innerEx);

            Assert.NotNull(ex);
            Assert.Equal(message, ex.Message);
            Assert.Equal(innerEx, ex.InnerException);
        }
    }
}
