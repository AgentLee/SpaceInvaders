# SpaceInvaders
CIS568 Assignment 2: Deconstructing a Classic

## Todo
- Need to restructure enemy code
- Horde Attack 
    - ~~Timer~~
    - Decide how they will attack
    - Move to coroutine
- Monster attack
- Resources
    - Lives
    - Shields
    - Shot types (burst fire, etc)
- New Models
- Might need to add more functionality to the player when it can go into space.
- High Scores
- Level Up
    - Need to adjust enemy speeds
- ~~Invulnerability~~
- ~~Add explosion to when the player is out of position.~~
- ~~Add collision between player and enemies~~

## Bugs
- Bullet placement for enemies
    - They don't even spawn now lol.
- Extra life doesn't show up (counter works but object doesn't).
- Explosions show up behind the object
    - Can be fixed with Orthographic camera
        - But this loses 3D-ness
- When the player gets destroyed twice it won't spawn again.
- ~~Red UFO spawning~~
    - ~~Only breaks when the player hits the RedUFO~~
    - ~~A bunch of them spawn after invincibility~~
        - I think this was implicitly fixed with the invincibility change.
- ~~Invincibility~~
    - ~~Need the player to return to the safe zone~~
- ~~Player rotation about x-axis at the start of the game.~~
- ~~Player falls to the abyss once passed the line.~~
- ~~Player can't move after lerping back to startPos.~~ 
    - Took the lerp out of the coroutine and did it once it finished. I have a lot of flags now though.
- ~~Need to fix coroutine for spawn~~ 
    - Reorganized the coroutine


