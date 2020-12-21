# NetworkEvents


Use Scriptable object ands AssetSets to trigger events over the network

Each NetworkEvent would have its own id. when event is invoked handlers on server are trigger, and a message is sent to client to invoke the event there as well.

This could be done with very little weaver work if we use generics for events

```cs
UnityEvent<T> Event;

// on server
public void Invoke(T v1) 
{
    // ...

    writer.Write<T>(v1);

    NetworkServer.Send(new EventMessage 
    {
        // ...
    });
}

// on client
public void HandleEventMessage(EventMessage msg) 
{
    // ...

    var v1 = reader.Read<T>()

    Event.Invoke(Event);
}
```


### Optimizations 

There could be 1 options, send message to all clients. or send message to subscribers.

Sor sending to subscribers, clients would need to send a message to the server to be added to a list and then server would send to just that list.


### Bonus

These could be client auth or server auth. ownership would be given by server. this would allow client to trigger events for just server or for all players (subset of players?)