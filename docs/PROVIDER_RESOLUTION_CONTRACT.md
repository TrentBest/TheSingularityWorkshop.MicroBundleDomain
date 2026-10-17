# Provider Resolution: Composition Boundary Contract

**Status:** architecture proposal; no provider registry or typed provider invocation is implemented by this document.

## The distinction

A MicroBundle can declare provider identifiers in `MicroBundleDescriptor.Providers`. Those identifiers are metadata. They do not, by themselves, locate a loaded object, establish a CLR interface, prove semantic compatibility, or invoke a capability.

The existing contracts deliberately separate three concerns:

| Concern | Owner |
|---|---|
| Declare a bundle identity, version, dependency IDs, and provider IDs | MicroBundleDomain |
| Find and materialize artifacts | MicroBundleRepository / host delivery layer |
| Resolve loaded providers, check compatibility, and invoke capabilities | Composition host (for example FSM_COS) or a separately authored integration bundle |

Do not turn MicroBundleDomain into a service locator. Its job is to describe the neutral capability boundary, not to prescribe how every host wires arbitrary domain-specific services.

## What exists today

- `MicroBundleDescriptor` declares bundle ID/version, dependency bundle IDs, and provider IDs.
- `IMicroBundle.Load(IMicroBundleLoadContext)` installs a capability into a host-provided context.
- `IMicroBundle.Arbitrate(IMicroBundleArbitrationContext, int)` lets a capability respond to the current composition.
- `IMicroBundleArbitrationContext.Bundles` exposes participating bundle instances, and `ExperienceContext` is opaque.

These contracts do **not** currently define a typed provider-resolution operation, provider contract IDs or versions, unit/timebase compatibility, evidence freshness, or an invocation result. A host must not imply those guarantees merely because matching provider IDs exist.

## Proposed host-side resolution flow

A composition host can implement the following without adding Physics or Gestures references to MicroBundleDomain:

1. Collect provider declarations from loaded bundle descriptors.
2. Resolve a requested provider ID to an actual loaded capability object using an explicit host-owned registry or resolver.
3. Require the caller to name the expected semantic contract and supported contract version.
4. Validate required units, time provenance, and freshness in the domain adapter that understands those semantics.
5. Return an explicit resolution outcome: resolved, missing, ambiguous, incompatible, or stale/invalid evidence. Never silently convert failure to success.
6. Invoke the typed contract through the adapter and preserve the distinction between evidence, assessment, intent, and outcome.

Provider identity is a lookup key, not a substitute for type or semantic validation. If multiple providers are allowed, selection policy must be explicit and deterministic rather than dependent on incidental bundle enumeration order.

## Physics ↔ Gestures example

For a contact-recovery experience:

- Physics publishes contact evidence in its own domain contract.
- A contact/load assessor interprets that evidence when the required physical model and context are available.
- An integration adapter maps a compatible assessment into the Gestures contact/recovery contract.
- Gestures recommends a next intent; Physics evaluates the next physical attempt.

Raw contact presence is not proof of support or slipping. Missing, stale, incompatible, or inconclusive assessment must remain unknown and lead to an explicit host policy such as awaiting new evidence.

The integration belongs at the composition/experience boundary or in a separately authored integration MicroBundle. Physics must not reference Gestures, Gestures must not reference Physics, and neither should depend on FSM_COS merely to be usable.

## Cross-bundle physical composition: material arbitration example

A higher-level experience may require a composite physical object—for example, a sword with a steel blade, copper guard, and wood grip. This is an **application-level MicroBundle arbitration workflow**. It belongs to the composition host and participating MicroBundles, not to the Physics NuGet package or this neutral contract package.

The host-side workflow is:

1. Discover the installed bundles and the material, property, model, and solver capabilities they explicitly expose.
2. Gather their declared assembly/part relationships and material evidence through supported contracts; do not inspect private implementation fields.
3. Invoke the appropriate domain capability to resolve material identity per part and assess which property evidence belongs to that material.
4. Match requested model capabilities against the evidence and declared conditions; keep missing, ambiguous, stale, or incompatible results explicit.
5. Resolve and validate typed model/solver providers at the host boundary, then assemble the requested experience from compatible capabilities.
6. When relevant bundles or evidence are added, removed, or versioned, re-arbitrate the affected composition deterministically and retain provenance for each decision.

The Physics repository currently supplies narrow domain evaluators for caller-provided evidence (assembly validation, material identity arbitration, property mapping, range assessment, and property-prerequisite checks). Those evaluators do not discover installed bundles, resolve provider IDs, own arbitration rounds, manage lifecycle changes, or assemble the final application. The host coordinates the workflow and invokes domain contracts through typed adapters.

This example does not require MicroBundleDomain to reference Physics. Provider identifiers remain neutral metadata; the host or an integration MicroBundle resolves them to actual typed contracts. See the [Physics composition-level discovery boundary](https://github.com/TrentBest/TheSingularityWorkshop.Physics/blob/development/docs/COMPOSABLE_PHYSICS_MODELS_SOLVERS_EXPERIENCES.md#composition-level-material-discovery-and-physics-mapping) for the current domain-evaluator boundary.

## Minimum executable proof before standardizing an API

Implement the resolver at the actual composition owner only after confirming its existing lifecycle and extension points. Cover at least:

- one provider resolved by stable ID and typed contract;
- missing and ambiguous provider IDs;
- incompatible contract versions;
- absent optional force/impulse values;
- stale or mismatched time provenance;
- deterministic resolution independent of bundle ordering;
- no conversion of unknown assessment into `Supported` or `Slipping`;
- clean operation when no optional assessment provider is installed.

Keep the first proof small and executable. Add a generic registry or new domain API only if the test demonstrates that the existing host seam cannot support it.

## Related architecture

- [Architecture](ARCHITECTURE.md)
- [Adventure 6 — Arbitration](adventures/06-arbitration.md)
- [Physics ↔ Gestures integration contract in Experiences](https://github.com/TrentBest/TheSingularityWorkshop.Experiences/blob/development/docs/PHYSICS_GESTURES_INTEGRATION.md)
- [Elements MicroBundle: Periodic Table Showcase](ELEMENTS_MICROBUNDLE_SHOWCASE.md)
- [Strategic Change Requests](STRATEGIC_CHANGE_REQUESTS.md)
