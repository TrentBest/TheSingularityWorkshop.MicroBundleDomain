namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Fluent authoring API for a configurable MicroBundle field.
/// A stable parameter ID is explicitly assigned by the capability author; it is
/// never inferred from a display name, nesting path, or array position.
/// </summary>
public sealed class MicroBundleFieldBuilder
{
    private readonly List<MicroBundleFieldBuilder> _children = [];
    private string? _parameterId;
    private object? _defaultValue;
    private double? _minimum;
    private double? _maximum;

    /// <summary>Creates a builder for a field with a human-readable name and semantic kind.</summary>
    public MicroBundleFieldBuilder(string name, MicroBundleFieldKind kind)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("A MicroBundle field name is required.", nameof(name));

        Name = name;
        Kind = kind;
    }

    /// <summary>Gets the human-readable field name.</summary>
    public string Name { get; }

    /// <summary>Gets the semantic field/control category.</summary>
    public MicroBundleFieldKind Kind { get; }

    /// <summary>
    /// Assigns the stable parameter ID authored by the capability owner.
    /// Keep this value unchanged when renaming or moving the field.
    /// </summary>
    public MicroBundleFieldBuilder WithId(string parameterId)
    {
        if (string.IsNullOrWhiteSpace(parameterId))
            throw new ArgumentException("A stable parameter ID is required.", nameof(parameterId));

        _parameterId = parameterId;
        return this;
    }

    /// <summary>Sets the field's default value.</summary>
    public MicroBundleFieldBuilder WithDefaultValue(object? value)
    {
        _defaultValue = value;
        return this;
    }

    /// <summary>Sets inclusive numeric bounds. Both bounds are required together.</summary>
    public MicroBundleFieldBuilder WithRange(double minimum, double maximum)
    {
        if (minimum > maximum)
            throw new ArgumentException("A field minimum cannot exceed its maximum.", nameof(minimum));

        if (Kind is not (MicroBundleFieldKind.Integer or MicroBundleFieldKind.Float))
            throw new InvalidOperationException("Only integer and float fields can have numeric ranges.");

        _minimum = minimum;
        _maximum = maximum;
        return this;
    }

    /// <summary>Adds a nested child field builder.</summary>
    public MicroBundleFieldBuilder AddChild(MicroBundleFieldBuilder child)
    {
        ArgumentNullException.ThrowIfNull(child);

        if (Kind != MicroBundleFieldKind.Object)
            throw new InvalidOperationException("Only object fields can contain child fields.");

        if (_children.Any(existing => StringComparer.Ordinal.Equals(existing.Name, child.Name)))
            throw new ArgumentException($"The object already contains a child named '{child.Name}'.", nameof(child));

        _children.Add(child);
        return this;
    }

    /// <summary>Builds an immutable field schema.</summary>
    public MicroBundleField Build() =>
        new(
            name: Name,
            kind: Kind,
            defaultValue: _defaultValue,
            minimum: _minimum,
            maximum: _maximum,
            children: _children.Select(child => child.Build()),
            parameterId: _parameterId);
}
