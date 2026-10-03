namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Requests one MicroBundle and optional configuration during composition.
/// </summary>
public readonly record struct MicroBundleDependencyRequest(
    ulong BundleId,
    ReadOnlyMemory<byte> Configuration)
{
    /// <summary>
    /// Creates an unconfigured request for a MicroBundle.
    /// </summary>
    public static MicroBundleDependencyRequest Unconfigured(ulong bundleId) =>
        new(bundleId, ReadOnlyMemory<byte>.Empty);
}
