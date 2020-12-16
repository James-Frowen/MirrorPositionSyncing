# Syncing Techniques 

Notes and summary about https://gafferongames.com/post/snapshot_compression/ and how it can be applied it [Mirror](https://github.com/vis2k/Mirror)


## Changed bit vs Index

When sending a stream of data, we need to be able to tell which object each block of data belongs to

#### Changed bit 

If many objects are changing:
- send all objects 
- first bit says if objects has changed or not
  - if not dont send data, meaning it only takes up 1 bit
- requires both sides to know what objects are being sent because no indexes are given

#### Indexing

If not many objects are moving:
- send object index with data


### Applied to Mirror

#### Changed bit 

Because or mirror's visibility system not all clients will know about all objects. This makes Changed bits harder to deal with because we dont want to send data about objects a client can not see. One solution to this would be to always send unchanged for objects that a client can not see. We may also need to the netid of the first object incase the server has been running for a while and is not starting at id=1.

#### Indexing

Indexing seems like it would be better fit more use cases for mirror.

It might be worth sending an netid offset at the start of the message so that id can be better packed. This would be good for servers that run for a long time as the netid is not reset.



