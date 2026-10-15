using TheSingularityWorkshop.MicroBundleDomain;

namespace SoccerMatchFixture;

internal static class Program
{
    private static int Main()
    {
        var soccer = new SoccerDomainMicroBundle();
        var context = new FixtureLoadContext(runtimeId: 9001);
        soccer.Load(context);

        var match = new DeterministicMatchFixture();
        var events = match.PlayFirstAttack();

        Console.WriteLine("SOCCER DOMAIN — DETERMINISTIC FIRST ATTACK");
        Console.WriteLine($"MicroBundle: {soccer.Descriptor.Id} v{soccer.Descriptor.Version}");
        Console.WriteLine($"Runtime: {context.RuntimeId}");
        Console.WriteLine($"Teams ready: {match.HomePlayers.Count} home / {match.AwayPlayers.Count} away");
        Console.WriteLine($"Final score: {match.HomeScore}-{match.AwayScore}");
        Console.WriteLine();
        foreach (var item in events)
            Console.WriteLine($"[{item.Sequence:00}] {item.Name}: {item.Detail}");

        if (match.HomeScore != 1 || match.AwayScore != 0)
        {
            Console.Error.WriteLine("Fixture failed: the authoritative goal outcome was not reflected in the score.");
            return 1;
        }

        if (!events.Any(x => x.Name == "PassCompleted") ||
            !events.Any(x => x.Name == "ShotAttempted") ||
            !events.Any(x => x.Name == "GoalAwarded"))
        {
            Console.Error.WriteLine("Fixture failed: the first attack did not produce the expected event sequence.");
            return 1;
        }

        Console.WriteLine();
        Console.WriteLine("PASS: deterministic build-up, completed pass, shot, and authoritative goal.");
        Console.WriteLine("Boundary: this is a domain fixture, not a physics solver, rig animation, or live match.");
        return 0;
    }
}

internal sealed class SoccerDomainMicroBundle : IMicroBundle
{
    public const ulong BundleId = 42020;

    public MicroBundleDescriptor Descriptor { get; } = new(
        id: BundleId,
        version: "0.1.0",
        providers:
        [
            new MicroBundleProvider("soccer.field"),
            new MicroBundleProvider("soccer.match"),
            new MicroBundleProvider("soccer.tactics"),
            new MicroBundleProvider("soccer.events")
        ]);

    public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; } = [];

    public void Load(IMicroBundleLoadContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (context.RuntimeId == 0)
            throw new InvalidOperationException("Soccer requires a non-zero runtime identity.");
    }

    public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (roundIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(roundIndex));
        return false;
    }
}

internal sealed class FixtureLoadContext(ulong runtimeId) : IMicroBundleLoadContext
{
    public ulong RuntimeId { get; } = runtimeId;

    public bool TryGetConfiguration(ulong bundleId, out ReadOnlyMemory<byte> configuration)
    {
        configuration = ReadOnlyMemory<byte>.Empty;
        return false;
    }
}

internal sealed record MatchEvent(int Sequence, string Name, string Detail);

internal sealed record PlayerState(string Name, string Team, string Role, string Zone, string Objective);

internal sealed class DeterministicMatchFixture
{
    private readonly List<MatchEvent> _events = [];
    private int _sequence;

    public IReadOnlyList<PlayerState> HomePlayers { get; } =
    [
        new("A", "Home", "BallCarrier", "MiddleLeft", "Advance while drawing the defender"),
        new("B", "Home", "SupportForward", "FrontCenter", "Arrive in a useful shooting lane"),
        new("C", "Home", "WideSupport", "FrontRight", "Stretch the defensive shape")
    ];

    public IReadOnlyList<PlayerState> AwayPlayers { get; } =
    [
        new("D", "Away", "Defender", "MiddleCenter", "Delay the ball carrier"),
        new("E", "Away", "CoverDefender", "FrontCenter", "Protect the central shooting lane"),
        new("F", "Away", "Goalkeeper", "GoalArea", "Protect the goal")
    ];

    public int HomeScore { get; private set; }
    public int AwayScore { get; private set; }

    public IReadOnlyList<MatchEvent> PlayFirstAttack()
    {
        Add("MatchReady", "Both teams have enough players to start this fixture.");
        Add("Kickoff", "Home starts in possession; attack direction is toward the away goal.");
        Add("PlayerAssigned", "A carries; B supports ahead of the ball; C stretches the right side.");
        Add("DefensiveShape", "D delays A while E protects the central lane; defenders track the ball and runners.");
        Add("OpportunityFound", "B's support position creates a higher-quality forward option than dribbling into pressure.");
        Add("PassAttempted", "A plays a forward pass toward B.");
        Add("PassCompleted", "The fixture's authoritative pass result confirms B receives the ball.");
        Add("PossessionChanged", "Home retains possession; B is now the ball carrier.");
        Add("ShotAttempted", "B shoots from the central-front zone; the goalkeeper is in position.");
        Add("ShotResolved", "The fixture's deterministic match authority resolves the attempt as a goal.");
        HomeScore++;
        Add("GoalAwarded", $"Home goal confirmed by match authority; score is now {HomeScore}-{AwayScore}.");
        Add("RestartReady", "The goal is recorded; the fixture is ready to restart from kickoff.");
        return _events.AsReadOnly();
    }

    private void Add(string name, string detail) =>
        _events.Add(new MatchEvent(++_sequence, name, detail));
}
