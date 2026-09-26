# TheSingularityWorkshop.MicroBundleDomain

**Domain-side foundation for MicroBundle definitions.**

A MicroBundle is a loadable semantic capability.

This repository holds reusable domain-side concepts for describing and composing MicroBundles without making the hosting ecosystem responsible for their meaning.

## The boundary

The ecosystem must be able to host a MicroBundle without understanding its domain.

~~~text
MicroBundle
  -> identity
  -> version
  -> dependencies
  -> configuration
  -> providers
  -> behavior
~~~

A hosting layer can discover, load, cache, serialize, and compose that bundle. The domain package decides what the bundle actually means.

## Code-derived MicroBundles

A MicroBundle can originate from code.

A domain package may define a capability in C#, expose its bundle identity and dependencies, and allow FSM_COS to load it as part of a runtime assembly.

~~~text
Code defines the capability.
FSM_COS loads the capability.
Forge composes the capability.
Hosting persists and distributes the composition.
GUI manifests the capability.
~~~

No layer needs to become the others.

## Composition

MicroBundles are composable. A capability may depend on another capability, and dependency graphs are part of composition metadata.

Optional capabilities remain optional. This enables demand-driven Experiences: if an Experience does not require a capability, the runtime should not load it merely because the domain package exists.

## Providers

A MicroBundle may expose providers for resources it supplies, such as preview data, mesh data, images, behavior, or domain-specific calculations.

These are examples, not constraints imposed by the hosting ecosystem.

The ecosystem should remain blind to whether a provider represents a mesh, image, physical property, or something entirely new.

## Geometry as a compatibility surface

In Forge, physical or visual forms can eventually express compatibility.

A state shell, transition socket, or condition ingot can make it visually obvious what can connect to what.

That manifestation is a GUI concern. The underlying compatibility remains a semantic/domain concern.

## Relationship to FSM_COS

FSM_COS is the runtime composition layer. This package supplies domain-side definitions that FSM_COS can load.

~~~text
Domain MicroBundle
       |
       v
    FSM_COS
       |
       v
 RuntimeAssembly
       |
       v
    Experience
~~~

## Packaging

This repository produces a reusable NuGet package:

~~~text
TheSingularityWorkshop.MicroBundleDomain
~~~

Domain-specific MicroBundle packages may reference it as a shared foundation while remaining independently publishable.

## Current status

This repository is a foundation for the MicroBundle domain model. Concrete domain families should remain separately owned rather than being accumulated into this package.

## License

MIT. See LICENSE.txt.
