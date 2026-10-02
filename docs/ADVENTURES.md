# MicroBundle Adventures

These are intentionally written as **adventures**, not reference documentation.

The goal is to get from:

> "I barely know what a MicroBundle is."

to:

> "I can build one, give it dependencies, publish it through a repository, and compose it."

You do not need to understand the entire Singularity Workshop ecosystem before starting.

## Adventure 0 — Meet the MicroBundle

Understand why **Micro** means focused capability, not file size; what the bundle boundary owns; what the repository owns; and what the composition host owns.

[Start Adventure 0](WHAT_IS_A_MICROBUNDLE.md)

## Adventure 1 — Your First Capability

Create a real `IMicroBundle` implementation in an ordinary .NET class library.

[Start Adventure 1](adventures/01-your-first-microbundle.md)

## Adventure 2 — Make Two Capabilities Meet

Create Greeting -> Language and see why the dependency belongs to the capability rather than the host.

[Start Adventure 2](adventures/02-two-bundles.md)

## Adventure 3 — Put Your Capabilities in a Repository

Separate authoring from delivery and learn why the repository is a boundary rather than a dependency of the domain.

[Start Adventure 3](adventures/03-your-own-repository.md)

## Adventure 4 — Make It Configurable

Create a `MicroBundleDefinition` with text, numbers, ranges, and nested objects.

[Start Adventure 4](adventures/04-configurable-bundles.md)

## Adventure 5 — Build Your Own Ecosystem

Replace the Workshop repository and composition implementations while keeping the domain contract.

[Start Adventure 5](adventures/05-your-ecosystem.md)

## Adventure 6 — Arbitration

Understand why loading and arbitration are separate and why the host owns convergence.

[Start Adventure 6](adventures/06-arbitration.md)

## Adventure map

~~~text
Meet the idea
     |
     v
First capability
     |
     v
Two capabilities
     |
     v
Repository
     |
     +------> Configuration
     |
     +------> Arbitration
     |
     v
Your ecosystem
~~~

## The destination

```text
MicroBundleDomain
    = what a capability is

MicroBundleRepository
    = where a capability comes from

Composition Host
    = how capabilities become a runtime

Application / Experience
    = what users actually experience
```

That is the whole game.
