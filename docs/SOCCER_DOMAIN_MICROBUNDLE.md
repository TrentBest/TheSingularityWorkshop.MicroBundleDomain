# Soccer Domain MicroBundle — Design Contract

**Status: domain design and implementation boundary; not a playable soccer simulation.**

This document defines the proposed soccer-domain MicroBundle for The Singularity Workshop. It is intentionally separate from Gestures: soccer supplies the rules, vocabulary, field context, and authoritative outcomes; Gestures evaluates physical procedures requested by that domain.

## The capability

The soccer MicroBundle represents the soccer environment and match domain. It should expose a stable, inspectable description of what the environment permits without hard-coding one team's tactics or requiring the composition host to understand soccer internals.

The first slice should cover:

- Field identity, dimensions, goal locations, boundaries, and configurable logical zones.
- Field reservation and availability over a time interval, with an exclusive active-match constraint per field.
- Match readiness requirements: two eligible teams, roster requirements, and explicitly configured match prerequisites.
- Match state and team/player role references, separate from the field's static geometry.
- A domain-defined action vocabulary and contextual requirements for requesting each action.
- Tactical target candidates, including skill-dependent shooting opportunities, without treating a candidate as a guaranteed outcome.
- Explicit evidence contracts for ball contact, possession changes, goals, fouls, and offside decisions.

The initial implementation should model data and deterministic rule checks before attempting full simulation or agent intelligence.

## Object and state boundaries

| Object | Owns |
| --- | --- |
| Field | Geometry, zones, availability, reservation and exclusive occupancy |
| Match | Schedule, participating teams, eligibility, phase, score and match rules |
| Team | Team-level strategy and tactical intent |
| Player | Identity, role, capabilities, assignment and individual state |
| Procedure vocabulary | Actions the domain allows and their required inputs/conditions |
| Gesture evaluator | Evaluation of a requested physical procedure into pose data |
| Rig / Renderer | Mapping pose data to a representation and presenting it |
| Match authority | Ball physics, contact, possession, scoring, fouls and offside decisions |

A field being available does not mean a match is ready. A match being ready does not mean it has started. A player reaching a pose does not mean the intended soccer action succeeded.

## Field coordinates and tactical zones

The field should retain continuous geometry as the source of truth. Logical zones are a derived vocabulary for tactical reasoning, not a replacement for coordinates.

The initial tactical partition may divide each team's perspective into three longitudinal bands (back, middle, front) and three lateral channels (left, center, right). Directions are relative to the team's attacking direction, so the same definitions remain valid when teams switch ends.

Zone membership should be derived from field geometry and team direction. The goal surface and penalty areas remain explicit geometry rather than merely a grid label. The schema should allow alternate partitions later without requiring the pose evaluator to change.

## Scheduling and readiness

Use distinct states rather than a single CanPlay flag:

1. **Available** — no conflicting reservation or active match occupies the field for the requested interval.
2. **Reserved** — a booking has been accepted for the interval.
3. **NotReady** — a match exists, but one or more required conditions are not satisfied.
4. **Ready** — configured prerequisites are satisfied.
5. **InProgress** — the match authority has explicitly started play.
6. **Completed / Abandoned** — terminal match outcomes, distinguished from readiness.

The exclusivity rule must be enforced by the field/booking authority so two independent match requests cannot both claim the same field interval. A MicroBundle declaration alone does not provide transactional storage or distributed locking; those guarantees belong to the runtime service that owns reservations.

Roster completeness is a match prerequisite, not a field property. Make roster size and eligibility configurable rather than assuming every competition uses the same team size.

## Environment vocabulary, participant choice

The domain publishes available procedures; a team or player chooses among them according to role, strategy, skill, and current state.

Initial illustrative vocabulary:

- Move into a zone / create separation
- Receive or trap the ball
- Pass to a teammate or target area
- Dribble / carry the ball
- Shoot at a candidate target
- Clear the ball
- Mark an opponent, block a lane, intercept, or attempt a tackle
- Goalkeeper-specific positioning and save attempts

These names describe domain capabilities, not a claim that this repository already implements the full procedure catalog. Each capability needs a definition, valid parameters, contextual preconditions, and a way for the match authority to report the relevant outcome.

## Skill-dependent opportunity sets

Do not model skill only as a scalar rating or make skill synonymous with guaranteed success.

A player profile may influence the candidate set of actions and target positions the decision layer considers viable. For example, a more practiced shooter might evaluate a broader range of angles and distances. Candidate quality should separately account for the situation: distance, angle, defensive pressure, goalkeeper position, available time, and passing alternatives.

Keep these measures distinct:

- **Permitted opportunity** — the domain allows the attempt.
- **Player capability** — the player profile supports considering the attempt.
- **Situational value** — the decision layer estimates whether the attempt is worthwhile.
- **Physical execution** — Gestures evaluates the requested procedure and the host presents it.
- **Authoritative result** — the match simulation resolves contact, ball trajectory, save/interception, and scoring.

A wider candidate set improves choice; it does not promise a goal.

## Example: support play

Player A controls the ball. Player B should not automatically chase the ball. The team decision layer can ask for candidate support positions based on the ball, teammates, opponents, zones, passing lanes, and shooting opportunities. It assigns Player B a spatial objective; the procedure layer chooses a valid movement/receiving procedure; Gestures evaluates the pose; the match simulation later determines whether a pass was completed.

The defensive side uses the same field and state but different objectives: restrict forward space, close lanes, protect dangerous areas, and coordinate the defensive line. Offside must be judged by the match authority using player and ball positions at the relevant moment of a teammate's play/touch and the applicable rule conditions. A defensive movement or a player's position alone does not issue an offside decision.

## Integration boundary with Gestures

The dependency direction should be:

```text
Soccer Domain MicroBundle
  ├─ field and match rules
  ├─ action vocabulary and contextual constraints
  ├─ player/team context and candidate targets
  └─ authoritative outcome contract
            |
            v
     procedure request
            |
            v
         Gestures
            |
            v
      EvaluatedPose
            |
            v
     rig / Renderer host

Match authority <--- observations and physical outcomes
```

The diagram describes data flow, not a new dependency from Gestures back into the soccer domain. Gestures must remain reusable and engine/domain-neutral. FSM_API and FSM_COS may coordinate lifecycle and composition in a consuming experience; neither belongs as a required dependency of the soccer domain's core data model unless a concrete integration justifies it.

## First implementation acceptance criteria

1. A field cannot accept overlapping active-match reservations for the same interval; boundary and concurrency semantics are explicit.
2. Match readiness rejects incomplete/invalid team registrations without confusing that result with field availability.
3. Zone classification is deterministic, handles team attack direction, and tests boundaries.
4. Each participant retains independent identity, role, assignment, progress, and interruption state.
5. The provider rejects an action that is not in the environment's current vocabulary or whose preconditions are unmet.
6. The decision layer can return multiple candidate support/shooting targets and rank them without treating ranking as outcome.
7. Gesture completion alone cannot grant possession, award a goal, certify contact, or decide offside.
8. Missing, delayed, denied, and indeterminate outcome evidence remain explicit states.
9. Tests include opposing teams, role variation, failed passes, intercepted passes, missed shots, saves, offside adjudication inputs, unavailable field slots, and incomplete rosters.
10. Examples clearly label illustrative data and do not claim physical or match-engine validation until such a simulation exists.

## Delivery sequence

1. Implement immutable field geometry and configurable zone classification with unit tests.
2. Implement field scheduling and match-readiness state transitions with conflict tests.
3. Define the soccer vocabulary and typed request/observation contracts.
4. Build a deterministic consumer-side tactical fixture with independent agents and explicit candidate positions.
5. Connect procedure requests to Gestures' existing pose evaluator.
6. Add a host-owned ball/match outcome simulator and test against explicit evidence.
7. Only then consider a live visual demonstration and Renderer adapter.

The first deliverable is a small, testable domain capability—not an attempt to implement all of soccer at once.
