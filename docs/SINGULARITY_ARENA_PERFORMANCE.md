# Singularity Arena — Shared Venue and Concert Event Fixture

**Status: executable reference fixture.** This example demonstrates shared venue scheduling for soccer, concerts, market fairs, and craft fairs, plus a deterministic concert cue-to-audience-intention projection. It does not implement a live stadium, persistent scheduling, ticketing, audio generation, physics, crowd locomotion, Gesture evaluation, or Renderer integration.

## Why a shared Arena contract

Singularity Arena is the venue; soccer, concerts, karaoke, market fairs, and craft fairs are event types. The venue schedule should not become a soccer-only object, and any event provider should not need to redefine venue identity or scheduling rules.

The example introduces:
- a shared in-memory **ArenaSchedule**;
- event records with stable ID, venue ID, event kind, provider ID, start/end times, and lifecycle state;
- same-venue overlap prevention and unique event IDs;
- validated event lifecycle transitions;
- a concert cue timeline using monotonic cue sequence and event-relative time;
- a deterministic projection from authoritative concert cues to individual audience intentions;
- stable per-Digiten reaction delay metadata and duplicate-cue suppression;
- tests for schedule boundaries, lifecycle, cue order, replay, and individual response variation.

The event provider IDs are independent: the soccer fixture schedules **soccer.match**; the concert fixture schedules **concert.performance**; market and craft fairs use **market.fair** and **craft.fair**. The scheduler knows the event kind and provider identifier, not the internal meaning of a goal, chorus, instrument, stall, or demonstration.

For the wider Experience catalog and the optional Economy boundary, see [Experiences Beyond the Arena](EXPERIENCES_AND_OPTIONAL_ECONOMY.md). A fair can run without Economy: the shared schedule declares no Economy dependency, and economic interactions are opt-in adapters owned outside the neutral scheduler.

## Run the fixture

With the .NET 8 SDK, run:

    dotnet run --project examples/SingularityArenaFixture/SingularityArenaFixture.csproj
    dotnet test tests/MicroBundleDomain.Tests/MicroBundleDomain.Tests.csproj --configuration Release

The console fixture schedules a soccer match, concert, market fair, and craft fair in separate time windows at the same venue, then walks a deterministic concert cue sequence through five distinct Digiten profiles. The output includes per-person intention and stable presentation-delay metadata.

## Audience intent is not physical movement

The concert director selects an intention such as **SingAlong**, **Clap**, **Sway**, **DanceInPlace**, **ListenQuietly**, or **WatchQuietly**. It does not animate a person or evaluate a pose. The next stage should map these intentions to provider-supplied Gesture definitions and anchors, then use the real Gestures CPU evaluator to produce evaluated pose data.

The layers stay separate:

1. The event/performance owner emits a cue such as **ChorusStarted**.
2. The participant policy chooses an intention using stable identity and profile context.
3. A Gesture provider selects a physical procedure and interaction anchors.
4. Gestures evaluates the requested procedure at the participant's own progress.
5. The host/world simulation reports actual movement, contact, obstruction, or other outcomes.
6. Audio and music systems own voice, sound, beat/phrase timing, and mixing.
7. Renderer/Visualize present the resulting state at an appropriate fidelity.

A chorus cue does not prove that the performer sang; a clap intention does not prove that hands made contact on the beat; and a completed Gesture timeline does not prove that the concert ended. Event lifecycle remains explicit and separate.

## Current limits and next steps

This is a deterministic fixture, not a complete MicroBundle-powered production service:
- The schedule is in memory and does not persist or dispatch wall-clock events.
- The schedule prevents overlap for a venue, but does not yet model multiple stages/spaces, capacity, seats, admission, vendor/stall records, inventory, or ticketing.
- The cue timeline is scripted and does not read a song file or an audio clock.
- The audience decision policy is deliberately small; it does not yet model local social influence, view obstruction, accessibility constraints, fatigue, or participant arrival/exit.
- Reaction delay is metadata. No scheduler waits on it.
- Intentions are not evaluated poses. The fixture does not reference the Gestures package and preserves the neutral MicroBundleDomain dependency direction.
- No authoritative movement, contact, audio, or crowd simulation is claimed.

The next useful milestone is to add venue-space and participant-presence contracts, then connect the concert intentions to real Gesture definitions in the Gestures consumer fixture. Only after that should the host add audio timing and Renderer presentation adapters.
