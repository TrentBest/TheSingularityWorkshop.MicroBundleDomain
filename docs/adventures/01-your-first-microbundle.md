# Adventure 1 — Your First MicroBundle

## What you are building

You will create a capability called `GreetingMicroBundle`.

It will do almost nothing. That is intentional. The first adventure is about understanding the boundary, not building a complicated application.

By the end:

~~~text
Your project
    |
    +-- GreetingMicroBundle
             |
             +-- implements IMicroBundle
~~~

## Before you start

You need .NET 8, a text editor or IDE, and a terminal.

You do **not** need FSM_COS, a database, Azure, REST, Blazor, WPF, or Unity.

## Step 1 — Create the project

~~~bash
dotnet new classlib -n MyFirstMicroBundle
cd MyFirstMicroBundle
~~~

This is an ordinary .NET library. MicroBundleDomain does not require a special project type.

## Step 2 — Add MicroBundleDomain

After `2.0.0-alpha.1` is intentionally published:

~~~bash
dotnet add package TheSingularityWorkshop.MicroBundleDomain --version 2.0.0-alpha.1
~~~

## Step 3 — Create the capability

Create `GreetingMicroBundle.cs`:

~~~csharp
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
    }

    public bool Arbitrate(
        IMicroBundleArbitrationContext context,
        int roundIndex)
    {
        return false;
    }
}
~~~

Build it:

~~~bash
dotnet build
~~~

You have authored your first MicroBundle.

## Step 4 — Understand the contract

Identity gives the capability a stable ID.

Version identifies the capability version.

Providers identify what it exposes without making the domain package interpret that meaning.

Dependencies say what other capabilities are required.

Load installs the capability into a runtime.

Arbitrate lets the capability react to the composition around it.

## Step 5 — Give it a dependency

Suppose Greeting needs Language:

~~~csharp
public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; } =
[
    MicroBundleDependencyRequest.Unconfigured(2001)
];
~~~

Greeting declares the requirement. It does not reach into the host and call a Language service.

## Step 6 — The big realization

Your `GreetingMicroBundle` can compile without knowing:

- who will run it
- where it will be stored
- how it will be transported
- which GUI will display it
- which composition engine will load it

That is the point.

You authored a capability, not a host plugin.

## Next

[Adventure 2 — Make Two Capabilities Meet](02-two-bundles.md)
