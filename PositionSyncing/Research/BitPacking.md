# Bit Packing

### Variable length int

for numbers that can be in different ranges

1 -> smallest
01 -> middle
11 -> largest

define how many bits for each size ahead of time, for example

smallest 5 bits => [0...32]
middle 10 bits => [0...1024]
largest 14 bits => [0...16384]


This can be applied to position and rotations too, and encode the smallest changes with less bits

For position:
range 0->200
precision 0.1

largest needs to be at most 2000

smallest 4 bits => [0...16] (+- 0.8)
middle 6 bits => [0...64]  (+- 3.2)
largest 11 bits => [0...2048] (+- 1024)

This should be optimized per game/data set

**This is best Applied with delta compression, as change will normally be small**