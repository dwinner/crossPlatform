using Bogus;
using Recipes.Client.Core.Validation;

namespace Recipes.Client.Core.UnitTests;

public class EmptyOrWithinRangeAttributeTests
{
   private const int MinValueStart = 5;
   private const int MinValueEnd = 10;
   private const int MaxValueStart = 11;
   private const int MaxValueEnd = 15;

   private readonly EmptyOrWithinRangeAttribute _sut;

   public EmptyOrWithinRangeAttributeTests()
   {
      _sut = new Faker<EmptyOrWithinRangeAttribute>()
         .RuleFor(
            attr => attr.MinLength,
            faker => faker.Random.Int(MinValueStart, MinValueEnd))
         .RuleFor(
            attr => attr.MaxLength,
            faker => faker.Random.Int(MaxValueStart, MaxValueEnd))
         .Generate();
   }

   [Fact]
   public void Value_WithinRange_IsValid()
   {
      // Arrange
      var input = new Faker().Random.String2(_sut.MinLength, _sut.MaxLength);

      // Act
      var isValid = _sut.IsValid(input);

      // Assert
      Assert.True(isValid);
   }

   [Fact]
   public void Value_TooShort_IsNotValid()
   {
      // Arrange
      var input = new Faker().Random.String2(1, MinValueStart - 1);

      // Act
      var isValid = _sut.IsValid(input);

      // Assert
      Assert.False(isValid);
   }

   [Fact]
   public void Value_TooLong_IsNotValid()
   {
      // Arrange
      var input = new Faker().Random.String2(MaxValueEnd + 1, MaxValueEnd + 10);

      // Act
      var isValid = _sut.IsValid(input);

      // Assert
      Assert.False(isValid);
   }

   [Fact]
   public void Value_Emtpy_IsValid()
   {
      // Arrange
      var input = string.Empty;

      // Act
      var isValid = _sut.IsValid(input);

      // Assert
      Assert.True(isValid);
   }

   [Fact]
   public void ValueNull_IsNotValid()
   {
      // Arrange
      string? input = null;

      // Act
      var isValid = _sut.IsValid(input);

      // Assert
      Assert.False(isValid);
   }
}