# Knowledge Model: From Documentation to an Ideal Domain

The documentation is not merely a collection of explanations. It is a growing knowledge surface around the MicroBundle model.

The long-term goal is to make the same underlying facts understandable from many directions without changing the facts themselves.

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
~~~

These are different views of the same model.

The goal is not one impossibly comprehensive page. The goal is a navigable body of knowledge.

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

A sequence of actions designed to teach someone how to accomplish something. Tutorials should assume less knowledge than architecture documents.

### Example

A concrete capability such as Greeting, Thermal, Material, or Rendering. Examples make an abstract contract tangible without becoming the contract itself.

### Theory

The reasoning behind the boundaries. Theory explains why a design exists; it should not be confused with an API guarantee.

### Vocabulary

Definitions that keep terminology stable across documents.

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

## Documentation quality rule

When adding a document, ask:

1. Which concept does this teach?
2. Which fact supports it?
3. Which relationship does it clarify?
4. Who is the intended reader?
5. Where should the reader go next?
6. Could the same fact be useful to tooling later?

This keeps documentation growth deliberate rather than repetitive.

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

The goal is not to eliminate disagreement. The goal is to make the architecture understandable enough that adoption, extension, replacement, and informed disagreement can all happen from a common factual foundation.
