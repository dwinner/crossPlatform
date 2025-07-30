using Recipes.Client.Core.ViewModels;
using Recipes.Mobile.Shared.TemplateSelectors;

namespace Recipes.Mobile.UnitTests;

// All the code in this file is included in all platforms.
public class InstructionsDataTemplateSelectorTests
{
   [Fact]
   public void SelectTemplate_NoteVM_Should_Return_NoteTemplate()
   {
      // Arrange
      var template = new DataTemplate();
      var sut = new InstructionsDataTemplateSelector
      {
         NoteTemplate = template,
         InstructionTemplate = new DataTemplate()
      };

      // Act
      var result = sut.SelectTemplate(AutoFaker.Generate<NoteViewModel>(), null);

      // Assert
      Assert.Equal(template, result);
   }

   [Fact]
   public void SelectTemplate_InstructionVM_Should_Return_InstructionTemplate()
   {
      var sut = new InstructionsDataTemplateSelector
      {
         NoteTemplate = new DataTemplate(),
         InstructionTemplate = new DataTemplate()
      };

      var result = sut.SelectTemplate(AutoFaker.Generate<InstructionViewModel>(), null);
      Assert.Equal(sut.InstructionTemplate, result);
   }

   [Fact]
   public void SelectTemplate_RecipeDetailVM_Should_Return_Null()
   {
      var template1 = new DataTemplate();
      var template2 = new DataTemplate();
      var sut = new InstructionsDataTemplateSelector
      {
         NoteTemplate = template1,
         InstructionTemplate = template2
      };

      var result = sut.SelectTemplate(AutoFaker.Generate<RecipeDetailViewModel>(), null);
      Assert.Null(result);
   }
}