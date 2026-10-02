using TheSingularityWorkshop.MicroBundleDomain;
using Xunit;

namespace MicroBundleDomain.Tests;

public sealed class MicroBundleRuntimeContractTests
{
    [Fact]
    public void MicroBundleContractUsesDomainOwnedDescriptorAndDependencyRequests()
    {
        var bundle = new TestBundle();

        Assert.Equal(42UL, bundle.Descriptor.Id);
        Assert.Equal(42UL, ((IMicroBundle)bundle).Id);
        Assert.Single(bundle.Dependencies);
        Assert.Equal(7UL, bundle.Dependencies[0].BundleId);
        Assert.Equal("test", bundle.Descriptor.Version);
    }

    [Fact]
    public void UnconfiguredDependencyRequestUsesEmptyConfiguration()
    {
        var request = MicroBundleDependencyRequest.Unconfigured(7UL);

        Assert.Equal(7UL, request.BundleId);
        Assert.True(request.Configuration.IsEmpty);
    }

    [Fact]
    public void LoadAndArbitrateReceiveDomainOwnedHostNeutralContexts()
    {
        var bundle = new TestBundle();
        var load = new TestLoadContext(99UL, 42UL);
        var arbitration = new TestArbitrationContext(99UL, bundle);

        bundle.Load(load);
        var changed = bundle.Arbitrate(arbitration, 0);

        Assert.True(load.WasRead);
        Assert.True(changed);
        Assert.Same(bundle, Assert.Single(arbitration.Bundles));
    }

    private sealed class TestBundle : IMicroBundle
    {
        public MicroBundleDescriptor Descriptor { get; } =
            new(42UL, "test", [new MicroBundleDependency(7UL)]);

        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; } =
            [new(7UL, "test"u8.ToArray())];

        public void Load(IMicroBundleLoadContext context) =>
            WasLoaded = context.TryGetConfiguration(42UL, out _);

        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex) =>
            context.RuntimeId == 99UL && context.Bundles.Count == 1 && roundIndex == 0;

        public bool WasLoaded { get; private set; }
    }

    private sealed class TestLoadContext(ulong runtimeId, ulong configuredBundleId) : IMicroBundleLoadContext
    {
        public ulong RuntimeId { get; } = runtimeId;
        public bool WasRead { get; private set; }

        public bool TryGetConfiguration(ulong bundleId, out ReadOnlyMemory<byte> configuration)
        {
            if (bundleId == configuredBundleId)
            {
                configuration = new byte[] { 1 };
                WasRead = true;
                return true;
            }

            configuration = default;
            return false;
        }
    }

    private sealed class TestArbitrationContext(ulong runtimeId, IMicroBundle bundle) : IMicroBundleArbitrationContext
    {
        public ulong RuntimeId { get; } = runtimeId;
        public IReadOnlyList<IMicroBundle> Bundles { get; } = [bundle];
        public object? ExperienceContext => null;
    }
}
