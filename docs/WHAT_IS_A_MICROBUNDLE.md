# What Is a MicroBundle?

## You do not need to be a programmer to start here

If the word **MicroBundle** is new to you, forget the code for a moment.

Imagine a large workshop.

You do not want one giant machine that knows how to do **everything**. You want useful pieces that can be brought into the workshop when they are needed:

- a temperature system
- a material system
- a rendering system
- a language system
- a structural-analysis system
- a door
- a sensor
- a payment capability
- a piece of business logic

Each piece has a job.

A **MicroBundle is a way of giving one such capability a well-defined boundary** so that it can be identified, brought into a larger system, configured, and combined with other capabilities without requiring the larger system to understand the capability's private meaning.

That is the idea.

You do not need to understand .NET, C#, FSM_COS, repositories, or serialization to understand the idea.

---

## The simplest possible definition

> **A MicroBundle is an independently identifiable capability that can tell a larger system what it is, what it needs, and how it participates when it is brought together with other capabilities.**

The word **Micro** describes the **scope of the capability**, not its file size.

The word **Bundle** describes the fact that the capability comes with the information and behavior needed to participate as a unit.

So:

**MicroBundle = focused capability boundary.**

---

## Start with something you already understand: a book

Before thinking about software, think about a **book series**.

Suppose a series contains several books:

~~~text
Series
  |
  +-- Book 1
  +-- Book 2
  +-- Book 3
  +-- Book 4
~~~

The series can be understood as a bundle of the books that belong to it. But each book is meaningful on its own.

Now look inside one book:

~~~text
Book
  |
  +-- Chapter 1
  +-- Chapter 2
  +-- Chapter 3
  +-- Chapter 4
~~~

And a chapter can have meaningful things of its own:

~~~text
Chapter
  |
  +-- Characters
  +-- Locations
  +-- Events
  +-- Objects
  +-- Dialogue
  +-- Descriptions
~~~

A character can be described through the story as well:

~~~text
Character
  |
  +-- identity
  +-- descriptions
  +-- relationships
  +-- appearances
  +-- outfits
  +-- actions
  +-- history
~~~

The important observation is **not** that a book is physically made from MicroBundles. This is a way to understand the compositional idea.

A MicroBundle is useful because a meaningful concept can stand on its own **and also participate in a larger meaningful concept**.

The series does not need to duplicate every detail of every book. It can reference the books. A book can reference its chapters. A chapter can reference the characters, events, places, and other concepts that give it meaning.

That is composition by reference rather than composition by duplication.

### The boundary can appear at many levels

There is no rule saying that a MicroBundle must represent a particular kind of thing.

For example:

~~~text
Series
  -> Book
      -> Chapter
          -> Character
              -> Outfit
          -> Location
          -> Event
~~~

Each node can be independently meaningful.

The useful question is not:

> "How small is it?"

The useful question is:

> **"What coherent meaning does this boundary represent?"**

That is why **Micro** is about scope, not physical size.

---

## Music makes the idea even more interesting

Now consider a song you already know.

Take a Journey song such as **"Don't Stop Believin'"**.

A listener thinks of it as one song. But that song can have many legitimate aspects and representations:

~~~text
Song
  |
  +-- composition
  +-- performance
  +-- recording
  +-- lyrics
  +-- musicians
  +-- instruments
  +-- artwork
  +-- release history
  +-- recording formats
       +-- 8-track
       +-- vinyl
       +-- cassette
       +-- CD
       +-- digital
~~~

The physical or digital representation can become another meaningful boundary.

An 8-track is not merely "the same bytes in a smaller box." It has its own historical context, physical characteristics, release information, and relationship to the recording.

A vinyl release has different information and constraints. A cassette release has another. A CD release has another. A digital release can carry yet another set of metadata.

And the song itself has history:

~~~text
Song
  |
  +-- written
  +-- recorded
  +-- mixed
  +-- released
  +-- re-released
  +-- remastered
  +-- represented in different formats
~~~

Those are all useful pieces of knowledge about the same larger concept.

A MicroBundle model should therefore be comfortable with **composition, representation, provenance, and history**. The fact that a concept participates in another bundle does not make the smaller concept meaningless. Quite the opposite: its independent meaning is what makes composition possible.

### The same song can participate in many bundles

A song might belong to:

~~~text
Song
  |
  +-- Album
  +-- Artist
  +-- Genre
  +-- Year / Era
  +-- Release
  +-- Format
  +-- Collection
  +-- Personal Playlist
~~~

The song is not necessarily "owned" by one of those contexts. It can be referenced by many contexts.

This is an important part of the MicroBundle idea: **composition does not have to mean containment**.

A bundle can express a meaningful relationship to other MicroBundles without physically swallowing their definitions.

---

## Books and music show why "bundle" does not mean "container"

The word *bundle* can make people imagine a box full of objects.

That is too restrictive for this model.

A better mental picture is a **network of meaningful references**:

~~~text
                  Series
                 /      \\
             Book       Book
              |
           Chapter
          /   |   \\
     Character Event Location
          |
        Outfit
~~~

And elsewhere:

~~~text
                Artist
               /      \\
            Album     Song
                        |
             +----------+----------+
             |          |          |
           Vinyl      Cassette     CD
             |          |          |
          release     release    release
~~~

The same concept may participate in several larger structures.

That is why a MicroBundle is better thought of as a **bounded unit of meaning** than as a physical container.

---

## Composition can carry history too

Once you stop thinking of a bundle as merely a box of implementation, another important possibility appears: **history can itself be composable information**.

A MicroBundle describing a book might relate the book to:

- its edition
- its publication history
- its chapters
- its characters
- its settings
- its illustrations
- its translations
- its adaptations

A MicroBundle describing music might relate a song to:

- its composition
- its performers
- its recording sessions
- its releases
- its formats
- its remasters
- its artwork
- its catalog history

The contract does not need to dictate which of these domains are correct. The domain author defines the meaningful relationships.

**The MicroBundle contract provides the seam; the domain provides the meaning.**

---

## Think about a toolbox

Imagine opening a toolbox.

You might find:

~~~text
Hammer
Screwdriver
Level
Wrench
Tape Measure
~~~

You do not need to rebuild the toolbox every time you want to use the hammer.

You identify the tool, take it out, and use it for its purpose.

Now imagine that the tools can also say:

~~~text
Hammer
  "I am a hammer."
  "I am version 2."
  "I require a handle."
  "Here is how I fit into the workshop."
~~~

That is closer to what a MicroBundle does.

The analogy is not the implementation. It is simply a way to understand the boundary.

---

## What makes the boundary useful?

A MicroBundle can carry several kinds of information:

~~~text
                    MicroBundle
                         |
          +--------------+--------------+
          |              |              |
       Identity      Dependencies   Configuration
          |              |              |
          +--------------+--------------+
                         |
                    Load + Arbitration
                         |
                         v
                 Composable capability
~~~

In ordinary language:

| Question | Meaning |
|---|---|
| **Who are you?** | Identity and version |
| **What do you need?** | Dependencies |
| **How should I configure you?** | Configuration |
| **How do you enter the runtime?** | Loading |
| **What happens when you meet other capabilities?** | Arbitration |

You do not have to understand the implementation of those mechanisms yet.

The important idea is that the capability brings its **own boundary** with it.

---

## What does “Micro” mean?

**Micro does not mean tiny.**

It does not mean:

- a tiny DLL
- a small amount of code
- a microservice
- a network service
- a disposable component

It means **focused**.

A MicroBundle might contain a small amount of code.

It might also contain a substantial implementation.

What matters is that its **responsibility is coherent**.

For example:

~~~text
Good boundary:

Thermal Capability
  -> thermal behavior

Material Capability
  -> material behavior

Rendering Capability
  -> rendering behavior
~~~

The implementation can be large.

The boundary remains understandable.

---

## What is a capability?

A **capability** is something a system can do, provide, understand, or make available.

Examples:

~~~text
"The system can calculate thermal behavior."

"The system can render geometry."

"The system can understand a material."

"The system can provide French language support."

"The system can expose a REST operation."

"The system can simulate a physical system."
~~~

The MicroBundle does not decide what those things mean.

**The capability author does.**

MicroBundleDomain provides the common boundary through which the capability can participate in a larger system.

---

## What is *not* a MicroBundle?

This distinction matters.

A MicroBundle is not simply:

- a folder in a project
- a namespace
- a NuGet package
- a DLL
- a microservice
- a GUI control
- a database record
- a manifest
- a repository

Those things may **contain, describe, transport, store, display, or deliver** a MicroBundle.

They are not automatically the capability boundary itself.

For example:

~~~text
NuGet
  -> can distribute the software that implements a MicroBundle

Repository
  -> can store or deliver a MicroBundle artifact

GUI
  -> can let a human configure a MicroBundle

Manifest
  -> can request a MicroBundle

Composition Host
  -> can assemble MicroBundles

MicroBundle
  -> is the capability being composed
~~~

Keeping those meanings separate is one of the reasons this package exists.

---

## A real-world example without code

Imagine an application that knows about buildings.

Someone creates a **Thermal Capability**.

The person who created it decides what thermal behavior means.

They might define:

~~~text
Thermal
  identity: Thermal
  version: 1.0
  needs: Material
  accepts: thermal configuration
  provides: thermal behavior
~~~

Now another person creates a **Material Capability**.

~~~text
Material
  identity: Material
  version: 3.0
  provides: material properties
~~~

The important thing is this:

**The application does not have to become a thermal engineer or a materials engineer just to put those capabilities together.**

The capabilities describe their own requirements.

The composition system works with the common contract.

That is the architectural idea.

---

## The three questions

A composable capability system deliberately separates three questions:

| Question | Boundary |
|---|---|
| **What is it?** | MicroBundleDomain |
| **Where can I get it?** | MicroBundleRepository |
| **How do I compose it?** | FSM_COS or another composition host |

### 1. What is it?

**MicroBundleDomain** defines the neutral capability contract.

It provides the vocabulary for identity, version, dependencies, providers, configuration, loading, and arbitration.

### 2. Where can I get it?

A **MicroBundleRepository** deals with artifacts.

It might use:

- a local filesystem
- Git
- HTTP
- Azure Blob Storage
- an object store
- a database
- a private enterprise service
- something completely different

The Domain package does not require any particular repository.

### 3. How do I compose it?

A **composition host** decides how capabilities become a runtime.

FSM_COS is one composition host.

Another organization can build another.

The MicroBundle contract does not belong to FSM_COS.

---

## Why does that separation matter?

Consider what happens if the host has to know every domain:

~~~text
if Thermal...
if Physics...
if Rendering...
if Materials...
if AEC...
if Magic...
if WhateverComesNext...
~~~

Every new capability becomes another special case.

The host becomes a giant encyclopedia of everybody else's domains.

MicroBundleDomain reverses that relationship.

Instead:

~~~text
Capability
    |
    v
MicroBundleDomain contract
    |
    v
Composition Host
~~~

The host learns **how to work with a MicroBundle**.

It does not have to learn what every MicroBundle means.

That is the **anti-if** idea behind the boundary.

---

## MicroBundle versus a plugin

A plugin usually asks:

> "How can I extend this particular host?"

A MicroBundle asks a broader question:

> "How can this capability identify itself, declare what it needs, accept configuration, load into a runtime, and participate in composition without becoming part of the host's domain model?"

A plugin can be implemented as a MicroBundle.

A MicroBundle does not have to belong to one particular plugin host.

---

## MicroBundle versus a microservice

A microservice is a distributed service boundary.

A MicroBundle is a capability composition boundary.

A MicroBundle can run entirely inside one process.

So **Micro** does not imply networking.

---

## MicroBundle versus NuGet

A NuGet package is primarily a **software distribution mechanism**.

A MicroBundle is a **runtime composition capability**.

They can work together:

~~~text
NuGet
  |
  +-- distributes implementation

MicroBundle
  |
  +-- represents capability

Repository
  |
  +-- delivers artifact

Composition Host
  |
  +-- assembles runtime
~~~

One does not replace the other.

---

## You can build your own ecosystem

The Singularity Workshop implementations are examples, not requirements.

A completely independent organization could create:

~~~text
ExampleCompany
  |
  +-- Thermal capabilities
  +-- Material capabilities
  +-- Rendering capabilities
  |
  +-- ExampleCompany.Repository
  |
  +-- ExampleCompany.Composition
~~~

They can keep their own domains, storage, delivery system, and composition host.

They only need to agree on the MicroBundle contract if they want interoperability through that contract.

That is an important property:

> **The contract is smaller than the ecosystem.**

You can adopt the contract without adopting everything around it.

---

## The passport analogy

Here is the mental model to keep:

> **A MicroBundle is a capability carrying its own passport.**

The passport answers:

~~~text
Who am I?
What version am I?
What do I require?
What do I expose?
How do I enter a runtime?
How do I respond to the capabilities around me?
~~~

The **repository** helps find and materialize the traveler.

The **composition host** decides how travelers are assembled.

The **application or Experience** decides what people ultimately see and do.

The passport is not the person.

The repository is not the person.

The airport is not the person.

The capability remains the capability.

---

## There are actually two audiences for the Domain package

This is an important distinction.

MicroBundleDomain contains two complementary kinds of contract:

### Runtime contract

For a composition host.

It describes the executable capability and its lifecycle:

~~~text
IMicroBundle
MicroBundleDescriptor
MicroBundleDependencyRequest
IMicroBundleLoadContext
IMicroBundleArbitrationContext
~~~

### Description contract

For editors and tooling.

It describes configurable structure without requiring the editor to execute the capability:

~~~text
MicroBundleDefinition
MicroBundleField
~~~

That means a future visual editor can ask:

> "What can I configure?"

without needing to understand the implementation of the capability.

A GUI can turn a string into a textbox.

A web editor can turn an integer range into a control.

A command-line tool can turn the same information into prompts.

**The Domain describes the meaning. The tool chooses the manifestation.**

---

## The architectural test

Here is the test that matters more than any particular class name:

> **Could another host consume this capability without learning the internal semantics of my domain?**

If yes, the boundary is doing useful work.

If the host must learn what Thermal, Element, Mesh, AEC, Magic, or some future domain means before it can compose the capability, the domain boundary has leaked.

---

## If you only remember five things

~~~text
1. A MicroBundle is a focused capability.

2. "Micro" means focused scope, not tiny code.

3. The capability owns its meaning.

4. The repository answers WHERE.
   The composition host answers HOW.

5. The common contract lets independently
   authored capabilities meet.
~~~

That is enough to continue.

You do not need to understand the code yet.

## Choose your next path

**I want to see it, not code it yet.**  
→ Continue through the [Adventures](ADVENTURES.md).

**I want to build one.**  
→ Read [Getting Started](GETTING_STARTED.md).

**I want to understand the architecture.**  
→ Read [Architecture](ARCHITECTURE.md).

**I want to understand why it was designed this way.**  
→ Read [Theory](THEORY.md).

**I want to know what each word means.**  
→ Read [Glossary](GLOSSARY.md).