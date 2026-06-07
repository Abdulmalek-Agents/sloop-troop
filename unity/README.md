# Sloop Troop — Unity Project (Unity 6000.4.4f1 + Netcode for GameObjects)

Onboarding for the networked prototype of **Sloop Troop**, the studio's #2 niche pick (constructive,
non-horror cooperative "friendslop"). Scaffolds the vertical slice from `../GDD.md` §12: one sloop, a 3-island
archipelago, co-carry physics, deck trim/capsize, the Swell director, proximity-voice stub, and one buildable
lighthouse — for 2–6 players (solo-playable with AI deckhands).

---

## 1. Requirements

| Tool | Version |
|---|---|
| Unity Editor | **6000.4.4f1** (Unity 6) |
| Render pipeline | **URP** |
| Networking | **Netcode for GameObjects (NGO)** + Unity Transport |
| Input | **Input System** package (new) |
| Voice | Proximity-voice transport (Vivox or Dissonance) — wired via `ProximityVoice` |

Packages are pinned in `Packages/manifest.json`. Open with Unity Hub → 6000.4.4f1.

## 2. First-time setup

1. **Unity Hub → Add → this `unity/` folder.** Open with 6000.4.4f1.
2. Let packages resolve (URP, NGO, Unity Transport, Input System).
3. Open `Assets/Scenes/Harbor.unity` (authored in-editor; only scaffolding + manifest ship in git).
4. Project Settings → Player → **Active Input Handling = Input System Package (New)**.
5. On `NetworkBootstrap`, set the Unity Transport address/port (defaults to 127.0.0.1:7777 for local test).
6. Press Play → **Host**. Build a second player via **ParrelSync**/clone or a built client → **Join**.

## 3. Netcode model (read this first)

- **Server-authoritative.** The host simulates all rigidbodies (salvage, sloop buoyancy). Clients send input;
  the server moves their `CrewController` and reconciles.
- **`Carryable`** objects are `NetworkObject` rigidbodies. Grabs are server RPCs; **co-carry** sums the grab
  forces from multiple crew, which is *the* comedy engine (and why we keep physics forgiving, not sim-accurate).
- **`MatchManager`** holds shared run state in `NetworkVariable`s (build progress, expedition, goal) so every
  client's HUD agrees.
- **Capsize is a soft-fail** by design — cheaper to net-sync and funnier than punishing precision.

## 4. Architecture at a glance

```
NetworkBootstrap        // host/join, Unity Transport, scene load
 └── MatchManager (NetworkBehaviour)     // build progress / expedition / goal (NetworkVariables)
      ├── SwellDirector  // telegraphed weather chaos, server-driven
      ├── BuildPad       // deliver Carryable -> Outpost progress (ServerRpc)
      └── Outpost        // blueprint + completion perk

Per-player (spawned NetworkObject)
 └── CrewController (NetworkBehaviour + PlayerInput)  // server-auth move, grab, yell
      └── ProximityVoice // distance-attenuated voice hook (Vivox/Dissonance)

World
 ├── Sloop (SloopBuoyancy) // buoyancy points + trim/list
 └── Carryable* (NetworkObject rigidbody) // grab / co-carry / build
```

## 5. Unity 6 features used

- **Netcode for GameObjects** + **Unity Transport** for the co-op session.
- **Input System** (`CrewControls.inputactions`) — Move/Grab/Yell/Interact.
- **URP** chunky low-poly look; **Cinemachine 3** crew-framing camera.
- **Awaitable** for async scene/island transitions.
- Physics: `ConfigurableJoint`-based grabbing for forgiving co-carry.

## 6. Where to start reading

`Networking/NetworkBootstrap.cs` → `Core/MatchManager.cs` → `Gameplay/Carryable.cs` →
`Gameplay/CrewController.cs`. Each file maps to the GDD system it implements.

## 7. Build targets

PC (Windows/Mac/Linux) + Steam (Workshop for island/blueprint mods). Console after netcode hardening.
