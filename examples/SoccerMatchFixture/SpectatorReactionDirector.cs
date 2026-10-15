namespace SoccerMatchFixture;

internal sealed record SpectatorProfile(
    string DigitenId,
    string Name,
    string Section,
    string Seat,
    int ReactionDelayMilliseconds);

internal sealed record SpectatorReaction(
    int EventSequence,
    string DigitenId,
    string Name,
    string Section,
    string Seat,
    string Gesture,
    int PresentationDelayMilliseconds,
    string TriggerEvent);

/// <summary>
/// Deterministic projection from authoritative match events to individual spectator intent.
/// Seat anchors and delay metadata are available to a host adapter; this class does not
/// schedule wall-clock work, animate rigs, or simulate crowd locomotion.
/// </summary>
internal sealed class SpectatorReactionDirector
{
    private readonly IReadOnlyList<SpectatorProfile> _spectators =
    [
        new("digiten.spectator-01", "Ari", "North Stand", "N-12", 180),
        new("digiten.spectator-02", "Bea", "North Stand", "N-13", 430),
        new("digiten.spectator-03", "Cy", "East Stand", "E-04", 760),
        new("digiten.spectator-04", "Dee", "East Stand", "E-05", 310),
        new("digiten.spectator-05", "Em", "South Stand", "S-21", 610)
    ];

    private readonly Dictionary<string, string> _individualStates = new(StringComparer.Ordinal);

    public SpectatorReactionDirector()
    {
        foreach (var spectator in _spectators)
            _individualStates.Add(spectator.DigitenId, "SeatedWatching");
    }

    public IReadOnlyDictionary<string, string> IndividualStates => _individualStates;

    public IReadOnlyList<SpectatorReaction> Observe(MatchEvent matchEvent)
    {
        ArgumentNullException.ThrowIfNull(matchEvent);

        var gesture = matchEvent.Name switch
        {
            "PassCompleted" => "TurnAttentionTowardPlay",
            "ShotAttempted" => "LeanForwardInAnticipation",
            "ShotResolved" => "FollowOutcome",
            "GoalAwarded" => "StandAndCelebrate",
            "RestartReady" => "SettleAndWatch",
            _ => null
        };

        if (gesture is null)
            return [];

        var reactions = new List<SpectatorReaction>(_spectators.Count);
        foreach (var spectator in _spectators)
        {
            _individualStates[spectator.DigitenId] = gesture;
            reactions.Add(new SpectatorReaction(
                matchEvent.Sequence,
                spectator.DigitenId,
                spectator.Name,
                spectator.Section,
                spectator.Seat,
                gesture,
                spectator.ReactionDelayMilliseconds,
                matchEvent.Name));
        }

        return reactions.AsReadOnly();
    }
}
