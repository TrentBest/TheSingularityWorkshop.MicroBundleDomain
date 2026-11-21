# MicroBundleDomain Contract Reference

This reference records the public contract as implemented on `development`. It separates API facts from responsibilities that remain with a composition host.

## Runtime capability

### `IMicroBundle`

A concrete capability supplies:

- `Descriptor`: its domain-owned descriptor;
- `Dependencies`: dependency requests, including optional opaque configuration bytes;
- `Load(context)`: installation into a host-provided runtime context;
- `Arbitrate(context, roundIndex)`: participation in host-driven composition arbitration.

The default `Id` implementation returns `Descriptor.Id`. The current runtime identifier is a non-zero `ulong`; it is not a portable artifact identity or a repository-local handle.

A MicroBundle does not create its own runtime, fetch its own dependencies, choose the global load order, or own the arbitration loop. Those are composition-host responsibilities.

## Descriptor and declarations

### `MicroBundleDescriptor`

The descriptor captures:

- non-zero numeric `Id`;
- non-empty `Version` string;
- optional dependency declarations;
- optional provider declarations.

The constructor snapshots dependency and provider enumerables into read-only collections. It rejects zero dependency IDs, duplicate dependency IDs, blank provider IDs, and duplicate provider IDs using ordinal comparison.

The Domain currently requires a non-empty version string; it does not parse or enforce Semantic Versioning.

### `MicroBundleDependency`

A descriptor dependency identifies another numeric bundle ID and may include an exact version. `Pinned(bundleId, version)` validates both values. The unpinned form is retained for compatibility, but it does not identify an immutable revision.

### `MicroBundleDependencyRequest`

A runtime request carries:

- numeric `BundleId`;
- opaque `ReadOnlyMemory<byte>` configuration;
- optional exact `Version`.

Use `Pinned(bundleId, version, configuration)` or `Unconfigured(bundleId, version)` for deterministic exact-version requests. The legacy one-argument `Unconfigured(bundleId)` overload is obsolete and does not pin an artifact version.

The Domain does not decode the configuration bytes. Their schema and serialization belong to the capability and its integration contract.

Descriptor dependencies and runtime dependency requests are related declarations, but they are different types: the descriptor records dependency identity/version metadata, while the runtime request can also carry configuration. The Domain does not currently enforce parity between those two collections. Hosts and bundle authors must not assume that parity is automatically validated.

## Host contexts

### `IMicroBundleLoadContext`

Exposes the runtime ID and a lookup for configuration associated with a bundle ID. It does not prescribe a serializer or expose storage/transport APIs.

### `IMicroBundleArbitrationContext`

Exposes the runtime ID, the bundles participating in the current composition, and an optional opaque `ExperienceContext`. A concrete host may provide a richer implementation, but host-specific types do not belong in this package.

The host controls arbitration rounds, termination/convergence policy, and failure handling. A MicroBundle returns whether its own arbitration step changed composition according to the host's documented convention.

## Editor and authoring description

### `MicroBundleDefinition`

A definition provides a human-readable name, a descriptor, and a recursively inspectable field collection. It is a schema/authoring surface, not executable behavior and not a GUI model.

Top-level field names must be non-empty and unique using ordinal comparison. Explicit parameter IDs, when present, must be unique across the complete nested definition.

`HasStableParameterIds` is true only when every field in the definition has a non-null parameter ID. An empty field collection is therefore complete by definition.

### `MicroBundleField` and `MicroBundleFieldKind`

The semantic field kinds are:

- `String`
- `Integer`
- `Float`
- `Boolean`
- `Object`

Fields can carry an optional default value, inclusive numeric bounds, nested children, and an optional explicit parameter ID. Numeric bounds must be supplied as a pair and the minimum cannot exceed the maximum. The base field constructor preserves compatibility with existing field construction; consumers should use the builder for authoring-time checks.

The `DefaultValue` property is typed as `object?`. The Domain does not define a wire format, deep-copy mutable objects, or guarantee that arbitrary default values can be serialized. Tooling/serialization layers must validate and encode values according to their own explicit schema.

### `MicroBundleFieldBuilder`

The fluent builder supports:

- `WithId`: assign an author-owned stable parameter ID;
- `WithDefaultValue`: supply a default;
- `WithRange`: set both inclusive numeric bounds for Integer/Float fields;
- `AddChild`: add a uniquely named child to an Object field;
- `Build`: create the field schema.

Stable IDs must not be derived from names, paths, array positions, random load-time values, or artifact hashes. See [Stable Parameter Identity](STABLE_PARAMETER_IDS.md).

## Explicitly outside this contract

MicroBundleDomain does not implement:

- portable ontology-to-runtime identity mapping;
- repository-local artifact addresses or storage;
- artifact hashing, signatures, publisher authentication, or trust policy;
- dependency graph traversal, ordering, or version conflict resolution;
- provider lookup or typed invocation;
- serialization, schema migration, or configuration override resolution;
- arbitration convergence, scheduling, execution, or RuntimeAssembly construction;
- Experience publication or host activation.

Those boundaries must be implemented by their owning layers and tested at integration seams. See [Identity Model](IDENTITY_MODEL.md), [Provider Resolution Contract](PROVIDER_RESOLUTION_CONTRACT.md), and [the Repository domain map](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/blob/development/docs/DOMAIN_MAP.md).

## API evolution rule

Before adding a public type or member, identify the consumer and the invariant it enables. Prefer an additive contract with tests over host-specific convenience APIs. Do not move repository, composition, serialization, scheduling, GUI, or application policy into the neutral Domain package merely because those systems consume MicroBundles.
