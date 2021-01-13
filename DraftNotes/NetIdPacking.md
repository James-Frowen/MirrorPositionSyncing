netId is a 32 bit int, 

The goal is to minimize this size for the average case.

1 -> 6 bits, 64
01 -> 11 bits, 2048
001 -> 16, 65k
000 -> 29, 536mil


this results to 
[0...64] 7 bits
[0...2048] 13 bits
[0...65k] 19 bits
[0...536mil] 32 bits
