using TheSingularityWorkshop.MicroBundleDomain;
using Xunit;

namespace MicroBundleDomain.Tests;

public sealed class MicroBundleProviderContractTests
{
    [Fact]
    public void Versioned_provider_declaration_preserves_exact_contract_version()
    {
        var provider = new MicroBundleProvider("diegetic-gui", "1.2.0");
        var descriptor = new MicroBundleDescriptor(42, "3.0.0", providers: [provider]);

        Assert.Equal("diegetic-gui", descriptor.Providers[0].Id);
        Assert.Equal("1.2.0", descriptor.Providers[0].ContractVersion);
    }

    [Fact]
    public void Legacy_provider_declaration_remains_valid_but_is_not_version_pinned()
    {
        var descriptor = new MicroBundleDescriptor(
            42,
            "3.0.0",
            providers: [new MicroBundleProvider("legacy-preview")]);

        Assert.Null(descriptor.Providers[0].ContractVersion);
    }

    [Fact]
    public void Provider_reference_requires_exact_owner_and_contract_versions()
    {
        Assert.True(new MicroBundleProviderReference(42, "3.0.0", "diegetic-gui", "1.2.0").IsValid);
        Assert.False(new MicroBundleProviderReference(0, "3.0.0", "diegetic-gui", "1.2.0").IsValid);
        Assert.False(new MicroBundleProviderReference(42, "", "diegetic-gui", "1.2.0").IsValid);
        Assert.False(new MicroBundleProviderReference(42, "3.0.0", " ", "1.2.0").IsValid);
        Assert.False(new MicroBundleProviderReference(42, "3.0.0", "diegetic-gui", "").IsValid);
    }

    [Fact]
    public void Preparation_is_an_optional_domain_lifecycle_contract()
    {
        var bundle = new PreparatoryBundle();
        var context = new TestPreparationContext(99);

        bundle.Prepare(context);

        Assert.Equal(99UL, bundle.PreparedRuntimeId);
    }

    private sealed class PreparatoryBundle : IMicroBundlePreparable
    {
        public ulong PreparedRuntimeId { get; private set; }

        public void Prepare(IMicroBundlePreparationContext context) =>
            PreparedRuntimeId = context.RuntimeId;
    }

    private sealed class TestPreparationContext(ulong runtimeId) : IMicroBundlePreparationContext
    {
        public ulong RuntimeId { get; } = runtimeId;

        public bool TryGetProvider<TProvider>(
            MicroBundleProviderReference reference,
            out TProvider? provider)
            where TProvider : class
        {
            provider = null;
            return false;
        }
    }
}
