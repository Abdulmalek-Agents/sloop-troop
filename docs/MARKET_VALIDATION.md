# Sloop Troop — Market Validation & Critic-Cycle Log

This document records the live-signal research and critic cycles that promoted **constructive, non-horror
cooperative ("friendslop")** to the studio's **#2 underserved niche for 2026–2027**.

---

## 1. The thesis in one line

> Co-op is the single most **under-supplied-vs-demand** category on Steam, and the breakout hits cluster in
> **horror/extraction/chaos** — leaving the **constructive, wholesome, build-together** lane wide open at the
> cheap, proximity-voice, streamable tier where the money actually is.

---

## 2. Demand signals (sourced)

### Co-op massively over-performs its share of supply
- VG Insights: co-op was **~6% of Steam releases in 2023 but 36% of all units sold**; the share of new-release
  units rose to **~46% in 2024 YTD**.
  - https://www.gamedeveloper.com/business/study-finds-co-op-games-keep-growing-in-numbers-and-sales-on-steam
  - https://www.statista.com/statistics/1496651/steam-co-op-game-share-new-releases-game-sales/
- Alinea Analytics (via NotebookCheck): **games with co-op modes grossed ~$8.2B on Steam in 2025**; co-op
  generated **>$4.1B in H1 2025 alone (+~11% YoY)** and **Steam's five best-selling new releases in H1 2025 were
  all co-op.**
  - https://www.notebookcheck.net/Games-with-co-op-modes-generated-8-2-billion-in-gross-revenue-on-Steam-in-2025.1218225.0.html
  - https://alineaanalytics.substack.com/p/games-with-co-op-generated-over-4

### The hits cluster in horror/extraction — and there's fatigue
- 2025 was "the year friendslop reigned": Lethal Company, R.E.P.O. (271k+ peak CCU), Content Warning, PEAK.
  - https://www.pcgamer.com/games/2025-was-the-year-friendslop-reigned-and-so-many-low-cost-ways-to-have-fun-with-your-pals-couldnt-have-come-at-a-better-time/
- PC Gamer (firsthand): "I've grown somewhat **weary of the way so many of these games rely on big scares and
  tension**." Documented appetite for non-scare co-op.
  - (same PC Gamer friendslop retrospective)

### PEAK proves the non-horror, constructive-adjacent lane
- PEAK (co-op **climbing**, not horror) sold **1M copies in ~6 days, 5M in the first month, 10M+ total**, built
  by a tiny team in ~4 months — "**Peak shows you don't need horror or quirky party games to be one of the best
  co-ops in years.**"
  - https://en.wikipedia.org/wiki/Peak_(video_game)
  - https://www.gamesradar.com/games/survival/peak-put-friendslop-on-the-map-in-2025...
- Studio head Nick Kaman: friendslop is about "**a real desire to connect and hang out in online worlds**...
  teamwork and communication, as opposed to just testing your individual skill." That is exactly the axis a
  **constructive** game leans into hardest.

### Cheap price is the growth lever
- "Gamble With Your Friends" (~$8) sold **1M copies in its first week**; coverage repeatedly notes ~$8 pricing
  makes it "much easier to convince your whole squad to join."
  - https://www.pcgamer.com/games/puzzle/friendslop-approaches-total-cultural-victory-as-co-op-hit-gamble-with-your-friends-sells-1-million-copies-in-one-week/
  - https://www.g2a.com/news/latest/gamble-with-your-friends-hits-1-million-sales-why-friendslop-games-are-taking-over/

---

## 3. Traction / comparable proof points

| Comp | Signal | Lesson for Sloop Troop |
|---|---|---|
| **PEAK** | 10M+ copies; non-horror; ~4-month dev; Unity (mod-friendly) | Non-horror co-op can be THE breakout; keep it cheap, moddable, proximity-voice |
| **Lethal Company** | 10M+; solo dev; "tapping social dynamics" drove it | Social/proximity-voice loop > production value |
| **R.E.P.O.** | 271k+ peak CCU; physics-based extraction | Physics comedy + co-carry is a proven laugh engine |
| **Content Warning** | 1M in first week (free day-one) | Cheap/viral launch mechanics work |
| **Moving Out / Overcooked** | durable physics co-op franchises | Constructive physics chaos has a long, evergreen tail |

---

## 4. Supply gap (the moat)

The proven, multi-million-selling friendslop formula (cheap + proximity voice + emergent physics comedy) is
almost entirely pointed at **survive/scare/extract**. The **constructive** corner — *build a world together* — at
that same cheap, online, proximity-voice tier is effectively **unoccupied**. Sloop Troop targets exactly that
empty quadrant (see GDD §10), inheriting the category's enormous over-performance while differentiating on tone.

---

## 5. Internal critic-cycle log (the 10 → 2 funnel)

This niche is pick **#2**. (Pick #1 is Dark-Cozy, repo `hearthshade`.) The shared 10-idea funnel:

1. Dark-cozy life-sim (cozy-horror) — *KEEP → #1 (`hearthshade`).*
2. **Constructive non-horror co-op (friendslop)** — *KEEP → #2 (this repo).*
3. Factory/automation sim — *CUT.* 2026 "absolutely stacked" with factory builders.
4. Deckbuilder roguelike — *CUT.* Lightning-in-a-bottle; saturated.
5. Open-world survival craft — *CUT.* Fell out of Steam top-10 genres; high budget.
6. Pure cozy farming sim — *CUT.* Stardew shadow + clone sea.
7. Story-rich narrative — *CAUTION.* Saturated; discoverability hard.
8. Solo analog/found-footage horror — *CAUTION.* Micro-budget flood; low moat.
9. Cozy-automation hybrid — *CAUTION.* Overlaps crowded automation space.
10. Social-deduction/party — *CUT.* Winner-take-all viral lottery.

**Critic challenges raised against #2 and how the design answers them:**
- *"Friendslop is already crowded."* → We do **not** make another horror/extraction title; we take the empty
  **constructive** lane PEAK hinted at and the rest ignored. Differentiation, not imitation.
- *"Networked carry-physics is hard."* → Server-authoritative rigidbodies + forgiving, soft-fail capsize; we
  design *around* jank as comedy rather than fighting for sim-grade fidelity.
- *"No friends = dead game."* → Solo + AI deckhands, quick-match, Discord rich presence, $9.99 squad-buy-in.
- *"Comedy needs scares to spike."* → The **Swell director** + **co-carry physics** + **proximity voice**
  manufacture set-piece moments without horror (the PEAK/Moving Out model).

**Profitability read:** the category over-performs supply by ~6–8× and grossed ~$8.2B on Steam in 2025; comps
(PEAK 10M, Lethal Company 10M, Gamble With Your Friends 1M/week) clear multi-million units at $8–15. A
differentiated *constructive* entry at $9.99 with Workshop mods comfortably meets the studio's
**>60% profitability-confidence + multi-million revenue-potential** bar — gated on netcode execution.

---

## 6. KPIs to track pre-launch

1. **Wishlists** + demo concurrency at Steam Next Fest (primary gate).
2. **Avg party size** in the demo (target ≥3; the social flywheel).
3. **Clip share rate** (capsize/co-carry moments per session) — virality proxy.
4. **Day-1 Workshop submissions** (modding flywheel health).

---

*Compiled by the Intelligence Division. Figures are from public secondary sources dated 2023–2026 and should be
re-pulled at green-light. Market estimates are directional, not guarantees.*
