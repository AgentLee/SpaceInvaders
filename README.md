# SpaceInvaders
CIS568 Assignment 2: Deconstructing a Classic

## Todo
- ~~Invulnerability~~
- Horde Attack (time till next horde)
- Resources
    - Lives
    - Shields
    - Shot types (burst fire, etc)
- New Models
- Need to restructure enemy code
- ~~Add explosion to when the player is out of position.~~
- Monster attack
- Might need to add more functionality to the player when it can go into space.
- ~~Add collision between player and enemies~~

## Bugs
- Red UFO spawning
    - ~~Only breaks when the player hits the RedUFO~~
    - A bunch of them spawn after invincibility
- Invincibility
    - ~~Need the player to return to the safe zone~~
- Bullet placement for enemies
    - They don't even spawn now lol.
- ~~Player rotation about x-axis at the start of the game.~~
- When the player gets destroyed twice it won't spawn again.
- ~~Player falls to the abyss once passed the line.~~
- ~~Player can't move after lerping back to startPos.~~ 
    - Took the lerp out of the coroutine and did it once it finished. I have a lot of flags now though.
- ~~Need to fix coroutine for spawn~~ 
    - Reorganized the coroutine
- Extra life doesn't show up (counter works but object doesn't).

