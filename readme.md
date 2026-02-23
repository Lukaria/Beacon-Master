# Beacon Master
Beacon Master is a Unity mobile game where you control a lighthouse beam to highlight approaching boats and defend yourself. The game features various boats, obstacles, locations, and beacons. As the game progresses, the difficulty increases, but players can choose abilities to help survive (these have different drop rates, so choose wisely!). Since the game is endless, the goal is to score as many points as possible by highlighting boats and obstacles.
Some promo screenshots of the game:

<p align="center">
    <img src="https://github.com/Lukaria/Beacon-Master/blob/main/PromoMaterials/locations_image.png" width="10%" />
    <img src="https://github.com/Lukaria/Beacon-Master/blob/main/PromoMaterials/beacons_image.png" width="10%" /> 
    <img src="https://github.com/Lukaria/Beacon-Master/blob/main/PromoMaterials/abilities_image.png" width="10%" /> 
    <img src="https://github.com/Lukaria/Beacon-Master/blob/main/PromoMaterials/defend_image.png" width="10%" /> 
    <img src="https://github.com/Lukaria/Beacon-Master/blob/main/PromoMaterials/mainmenu_image.png" width="10%" /> 
    <img src="https://github.com/Lukaria/Beacon-Master/blob/main/PromoMaterials/gameplay_image.png" width="10%" /> 
</p>


## Features
This project utilizes several technologies to optimize performance and architecture: 
- UniTask: Used for efficient asynchronous code to reduce memory allocations.
- Addressables: Simplifies content management and asynchronous asset loading. 
- Zenject: Handles dependency injection across all contexts (Project, Scene, GameObject) and manages event-based logic via SignalBus.
- Unity Ads: For ad implementation.
- R3: To react to game value updates across different systems.
- MessagePack: Provides encrypted local storage for user save data.
- DOTween: Powers visual effects like animations and camera shakes.
- Cinemachine: Manages camera angles and movement.
- Zlinq: Reduces memory allocations when working with collections.
- Nice Vibrations: Implements haptic feedback for mobile devices.

I also utilized Unity features such as Decals, Shaders, and Post-Processing to create the environment and beacon beams. These add significant visual depth with a very low performance cost.

## Q&A
- How can I play?
    The game is currently in the testing phase (Google Play requires 12 testers for public release). You can join as a tester via [Link] or download the latest release directly from GitHub (available for iOS and Android).
- Why balance is so broken sometimes?
    The game is currently tuned to show players as much content as possible in a short timeframe. In rare cases, this means you might lose quickly. Good news: I hate intrusive ads, so there is only one ad placement that can be easily skipped using in-game currency.
- Found a bug?
    Bugs can happen, but I tried to fix all that i have found. If you found any it will be awesome if you contact me via LinkedIn or github issue for this repo, I strongly appreciate it
- How is the code quality?
    Well, code is not ideal and I know about it (small todos, suddenly appeared god object), but Im tired a little of this project and sometimes things do not need to be perfect. I plan to fix this issues later, despite this code is pretty good in my opinion




