

## Scene Loading


key points:
- loading does not pause transport
- actiontype => single/additive
- scene instance Id => unique ID used to keep track of loading, can use this id later to keep track of which scenes are loaded
- load for list of connections instead of all (can be all too, but options)
    - server keeps track of which scene are loaded for each clients, good for:
        - multiple matches 
        - AOI in large worlds 

function logic
```
load scene for connections:
    create scene message { scene to load, actiontype, scene instance Id }
    send to each connection
    if scene not loaded
        load scene on server
    
    // later
    ON CLIENT: 
        start scene load
        use completed event to reply to server
            send to server { scene instance Id }
    ON SERVER:
        event for each connection finished loading
            if server loaded
                send spawn message for scene objects (respawn players????)
        event for server finished loading
            for each loaded connection
                send spawn message for scene objects (respawn players????)
        event for all connections + server finished loading
            // user can use this event to start the game
```            

scene instance Id:
- can use unity's scene handle
- server will send its handle to client
    - client will keep map of serverID -> localID
- server sends this Id with message so client knows:
    - which scene to spawn objects in
    - which scene sceneObjects belong to



## Spawning scene objects

```
struct SpawnMessage
{
    netid,
    spawnID,
    spawnType,
    scene,
    ...other initial state
}
emum SpawnType 
{
    prefab,
    prefabPool?,
    sceneActionId, (see above)
    custom,
}
```

spawnID:
- can be ulong or uint
- replaces sceneId and assetId
- must be unique within a scene
    - doesn't have to be unique between scene

