[TestFixture]
public class Samples
{
    [Test]
    public Task Typed()
    {
        var id = Ulid.NewUlid();
        return Verify(id);
    }

    [Test]
    public Task String()
    {
        var id = Ulid.NewUlid().ToString();
        return Verify(id);
    }

    [Test]
    public Task StringDontScrub() =>
        Verify("01JGXD29BZA4PD6KSHBZWHMDPQ")
            .DontScrubUlids();

    [Test]
    public Task Prefixed()
    {
        var id = Ulid.NewUlid().ToString();
        return Verify($" {id}");
    }

    [Test]
    public Task Suffixed()
    {
        var id = Ulid.NewUlid().ToString();
        return Verify($"{id} ");
    }

    [Test]
    public Task Padded()
    {
        var id = Ulid.NewUlid().ToString();
        return Verify($" {id} ");
    }

    [Test]
    public Task TooLong() =>
        Verify("01JGXD29BZA4PD6KSHBZWHMDPQA");

    [Test]
    public Task TooLongPrefixed() =>
        Verify(" 01JGXD29BZA4PD6KSHBZWHMDPQA");

    [Test]
    public Task TooLongSuffixed() =>
        Verify("01JGXD29BZA4PD6KSHBZWHMDPQA ");

    [Test]
    public Task TooLongPadded() =>
        Verify(" 01JGXD29BZA4PD6KSHBZWHMDPQA ");

    #region DontScrub

    [Test]
    public Task DontScrubFluent()
    {
        var id = Ulid.Parse("01JGXG0GDGQEP47CBQ65E50HYH");
        var target = new Person
        {
            Id = id,
            Name = "Sarah",
            Description = $"Sarah ({id})"
        };
        return Verify(target)
            .DontScrubUlids();
    }

    [Test]
    public Task DontScrubInstance()
    {
        var id = Ulid.Parse("01JGXG0GDGQEP47CBQ65E50HYH");
        var target = new Person
        {
            Id = id,
            Name = "Sarah",
            Description = $"Sarah ({id})"
        };
        var settings = new VerifySettings();
        settings.DontScrubUlids();
        return Verify(target, settings);
    }

    #endregion

    #region Nested

    [Test]
    public Task UlidScrubbing()
    {
        var id = Ulid.NewUlid();
        var target = new Person
        {
            Id = id,
            Name = "Sarah",
            Description = $"Sarah ({id})"
        };
        return Verify(target);
    }

    #endregion
}

public class Person
{
    public required Ulid Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}