namespace JsonValidator.Tests;

public class ExceptionMessageTests
{
    [Fact]
    public void TestComplexObject()
    {
        void Act() => JsonDocument.Parse(
                """
                {
                  "name": "Jenny Smith",
                  "level": "2",
                  "isAvailable": true,
                  "nextLevels": [ 3, 4, 5, 6 ]
                }
                """)
            .ValidateMatch(
                new
                {
                    id = 42,
                    name = "Jenny Doe",
                    level = 2,
                    nextLevels = new[] { 3, 4, 5 }
                }, new ValidationConfiguration(exactMatch: true));

        var exception = Assert.Throws<ValidationFailedException>(Act);

        Assert.Contains("'$.id' not found", exception.Message);
        Assert.Contains("Value for '$.name' was 'Jenny Smith' but should have been 'Jenny Doe'", exception.Message);
        Assert.Contains("Type for '$.level' was String but should have been Number", exception.Message);
        Assert.Contains("Excess array elements in the JSON document: '$.nextLevels[3]'", exception.Message);
        Assert.Contains("Excess found in the JSON document: '$.isAvailable'", exception.Message);
    }
}
