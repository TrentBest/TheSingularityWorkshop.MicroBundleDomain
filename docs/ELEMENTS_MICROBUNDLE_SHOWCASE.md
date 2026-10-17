# Elements MicroBundle: Periodic Table Showcase

**Status:** design proposal; no canonical Elements MicroBundle or periodic-table UI is implemented by this document.

## Purpose

Use the chemical elements as the first visible demonstration of MicroBundle discovery and arbitration. Element data is independently useful; a periodic table is one experience composed from that data, not the owner of the data.

## Where are the elements defined today?

A search of the current MicroBundleDomain and FSM_COS source branches did not find an existing canonical Elements MicroBundle or periodic-table dataset. MicroBundleDomain intentionally has no built-in chemistry knowledge. That is correct for a neutral contract package, but it means the actual element records still need an explicitly owned home.

Do not add the periodic table as hard-coded knowledge inside MicroBundleDomain or FSM_COS.

## Proposed ownership

- **Elements domain bundle(s):** canonical, versioned records and stable element identities.
- **MicroBundleDomain:** neutral bundle contracts only.
- **MicroBundleRepository:** discovery and delivery of the element bundle artifacts.
- **FSM_COS / composition host:** resolves requested element capabilities and assembles the requested composition.
- **WebPage:** presents the periodic table, search, filters, and detail panels; it does not become the scientific source of truth.
- **Physics / chemistry evaluators:** consume supported element/material data through domain contracts where needed; they do not own the UI or MicroBundle discovery.

## One bundle per element, or one bundle with 118 records?

Start with **one Elements bundle exposing 118 separately addressable element records** unless independent distribution/versioning of individual elements proves useful. This avoids 118 package artifacts merely for visual modularity while still allowing arbitration to request a specific element by stable identity.

If the ecosystem later benefits from independently authored isotope, electron-configuration, spectroscopy, or teaching-content providers, those can be separate capabilities that reference the same stable element identity.

The important unit of composition is the capability boundary, not the number of DLLs.

## Initial record contract

Every element record should have a stable identity and explicitly sourced, versioned data. The first useful fields are:

- atomic number (primary ordering identity for known elements);
- symbol;
- English name;
- standard atomic weight or mass number, with the distinction made explicit;
- category and group/period placement, with any classification policy named;
- electron configuration, when sourced;
- source identifier, source version/date, and provenance for fields;
- field-level unknown/unavailable state rather than invented values.

Do not conflate standard atomic weight with the mass number of a specific isotope. Some elements have no standard atomic weight in the ordinary sense; represent the appropriate source-defined value or status instead of manufacturing a numeric default.

Scientific records need a declared source and licensing review before we import a complete dataset. UI-friendly colors and layout positions are presentation metadata, not chemical facts.

## First arbitration demonstration

1. The experience requests a periodic table.
2. The host discovers the Elements capability and any optional property/teaching-content providers.
3. Arbitration checks which requested capabilities are present and compatible.
4. The composition result includes the element records and available optional capabilities, with missing capabilities reported explicitly.
5. WebPage renders the table from the composed data.
6. Selecting an element requests its stable identity and presents whichever detail capabilities are available.

A missing spectroscopy provider should remove or mark that section as unavailable; it must not prevent the basic element record from being displayed. An unavailable record must not be silently substituted with another element.

## A small first milestone

- Define the versioned record schema and stable element identity.
- Select a source with suitable provenance and license terms.
- Create an executable fixture containing a small representative subset (for example H, C, Fe, Cu, and Au) to test identity, provenance, missing fields, and deterministic ordering.
- Expand to all 118 records only after schema and source validation.
- Build the WebPage experience as a consumer of the composed records, not as a second dataset.

This is a showcase of arbitration because it makes the difference visible between **what data exists**, **which providers are available**, and **how one experience chooses to present it**.

## Related contracts

- [Architecture](ARCHITECTURE.md)
- [Provider Resolution: Composition Boundary Contract](PROVIDER_RESOLUTION_CONTRACT.md)
- [Strategic Change Requests](STRATEGIC_CHANGE_REQUESTS.md)
