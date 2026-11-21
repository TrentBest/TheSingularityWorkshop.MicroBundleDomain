# MicroBundle Identity and Runtime Resolution

This document defines how portable MicroBundle identity becomes a fast local runtime handle. The key distinction is simple:

**Ontology strings travel. Numeric indexes stay local.**

An experience manifest must describe what it consumes using portable ontology strings and exact versions. Repository and platform caches may each map those strings to numeric indexes for fast lookup, but those indexes belong to their respective storage/runtime layers and must never leak into a portable manifest.

## End-to-end lifecycle

### 1. Creator registration

A MicroBundle creator registers a capability using its canonical ontology string, such as `TheSingularityWorkshop.Renderer`, and publishes a version.

The MicroBundle Repository owns the mapping from the ontology/revision to its repository storage index. The repository stores the immutable artifact payload in its blob at that index.

The repository index is an implementation detail of repository storage. It is not the MicroBundle's portable identity and is not the FSM_COS runtime ID.

An immutable published revision is identified by:

```text
(canonical ontology string, exact version, SHA-256)
```

The ontology string identifies the capability. The version selects its release. The digest verifies the exact immutable bytes.

### 2. Experience authoring and immediate acquisition

A developer opens the Workshop experience/editor in their platform of choice. The editor presents the repository's diegetic representation of available MicroBundles.

When the developer selects a MicroBundle, the platform immediately requests it by ontology string (and the selected version/revision when applicable). The repository resolves that portable identifier through its own index and returns the artifact bytes. The platform verifies the artifact and places it in its local cache.

This is acquisition for authoring and execution; it does not wait until the experience is published.

### 3. Portable experience manifest

When the developer publishes the experience, the editor creates a manifest describing the MicroBundles selected for that experience.

The manifest records each MicroBundle's:
- canonical ontology string;
- exact approved version;
- integrity digest/revision information required to retrieve and verify the immutable artifact.

The manifest does **not** record:
- the repository's blob index;
- a machine's local cache index;
- an FSM_COS runtime slot/handle.

The manifest is portable between platforms and machines because it carries semantic identity and version constraints, not storage addresses.

### 4. FSM_COS local resolution and fast execution

FSM_COS reads the manifest's ontology string and resolves it through the platform's local cached mapping.

For example:

```text
"TheSingularityWorkshop.Visualize" -> local slot 10
```

The local mapping returns the compact numeric handle used by the runtime. FSM_COS can then use its existing `ulong`-based contracts and array/index dereferencing for the hot path.

Before execution, FSM_COS verifies that the locally cached artifact matches the manifest's exact approved version and integrity identity. A missing artifact, unknown ontology, incompatible version, or digest mismatch fails closed. It must not silently fetch or substitute a newer version during composition.

The runtime handle is local to the platform's mapping/cache namespace. It is not portable identity and must not be serialized into the experience manifest.

### 5. Creator updates and consumer approval

When a creator publishes a newer version, the repository/Workshop ecosystem notifies consumers that an update is available.

An existing experience is not silently upgraded. Its author opens the editor, reviews and manually approves the candidate MicroBundle version, validates that the experience still works, and then publishes a new version of the experience manifest referencing the newly approved MicroBundle version.

The old manifest continues to identify the old approved version. Notifications do not mutate a published manifest or change a running experience.

## Identity and index ownership

| Value | Example | Owner | Portable? |
|---|---|---|---|
| Canonical ontology identity | `TheSingularityWorkshop.Visualize` | Capability author / semantic contract | Yes |
| Exact MicroBundle version | `1.2.0` | Capability publisher | Yes |
| Immutable content digest | SHA-256 | Artifact/repository contract | Yes |
| Repository blob index | repository slot | MicroBundleRepository | No |
| Platform cache/runtime index | `ulong` value `10` | Platform/FSM_COS cache | No |

There are two independent index mappings:

**Repository storage mapping**

```text
(ontology string, exact version, digest)
    -> repository-local blob index
    -> immutable artifact bytes
```

**Platform runtime mapping**

```text
ontology string
    -> platform-local numeric handle (for example, 10)
    -> cached, verified MicroBundle/runtime object
```

The numeric values in these two mappings may differ. Neither mapping may borrow the other's index as identity.

## Runtime API compatibility

The current Domain API exposes `MicroBundleDescriptor.Id` and `IMicroBundle.Id` as non-zero `ulong` values. `MicroBundleDependency` and `MicroBundleDependencyRequest` also use numeric runtime IDs.

Keep that efficient runtime representation. Under this lifecycle, those numeric IDs are local runtime handles resolved from portable ontology declarations before FSM_COS executes the dependency graph. They are not repository blob indexes and are not the portable identifiers authored into manifests.

The boundary that turns a manifest ontology string into a local runtime handle belongs to the platform/FSM_COS composition path. It must:
- resolve only registered/cached ontology identities;
- maintain a consistent local mapping for the platform cache lifetime;
- ensure distinct ontology identities cannot collide;
- bind the resolved handle to the requested exact version and verified artifact;
- resolve portable dependency declarations into the corresponding local runtime handles before numeric dependency execution;
- reject unknown identities, duplicate/conflicting registrations, missing artifacts, version mismatches, digest mismatches, and ambiguous dependency resolution.

A cache rebuild may assign different local handles, provided the mapping is rebuilt consistently before composition and no persisted portable manifest depends on those handles. If a local runtime cache persists numeric handles across restarts, its mapping metadata must persist with it and be validated before reuse.

Do not hash ontology strings into arbitrary `ulong` values as a substitute for a registered mapping. Do not use the repository blob index as the runtime ID. Do not add a global numeric identity requirement where a platform-local mapping is sufficient.

Existing numeric-only manifests, if any, require an explicit compatibility adapter or migration plan. Do not silently reinterpret their values as repository indexes or ontology identifiers.

## Ontology identifier is not an ontology signature

The portable ontology identifier (for example, `TheSingularityWorkshop.Renderer`) names a capability independently of a repository.

Do not conflate it with the nine-layer integer `OntologySignature` from older Architect / Singularity Hub work. That signature is a semantic coordinate composed of nine integer layer values, with editor-facing strings resolved through a separate registry. It is not the repository artifact index, the local FSM_COS handle, or the portable MicroBundle identifier.

These concepts may be related later through an explicit contract, but they are distinct values today.

## Security and failure behavior

A SHA-256 digest verifies exact bytes; it does not authenticate a publisher or establish that an artifact is safe to execute. Publisher authorization and execution trust remain separate policy concerns.

The resolution/composition path must fail closed for:
- unknown ontology identity;
- missing local artifact/cache entry;
- missing repository artifact during acquisition;
- invalid or conflicting local mapping;
- duplicate ontology registration;
- incompatible or unapproved version;
- digest mismatch or corrupted bytes;
- unresolved dependency;
- materialized descriptor/runtime ID inconsistent with the resolved local handle.

An update notification is advisory. Only an explicitly approved and newly published experience manifest changes the version an experience consumes.

## Existing implementation boundaries

The MicroBundle Repository owns portable revision ingestion, repository-local blob indexing, immutable artifact retrieval, and byte verification.

The Domain owns the runtime object/descriptor contracts and numeric local-handle representation.

FSM_COS/platform composition owns manifest resolution, local ontology-to-runtime mapping, version verification, and conversion of portable dependency declarations into numeric runtime dependency handles.

The experience editor owns authoring-time acquisition, review/approval of creator updates, and publication of portable manifests.

AnyApp owns the host lifecycle and consumes the composed runtime assembly. Renderer and Visualize remain capability implementations delivered as MicroBundles, not NuGet capability packages.

The Repository's existing FSM_COS integration adapter accepts an explicit runtime-ID-to-artifact-address mapping and verifies materialized IDs. That is a useful adapter boundary, but the repository storage index and platform runtime handle remain separate namespaces.

## Required cross-repository proof

The aligned implementation must demonstrate:

1. Creator registration by canonical ontology string and immutable version.
2. Repository resolution from portable revision to repository-local blob index and verified bytes.
3. Immediate authoring-time acquisition by ontology string.
4. Portable manifest publication containing ontology strings, exact versions, and required integrity identity, with no local indexes.
5. Platform/FSM_COS resolution from ontology string to a local numeric handle.
6. Exact version and digest validation before composition/execution.
7. Dependency declarations resolved portably and projected to local numeric runtime handles.
8. Fast numeric runtime/array access after resolution.
9. Creator update notification without automatic upgrade.
10. Explicit author approval and a new experience-manifest version for upgrades.
11. Fail-closed behavior for missing, unknown, conflicting, corrupt, incompatible, or unapproved artifacts.
12. Compatibility or migration coverage for existing numeric-only manifests.
13. Consistent behavior across MicroBundleDomain, MicroBundleRepository, FSM_COS, Experiences, Renderer, Visualize, and AnyApp.

A repository-index unit test or a numeric runtime unit test alone is not an end-to-end proof.

## Related contracts

- [MicroBundle architecture](ARCHITECTURE.md)
- [Stable parameter identity](STABLE_PARAMETER_IDS.md)
- [Provider resolution contract](PROVIDER_RESOLUTION_CONTRACT.md)
- [Domain identity reconciliation issue #28](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/issues/28)
- [Repository portable identity reconciliation](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/blob/development/docs/IDENTITY_RECONCILIATION.md)
- [Repository FSM_COS integration adapter](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/tree/development/src/FSM_COS/TheSingularityWorkshop.MicroBundleRepository.FSM_COS)
- [Repository portable identity PR #39](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/pull/39)
