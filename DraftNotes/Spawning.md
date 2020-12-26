# Spawning

Current the SpawnMessage is used for many things:
- Link and enable scene objects using sceneId
- Instantiate a new object from a prefab using AssetId
- Update properties of an existing object


Rough notes:
- does too much
- updates all properties not just one changed ones
    - means position changes when changing authority
- sceneid + assetid have to be send with every message
- Client handler needs many checks in the spawn function to work out which method to use to Instantiate or enable the object
- custom spawn handler use assetId when they could use a hash or id that would be shorter
- SpawnMessage is also used for visibility, meaning if an object leaves and enter visibility it is fully respawned even if none of its properties have changed

https://github.com/vis2k/Mirror/issues/2256