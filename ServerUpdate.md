# ServerUpdate

Current `NetworkServer.Update` loops through all spawned objects and Actively checks if an object is dirty. If they are dirty then it sends a message to every observers with the changed data:

```
foreach spawned NetworkIdentity
    if not null (unity equals)
        if has Observers
            Get Writers from Pool

            foreach NetworkBehaviour
                if dirtyBits or any syncObjects dirty
                    write dirty values

            if any data was written to pool
                create UpdateVarsMessage (id + segment from writer)
                foreach Observers
                    send message to transport
```

This sounds simple on the surface but this approach has major flaws 


### O(n^2) Sends to transport

If there are 200 player objects that all update a syncvar then there are 40,000 send calls to the transport, and if the transport doesn't do any batching this will be 40,000 separate network message that need to be sent.

### Dirty check

For every object we take a writer out of the pool before we even know if it is dirty or now. This adds un-needed work that adds up when you have many Network objects.

This can be come a problem when you want passive Network objects in the scene that only open when the player interacts with them. 

For example a object (wall/tree/etc) that has health and can be destroyed, but does nothing without the players actions. Currently the work around for this situation is to avoid making the passive objects Network objects. This feels like it is telling people not to use mirror's features to solve a problem because it is not well optimized.