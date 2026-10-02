namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Describes a MicroBundle capability without imposing meaning on the hosting ecosystem.
/// </summary>
public sealed class MicroBundleDescriptor
{
    private readonly IReadOnlyList<MicroBundleDependency> _dependencies;
    private readonly IReadOnlyList<MicroBundleProvider> _providers;

    /// <summary>Creates a descriptor for a MicroBundle capability.</summary>
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

    /// <summary>Gets the stable MicroBundle identity.</summary>
    public ulong Id { get; }

    /// <summary>Gets the declared MicroBundle version.</summary>
    public string Version { get; }

    /// <summary>Gets the declared MicroBundle dependencies.</summary>
    public IReadOnlyList<MicroBundleDependency> Dependencies => _dependencies;

    /// <summary>Gets the providers exposed by the MicroBundle.</summary>
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
public readonly record struct MicroBundleDependency(ulong BundleId);

/// <summary>
/// Identifies a provider exposed by a MicroBundle.
/// </summary>
public readonly record struct MicroBundleProvider(string Id);
