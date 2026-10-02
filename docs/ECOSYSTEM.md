# MicroBundle Ecosystem Guide

## Purpose

`TheSingularityWorkshop.MicroBundleDomain` is deliberately small because it is the vocabulary at the edge of a larger ecosystem.

It lets an author describe a MicroBundle without requiring the author to surrender ownership of the domain to the host.

~~~text
Third-party domain
      |
      v
their MicroBundle definitions
      |
      v
their MicroBundle repository
      |  admission / trust decision
      v
Singularity Workshop ecosystem
      |
      v
FSM_COS composition
      |
      v
Experience
~~~

The important boundary is the **human-controlled admission decision**.

A repository existing somewhere on the Internet does not automatically make its contents available to an ecosystem. An ecosystem owner can decide which external repositories are admitted, which artifacts are permitted, and when that relationship is revoked.

The domain package describes capabilities. It does not make trust decisions.

## Build your own MicroBundle ecosystem

A separate organization can create a complete MicroBundle ecosystem without modifying the hosting application.

At minimum, the organization needs:

1. a set of MicroBundle definitions;
2. stable bundle identities;
3. explicit versions;
4. declared dependencies;
5. declared providers where appropriate;
6. executable or data-backed bundle artifacts;
7. a repository capable of storing and delivering those artifacts; and
8. a publication/admission policy for the ecosystems that are allowed to consume them.

The repository can be implemented independently. The core repository contract in TheSingularityWorkshop.MicroBundleRepository is intentionally platform-neutral, while its Azure implementation uses Blob Storage as one delivery substrate.

That means an organization can own its own repository and still describe its capabilities with this package.

## A small domain

A domain author can start with a descriptor:

~~~csharp
var hydrogen = new MicroBundleDescriptor(
    id: 1001,
    version: "1.0.0",
    providers:
    [
        new MicroBundleProvider("element")
    ]);
~~~

The descriptor says **what the capability declares**.

It does not say:

- how a GUI renders it;
- how FSM_COS arbitrates it;
- where the artifact is stored;
- whether an ecosystem trusts its repository;
- whether the capability is licensed for a particular Experience; or
- whether a particular host is permitted to load it.

Those decisions belong to other boundaries.

## Configurable domain data

A domain can also expose editor-oriented structure:

~~~csharp
var material = new MicroBundleDefinition(
    "Hydrogen",
    hydrogen,
    [
        new MicroBundleField(
            "AtomicNumber",
            MicroBundleFieldKind.Integer,
            defaultValue: 1,
            minimum: 1,
            maximum: 118),

        new MicroBundleField(
            "Symbol",
            MicroBundleFieldKind.String,
            defaultValue: "H")
    ]);
~~~

The field schema is intentionally semantic rather than GUI-specific.

A Blazor, WPF, Unity, WebGPU, VR, or future GUI adapter can decide how an Integer, Float, Boolean, String, or Object is manifested.

The domain package therefore describes the **shape of the authoring surface**, not the pixels.

## Dependencies

Dependencies express composition requirements:

~~~csharp
var hydrogen = new MicroBundleDescriptor(
    id: 1001,
    version: "1.0.0",
    dependencies:
    [
        new MicroBundleDependency(1000)
    ]);
~~~

The dependency is an assertion that another capability is part of the composition.

The domain package does not resolve the graph.

That belongs to FSM_COS, where dependency closure, configured loading, arbitration, and convergence can be performed against the actual installed ecosystem.

## Providers

Providers are intentionally opaque identifiers:

~~~csharp
new MicroBundleProvider("element")
new MicroBundleProvider("mesh")
new MicroBundleProvider("thermal")
new MicroBundleProvider("fracture")
~~~

The hosting ecosystem must not become a giant switch statement over domain names.

Instead:

~~~text
Domain package
    defines meaning
         |
         v
MicroBundleDomain
    describes capability
         |
         v
FSM_COS
    composes capability
         |
         v
Experience / host
    manifests capability
~~~

This is what allows a new scientific, entertainment, AEC, educational, or commercial domain to arrive without teaching the host what every domain means.

## Repository ownership

An organization may operate its own MicroBundle repository.

For example:

~~~text
ExampleCo MicroBundle Repository
|
+-- licensed digital toys
+-- character assets
+-- animation capabilities
+-- educational content
+-- domain-specific behavior
~~~

The organization controls those artifacts and their licensing.

The Singularity Workshop ecosystem does not need to copy those artifacts into its own source repository. Instead, the external repository can expose an agreed delivery contract.

This creates a useful network effect:

~~~text
External author
      |
      | creates and hosts capabilities
      v
External repository
      |
      | admitted by ecosystem owner
      v
Singularity Workshop
      |
      +---- Experience A
      +---- Experience B
      +---- Experience C
      |
      v
new compositions created by users
~~~

The external author gains an additional distribution and composition surface.

The host ecosystem gains capabilities it did not have to author itself.

The user gains access to a larger vocabulary of things that can be composed.

## Human-in-the-gap admission

Federation must not imply automatic trust.

A useful lifecycle is:

~~~text
repository discovered
        |
        v
metadata reviewed
        |
        v
ownership / license / compatibility checked
        |
        v
human admission decision
      /   \
   admit  reject
     |
     v
repository becomes an allowed source
     |
     v
artifacts may participate in composition
~~~

The admission record belongs outside `MicroBundleDescriptor`.

That separation is deliberate.

A descriptor should remain portable even when the same MicroBundle is used only by its author, offered to a partner, admitted into the Singularity Workshop ecosystem, revoked from that ecosystem, or consumed by another compatible host.

Trust is contextual. Capability identity should not be.

## Licensed digital reality

This model is intentionally compatible with commercial content.

A toy manufacturer could define digital representations of its physical product families. A game publisher could define characters, environments, props, animation systems, or licensed worlds. An educational organization could define laboratory equipment or curriculum-specific simulation capabilities.

The MicroBundle contract does not decide whether those things are games, toys, simulations, educational material, or AEC assets.

It provides a common composition boundary.

The licensing relationship remains an explicit business and governance concern.

## What belongs in this package

This package should continue to own:

- MicroBundle identity;
- version;
- dependency declarations;
- provider declarations;
- editor-oriented field structure;
- validation of those local declarations; and
- documentation of the portable domain contract.

It should **not** grow into:

- Azure storage;
- repository federation;
- authentication;
- license enforcement;
- dependency arbitration;
- Experience hosting;
- GUI implementation; or
- domain-specific scientific semantics.

Those concerns have their own boundaries.

## Release-readiness invariant

A 1.0 release of this package should let a developer answer all of these questions from the documentation:

1. What is a MicroBundle?
2. Why is it separate from FSM_COS?
3. How do I define one?
4. How do I declare dependencies?
5. How do I describe configurable data?
6. How do I expose providers?
7. How do I create my own MicroBundle repository?
8. How does an external repository participate in a larger ecosystem?
9. Where does human admission/trust belong?
10. What does this package deliberately **not** do?

That documentation is part of the product, not decoration.
