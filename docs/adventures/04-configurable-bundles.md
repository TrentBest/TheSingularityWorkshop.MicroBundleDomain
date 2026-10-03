# Adventure 4 — Make It Configurable

## The goal

Let a tool ask:

> What can I configure on this capability?

without loading a GUI-specific framework into your domain.

## Define a schema

~~~csharp
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
~~~

The definition says:

~~~text
Greeting
 +-- message : String
 +-- repeat  : Integer [1..10]
~~~

## Why this is not a GUI

MicroBundleDomain does not say "render a textbox."

It says:

~~~text
this is a string
this is an integer
this has a range
~~~

A GUI, CLI, web editor, or future tool decides how to manifest that information.

## Nested data

Objects can contain children:

~~~text
Material
 +-- name
 +-- thermal
      +-- conductivity
      +-- expansion
~~~

The schema remains recursively inspectable.

## Next

[Adventure 5 — Build Your Own Ecosystem](05-your-ecosystem.md)
