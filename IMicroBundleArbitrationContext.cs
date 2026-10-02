namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Host-neutral context supplied during MicroBundle arbitration.
/// </summary>
public interface IMicroBundleArbitrationContext
{
    /// <summary>Gets the runtime identity being assembled.</summary>
    ulong RuntimeId { get; }

    /// <summary>Gets the bundles currently participating in the composition.</summary>
    IReadOnlyList<IMicroBundle> Bundles { get; }

    /// <summary>
    /// Gets the optional host experience context as an opaque domain-neutral value.
    /// Hosts may expose a stronger typed context through their own concrete implementation.
    /// </summary>
    object? ExperienceContext { get; }
}
