# Sloop Troop — Unity Project Structure, Scene Manifest & Prefab Specs

## Folder layout

```
unity/
├── Packages/manifest.json              # NGO, Unity Transport, URP, Input System, Cinemachine
├── ProjectSettings/ProjectVersion.txt  # 6000.4.4f1
├── README.md
├── ProjectStructure.md                 # this file
└── Assets/
    ├── Scripts/
    │   ├── Core/        GameBootstrap, MatchManager (NetworkBehaviour)
    │   ├── Networking/  NetworkBootstrap, ProximityVoice
    │   ├── Gameplay/    CrewController, Carryable, SloopBuoyancy
    │   ├── Build/       BuildPad, Outpost
    │   ├── Director/    SwellDirector
    │   ├── UI/          HUDController
    │   └── Data/        SalvageData, OutpostBlueprint
    ├── Scenes/          Boot.unity, Harbor.unity            (authored in-editor)
    ├── Prefabs/         (specs below)
    ├── Art/             low-poly sloop, salvage, island kit
    ├── Audio/           splashes, bonks, foghorn, shanty stems
    └── Settings/        URP asset, CrewControls.inputactions, NetworkManager config
```

## Scene manifest

### `Boot.unity`
- `NetworkBootstrap` — host/join UI + Unity Transport; loads `Harbor` over the network.

### `Harbor.unity` (vertical slice)
| Object | Components | Notes |
|---|---|---|
| `NetworkManager` | NetworkManager, UnityTransport | NGO singleton |
| `MatchManager` | MatchManager (NetworkBehaviour) | build/expedition/goal NetworkVariables |
| `Sloop` | SloopBuoyancy, Rigidbody, buoyancy points | the shared deck; trim/list |
| `Stations` (Helm/Sail/Anchor/Bilge) | StationInteractable | require a crew member each |
| `BuildPad` | BuildPad (ServerRpc deliver) | converts salvage → Outpost progress |
| `Outpost_Lighthouse` | Outpost | blueprint + completion perk (reveal next island) |
| `SwellDirector` | SwellDirector | telegraphed weather chaos (server) |
| `SalvageSpawner` | spawns `Carryable` NetworkObjects | crates + heavy beams |
| `CrewSpawnPoints` | transforms | up to 6 player spawns |
| `HUDCanvas` | HUDController | build %, trim, swell timer, crew, carry |
| `CM crewCam` | CinemachineCamera | frames the crew/deck |

## Prefab specs

**`Crew.prefab`** (player, NetworkObject)
- `CrewController` (NetworkBehaviour) — server-auth movement; Grab/Yell/Interact via `CrewControls`.
- `ProximityVoice` — distance-attenuated voice channel; "speaking" ring on local + remote avatars.
- Capsule + chunky low-poly mesh + ragdoll for capsize gags.

**`Carryable.prefab`** (NetworkObject)
- `Carryable` + `Rigidbody` + `NetworkRigidbody`. Fields: `weightClass` (Light/Heavy), `salvageValue`.
- Grab = server RPC creating a `ConfigurableJoint` to the carrier hand; **Heavy** needs ≥2 joints (co-carry).

**`Sloop.prefab`** (NetworkObject)
- `SloopBuoyancy` samples N buoyancy points; **trim** from cargo distribution lists the deck and slides loose
  `Carryable`s; bilge fills in storms (pump station empties it). Capsize = soft-fail scatter.

**`Outpost_Lighthouse.prefab`**
- `Outpost` with a `OutpostBlueprint` (piece count + perk). Completion grants a crew-wide perk and reveals the
  next island.

**`BuildPad.prefab`**
- Trigger volume; `DeliverServerRpc(NetworkObjectReference salvage)` consumes salvage and advances the Outpost.

## Data (ScriptableObjects)

- `SalvageData` — id, displayName, weightClass, salvageValue, mesh.
- `OutpostBlueprint` — id, requiredPieces, completionPerk (enum), revealsNextIsland (bool).

## Difficulty / tuning knobs (MatchManager)

| Knob | Effect |
|---|---|
| Crew size scaling | goal + spawn rate scale to party size |
| Swell cadence | seconds between swells; shortens as loose-cargo rises |
| Trim tolerance | loose-cargo count before the deck lists/capsizes |
