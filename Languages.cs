namespace ITask5;

public static class Languages
{
    public static readonly IReadOnlyList<string> All = new[]{"en","pl"}.AsReadOnly();

    public static readonly Dictionary<string, string> FullNames = new()
    {
        {"en", "English"},
        {"pl", "Polski"}
    };
}