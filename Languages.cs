using System.Globalization;
namespace ITask5;

public static class Languages
{
    public static IReadOnlyList<string> All => new []{"en", "pl"};
    public static string GetLanguageName(string twoLetterCode)
    {
        var culture = new CultureInfo(twoLetterCode);
        return Capitalize(culture.NativeName);
    }

    private static string Capitalize(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            throw new ArgumentException();
        }
        return char.ToUpper(s[0]) + s.Substring(1);
    }
}