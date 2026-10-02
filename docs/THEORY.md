# MicroBundle Domain Theory

## What "Micro" means

**Micro describes scope, not binary size.**

A MicroBundle is a focused capability boundary. It may contain a small implementation or a substantial one. The useful property is that the capability can be identified, versioned, configured, loaded, and composed independently.

Do not confuse this with a microservice. A MicroBundle can live entirely inside one process.

The word *bundle* is also not a claim of invention. Bundles, modules, packages, plugins, and components are established software concepts. The distinct value here is the particular contract and the separation of:

~~~text
what  -> MicroBundleDomain
where -> MicroBundleRepository
how   -> composition host
~~~

## The MicroBundle as a semantic atom

The MicroBundle is the unit by which a capability enters the FSM ecosystem.

It is intentionally larger than a primitive value and smaller than an Experience.

~~~text
Provider
   |
MicroBundle
   |
Composition
   |
Experience
   |
Digital Reality
~~~

A provider supplies something. A MicroBundle gives that capability an identity and lifecycle contract. A composition combines capabilities. An Experience manifests the resulting composition.

## Meaning belongs to the domain

The hosting ecosystem should not contain branches such as:

~~~text
if bundle is AtomicBundle ...
if bundle is MeshBundle ...
if bundle is ThermalBundle ...
~~~

That would turn the ecosystem into a collection of domain assumptions.

Instead:

~~~text
Domain package -> declares meaning
Ecosystem      -> facilitates capability
~~~

This permits a new domain to arrive without requiring the host to be rewritten.

## Code and data

The MicroBundle boundary supports both code-derived and data-driven capabilities.

A code-defined bundle can provide executable behavior. A data-defined bundle can provide structured semantic information. A composition can combine both.

The runtime should not require every semantic capability to become a new hard-coded application type.

## Dependencies are declarations

A dependency says that one capability requires another capability to be available.

It does not necessarily mean that every resource of that dependency must be loaded immediately.

That distinction supports lazy and demand-driven runtime construction.

## The Experience boundary

An Experience is a composition of MicroBundles that can be manifested and executed.

The domain layer should make it possible to express:

~~~text
Experience =
    identity
  + composition
  + configuration
  + behavior
  + lineage
~~~

without making the Experience responsible for how a browser, desktop, Unity client, or server renders it.

## Lineage as structure

When an Experience is modified and published again, its ancestry should remain representable.

~~~text
Experience C
   |
derived from
   v
Experience B
   |
derived from
   v
Experience A
~~~

This ancestry can later be manifested as recognition, attribution, or a Hall of Donors without putting presentation logic into the domain contract.

## Atomic proving ground

A useful early proving ground is the separation of related atomic capabilities.

~~~text
Atomic Core
   |
   +-- Thermal
   |
   +-- Material Physics
~~~

An Experience that needs only elemental identity should not need thermal or fracture data.

This demonstrates the larger principle:

**capabilities should be loadable independently and composed only when required.**

## Architectural invariant

A new MicroBundle domain should be able to enter the ecosystem without requiring WebPage to learn the domain's internal semantics.

That is the architectural test.
