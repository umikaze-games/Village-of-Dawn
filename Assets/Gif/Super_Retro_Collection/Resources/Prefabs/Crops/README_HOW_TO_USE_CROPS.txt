HOW TO USE CROPS PREFABS EFFICIENTLY

====================================================
Context
====================================================

Crops have multiple stages of growth, so you may want to change their sprite at certain points.

====================================================
Script
====================================================

To change the stage of a crop, you can use the attached script SpriteSelectFrame, 
located at Assets/Gif/Super_Retro_Collection/Scripts/SpriteSelectFrame.cs.

The script automatically assigns the SpriteRenderer attached to your crop prefab :

 - You can adjust the crop stage by changing the value of the public variable "Sprite Index" (from 0 to N).
 - You can also change the spritesheet location if necessary from the public variable "Spritesheet location".

====================================================
Note
====================================================

It works in both Edit mode and Play mode for a smoother experience. :)

