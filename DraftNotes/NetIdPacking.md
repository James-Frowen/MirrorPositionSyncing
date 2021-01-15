# Network Id syncing

netId is a 32 bit int

## Goal

The goal is to minimize this size for the average case.

Below are some Solution to reduce bandwidth of int, some solutions can work together to get greater saving

## Solution 1: Variable size bit packing

Pack id into different number of bits based on its value.

This can be done by writing what size it is, then the value

### example
encoding:
```
1 -> 6 bits, 64
01 -> 11 bits, 2048
001 -> 16, 65k
000 -> 29, 536mil
```

```
if id < 64
    write 1 as 1 bit
    write id as 6 bits
else if id < 2048
    write 01 as 2 bit
    write id as 11 bits
else if id < 65k
    write 001 as 3 bit
    write id as 16 bits
else if id <  536mil
    write 000 as 3 bit
    write id as 29 bits
```

ranges can be stacks, so 2nd smallest can be 64->2112.

encoding can be changed based on use case/data to minimize bandwidth

bit count for id values:
```
[0...64] 7 bits
[64...2112] 13 bits
[2k...67k] 19 bits
[67k...536mil] 32 bits
```

## Solution 2: Send id compared to previous id

If data for multiple objects are send in same message, then the Ids can be sent compared to the previous one.

For example sending data for 
```
ids = [10, 13, 20, 21]
```

can be send as 
```
ids = [10, 3, 7, 1]
```

if different could be large then you can send a bit to say if the id is absolute or relative to previous.

This can be combine with [Solution 1](#solution-1-variable-size-bit-packing)


## Solution 3: Send Id relative to known offset on client

Server would keep track of id offset for client

Server could then send ids relative to that offset, keeping values low

This offset could be updated when client replies with which snapshot they received last
