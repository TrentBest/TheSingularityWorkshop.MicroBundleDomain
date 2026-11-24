# Typed Provider Contracts and Preparation

## Purpose

A provider declaration is an opaque statement that a MicroBundle exposes a capability. The domain contract records identity and version without defining the capability's meaning or selecting a dependency-injection container.

A provider declaration can be versioned:

```csharp
new MicroBundleProvider("diegetic-gui", "1.2.0")
```

The one-argument constructor remains valid for legacy opaque declarations. An unversioned declaration cannot be used for exact typed provider resolution.

## Exact provider identity

A `MicroBundleProviderReference` pins four values:

- owning MicroBundle ID;
- owning MicroBundle version;
- provider ID;
- provider contract version.

The preparation context exposes a deterministic, immutable `Providers` snapshot of version-pinned declarations across the resolved dependency closure. Legacy unversioned declarations remain visible in descriptors but are omitted from this exact-resolution catalogue. A reference must match a declaration in the resolved composition; a matching name alone is insufficient.

## Host-owned resolution

The host implements `IMicroBundleProviderResolver`. It resolves a provider reference into the requested CLR capability type:

```csharp
bool TryResolve<TProvider>(
    MicroBundleProviderReference reference,
    out TProvider? provider)
    where TProvider : class;
```

The domain package does not own a global registry, service locator, GUI framework, or provider implementation.

## Optional preparation lifecycle

A MicroBundle may implement `IMicroBundlePreparable`. It can inspect `context.Providers` to discover available, version-pinned declarations before requesting a typed handle:

```csharp
public void Prepare(IMicroBundlePreparationContext context)
{
    if (context.TryGetProvider<IDiegeticGuiCapability>(
        providerReference,
        out var gui))
    {
        // Retain the typed host capability for the later Load/host handoff.
    }
}
```

The example interface is illustrative and is not supplied by MicroBundleDomain.

The composition host resolves and validates the complete dependency closure first. It then calls each preparable bundle once, in deterministic bundle-ID order, before calling any bundle's `Load`. Preparation is for validation and acquiring typed handles; it must not perform irreversible external side effects.

Bundles that do not implement `IMicroBundlePreparable` remain fully compatible. Hosts without a provider resolver simply return `false` for typed provider lookups.

## Responsibility boundaries

| Concern | Owner |
|---|---|
| Provider ID and exact contract version | MicroBundleDomain |
| Typed provider implementation and resolution | Host / adapter |
| Dependency closure and preparation order | Composition host |
| GUI tools, stations, rendering, and creator policy | GUI / authoring host |
| Bundle installation and arbitration | MicroBundle lifecycle and composition host |

MicroBundleDomain deliberately does not define `IDiegeticGUI` or any GUI-specific type. A GUI contract belongs in the relevant GUI/authoring integration layer; the domain only provides the neutral, typed seam through which a host can resolve it.
