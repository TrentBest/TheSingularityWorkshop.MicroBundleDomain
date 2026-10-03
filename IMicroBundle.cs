namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Executable contract for a loadable MicroBundle capability.
/// </summary>
public interface IMicroBundle
{
    /// <summary>
    /// Gets the domain-owned identity and capability descriptor.
    /// </summary>
    MicroBundleDescriptor Descriptor { get; }

    /// <summary>
    /// Gets the stable MicroBundle identity.
    /// </summary>
    ulong Id => Descriptor.Id;

    /// <summary>
    /// Gets the MicroBundles required before this capability can load.
    /// </summary>
    IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; }

    /// <summary>
    /// Installs the capability into a host-provided composition context.
    /// </summary>
    void Load(IMicroBundleLoadContext context);

    /// <summary>
    /// Participates in host-driven composition arbitration.
    /// </summary>
    bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex);
}
