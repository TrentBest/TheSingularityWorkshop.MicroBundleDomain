# Adventure 5 — Build Your Own Ecosystem

## The goal

Replace the Workshop implementations while keeping the contract.

You write:

~~~csharp
public sealed class MyCapability : IMicroBundle
{
    // your domain
}
~~~

The only required foundation is MicroBundleDomain.

Then create your own repository:

~~~text
MyCompany.MicroBundleRepository
~~~

It can use whatever storage and delivery model your environment needs.

Then create your own composition host:

~~~text
MyCompany.CompositionHost
~~~

It decides dependency resolution, loading, arbitration, convergence, and runtime exposure.

## The resulting ecosystem

~~~text
                 MicroBundleDomain
                  ^       ^       ^
                  |       |       |
              Bundle  Repository  Host
                  |       |       |
                  +-------+-------+
                          |
                       Runtime
                          |
                          v
                      Experience
~~~

The implementations can belong to different organizations.

## The portability test

If a capability can move from Host A to Host B without being rewritten to understand Host B, the capability boundary is doing its job.

You are not adopting a giant framework.

You can adopt one layer at a time:

~~~text
Need a capability contract?
    -> MicroBundleDomain

Need artifact delivery?
    -> your repository

Need composition?
    -> your host

Need tooling?
    -> your tools

Need presentation?
    -> your application
~~~

## Next

[Adventure 6 — Arbitration](06-arbitration.md)
