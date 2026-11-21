namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Additive contract for a MicroBundle that declares portable identity and dependencies.
/// </summary>
/// <remarks>
/// <para>
/// The implementing object's existing <see cref="IMicroBundle.Id"/> and
/// <see cref="MicroBundleDescriptor.Id"/> remain numeric runtime identities. A platform
/// materializer may bind that identity to its own local slot, but it must do so explicitly
/// and validate the relationship; the portable ontology is never converted to a global ID.
/// </para>
/// <para>
/// Existing <see cref="IMicroBundle"/> implementations remain valid and continue to use
/// their numeric dependency declarations. A host should use this interface only when it
/// is implementing the portable composition path.
/// </para>
/// </remarks>
public interface IPortableMicroBundle : IMicroBundle
{
    /// <summary>Gets the portable ontology identity and exact revision represented by this object.</summary>
    PortableMicroBundleReference PortableIdentity { get; }

    /// <summary>Gets portable dependencies, each with its own exact revision and optional configuration.</summary>
    IReadOnlyList<PortableMicroBundleDependency> PortableDependencies { get; }
}
