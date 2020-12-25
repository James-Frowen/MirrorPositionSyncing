# SnapShot Interpolation

Notes and summary about https://gafferongames.com/post/snapshot_interpolation/ and how it can be applied it [Mirror](https://github.com/vis2k/Mirror)


# Summary

- Interpolate between positions
- Include Linear velocity with snapshot if objects have acceleration
- Use UDP 
- 10 hz with 350ms latency (250kb/s)
- 60 hz with 85ms latency (1.5mb/s)
- Extrapolation is hard, fails with collisions


## deterministic lockstep vs Snapshot Interpolation

Deterministic lockstep sends inputs between instances and each simulates the game locally. This is ok for low player count, but requires all inputs before it can fully simulate frame N. 

Snapshot collects relevant state (eg position/rotation) then sends this state to all connections then the clients Interpolate to give an approximation. 

Snapshot takes up a lot more bandwidth, but can scale to more players.


# Snapshot Concepts 

### Object State

Send all objects in order with
```
struct ObjectState
{
    bool interacting;
    vec3f position;
    quat4f orientation;
}
```

Mirror message sent 
```
struct ObjectStateMessage : NetworkMessage
{
    ObjectState[] state;
}
```

Sending `interacting` In Mirror will be difficult because clients might not know about all objects (Interest Management) so `ObjectState` will have to contain the netId instead of interacting, and we can just not send objects that are not interacting.

Only sending changed objects it mentioned in th snapshot compression article. There is a point where sending 1 bit per object to not interacting is worse than sending the id. This depends on the size of the ID and if the interacting is packed as 1 bit or written as a byte.

For this snapshot it is 225 bits/object. For 900 objects this is 25 kilobytes. Syncing this data at 60hz is 1.5 megabytes/second, and this is for each client. If you have 100 clients this would then be 150 megabytes/second. 

This is not viable for most internet connections!

### Linear Interpolation

Client interpolates between snapshots. Can send at 10 hz because the client can smoothly move between them.

This trades latency for smoothness, the latest position from the server will not be set right away.

Linear interpolation will look bad when objects are speed increase and decreases. this is because objects will take the shortest path from old to new position.

sending at higher HZ helps with this, but at the cost of bandwidth

### Hermite Interpolation

Interpolates between positions while considering linear velocity at each sample point. 

Using hermite splines. Passing start/end points with start/end velocity.

Increases bandwidth because velocity is being sent, but data can still be sent at 10hz.


### Quaternions Interpolation

higher order (velocity) is not needed for rotation.

Use spherical linear interpolation.



### Handing Real world Conditions

Send snapshot over UDP.

Snapshots are time critical but not dont need to be reliable.

Hold snapshots in interpolation for appropriate amount of time before interpolation. Buffer should have enough so that 2 snapshots in a row can be missed and still have something to interpolate towards. Add a few frames extra delay to handle jitter.

x3 packet send date, 10hz = 300ms 
350ms to handle jitter

One technique to hide delay from buffer is to use extrapolation. (Common in Fps/racing/etc)

Extrapolation doesn't work well for rigidbodies because of motion can be unpredictable (eg velocity can change rapidly). it doesn't work well because the Extrapolation doesn't know about the physics simulations or collisions.


### Conclusion

350ms seems like a high delay, we can decrease that delay by increasing syncrate. but in order to do that we'd have to decrease bandwidth

60hz = 85ms





