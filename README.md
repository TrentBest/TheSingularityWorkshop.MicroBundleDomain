# TheSingularityWorkshop.MicroBundleDomain

**A neutral contract for independently authored capabilities.**

> **A MicroBundle is a focused capability that can identify itself, declare what it needs, accept configuration, enter a runtime, and participate in composition without requiring the host to understand its private domain.**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet version](https://img.shields.io/nuget/v/TheSingularityWorkshop.MicroBundleDomain?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain)
[![NuGet downloads](https://img.shields.io/nuget/dt/TheSingularityWorkshop.MicroBundleDomain?logo=nuget&style=flat-square)](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain)
[![Build Status](https://img.shields.io/github/actions/workflow/status/TrentBest/TheSingularityWorkshop.MicroBundleDomain/package.yml?branch=master&style=flat-square&logo=github)](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/actions/workflows/build.yml)
[![Code Coverage](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.MicroBundleDomain/graph/badge.svg)](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.MicroBundleDomain)

![Opaque MicroBundle Capability Core](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/master/docs/images/microbundle-domain-01.png)

## Before the code: what is the idea?

If you are not a developer, **you can still start here**.

Before the workshop, start with things humans already know how to organize: **books and music**.

A book series can be understood as books; a book as chapters; a chapter through characters, places, events, and descriptions. A character can have appearances, outfits, relationships, and history. None of those relationships require the larger concept to duplicate the smaller concept.

Music provides another familiar example. A song can be related to its composition, performance, recording, release history, album, artist, and representations such as **8-track, vinyl, cassette, CD, or digital**. Those representations can have their own history and metadata while still referring to the same larger musical work.

These examples reveal the idea before software does: **a MicroBundle is a bounded unit of meaning that can participate in larger meaning.**

Now imagine a workshop full of capabilities: thermal behavior, materials, rendering, language, structural analysis, sensors, business rules, or something nobody has invented yet.

A MicroBundle gives one capability a boundary that lets it say:

~~~text
Who am I?
What version am I?
What do I require?
What do I expose?
How do I enter a runtime?
How do I react to the capabilities around me?
~~~

The larger system can work with that common boundary without learning the private meaning of every capability.

**That is the idea before it is an API.**

## Who is this for?

| If you are... | Start here |
|---|---|
| **Curious** | [What Is a MicroBundle?](docs/WHAT_IS_A_MICROBUNDLE.md) |
| **A non-coder** | [What Is a MicroBundle?](docs/WHAT_IS_A_MICROBUNDLE.md), then [Adventures](docs/ADVENTURES.md) |
| **A developer** | [Getting Started](docs/GETTING_STARTED.md) |
| **An architect** | [Architecture](docs/ARCHITECTURE.md) and [Ecosystem Guide](docs/ECOSYSTEM.md) |
| **A toolmaker** | [Description contract](docs/ARCHITECTURE.md), [Adventures](docs/ADVENTURES.md) |
| **A researcher / knowledge designer** | [Knowledge Model](docs/KNOWLEDGE_MODEL.md) and [Theory](docs/THEORY.md) |
| **Someone evaluating the idea** | [FAQ](docs/FAQ.md), [What Is a MicroBundle?](docs/WHAT_IS_A_MICROBUNDLE.md), then the architecture |

You do not need to read everything in order.

The documentation is deliberately a **body of knowledge with multiple entry points**, not one giant technical page.

## Start here

If you know almost nothing about MicroBundles, **do not start with the API**.

1. **[What Is a MicroBundle?](docs/WHAT_IS_A_MICROBUNDLE.md)** — plain-language explanation; no code required.
2. **[Adventures](docs/ADVENTURES.md)** — progressive learning from idea to ecosystem.
3. **[Getting Started](docs/GETTING_STARTED.md)** — implementation guide.
4. **[Ecosystem Guide](docs/ECOSYSTEM.md)** — what / where / how.
5. **[Architecture](docs/ARCHITECTURE.md)** — ownership and dependency direction.
6. **[FAQ](docs/FAQ.md)** — practical questions and objections.
7. **[Glossary](docs/GLOSSARY.md)** — stable vocabulary.
8. **[Knowledge Model](docs/KNOWLEDGE_MODEL.md)** — how the documentation relates to a future structured knowledge domain.

## What does “Micro” mean?

**Not binary size. Not a microservice. Not a tiny DLL.**

Micro means **focused scope**.

A MicroBundle should represent one coherent capability that can be independently identified, versioned, configured, loaded, and composed.

The capability might be tiny.

It might also have a substantial implementation.

The point is that the **boundary is focused**.

## What is a MicroBundle?

The word *bundle* itself is not new. Software has long used bundles, packages, modules, plugins, and components.

The important part is the contract:

~~~text
MicroBundle
  |
  +-- Identity
  +-- Version
  +-- Dependencies
  +-- Providers
  +-- Configuration
  +-- Load
  +-- Arbitration
~~~

That contract lets a host work with a capability without learning the capability's internal domain.

## The three questions

The MicroBundle ecosystem deliberately separates:

| Question | Boundary |
|---|---|
| **What is this capability?** | MicroBundleDomain |
| **Where can I get it?** | MicroBundleRepository |
| **How do I compose it?** | FSM_COS or another composition host |

This is the central idea.

### MicroBundleDomain — what

This package defines the neutral capability contract.

### MicroBundleRepository — where

A repository discovers, retrieves, verifies, caches, and/or materializes capability artifacts.

The repository is **not a dependency of this domain package**.

### Composition host — how

FSM_COS is one composition host. You can build another.

The domain contract does not require a particular composition engine.

## You can build your own MicroBundle ecosystem

This is not a requirement to adopt the entire Singularity Workshop stack.

A third party can create:

~~~text
Acme.Thermal
Acme.Materials
Acme.Rendering
       |
       +-- implement MicroBundleDomain

Acme.MicroBundleRepository
       |
       +-- uses its own storage/delivery

Acme.CompositionHost
       |
       +-- uses its own runtime rules
~~~

The capabilities remain the author's domain.

The repository remains the author's delivery system.

The composition host remains the author's runtime.

**MicroBundleDomain is the neutral seam connecting them.**

The capability owns its meaning and lifecycle participation. The composition host owns the composition process itself: dependency resolution, ordering, arbitration rounds, convergence, and the resulting runtime assembly.

## Why this package exists

Without a common contract, a host tends to accumulate domain knowledge:

~~~text
if Physics...
if Thermal...
if Rendering...
if AEC...
if Magic...
if WhateverComesNext...
~~~

That does not scale.

With MicroBundleDomain, the host learns one thing:

~~~text
"I know how to work with an IMicroBundle."
~~~

A new domain can arrive without teaching the host what that domain means.

---

## Why would I use this?

If you build a modular application, you eventually hit the same problem:

> **How do I let a capability bring its own identity, dependencies, configuration, loading behavior, and composition behavior without teaching my host application what that capability means?**

Without a shared domain contract, the host starts accumulating knowledge:

```text
if bundle is Physics...
if bundle is Rendering...
if bundle is REST...
if bundle is AEC...
if bundle is Magic...
```

Every new capability becomes another host-specific integration.

**MicroBundleDomain moves that responsibility to the capability itself.**

A MicroBundle can describe what it is, declare what it needs, accept host-provided configuration, load itself, and participate in host-driven composition arbitration. The host still owns the composition process and only needs to understand the contract.

That gives you a clean separation:

```text
Your domain package
      |
      | defines meaning + behavior
      v
MicroBundleDomain
      |
      | provides the neutral contract
      v
Your composition host
      |
      | decides how capabilities are assembled
      v
Your application / Experience
```

**It is the seam that lets your domain remain yours.**

---

## Two complementary contracts

MicroBundleDomain deliberately contains two related contracts for two different consumers:

| Contract | Primary consumer | Purpose |
|---|---|---|
| **Runtime contract** | Composition hosts | Describe, load, and arbitrate executable capabilities. |
| **Description contract** | Editors and tooling | Inspect and author configurable capability structure without executing it. |

The runtime side is centered on `IMicroBundle`, `MicroBundleDescriptor`, `MicroBundleDependencyRequest`, `IMicroBundleLoadContext`, and `IMicroBundleArbitrationContext`.

The description side is centered on `MicroBundleDefinition` and `MicroBundleField`.

They share identity information deliberately, but they are not interchangeable representations of the same thing. A runtime host needs an executable capability; tooling needs an inspectable schema.

## What you actually get

| Contract | What it gives you |
|---|---|
| `IMicroBundle` | A stable executable lifecycle for a capability |
| `MicroBundleDescriptor` | Identity, version, dependencies, and provider declarations |
| `MicroBundleDependencyRequest` | A dependency request plus optional configuration bytes |
| `MicroBundleDefinition` | An editor/tooling-facing description of configurable data |
| `MicroBundleField` | Recursive, inspectable configuration schema |
| `IMicroBundleLoadContext` | Host-neutral configuration access during loading |
| `IMicroBundleArbitrationContext` | Host-neutral access to the current composition during arbitration |

That is intentionally small.

This package does **not** try to become your storage system, REST client, renderer, GUI framework, application host, or composition engine. Those are separate concerns.

---

## The 30-second example

Suppose you are building a **ThermalCapability**.

You do not want FSM_COS, a desktop application, a browser, or a future distributed host to know what "thermal capability" means.

Your package can own that meaning:

```csharp
using TheSingularityWorkshop.MicroBundleDomain;

public sealed class ThermalCapability : IMicroBundle
{
    public MicroBundleDescriptor Descriptor { get; } =
        new(
            id: 4201,
            version: "1.0.0",
            providers:
            [
                new MicroBundleProvider("thermal")
            ]);

    public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; } =
        [];

    public void Load(IMicroBundleLoadContext context)
    {
        // Configuration is opaque to MicroBundleDomain; Thermal interprets it.
    }

    public bool Arbitrate(
        IMicroBundleArbitrationContext context,
        int roundIndex)
    {
        // Inspect the composition and reconcile Thermal with participating capabilities.
        return false;
    }
}
```

The important part is what **isn't** here.

There is no FSM_COS, WebPage, WPF, Blazor, REST, Azure, renderer, or application-specific base-class dependency.

Your capability stays portable. A composition host can consume it later.

---

## What problem does the lifecycle solve?

A plain interface can tell you that a class has methods. It does not establish the **meaning of the boundary**.

MicroBundleDomain establishes a deliberately constrained lifecycle:

```text
          Descriptor
              |
              v
      "What capability is this?"
              |
              v
        Dependencies
              |
              v
      "What must exist first?"
              |
              v
             Load
              |
              v
      "Install into this runtime."
              |
              v
          Arbitration
              |
              v
      "Reconcile with the
       assembled composition."
```

The package gives every participating capability the same vocabulary without forcing every capability into the same implementation.

---

## Why not just put this in the host?

Because that reverses the dependency.

The intended direction is:

```text
MicroBundleDomain
      ^
      |
domain packages implement it
      ^
      |
composition hosts consume it
```

Not:

```text
FSM_COS
  ^
  |
every domain package
```

If your domain package has to reference the composition engine merely to become composable, the supposedly independent domain has already become host-dependent.

**MicroBundleDomain is the neutral seam that prevents that.**

---

## Where it fits

MicroBundleDomain is the **meaning and executable contract**.

MicroBundleRepository is the **artifact boundary**.

FSM_COS is the **composition engine**.

```text
                 Author
                   |
                   v
          Domain MicroBundle
                   |
                   v
        +---------------------+
        |  MicroBundleDomain  |
        |                     |
        | identity            |
        | version             |
        | dependencies        |
        | providers           |
        | definition          |
        | executable contract |
        +----------+----------+
                   |
                   v
        +---------------------+
        | MicroBundleRepository|
        |                     |
        | artifact storage    |
        | retrieval           |
        | materialization     |
        +----------+----------+
                   |
                   v
        +---------------------+
        |       FSM_COS       |
        |                     |
        | manifest            |
        | dependency closure  |
        | loading             |
        | arbitration         |
        | RuntimeAssembly     |
        +----------+----------+
                   |
          +--------+--------+
          v        v        v
       WebApp    AnyApp   Other Host
```

**The repository knows where the capability is.  
The domain knows what the capability is.  
The composition engine decides how capabilities become a runtime.**

That separation is the point.

---

## Code-defined or data-driven?

The domain contract supports both.

### Code-defined capability

A package can ship executable behavior:

```text
Assembly
   |
   +-- IMicroBundle implementation
```

Useful when the capability contains algorithms, runtime behavior, or integration logic.

### Data-defined capability

The descriptor and definition can describe semantic/configuration data:

```text
MicroBundleDefinition
   |
   +-- name
   +-- identity/version
   +-- fields
       +-- value
       +-- limits
       +-- nested fields
```

Useful for tooling, editors, manifests, generated controls, and systems that need to inspect a capability without understanding its implementation.

### Both together

```text
Definition       -> tells tooling what can be configured
Implementation   -> performs the capability
Composition host -> decides when and where it participates
```

---

## Dependencies are capability declarations

A dependency is not a reference to an application subsystem.

It is a declaration:

> "This capability requires capability X to participate in its composition."

For example:

```text
Thermal
   |
   +-- requires -> Material
                      |
                      +-- requires -> Element
```

The dependency graph can therefore be assembled from the capabilities themselves. The host does not need a hard-coded table of domain-specific dependencies.

That is what makes demand-driven composition possible.

---

## Configuration stays opaque to the domain package

`MicroBundleDependencyRequest` carries optional `ReadOnlyMemory<byte>` configuration.

MicroBundleDomain intentionally does not dictate whether those bytes represent binary data, a compact protocol, serialized configuration, generated data, or something defined by another package.

**The domain contract provides the boundary. Your serialization/protocol choice remains yours.**

---

## The editor/tooling boundary

`MicroBundleDefinition` exists for systems that need to **inspect and author** a capability without executing it.

```text
MicroBundleDefinition
       |
       +-- String
       +-- Integer [min/max]
       +-- Float   [min/max]
       +-- Boolean
       +-- Object
             +-- child
             +-- child
```

A GUI adapter can turn those semantic categories into controls. A manifest editor can turn them into fields. A tooling package can validate them.

MicroBundleDomain does not decide whether the result is Blazor, WPF, web, desktop, terminal, or something that does not exist yet.

---

## What this package deliberately does NOT do

This is important because it defines the value of the package.

- **It does not store MicroBundles.** Use a repository/storage layer.
- **It does not execute manifests.** That belongs to a composition engine such as FSM_COS.
- **It does not know REST.** REST is a transport concern.
- **It does not know Azure.** Cloud storage is an implementation concern.
- **It does not know GUI.** GUI is a manifestation concern.
- **It does not know Unity, WPF, or Blazor.** Those are host/platform concerns.
- **It does not define your domain.** A thermal bundle remains thermal because **you** define it as thermal.

The package gives that capability a common compositional boundary without taking ownership of the capability itself.

---

## When should you use it?

Use MicroBundleDomain when you need capabilities that are:

- independently authored
- independently versioned
- discoverable by identity
- composable through declared dependencies
- configurable by a host
- loadable without host-specific contracts
- able to participate in composition arbitration
- inspectable by tooling
- portable across different manifestations

You probably **do not** need it for a conventional monolithic application where every feature is compiled directly into one host and there is no need for independent composition.

That is intentional.

---

## Performance baseline

The domain contract has a companion benchmark project:

**[MicroBundleDomain_Benchmarks](https://github.com/TrentBest/MicroBundleDomain_Benchmarks)**

The benchmark measures **MicroBundleDescriptor construction** as dependency and provider composition grows. It deliberately does **not** benchmark FSM_COS composition.

The first recorded Release run used BenchmarkDotNet 0.15.2 on .NET 8.0.31 with an Intel Core i5-10400F and RyuJIT AVX2.

| Count | Scenario | Mean | Allocated |
|---:|---|---:|---:|
| 0 | Empty descriptor | 163.5 ns | 304 B |
| 1 | Dependencies + providers | 281.5 ns | 800 B |
| 4 | Dependencies + providers | 474.1 ns | 1,232 B |
| 16 | Dependencies + providers | 903.7 ns | 2,208 B |
| 64 | Dependencies + providers | 2,761.9 ns | 8,224 B |

The complete matrix is maintained in the benchmark repository.

The important architectural qualification is:

~~~text
MicroBundleDomain benchmark
        │
        ▼
descriptor construction
        │
        X
   not FSM_COS
~~~

These numbers tell us about the cost of the domain descriptor contract. They do not tell us the cost of dependency traversal, loading, arbitration, or RuntimeAssembly creation.

For the complete result matrix, methodology, and instructions for finding the measured methods, see **[docs/BENCHMARKING.md](docs/BENCHMARKING.md)**.

## Documentation

- **[What Is a MicroBundle?](docs/WHAT_IS_A_MICROBUNDLE.md)** — plain-language introduction.
- **[Adventures](docs/ADVENTURES.md)** — learn by progression rather than reference.
- **[Getting Started](docs/GETTING_STARTED.md)** — build your first capability.
- **[Architecture](docs/ARCHITECTURE.md)** — ownership and dependency direction.
- **[Theory](docs/THEORY.md)** — why the model exists.
- **[Knowledge Model](docs/KNOWLEDGE_MODEL.md)** — how these explanations can form a structured knowledge surface.

---

## Version and package status

**Package:** `TheSingularityWorkshop.MicroBundleDomain`  
**Next release:** `1.0.1`  
**Target:** .NET 8  
**License:** MIT

The repository previously published a `1.0.0` package before the ownership boundary was finalized. That package is immutable on NuGet.

The current source corrects the ownership model by making the executable MicroBundle contract domain-owned. The `1.0.1` source is staged for review; publication is a separate release decision.

---

## Related ecosystem

- [FSM_API](https://github.com/TrentBest/FSM_API) — low-level state foundation
- [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) — composition and runtime assembly
- [MicroBundleRepository](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository) — artifact storage/retrieval boundary
- [FSM_REST](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST) — REST capability/transport boundary

---

![Trent Best](https://avatars.githubusercontent.com/u/16405167?v=4&size=200)

---

## Resources & Support

- **NuGet:** [TheSingularityWorkshop.MicroBundleDomain](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain)
- **GitHub:** [TheSingularityWorkshop.MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain)
- **Patreon:** [Support The Singularity Workshop](https://www.patreon.com/c/TheSingularityWorkshop)
- **PayPal:** [Support The Singularity Workshop](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<p align="center">
  <a href="https://github.com/TrentBest/FSM_API">
    <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" height="200">
  </a>
</p>

<p align="center">
  <em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br>
  <strong>Because state shouldn't be a mess.</strong>
</p>
