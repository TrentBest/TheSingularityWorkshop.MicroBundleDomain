# Adventure 3 — Put Your Capabilities in a Repository

## The goal

Separate **authoring** from **delivery**.

~~~text
Author
  |
  v
MicroBundle artifact
  |
  v
Repository
  |
  v
Consumer
  |
  v
Composition
~~~

A repository does not make something a MicroBundle. The capability is already a MicroBundle. The repository gives other systems a way to find and materialize it.

## What a repository can know

A repository may track identity, version, artifact identity, content/hash, dependencies, location, retrieval information, and materialization.

It may also provide discovery, caching, verification, and access control.

## Build your own repository ecosystem

Nothing requires you to use the Singularity Workshop repository.

Your repository might use:

~~~text
filesystem
Git
S3
Azure Blob
database
private artifact service
~~~

The domain contract should not change.

## Why this is powerful

Different organizations can publish different capability families:

~~~text
Repository A -- mechanical
Repository B -- architectural
Repository C -- simulation
           |
           v
        Catalog
           |
           v
     Composition Host
~~~

The host does not need to become the owner of those domains.

## The architectural trap

Do not make:

~~~text
MicroBundleDomain
      |
      v
Repository
~~~

That would force every capability author to adopt one delivery implementation.

Instead:

~~~text
MicroBundleDomain
   ^       ^
   |       |
Bundle  Repository
~~~

Both understand the domain contract. Neither requires the other to define what a MicroBundle means.

## NuGet is different

NuGet can distribute the code implementing your capability.

A MicroBundle repository can provide runtime artifact discovery/materialization.

They can cooperate without being the same system.

## Next

[Adventure 4 — Make It Configurable](04-configurable-bundles.md)
