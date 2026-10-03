# MicroBundle Glossary

A shared vocabulary prevents documentation, implementation, and future data models from drifting apart.

## Arbitration

A composition phase in which loaded capabilities inspect the assembled context and determine whether they need to change something.

The composition host owns the loop. The MicroBundle owns its response.

## Artifact

A stored or transported representation of a capability.

## Capability

A coherent unit of behavior or meaning that can be independently identified and composed.

## Composition

The act of assembling multiple capabilities into a runtime that operates as a coherent whole.

## Composition Host

The runtime system that resolves dependencies, loads capabilities, runs arbitration, and exposes the resulting assembly.

FSM_COS is one composition host.

## Configuration

Data supplied to a capability to determine how it operates.

MicroBundleDomain carries configuration at the contract boundary without prescribing its serialization format.

## Dependency

A declaration that one capability requires another capability to participate in its composition.

## Domain

The meaning and behavior owned by a capability author.

## Experience

A host-level manifestation of an assembled runtime, potentially providing presentation, interaction, sensory behavior, or application-specific meaning.

## Micro

In MicroBundle, **Micro means focused scope**, not binary size and not network topology.

## MicroBundle

A focused, independently identifiable capability that can declare dependencies, accept configuration, load into a host-provided context, and participate in composition arbitration.

## MicroBundleDefinition

A semantic, inspectable description of configurable capability data. It is not a GUI framework.

## MicroBundleDomain

The package defining the neutral MicroBundle contract and related domain-side structures.

It answers: **What is this capability?**

## MicroBundleRepository

A system that discovers, retrieves, stores, verifies, caches, or materializes MicroBundle artifacts.

It answers: **Where can I get this capability?**

## Manifest

A host or application description of the capabilities and configuration requested for a runtime or experience.

A manifest is an input to composition, not the definition of a MicroBundle.

## Materialization

Turning a stored or transported artifact into an executable or otherwise usable MicroBundle representation.

## Provider

A named capability surface declared by a MicroBundle. The Domain records provider identity without assigning domain-specific semantics to provider names.

## Runtime Assembly

The composed set of loaded capabilities produced by a composition host.

## Serialization

Representation of structured data as bytes or another transport form. Serialization is outside the core Domain contract.

## Transport

The mechanism used to move artifacts or requests between systems. REST is one transport option, not a Domain requirement.

## The three questions

~~~text
WHAT?  -> MicroBundleDomain
WHERE? -> MicroBundleRepository
HOW?   -> Composition Host
~~~
