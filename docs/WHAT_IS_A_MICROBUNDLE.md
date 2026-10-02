# What Is a MicroBundle?

## Start with **Micro**

**Micro does not mean a tiny file.**

It means a **focused unit of capability**.

A MicroBundle is small in *scope*: it represents one coherent capability that can be identified, versioned, configured, loaded, and composed independently. A MicroBundle may contain very little code or a substantial internal implementation.

**Micro describes the capability boundary, not the number of bytes.**

## And what does "Bundle" mean?

The word *bundle* is not new. Software has used bundles, packages, modules, plugins, components, and similar ideas for decades.

We do not need to claim that nobody has ever discussed bundles. The useful question is:

> **What does this particular bundle boundary guarantee?**

A MicroBundle combines:

~~~text
                  MicroBundle
                       |
       +---------------+----------------+
       |               |                |
    Identity       Dependencies     Configuration
       |               |                |
       +---------------+----------------+
                       |
                 Load + Arbitration
                       |
                       v
                Composable capability
~~~

That gives a composition host a common vocabulary without requiring the host to understand the domain.

## A MicroBundle is a capability, not a feature folder

A folder such as Physics/ or Rendering/ is organization.

A MicroBundle is a **runtime boundary**.

For example:

~~~text
ThermalMicroBundle
    identity: Thermal
    version: 1.0.0
    requires: Material
    configuration: temperature model
    load: install thermal capability
    arbitration: reconcile with composition
~~~

The host does not need an "if this is Thermal" branch.

It only needs to understand the MicroBundle contract.

## The three questions

A composable capability system deliberately separates:

| Question | Boundary |
|---|---|
| **What is it?** | MicroBundleDomain |
| **Where can I get it?** | MicroBundleRepository |
| **How do I compose it?** | FSM_COS or another composition host |

### MicroBundleDomain — what

Your domain defines the capability.

It owns identity, version, dependencies, providers, configuration schema, executable behavior, and arbitration behavior.

It does **not** need to know where the capability will run.

### MicroBundleRepository — where

A repository delivers artifacts.

It may use a local directory, Git, HTTP, Azure Blob Storage, a database, an object store, a private enterprise service, or something entirely custom.

The repository implementation is free to choose its storage model.

### FSM_COS — how

FSM_COS is one composition host. Another organization can build another.

The composition host resolves the requested graph and turns it into a runtime.

## You can build your own MicroBundle ecosystem

The Singularity Workshop repository is **one implementation**, not a law of nature.

You could build:

~~~text
Acme.MicroBundleDomain
        |
        +-- shared contract

Acme.MicroBundleRepository
        |
        +-- S3
        +-- Git
        +-- private artifact service

Acme.CompositionHost
        |
        +-- your runtime

Acme.Thermal
Acme.Materials
Acme.Rendering
~~~

The capabilities remain yours. The repository remains yours. The composition host remains yours. The contract is the common seam.

## MicroBundle versus NuGet package

A NuGet package is primarily a **software distribution mechanism**.

A MicroBundle is a **runtime composition capability**.

A NuGet package can contain a MicroBundle implementation:

~~~text
NuGet package
     |
     +-- MicroBundle implementation
              |
              +-- Repository materialization
                         |
                         +-- Composition host
~~~

Do not confuse the package boundary with the capability boundary.

## MicroBundle versus plugin

A plugin usually answers:

> "Can I extend this host with additional code?"

A MicroBundle answers:

> "Can this capability identify itself, declare what it needs, accept configuration, load into a runtime, and participate in composition without becoming part of the host's domain model?"

A plugin can be implemented as a MicroBundle. A MicroBundle does not have to be tied to one plugin host.

## MicroBundle versus microservice

A microservice is a distributed service boundary.

A MicroBundle is a capability composition boundary.

A MicroBundle can run entirely inside one process. The word **Micro** does not imply a network boundary.

## The simplest mental model

Think of a MicroBundle as a **capability with a passport**.

The passport answers:

~~~text
Who am I?
What version am I?
What do I require?
What do I expose?
How do I load?
How do I react to the other capabilities around me?
~~~

The repository finds the passport and implementation.

The composition host decides how capabilities become a runtime.

The application decides what the resulting runtime means to its users.

## The architectural test

Ask:

> **Could another host consume this capability without learning the internal semantics of my domain?**

If yes, the boundary is working.

If the host must learn what Thermal, Element, Mesh, AEC, or Magic means before it can compose the capability, the domain boundary has leaked.

## Keep going

- [Getting Started](GETTING_STARTED.md)
- [Adventures](ADVENTURES.md)
- [Ecosystem Guide](ECOSYSTEM.md)
- [Architecture](ARCHITECTURE.md)
- [Theory](THEORY.md)