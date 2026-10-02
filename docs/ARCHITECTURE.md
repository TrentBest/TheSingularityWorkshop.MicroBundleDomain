# Architecture

## The boundary in one sentence

**MicroBundleDomain defines what a capability is; it does not decide where it is stored or how a host composes it.**

That sentence is the most important architectural rule in this repository.

## Three different questions

A composable capability system has three separate questions:

| Question | Owner |
|---|---|
| What is this capability? | MicroBundleDomain |
| Where is its artifact? | MicroBundleRepository |
| How do these capabilities become a runtime? | FSM_COS / composition host |

Confusing these questions creates the coupling this package exists to prevent.

## The portable MicroBundle core

For the capability ecosystem itself, keep the foundation small:

~~~text
MicroBundleDomain
       ^
       |
MicroBundleRepository
~~~

MicroBundleDomain defines **what** a capability is and the neutral contracts through which it participates in a runtime.

A repository implementation handles **where** the capability artifact comes from.

A composition host is an additional consumer that answers **how** capabilities become a runtime. The host owns dependency resolution, ordering, the arbitration loop, convergence, and `RuntimeAssembly`.

This means a third party can replace the repository and composition host without replacing the domain contract.

The important distinction is:

- **Domain is required to author a MicroBundle.**
- **Repository is required only when delivery/discovery/materialization is needed.**
- **FSM_COS is one composition implementation, not part of the domain definition.**

That is why MicroBundleDomain has no dependency on the repository.

## 1. MicroBundleDomain: meaning

The domain package owns the semantic and executable contract:

```text
MicroBundleDomain
├── identity
├── version
├── dependencies
├── providers
├── definition/schema
├── load contract
└── arbitration contract
```

It does not know whether a capability represents:

- an element
- thermal behavior
- material physics
- a mesh
- a REST operation
- a UI feature
- an AEC object
- a game mechanic
- something nobody has invented yet

That blindness is intentional.

## 2. MicroBundleRepository: location

A repository answers a different question:

> **Where can I get the artifact that implements this capability?**

A repository may know about:

- artifact identity
- immutable content
- hashes
- storage
- retrieval
- transport
- materialization

It can use MicroBundleDomain to understand what it materializes.

It should not make MicroBundleDomain depend on the repository.

```text
MicroBundleDomain
       ▲
       │ implements / materializes
       │
MicroBundleRepository
```

The repository is a consumer of the domain contract.

## 3. FSM_COS: composition

FSM_COS answers:

> **Given a manifest and available capabilities, how do I assemble a runtime?**

Its responsibilities include:

- manifest execution
- dependency traversal
- dependency closure
- loading
- arbitration
- convergence
- RuntimeAssembly

FSM_COS consumes the MicroBundleDomain contract.

MicroBundleDomain does not consume FSM_COS.

```text
MicroBundleDomain ───────► FSM_COS
       contract             orchestration
```

The arrow represents dependency direction: the composition host depends on the domain contract.

## Why this direction matters

Imagine a new domain package:

```text
TheSingularityWorkshop.Thermal
```

If Thermal references FSM_COS, then Thermal cannot exist independently of the composition engine.

Instead:

```text
Thermal
   │
   └── implements MicroBundleDomain
                     ▲
                     │
                  FSM_COS
```

Now both sides understand the same contract without becoming dependent on one another.

That is the reusable seam.

## Runtime contract versus description contract

MicroBundleDomain contains two complementary contract surfaces:

```text
MicroBundleDomain
│
├── Runtime contract
│   ├── IMicroBundle
│   ├── MicroBundleDescriptor
│   ├── MicroBundleDependencyRequest
│   ├── IMicroBundleLoadContext
│   └── IMicroBundleArbitrationContext
│
└── Description contract
    ├── MicroBundleDefinition
    └── MicroBundleField
```

The **runtime contract** is for composition hosts. It defines the executable lifecycle and the information a capability exchanges with its host.

The **description contract** is for authoring and tooling. It exposes inspectable configuration structure so an editor, Forge, manifest tool, or GUI adapter can reason about a capability without executing it.

This does not make the domain package GUI-aware. The domain defines semantic field categories; a GUI or editor decides how those categories are manifested.

## Runtime lifecycle

The executable contract has four conceptual stages.

### 1. Describe

```csharp
MicroBundleDescriptor Descriptor
```

The bundle identifies itself and declares its static composition metadata.

### 2. Request

```csharp
IReadOnlyList<MicroBundleDependencyRequest> Dependencies
```

The bundle declares capabilities it requires.

Requests may carry opaque configuration bytes.

### 3. Load

```csharp
void Load(IMicroBundleLoadContext context)
```

The host supplies a context through which the bundle can access runtime-specific configuration.

The bundle installs its capability.

### 4. Arbitrate

```csharp
bool Arbitrate(
    IMicroBundleArbitrationContext context,
    int roundIndex)
```

The bundle can inspect the current composition and respond to other participating capabilities.

The host controls the arbitration loop and convergence policy. A MicroBundle participates in that process; it does not become the composition engine.

## Static description versus executable contract

There are two related but distinct descriptions.

### `MicroBundleDescriptor`

This is the compact runtime-facing identity:

```text
ID
Version
Dependencies
Providers
```

### `MicroBundleDefinition`

This is the tooling/editor-facing schema:

```text
Name
Descriptor
Fields
  ├── String
  ├── Integer
  ├── Float
  ├── Boolean
  └── Object
```

A host can use the descriptor to reason about composition.

Tooling can use the definition to reason about configuration.

Neither one needs to understand the internal implementation of the domain.

## Why providers are opaque

A provider has an identifier, but MicroBundleDomain does not interpret the identifier.

For example:

```text
"thermal"
"mesh"
"preview"
"physics"
"whatever-you-need"
```

The domain package can expose those identifiers while another layer decides what they mean.

This prevents the foundation from becoming a giant registry of every possible capability in the ecosystem.

## Why configuration is opaque

The contract uses:

```csharp
ReadOnlyMemory<byte>
```

rather than selecting JSON, XML, a particular serializer, or a particular protocol.

That matters because configuration is a boundary, not a domain decision.

A system can choose deterministic binary serialization, a compact protocol, generated data, or another representation without changing the MicroBundle contract.

## Composition remains demand-driven

Dependencies describe what is required by a capability.

They do not turn the entire universe of possible capabilities into one application.

Conceptually:

```text
Requested capability
        │
        ▼
required dependencies
        │
        ▼
required dependencies of dependencies
        │
        ▼
closed composition
```

A host can therefore construct only the capability graph required by the manifest.

## Host neutrality

The load and arbitration contexts intentionally expose only host-neutral information.

### Load context

Provides:

- runtime identity
- requested configuration

### Arbitration context

Provides:

- runtime identity
- participating bundles
- opaque experience context

A concrete host can implement these interfaces with richer behavior without forcing that host's types into the domain package.

## Anti-coupling test

When adding a new MicroBundle domain, ask:

> **Could I author and compile this capability without referencing the application that will eventually consume it?**

If yes, the boundary is doing its job.

If no, determine which host concern leaked into the domain package.

Common leaks include:

- GUI controls
- storage clients
- REST clients
- application service locators
- host-specific base classes
- renderer types
- Unity types
- WPF types
- Blazor types

Those belong outside this package.

## Relationship to Experience

A MicroBundle is smaller than an Experience.

```text
Provider
   │
MicroBundle
   │
Composition
   │
Experience
   │
Digital Reality
```

An Experience can be composed from many capabilities.

The domain contract makes those capabilities independently expressible without making MicroBundleDomain responsible for the Experience's presentation.

## Relationship to Forge

Forge can author or manipulate compositions.

That does not make Forge part of the MicroBundle contract.

```text
MicroBundleDomain → defines the capability
Forge             → authors composition
FSM_COS            → executes composition
Host               → manifests result
```

Keeping those roles separate allows the same capability model to be used by different authoring and hosting environments.

## Architectural invariant

A new MicroBundle domain should be able to enter the ecosystem without requiring the host application to learn the domain's internal semantics.

That is the test.

If the host must add a new domain-specific branch every time a new capability appears, the composition boundary has failed.

## Further reading

- **[Getting Started](GETTING_STARTED.md)** — implement a capability.
- **[MicroBundle Domain Theory](THEORY.md)** — the conceptual model.
- **README** — user-facing overview and package value.
