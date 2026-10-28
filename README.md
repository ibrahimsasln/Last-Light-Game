# Last Light

<img src="docs/cover.png" alt="Last Light" width="420">

[Play it in the browser on itch.io](https://brahimux.itch.io/lastlight)

A 2D wave-survival platformer made in Unity 6. You defend a village at night against zombies, slimes and ghosts, and spend the coins they drop on upgrades between waves.

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

Scripts are in `Assets/Scripts` with one folder (and namespace) per feature. Balancing data is in `Assets/Data`.

**Waves.** `WaveManager` is a small state machine, either preparing or in a wave. Everything it needs comes from a `WaveSettings` ScriptableObject: enemy count, spawn rate, how much enemies scale each wave, the elite chance and a weighted spawn table. Tuning difficulty never means touching code. When a wave starts or ends it raises `WaveStarted` or `WaveCompleted`. The wave text, the lighting and the music subscribe to those events, and `WaveManager` has no idea they exist. Most of the game is wired this way (observer pattern with plain C# events), which keeps systems from depending on each other directly.

**Enemies.** Zombies, slimes and ghosts all use the same `Enemy` and `Health` components. The only thing that changes is movement, which sits behind an `IEnemyMovement` interface. Zombies walk and jump over walls, slimes leap at you (a little state machine of its own) and ghosts fly through walls. A new enemy type is mostly a new movement script plus a prefab.

**Object pooling.** Bullets, coins and enemies come from `UnityEngine.Pool` instead of being instantiated and destroyed, so nothing piles up for the garbage collector during a wave. Enemies were the tricky part, because a reused enemy keeps the state it died with. `Enemy` saves its base health, damage and coin count in `Awake` and recalculates them in `Initialize`. Without that, an elite zombie going back to the pool would come out as a normal zombie with double health. `EnemyPool` keeps one pool per enemy prefab, and anything that falls off the map is sent back through an `IDespawnable` interface instead of being destroyed.

**Upgrades and shops.** Every upgrade is a ScriptableObject that inherits from `UpgradeDefinition` and implements its own `Apply`. Prices grow exponentially and there's no level cap. A shop is just a list of upgrades (`ShopCatalog`), so the statue and the anvil are the same script with different data. The stone and both shops share a base class, `BetweenWavesInteractable`, which blocks interaction during a wave; each one only fills in `OnInteract`.

**Player.** Instead of one big Player script, the player is a set of small components: movement, aiming, shooting, health, wallet, upgrades and interaction. They talk through events. For example the audio plays the shot sound by listening to `PlayerShooting.Fired`.

**Game flow and UI.** `GameManager` holds a `GameState` (main menu, playing, shopping, paused, game over). Changing state sets the time scale and switches the Input System between the Player and UI action maps. The menus are built from code by `UIFactory`, with the font, colours and sprites coming from a `UITheme` asset.

**Dependencies.** There are no singletons and no `Find` calls. Scene objects are linked in the Inspector, and spawned objects get what they need through an `Initialize` method.

**Unity features used.** Tilemaps with a Rule Tile, and a Composite Collider so the ground is a single collider. Cinemachine with a Confiner 2D to keep the camera inside the map. URP 2D lights for the day and night change. Parallax background layers. Animator parameters cached as hashes. Music crossfading between two audio sources. High scores and the volume setting are saved with PlayerPrefs, the scores as JSON. The build target is WebGL.

## What I'd do next

- Unit tests for the parts that are pure logic, like `WaveSettings` and `HighScoreTable`.
- Build the menus as prefabs instead of in code, so they can be laid out in the editor.

## Running it

Open the project with Unity 6000.3 and open `Assets/Scenes/Level1.unity`.

The art and audio packs are not included in this repository because their licenses don't allow redistribution. To run the project you need to download them (links below) and put them in `Assets/ThirdParty/`.

## Credits

- Village props and tiles: [Cainos](https://cainos.itch.io/pixel-art-platformer-village-props)
- Background: [GandalfHardcore](https://gandalfhardcore.itch.io/)
- Characters, enemies, UI sprites, music and sound effects: [Undead Survivor Assets Pack by Goldmetal](https://assetstore.unity.com/packages/2d/undead-survivor-assets-pack-238068)
- Font: [Pixelify Sans](https://fonts.google.com/specimen/Pixelify+Sans) (SIL Open Font License)
- Some sprites from the GameDev.tv course
