# Sloop Troop — Game Design Document (GDD v0.1)

> **Salvage together. Sink together.**
> A cheap, chaotic, **constructive** co-op comedy: 2–6 friends crew one leaky sloop, haul physics-junk out of
> the sea, and rebuild a sunken archipelago — while everything that can go wrong, gloriously does.

| | |
|---|---|
| **Working title** | Sloop Troop |
| **Genre** | Co-op physics comedy ("friendslop") — **constructive / non-horror** vertical |
| **Sub-tags** | Online co-op, Physics, Sandbox, Building, Proximity chat, Funny |
| **Engine** | Unity 6000.4.4f1 (URP) + Netcode for GameObjects |
| **Players** | 2–6 online co-op (drop-in/drop-out), proximity voice; solo-playable |
| **Platforms** | PC (Steam) primary, Steam Workshop mods → console later |
| **Price** | **$9.99** (whole-squad-buys-in pricing) |
| **Session** | 20–40 min "expeditions"; pick-up-and-play |

---

## 1. Vision Statement

2025 proved a thesis the whole industry is now scrambling to copy: a tiny team can sell **millions** of copies
of a cheap, funny, **co-op** game built around **proximity voice and emergent chaos** — Lethal Company, R.E.P.O.,
Content Warning, and especially **PEAK** (10M+ copies). But that gold rush has a blind spot: almost every hit is
**horror, extraction, or pure chaos**. PEAK's own success showed *you don't need horror* — and players are
openly admitting fatigue with co-op that "relies on big scares and tension."

Sloop Troop claims the **underserved constructive corner** of friendslop: same cheap price, same streamable
proximity-voice slapstick, same "the point is to hang out with your friends" design — but the goal is to **build
something together**, not survive a monster. You crew a comically under-engineered sloop, dredge physics-junk
from the sea, and rebuild island outposts. The comedy comes from **cooperation under physics**, not from dread.

**Design north star:** every laugh should come from *six people trying to move one awkward object at the same
time*, never from a jump-scare. Wholesome chaos, not horror chaos.

---

## 2. Market Gap (why this, why now)

See `docs/MARKET_VALIDATION.md` for full sourcing. Thesis summary:

- **Co-op is wildly under-supplied vs demand:** ~6% of Steam releases but **36% of units sold (2023)**, rising
  to **~46% in 2024**; **games with co-op grossed ~$8.2B on Steam in 2025.** The category over-performs by ~6–8×.
- **The hits cluster in horror/extraction/chaos.** PEAK explicitly proved **non-horror** co-op can be the
  biggest breakout of the year, and press/players note **fatigue with scare-reliant** friendslop.
- **Constructive co-op (build *together*) is the open lane.** "Friendslop + creation" barely exists at the
  cheap, streamable, proximity-voice tier where the money is.
- **Cheap is a feature.** $8–10 pricing is the documented growth unlock ("easier to convince your whole squad").

**Positioning sentence:** *"PEAK's proximity-voice slapstick meets Moving Out's physics comedy — but you're
rebuilding a world together instead of surviving a monster."*

---

## 3. Core Pillars

1. **Proximity voice is the core mechanic, not a feature.** Distance-attenuated chat; the game is a machine for
   generating "you had to be there" moments.
2. **Physics is the comedian.** One object often needs 2+ people; everything is bumpable, droppable, sinkable.
3. **Constructive goal.** You leave each island *better*. Progress is visible, shared, and celebratory.
4. **Cheap, fast, clippable.** 20–40 min expeditions, instant re-queue, built for Twitch/TikTok clips.

---

## 4. Core Gameplay Loop

1. **Set sail.** The crew shares one sloop. Steering, sail, anchor, and bilge-pump are all separate stations —
   nobody can do it alone.
2. **Find a wreck/island.** Drop anchor in choppy water.
3. **Salvage.** Dive/haul physics-junk (crates, beams, engine parts, a confused goat) back aboard. Heavy items
   need multiple players; the deck has limited space and a **Load/Trim** balance (overload one side → list → slide
   everything overboard).
4. **Build.** Ferry salvage to the island's **Build Pad** to reconstruct outposts (lighthouse, dock, market,
   bridge). Building is collaborative snap-together with deliberate jank.
5. **The Swell.** Periodic telegraphed waves/storms shove loose objects and players — the comedy spike.
6. **Cast off** to the next island. Persistent archipelago "fills in" as the crew rebuilds it over a run.

> The hook we're chasing: the *constructive* version of PEAK's stamina-management tension. Instead of "don't
> fall off the mountain," it's "don't let the sloop capsize while six of us argue about where the lighthouse goes."

---

## 5. Systems Design

### 5.1 Proximity Voice (Networking)
Distance-attenuated, occlusion-aware voice over Netcode + a voice transport (e.g., Vivox/Dissonance). Falloff and
a visible "speaking" indicator turn shouting-across-the-deck into the primary content. Push-to-yell increases
radius for the comedic "OVER HEEERE."

### 5.2 Carry Physics
Every salvage object is a networked rigidbody. Players grab with a configurable joint; **co-carry** sums forces,
so a long beam carried by two players who don't coordinate swings wildly. Drop, throw, and "accidentally bonk a
teammate overboard" are all first-class.

### 5.3 Sloop Buoyancy & Trim
The sloop floats via sampled buoyancy points. **Trim** = load distribution; uneven cargo lists the boat and items
slide. The **bilge** fills during storms and must be pumped. Capsize is a soft-fail (everyone bobs up, salvage
scatters) — funny, not punishing.

### 5.4 Build System
Salvage converts to **build pieces** at the Build Pad; pieces snap to an outpost blueprint with intentional
wobble. Finishing an outpost grants a crew-wide perk (e.g., a repaired lighthouse reveals the next island; a
market lets you trade junk for upgrades like a bigger net or a second dinghy).

### 5.5 The Swell (chaos director)
A lightweight director escalates weather based on crew progress and over-confidence (too much loose cargo →
bigger swell). Always telegraphed (horizon darkens, gull alarm) so failure feels earned and hilarious.

### 5.6 Progression
Per-run: rebuild the archipelago, unlock outposts and tools. Meta: cosmetic crew customization + Steam Workshop
**island/blueprint mods** (Unity + Netcode + an open data format = a modding flywheel, exactly the engine of
these games' long tails).

---

## 6. Unique Selling Point (USP)

**The first cheap, proximity-voice friendslop where the whole point is to BUILD a world together, not survive one.**
It takes the proven, multi-million-selling co-op formula and points it at the one underserved direction the
breakout hits left open: **constructive, wholesome, physics-comedy cooperation.**

---

## 7. Target Audience

- **Primary:** the PEAK/Lethal Company/R.E.P.O. "friendslop" audience (squads of 3–6, 18–30, Discord-native)
  who want the social loop **without** horror.
- **Secondary:** physics-comedy co-op fans (Moving Out, Overcooked, Lethal Company) and cozy-co-op players who
  bounce off scare-based games.
- **Tertiary:** streamers/clip creators — the format is engineered for "you had to be there" virality.

---

## 8. Platform & Launch Strategy

1. **Steam first**, Unity + Netcode; **Steam Workshop** for islands/blueprints from day one.
2. **Demo in Steam Next Fest**; the proximity-voice capsize clip is the marketing.
3. **$9.99** to maximize whole-squad conversion (the documented friendslop growth lever).
4. **Console** after netcode hardening; cross-play a stretch goal.

---

## 9. Monetization

- **$9.99 premium**, buy-to-play. No FTP, no battle pass at launch.
- **Post-launch:** optional **cosmetic** crew/sloop packs only (hats, paint, horns). Gameplay stays free to keep
  the "everyone just buys it" math intact.
- **Mods are free** (Workshop) — they are user-acquisition, not a revenue line.

---

## 10. Market Positioning Map

```
                       CONSTRUCTIVE / WHOLESOME
                                |
        Moving Out (couch)      |     [ SLOOP TROOP ]   <- open lane:
        Overcooked              |      cheap, online, proximity-voice,
                                |      constructive friendslop
 ---SINGLE / COUCH-------------+----ONLINE + PROXIMITY VOICE---
                                |
        Lethal Company          |     PEAK (climb)
        R.E.P.O. (horror)       |     Content Warning
                                |
                       SURVIVE / SCARE / EXTRACT
```

Sloop Troop is the only entry in the **constructive × online-proximity-voice** corner.

---

## 11. Art & Audio Direction

- **Art:** bright, chunky, low-poly "bath-toy" aesthetic — wobbly, readable, cheap to produce, instantly
  legible in a thumbnail. Exaggerated ragdolls and squash-and-stretch for comedy.
- **Audio:** the mix is built around **voice**; SFX are punchy and cartoonish (splashes, bonks, foghorn).
  Music is light sea-shanty that swells with the Swell.

---

## 12. Vertical Slice Scope (first milestone)

One sloop (4 stations), one archipelago of 3 islands, ~8 salvage types, co-carry physics, trim/capsize, one
buildable outpost (the lighthouse), the Swell director, proximity-voice stub, 2–4 player session. The HTML
prototype models the salvage→trim→build loop in single-player; the Unity project scaffolds the networked version.

---

## 13. Risks & Mitigations

| Risk | Mitigation |
|---|---|
| Netcode/physics desync (carry + buoyancy is hard) | Server-authoritative rigidbodies, client prediction only on local player; capsize is forgiving by design |
| "Friendslop" is getting crowded | We target the **non-horror constructive** lane specifically; differentiation is the moat |
| No friends = no fun (social dependency) | Solo-playable with AI deckhands; quick-match + Discord rich presence; cheap price lowers squad friction |
| Comedy doesn't land without scares | Physics + co-carry + proximity voice are the proven comedy engine (PEAK/Moving Out); Swell director guarantees set-piece moments |

---

## 14. Deliverables in this repo

- `GDD.md` — this document
- `docs/MARKET_VALIDATION.md` — sources, traction signals, critic-cycle log
- `prototype/index.html` — self-contained HTML5 Canvas prototype of the salvage → trim → build loop (with AI crew)
- `unity/` — Unity 6000.4.4f1 + Netcode project structure, core C# scripts, scene manifest, prefab specs, README
- `branding/` — logo + key-art (SVG)
