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
/// <see cref="PortableDependencies"/> is the authoritative dependency declaration for the
/// portable composition path. The host resolves that complete portable closure first, then
/// maps every reference to a local numeric runtime identity. The materialized object's
/// <see cref="MicroBundleDescriptor.Dependencies"/> and
/// <see cref="IMicroBundle.Dependencies"/> must each describe that same mapped dependency
/// set. Runtime requests must preserve the corresponding configuration bytes and exact
/// version pins. The Domain does not perform or enforce this projection/parity check.
/// </para>
/// <para>
/// Existing <see cref="IMicroBundle"/> implementations remain valid and continue to use
/// their numeric dependency declarations. A host should use this interface only when it
/// is implementing the portable composition path; it must fail closed if the mapped
/// descriptor declarations and runtime requests disagree.
/// </para>
/// </remarks>
public interface IPortableMicroBundle : IMicroBundle
{
    /// <summary>Gets the portable ontology identity and exact revision represented by this object.</summary>
    PortableMicroBundleReference PortableIdentity { get; }

    /// <summary>
    /// Gets the authoritative portable dependencies for portable composition, each with
    /// its own exact revision and optional request configuration.
    /// </summary>
    IReadOnlyList<PortableMicroBundleDependency> PortableDependencies { get; }
}
