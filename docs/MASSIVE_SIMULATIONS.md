# Massive Simulations: Dominoes, Marbles, and MicroBundle Scale

## Purpose

This note connects the MicroBundle model to simulations containing millions—or eventually far more—of interacting things. The examples are deliberately concrete: dominoes arranged in a large layout, marbles moving through that layout, ramps and overpasses, and material or collision rules that change how motion unfolds.

The architectural point is **not** that every marble or domino should be a separate loaded MicroBundle. A MicroBundle is a focused capability boundary, not a promise that each simulated object becomes a class, assembly, repository record, or runtime plugin.

## The three boundaries remain separate

```text
MicroBundleDomain     What a capability means
MicroBundleRepository Where its immutable artifact is retrieved
Composition host      How required capabilities become a runtime
Simulation state      The many dominoes, marbles, cells, contacts, and events
```

The Domain contract describes capability identity, dependencies, providers, configuration, loading, and arbitration. It does not define a physics engine, world storage format, object-count limit, or scheduling policy. Those must be specified by the relevant physics/simulation capabilities and host.

## One capability can govern many simulated entities

For a domino-and-marble world, prefer a small set of cohesive capabilities whose data describes many entities:

- **Layout** — the authored arrangement and constraints.
- **Geometry / collision** — shapes, bounds, contact candidates, and collision policy.
- **Motion** — positions, velocities, angular state, and integration rules.
- **Material response** — mass, restitution, friction, stiffness, and break thresholds.
- **Domino interaction** — tipping, impact transfer, and stable/resting states.
- **Observation / presentation** — what the current observer needs to inspect and render.
- **Persistence / replay** — checkpoints, deterministic inputs, and timeline branches.

These are examples of possible boundaries, not a claim that the current Domain package implements them. A single capability can operate over a large array or partitioned set of entities. Entity identity and state belong to the simulation's data model; capability identity belongs to MicroBundleDomain.

## A layout bit field is authored data, not a runtime object graph

Suppose a cell uses bits to describe stacked occupancy:

```text
bit 0 -> level 0 occupied
bit 1 -> level 1 occupied
bit 2 -> level 2 occupied
...
```

Then values 0, 1, 2, 4, and 5 have the usual binary interpretations. If the intended rule is instead that a value means a *height* and higher levels imply support or overpass constraints, document that as a distinct encoding; do not silently conflate integer height with independent occupancy bits.

For example, independent occupancy bits mean:
- 0 = no occupied levels
- 1 = level 0
- 2 = level 1
- 3 = levels 0 and 1

A height encoding means:
- 0 = no structure
- 1 = structure through level 0
- 2 = structure through levels 0–1

Those are different models. Choose one, define support and clearance, then test edge cases. The data representation should let the simulator reason about large layouts without creating a heavyweight object for every empty cell.

## Massive counts: separate static description from changing state

A scalable design distinguishes at least four kinds of data:

1. **Definitions** — reusable rules and material properties.
2. **Initial conditions** — starting layout, initial velocities, and configured parameters.
3. **Mutable simulation state** — positions, velocities, contacts, sleep/rest state, and current cell occupancy.
4. **Events / history** — impacts, state transitions, user inputs, checkpoint markers, and replay branches.

Do not store all changing state in a MicroBundle descriptor or dependency list. Those contracts describe composition, not the world's per-tick state.

## Determinism and update order

If reproducibility matters, define the simulation's ordering rules explicitly:
- stable entity IDs and stable ordering for tie-breaking;
- a documented fixed or variable step policy;
- how collision candidates are ordered and deduplicated;
- how numerical tolerances and resting contacts are handled;
- how parallel partitions exchange boundary effects;
- what is serialized in a checkpoint;
- which inputs are required to replay from that checkpoint.

Parallelism alone does not guarantee determinism. A stable update schedule and stable conflict resolution are required if identical inputs must produce identical state.

## Scale by partitioning work, not by multiplying plugin instances

For very large worlds, a host can partition simulation state into spatial cells or regions and schedule different work at different rates. Nearby/high-risk interactions may need frequent updates, while distant or settled regions may be updated less often. Such policies are simulation-engine decisions; they are not guaranteed by MicroBundleDomain.

A useful conceptual pipeline is:

```text
Authored layout + initial conditions
              |
              v
       Capability composition
              |
              v
      Partitioned world state
              |
       +------+------+
       |             |
       v             v
 active contacts   quiet regions
 high-rate work    lower-rate work
       |             |
       +------+------+
              v
       state / event output
              |
       +------+------+
       |             |
       v             v
  observer view   checkpoint/replay
```

Partition boundaries need explicit rules for cross-cell collisions, domino impacts, shared constraints, and moving objects that cross regions. A partition is an optimization boundary, not permission to change the physical result.

## Repository versus world state

MicroBundleRepository stores and delivers immutable, versioned artifacts. It is not the live database for millions of marbles. A simulation may use MicroBundles to deliver its rules and algorithms, while its large changing state uses a separately designed state store, checkpoint format, or streaming transport.

Do not publish every marble position as a new MicroBundle artifact every frame. That would confuse executable capability distribution with high-frequency state replication.

## What is guaranteed today—and what is not

The current Domain contract gives a common vocabulary for capability identity, dependencies, providers, configuration, loading, and arbitration. It does **not** currently guarantee:
- million-entity throughput;
- a particular physics algorithm;
- GPU execution;
- distributed simulation;
- deterministic floating-point results;
- a standard entity schema or checkpoint format;
- automatic LOD or multi-rate scheduling.

Those are design and validation targets for simulation/physics implementations. State them as requirements and prove them with tests and benchmarks rather than implying they follow automatically from MicroBundles.

## Recommended proof sequence

1. Encode a small domino layout and verify the layout semantics.
2. Simulate one domino tipping into its neighbor.
3. Add a marble with a documented collision/material model.
4. Add ramps and stacked/overpass cases, testing support and clearance.
5. Run identical input sequences twice and compare serialized states.
6. Scale entity count while recording throughput, memory, allocation rate, and error drift.
7. Partition the world and test cross-boundary interactions against the single-partition reference.
8. Only then introduce lower update rates for quiet or distant regions, with explicit error bounds.

The goal is not to call a system scalable because its components are modular. The goal is to use modular capability boundaries to keep the model understandable while measuring the actual simulation's behavior at scale.

## Related reading

- [MicroBundle Architecture](ARCHITECTURE.md)
- [Knowledge Model](KNOWLEDGE_MODEL.md)
- [Adventure 7 — From Artifact to Runtime](adventures/07-artifact-to-runtime.md)
- [Provider Resolution Contract](PROVIDER_RESOLUTION_CONTRACT.md)
