# Soccer Match Experience — From Domain Decisions to a Living Arena

**Status: experience architecture and implementation plan. This document does not claim that a soccer simulation, crowd system, stadium renderer, audio engine, or live arena already exists.**

The goal is to stage a convincing soccer match inside Singularity City. The first milestone is not a complete sports simulator. It is a small, coherent match in which decisions have visible consequences and the presentation consistently reflects the authoritative match state.

## The feeling we are trying to create

A spectator should be able to glance at the pitch and understand:

- which teams are playing, the score, the time, and the current phase;
- where the ball is, who has it, and which direction each team attacks;
- why players are moving even when they are nowhere near the ball;
- when an attack is building, when danger is increasing, and why a chance opened or closed;
- whether a pass succeeded, a shot was saved, play was stopped, or a goal was awarded.

The spectacle should emerge from the match rather than from unrelated animation. A celebration follows an awarded goal; a goalkeeper reaction follows a shot outcome; the crowd's strongest reaction follows a meaningful event. Presentation must never invent a goal, possession change, foul, or offside decision.

## System boundaries

| Layer | Responsibility | Must not pretend to own |
|---|---|---|
| Arena / venue | Pitch geometry, goals, markings, stands, lighting, venue identity | Match rules or player decisions |
| Soccer domain MicroBundle | Field availability, match setup, teams, player roles, legal action vocabulary, tactical context | Rendering or physical truth without an authoritative simulation |
| Tactical decision layer | Select role-appropriate objectives and rank candidate actions/positions | Guaranteed pass, shot, or goal outcomes |
| Gestures | Evaluate requested movement/action procedures into pose data | Ball contact, possession, scoring, offside adjudication |
| Match authority | Ball movement, contact, possession, rule decisions, score, clock and restart state | Visual style or camera composition |
| Renderer / host rig | Map evaluated poses to a concrete rig and present the scene | Inventing a public rig contract before one is agreed |
| Presentation director | Choose camera, sound, crowd cues, scoreboard and replay in response to confirmed events | Changing the authoritative match state |

These boundaries can be implemented in stages. They are not a claim that each layer currently exists as a working package.

## The minimum convincing match

Start with a controlled demonstration, not an entire league.

1. **Venue is ready.** A clearly marked pitch, two goals, stands, team colors, a scoreboard, a match clock, and a camera with a useful overview.
2. **Teams take the field.** Players enter, occupy distinct starting positions, and orient toward the correct goal. A whistle or other explicit start event transitions the match into play.
3. **The ball is live.** The ball has an authoritative position and velocity. Possession belongs to a player or is explicitly loose; it is never inferred solely from animation.
4. **An attack develops.** The carrier evaluates forward progress, passing options, pressure, and shooting opportunities. Teammates seek space and support positions rather than crowding the ball.
5. **Defense responds.** Defenders mark, cover lanes, pressure the carrier, recover toward goal, or coordinate a line. The goalkeeper positions relative to the ball and likely threat.
6. **An outcome resolves.** A pass may arrive, be missed, or be intercepted. A shot may miss, hit the frame, be saved, or score. The match authority decides which occurred.
7. **The venue reacts.** The camera tracks the meaningful action; players react to the actual outcome; crowd intensity, commentary cues, and scoreboard changes follow confirmed events.
8. **Play restarts correctly.** Goals, touchline exits, fouls, and other stoppages transition to explicit restart states. Players reset to appropriate positions before play resumes.

This loop should work end to end before adding many more players, advanced tactics, or cinematic effects.

## Make off-ball movement meaningful

The important visual signal is not simply that all players move. It is that each player appears to have a reason to move.

For each player, keep independent state such as:

- team, role, identity and current assignment;
- current objective and intended destination;
- current action/procedure and progress;
- stamina or effort budget, if modeled;
- awareness or information available to that player;
- interruption/recovery state;
- current relationship to the ball, teammates, opponents, zones and goal.

For example, while Player A carries the ball, Player B may create width, Player C may make a forward run, and Player D may remain behind the ball as defensive cover. The best support position depends on the ball carrier's pressure, the defense's shape, passing lanes, the attacking direction, and the next likely play. They should not all chase the same point.

Tactical intent should update as evidence changes. A blocked lane or intercepted pass should produce a new decision, not a canned continuation of the old plan.

## Events are the bridge between simulation and spectacle

Define a match-event stream or equivalent explicit state transitions. The initial vocabulary can include:

- MatchStarted, PossessionChanged, PassAttempted, PassCompleted, PassIntercepted
- ShotAttempted, ShotSaved, ShotMissed, GoalAwarded
- BallOutOfPlay, FoulCalled, OffsideCalled
- HalfEnded, MatchEnded, RestartReady

These names are proposals for the consumer/host contract, not existing public API declarations. Each event should carry enough identity and context to explain what happened: match, period/clock, participants where relevant, ball state or observation reference, and authoritative source. Distinguish an attempt from a confirmed outcome. Use explicit unknown/indeterminate states when evidence is unavailable.

Presentation subscribes to these outcomes. For example, ShotAttempted may trigger a tighter camera, but only GoalAwarded updates the score and launches the full goal celebration. A save should visibly differ from a miss. A referee signal should correspond to an actual rule decision.

## The first visual language

Prioritize readable silhouettes and clear state before fine detail:

- Team colors and distinct goalkeeper appearance.
- Stable player identity across frames; no swapping identities when formations change.
- Clear ball visibility and plausible ball-to-player spacing.
- Anticipation before major actions, follow-through after them, and recovery after interruption.
- Turning, acceleration, deceleration, changes of direction, and short repositioning—not just sliding between points.
- Shared movement procedures with per-player parameters, timing variation, and role-specific goals.
- A camera that preserves tactical context but can follow a shot, save, or goal without losing the ball.
- Scoreboard, clock, and referee/crowd cues driven by match state.
- Crowd activity with restrained ambient variation, plus stronger reactions to meaningful events.

Use the current Gestures evaluator only for what it actually provides: deterministic pose evaluation. A host-specific rig mapping, locomotion controller, ball physics, and sound/presentation layer are separate work items until implemented and validated.

## Build and validation sequence

### Stage 1 — Domain fixture
Create deterministic field geometry, team and player state, ball state, match phases, role objectives, action requests, and explicit outcomes. Include one short attack in which the carrier passes to a support player, followed by a shot and a confirmed outcome. Include a failure path such as an intercepted pass or saved shot.

### Stage 2 — Tactical readability
Test that support players create useful options, defenders respond to the ball and threats, players retain independent assignments, and decisions change when lanes close. Add high-line/offside scenarios only with correct timing inputs and authoritative adjudication.

### Stage 3 — Gesture and rig handoff
Translate approved movement/action requests to Gestures procedures. Validate identity, facing, target, timing, interruption, and recovery. Define a concrete host rig adapter only after the target Renderer/rig contract is inspected and agreed; do not invent one in the domain package.

### Stage 4 — Ball and match authority
Implement or connect the authoritative ball/match simulation. Test pass outcomes, loose balls, possession changes, shots, saves, goals, out-of-play restarts, and rule decisions independently of visual success.

### Stage 5 — Arena presentation
Add the pitch and goal visuals, stable player representations, camera, scoreboard/clock, crowd ambience, event-linked sound and reactions. Make the same match event stream drive every presentation element so they cannot contradict one another.

### Stage 6 — Demonstration and scale
Record deterministic scenarios and validate the experience at normal spectator distance. Then add more tactical choices, player variation, camera styles, replays, and larger rosters based on measured readability and performance.

## Acceptance checks for the first playable slice

- Two complete eligible teams enter a field and start only when the match authority says they may.
- Every player has stable identity and independent state.
- The ball has an explicit state, and possession is never guessed from pose.
- The ball carrier and at least one support player pursue different, complementary objectives.
- A defender can alter the attack by closing a lane or intercepting a pass.
- The same scenario can produce a success and a failure path without changing the rules.
- A completed gesture cannot by itself award a pass, possession, foul, offside, or goal.
- The score changes only after a confirmed goal event.
- Camera, sound, crowd, and player reactions are consequences of match events, not competing sources of truth.
- A deterministic replay of the same inputs produces the same domain decisions and outcomes, where the chosen simulation model promises determinism.
- Tests and documentation distinguish implemented behavior from illustrative fixtures and unimplemented presentation.

## The principle

**The audience should see the consequences of the game; the players should act on the state of the game; and the match authority should decide what actually happened.**

That separation is what lets Singularity City host a soccer match that feels coherent rather than a collection of animations.
