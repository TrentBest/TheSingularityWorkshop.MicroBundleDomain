# Stable Parameter Identity

A configurable field has two different concepts:

- **Name**: a human-readable label that may change for clarity or localization.
- **Parameter ID**: an explicit identity assigned by the capability author and preserved when the field is renamed or moved.

Names and nested paths are useful for display and navigation. They are not durable keys for saved configuration. A path such as `Physical.Density` changes when a field is reorganized; an array index changes when siblings are reordered.

## Assign IDs when authoring the schema

Use the fluent field builder to assign a stable ID:

~~~csharp
using TheSingularityWorkshop.MicroBundleDomain;

var density = new MicroBundleFieldBuilder(
        "Density",
        MicroBundleFieldKind.Float)
    .WithId("com.example.matter.density")
    .WithDefaultValue(1.0)
    .WithRange(0, 1000)
    .Build();

var physical = new MicroBundleFieldBuilder(
        "Physical",
        MicroBundleFieldKind.Object)
    .WithId("com.example.matter.physical")
    .AddChild(
        new MicroBundleFieldBuilder("Density", MicroBundleFieldKind.Float)
            .WithId("com.example.matter.density")
            .WithDefaultValue(1.0)
            .WithRange(0, 1000))
    .Build();
~~~

IDs are authored data, not generated from a field's current name. Use a namespace you control, such as a reverse-domain prefix, to reduce collisions across independently authored capabilities. Treat an assigned ID as immutable once configuration has been saved against it.

## Rules

1. Assign an ID to every field whose value must survive schema evolution.
2. Keep that ID unchanged when renaming, localizing, or moving a field.
3. Assign a new ID when a field represents a genuinely different parameter.
4. IDs must be unique across the entire definition, including nested fields.
5. Do not derive IDs from display names, nesting paths, ordering, random values generated at load time, or artifact content hashes.
6. Use `MicroBundleDefinition.HasStableParameterIds` to check whether the complete schema has explicit IDs.

Existing code that constructs `MicroBundleField` without a parameter ID remains source-compatible. Such fields have `ParameterId == null`, and the definition reports incomplete stable identity. Tooling must not silently substitute a name or path and claim it is stable.

## What this does—and does not—guarantee

MicroBundleDomain owns the field identity contract. A repository may store and deliver the resulting versioned artifact, but artifact identity (bundle ID, version, content hash) is not the identity of an individual parameter.

Stable IDs are a prerequisite for robust configuration overrides, schema migration, and portable authoring artifacts. They do not, by themselves, define how overrides are serialized, how removed fields are migrated, or how conflicting values are arbitrated. Those are separate contracts that consumers can build on top of this identity.

## Next

- [Getting Started](GETTING_STARTED.md)
- [Architecture](ARCHITECTURE.md)
- [Ecosystem Guide](ECOSYSTEM.md)
