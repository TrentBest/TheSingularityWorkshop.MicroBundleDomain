# Adventure 7 — From Artifact to Runtime

## The goal

See the complete journey of a capability from authoring to execution.

~~~text
Author
  |
  v
MicroBundle implementation
  |
  v
Artifact
  |
  v
Repository
  |
  v
Materialization
  |
  v
Composition Host
  |
  v
Runtime Assembly
  |
  v
Application / Experience
~~~

## Step 1 — Author

A developer implements the MicroBundle contract.

The capability owns its meaning and behavior.

It does not need to know whether the eventual consumer is WebPage, AnyApp, a desktop application, or another runtime.

## Step 2 — Package or materialize

The implementation can be distributed as ordinary software or represented as a repository artifact.

This is where the distinction between **software distribution** and **runtime capability distribution** becomes important.

~~~text
NuGet
  -> distributes implementation code

MicroBundleRepository
  -> discovers and materializes capability artifacts
~~~

They can cooperate, but they answer different questions.

## Step 3 — Repository

The composition process needs a capability.

The repository answers:

> Where can I get it?

A repository implementation may search local storage, Git, object storage, HTTP, Azure Blob, or another system.

The Domain contract does not care which.

## Step 4 — Materialization

The repository turns the selected artifact into something the composition host can consume.

The exact materialization mechanism belongs to the repository and host integration.

## Step 5 — Composition

The composition host answers:

> How do these capabilities become a runtime?

It resolves the requested dependency closure, loads the capabilities, and runs arbitration.

FSM_COS is one implementation of this role.

## Step 6 — Experience

The resulting Runtime Assembly can be exposed to an application or Experience.

The application decides what users see and do.

The capability did not have to become application-specific to get there.

## The architectural realization

You can replace any implementation boundary without redefining the capability contract:

~~~text
                  MicroBundleDomain
                         |
             +-----------+-----------+
             |           |           |
          Bundle A    Repository A  Host A
             |           |           |
             +-----------+-----------+
                         |
                       Runtime

                         OR

                  MicroBundleDomain
                         |
             +-----------+-----------+
             |           |           |
          Bundle B    Repository B  Host B
             |           |           |
             +-----------+-----------+
                         |
                       Runtime
~~~

The contract remains the seam.

## What you learned

A MicroBundle ecosystem is not one application.

It is a set of independently replaceable boundaries:

- capability
- artifact repository
- composition host
- presentation/application

That is why the Domain package must remain neutral.

## Continue

- [Adventure 6 — Arbitration](06-arbitration.md)
- [FAQ](../FAQ.md)
- [Ecosystem Guide](../ECOSYSTEM.md)
