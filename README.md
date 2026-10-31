# Last Light

<img src="docs/cover.png" alt="Last Light" width="420">

[Play it in the browser on itch.io](https://brahimux.itch.io/lastlight)

A 2D wave-survival platformer made in Unity 6. You try to survive the night against waves of zombies, slimes and ghosts, and spend the coins they drop on upgrades between waves.

It started as the TileVania project from the GameDev.tv Unity 2D course. After finishing the course I kept going and turned it into a different game, mostly to practise structuring a project properly.

## How it plays

You start a wave by touching the magic stone. The sky gets darker, the music changes and enemies start coming in from the edges of the map. When the last one dies the light comes back, the leftover coins fly to you and you can visit the two shops:

- the statue sells max health, move speed, a dodge and healing
- the anvil sells fire rate, damage and multi-shot

There is no level cap, but prices go up every time you buy something. Waves keep getting bigger and enemies get more health, damage and speed, so every run ends at some point. Some enemies spawn as red elites with more health and more coins. Your ten best runs are saved locally.

## Screenshots

![Wave 1](docs/gameplay.png)

<img src="docs/menu.png" alt="Main menu" width="32%"> <img src="docs/shop-statue.png" alt="Statue shop" width="32%"> <img src="docs/shop-anvil.png" alt="Anvil shop" width="32%">

## Controls

| Action | Key |
|---|---|
| Move | A / D |
| Jump | Space |
| Aim and shoot | Mouse |
| Start wave / open shop | E |
| Dodge (after buying it) | Shift |
| Pause | Esc or P |

## How it's built

Scripts are in `Assets/Scripts`, one folder per feature. Balancing data is in `Assets/Data`.

- **Waves:** `WaveManager` is a small state machine. Enemy counts, scaling and the spawn table come from a `WaveSettings` ScriptableObject.
- **Events:** systems talk through C# events (observer pattern). The wave text, lighting and music listen to `WaveStarted` and `WaveCompleted`, and `WaveManager` doesn't know about them.
- **Enemies:** all types share `Enemy` and `Health`. Only movement changes, behind `IEnemyMovement` (strategy pattern).
- **Object pooling:** bullets, coins and enemies are reused with `UnityEngine.Pool`. Enemies recalculate their stats from base values when they respawn, so an elite doesn't come back with double health.
- **Upgrades:** each one is a ScriptableObject subclass of `UpgradeDefinition`. A shop is just a list of them, so the statue and the anvil run the same script.
- **Interactables:** the stone and shops inherit from `BetweenWavesInteractable` and only implement `OnInteract` (template method).
- **Game flow:** `GameManager` is a state machine that sets the time scale and switches input maps. `UIFactory` builds the menus from code.
- **Dependencies:** no singletons or `Find` calls. References are set in the Inspector or passed through `Initialize`.

Also uses Tilemaps with a Rule Tile, Cinemachine, URP 2D lights, the new Input System and PlayerPrefs with JSON for high scores. Built for WebGL.

## Running it

The art and audio packs are not included in this repository because their licenses don't allow redistribution. To run the project you need to download them (links below) and put them in `Assets/ThirdParty/`.

## Credits

- Village props and tiles: [Cainos](https://cainos.itch.io/pixel-art-platformer-village-props)
- Background: [GandalfHardcore](https://gandalfhardcore.itch.io/)
- Characters, enemies, UI sprites, music and sound effects: [Undead Survivor Assets Pack by Goldmetal](https://assetstore.unity.com/packages/2d/undead-survivor-assets-pack-238068)
- Font: [Pixelify Sans](https://fonts.google.com/specimen/Pixelify+Sans) (SIL Open Font License)
- Some sprites from the GameDev.tv course
