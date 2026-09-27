using TheSingularityWorkshop.MicroBundleDomain;
using Xunit;

namespace MicroBundleDomain.Tests;

public sealed class MicroBundleDescriptorTests
{
    [Fact]
    public void Descriptor_preserves_identity_and_version()
    {
        var descriptor = new MicroBundleDescriptor(42, "1.2.3");

        Assert.Equal(42UL, descriptor.Id);
        Assert.Equal("1.2.3", descriptor.Version);
    }

    [Fact]
    public void Descriptor_preserves_declared_dependencies()
    {
        var descriptor = new MicroBundleDescriptor(
            42,
            "1.0.0",
            new[]
            {
                new MicroBundleDependency(7),
                new MicroBundleDependency(11)
            });

        Assert.Equal(new ulong[] { 7, 11 }, descriptor.Dependencies.Select(x => x.BundleId));
    }

    [Fact]
    public void Descriptor_preserves_declared_providers_without_interpreting_them()
    {
        var descriptor = new MicroBundleDescriptor(
            42,
            "1.0.0",
            providers: new[]
            {
                new MicroBundleProvider("preview"),
                new MicroBundleProvider("mesh")
            });

        Assert.Equal(new[] { "preview", "mesh" }, descriptor.Providers.Select(x => x.Id));
    }

    [Fact]
    public void Descriptor_does_not_require_optional_capabilities()
    {
        var descriptor = new MicroBundleDescriptor(42, "1.0.0");

        Assert.Empty(descriptor.Dependencies);
        Assert.Empty(descriptor.Providers);
    }

    [Fact]
    public void Descriptor_rejects_zero_identity()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MicroBundleDescriptor(0, "1.0.0"));
    }

    [Fact]
    public void Descriptor_rejects_missing_version()
    {
        Assert.Throws<ArgumentException>(() => new MicroBundleDescriptor(42, " "));
    }

    [Fact]
    public void Descriptor_rejects_duplicate_dependencies()
    {
        Assert.Throws<ArgumentException>(() => new MicroBundleDescriptor(
            42,
            "1.0.0",
            new[]
            {
                new MicroBundleDependency(7),
                new MicroBundleDependency(7)
            }));
    }

    [Fact]
    public void Descriptor_rejects_zero_dependency_identity()
    {
        Assert.Throws<ArgumentException>(() => new MicroBundleDescriptor(
            42,
            "1.0.0",
            new[] { new MicroBundleDependency(0) }));
    }

    [Fact]
    public void Descriptor_rejects_duplicate_provider_ids()
    {
        Assert.Throws<ArgumentException>(() => new MicroBundleDescriptor(
            42,
            "1.0.0",
            providers: new[]
            {
                new MicroBundleProvider("preview"),
                new MicroBundleProvider("preview")
            }));
    }

    [Fact]
    public void Descriptor_rejects_provider_without_identity()
    {
        Assert.Throws<ArgumentException>(() => new MicroBundleDescriptor(
            42,
            "1.0.0",
            providers: new[] { new MicroBundleProvider(" ") }));
    }
}
