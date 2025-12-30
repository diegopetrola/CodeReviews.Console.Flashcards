using Flashcards.DiegoPetrola.DTOs;
using Flashcards.DiegoPetrola.Entities;

namespace Flashcards.DiegoPetrola.Services
{
    public interface IStudyService
    {
        Task<List<StudySessionDto>> GetStudyReport(DateTime startTime, DateTime endTime);
        Task SaveStudySession(StudySession session);
    }
}