using ITask5.Models;
using ITask5.Services.DataGenerator;

namespace ITask5.Services.TextGenerator;

public static class LikesRandomizer
{
    public static void GenerateLikesForSongs(List<SongViewModel> songs, GenerationParameters parameters)
    {
        Random random = new Random(HashCode.Combine(parameters.Seed, parameters.Page));
        foreach (SongViewModel song in songs)
        {
            song.Likes = GenerateLikesForSong(parameters.Likes, random);
        }
    }
    
    private static int GenerateLikesForSong(float targetAverage, Random random)
    {
        if (targetAverage <= 0) return 0;
        if (targetAverage >= 10) return 10;
        return SimpleDistribution(targetAverage, random);
    }

    private static int SimpleDistribution(float targetAverage, Random random)
    {
        int baseline = (int)targetAverage;
        float mod = targetAverage % 1f;
        return random.NextSingle() < mod ? baseline + 1 : baseline;
    }

    private static int PoissonLikeDistribution(double lambda, Random random)
    {
        double threshold = random.NextDouble();
        double cumulative = 0;
        for (int k = 0; k <= 10; k++)
        {
            cumulative += PoissonProbability(k, lambda);
            if (threshold <= cumulative)
                return k;
        }
        return 10;
    }
    
    private static double PoissonProbability(int k, double lambda)
    {
        return Math.Pow(lambda, k) * Math.Exp(-lambda) / Factorial(k);
    }
    
    private static double Factorial(int n)
    {
        if (n <= 1) return 1;
        double result = 1;
        for (int i = 2; i <= n; i++) result *= i;
        return result;
    }
}