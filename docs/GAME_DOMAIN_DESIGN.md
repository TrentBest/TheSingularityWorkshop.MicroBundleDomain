# Game-domain design: keep composition generic, keep rules authoritative

> Status: proposed design note. This document does not add public API or authorize a package release.
>
> Related work: [generic game abstractions](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/issues/13), [composable chess reference](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/issues/12), and [AnyApp chess integration](https://github.com/TrentBest/AnyApp/issues/24).

## The boundary in one picture

```text
                    AnyApp
              presentation + input
                       |
                       v
             Game Experience / host
           composes and runs capabilities
                       |
          +------------+-------------+
          |            |             |
          v            v             v
    Generic game    Chess rules   HeadlessAi
    contracts       + state       move proposal
          |            |             |
          +------------+-------------+
                       |
              authoritative validator
                       |
               accepted state change

    FSM_COS resolves and composes MicroBundles.
    MicroBundleDomain describes MicroBundle contracts only.
    MicroBundleRepository discovers and delivers artifacts.
```

The critical distinction is that an agent can **propose** an action, but only the selected game's rules can **authorize and apply** it. UI code must not become the rules engine, and a model response must never be treated as a legal move merely because it parses.

## Recommended ownership

| Concern | Owner | Deliberately does not own |
|---|---|---|
| Bundle identity, dependency declarations, load/arbitration contract | MicroBundleDomain | Game pieces, board coordinates, turns, chess rules |
| Artifact discovery and delivery | MicroBundleRepository | Interpreting chess semantics |
| Generic game vocabulary, if proven reusable | A future independent game-domain package | MicroBundle loading or UI rendering |
| Chess position, legal moves, special rules, game outcome | Chess-specific domain/capabilities | Window layout, provider transport |
| Model/provider communication | HeadlessAi | Legal-move authority or mutation of game state |
| Composition, ordering, runtime assembly | FSM_COS / composition host | Owning chess-specific semantics |
| Presentation, input, interaction feedback | AnyApp | Duplicated rules or direct trust in model output |

Do not create a new public package merely to give these concepts a home. First prove the proposed generic contracts against chess and at least one contrasting topology or action model; then extract only the stable seam.

## Separate concepts before choosing interfaces

1. **Definition / type identity** — reusable description of a kind of piece or game capability, with stable identity and version/compatibility information.
2. **Instance data** — a particular piece or entity in one match. It refers to a type; it does not copy executable behavior.
3. **Setup and ingestion** — validated input that creates an initial state and binds references to available definitions. Keep source data, schema, behavior, and live state conceptually distinct.
4. **Topology and addresses** — the locations or relationships that exist in a game. A square grid is one possible topology, not the definition of a board.
5. **Occupancy and state** — what currently occupies a location and other current match facts. State changes should be explicit and deterministic.
6. **Actions and transitions** — a proposed action, validation result, and accepted state transition. A rejected action must leave authoritative state unchanged.
7. **Lifecycle and outcome** — started, active, paused or ended states and game-specific outcomes. Turns are a capability, not an assumption imposed on every game.
8. **Policy / agent** — chooses or proposes actions from an observation. Policy is replaceable and cannot bypass validation.

These may be separate contracts, capabilities, or data records. The list is a separation of responsibilities, not a requirement to create one MicroBundle or package per line.

## Minimal design tests

Before freezing generic public contracts, require the following:

- **Neutral vocabulary:** no chess-only terms in generic contracts; no assumption that every game has pieces, turns, a board, or square coordinates.
- **Identity integrity:** duplicate instance IDs, unknown type references, incompatible versions, and unresolved capability dependencies fail validation with actionable diagnostics.
- **Deterministic setup:** the same validated manifest and setup data produce the same initial state.
- **Topology is not legality:** topology answers which locations/relationships exist; game rules decide whether an action is legal.
- **Atomic rejection:** an illegal action does not partially mutate state, advance the turn, or alter the outcome.
- **Proposal is not authority:** arbitrary model output is parsed into a proposal, validated by the game, and rejected safely if invalid.
- **UI independence:** rules and transition tests run without WPF, a window, a provider key, or network access.
- **Contrast case:** at least one second topology/action shape is used to expose chess assumptions before promoting abstractions as generic.
- **Composition boundary:** MicroBundleDomain remains usable without a reference to the game domain, HeadlessAi, AnyApp, or FSM_COS.

## Chess as the demanding reference case

Chess should first be a correct deterministic domain, then an Experience composed from capabilities. Keep the initial implementation focused enough to test end-to-end, but do not pretend the game is complete until it handles all standard rules.

At minimum, correctness work must cover legal movement, obstruction and capture, own-king safety, castling, en passant, promotion, check/checkmate/stalemate, and draw/termination rules. Tests should include positions that exercise special rules and prove illegal actions preserve state. A visual board with model-generated moves is not yet a playable or trustworthy chess implementation.

Candidate MicroBundle boundaries—piece definitions, movement primitives, board setup, legal-action validation, special rules, turn lifecycle, outcome detection, and agent policy—remain hypotheses. Combine capabilities when separate bundles would add ceremony without a useful independent identity or reuse case.

## Delivery sequence

1. Validate the existing MicroBundleDomain descriptor, dependency, load-context, and arbitration seams; do not expand them with game-specific API.
2. Design the smallest candidate game-domain contracts in a separate domain boundary, without an upward dependency on FSM_COS.
3. Implement a deterministic chess rules/state slice and tests independent of AnyApp and live AI providers.
4. Integrate a local legal-move opponent or deterministic policy first, so the game is playable without credentials or network access.
5. Add HeadlessAi player adapters that propose moves; validate every proposal through the same authoritative rules path.
6. Add GPT-vs-Gemini selection only after both provider paths and failure/timeout behavior are real and testable. Clearly identify which calls are live and may incur charges.
7. Compose the capability set through a manifest and present it in AnyApp. Keep publishing disabled unless explicitly approved.

## Release and evidence policy

This is design work only. Do not publish packages as a side effect of this effort. For every implementation milestone, report the exact branch/commit, automated test evidence, and any unverified behavior. Never describe simulated output as AI-generated or describe an incomplete ruleset as a complete chess game.
