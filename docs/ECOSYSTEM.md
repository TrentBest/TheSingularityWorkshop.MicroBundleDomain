# MicroBundle Ecosystem Guide

## Layers, not one giant framework

The goal is not one enormous framework that owns everything.

The ecosystem is layered:

~~~text
                  EXPERIENCE / APPLICATION
                           ^
                           |
                    RuntimeAssembly
                           ^
                           |
                  Composition Host
                           ^
                           |
                 Repository / Catalog
                           ^
                           |
                  MicroBundle artifacts
                           ^
                           |
                  MicroBundleDomain
~~~

The domain contract is the narrow seam at the bottom.

## The portable core

### MicroBundleDomain

Defines the capability contract.

It should remain independent of storage, REST, GUI, cloud provider, application host, and composition implementation.

### MicroBundleRepository

Provides a way to locate and materialize capabilities.

It is an **implementation boundary**, not a required dependency of MicroBundleDomain.

A repository can be local, remote, public, private, centralized, federated, cloud-backed, or entirely custom.

### Composition host

A MicroBundle author should be able to write a capability without knowing which composition engine will consume it.

FSM_COS is the Singularity Workshop implementation. Another project can provide another host.

## A third-party ecosystem

Imagine ExampleWorks:

~~~text
ExampleWorks.MicroBundleRepository
ExampleWorks.Composition
ExampleWorks.Structural
ExampleWorks.HVAC
ExampleWorks.Electrical
ExampleWorks.BuildingGeometry
~~~

Their repository might use an enterprise object store. Their composition host might be a server, desktop application, or service.

Nothing requires those capabilities to become part of the Singularity Workshop ecosystem.

## The dependency rule

> **Depend downward on contracts; adapt upward at boundaries.**

Desired shape:

~~~text
                 MicroBundleDomain
                  ^       ^
                  |       |
            Repository   Host
                  |       |
                  +-- adapters --+
~~~

Not a domain package that references repository, FSM_COS, REST, Azure, or GUI.

## Repository is a delivery ecosystem

A repository can track:

~~~text
Identity
   |
Version
   |
Artifact
   |
Integrity
   |
Dependencies
   |
Materialization
~~~

It can also become a federation boundary:

~~~text
Repository A --+
Repository B --+--> Catalog --> Composition Host
Repository C --+
~~~

Different organizations can publish different capability families.

## NuGet and MicroBundle repositories are different

NuGet distributes software packages.

A MicroBundle repository provides runtime capability artifact/discovery/materialization.

They can cooperate:

~~~text
NuGet
  |
  +-- implementation assembly

MicroBundleRepository
  |
  +-- capability artifact/version/metadata

Composition Host
  |
  +-- runtime assembly
~~~

A MicroBundle repository can use another delivery mechanism when NuGet is not the right fit.

## The authoring story

A developer should be able to start with only:

~~~text
.NET
+
MicroBundleDomain
~~~

and create a capability.

Later:

~~~text
+ MicroBundleRepository
~~~

when delivery/discovery is needed.

Later still:

~~~text
+ a composition host
~~~

when runtime assembly is needed.

**Do not make a beginner understand the entire ecosystem before they can create their first useful thing.**

## The ecosystem promise

Someone invents a capability, implements the contract, publishes it through a repository, another system discovers it, a composition host assembles it, and an application manifests it.

The creator does not need to own every layer.

The consumer does not need to understand every layer.

**The contract connects them.**
