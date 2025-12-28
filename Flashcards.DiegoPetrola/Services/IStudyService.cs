using Flashcards.DiegoPetrola.Entities;

namespace Flashcards.DiegoPetrola.Services
{
    public interface IStudyService
    {
        Task SaveStudySession(StudySession session);
    }
}