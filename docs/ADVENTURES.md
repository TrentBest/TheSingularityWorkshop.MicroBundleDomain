# MicroBundle Adventures

These are intentionally written as **adventures**, not reference documentation.

They are also intentionally layered for different readers.

You can be:

- **Curious** — you want to understand the idea.
- **A builder** — you want to create a capability.
- **An architect** — you want to understand the boundaries.
- **A toolmaker** — you want to build editors, repositories, or other infrastructure around the contract.

You do **not** need to be a programmer to begin.

## The learning path

~~~text
Curiosity
   |
   v
What is a MicroBundle?
   |
   v
See a capability in context
   |
   v
Build one
   |
   v
Give it dependencies
   |
   v
Deliver it through a repository
   |
   v
Make it configurable
   |
   v
Understand arbitration
   |
   v
Build an independent ecosystem
~~~

The early adventures explain the idea before asking you to write code.

---

## Adventure 0 — Meet the MicroBundle

**For everyone. No code required.**

Learn:

- what "Micro" means
- what a capability is
- what a MicroBundle is and is not
- why the boundary matters
- why repository and composition are separate concerns
- how the pieces fit together

[Start Adventure 0](WHAT_IS_A_MICROBUNDLE.md)

---

## Adventure 0.5 — Explore a Book and a Song

**For everyone. No code required.**

Before building a MicroBundle, try seeing one in things humans already organize naturally.

### The book path

Start with a book series:

~~~text
Series
  -> Book
      -> Chapter
          -> Character
              -> Outfit / Description / History
~~~

Ask yourself at each level:

> **Could this concept be identified and understood on its own?**

If yes, you have found a plausible boundary of meaning.

Then notice that the same concepts can participate in other relationships. A character can appear in multiple chapters. A character can have multiple outfits. A book can belong to a series and also to an edition, translation, adaptation, or publication history.

### The music path

Now take a familiar song and follow it through its representations:

~~~text
Song
  -> Recording
      -> Release
          -> 8-track
          -> Vinyl
          -> Cassette
          -> CD
          -> Digital
~~~

Then add the surrounding knowledge:

~~~text
Song
  -> Artist
  -> Album
  -> Genre
  -> Composition
  -> Performance
  -> Release history
~~~

The point is not to claim that every item above must become a MicroBundle. The point is to notice how naturally humans already organize complex meaning into **independently recognizable concepts connected by relationships**.

That is the mental model we want you to carry into the technical adventures.

[Return to Adventure 0](WHAT_IS_A_MICROBUNDLE.md)

---

## Adventure 1 — Your First Capability

**For builders.**

Create a real `IMicroBundle` implementation in an ordinary .NET class library.

[Start Adventure 1](adventures/01-your-first-microbundle.md)

---

## Adventure 2 — Make Two Capabilities Meet

**For builders and architects.**

Create Greeting → Language and see why the dependency belongs to the capability rather than the host.

[Start Adventure 2](adventures/02-two-bundles.md)

---

## Adventure 3 — Put Your Capabilities in a Repository

**For builders, architects, and infrastructure designers.**

Separate authoring from delivery and learn why the repository is a boundary rather than a dependency of the Domain.

[Start Adventure 3](adventures/03-your-own-repository.md)

---

## Adventure 4 — Make It Configurable

**For builders and toolmakers.**

Create a `MicroBundleDefinition` with text, numbers, ranges, and nested objects.

[Start Adventure 4](adventures/04-configurable-bundles.md)

---

## Adventure 5 — Build Your Own Ecosystem

**For architects and ecosystem designers.**

Replace the Workshop repository and composition implementations while keeping the Domain contract.

[Start Adventure 5](adventures/05-your-ecosystem.md)

---

## Adventure 6 — Arbitration

**For builders and architects.**

Understand why loading and arbitration are separate and why the host owns convergence.

[Start Adventure 6](adventures/06-arbitration.md)

---

## Adventure 7 — From Artifact to Runtime

**For everyone who wants to see the complete journey.**

Follow a capability through authoring, repository materialization, composition, runtime assembly, and presentation.

[Start Adventure 7](adventures/07-artifact-to-runtime.md)

---

## Adventure map

~~~text
                  THE IDEA
                     |
                     v
              First Capability
                     |
                     v
              Dependencies
                     |
             +-------+-------+
             |               |
             v               v
         Repository     Configuration
             |               |
             +-------+-------+
                     |
                     v
                Arbitration
                     |
                     v
              Your Ecosystem
                     |
                     v
               Runtime / Experience
~~~

## The destination

~~~text
MicroBundleDomain
    = what a capability is

MicroBundleRepository
    = where a capability comes from

Composition Host
    = how capabilities become a runtime

Application / Experience
    = what people ultimately experience
~~~

You do not have to adopt all four layers.

You can stop at the contract.

You can build your own repository.

You can build your own composition host.

You can build tools around the description contract.

**The architecture is designed so that the layers can be adopted independently.**

## A note about the documentation itself

These adventures are deliberately more expansive than a conventional API README.

That is intentional.

The same concepts will appear in several forms:

- plain-language explanations
- diagrams
- examples
- tutorials
- architecture documents
- theory
- glossary definitions
- eventually, potentially, machine-readable knowledge

The repetition is useful when it teaches the same fact from a different angle.

The goal is not merely to document an API.

The goal is to make the underlying model understandable enough that a person can encounter it, question it, learn it, build with it, or build something else compatible with it.

## Continue

- [What Is a MicroBundle?](WHAT_IS_A_MICROBUNDLE.md)
- [Getting Started](GETTING_STARTED.md)
- [Ecosystem Guide](ECOSYSTEM.md)
- [Architecture](ARCHITECTURE.md)
- [Knowledge Model](KNOWLEDGE_MODEL.md)
