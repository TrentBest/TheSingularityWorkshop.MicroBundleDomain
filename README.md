# TheSingularityWorkshop.MicroBundleDomain

**Domain-side foundation for MicroBundle definitions.**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet version](https://img.shields.io/nuget/v/TheSingularityWorkshop.MicroBundleDomain?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain)
[![NuGet downloads](https://img.shields.io/nuget/dt/TheSingularityWorkshop.MicroBundleDomain?logo=nuget&style=flat-square)](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain)
[![Build Status](https://img.shields.io/github/actions/workflow/status/TrentBest/TheSingularityWorkshop.MicroBundleDomain/package.yml?branch=master&style=flat-square&logo=github)](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/actions/workflows/package.yml)
[![Last commit](https://img.shields.io/github/last-commit/TrentBest/TheSingularityWorkshop.MicroBundleDomain/master)](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/commits/master)
[![Code Coverage](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.MicroBundleDomain/graph/badge.svg)](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.MicroBundleDomain)

![Opaque MicroBundle Capability Core](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/master/docs/images/microbundle-domain-01.png)

A MicroBundle is a **loadable semantic capability**.

This repository provides the small, reusable domain-side descriptors needed to identify a MicroBundle and declare its composition surface without teaching the hosting ecosystem what the capability means.

## What belongs here

The first contract is deliberately narrow:

~~~text
MicroBundleDescriptor
    ├── identity
    ├── version
    ├── dependencies
    └── providers
~~~

The descriptor records composition facts. It does not implement a domain such as atoms, thermal properties, meshes, logic gates, or AEC.

### Dependencies

A dependency declares that another MicroBundle is part of the capability's composition.

### Providers

A provider is identified but not interpreted here.

For example, a domain package may choose to expose providers named `preview`, `mesh`, or something completely different. This package does not assign semantics to those names.

That keeps the ecosystem blind to domain meaning.

## Code-derived MicroBundles

A domain package may define executable behavior in code and expose its composition metadata through this foundation.

~~~text
Code defines the capability.
MicroBundleDomain describes the capability.
FSM_COS composes the capability.
Forge authors the composition.
Hosting persists and distributes it.
GUI manifests it.
~~~

The layers remain separate.

## Demand-driven composition

Optional capabilities remain optional.

A bundle that declares no dependencies or providers can stand alone. A richer bundle can declare only the capabilities it needs. The runtime can then resolve and load the resulting composition rather than pulling an entire domain into memory.

## Geometry as a compatibility surface

The Forge metaphor makes compatibility visible: a state shell exposes a transition surface, while a condition supplies the shape that can fit it.

![Forge geometry compatibility surface](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/master/docs/images/microbundle-domain-02.png)

The geometry is not the runtime itself. It is a manifestation of the contracts that determine whether a capability can compose.

## Relationship to FSM_COS

FSM_COS owns runtime composition. This package owns domain-side description.

~~~text
Domain MicroBundle
       |
       v
MicroBundleDomain
       |
       v
    FSM_COS
       |
       v
 RuntimeAssembly
~~~

The descriptor is not a replacement for `IMicroBundle`. It is metadata that a domain package can use while implementing or generating runtime MicroBundles.

## Packaging

**Package:** `TheSingularityWorkshop.MicroBundleDomain`  
**Version:** `0.1.0-alpha.1`  
**Target:** .NET 8  
**License:** MIT

Concrete domain families should remain separately owned and publishable.

## Status

This is an alpha foundation. The contract is intentionally small so that WebPage can consume domain packages without the hosting layer accumulating domain-specific assumptions.

See [MicroBundle Domain Theory](docs/THEORY.md).

![Trent Best](https://avatars.githubusercontent.com/u/16405167?v=4&size=200)
