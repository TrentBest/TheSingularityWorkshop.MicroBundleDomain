# Strategic Change Requests

**Status:** ecosystem design proposal; no strategic-change request API, inbox, or aggregation service is implemented by this document.

## Purpose

A creator ecosystem needs a respectful way for users to signal which new capabilities would make an existing MicroBundle more useful. A user who owns a desktop variant may want a web variant. They should be able to express that interest without framing the current desktop-only bundle as defective or pressuring the author to promise a roadmap.

Call this signal a **Strategic Change Request**: a non-binding request for a new capability, variant, integration, or direction for an existing MicroBundle.

It is feedback and demand discovery—not a command, bug report, public complaint, contract change, or promise of delivery.

## Example

A user opens a desktop application MicroBundle and selects:

> **Request another variant** → Web experience

They can optionally explain their use case, choose whether to share their request with the creator, and indicate whether they would consider supporting or purchasing the variant if the creator offers it.

The creator receives a respectful signal such as:

> Several users have expressed interest in a web variant of this capability. Their use cases include browser access from managed devices and sharing links with collaborators. No delivery commitment has been made.

The current desktop variant remains valid. The creator decides whether, when, and how to respond.

## Keep the request neutral and actionable

A request should describe the desired change, not prescribe the implementation. For example, “make this capability usable from a browser” is better than demanding a particular framework, repository layout, or architecture.

Suggested fields:

| Field | Purpose |
|---|---|
| Request ID | Stable identity for the request |
| Target bundle ID | Identifies the capability being discussed |
| Target bundle version | Optional version context; not a promise to fork that version |
| Requested change kind | New variant, platform, integration, content, capability, or other |
| Desired outcome | Short user-language description of the need |
| Use case | Optional context that helps the creator evaluate the opportunity |
| Target environment | For example web, desktop, mobile, server, or XR |
| Interest signal | Optional: interested, would consider paying, or seeking sponsorship |
| Visibility/consent | Whether the creator may receive the request and whether it may be aggregated |
| Created/updated time | Enables transparent aggregation and aging |
| Status | New, acknowledged, planned, in progress, delivered, declined, or deferred—only when set by the creator/system with clear attribution |

Do not require a user to state willingness to pay. A request is useful without it. Do not treat interest signals as a purchase, pre-order, or binding financial commitment.

## Ownership and flow

1. **User or consuming experience** creates a request against a stable bundle identity.
2. **Ecosystem feedback service or repository-adjacent service** validates, stores, and aggregates requests under an explicit privacy policy.
3. **Bundle developer/maintainer** receives the signal through a creator-controlled inbox or dashboard, if the user has consented to sharing it.
4. **Developer** may acknowledge, ask a question, mark a direction as planned, decline, defer, or ship a new variant.
5. **Repository/catalog** can later expose a delivered variant as a distinct artifact/capability with its own identity, compatibility declarations, and provenance.

MicroBundleDomain should not become the feedback database, notification service, billing system, or roadmap manager. Its neutral contracts can eventually carry a portable request envelope if real consumers demonstrate that a shared type is useful; transport, identity, privacy, aggregation, and notification remain host/service concerns.

## Creator respect and user trust

- Phrase requests as expressions of interest, never as accusations that a creator failed.
- Do not publicly rank, shame, or name creators who decline requests.
- Aggregate demand only where users have consented and where small groups cannot be used to infer private identities.
- Let creators choose whether to receive requests and which channels they support.
- Do not promise a delivery date or imply the creator has accepted a request unless they explicitly say so.
- Let creators decline or defer without needing to justify their decision publicly.
- Distinguish “requested,” “acknowledged,” “planned,” and “available.” Never collapse them into one status.
- Keep bug reports, security reports, support questions, and strategic requests as distinct intent types.

## Sustainable creator economics

The request is a signal that can help creators decide where to invest their time. With explicit consent and suitable privacy controls, aggregated demand may support:

- prioritizing a new platform variant;
- deciding whether a new content pack is worth authoring;
- discovering opportunities for paid add-ons, sponsorship, grants, or commissions;
- finding several users who want to fund the same extension;
- estimating interest before spending time on implementation.

The ecosystem should help creators make informed choices, not turn user interest into unpaid obligations. Any crowdfunding, preorder, subscription, or payment flow needs a separate, explicit commercial agreement and refund/fulfilment rules.

## Suggested lifecycle

```text
Requested
   ├── Acknowledged
   ├── Deferred
   ├── Declined
   └── Planned
          └── In progress
                 └── Delivered (link to the new capability/variant)
```

These are not promises or mandatory states. The maintainer controls roadmap status, and users should be able to tell who set each status and when.

## Minimum proof before adding a core API

Begin with a host-level prototype and a documented schema. Test that:

- a desktop bundle can receive a request for a web variant without changing its current identity;
- the request is clearly non-binding and distinct from a bug report;
- consent and visibility are explicit;
- aggregate counts do not expose individual users;
- creators can decline or ignore requests without public shaming;
- a delivered web variant is a discoverable capability with explicit compatibility, not an implicit mutation of the desktop artifact;
- no request causes automatic code generation, publishing, or package release.

Only standardize a MicroBundleDomain type if multiple independent hosts need the same portable envelope. Do not add fields to `MicroBundleDescriptor` merely to represent user feedback; bundle metadata and community intent have different lifecycles.

## Related architecture

- [Architecture](ARCHITECTURE.md)
- [Provider Resolution: Composition Boundary Contract](PROVIDER_RESOLUTION_CONTRACT.md)
- [Elements MicroBundle: Periodic Table Showcase](ELEMENTS_MICROBUNDLE_SHOWCASE.md)
