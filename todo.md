# SpaceInvaders
CIS568 Assignment 2: Deconstructing a Classic

## Todo
- ~~Need to restructure enemy code~~
    - Need to move back to `EnemyController`. 
        - Made each group of enemies into a prefab.
            - Each group gets `NewEnemyScript` which controls movement and shooting.
            - Each individual enemy gets `NewEnemyControllerScript` which handles collisions.
- Horde Attack 
    - ~~Timer~~
    - ~~Decide how they will attack~~
    - ~~Move to coroutine~~
    - Scoring system
    - Star field
- Monster attack
- Resources (Only have them spawn during the invulnerability)
    - Lives
        - Max at 5
        - They become points after 5th life.
    - Shields
        - Bases can take more damage
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
- ~~Bullet placement for enemies~~
    - ~~They don't even spawn now lol.~~
- ~~Extra life doesn't show up (counter works but object doesn't).~~
    - Changed the concept
    - Need to fix life hiding from (left to right) to (right to left)
- Explosions show up behind the object
    - Can be fixed with Orthographic camera
        - But this loses 3D-ness
- ~~When the player gets destroyed twice it won't spawn again.~~
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


