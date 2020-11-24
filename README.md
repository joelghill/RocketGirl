# RocketGirl
Play it here: http://joel.hillspawn.com/RocketGirl/

## Where to get software

* [Tiled2D](http://www.mapeditor.org/): Tile map editor for generating prefabs (No longer really used). 
* [SourceTree](https://www.sourcetreeapp.com/): An excellent GUI based version tracking app. Works with BitBucket and GitHub.


## Important Files and What They Do

#### Assets/
* SpriteCollider.cs - Script to handle collision logic for entities. Based on sending raycasts.
* Rotate.cs - Script to rotate the level and level objects to shift player perspective
* zPosition.cs - script to position the attached object into the appropriate z depth.

#### Assets/Utilities/
* Trile.cs - representation of our basic level building block. Is basically a game object with 6 sprites forming a 3D cube.

#### Assets/Player/
* Avatar.cs - base class that handles entitiy movement. Also currently resolves collsions.
* FollowTarget.cs - simple script that has camera follow target movement.

#### Assets/MegaMan/
* Player.cs - Extends Avatar class. Handles player movement and user interaction.

#### Assets/Editor/

This folder has many helper scripts used to generate "Triles" from sprites, edit levels in 3D, or import levels from Tiled2D (a now outdated and broken script).


## Tiled2d settings (Depreciated - this appoach no longer used in game)

Importing tile maps and levels from Tiled2D relies on setting the properties on a per tile basis. Currently, the properties for setting the cube faces and collision behaviours are customizable:

* "collider" - can be set to these types:
  * "None" - object will not collide with anything
  * "Solid" - will collide with everything from all sides. Entities cannot move through these blocks
  * "SemiSolid"  - One way platforms
  * "Moveable" - Solid blocks, but can be moved by player.

* To set the faces of the cubes to specific tiles, set each of the following to the index of the tile you want:
  * "top"
  * "bottom"
  * "left"
  * "right"
  * "back"

The front of the cubes are always set to the index of their tile.

### Default Properties for tiles

* "collider" - None
* All sides are set to the index of the tile.
