# Knowledge Model: From Documentation to an Ideal Domain

The documentation is not merely a collection of explanations. It is a growing knowledge surface around the MicroBundle model.

The long-term goal is to make the same underlying facts understandable from many directions without changing the facts themselves.

## The larger idea

There is a useful distinction between **knowledge** and **documentation**.

Documentation is written for people.

A knowledge domain is organized around stable concepts and relationships so that people **and, eventually, tools** can use the same underlying information.

This repository is not claiming that its Markdown files are already an Ideal Domain.

Instead, the documentation can be treated as a **candidate source for one** if the underlying concepts prove stable, useful, and carefully curated.

That distinction matters.

~~~text
Human understanding
        |
        v
Explanations
        |
        v
Examples / Tutorials / Theory
        |
        v
Repeated stable concepts
        |
        v
Curated factual model
        |
        v
Potential machine-readable domain
~~~

The documentation is therefore doing two jobs at once:

1. teaching humans;
2. exposing concepts that may later be curated into structured knowledge.

The second job should never be allowed to corrupt the first.

---

## One body of knowledge, many entry points

Different readers arrive with different questions:

~~~text
"What is a MicroBundle?" -> WHAT_IS_A_MICROBUNDLE
"Show me how."          -> GETTING_STARTED
"I learn by doing."     -> ADVENTURES
"Where is the repository?" -> ECOSYSTEM
"Why this architecture?"  -> THEORY
"What does this word mean?" -> GLOSSARY
"I have an objection." -> FAQ
"How could this become structured knowledge?" -> KNOWLEDGE_MODEL
~~~

These are different views of the same model.

The goal is not one impossibly comprehensive page.

The goal is a navigable body of knowledge in which a person can enter at the level they understand and move deeper without changing the underlying facts.

---

## The layers of understanding

A strong documentation system should allow the reader to move from intuition to precision:

~~~text
Level 1 — Human idea
    "A capability with a boundary."

Level 2 — Concrete example
    "Imagine Thermal and Material."

Level 3 — Vocabulary
    "Capability, dependency, repository, host."

Level 4 — Contract
    "IMicroBundle and its related types."

Level 5 — Architecture
    "WHAT / WHERE / HOW."

Level 6 — Theory
    "Why the dependency direction exists."

Level 7 — Independent creation
    "Build your own ecosystem."
~~~

A reader should be able to stop at any level and still retain something useful.

A developer may descend rapidly.

A non-coder may remain near the first three levels.

An architect may move repeatedly between levels.

A toolmaker may need the contract and description model without caring about the tutorial story.

---

## Facts, models, examples, and theory

A durable knowledge system should distinguish kinds of information.

### Contract facts

Facts checked directly against the package:

- interface members
- descriptor fields
- dependency request structure
- lifecycle methods
- field kinds
- documented dependency direction

### Architectural model

The intended relationships:

- Domain = WHAT
- Repository = WHERE
- Composition Host = HOW
- capability authors own domain meaning

### Tutorial

A sequence of actions designed to teach someone how to accomplish something.

Tutorials should assume less knowledge than architecture documents.

### Example

A concrete capability such as Greeting, Thermal, Material, or Rendering.

Examples make an abstract contract tangible without becoming the contract itself.

### Theory

The reasoning behind the boundaries.

Theory explains why a design exists; it should not be confused with an API guarantee.

### Vocabulary

Definitions that keep terminology stable across documents.

### Curation

A future knowledge-domain layer should not blindly ingest every sentence in every document.

Curation should identify:

- stable concepts
- explicit relationships
- verified facts
- version-sensitive facts
- examples
- hypotheses or design theory

That prevents a colorful explanation from accidentally becoming an asserted fact.

---

## Documentation as an ontology seed

An ontology becomes useful when concepts have stable identities and explicit relationships.

The current model already has relationships such as:

~~~text
MicroBundle
  +-- has Identity
  +-- has Version
  +-- declares Dependency
  +-- exposes Provider
  +-- accepts Configuration
  +-- performs Load
  +-- participates in Arbitration

MicroBundleRepository
  +-- discovers Artifact
  +-- retrieves Artifact
  +-- materializes MicroBundle

CompositionHost
  +-- resolves Dependencies
  +-- loads MicroBundles
  +-- runs Arbitration
  +-- produces RuntimeAssembly
~~~

This is useful before it becomes machine-readable because it gives future tooling a stable vocabulary to preserve.

---

## Concise facts do not replace abundant explanations

A concise factual representation and a rich teaching surface solve different problems.

~~~text
                 Ideal Domain
                      |
          +-----------+-----------+
          |           |           |
        Facts      Relations   Vocabulary
          |           |           |
          +-----------+-----------+
                      |
              Human explanations
          +-----------+-----------+
          |           |           |
       Tutorials   Examples     Theory
          |           |           |
          +-----------+-----------+
                      |
                 Applications
~~~

The factual layer can remain concise while the educational layer grows.

That means documentation can become richer without corrupting the underlying facts.

This is especially important for a project intended to be understood by people who do not share the author's implementation background.

---

## A future machine-readable form

The current Markdown is intentionally human-first.

A later representation could describe concepts explicitly:

~~~text
Concept: MicroBundle
  type: Capability
  hasIdentity: true
  hasVersion: true
  declaresDependency: MicroBundle
  supportsConfiguration: true
  lifecycle: Load, Arbitrate

Concept: MicroBundleRepository
  type: ArtifactSystem
  discovers: MicroBundleArtifact
  materializes: MicroBundle

Relation:
  MicroBundleDomain defines MicroBundle
  MicroBundleRepository delivers MicroBundleArtifact
  CompositionHost composes MicroBundle
~~~

The machine-readable model should be derived from stable concepts, not invented separately from them.

A future curation process could also preserve provenance:

~~~text
Concept
  |
  +-- factual definition
  +-- source document
  +-- implementation evidence
  +-- version
  +-- related concepts
  +-- examples
  +-- explanatory views
~~~

That would allow a concise knowledge representation to remain connected to the richer explanations that taught the concept in the first place.

---

## Documentation quality rule

When adding a document, ask:

1. Which concept does this teach?
2. Which fact supports it?
3. Which relationship does it clarify?
4. Who is the intended reader?
5. Where should the reader go next?
6. Could the same fact be useful to tooling later?
7. Is this a fact, example, explanation, theory, or hypothesis?
8. If this sentence became structured knowledge later, would we still stand behind its meaning?

That last question is deliberately difficult.

It is a quality filter.

---

## The end state

The documentation should eventually let a person encounter the project from almost any reasonable question and find a path forward:

~~~text
Question
   |
Explanation
   |
Example
   |
Tutorial
   |
Implementation
   |
Architecture
   |
Theory
   |
Independent creation
~~~

The goal is not to eliminate disagreement.

The goal is to make the architecture understandable enough that adoption, extension, replacement, and informed disagreement can all happen from a common factual foundation.

If the documentation succeeds exceptionally well, the explanations themselves become raw material for something more structured:

**a curated domain of knowledge about the concepts the Workshop has learned to make explicit.**

That possibility is a reason to be unusually careful with terminology, relationships, examples, provenance, and the distinction between fact and explanation.
