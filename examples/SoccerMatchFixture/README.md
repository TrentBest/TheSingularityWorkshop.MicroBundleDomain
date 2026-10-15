# Deterministic Soccer Match Fixture

This runnable example is the first executable domain fixture for the Soccer MicroBundle work.

It exercises a small, deterministic attack:

1. Both sides are ready.
2. Players have distinct roles and off-ball objectives.
3. The ball carrier identifies a useful support option rather than having every player chase the ball.
4. A pass is attempted and confirmed.
5. A shot is attempted and resolved by the fixture's match authority.
6. Only the confirmed goal outcome changes the score.
7. The match becomes ready for a restart.

The fixture implements the neutral `IMicroBundle` contract and declares soccer providers. It does not add a dependency from MicroBundleDomain to FSM_COS, Gestures, Renderer, or a GUI.

## Run

From the repository root:

```sh
dotnet run --project examples/SoccerMatchFixture/SoccerMatchFixture.csproj
```

## Scope

This is a deterministic domain example, not a complete soccer simulator. The match result is a deliberately scripted fixture outcome; it does not calculate ball physics, goalkeeper reach, collisions, fouls, offside, or animation. Those responsibilities must be implemented and validated in their appropriate domain/host boundaries before this is presented as live gameplay.
