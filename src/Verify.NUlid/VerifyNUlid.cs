namespace VerifyTests;

public static class VerifyNUlid
{
    public static bool Initialized { get; private set; }

    public static void Initialize()
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;

        InnerVerifier.ThrowIfVerifyHasBeenRun();

        CounterContext.Init();
        VerifierSettings.ScrubWindow(ulidLength, ulidLength, ScrubInline, requireWordBoundary: true);
        VerifierSettings
            .AddExtraSettings(_ =>
                _.Converters.Add(new UlidConverter()));
    }

    const int ulidLength = 26;

    // The engine supplies each 26 char window that sits on a word boundary, so only the
    // content check remains here.
    static string? ScrubInline(ReadOnlySpan<char> window, Counter counter, IReadOnlyDictionary<string, object> context)
    {
        if (!context.ScrubUlids())
        {
            return null;
        }

        foreach (var ch in window)
        {
            if (!char.IsLetterOrDigit(ch))
            {
                return null;
            }
        }

        if (!Ulid.TryParse(window.ToString(), out var ulid))
        {
            return null;
        }

        var next = CounterContext.Current.Next(ulid);
        return $"Ulid_{next}";
    }

    public static void DontScrubUlids(this VerifySettings settings) =>
        settings.Context["ScrubUlids"] = false;

    public static SettingsTask DontScrubUlids(this SettingsTask settings)
    {
        settings.CurrentSettings.DontScrubUlids();
        return settings;
    }

    internal static bool ScrubUlids(this IReadOnlyDictionary<string, object> context)
    {
        if (context.TryGetValue("ScrubUlids", out var value))
        {
            return (bool) value;
        }

        return true;
    }
}