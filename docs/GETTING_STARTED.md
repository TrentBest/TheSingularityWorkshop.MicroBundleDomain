# Getting Started

This guide answers the practical question:

> **How do I turn one of my capabilities into a MicroBundle that another composition host can use without taking a dependency on that host?**

## 1. Add the package

The intended package reference is:

```xml
<PackageReference Include="TheSingularityWorkshop.MicroBundleDomain"
                  Version="2.0.0-alpha.1" />
```

> The corrected `2.0.0-alpha.1` source is currently staged for release review. Use the version available in your configured package source when it is published.

The package targets .NET 8 and has no dependency on FSM_COS, a GUI framework, REST, storage, or a host application.

## 2. Define your capability

Start with `IMicroBundle`.

A minimal capability looks like this:

```csharp
using TheSingularityWorkshop.MicroBundleDomain;

public sealed class GreetingMicroBundle : IMicroBundle
{
    public MicroBundleDescriptor Descriptor { get; } =
        new(
            id: 1001,
            version: "1.0.0",
            providers:
            [
                new MicroBundleProvider("greeting")
            ]);

    public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; } =
        [];

    public void Load(IMicroBundleLoadContext context)
    {
        // Initialize the capability for this runtime.
    }

    public bool Arbitrate(
        IMicroBundleArbitrationContext context,
        int roundIndex)
    {
        // Return true when this capability changed the composition
        // and another arbitration round is required.
        return false;
    }
}
```

You now have a capability with a domain-owned identity and lifecycle.

## 3. Give it dependencies

Suppose the greeting capability needs a localization capability:

```csharp
public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; } =
[
    MicroBundleDependencyRequest.Unconfigured(2001)
];
```

Or provide configuration bytes with the request:

```csharp
public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; } =
[
    new(
        BundleId: 2001,
        Configuration: configurationBytes)
];
```

The important distinction is that your bundle **declares the dependency**.

It does not reach into the host and load it itself.

## 4. Read host-supplied configuration

The host supplies an `IMicroBundleLoadContext`.

Your bundle can request its configuration:

```csharp
public void Load(IMicroBundleLoadContext context)
{
    if (!context.TryGetConfiguration(
        Descriptor.Id,
        out var configuration))
    {
        return;
    }

    // Interpret configuration according to your domain/protocol.
}
```

MicroBundleDomain deliberately treats the configuration as opaque bytes.

That keeps serialization and protocol choices outside the domain contract.

## 5. Participate in arbitration

Loading establishes the capability in the runtime.

Arbitration lets the capability respond to the **composition around it**.

```csharp
public bool Arbitrate(
    IMicroBundleArbitrationContext context,
    int roundIndex)
{
    foreach (var bundle in context.Bundles)
    {
        // Inspect participating capabilities.
    }

    return false;
}
```

Return `true` only when your bundle actually changed something that requires another composition round.

The host owns the arbitration loop. The MicroBundle owns its response to the composition.

## 6. Add an editor definition when your capability is configurable

If your capability has data that tooling should be able to inspect, define a `MicroBundleDefinition`:

```csharp
var definition = new MicroBundleDefinition(
    name: "Greeting",
    descriptor: bundle.Descriptor,
    fields:
    [
        new MicroBundleField(
            name: "message",
            kind: MicroBundleFieldKind.String,
            defaultValue: "Hello"),

        new MicroBundleField(
            name: "repeat",
            kind: MicroBundleFieldKind.Integer,
            defaultValue: 1,
            minimum: 1,
            maximum: 10)
    ]);
```

A tooling or GUI package can inspect this schema without knowing what a Greeting actually does.

## 7. Keep the domain package independent

This is the rule that makes the package useful.

Your MicroBundle implementation should not need references to:

- FSM_COS
- MicroBundleRepository
- FSM_REST
- Blazor
- WPF
- Unity
- Azure
- a particular application

The capability should depend on the **contract**, not the host.

```text
                    ┌──────────────────────┐
                    │ Your Domain Package  │
                    │                      │
                    │ GreetingMicroBundle  │
                    └──────────┬───────────┘
                               │ implements
                               ▼
                    ┌──────────────────────┐
                    │   MicroBundleDomain  │
                    └──────────┬───────────┘
                               ▲
                               │ consumes
                    ┌──────────┴───────────┐
                    │    Composition Host  │
                    │      FSM_COS/etc.    │
                    └──────────────────────┘
```

This means the same domain capability can be consumed by more than one host.

## 8. Understand what happens after your package

MicroBundleDomain does not perform the whole composition operation.

A typical host pipeline is:

```text
Manifest
   │
   ▼
Repository / catalog
   │
   ▼
MicroBundle instances
   │
   ▼
dependency closure
   │
   ▼
Load
   │
   ▼
Arbitration
   │
   ▼
RuntimeAssembly
```

In the Singularity Workshop ecosystem, FSM_COS performs that orchestration.

Your domain package does not need to know that.

## A useful mental model

Think of a MicroBundle as a **capability that carries its own passport**.

The passport answers:

- Who are you?
- What version are you?
- What capabilities do you require?
- What do you expose?
- How do I install you?
- How do you participate when you meet other capabilities?

The composition host can then work with the passport without becoming an expert in the capability's internal domain.

## Next steps

- Read **[Architecture](ARCHITECTURE.md)** to understand why the dependencies point in these directions.
- Read **[MicroBundle Domain Theory](THEORY.md)** for the larger model.
- Look at the independent runtime contract tests in `tests/MicroBundleDomain.Tests`.
- Look at FSM_COS for one composition-host implementation of the contracts.
