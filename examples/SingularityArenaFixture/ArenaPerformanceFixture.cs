namespace SingularityArenaFixture;

/// <summary>Event kinds supported by the shared venue schedule.</summary>
public enum ArenaEventKind
{
    SoccerMatch,
    Concert,
    Karaoke
}

/// <summary>Lifecycle owned by the scheduled event domain, not by presentation.</summary>
public enum ArenaEventState
{
    Scheduled,
    DoorsOpen,
    InProgress,
    Paused,
    Completed,
    Cancelled
}

/// <summary>An event assigned to a venue and an event-specific provider.</summary>
public sealed class ArenaEvent
{
    internal ArenaEvent(string id, string venueId, ArenaEventKind kind,
        DateTimeOffset startsAt, DateTimeOffset endsAt, string providerId)
    {
        Id = id;
        VenueId = venueId;
        Kind = kind;
        StartsAt = startsAt;
        EndsAt = endsAt;
        ProviderId = providerId;
    }

    public string Id { get; }
    public string VenueId { get; }
    public ArenaEventKind Kind { get; }
    public DateTimeOffset StartsAt { get; }
    public DateTimeOffset EndsAt { get; }
    public string ProviderId { get; }
    public ArenaEventState State { get; private set; } = ArenaEventState.Scheduled;

    public void TransitionTo(ArenaEventState next)
    {
        var allowed = (State, next) switch
        {
            (ArenaEventState.Scheduled, ArenaEventState.DoorsOpen) => true,
            (ArenaEventState.Scheduled, ArenaEventState.Cancelled) => true,
            (ArenaEventState.DoorsOpen, ArenaEventState.InProgress) => true,
            (ArenaEventState.DoorsOpen, ArenaEventState.Cancelled) => true,
            (ArenaEventState.InProgress, ArenaEventState.Paused) => true,
            (ArenaEventState.InProgress, ArenaEventState.Completed) => true,
            (ArenaEventState.InProgress, ArenaEventState.Cancelled) => true,
            (ArenaEventState.Paused, ArenaEventState.InProgress) => true,
            (ArenaEventState.Paused, ArenaEventState.Cancelled) => true,
            _ => false
        };

        if (!allowed)
            throw new InvalidOperationException($"Invalid arena event transition: {State} -> {next}.");

        State = next;
    }
}

/// <summary>
/// In-memory deterministic scheduling reference for one venue. It validates identity,
/// intervals, and event lifecycle; persistence and wall-clock dispatch belong to a host.
/// </summary>
public sealed class ArenaSchedule
{
    private readonly List<ArenaEvent> _events = [];
    private readonly HashSet<string> _eventIds = new(StringComparer.Ordinal);

    public IReadOnlyList<ArenaEvent> Events => _events.AsReadOnly();

    public ArenaEvent Schedule(string id, string venueId, ArenaEventKind kind,
        DateTimeOffset startsAt, DateTimeOffset endsAt, string providerId)
    {
        RequireIdentifier(id, nameof(id));
        RequireIdentifier(venueId, nameof(venueId));
        RequireIdentifier(providerId, nameof(providerId));
        if (endsAt <= startsAt)
            throw new ArgumentException("Event end must be after its start.", nameof(endsAt));
        if (!_eventIds.Add(id))
            throw new InvalidOperationException($"Arena event ID '{id}' is already scheduled.");

        var overlaps = _events.Any(existing =>
            existing.VenueId == venueId &&
            existing.State != ArenaEventState.Cancelled &&
            startsAt < existing.EndsAt && existing.StartsAt < endsAt);
        if (overlaps)
        {
            _eventIds.Remove(id);
            throw new InvalidOperationException($"Venue '{venueId}' already has an overlapping event.");
        }

        var scheduled = new ArenaEvent(id, venueId, kind, startsAt, endsAt, providerId);
        _events.Add(scheduled);
        return scheduled;
    }

    private static void RequireIdentifier(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Identifier must not be empty.", parameterName);
    }
}

/// <summary>MicroBundle exposing the shared venue and event-scheduling capability.</summary>
public sealed class SingularityArenaMicroBundle : IMicroBundle
{
    public const ulong BundleId = 42030;

    public MicroBundleDescriptor Descriptor { get; } = new(
        id: BundleId,
        version: "0.1.0",
        providers:
        [
            new MicroBundleProvider("arena.venue"),
            new MicroBundleProvider("arena.schedule"),
            new MicroBundleProvider("arena.event-lifecycle")
        ]);

    public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; } = [];

    public void Load(IMicroBundleLoadContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (context.RuntimeId == 0)
            throw new InvalidOperationException("Singularity Arena requires a non-zero runtime identity.");
    }

    public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (roundIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(roundIndex));
        return false;
    }
}

/// <summary>Stable cue kinds emitted by the concert/performance domain.</summary>
public enum ConcertCueKind
{
    PerformerEnteredStage,
    VerseStarted,
    ChorusStarted,
    InstrumentalBreakStarted,
    FinalChorusStarted,
    EncoreRequested,
    ShowCompleted
}

/// <summary>A cue is an authoritative event-time marker, not an audio sample.</summary>
public sealed record ConcertCue(int Sequence, double EventTimeSeconds, ConcertCueKind Kind);

/// <summary>Validated cue timeline that can be replayed deterministically in tests.</summary>
public sealed class ConcertCueTimeline
{
    public ConcertCueTimeline(IEnumerable<ConcertCue> cues)
    {
        ArgumentNullException.ThrowIfNull(cues);
        var materialized = cues.ToArray();
        if (materialized.Length == 0)
            throw new ArgumentException("A concert timeline requires at least one cue.", nameof(cues));

        for (var i = 0; i < materialized.Length; i++)
        {
            var cue = materialized[i];
            if (cue.Sequence <= 0 || !double.IsFinite(cue.EventTimeSeconds) || cue.EventTimeSeconds < 0d)
                throw new ArgumentException("Cue sequence must be positive and event time finite/non-negative.", nameof(cues));
            if (i > 0 && (materialized[i - 1].Sequence >= cue.Sequence ||
                          materialized[i - 1].EventTimeSeconds > cue.EventTimeSeconds))
                throw new ArgumentException("Cue sequences must increase and event times must not move backward.", nameof(cues));
        }

        Cues = Array.AsReadOnly(materialized);
    }

    public IReadOnlyList<ConcertCue> Cues { get; }

    public static ConcertCueTimeline CreateDemo() => new(
    [
        new(1, 0d, ConcertCueKind.PerformerEnteredStage),
        new(2, 18d, ConcertCueKind.VerseStarted),
        new(3, 42d, ConcertCueKind.ChorusStarted),
        new(4, 68d, ConcertCueKind.InstrumentalBreakStarted),
        new(5, 91d, ConcertCueKind.FinalChorusStarted),
        new(6, 116d, ConcertCueKind.EncoreRequested),
        new(7, 132d, ConcertCueKind.ShowCompleted)
    ]);
}

public enum MusicalFamiliarity
{
    Low,
    Some,
    High
}

/// <summary>Context used to select an audience intention; no movement is simulated here.</summary>
public sealed record ConcertAudienceProfile(
    string DigitenId,
    string SeatAnchor,
    MusicalFamiliarity Familiarity,
    int Expressiveness,
    bool PrefersSingingAlong,
    string? SocialGroupId = null);

public enum AudienceGestureIntent
{
    WatchPerformer,
    ListenQuietly,
    SingAlong,
    Clap,
    Sway,
    DanceInPlace,
    WatchQuietly
}

/// <summary>A selected intention with stable delay metadata for a host scheduler.</summary>
public sealed record ConcertAudienceReaction(
    int CueSequence,
    string DigitenId,
    string SeatAnchor,
    AudienceGestureIntent Intent,
    int PresentationDelayMilliseconds);

/// <summary>
/// Deterministic cue-to-intention projection. It does not schedule wall-clock work,
/// evaluate Gesture poses, generate audio, or simulate crowd locomotion.
/// </summary>
public sealed class ConcertAudienceReactionDirector
{
    private readonly IReadOnlyList<ConcertAudienceProfile> _audience;
    private readonly HashSet<int> _observedCueSequences = [];
    private readonly Dictionary<string, AudienceGestureIntent> _individualStates = new(StringComparer.Ordinal);

    public ConcertAudienceReactionDirector(IEnumerable<ConcertAudienceProfile> audience)
    {
        ArgumentNullException.ThrowIfNull(audience);
        var materialized = audience.ToArray();
        if (materialized.Any(profile => profile is null ||
            string.IsNullOrWhiteSpace(profile.DigitenId) ||
            string.IsNullOrWhiteSpace(profile.SeatAnchor) ||
            profile.Expressiveness is < 0 or > 100))
            throw new ArgumentException("Audience profiles require identity, seat anchor, and expressiveness in [0, 100].", nameof(audience));
        if (materialized.Select(profile => profile.DigitenId).Distinct(StringComparer.Ordinal).Count() != materialized.Length)
            throw new ArgumentException("Audience Digiten IDs must be unique.", nameof(audience));

        _audience = Array.AsReadOnly(materialized);
    }

    public IReadOnlyDictionary<string, AudienceGestureIntent> IndividualStates => _individualStates;

    public IReadOnlyList<ConcertAudienceReaction> Observe(ConcertCue cue)
    {
        ArgumentNullException.ThrowIfNull(cue);
        if (cue.Sequence <= 0 || !double.IsFinite(cue.EventTimeSeconds) || cue.EventTimeSeconds < 0d)
            throw new ArgumentException("Cue must have a positive sequence and finite non-negative event time.", nameof(cue));
        if (!_observedCueSequences.Add(cue.Sequence))
            return [];

        var reactions = new List<ConcertAudienceReaction>(_audience.Count);
        foreach (var profile in _audience)
        {
            var intent = ChooseIntent(profile, cue.Kind);
            _individualStates[profile.DigitenId] = intent;
            reactions.Add(new ConcertAudienceReaction(
                cue.Sequence, profile.DigitenId, profile.SeatAnchor, intent, StableDelay(profile.DigitenId, cue.Sequence)));
        }

        return reactions.AsReadOnly();
    }

    private static AudienceGestureIntent ChooseIntent(ConcertAudienceProfile profile, ConcertCueKind cue) =>
        cue switch
        {
            ConcertCueKind.PerformerEnteredStage => AudienceGestureIntent.WatchPerformer,
            ConcertCueKind.VerseStarted when profile.PrefersSingingAlong &&
                profile.Familiarity == MusicalFamiliarity.High && profile.Expressiveness >= 45
                => AudienceGestureIntent.SingAlong,
            ConcertCueKind.ChorusStarted or ConcertCueKind.FinalChorusStarted
                when profile.PrefersSingingAlong && profile.Familiarity != MusicalFamiliarity.Low
                => AudienceGestureIntent.SingAlong,
            ConcertCueKind.ChorusStarted or ConcertCueKind.FinalChorusStarted
                when profile.Expressiveness >= 60 => AudienceGestureIntent.Clap,
            ConcertCueKind.ChorusStarted or ConcertCueKind.FinalChorusStarted
                when profile.Expressiveness >= 30 => AudienceGestureIntent.Sway,
            ConcertCueKind.InstrumentalBreakStarted when profile.Expressiveness >= 70
                => AudienceGestureIntent.DanceInPlace,
            ConcertCueKind.InstrumentalBreakStarted when profile.Expressiveness >= 30
                => AudienceGestureIntent.Sway,
            ConcertCueKind.ShowCompleted when profile.Expressiveness >= 50 => AudienceGestureIntent.Clap,
            ConcertCueKind.ShowCompleted => AudienceGestureIntent.WatchQuietly,
            _ => AudienceGestureIntent.ListenQuietly
        };

    private static int StableDelay(string digitenId, int cueSequence)
    {
        // FNV-1a over UTF-16 code units plus cue identity; avoids process-randomized string hashes.
        ulong hash = 14695981039346656037UL;
        foreach (var character in digitenId)
        {
            hash ^= character;
            hash *= 1099511628211UL;
        }

        hash ^= (uint)cueSequence;
        hash *= 1099511628211UL;
        return 120 + (int)(hash % 681UL);
    }
}
