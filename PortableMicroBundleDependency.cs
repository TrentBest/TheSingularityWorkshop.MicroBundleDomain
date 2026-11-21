namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Declares a portable dependency on an exact MicroBundle revision.
/// </summary>
/// <remarks>
/// Request configuration is deliberately separate from <see cref="PortableMicroBundleReference"/>:
/// changing configuration does not change artifact identity.
/// </remarks>
public readonly record struct PortableMicroBundleDependency
{
    public PortableMicroBundleDependency(
        PortableMicroBundleReference reference,
        ReadOnlyMemory<byte> configuration = default)
    {
        if (!reference.IsValid)
            throw new ArgumentException("A valid portable MicroBundle reference is required.", nameof(reference));

        Reference = reference;
        Configuration = configuration;
    }

    /// <summary>Gets the portable identity and exact revision required by this dependency.</summary>
    public PortableMicroBundleReference Reference { get; }

    /// <summary>Gets optional request configuration; it is not part of artifact identity.</summary>
    public ReadOnlyMemory<byte> Configuration { get; }

    /// <summary>Gets whether this dependency contains a valid portable reference.</summary>
    public bool IsValid => Reference.IsValid;
}
