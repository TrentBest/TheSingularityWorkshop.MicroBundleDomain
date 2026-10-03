# Adventure 6 — Arbitration

## The goal

Loading and arbitration are different.

Loading answers:

> "Install this capability."

Arbitration answers:

> "Now that I can see the other capabilities in this composition, do I need to react?"

## Why separate them?

Imagine:

~~~text
Thermal
Material
Physics
~~~

Thermal may load its own capability first.

Only after the composition is assembled can it discover that Material exposes thermal conductivity or that Physics provides a simulation context.

Thermal can then reconcile itself with those capabilities.

## The contract

~~~csharp
public bool Arbitrate(
    IMicroBundleArbitrationContext context,
    int roundIndex)
{
    foreach (var bundle in context.Bundles)
    {
        // Inspect the current composition.
    }

    return false;
}
~~~

The host owns the loop. The capability owns its response.

## The important rule

Return true only when your bundle actually changed something that requires another arbitration round.

That makes convergence an orchestration concern.

## The full model

~~~text
Describe
   |
Declare dependencies
   |
Repository finds/materializes
   |
Host loads
   |
Capabilities arbitrate
   |
Runtime emerges
~~~

You started with one class.

You now have a model for an ecosystem of independently authored capabilities.

## Continue

- [Adventures](../ADVENTURES.md)
- [Architecture](../ARCHITECTURE.md)
- [What Is a MicroBundle?](../WHAT_IS_A_MICROBUNDLE.md)
