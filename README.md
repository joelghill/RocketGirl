# RocketGirl
CMPT 306 Game design project.

## Where to get software

* [Tiled2D](http://www.mapeditor.org/): Tile map editor for generating prefabs.
* [SourceTree](https://www.sourcetreeapp.com/): An excellent GUI based version tracking app. Works with BitBucket and GitHub.

## Tiled2d settings

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
