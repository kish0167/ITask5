using ITask5.Models;

namespace ITask5.Services.DataGenerator;

public interface IDataGenerator
{
    public PageViewModel GeneratePage(string? language, string? seed, float? likes, int? page, ISession session);
}