# Portable dependency contract

## Purpose

The Domain contract now has an additive representation for portable MicroBundle identity and dependency declarations. It does not replace the existing numeric runtime API.

A portable reference is:

```text
(canonical ontology string, exact version, optional SHA-256)
```

For example:

```csharp
var renderer = new PortableMicroBundleReference(
    "TheSingularityWorkshop.Renderer",
    "1.2.0",
    "0123456789abcdef0123456789abcdef0123456789abcdef0123456789abcdef");
```

The ontology string identifies the capability across repositories and machines. Version selects the immutable revision. SHA-256, when present, pins exact bytes. A digest verifies bytes; it does not authenticate a publisher.

## Portable dependencies

An `IPortableMicroBundle` adds two declarations to an existing `IMicroBundle`:

- `PortableIdentity`: the portable identity/revision represented by this materialized object.
- `PortableDependencies`: portable references to exact dependency revisions, with optional request configuration.

Configuration is carried beside the reference, not inside it. It does not affect artifact identity.

## Numeric compatibility remains

The existing `IMicroBundle.Id`, `MicroBundleDescriptor.Id`, `MicroBundleDependency`, and `MicroBundleDependencyRequest` contracts remain unchanged. Existing implementations and numeric manifests continue to work.

For the portable path, the host/FSM_COS materializer must explicitly bind the portable reference to a platform-local runtime slot and materialize an object whose numeric runtime identity is reconciled with that slot. This Domain contract does not allocate slots, persist mappings, retrieve bytes, verify a repository response, or compose a dependency graph.

Never use a repository storage handle as a runtime ID. Never hash an ontology string into an invented global ID. Never serialize a platform-local slot into an Experience manifest.

## Validation boundaries

The value object rejects empty/non-canonical ontology strings, unsafe path/query characters, non-normalized Unicode, missing/unsafe versions, and malformed SHA-256 values. SHA-256 is normalized to lowercase.

A missing digest remains representable because some upstream contracts may resolve an immutable version before a digest is available. A publication or runtime policy that requires byte pinning must additionally require `IsContentPinned` and verify the bytes before materialization/activation.

## Ownership

- Creator: chooses the canonical ontology and publishes immutable versions.
- Repository: maps portable revisions to private storage and returns verified bytes.
- Platform cache/FSM_COS: maps ontology to local runtime slots and composes portable dependencies.
- Domain: defines portable identity/dependency value shapes and preserves the existing numeric runtime contract.
- Experience author: approves creator updates and publishes a new manifest version; existing manifests remain pinned.

This is a contract primitive, not end-to-end proof of repository retrieval, cache persistence, materialization, FSM_COS composition, or playability.
