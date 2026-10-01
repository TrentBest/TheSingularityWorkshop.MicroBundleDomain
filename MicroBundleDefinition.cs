namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Editor-time definition of a MicroBundle. The definition contains the semantic
/// identity plus the schema needed for tooling to inspect and edit its configurable data.
/// </summary>
public sealed class MicroBundleDefinition
{
    private readonly IReadOnlyList<MicroBundleField> _fields;

    public MicroBundleDefinition(
        string name,
        MicroBundleDescriptor descriptor,
        IEnumerable<MicroBundleField>? fields = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("A MicroBundle name is required.", nameof(name));

        ArgumentNullException.ThrowIfNull(descriptor);

        _fields = MaterializeFields(fields);

        Name = name;
        Descriptor = descriptor;
    }

    public string Name { get; }

    public MicroBundleDescriptor Descriptor { get; }

    public ulong Id => Descriptor.Id;

    public string Version => Descriptor.Version;

    public IReadOnlyList<MicroBundleField> Fields => _fields;

    private static IReadOnlyList<MicroBundleField> MaterializeFields(
        IEnumerable<MicroBundleField>? fields)
    {
        var values = (fields ?? []).ToArray();

        if (values.Any(x => string.IsNullOrWhiteSpace(x.Name)))
            throw new ArgumentException("A MicroBundle field requires a name.", nameof(fields));

        if (values.Select(x => x.Name).Distinct(StringComparer.Ordinal).Count() != values.Length)
            throw new ArgumentException("A MicroBundle cannot declare the same field more than once.", nameof(fields));

        return Array.AsReadOnly(values);
    }
}

/// <summary>
/// Describes one editable value in a MicroBundle definition.
/// Nested fields make the schema recursively inspectable by editor tooling.
/// </summary>
public sealed class MicroBundleField
{
    private readonly IReadOnlyList<MicroBundleField> _children;

    public MicroBundleField(
        string name,
        MicroBundleFieldKind kind,
        object? defaultValue = null,
        double? minimum = null,
        double? maximum = null,
        IEnumerable<MicroBundleField>? children = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("A MicroBundle field name is required.", nameof(name));

        if (minimum.HasValue && maximum.HasValue && minimum > maximum)
            throw new ArgumentException("A field minimum cannot exceed its maximum.", nameof(minimum));

        if (kind is MicroBundleFieldKind.Integer or MicroBundleFieldKind.Float &&
            (minimum.HasValue ^ maximum.HasValue))
            throw new ArgumentException(
                "Numeric fields must provide both minimum and maximum when either is provided.",
                nameof(minimum));

        _children = MaterializeChildren(children);

        Name = name;
        Kind = kind;
        DefaultValue = defaultValue;
        Minimum = minimum;
        Maximum = maximum;
    }

    public string Name { get; }

    public MicroBundleFieldKind Kind { get; }

    public object? DefaultValue { get; }

    public double? Minimum { get; }

    public double? Maximum { get; }

    public IReadOnlyList<MicroBundleField> Children => _children;

    private static IReadOnlyList<MicroBundleField> MaterializeChildren(
        IEnumerable<MicroBundleField>? children)
    {
        var values = (children ?? []).ToArray();

        if (values.Any(x => string.IsNullOrWhiteSpace(x.Name)))
            throw new ArgumentException("A nested MicroBundle field requires a name.", nameof(children));

        if (values.Select(x => x.Name).Distinct(StringComparer.Ordinal).Count() != values.Length)
            throw new ArgumentException("A MicroBundle object cannot declare the same child field more than once.", nameof(children));

        return Array.AsReadOnly(values);
    }
}

/// <summary>
/// Semantic editor control categories. GUI adapters decide how each category is manifested.
/// </summary>
public enum MicroBundleFieldKind
{
    String,
    Integer,
    Float,
    Boolean,
    Object
}
