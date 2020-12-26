# ScriptableObjects

Mirror has almost no support for scriptable objects, all it can do is create a new instance of an object when it is sent over network, this is extremely limited and ideally should never happen because of the additional allocations (struct should be sent instead).

### Networking features for ScriptableObjects 

It would be really useful to be able to use syncvar and CMD/RPC from within a scriptable objects. 

This could achieved by assigning a fixed id like scene Id.

### SyncLists as ScriptableObjects

With greater support Synclists and dictionaries could be made into scriptableobjects instead of being forced to be inside of a MonoBehaviour.


https://www.youtube.com/watch?v=WLDgtRNK2VE