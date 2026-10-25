# Tilemania

A 2D wave-survival platformer made with Unity 6. Fight endless waves of zombies, slimes and ghosts, collect coins, and spend them on upgrades between waves.

<!-- Add a gameplay GIF here: ![Gameplay](docs/gameplay.gif) -->
<!-- Play in the browser: https://<your-itch-username>.itch.io/tilemania -->

## Gameplay

- **Endless waves.** Every wave has more enemies, and they get tougher and faster. Elite enemies (red) appear more often over time and drop extra coins.
- **Three enemy types.** Zombies chase you and jump over walls, slimes wait and leap at you, ghosts fly through the level.
- **Upgrade shops between waves.**
  - *Statue of Vigor:* max health, move speed, dodge, heal
  - *Blacksmith's Anvil:* fire rate, damage, multi-shot
- **Mouse aiming** with a rotating gun, knockback, invulnerability frames and a dodge dash.

## Controls

| Action | Key |
|---|---|
| Move | A / D |
| Jump | Space |
| Aim / Shoot | Mouse / Left click |
| Interact (start wave, open shop) | E |
| Dodge (after unlocking) | Shift |
| Pause | Esc |

## Technical highlights

- **Component-based player:** movement, shooting, aiming, health, wallet and upgrades are separate components with single responsibilities.
- **Event-driven communication:** systems talk through C# events (`Health.Died`, `WaveManager.WaveCompleted`, `PlayerWallet.CoinsChanged`), so UI, audio and lighting react without being referenced by gameplay code.
- **Data-driven design with ScriptableObjects:** wave difficulty, the enemy spawn table, every upgrade, shop catalogs, audio and UI theme are assets that can be tuned without touching code.
- **Strategy pattern for upgrades:** each upgrade is a small `UpgradeDefinition` subclass; adding a new upgrade means adding a class and an asset.
- **Interfaces for swappable behaviour:** `IEnemyMovement` (walking, jumping, flying enemies) and `IInteractable` (wave stone, shops).
- **State machines** for the game flow (menu, playing, shopping, paused, game over), the slime behaviour and the wave loop.
- **Explicit dependencies:** scene references are assigned in the Inspector and spawned enemies receive theirs through `Initialize(...)`.
- **Code-built UI:** menus and shop screens are generated at runtime from data by a small UI factory.

## Project structure

```
Assets/
  Scripts/      Audio, Combat, Coin, Core, Enemies, Environment, Interaction, Managers, Player, UI, Upgrades
  Data/         WaveSettings, upgrades, shop catalogs, audio library, UI theme
  Prefabs/      Player, enemies, managers, environment
  Scenes/       Level1
  ThirdParty/   Licensed art and audio (not included in this repository)
```

## Running the project

1. Open the project with **Unity 6000.3** (URP 2D).
2. Third-party assets are not redistributed in this repository. Download the packs listed below and place them under `Assets/ThirdParty/`.
3. Open `Assets/Scenes/Level1.unity` and press Play.

## Credits

Art and audio are used under their respective licenses:

- **Pixel Art Platformer – Village Props** by [Cainos](https://cainos.itch.io/pixel-art-platformer-village-props)
- **Background layers** by [GandalfHardcore](https://gandalfhardcore.itch.io/)
- **Undead Survivor Assets Pack** by [Goldmetal](https://assetstore.unity.com/packages/2d/undead-survivor-assets-pack-238068) (characters, enemies, sounds, font)
- Course sprites from **GameDev.tv**
- Coin pickup sound from [Pixabay](https://pixabay.com/)
