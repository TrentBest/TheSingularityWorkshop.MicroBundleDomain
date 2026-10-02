namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Host-neutral context supplied while a MicroBundle is installed.
/// </summary>
public interface IMicroBundleLoadContext
{
    /// <summary>Gets the runtime identity being assembled.</summary>
    ulong RuntimeId { get; }

    /// <summary>
    /// Gets configuration supplied for a MicroBundle request.
    /// </summary>
    bool TryGetConfiguration(
        ulong bundleId,
        out ReadOnlyMemory<byte> configuration);
}
