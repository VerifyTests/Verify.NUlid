# <img src="/src/icon.png" height="30px"> Verify.NUlid

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://ci.appveyor.com/api/projects/status/qv02ovfsvogyqrer?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-NUlid)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.NUlid.svg)](https://www.nuget.org/packages/Verify.NUlid/)

Extends [Verify](https://github.com/VerifyTests/Verify) to enable scrubbing of Universally Unique Lexicographically Sortable Identifiers via the [NUlid](https://github.com/RobThree/NUlid) package.


**See [Milestones](../../milestones?state=closed) for release notes.**


## NuGet package

https://nuget.org/packages/Verify.NUlid/


## Usage

Call `VerifyNUlid.Initialize()` once at assembly load time.

<!-- snippet: Initialize -->
<a id='snippet-Initialize'></a>
```cs
[ModuleInitializer]
public static void Init() =>
    VerifyNUlid.Initialize();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-Initialize' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


ULIDs will then be scrubbed:

<!-- snippet: Nested -->
<a id='snippet-Nested'></a>
```cs
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
```
<sup><a href='/src/Tests/Samples.cs#L93-L108' title='Snippet source file'>snippet source</a> | <a href='#snippet-Nested' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Result:

<!-- snippet: Samples.UlidScrubbing.verified.txt -->
<a id='snippet-Samples.UlidScrubbing.verified.txt'></a>
```txt
{
  Id: Ulid_1,
  Name: Sarah,
  Description: Sarah (Ulid_1)
}
```
<sup><a href='/src/Tests/Samples.UlidScrubbing.verified.txt#L1-L5' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.UlidScrubbing.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Disabling Scrubbing

To disable scrubbing use `DontScrubUlids()`

<!-- snippet: DontScrub -->
<a id='snippet-DontScrub'></a>
```cs
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
```
<sup><a href='/src/Tests/Samples.cs#L60-L91' title='Snippet source file'>snippet source</a> | <a href='#snippet-DontScrub' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Result: 

<!-- snippet: Samples.DontScrubInstance.verified.txt -->
<a id='snippet-Samples.DontScrubInstance.verified.txt'></a>
```txt
{
  Id: 01JGXG0GDGQEP47CBQ65E50HYH,
  Name: Sarah,
  Description: Sarah (01JGXG0GDGQEP47CBQ65E50HYH)
}
```
<sup><a href='/src/Tests/Samples.DontScrubInstance.verified.txt#L1-L5' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.DontScrubInstance.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
