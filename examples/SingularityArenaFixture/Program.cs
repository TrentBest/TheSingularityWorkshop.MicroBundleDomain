using SingularityArenaFixture;
using TheSingularityWorkshop.MicroBundleDomain;

var arenaBundle = new SingularityArenaMicroBundle();
var loadContext = new FixtureLoadContext(runtimeId: 9002);
arenaBundle.Load(loadContext);

var schedule = new ArenaSchedule();
var day = new DateTimeOffset(2030, 6, 15, 0, 0, 0, TimeSpan.Zero);
var soccer = schedule.Schedule(
    "arena.event.soccer-001",
    "singularity-arena",
    ArenaEventKind.SoccerMatch,
    day.AddHours(18),
    day.AddHours(20),
    "soccer.match");
var concert = schedule.Schedule(
    "arena.event.concert-001",
    "singularity-arena",
    ArenaEventKind.Concert,
    day.AddHours(21),
    day.AddHours(23),
    "concert.performance");
var marketFair = schedule.Schedule(
    "arena.event.market-fair-001",
    "singularity-arena",
    ArenaEventKind.MarketFair,
    day.AddDays(1).AddHours(10),
    day.AddDays(1).AddHours(14),
    "market.fair");
var craftFair = schedule.Schedule(
    "arena.event.craft-fair-001",
    "singularity-arena",
    ArenaEventKind.CraftFair,
    day.AddDays(1).AddHours(14),
    day.AddDays(1).AddHours(18),
    "craft.fair");

concert.TransitionTo(ArenaEventState.DoorsOpen);
concert.TransitionTo(ArenaEventState.InProgress);

var audience = new ConcertAudienceReactionDirector(
[
    new("digiten.ari", "north.n12", MusicalFamiliarity.High, 85, true, "friends.north"),
    new("digiten.bea", "north.n13", MusicalFamiliarity.Low, 75, false, "friends.north"),
    new("digiten.cy", "east.e04", MusicalFamiliarity.Some, 20, false),
    new("digiten.dee", "east.e05", MusicalFamiliarity.High, 45, true),
    new("digiten.em", "south.s21", MusicalFamiliarity.Some, 50, false)
]);

var timeline = ConcertCueTimeline.CreateDemo();

Console.WriteLine("SINGULARITY ARENA — SHARED VENUE / DISTINCT EVENT PROVIDERS");
Console.WriteLine($"MicroBundle: {arenaBundle.Descriptor.Id} v{arenaBundle.Descriptor.Version}");
Console.WriteLine($"Runtime: {loadContext.RuntimeId}");
Console.WriteLine($"Venue: singularity-arena");
Console.WriteLine($"[{soccer.StartsAt:HH:mm}–{soccer.EndsAt:HH:mm}] {soccer.Kind} -> {soccer.ProviderId}");
Console.WriteLine($"[{concert.StartsAt:HH:mm}–{concert.EndsAt:HH:mm}] {concert.Kind} -> {concert.ProviderId}");
Console.WriteLine($"[{marketFair.StartsAt:ddd HH:mm}–{marketFair.EndsAt:HH:mm}] {marketFair.Kind} -> {marketFair.ProviderId}");
Console.WriteLine($"[{craftFair.StartsAt:ddd HH:mm}–{craftFair.EndsAt:HH:mm}] {craftFair.Kind} -> {craftFair.ProviderId}");
Console.WriteLine();
Console.WriteLine("CONCERT CUES -> INDIVIDUAL AUDIENCE INTENTIONS");

foreach (var cue in timeline.Cues)
{
    var reactions = audience.Observe(cue);
    Console.WriteLine($"[{cue.Sequence:00}] {cue.EventTimeSeconds,6:0.0}s {cue.Kind}");
    foreach (var reaction in reactions)
    {
        Console.WriteLine(
            $"    +{reaction.PresentationDelayMilliseconds,3}ms " +
            $"{reaction.DigitenId} ({reaction.SeatAnchor}) -> {reaction.Intent}");
    }

    if (audience.Observe(cue).Count != 0)
    {
        Console.Error.WriteLine("Fixture failed: replaying a cue must not emit duplicate reactions.");
        return 1;
    }
}

concert.TransitionTo(ArenaEventState.Completed);

var chorus = timeline.Cues.Single(cue => cue.Kind == ConcertCueKind.ChorusStarted);
var chorusReactions = new ConcertAudienceReactionDirector(
[
    new("digiten.ari", "north.n12", MusicalFamiliarity.High, 85, true),
    new("digiten.bea", "north.n13", MusicalFamiliarity.Low, 75, false),
    new("digiten.cy", "east.e04", MusicalFamiliarity.Some, 20, false),
    new("digiten.dee", "east.e05", MusicalFamiliarity.High, 45, true),
    new("digiten.em", "south.s21", MusicalFamiliarity.Some, 50, false)
]).Observe(chorus);

if (schedule.Events.Count != 4 ||
    soccer.Kind != ArenaEventKind.SoccerMatch ||
    concert.Kind != ArenaEventKind.Concert ||
    marketFair.Kind != ArenaEventKind.MarketFair ||
    craftFair.Kind != ArenaEventKind.CraftFair ||
    chorusReactions.Select(reaction => reaction.Intent).Distinct().Count() < 3 ||
    chorusReactions.Select(reaction => reaction.DigitenId).Distinct(StringComparer.Ordinal).Count() != 5 ||
    concert.State != ArenaEventState.Completed)
{
    Console.Error.WriteLine("Fixture failed: venue scheduling, event separation, audience individuality, or lifecycle invariant failed.");
    return 1;
}

Console.WriteLine();
Console.WriteLine("PASS: one venue schedules soccer, concert, market fair, and craft fair events through distinct providers; concert audience intentions remain individual and repeatable.");
Console.WriteLine("Boundary: schedule is in-memory; cue reactions are intent metadata, not evaluated poses, audio, locomotion, or a live crowd simulation.");
return 0;

internal sealed class FixtureLoadContext(ulong runtimeId) : IMicroBundleLoadContext
{
    public ulong RuntimeId { get; } = runtimeId;

    public bool TryGetConfiguration(ulong bundleId, out ReadOnlyMemory<byte> configuration)
    {
        configuration = ReadOnlyMemory<byte>.Empty;
        return false;
    }
}
