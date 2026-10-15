# Soccer Match Fixture — first executable slice

This example moves the soccer work from a conceptual contract into a small runnable artifact while preserving clear ownership boundaries.

## What it demonstrates

- A soccer MicroBundle can implement `IMicroBundle` without adding FSM_COS or renderer dependencies to MicroBundleDomain.
- The fixture represents field zones, player roles, and per-player objectives separately.
- Off-ball support is intentional: a forward seeks a useful shooting lane while the ball carrier evaluates a pass option.
- Match events distinguish attempts from confirmed outcomes.
- The score changes only after the fixture's match authority emits `GoalAwarded`.
- A restart-ready state follows the goal.

## What it does not demonstrate

The attack outcome is scripted and deterministic. This slice does not calculate ball movement, contact, interception probability, goalkeeper reach, fouls, offside, physical movement, or poses. It is not a playable match and does not yet drive Gestures, Renderer, WebPage, or Singularity City.

## Next implementation seam

Replace the scripted pass and shot resolutions with explicit domain decisions and authoritative result records. Then test contested outcomes (interception, save, miss, out of play), restart rules, and offside at the moment a teammate plays the ball. Keep visual/pose completion separate from match truth.
