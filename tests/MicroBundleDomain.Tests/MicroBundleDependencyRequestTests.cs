using TheSingularityWorkshop.MicroBundleDomain;
using Xunit;

namespace MicroBundleDomain.Tests;

public sealed class MicroBundleDependencyRequestTests
{
    [Fact]
    public void Pinned_request_keeps_version_separate_from_configuration()
    {
        var configuration = new byte[] { 1, 2, 3 };

        var request = MicroBundleDependencyRequest.Pinned(42, "1.2.3", configuration);

        Assert.Equal(42UL, request.BundleId);
        Assert.Equal("1.2.3", request.Version);
        Assert.True(request.IsPinned);
        Assert.Equal(configuration, request.Configuration.ToArray());
    }

    [Fact]
    public void Unconfigured_exact_version_request_is_pinned()
    {
        var request = MicroBundleDependencyRequest.Unconfigured(42, "1.2.3");

        Assert.True(request.IsPinned);
        Assert.Equal("1.2.3", request.Version);
        Assert.Empty(request.Configuration.ToArray());
    }

    [Fact]
    public void Pinned_request_rejects_invalid_identity_or_version()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            MicroBundleDependencyRequest.Pinned(0, "1.0.0"));
        Assert.Throws<ArgumentException>(() =>
            MicroBundleDependencyRequest.Pinned(42, " "));
    }

    [Fact]
    public void Legacy_request_remains_explicitly_unpinned()
    {
#pragma warning disable CS0618
        var request = MicroBundleDependencyRequest.Unconfigured(42);
#pragma warning restore CS0618

        Assert.False(request.IsPinned);
        Assert.Null(request.Version);
    }
}
