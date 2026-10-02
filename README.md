# TheSingularityWorkshop.MicroBundleDomain

**Domain-side foundation for MicroBundle identity, composition metadata, and runtime contracts.**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet version](https://img.shields.io/nuget/v/TheSingularityWorkshop.MicroBundleDomain?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain)
[![NuGet downloads](https://img.shields.io/nuget/dt/TheSingularityWorkshop.MicroBundleDomain?logo=nuget&style=flat-square)](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain)
[![Build Status](https://img.shields.io/github/actions/workflow/status/TrentBest/TheSingularityWorkshop.MicroBundleDomain/package.yml?branch=master&style=flat-square&logo=github)](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/actions/workflows/build.yml)
[![Last commit](https://img.shields.io/github/last-commit/TrentBest/TheSingularityWorkshop.MicroBundleDomain/master)](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/commits/master)
[![Code Coverage](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.MicroBundleDomain/graph/badge.svg)](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.MicroBundleDomain)

![Opaque MicroBundle Capability Core](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/master/docs/images/microbundle-domain-01.png)

A MicroBundle is a **loadable semantic capability**.

This repository provides the domain-side meaning and executable contract of a MicroBundle without owning storage, transport, or composition orchestration.

## Ownership boundary

```text
MicroBundleDomain
    identity / version / dependencies / providers
    definition / schema / executable IMicroBundle contract
          |
          v
MicroBundleRepository
    artifact identity / immutable bytes / storage / retrieval
          |
          v
FSM_COS
    manifest execution / dependency traversal / arbitration / RuntimeAssembly
```

**MicroBundleDomain defines what a MicroBundle is. MicroBundleRepository stores and retrieves its artifact representation. FSM_COS composes requested capabilities.**

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

FSM_COS owns runtime composition, but it does not own the MicroBundle contract. The executable `IMicroBundle` contract belongs to this domain package. FSM_COS supplies concrete host contexts when it performs composition.

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

The descriptor and definition describe the capability, while `IMicroBundle` defines the executable capability contract. Host contexts remain neutral so the domain package does not depend on FSM_COS.

## Packaging

**Package:** `TheSingularityWorkshop.MicroBundleDomain`  
**Source correction:** `2.0.0-alpha.1`  
**Target:** .NET 8  
**License:** MIT

Concrete domain families should remain separately owned and publishable.

The corrected contract is staged as `2.0.0-alpha.1` because moving the executable MicroBundle contract into the domain package is a structural ownership correction.

## Status

The repository was published as `1.0.0` before the ownership boundary was finalized. That publication was premature. NuGet's immutability means published `1.0.0` cannot be replaced in place, so this repository records the correction rather than pretending the historical package does not exist. No release is implied by this source correction.

See [MicroBundle Domain Theory](docs/THEORY.md).

![Trent Best](https://avatars.githubusercontent.com/u/16405167?v=4&size=200)

---

## 🔗 Resources & Support

### 📦 Related packages

- [TheSingularityWorkshop.FSM_API](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_API)
- [TheSingularityWorkshop.MicroBundleRepository](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository)
- [TheSingularityWorkshop.FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)
- [TheSingularityWorkshop.MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain)

### 💖 Support The Singularity Workshop

- **Patreon:** [Support us on Patreon](https://www.patreon.com/c/TheSingularityWorkshop)
- **PayPal:** [Make a donation](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<p align="center">
  <a href="https://github.com/TrentBest/FSM_API">
    <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" height="200">
  </a>
</p>

<p align="center">
  <em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br>
  <strong>Because state shouldn't be a mess.</strong>
</p>
