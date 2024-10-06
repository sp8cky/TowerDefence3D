# TowerDefence3D

This project is a Tower Defence implementation in 3D using Unity. It follows the classic 2D-TD game, but you can interact with the game as a player and attack enemies yourself and perform other actions. The game is turn-based, between rounds the player has time to build up his defence, then the waves start one after the other, which become more difficult from time to time. See [Project status](#project-status) for more information.

1. [Implemented Features](#Implemented-Features)
2. [Installation](#installation)
3. [Contributing](#contributing)
4. [License](#license)
5. [Credits](#credits)
6. [Acknowledgments](#acknowledgments)
   
## Implemented Features:
- Player movement: Control the player character using WASD and Space for jump.
- Ground and logic Grid with Path: When the game starts, spawn and base are displayed and the path between them is coloured.
- Different kinds of enemies with different properies.
- Different kinds of towers, which can be placed right and left the path.
- Score and health system: The player receives points for destroying the enemies. The base loses health when enemies reach it. The player can also be attacked by some opponents and loses health. The game ends, when the base has no health anymore. The player get freezed, if he looses all health.
- Rounds and game states
- UI with important informations
- Camera switch between game and player view

### Project status
- Still in progress

### Future implementations
- new Enemy-Types, attacks
- UI
- Player attacks

## Installation
### Prerequisites and important info
- Unity Version: This project was developed using Unity LTS version 2022.3.26f1.
- Compatibility: The game is designed for 3D gameplay and is compatible with desktop platforms.

### Installation Steps
Clone the repository and add it to Unity Hub
```bash
git clone https://github.com/sp8cky/Space-Survivor
```

### Customization Options:
- Adjust player movement speed: Players can tweak the movement speed of the player character in the PlayerController script.
- Modify enemy behavior: Users can customize various aspects of the enemy behavior, such as movement speed, spawn rate, and attack patterns, by adjusting parameters in the EnemyController script.
- Change game visuals: Users can replace the default sprites with their own artwork to customize the game's appearance.


## Contributing
- Feedback and contributions are welcome! If you encounter any issues or have suggestions for improvements, feel free to open an issue or submit a pull request on GitHub.

### How to Contribute
1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit your changes: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin feature/your-feature`
5. Open a pull request

## License:
- This project is licensed under the MIT-License. See the LICENSE file for details.

## Credits:
- This project was created by sp8cky.

## Acknowledgments

