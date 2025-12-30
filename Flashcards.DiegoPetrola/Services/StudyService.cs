using Flashcards.DiegoPetrola.Context;
using Flashcards.DiegoPetrola.DTOs;
using Flashcards.DiegoPetrola.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<StudySessionDto>> GetStudyReport(DateTime startTime, DateTime endTime)
    {
        var result = await context.StudySessions.Include(s => s.CardStack)
            .Where(s => s.StartTime >= startTime && s.EndTime <= endTime)
            .GroupBy(s => new
            {
                s.StartTime.Year,
                s.StartTime.Month,
                StackName = s.CardStack.Name,
            })
            .Select(group => new StudySessionDto
            {
                Month = group.Key.Month,
                Year = group.Key.Year,
                CardStackName = group.Key.StackName,
                Score = group.Average(s => s.Score)
            }).ToListAsync();

        return result;
    }
}
