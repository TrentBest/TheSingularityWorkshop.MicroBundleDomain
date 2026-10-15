# Experiences Beyond the Arena — Markets, Craft Fairs, and Optional Economy

**Status: domain direction and scheduling reference.** The shared Arena fixture is one example of a broader Experience pattern: a host venue schedules independently provided activities. Soccer, concerts, karaoke, market fairs, and craft fairs use a neutral scheduling boundary.

## Experience catalog

| Experience | Core participation | Optional capabilities |
|---|---|---|
| Soccer / tournament | teams, match lifecycle, field, referee and match rules | ticketing, concessions, merchandise, prize economy |
| Concert / karaoke | performers, cues, audience intentions, venue spaces | tickets, merchandise, vendors, paid requests |
| Market fair | stalls, browsing, conversation, demonstrations, discovery | vendor inventory, prices, payments, commissions |
| Craft fair | makers, work-in-progress, materials, demonstrations, commissions | craft supply stock, order tracking, purchases |
| Workshop / class | instructor, lesson sequence, learner participation | registration, materials fees, optional paid sessions |
| Exhibition / gallery | exhibits, tours, creator stories, visitor attention | catalog sales, commissions, donations |
| Festival / civic gathering | scheduled activities, public spaces, volunteer roles | vendors, fundraising, bookings, optional tickets |
| Auction / exchange | lots, offers, ownership and settlement rules | Economy/provider adapters, where supported |
| Transit / cargo event | arrivals, departures, loading, queues and handoffs | fares, freight quotes, inventory and settlement |

This is a vocabulary for composition, not a promise that every listed activity is already implemented. The event scheduler should know venue identity, event identity, time interval, provider identity, and lifecycle. Each Experience provider owns its domain-specific rules.

## Economy is a connection, not a prerequisite

A market fair should still be a useful social Experience when no Economy capability is installed. Digitens can arrive, inspect a display, talk with a maker, watch a demonstration, ask a question, or leave. None of those actions should fail because a price or payment provider is absent.

When Economy is present, selected stall or activity providers may expose additional capabilities through an adapter:

- inventory and offer descriptions;
- prices and currencies;
- purchases, commissions, reservations, or paid participation;
- receipts and transaction status, if the selected provider supports them.

The dependency direction is optional:

    Market / Craft Fair Experience
              |
              +---- core: social presence, discovery, demonstrations
              |
              +---- optional adapter -> Economy semantics
                                         |
                                         +-> provider integration, if configured

Do not make every stall inherit from Economy, make the fair MicroBundle depend on a payment provider, or assume a payment has happened because an offer was displayed. The Experience defines the activity; Economy defines the optional economic intent/semantics; a provider adapter performs whatever integration it explicitly supports.

Economy is still exploratory and should not be presented as a production payment system. No live money movement or payment-provider integration is implied by this design.

## Composition examples

A market fair with no Economy capability:

- The venue and fair are scheduled.
- Makers and visitors participate in demonstrations and conversation.
- Stalls may expose non-commercial descriptions and creator information.
- No checkout or financial state is fabricated.

The same fair with Economy composed:

- A stall may expose an offer or inventory view through an optional adapter.
- A visitor may request a purchase or commission.
- The configured provider returns its actual result.
- The fair can still host non-commercial stalls, social activity, and demonstrations.

The capability should be discoverable and composable without forcing unrelated Experience types to acquire it. A soccer match does not need Economy to exist; a craft fair does not need Economy to demonstrate craft.

## Current fixture and limits

The in-memory Singularity Arena fixture proves shared venue scheduling for soccer and concert events. This extension adds MarketFair and CraftFair event kinds and reference tests for distinct provider IDs. It does not implement vendor records, stalls, capacity, admission, inventory, payment, persistence, wall-clock dispatch, or an Experience browser UI.

Recommended next proof sequence:

1. Define venue spaces and public/private participation independently of commerce.
2. Add market/craft fair provider data: stall identity, maker identity, activity, and optional offer reference.
3. Keep offer references nullable and adapter-resolved; no Economy dependency in the neutral event scheduler.
4. Add tests proving fair scheduling and visitor participation with no Economy adapter.
5. Add a separate optional Economy adapter fixture and test both absent and present configurations.
6. Connect the Experience to WebPage's manifest-driven presentation and prove the visitor journey.

## Ownership

- **MicroBundleDomain:** neutral composable capability contract.
- **Arena / Experience scheduling:** venue and event identity, time windows, lifecycle.
- **Market or craft-fair provider:** stalls, maker activities, demonstrations, and optional offer references.
- **Economy:** optional economic intent and semantics; not a prerequisite for the fair.
- **Payment / commerce provider:** provider-specific execution and authoritative transaction results.
- **WebPage:** visitor-facing presentation and integration proof, not a duplicate domain implementation.
