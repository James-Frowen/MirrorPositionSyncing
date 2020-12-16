# Syncing Techniques 

Notes and summary about https://gafferongames.com/post/snapshot_compression/ and how it can be applied it [Mirror](https://github.com/vis2k/Mirror)


## Changed bit vs Index

#### Changed bit 

If many objects are changing:
- send all objects 
- first bit says if objects has changed or not
  - if not dont send data, meaning it only takes up 1 bit
- requires both sides to know what objects are being sent because no indexes are given

#### Indexing

If not many objects are moving:
- send object index with data


### Mirror

Because or mirror's visibility system not all clients will know about all objects. this makes Changed bits harder to deal with because we dont want to send data about objects a client can not see. One solution to this would be to always send unchanged for objects that a client can not see. We may also need to the netid of the first object incase the server has been running for a while and is not starting at id=1.




