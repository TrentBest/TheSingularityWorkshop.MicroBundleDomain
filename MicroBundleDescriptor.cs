namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Describes a MicroBundle capability without imposing meaning on the hosting ecosystem.
/// </summary>
public sealed class MicroBundleDescriptor
{
    private readonly IReadOnlyList<MicroBundleDependency> _dependencies;
    private readonly IReadOnlyList<MicroBundleProvider> _providers;

    /// <summary>
    /// Initializes a MicroBundle descriptor from its portable identity and declared composition surface.
    /// </summary>
    /// <param name="id">Stable non-zero identity of the MicroBundle.</param>
    /// <param name="version">Domain-defined version string for the capability.</param>
    /// <param name="dependencies">Optional capabilities required by the MicroBundle.</param>
    /// <param name="providers">Optional opaque provider identifiers exposed by the MicroBundle.</param>
    public MicroBundleDescriptor(
        ulong id,
        string version,
        IEnumerable<MicroBundleDependency>? dependencies = null,
        IEnumerable<MicroBundleProvider>? providers = null)
    {
        if (id == 0)
            throw new ArgumentOutOfRangeException(nameof(id), "A MicroBundle ID must be non-zero.");

        if (string.IsNullOrWhiteSpace(version))
            throw new ArgumentException("A MicroBundle version is required.", nameof(version));

        _dependencies = MaterializeDependencies(dependencies);
        _providers = MaterializeProviders(providers);

        Id = id;
        Version = version;
    }

    /// <summary>
    /// Gets the stable identity of the MicroBundle.
    /// </summary>
    public ulong Id { get; }

    /// <summary>
    /// Gets the version declared by the MicroBundle author.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Gets the declared dependency identities.
    /// </summary>
    public IReadOnlyList<MicroBundleDependency> Dependencies => _dependencies;

    /// <summary>
    /// Gets opaque provider identifiers declared by the MicroBundle.
    /// </summary>
    public IReadOnlyList<MicroBundleProvider> Providers => _providers;

    private static IReadOnlyList<MicroBundleDependency> MaterializeDependencies(
        IEnumerable<MicroBundleDependency>? dependencies)
    {
        var values = (dependencies ?? []).ToArray();

        if (values.Any(x => x.BundleId == 0))
            throw new ArgumentException("A MicroBundle dependency ID must be non-zero.", nameof(dependencies));

        if (values.Select(x => x.BundleId).Distinct().Count() != values.Length)
            throw new ArgumentException("A MicroBundle cannot declare the same dependency more than once.", nameof(dependencies));

        return Array.AsReadOnly(values);
    }

    private static IReadOnlyList<MicroBundleProvider> MaterializeProviders(
        IEnumerable<MicroBundleProvider>? providers)
    {
        var values = (providers ?? []).ToArray();

        if (values.Any(x => string.IsNullOrWhiteSpace(x.Id)))
            throw new ArgumentException("A MicroBundle provider requires an ID.", nameof(providers));

        if (values.Select(x => x.Id).Distinct(StringComparer.Ordinal).Count() != values.Length)
            throw new ArgumentException("A MicroBundle cannot declare the same provider more than once.", nameof(providers));

        return Array.AsReadOnly(values);
    }
}

/// <summary>
/// Declares a dependency on another MicroBundle without interpreting its domain semantics.
/// </summary>
/// <param name="BundleId">Stable identity of the required MicroBundle.</param>
public readonly record struct MicroBundleDependency(ulong BundleId);

/// <summary>
/// Identifies a provider exposed by a MicroBundle.
/// </summary>
/// <param name="Id">Opaque provider identifier interpreted by the consuming domain.</param>
public readonly record struct MicroBundleProvider(string Id);
