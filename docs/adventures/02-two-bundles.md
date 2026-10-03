# Adventure 2 — Make Two Capabilities Meet

## The goal

Create:

~~~text
Greeting
   |
   +-- requires --> Language
~~~

## Why not put the dependency in the host?

A host branch such as `if (bundle is GreetingMicroBundle)` works once. Add Thermal, Rendering, Physics, Materials, AEC, and Audio and the host becomes a catalog of domain knowledge.

MicroBundles reverse that responsibility.

## Create Language

~~~csharp
public sealed class LanguageMicroBundle : IMicroBundle
{
    public MicroBundleDescriptor Descriptor { get; } =
        new(
            id: 2001,
            version: "1.0.0",
            providers:
            [
                new MicroBundleProvider("language")
            ]);

    public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; } =
        [];

    public void Load(IMicroBundleLoadContext context)
    {
    }

    public bool Arbitrate(
        IMicroBundleArbitrationContext context,
        int roundIndex)
    {
        return false;
    }
}
~~~

## Make Greeting require Language

~~~csharp
public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; } =
[
    MicroBundleDependencyRequest.Unconfigured(2001)
];
~~~

The relationship is now inside the capability model.

## The repository enters the picture

The host eventually asks:

> Where do I get Language?

That is not MicroBundleDomain's job.

The repository answers it:

~~~text
Greeting request
       |
       v
Repository
       |
       v
Language artifact
       |
       v
MicroBundle instance
~~~

## Complete flow

~~~text
Manifest / request
       |
       v
Repository / catalog
       |
       v
Greeting
       |
       +-- requires --> Language
                            |
                            v
                     dependency closure
                            |
                            v
                          Load
                            |
                            v
                       Arbitration
                            |
                            v
                    Runtime Assembly
~~~

The exact orchestration is host-specific. The capability contract is not.

## What you learned

~~~text
MicroBundleDomain = what
Repository         = where
Composition Host   = how
~~~

Keep those questions separate and the architecture stays extensible.

## Next

[Adventure 3 — Put Your Capabilities in a Repository](03-your-own-repository.md)
