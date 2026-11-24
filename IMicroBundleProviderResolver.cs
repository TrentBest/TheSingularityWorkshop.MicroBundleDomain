namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Resolves a typed provider handle for an exact, declared MicroBundle provider.
/// Implementations belong to the host; the domain contract does not prescribe a service container.
/// </summary>
public interface IMicroBundleProviderResolver
{
    /// <summary>
    /// Attempts to resolve the requested provider as the requested capability type.
    /// The reference pins the owning bundle identity/version and provider contract version.
    /// </summary>
    bool TryResolve<TProvider>(
        MicroBundleProviderReference reference,
        out TProvider? provider)
        where TProvider : class;
}
