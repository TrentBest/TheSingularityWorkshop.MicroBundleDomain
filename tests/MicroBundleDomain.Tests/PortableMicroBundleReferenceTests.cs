using TheSingularityWorkshop.MicroBundleDomain;
using Xunit;

namespace MicroBundleDomain.Tests;

public sealed class PortableMicroBundleReferenceTests
{
    [Fact]
    public void Reference_preserves_portable_identity_without_numeric_handles()
    {
        var reference = new PortableMicroBundleReference(
            "TheSingularityWorkshop.Visualize",
            "1.2.0");

        Assert.True(reference.IsValid);
        Assert.False(reference.IsContentPinned);
        Assert.Equal("TheSingularityWorkshop.Visualize", reference.Ontology);
        Assert.Equal("1.2.0", reference.Version);
        Assert.Null(reference.Sha256);
    }

    [Fact]
    public void Reference_normalizes_sha256_to_lowercase()
    {
        var digest = new string('A', 64);
        var reference = new PortableMicroBundleReference("workshop/renderer", "1.0.0", digest);

        Assert.True(reference.IsContentPinned);
        Assert.Equal(new string('a', 64), reference.Sha256);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("leading space")]
    [InlineData("ontology?query")]
    [InlineData("ontology#fragment")]
    [InlineData("ontology%2Fother")]
    [InlineData("/ontology")]
    [InlineData("ontology/")]
    [InlineData("ontology//other")]
    [InlineData("ontology/../other")]
    [InlineData("ontology\\other")]
    [InlineData("ontology\u0000other")]
    public void Reference_rejects_noncanonical_ontology(string ontology)
    {
        Assert.Throws<ArgumentException>(() =>
            new PortableMicroBundleReference(ontology, "1.0.0"));
    }

    [Fact]
    public void Reference_rejects_non_normalized_unicode_ontology()
    {
        Assert.Throws<ArgumentException>(() =>
            new PortableMicroBundleReference("cafe\u0301", "1.0.0"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" 1.0.0")]
    [InlineData("1/0")]
    [InlineData("1\\0")]
    [InlineData("1?query")]
    [InlineData("1#fragment")]
    [InlineData("1%2F0")]
    [InlineData("1\u0000.0")]
    public void Reference_rejects_unsafe_or_missing_version(string version)
    {
        Assert.Throws<ArgumentException>(() =>
            new PortableMicroBundleReference("workshop/renderer", version));
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-a-digest")]
    [InlineData("0123456789abcdef")]
    [InlineData("0123456789abcdef0123456789abcdef0123456789abcdef0123456789abcdeg")]
    public void Reference_rejects_malformed_sha256(string digest)
    {
        Assert.Throws<ArgumentException>(() =>
            new PortableMicroBundleReference("workshop/renderer", "1.0.0", digest));
    }

    [Fact]
    public void Reference_rejects_null_required_values()
    {
        Assert.Throws<ArgumentException>(() =>
            new PortableMicroBundleReference(null!, "1.0.0"));
        Assert.Throws<ArgumentException>(() =>
            new PortableMicroBundleReference("workshop/renderer", null!));
    }

    [Fact]
    public void Default_reference_is_invalid()
    {
        Assert.False(default(PortableMicroBundleReference).IsValid);
        Assert.False(default(PortableMicroBundleReference).IsContentPinned);
    }

    [Fact]
    public void Portable_dependency_keeps_configuration_separate_from_artifact_identity()
    {
        var reference = new PortableMicroBundleReference("workshop/renderer", "1.0.0");
        var configuration = new byte[] { 1, 2, 3 };
        var dependency = new PortableMicroBundleDependency(reference, configuration);

        Assert.True(dependency.IsValid);
        Assert.Equal(reference, dependency.Reference);
        Assert.Equal(configuration, dependency.Configuration.ToArray());
        Assert.Null(dependency.Reference.Sha256);
    }

    [Fact]
    public void Portable_dependency_rejects_default_reference()
    {
        Assert.Throws<ArgumentException>(() =>
            new PortableMicroBundleDependency(default));
        Assert.False(default(PortableMicroBundleDependency).IsValid);
    }
}
