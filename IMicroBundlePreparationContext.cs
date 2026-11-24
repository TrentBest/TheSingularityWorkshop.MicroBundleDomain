namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Host-neutral, read-only capability access supplied during composition preparation.
/// Preparation runs after the dependency closure is resolved and before any bundle is loaded.
/// </summary>
public interface IMicroBundlePreparationContext
{
    /// <summary>Gets the runtime identity being assembled.</summary>
    ulong RuntimeId { get; }

    /// <summary>
    /// Attempts to resolve a typed handle for an exact provider declared by a participating bundle.
    /// Returns false when the provider is not declared, its version does not match, or the host
    /// has no resolver for it.
    /// </summary>
    bool TryGetProvider<TProvider>(
        MicroBundleProviderReference reference,
        out TProvider? provider)
        where TProvider : class;
}
