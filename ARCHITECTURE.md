<!--
  Goes in the public GEODE showcase repo as ARCHITECTURE.md.
  This is a writeup, not source. Fill in the TODOs with your real specifics — keep it honest
  and concrete. It shows recruiters how you think, and it doubles as your interview prep for
  "tell me about a hard technical problem on GEODE."
  Tip: 1-2 small diagrams or screenshots here go a long way.
-->

# GEODE — Architecture & Engineering Notes

GEODE is a co-op survival + tower-defense game built solo in **Unity / C#**, shipped to Steam in June 2026 (100% positive rating). This is a high-level look at how it's built. Full source is private; happy to walk through it on request.

## At a glance
- **Engine / language:** Unity, C#
- **Multiplayer:** Unity Netcode for GameObjects + peer-to-peer, up to 12 players
- **Scope:** solo-developed over ~2.5 years (with a full architecture rewrite partway through)

## Multiplayer & networking
Real-time co-op for up to 12 players using Unity's Netcode for GameObjects over a peer-to-peer model.
- **Authority model:** I went with a fully Server-Authoritative model. The host owns the source of truth, and connected players have to request things like dealing damage, picking up items, and interacting with inventory systems. Pairing this with local prediction removes any feeling of lag, but keeps the fully secured server state, which ensures a properly synced game. 
- **Hardest networking problem you solved:** The biggest hurdle I ran into was easily dealing with game lifecycle issues. Properly shutting down lobbies and cleanly returning clients to the main menu was a big headache. This wasn't necessarily a difficult issue to solve, but it became very complex and twisted because I did a poor job setting it up the beginning. Thinking about cleanup from the get-go would have resulted in a much easier job rather than needing to go back and ensure everything had a single entry point to clean up.

## The rewrite (why I restarted)
About a year into development I rebuilt the project from the ground up.
- **What went wrong in v1:** Scalability issues. Inheritence was sloppy, didn't use interfaces. The thought of continuing ontop of a poor foundation was dreadful, opted for a full restart instead.
- **What changed in v2:** applied SOLID principles and clearer OOP class hierarchies so new systems could be added without touching unrelated code.
- **Result:** Cleaner, scalable container/inventory systems, full Multiplayer, Successful Steam release.

## Core gameplay systems
- **Tower defense / enemy waves:** Scaling enemies each night, lots of tower variations to defend with, replayability mechanics with unlockable perks and progression.
- **Survival / world systems:** Gathering resources, discovering abandoned ruins and camps, discovering mysterious statues in the wild...
- **A system you're proud of:** Fully implementing multiplayer using Unity's NGO system was a huge success for me. Started off with youtube tutorials, and then scaled up to support larger systems. This was a lot of work and hours upon hours of bugfixing, but oh man was it worth it.

## Performance
Profiled and optimized with the **Unity Profiler**.
- **What you optimized:** I had lots of issues with lag on larger worlds, simply because of the amount of objects in the world. My first thought was to implement chunk culling, but after weeks implementing it, the profiler showed no change. After digging deeper, I discovered that deactivated network objects (the environment trees, rocks), still take up update time even though they're deactive. So I switched to a seeded chunk loading system similar to Minecraft to load chunks on demand. I immediately saw results, and the profiler confirmed an 80% performance increase in framerate, and significantly lower network traffic.

## What I'd do differently
I went with a Server-Authoritative approach, but looking back I'm not sure if it was the right call. The game was intended to be played between friends, and one of the biggest benefits of a server-controlled system is the source of truth. This makes it more difficult to cheat, but it requires a bit more nuianced code and depth. I think if people want to cheat in a game with their friends, go ahead and let them, it might have given me more time to develop other features and content. 

---
*Built by [Quincy Stokes](https://www.quincystokes.com/) [Play on Steam](https://store.steampowered.com/)*
