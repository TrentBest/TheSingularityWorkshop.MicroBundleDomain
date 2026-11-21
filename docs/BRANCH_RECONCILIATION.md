# Branch reconciliation — current audit

**Audit date:** 2026-11-20 / refreshed 2026-11-21  
**Inspected development head:** `568197b95181cafbc2bbea0424b58bc87d29a211`  
**Live remote inventory:** 12 branches. Canonical policy is exactly `master` + `development`. No branch was deleted, force-pushed, merged, or rewritten by this audit.

## Current branch inventory

| Branch | Audited head | Relationship / PR | Disposition |
|---|---|---|---|
| `master` | `cd73eb5b` | Canonical released baseline; published package 1.0.1 was built from the .NET 8 line | Retain unchanged |
| `development` | `568197b9` | Canonical active source; .NET 10 line | Retain |
| `chore/standardize-ci-microbundle-domain` | `278effd4` | PR #9 open; 1 ahead, 47 behind | Retain; active CI review |
| `docs/readme-standard` | `8aff1ad5` | PR #10 open; 3 ahead, 47 behind | Retain; active documentation review |
| `work/ciai-evidence-submodule-checkout` | `d4b0db98` | PR #25 open; 1 ahead, 17 behind | Retain; active CiAi checkout repair |
| `work/soccer-domain-microbundle` | `3bde7fe1` | PR #16 open; 46 ahead, 47 behind | Retain; active domain design work |
| `release-prep/microbundle-ecosystem-foundation` | `bff22f0d` | PR #5 closed unmerged to master; 5 ahead, 100 behind | Preserve pending selective source/docs audit |
| `architecture/runtime-contract-ownership` | `e81747aa` | PR #6 merged to master; 0 ahead, 50 behind | **READY FOR OWNER DELETION** |
| `docs/benchmarking` | `b400a16f` | PR #7 merged into architecture branch; 0 ahead, 55 behind | **READY FOR OWNER DELETION** |
| `design/game-domain-boundaries` | `3c8d6471` | PR #14 merged to development; 1 ahead, 46 behind; its only changed file is byte-identical to development | **READY FOR OWNER DELETION** |
| `feature/stable-parameter-identity` | `6edc6166` | PR #15 merged to development; 7 ahead, 45 behind | **READY FOR OWNER DELETION** after verified supersession |
| `work/ciai-evidence-adoption` | `4c9d9096` | PR #24 merged to development; 3 ahead, 18 behind | **READY FOR OWNER DELETION** after verified supersession |

## Deletion evidence

The following exact refs are ready for the owner to delete in one audited batch:

- `TrentBest/TheSingularityWorkshop.MicroBundleDomain:architecture/runtime-contract-ownership`
- `TrentBest/TheSingularityWorkshop.MicroBundleDomain:docs/benchmarking`
- `TrentBest/TheSingularityWorkshop.MicroBundleDomain:design/game-domain-boundaries`
- `TrentBest/TheSingularityWorkshop.MicroBundleDomain:feature/stable-parameter-identity`
- `TrentBest/TheSingularityWorkshop.MicroBundleDomain:work/ciai-evidence-adoption`

Evidence:
- `architecture/runtime-contract-ownership` and `docs/benchmarking` have zero commits ahead of current `development`; both are historical ancestors with no unique file diff.
- `design/game-domain-boundaries` has one ahead commit, but its only changed file, `docs/GAME_DOMAIN_DESIGN.md`, has the exact same blob SHA on the branch and current `development`. PR #14 is merged and no workflow references the branch.
- `feature/stable-parameter-identity` belongs to merged PR #15. Its stable parameter ID API, builder, docs, tests, and workflow changes are present in current `development`; the remaining blob differences are later development edits. The branch has no open PR.
- `work/ciai-evidence-adoption` belongs to merged PR #24. Its only changed path is `.github/workflows/ciai-evidence.yml`; current `development` contains the same pinned evidence flow plus the recursive submodule checkout correction now tracked separately by open PR #25. The branch has no open PR.
- Current `build.yml`, `sith-policy.yml`, `ciai-evidence.yml`, and solution files contain no explicit references to the five deletion-candidate branch names.

No deletion was performed. The owner remains responsible for removing these refs.

## Active and preserved work

- PR #9: standardize the .NET CI workflow. Keep its branch until reviewed and integrated or explicitly rejected.
- PR #10: README / Getting Started alignment. Keep its branch until reviewed and integrated or explicitly rejected.
- PR #16: soccer domain MicroBundle design. Keep its branch; it contains substantial unique design and source work.
- PR #25: fix the CiAi evidence observer's pinned source checkout. Keep its branch until its checks and retry path are accepted.
- PR #5 / `release-prep/microbundle-ecosystem-foundation`: closed without merge and still contains five commits ahead of development. Preserve until each unique code/documentation change is reviewed; do not delete based only on the closed PR state.

## Release and package evidence

- NuGet package `TheSingularityWorkshop.MicroBundleDomain 1.0.1` is already published and remains the immutable release baseline.
- `master` targets .NET 8; current `development` targets .NET 10. The development project file still needs a new package version and an explicit target-framework compatibility decision before another package can be released.
- [Pack and publish workflow #44874361365](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/actions/runs/44874361365) completed successfully on the development documentation checkpoint; the `publish_nuget` job was **skipped**. No package was published by this work.
- The live Sith policy check fails while the remote branch inventory contains 12 branches. Publication must remain fail-closed until the owner reduces the inventory to exactly `master` + `development`.

## Identity contract coordination

Current `development` now contains [docs/IDENTITY_MODEL.md](IDENTITY_MODEL.md), which explicitly distinguishes portable semantic identity, the current runtime/domain numeric identity, and the repository-local storage handle. [Issue #28](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/issues/28) remains open for the cross-repository mapping and dependency-closure proof. Do not reinterpret existing numeric IDs or claim that the repository storage binding alone solves runtime identity.

---

**Disposition:** five branches are ready for owner deletion; four branches carry active PRs; one closed release-prep branch remains preserved for selective audit; `master` and `development` remain untouched. The repository is not Sith-compliant until the owner performs the deletion batch and the live branch inventory is rechecked.
