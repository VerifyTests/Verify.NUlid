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
        VerifierSettings.AddScrubber(ScrubInline);
        VerifierSettings
            .AddExtraSettings(_ =>
                _.Converters.Add(new UlidConverter()));
    }

    static void ScrubInline(StringBuilder builder, Counter counter, IReadOnlyDictionary<string, object> context)
    {
        if (!context.ScrubUlids())
        {
            return;
        }

        const int ulidLength = 26;
        var builderIndex = 0;
        var index = 0;

        while (true)
        {
            if (index > builder.Length - ulidLength)
            {
                break;
            }

            var nextIndex = index + ulidLength;
            if (nextIndex < builder.Length )
            {
                var nextChar = builder[nextIndex];
                if (char.IsLetterOrDigit(nextChar))
                {
                    index++;
                    builderIndex++;
                    continue;
                }
            }

            if (index > 0)
            {
                if (char.IsLetterOrDigit(builder[index - 1]))
                {
                    index++;
                    builderIndex++;
                    continue;
                }
            }

            var slice = builder.ToString(index, ulidLength);
            if (slice.Any(_ => !char.IsLetterOrDigit(_)) ||
                !Ulid.TryParse(slice, out var ulid))
            {
                index++;
                builderIndex++;
                continue;
            }

            var next = CounterContext.Current.Next(ulid);
            builder.Overwrite($"Ulid_{next}", builderIndex, ulidLength);
            builderIndex += ulidLength;
            index += ulidLength;
        }
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

    static void Overwrite(this StringBuilder builder, string value, int index, int length)
    {
        builder.Remove(index, length);
        builder.Insert(index, value);
    }
}