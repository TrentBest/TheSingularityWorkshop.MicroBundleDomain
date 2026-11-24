namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Identifies one provider declaration by its owning bundle and exact contract versions.
/// </summary>
public readonly record struct MicroBundleProviderReference(
    ulong BundleId,
    string BundleVersion,
    string ProviderId,
    string ContractVersion)
{
    /// <summary>Gets whether every identity and version component is present.</summary>
    public bool IsValid =>
        BundleId != 0 &&
        !string.IsNullOrWhiteSpace(BundleVersion) &&
        !string.IsNullOrWhiteSpace(ProviderId) &&
        !string.IsNullOrWhiteSpace(ContractVersion);
}
