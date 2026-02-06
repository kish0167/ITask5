namespace ITask5.Helpers;

public static class SongHelpers
{
    public static readonly string[] Colors = new[] 
    { 
        "#FF6B6B", "#4ECDC4", "#45B7D1", "#96CEB4", "#FFEAA7", 
        "#DDA0DD", "#98D8C8", "#F7DC6F", "#BB8FCE", "#85C1E9" 
    };

    public static string GetRandomColor()
    {
        Random random = new Random();
        return Colors[random.Next(Colors.Length)];
    }

    public static string FormatDuration(int? seconds)
    {
        TimeSpan ts = TimeSpan.FromSeconds(seconds ?? 0);
        return ts.Minutes > 0 ? $"{ts.Minutes}:{ts.Seconds:D2}" : $"0:{ts.Seconds:D2}";
    }
}