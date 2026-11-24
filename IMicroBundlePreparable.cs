namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Optional lifecycle contract for bundles that need typed capabilities during preparation.
/// </summary>
/// <remarks>
/// The host invokes <see cref="Prepare"/> after resolving and validating the complete dependency
/// closure, in deterministic bundle-ID order, and before invoking any bundle's <c>Load</c>.
/// Implementations should validate and retain host-provided handles only. They must not perform
/// irreversible external side effects during preparation.
/// </remarks>
public interface IMicroBundlePreparable
{
    /// <summary>Prepares this bundle against the resolved composition and host capability handles.</summary>
    void Prepare(IMicroBundlePreparationContext context);
}
