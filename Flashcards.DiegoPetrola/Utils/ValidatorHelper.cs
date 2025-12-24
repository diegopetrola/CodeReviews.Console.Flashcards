using Flashcards.DiegoPetrola.Entities;

namespace Flashcards.DiegoPetrola.Utils;

public static class ValidatorHelper
{
    public static string ValidateFlashard(Flashcard card)
    {
        var validationMessage = "";
        validationMessage += card.Question == "" ? "Question can't be null\n" : "";
        validationMessage += card.Answer == "" ? "Answer can't be null\n" : "";
        validationMessage += card.CardStackId == 0 ? "Please provide the Stack whose this card belongs\n" : "";

        return validationMessage;
    }

    public static string ValidateCardStack(CardStack stack)
    {
        var validationMessage = "";
        validationMessage += stack.Name == "" ? "Name can't be null\n" : "";
        validationMessage += stack.Description == "" ? "Description can't be null\n" : "";

        return validationMessage;
    }
}
