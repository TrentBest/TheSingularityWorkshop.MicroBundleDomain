namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Allows a MicroBundle to export its complete effective configuration after
/// publish-time arbitration so a later runtime can restore that resolved state
/// without repeating arbitration.
/// </summary>
/// <remarks>
/// The exported bytes must be deterministic and consumable as this bundle's
/// configuration input on a subsequent load. They must include the bundle's
/// defaults and any arbitration-derived decisions, not merely the original
/// override file. Host-specific settings and secrets do not belong in this
/// payload. Bundles that cannot satisfy this contract must not be included in
/// a publication that claims arbitration has already been resolved.
/// </remarks>
public interface IMicroBundleResolvedConfiguration
{
    /// <summary>
    /// Exports the complete, deterministic configuration that reproduces this
    /// bundle's resolved state when supplied to a later load.
    /// </summary>
    ReadOnlyMemory<byte> ExportResolvedConfiguration();
}
