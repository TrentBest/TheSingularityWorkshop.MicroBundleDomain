using TheSingularityWorkshop.MicroBundleDomain;

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
}
