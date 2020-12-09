Comparing how many bytes are sent and write count for different methods


## Summary
```
1960 KB     NetworkTransform (v26)
1720 KB     NetworkTransform (master)
840  KB     NetworkTransform (Position only)
540  KB     Positions System (uncompressed)
260  KB     Positions System (compressed)
380  KB     Positions+rotation System (compressed)
```


## 200 NetworkTransform (v26)
```
Single message 49 bytes
Header                            Position
sends 5A-42-18-12 -01-0C-00-00-00 -9A-C1-05-43-A0-91-9A-41-7F-B0-23-43 ... rotation+scale 16+12 bytes

Header breakdown
---
MessageHash             5A-42
NetId(packed)           18
PayloadLength(packed)   12
CompIndex               01
LengthWritten           0C-00-00-00

per update 
40000 writes
1960000 bytes
```

## 200 NetworkTransform (master)
```
Single message 43 bytes
Header                            Position
sends 5A-42-...                   -9A-C1-05-43-A0-91-9A-41-7F-B0-23-43 ... rotation+scale 4+12 bytes

Header breakdown
---
MessageHash             5A-42
NetId(packed)           4 bytes
PayloadLength(packed)   4 bytes
CompIndex               01
LengthWritten           0C-00-00-00

per update 
40000 writes
1720000 bytes
```

## 200 NetworkTransform (Position only)
```
Single message 21 bytes
Header                            Position
sends 5A-42-18-12 -01-0C-00-00-00 -9A-C1-05-43-A0-91-9A-41-7F-B0-23-43

Header breakdown
---
MessageHash             5A-42
NetId(packed)           18
PayloadLength(packed)   12
CompIndex               01
LengthWritten           0C-00-00-00

per update 
40000 writes
840000 bytes
```

## 200 positions System (uncompressed)
```
MessageHash     2 bytes
Count           2 bytes

foreach position
    NetId(packed)  1 or 2 bytes
    Position       12 bytes

total message size 
between 2604->2804
around to 2700 for simplicity

per update 
200 writes
540000 bytes
```

## 200 positions System (compressed)
Compression
```
Ranges
x 0-> 500
x 0-> 60
z 0-> 500

precision = 0.05f
```

```
MessageHash     2 bytes
Count           2 bytes

foreach position
    NetId(packed)  1 or 2 bytes
    Position       39 bits = 5 bytes

total message size 
between 1204->1404
around to 1300 for simplicity

per update 
200 writes
260000 bytes
```

## 200 positions+rotation System (compressed)
```
MessageHash     2 bytes
Count           2 bytes

foreach position
    NetId(packed)  1 or 2 bytes
    Position       39 bits = 5 bytes
    rotation       23 bits = 3 bytes

total message size 
between 1804->2004
around to 1900 for simplicity

per update 
200 writes
380000 bytes
```
