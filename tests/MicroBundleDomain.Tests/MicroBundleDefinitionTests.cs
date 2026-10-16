using TheSingularityWorkshop.MicroBundleDomain;
using Xunit;

namespace MicroBundleDomain.Tests;

public sealed class MicroBundleDefinitionTests
{
    [Fact]
    public void Definition_ExposesDescriptorAndFields()
    {
        var descriptor = new MicroBundleDescriptor(42, "1.0.0");
        var definition = new MicroBundleDefinition(
            "Hydrogen",
            descriptor,
            [
                new MicroBundleField("AtomicNumber", MicroBundleFieldKind.Integer, 1, 1, 118),
                new MicroBundleField("Symbol", MicroBundleFieldKind.String, "H")
            ]);

        Assert.Equal("Hydrogen", definition.Name);
        Assert.Equal((ulong)42, definition.Id);
        Assert.Equal("1.0.0", definition.Version);
        Assert.Equal(2, definition.Fields.Count);
        Assert.Equal(1, definition.Fields[0].Minimum);
        Assert.Equal(118, definition.Fields[0].Maximum);
        Assert.False(definition.HasStableParameterIds);
    }

    [Fact]
    public void Definition_SupportsRecursiveFields()
    {
        var definition = new MicroBundleDefinition(
            "Matter",
            new MicroBundleDescriptor(42, "1.0.0"),
            [
                new MicroBundleField(
                    "Physical",
                    MicroBundleFieldKind.Object,
                    children:
                    [
                        new MicroBundleField("Density", MicroBundleFieldKind.Float, 1.0, 0, 1000),
                        new MicroBundleField("Label", MicroBundleFieldKind.String, "Hydrogen")
                    ])
            ]);

        Assert.Equal(2, definition.Fields[0].Children.Count);
        Assert.Equal("Density", definition.Fields[0].Children[0].Name);
    }

    [Fact]
    public void Definition_RejectsDuplicateFieldNames()
    {
        Assert.Throws<ArgumentException>(() =>
            new MicroBundleDefinition(
                "Hydrogen",
                new MicroBundleDescriptor(42, "1.0.0"),
                [
                    new MicroBundleField("Value", MicroBundleFieldKind.Integer),
                    new MicroBundleField("Value", MicroBundleFieldKind.Float)
                ]));
    }

    [Fact]
    public void FluentBuilder_AssignsExplicitStableIdsAndPreservesThemAcrossRenames()
    {
        var original = new MicroBundleFieldBuilder("display-name", MicroBundleFieldKind.Integer)
            .WithId("com.example.physics.mass")
            .WithDefaultValue(10)
            .WithRange(0, 100)
            .Build();

        var renamed = new MicroBundleFieldBuilder("Mass", MicroBundleFieldKind.Integer)
            .WithId("com.example.physics.mass")
            .WithDefaultValue(10)
            .WithRange(0, 100)
            .Build();

        Assert.Equal("com.example.physics.mass", original.ParameterId);
        Assert.Equal(original.ParameterId, renamed.ParameterId);
        Assert.NotEqual(original.Name, renamed.Name);
    }

    [Fact]
    public void Definition_ReportsCompleteStableIdentityAcrossNestedFields()
    {
        var definition = new MicroBundleDefinition(
            "Matter",
            new MicroBundleDescriptor(42, "1.0.0"),
            [
                new MicroBundleFieldBuilder("Physical", MicroBundleFieldKind.Object)
                    .WithId("example.matter.physical")
                    .AddChild(
                        new MicroBundleFieldBuilder("Density", MicroBundleFieldKind.Float)
                            .WithId("example.matter.density")
                            .WithDefaultValue(1.0)
                            .WithRange(0, 1000))
                    .Build()
            ]);

        Assert.True(definition.HasStableParameterIds);
    }

    [Fact]
    public void Definition_RejectsDuplicateParameterIdsAcrossNestedFields()
    {
        var fields = new[]
        {
            new MicroBundleFieldBuilder("Physical", MicroBundleFieldKind.Object)
                .WithId("example.shared")
                .AddChild(new MicroBundleFieldBuilder("Density", MicroBundleFieldKind.Float)
                    .WithId("example.shared"))
                .Build()
        };

        Assert.Throws<ArgumentException>(() =>
            new MicroBundleDefinition("Matter", new MicroBundleDescriptor(42, "1.0.0"), fields));
    }

    [Fact]
    public void FluentBuilder_RejectsRangeForNonNumericField()
    {
        Assert.Throws<InvalidOperationException>(() =>
            new MicroBundleFieldBuilder("Label", MicroBundleFieldKind.String).WithRange(0, 1));
    }

    [Fact]
    public void FluentBuilder_RejectsChildrenForNonObjectField()
    {
        Assert.Throws<InvalidOperationException>(() =>
            new MicroBundleFieldBuilder("Label", MicroBundleFieldKind.String)
                .AddChild(new MicroBundleFieldBuilder("Nested", MicroBundleFieldKind.String)));
    }
}
