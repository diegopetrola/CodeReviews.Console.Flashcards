using Flashcards.DiegoPetrola.Context;
using Flashcards.DiegoPetrola.Entities;

namespace Flashcards.DiegoPetrola.Services;

public class StudyService(FlashcardContext context) : IStudyService
{
    public async Task SaveStudySession(StudySession session)
    {
        session.EndTime = DateTime.Now;
        if (session.StartTime > session.EndTime)
            throw new ArgumentException("Start Time needs to be greater than End Time.");

        await context.AddAsync(session);
    }
}
