namespace TheSingularityWorkshop.MicroBundleDomain;

/// <summary>
/// Editor-time definition of a MicroBundle. The definition contains the semantic
/// identity plus the schema needed for tooling to inspect and edit its configurable data.
/// </summary>
public sealed class MicroBundleDefinition
{
    private readonly IReadOnlyList<MicroBundleField> _fields;

    /// <summary>
    /// Initializes a new editor-time MicroBundle definition.
    /// </summary>
    /// <param name="name">Human-readable name of the MicroBundle.</param>
    /// <param name="descriptor">Runtime identity and capability descriptor for the MicroBundle.</param>
    /// <param name="fields">Optional recursively inspectable configuration fields.</param>
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

    /// <summary>
    /// Gets the human-readable MicroBundle name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the runtime identity and capability descriptor.
    /// </summary>
    public MicroBundleDescriptor Descriptor { get; }

    /// <summary>
    /// Gets the MicroBundle identity.
    /// </summary>
    public ulong Id => Descriptor.Id;

    /// <summary>
    /// Gets the MicroBundle version.
    /// </summary>
    public string Version => Descriptor.Version;

    /// <summary>
    /// Gets the recursively inspectable configuration fields.
    /// </summary>
    public IReadOnlyList<MicroBundleField> Fields => _fields;

    private static IReadOnlyList<MicroBundleField> MaterializeFields(
        IEnumerable<MicroBundleField>? fields)
    {
        var values = (fields ?? []).ToArray();

        if (values.Any(x => x is null || string.IsNullOrWhiteSpace(x.Name)))
            throw new ArgumentException("A MicroBundle field requires a non-null name.", nameof(fields));

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

    /// <summary>
    /// Initializes a new editable MicroBundle field.
    /// </summary>
    /// <param name="name">Field name.</param>
    /// <param name="kind">Semantic value/control category.</param>
    /// <param name="defaultValue">Optional default value for the field.</param>
    /// <param name="minimum">Optional inclusive numeric minimum.</param>
    /// <param name="maximum">Optional inclusive numeric maximum.</param>
    /// <param name="children">Optional recursively nested fields.</param>
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

    /// <summary>
    /// Gets the field name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the semantic field/control category.
    /// </summary>
    public MicroBundleFieldKind Kind { get; }

    /// <summary>
    /// Gets the optional default value.
    /// </summary>
    public object? DefaultValue { get; }

    /// <summary>
    /// Gets the optional inclusive numeric minimum.
    /// </summary>
    public double? Minimum { get; }

    /// <summary>
    /// Gets the optional inclusive numeric maximum.
    /// </summary>
    public double? Maximum { get; }

    /// <summary>
    /// Gets recursively nested child fields.
    /// </summary>
    public IReadOnlyList<MicroBundleField> Children => _children;

    private static IReadOnlyList<MicroBundleField> MaterializeChildren(
        IEnumerable<MicroBundleField>? children)
    {
        var values = (children ?? []).ToArray();

        if (values.Any(x => x is null || string.IsNullOrWhiteSpace(x.Name)))
            throw new ArgumentException("A nested MicroBundle field requires a non-null name.", nameof(children));

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
