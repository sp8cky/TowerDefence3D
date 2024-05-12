# TowerDefence3D

## Concept
This project is a Tower Defence implementation in 3D using Unity. It follows the classic 2D-TD game, but you can interact with the game as a player and attack enemies yourself and perform other actions. The game is turn-based, between rounds the player has time to build up his defence, then the waves start one after the other, which become more difficult from time to time. See *Project status* for more information.

## Implemented Features:
- Player movement: Control the player character using WASD and Space for jump.
- Ground and logic Grid with Path: When the game starts, spawn and base are displayed and the path between them is coloured.
- Different kinds of enemies with different properies.
- Different kinds of towers, which can be placed right and left the path.
- Score and health system: The player receives points for destroying the enemies. The base loses life when enemies reach it. The player can also be attacked by some opponents and loses health. The game ends, when the base has no health anymore. The player can't do actions, if he looses all health.

## Important Notes:
- Unity Version: This project was developed using Unity LTS version 2022.3.26f1.
- Compatibility: The game is designed for 3D gameplay and is compatible with both desktop and mobile platforms.
- Code Structure: The project follows a modular code structure, making it easy to understand and extend. Scripts are organized into logical components such as PlayerController, EnemyController, and GameManager.
- Feedback and Contributions: I haven't been working with Unity for that long, Feedback and contributions are welcome! If you encounter any issues or have suggestions for improvements, feel free to open an issue or submit a pull request on GitHub.

## Project status
- Still in progress

### Future implementations
- new Enemy-Types, attacks
- UI
- Player attacks

## Getting Started:
- Clone or download the project repository from [git@github.com:sp8cky/TowerDefence3D.git](https://github.com/sp8cky/TowerDefence3D)
- Open the project in Unity.
- Explore the scripts and scene files to understand the project structure and gameplay mechanics.
- Make adjustments or modifications as desired to customize the game.
- Test the game in the Unity Editor or build and deploy it to your target platform.

### Customization Options:
- Adjust player movement speed: Players can tweak the movement speed of the player character in the PlayerController script.
- Modify enemy behavior: Users can customize various aspects of the enemy behavior, such as movement speed, spawn rate, and attack patterns, by adjusting parameters in the EnemyController script.
- Change game visuals: Users can replace the default sprites with their own artwork to customize the game's appearance.

## Credits:
- This project was created by sp8cky.

## License:
- This project is licensed under the MIT-License. See the LICENSE file for details.

