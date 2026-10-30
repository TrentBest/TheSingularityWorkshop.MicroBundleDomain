namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Requests one exact MicroBundle artifact and optional request configuration during composition.
/// </summary>
/// <remarks>
/// Configuration is input to a dependency request; it is not part of artifact identity.
/// A request without an exact version is a legacy, unresolved request and must not be
/// accepted into a published immutable dependency closure.
/// </remarks>
public readonly record struct MicroBundleDependencyRequest(
    ulong BundleId,
    ReadOnlyMemory<byte> Configuration)
{
    /// <summary>Gets the exact requested artifact version, or null for a legacy unresolved request.</summary>
    public string? Version { get; init; }

    /// <summary>Gets whether this request pins an exact artifact version.</summary>
    public bool IsPinned => BundleId != 0 && !string.IsNullOrWhiteSpace(Version);

    /// <summary>Creates a request pinned to one exact MicroBundle version.</summary>
    public static MicroBundleDependencyRequest Pinned(
        ulong bundleId,
        string version,
        ReadOnlyMemory<byte> configuration = default)
    {
        if (bundleId == 0)
            throw new ArgumentOutOfRangeException(nameof(bundleId), "A MicroBundle ID must be non-zero.");

        ArgumentException.ThrowIfNullOrWhiteSpace(version);

        return new MicroBundleDependencyRequest(bundleId, configuration)
        {
            Version = version
        };
    }

    /// <summary>
    /// Creates a legacy unconfigured request without a version pin.
    /// Such a request may be used by existing in-process integrations but cannot
    /// be published as part of a deterministic immutable dependency closure.
    /// </summary>
    [Obsolete("Use Pinned(bundleId, version) for publishable exact-version dependencies.")]
    public static MicroBundleDependencyRequest Unconfigured(ulong bundleId) =>
        new(bundleId, ReadOnlyMemory<byte>.Empty);

    /// <summary>Creates an unconfigured request pinned to one exact MicroBundle version.</summary>
    public static MicroBundleDependencyRequest Unconfigured(ulong bundleId, string version) =>
        Pinned(bundleId, version, ReadOnlyMemory<byte>.Empty);
}
