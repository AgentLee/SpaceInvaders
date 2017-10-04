# SpaceInvaders
CIS568 Assignment 2: Deconstructing a Classic

# Overview

The goal of this project was to familiarize myself with developing in Unity by recreating Space Invaders and putting my own twist to it.

# Demo

 [![](demo/demo_screenshot.png)](https://vimeo.com/236786590)

# How to Play

**Goal**: Destroy all the enemy ships before you run out of lives, the bases get destroyed, or invade the base. 

| Key                   | Movement      |
| -------------         |-------------  | 
| W/Up Arrow            | Move Up       | 
| A/Left Arrow          | Move Left     | 
| S/Down Arrow          | Move Down     | 
| D/Right Arrow         | Move Right    | 
| Space Bar/Left Click  | Fire Bullet   | 

# Original Gameplay
All the features and game mechanics from the original Space Invaders are implemented. 
- Enemy UFO's...
    - move faster as their numbers decrease and when they move down
    - shoot at the player at various rates and times throughout the game
    - give the player points (10, 20, 30) when destroyed
- Players...
    - start with 3 lives
    - shoot at the enemies to move onto the next level
- Red UFO's...
    - spawn periodically throughout the game
    - give the player a random point value (50, 100, 150) when destroyed
- Game Over
    - All the bases are destroyed
    - Player loses all their lives
    - The enemies reach the bases
- Level Up
    - All the enemies get destroyed
    - Enemies spawn closer to the base

# Remixed Gameplay
All of the original gameplay is still the same but with extra features and mechanics. 

- Red UFO's allow the player to become "invincible" and move all around the screen.
    - They are only given 10-20 seconds to destroy as many enemies as they can or grab as many resources as they can before time runs out.
    - When there are 5 seconds left, the status bar warns the player to return to the safe zone.
        - If they don't, they get destroyed and lose a life.
- Horde Atack
    - The idea behind this was that the player gets launched into space via hyperspace and they have to destroy as many enemies that fly past them as they can. For every enemy that gets past the player, they lose a point. I have plans to have these enemies appear in the original game screen as continuation. So if 5 enemy ships get past the player, then 5 would show up when the horde attack is over. 
    - There is a timer in the top right corner that counts down to when the next attack will occur. 
- Resources can appear at random times. The player shoots at them to get them. 
    - Extra lives
    - Shields
        - Bases don't lose health when the enemy or player shoot at them for about 3 seconds.
            - These *should* be able to stack, so if a player shoots at 3 shield resources then they get 9 seconds of freedom.
    - Bullet types (not yet implemented)
        - Burst fire
        - Nuke

# Future Enhancements
- Add bullet types
- Create a low poly aesthetic 
- Fix hyperspace effect
- Tweaks to timing 
- Tweaks to shielding 