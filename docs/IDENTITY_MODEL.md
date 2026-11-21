# MicroBundle Identity Model

This document is the normative boundary map for identity. It distinguishes the identity a capability author assigns, the runtime identifier used by current composition contracts, and the private numeric handle a repository may use internally.

The distinction is required for portable artifact exchange. A number allocated by one repository must never silently become a globally meaningful capability identity.

## The three identity layers

| Layer | Current representation | Owner | Meaning |
|---|---|---|---|
| Portable semantic identity | Canonical ontology identifier | Domain author / semantic model | Which capability is being named independently of a particular repository |
| Runtime/domain identity | Non-zero `ulong` exposed by `MicroBundleDescriptor.Id` and `IMicroBundle.Id` | MicroBundleDomain and composition contract | The identifier current in-process consumers use to refer to a participating MicroBundle |
| Repository-local storage handle | Non-zero `ulong` inside a repository artifact address/binding | MicroBundleRepository implementation | A private locator allocated and persisted by one repository; it is not portable |

An immutable portable artifact revision additionally includes the exact version and SHA-256 digest:

```text
(canonical ontology, exact version, SHA-256)
```

The digest identifies bytes. It does not identify the publisher, prove authorization, or establish that the bytes are safe to execute.

## Ownership rules

1. A capability author owns its semantic identity and versioning intent.
2. MicroBundleDomain owns the runtime contract and the meaning of its current numeric `Id`.
3. A repository owns any private numeric handle and the durable mapping to a portable artifact revision.
4. A composition host owns resolution of its manifest/dependency graph and must not infer identity from display names, assembly names, paths, or repository-local handles.
5. An artifact hash is an integrity check over exact bytes. It is not a substitute for publisher authentication or trust policy.
6. The repository does not own ontology meaning, dependency semantics, arbitration, or runtime assembly.

## Mapping is an explicit boundary

A portable revision and a runtime/domain identifier are not interchangeable. When a host needs to compose a portable artifact, it must obtain a validated binding from an authorized repository/materialization boundary and verify that the returned artifact matches the requested portable revision.

The mapping contract must reject:

- an unknown portable ontology identity;
- a zero or otherwise invalid runtime identifier;
- a duplicate or conflicting binding;
- a version mismatch;
- a content-hash mismatch;
- an artifact whose bytes do not match its SHA-256;
- an ambiguous dependency resolution.

A repository-local storage handle must not escape as if it were a globally stable semantic or runtime identifier.

## Current implementation and compatibility

The current Domain API exposes `MicroBundleDescriptor.Id` as a non-zero `ulong`. `MicroBundleDependency` and `MicroBundleDependencyRequest` currently express dependencies using numeric bundle IDs; the request type also carries optional opaque configuration bytes and supports exact-version pinning.

The repository's current development contract addresses artifacts by numeric bundle ID, version, and SHA-256. A separate repository development change set proposes portable `(ontology, version, SHA-256)` identity and durable repository-local bindings. Those changes are under review in the Repository PR chain #39 → #40 → #41; they are not yet part of the canonical development branch.

Therefore this document does **not** silently reinterpret existing numeric IDs, add an unverified ontology-to-runtime mapping, or claim that portable dependency resolution is complete. Existing numeric manifests and dependencies remain on their current contract until the mapping is specified, tested, and integrated with FSM_COS and its consumers.

## Required proof before changing the public runtime contract

The complete cross-repository acceptance suite must prove:

1. A portable ontology revision maps deterministically to the intended runtime/domain identity.
2. Repository-local handles remain private and can differ between repositories.
3. Repeated ingestion of the same immutable revision is idempotent.
4. Conflicting or duplicate mappings fail closed.
5. Unknown ontology identities do not receive invented runtime IDs.
6. Version and SHA-256 mismatches are rejected.
7. Dependency closure resolves through the same explicit mapping boundary.
8. Existing numeric manifests either remain compatible or have a documented, tested migration path.
9. Mapping behavior is deterministic under concurrent writers and repository restart.
10. FSM_COS, Renderer, Visualize, AnyApp, and repository adapters agree on the same semantics.

These are cross-repository proofs. A unit test of a value object or repository index alone is not sufficient to claim end-to-end identity reconciliation.

## Related contracts

- [MicroBundle architecture](ARCHITECTURE.md)
- [Stable parameter identity](STABLE_PARAMETER_IDS.md)
- [Provider resolution contract](PROVIDER_RESOLUTION_CONTRACT.md)
- [Repository implementation map](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/blob/development/docs/DOMAIN_MAP.md)
- [Domain identity reconciliation issue #28](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/issues/28)
- [Repository portable identity PR #39](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/pull/39)
