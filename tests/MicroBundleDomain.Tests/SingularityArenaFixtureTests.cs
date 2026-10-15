using SingularityArenaFixture;
using Xunit;

namespace MicroBundleDomain.Tests;

public sealed class SingularityArenaFixtureTests
{
    [Fact]
    public void OneVenueCanScheduleSoccerAndConcertInSeparateTimeWindows()
    {
        var schedule = new ArenaSchedule();
        var day = new DateTimeOffset(2030, 6, 15, 0, 0, 0, TimeSpan.Zero);

        var soccer = schedule.Schedule("soccer-1", "arena", ArenaEventKind.SoccerMatch,
            day.AddHours(18), day.AddHours(20), "soccer.match");
        var concert = schedule.Schedule("concert-1", "arena", ArenaEventKind.Concert,
            day.AddHours(20), day.AddHours(22), "concert.performance");

        Assert.Equal(2, schedule.Events.Count);
        Assert.Equal(ArenaEventKind.SoccerMatch, soccer.Kind);
        Assert.Equal(ArenaEventKind.Concert, concert.Kind);
        Assert.NotEqual(soccer.ProviderId, concert.ProviderId);
    }

    [Fact]
    public void MarketAndCraftFairsUseDistinctProvidersWithoutAnEconomyDependency()
    {
        var schedule = new ArenaSchedule();
        var start = new DateTimeOffset(2030, 6, 16, 10, 0, 0, TimeSpan.Zero);
        var market = schedule.Schedule("market-1", "city-square", ArenaEventKind.MarketFair,
            start, start.AddHours(4), "market.fair");
        var craft = schedule.Schedule("craft-1", "city-square", ArenaEventKind.CraftFair,
            start.AddHours(4), start.AddHours(8), "craft.fair");
        var bundle = new SingularityArenaMicroBundle();

        Assert.Equal(2, schedule.Events.Count);
        Assert.Equal("market.fair", market.ProviderId);
        Assert.Equal("craft.fair", craft.ProviderId);
        Assert.Empty(bundle.Dependencies);
        Assert.DoesNotContain(bundle.Descriptor.Providers, provider =>
            provider.Id.Contains("economy", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void VenueRejectsOverlappingEventsAndDuplicateIds()
    {
        var schedule = new ArenaSchedule();
        var start = new DateTimeOffset(2030, 6, 15, 18, 0, 0, TimeSpan.Zero);
        schedule.Schedule("event-1", "arena", ArenaEventKind.SoccerMatch,
            start, start.AddHours(2), "soccer.match");

        Assert.Throws<InvalidOperationException>(() =>
            schedule.Schedule("event-2", "arena", ArenaEventKind.Concert,
                start.AddHours(1), start.AddHours(3), "concert.performance"));
        Assert.Throws<InvalidOperationException>(() =>
            schedule.Schedule("event-1", "other-venue", ArenaEventKind.Concert,
                start.AddHours(3), start.AddHours(4), "concert.performance"));
        Assert.Single(schedule.Events);
    }

    [Fact]
    public void EventLifecycleRejectsSkippedAndTerminalTransitions()
    {
        var schedule = new ArenaSchedule();
        var start = new DateTimeOffset(2030, 6, 15, 18, 0, 0, TimeSpan.Zero);
        var concert = schedule.Schedule("concert-1", "arena", ArenaEventKind.Concert,
            start, start.AddHours(2), "concert.performance");

        Assert.Throws<InvalidOperationException>(() => concert.TransitionTo(ArenaEventState.Completed));
        concert.TransitionTo(ArenaEventState.DoorsOpen);
        concert.TransitionTo(ArenaEventState.InProgress);
        concert.TransitionTo(ArenaEventState.Completed);
        Assert.Throws<InvalidOperationException>(() => concert.TransitionTo(ArenaEventState.InProgress));
    }

    [Fact]
    public void ConcertAudienceIntentionsAreIndividualAndReplayable()
    {
        var cue = new ConcertCue(3, 42d, ConcertCueKind.ChorusStarted);
        var audience = CreateAudience();

        var first = audience.Observe(cue);
        var replay = CreateAudience().Observe(cue);

        Assert.Equal(5, first.Count);
        Assert.Equal(first.Select(item => item.Intent), replay.Select(item => item.Intent));
        Assert.Equal(first.Select(item => item.PresentationDelayMilliseconds), replay.Select(item => item.PresentationDelayMilliseconds));
        Assert.True(first.Select(item => item.Intent).Distinct().Count() >= 3);
        Assert.Equal(5, first.Select(item => item.DigitenId).Distinct(StringComparer.Ordinal).Count());
        Assert.All(first, item => Assert.InRange(item.PresentationDelayMilliseconds, 120, 800));
    }

    [Fact]
    public void EncoreCueAllowsDifferentLevelsOfAudienceParticipation()
    {
        var reactions = CreateAudience().Observe(new ConcertCue(6, 116d, ConcertCueKind.EncoreRequested));

        Assert.Equal(AudienceGestureIntent.Clap,
            reactions.Single(reaction => reaction.DigitenId == "digiten.ari").Intent);
        Assert.Equal(AudienceGestureIntent.WatchQuietly,
            reactions.Single(reaction => reaction.DigitenId == "digiten.cy").Intent);
    }

    [Fact]
    public void DuplicateCueDoesNotEmitDuplicateAudienceReactions()
    {
        var audience = CreateAudience();
        var cue = new ConcertCue(3, 42d, ConcertCueKind.ChorusStarted);

        Assert.Equal(5, audience.Observe(cue).Count);
        Assert.Empty(audience.Observe(cue));
    }

    [Fact]
    public void TimelineRejectsOutOfOrderSequenceAndBackwardsEventTime()
    {
        Assert.Throws<ArgumentException>(() => new ConcertCueTimeline(
        [
            new ConcertCue(2, 1d, ConcertCueKind.VerseStarted),
            new ConcertCue(1, 2d, ConcertCueKind.ChorusStarted)
        ]));

        Assert.Throws<ArgumentException>(() => new ConcertCueTimeline(
        [
            new ConcertCue(1, 2d, ConcertCueKind.VerseStarted),
            new ConcertCue(2, 1d, ConcertCueKind.ChorusStarted)
        ]));
    }

    private static ConcertAudienceReactionDirector CreateAudience() => new(
    [
        new("digiten.ari", "north.n12", MusicalFamiliarity.High, 85, true, "friends.north"),
        new("digiten.bea", "north.n13", MusicalFamiliarity.Low, 75, false, "friends.north"),
        new("digiten.cy", "east.e04", MusicalFamiliarity.Some, 20, false),
        new("digiten.dee", "east.e05", MusicalFamiliarity.High, 45, true),
        new("digiten.em", "south.s21", MusicalFamiliarity.Some, 50, false)
    ]);
}
