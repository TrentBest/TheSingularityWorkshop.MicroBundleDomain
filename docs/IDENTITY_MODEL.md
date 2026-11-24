# MicroBundle Identity Model

This document is the normative boundary map for portable MicroBundle identity, repository storage, and platform-local runtime indexing.

The portable contract is ontology-string based. Numeric values are implementation handles allocated inside a repository or a platform cache; neither is portable semantic identity, and the two numeric namespaces must never be conflated.

## The identity layers

| Layer | Representation | Owner | Meaning |
|---|---|---|---|
| Portable capability identity | Canonical ontology string, e.g. TheSingularityWorkshop.Renderer | Capability author / semantic model | Which capability is being named across repositories, experiences, and machines |
| Immutable artifact revision | Portable ontology + exact version + SHA-256 | Creator/repository contract | Which exact bytes are selected and verified |
| Repository storage index | Repository-private numeric handle or blob locator | MicroBundleRepository | Where this repository stores the immutable bytes |
| Platform/FSM_COS runtime index | Platform-local numeric handle/slot, e.g. ulong 10 | Platform cache / FSM_COS | Fast local lookup after resolving the portable ontology identity |
| Domain runtime object identity | Current MicroBundleDescriptor.Id / IMicroBundle.Id numeric contract | MicroBundleDomain + runtime adapter | The numeric identity exposed to existing in-process consumers; its relationship to the platform slot must be explicit at the materialization boundary |

An immutable artifact revision is:

~~~
(canonical ontology, exact version, SHA-256)
~~~

The ontology names the capability. The version expresses the selected release. The digest identifies exact bytes. The digest does not identify the publisher, prove authorization, or establish that bytes are safe to execute.

## Normative portable loading flow

1. **Creator registration.** A creator registers a capability using its canonical ontology string. MicroBundleRepository persists the ontology-to-repository-storage mapping and stores immutable artifact revisions under that repository-owned index.
2. **Experience authoring.** The Workshop editor presents the repository's MicroBundle representation. Selecting a capability downloads it by ontology string; the repository resolves its private index and returns the requested immutable revision. The platform verifies and caches the bytes locally.
3. **Manifest publication.** The experience manifest records portable ontology strings and exact versions. Where the repository artifact contract supplies an integrity digest, the manifest records that digest as well. It never serializes repository storage handles or platform-local runtime slots.
4. **FSM_COS resolution.** FSM_COS resolves each manifest ontology string through the platform's local cache/index. The resulting numeric slot is local to that platform cache and is used for fast array/index access. Before activation, FSM_COS verifies the cached revision against the manifest's exact version and any declared digest. Missing or mismatched content fails closed.
5. **Dependency composition.** Dependencies are declared portably by ontology string and exact version (plus digest when available/required). FSM_COS resolves the dependency closure through the same local cache/index and converts each portable declaration to a local runtime slot before composition.
6. **Creator updates.** A newer creator revision may trigger a notification, but it never silently changes an existing experience. The author reviews and approves the update, validates the experience, and publishes a new experience-manifest version referencing the approved revision.

## Ontology identifier is not an ontology signature

The portable ontology identifier (for example, TheSingularityWorkshop.Renderer) names a capability for artifact exchange.

Do not conflate it with the older nine-layer integer OntologySignature found in Architect / Singularity Hub. That signature is a semantic coordinate composed of nine integer layer values, with editor-facing strings resolved through a separate registry. It is not the portable artifact key, a repository storage handle, or an FSM_COS runtime slot.

The current MicroBundle contracts define no conversion between the string identifier and the nine-layer signature. Keep them distinct unless a future explicit Domain-owned contract defines and tests a relationship.

## Ownership rules

1. The capability author owns the canonical ontology identity and versioning intent.
2. MicroBundleRepository owns only its repository-private storage index and immutable byte retrieval/verification.
3. The platform cache/FSM_COS owns the local ontology-to-runtime-slot mapping, its persistence/lifecycle, and the conversion from portable manifest declarations to local fast-path handles.
4. MicroBundleDomain owns the shape and semantics of the runtime object contract. Its current numeric Id must not be treated as portable identity or silently equated with a repository handle.
5. The materialization/adapter boundary must explicitly establish the relationship between the FSM_COS local slot and the numeric identity exposed by the runtime object. If the loaded artifact's embedded numeric identity conflicts with the assigned local slot, reject it or use a deliberate adapter contract; never silently overwrite or assume equality.
6. The composition host owns dependency graph resolution and must not infer identity from display names, assembly names, paths, repository-local handles, or arbitrary hashes.
7. A content hash is an integrity check over exact bytes, not publisher authentication or trust policy.

## The two numeric indexes are different namespaces

**Repository mapping — artifact location:**

(ontology, exact version, SHA-256) → repository-private storage locator → verified bytes

This is a Repository responsibility. The storage locator may differ across repository instances and must never appear in an experience manifest or become a runtime ID.

**Platform mapping — runtime acceleration:**

canonical ontology → platform-local runtime slot

This is a platform/FSM_COS responsibility. It is local to a cache/runtime instance, may differ between machines, and exists to support efficient local lookup. Cache persistence must preserve or safely rebuild the mapping; collisions, duplicate/conflicting registrations, stale mappings, and invalidation must be handled deterministically and fail closed.

These mappings are independent. A repository handle is not a platform slot, and neither is the portable ontology identity.

## Current Domain API and compatibility boundary

The current Domain API exposes MicroBundleDescriptor.Id and IMicroBundle.Id as numeric values. MicroBundleDependency and MicroBundleDependencyRequest currently express dependencies through numeric bundle IDs; the request also carries opaque configuration bytes and supports optional exact-version pinning.

Those types describe the existing in-process/runtime contract. The development source now also provides the additive `PortableMicroBundleReference`, `PortableMicroBundleDependency`, and `IPortableMicroBundle` contracts. Portable declarations use canonical ontology + exact version (+ optional SHA-256); dependency request configuration remains separate from artifact identity. The portable interface is authoritative only for the portable composition path. FSM_COS/materialization hosts must resolve the complete portable closure, map it to platform-local runtime identities, and validate that both numeric descriptor dependencies and runtime requests are exact projections of the portable declarations. The Domain exposes the declarations but does not perform mapping, retrieval, digest verification, or parity enforcement.

The current Repository development line implements portable (ontology, exact version, SHA-256) artifact identity, a durable repository-private storage mapping, REST identity ingestion/resolution, and the publisher surface. The separate Repository FSM_COS integration adapter materializes artifacts and validates runtime identities through an explicit mapping. That is useful compatibility infrastructure, but an explicit runtimeId → repository address map is not the final authoring/manifest contract described above.

The target flow is:
- portable manifests and dependency declarations carry ontology identity + exact version (+ digest where required);
- Repository resolves portable artifact revisions to repository-private storage;
- FSM_COS resolves portable ontology identities to platform-local numeric slots;
- Domain's existing numeric runtime API remains an in-process compatibility surface until an additive adapter/API contract is implemented and tested.

No existing numeric field is silently redefined by this document.

## Fail-closed requirements

Reject or stop composition for:
- unknown ontology identities or missing artifact bytes;
- non-canonical or invalid ontology strings;
- duplicate/conflicting ontology registrations;
- repository storage-handle collisions or platform runtime-slot collisions;
- stale, ambiguous, or invalid local cache mappings;
- exact-version mismatch;
- SHA-256 mismatch or bytes that fail digest verification;
- dependency cycles or unresolved/ambiguous dependency closure where the composition contract disallows them;
- a materialized runtime object whose identity cannot be reconciled with its assigned local slot;
- an unapproved creator update attempting to alter an already-published experience.

A notification or newly available revision must never mutate an existing manifest or running experience.

## Required cross-repository proof

1. The same portable ontology/revision can be stored by two repository instances with different private storage handles and retrieved as identical verified bytes.
2. An experience manifest contains ontology strings and exact versions, never repository handles or platform slots.
3. A platform resolves the same portable manifest into its own local slots; slot values are not required to match across machines.
4. Cache restart/rebuild preserves deterministic mapping behavior or safely reconstructs it without collisions.
5. Root and transitive dependency resolution use the same portable identity/version rules.
6. Version and digest mismatches fail before materialization/activation.
7. The runtime materialization adapter proves the relationship between the local slot and the existing Domain numeric object identity; mismatch fails closed.
8. Legacy numeric manifests/catalogs continue to work through an explicit compatibility path during migration.
9. Creator updates remain opt-in and produce a new experience-manifest version.
10. Tests cover duplicate registrations, collisions, unknown identities, missing bytes, stale cache entries, concurrency, and repository/platform restart.

A unit test of a value object or repository index alone is not sufficient to claim end-to-end portable loading.

## Related contracts

- [MicroBundle architecture](ARCHITECTURE.md)
- [Stable parameter identity](STABLE_PARAMETER_IDS.md)
- [Provider resolution contract](PROVIDER_RESOLUTION_CONTRACT.md)
- [Repository identity reconciliation](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/blob/development/docs/IDENTITY_RECONCILIATION.md)
- [Repository FSM_COS integration adapter](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/tree/development/src/FSM_COS/TheSingularityWorkshop.MicroBundleRepository.FSM_COS)
- [Domain identity reconciliation issue #28](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/issues/28)
