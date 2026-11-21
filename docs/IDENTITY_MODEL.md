# MicroBundle Identity Model

This document is the normative boundary map for identity. It distinguishes the identity a capability author assigns, the runtime identifier used by current composition contracts, and the private numeric handle a repository may use internally.

The distinction is required for portable artifact exchange. A number allocated by one repository must never silently become a globally meaningful capability identity.

## The three identity layers

| Layer | Current representation | Owner | Meaning |
|---|---|---|---|
| Portable semantic identity | Canonical ontology identifier; not yet a first-class type in the current Domain API | Domain author / semantic model | Which capability is being named independently of a particular repository |
| Runtime/domain identity | Non-zero `ulong` exposed by `MicroBundleDescriptor.Id` and `IMicroBundle.Id` | MicroBundleDomain and composition contract | The identifier current in-process consumers use to refer to a participating MicroBundle |
| Repository-scoped storage handle | Non-zero `ulong` in `MicroBundleArtifactAddress` | MicroBundleRepository implementation | A repository-scoped locator; it is not a portable semantic or runtime identity |

An immutable portable artifact revision additionally includes the exact version and SHA-256 digest:

```text
(canonical ontology, exact version, SHA-256)
```

The digest identifies bytes. It does not identify the publisher, prove authorization, or establish that the bytes are safe to execute.

## Ownership rules

1. A capability author owns its semantic identity and versioning intent.
2. MicroBundleDomain owns the runtime contract and the meaning of its current numeric `Id`.
3. A repository owns its repository-scoped numeric handle and the durable mapping from a portable artifact revision to that handle.
4. A composition host owns resolution of its manifest/dependency graph and must not infer identity from display names, assembly names, paths, or repository-local handles.
5. An artifact hash is an integrity check over exact bytes. It is not a substitute for publisher authentication or trust policy.
6. The repository does not own ontology meaning, dependency semantics, arbitration, or runtime assembly.

## There are two different mappings

**Mapping A — artifact location (Repository responsibility):**

`(ontology, exact version, SHA-256) → repository-scoped storage handle → verified artifact bytes`.

Repository PR #39 implemented this durable mapping in canonical Repository `development`. It does not, by itself, map a portable identity to the runtime/domain ID used by MicroBundle dependencies and FSM_COS manifests.

**Mapping B — runtime identity (cross-repository contract):**

`portable semantic identity → runtime/domain ID`.

This mapping is not yet implemented as a first-class Domain contract. It must be specified with FSM_COS and consumers before a portable artifact can safely become a runtime dependency. The host/materializer must verify the descriptor's runtime ID against the intended binding; it must not assume that the repository storage handle is that ID.

The mapping contract must reject:

- an unknown portable ontology identity;
- a zero or otherwise invalid runtime identifier;
- a duplicate or conflicting binding;
- a version mismatch;
- a content-hash mismatch;
- an artifact whose bytes do not match its SHA-256;
- an ambiguous dependency resolution.

A repository-scoped storage handle may be used within that repository's artifact API, but it must never be interpreted as a globally stable semantic or runtime identifier.

## Current implementation and compatibility

The current Domain API exposes `MicroBundleDescriptor.Id` and `IMicroBundle.Id` as the runtime/domain identity. `MicroBundleDependency` and `MicroBundleDependencyRequest` currently express dependencies using numeric runtime bundle IDs; the request type also carries optional opaque configuration bytes and supports exact-version pinning.

The Repository PR chain #39 → #40 → #41 is now merged into canonical Repository `development`. It implements portable `(ontology, exact version, SHA-256)` identity, durable repository-scoped bindings, REST identity ingestion/resolution with Workshop-only mutation authorization, and the authenticated publisher CLI. The repository's numeric artifact address is now explicitly a repository-local storage handle. The REST delivery package no longer references Domain or FSM_COS; a separate `TheSingularityWorkshop.MicroBundleRepository.FSM_COS` integration package owns runtime catalog/materialization.

That integration package accepts an explicit `runtimeId -> MicroBundleArtifactAddress` map and verifies both `IMicroBundle.Id` and `MicroBundleDescriptor.Id` after materialization. It proves that runtime IDs and repository-local storage handles can differ. It does **not** yet derive the mapping from portable semantic identity. The Domain still has no first-class ontology identity type or portable-to-runtime mapping.

Therefore this document does **not** silently reinterpret existing numeric IDs, add an unverified ontology-to-runtime mapping, or claim that portable dependency resolution is complete. Existing numeric manifests and dependencies remain on their current contract until the Domain-owned mapping is specified, tested, and integrated with FSM_COS and its consumers.

## Required proof before changing the public runtime contract

The complete cross-repository acceptance suite must prove:

1. The portable ontology identity has one canonical representation and explicit version semantics.
2. A portable revision maps deterministically to a repository-scoped storage handle; handles may differ between repositories.
3. A separate portable-to-runtime mapping resolves the intended runtime/domain identity without reusing the storage handle.
4. Repeated ingestion of the same immutable revision is idempotent.
5. Conflicting or duplicate mappings fail closed.
6. Unknown ontology identities do not receive invented runtime IDs.
7. Version and SHA-256 mismatches are rejected.
8. Dependency closure resolves through the explicit portable-to-runtime mapping.
9. Existing numeric manifests either remain compatible or have a documented, tested migration path.
10. Mapping behavior is deterministic under concurrent writers and repository restart.
11. FSM_COS, Renderer, Visualize, AnyApp, and repository adapters agree on the same semantics.

These are cross-repository proofs. A unit test of a value object or repository index alone is not sufficient to claim end-to-end identity reconciliation.

## Related contracts

- [MicroBundle architecture](ARCHITECTURE.md)
- [Stable parameter identity](STABLE_PARAMETER_IDS.md)
- [Provider resolution contract](PROVIDER_RESOLUTION_CONTRACT.md)
- [Repository implementation map](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/blob/development/docs/DOMAIN_MAP.md)
- [Domain identity reconciliation issue #28](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/issues/28)
- [Repository portable identity reconciliation](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/blob/development/docs/IDENTITY_RECONCILIATION.md)
- [Repository FSM_COS integration adapter](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/tree/development/src/FSM_COS/TheSingularityWorkshop.MicroBundleRepository.FSM_COS)
- [Repository portable identity PR #39](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/pull/39)
