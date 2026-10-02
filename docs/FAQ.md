# MicroBundle FAQ

This page answers the questions developers are likely to ask before deciding whether the model is useful.

## Is a MicroBundle a NuGet package?

No. NuGet distributes software. A MicroBundle is a runtime capability boundary.

A NuGet package can contain a MicroBundle implementation, while a MicroBundle repository can provide runtime artifacts.

## Is a MicroBundle a plugin?

It can be used as one, but it is not defined by a particular host. A MicroBundle is defined by its capability contract.

## Does Micro mean small?

No. **Micro means focused scope.**

A MicroBundle may have a substantial implementation. The boundary is focused; the implementation does not have to be tiny.

## Does every MicroBundle need a repository?

No.

A capability can be authored and used directly in code. A repository becomes useful when capabilities need discovery, delivery, persistence, caching, verification, or materialization.

The Domain package does not depend on a repository implementation.

## Does every MicroBundle need FSM_COS?

No. FSM_COS is one composition host.

Another host can implement its own dependency resolution, loading, arbitration, and runtime assembly rules while consuming the same MicroBundleDomain contract.

## What does a repository know?

A repository may know artifact identity, version, content or hash, storage location, retrieval information, caching, verification, and materialization.

It should not become the owner of the capability's domain meaning.

## Why not put repository access into MicroBundleDomain?

Because that would make delivery part of the capability contract.

The three questions remain separate:

~~~text
WHAT?  -> MicroBundleDomain
WHERE? -> MicroBundleRepository
HOW?   -> Composition Host
~~~

## Why does a MicroBundle have arbitration?

Loading and composition are different moments.

A capability can load its own behavior and later discover, during composition, that another capability changes what it should provide or how it should behave.

The host owns the arbitration loop. The capability owns its response.

## Is the configuration format defined here?

Only at the boundary.

Dependency requests carry opaque configuration bytes. The Domain does not dictate JSON, custom binary data, or any other serialization strategy.

## Is MicroBundleDefinition a GUI model?

No. It is a semantic description of configurable data.

A GUI, CLI, manifest editor, or future tool can decide how to present that information.

## What if I only want the Domain contract?

That is a valid adoption path.

Reference MicroBundleDomain, implement your capability, and build your own repository, composition host, tooling, and application around it.

## What if I want the Workshop ecosystem?

The layers can cooperate:

~~~text
MicroBundleDomain
      |
      v
MicroBundleRepository
      |
      v
FSM_COS
      |
      v
WebPage / AnyApp / another host
~~~

Each layer answers a different question.

## Where should I go next?

New to the model:
1. [What Is a MicroBundle?](WHAT_IS_A_MICROBUNDLE.md)
2. [Adventure 1](adventures/01-your-first-microbundle.md)
3. [Adventure 2](adventures/02-two-bundles.md)
4. [Adventure 3](adventures/03-your-own-repository.md)
5. [Getting Started](GETTING_STARTED.md)

Designing an ecosystem:
1. [Ecosystem Guide](ECOSYSTEM.md)
2. [Architecture](ARCHITECTURE.md)
3. [Theory](THEORY.md)
4. [Adventure 5](adventures/05-your-ecosystem.md)
5. [Adventure 6](adventures/06-arbitration.md)
