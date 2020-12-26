Writing 1000 rotations to buffer
Time to complete 10,000 iterations (eg 10mil writes)
Tested in Server Build

```md
Method                                       | Time(ms) | Bytes
---------------------------------------------|----------|-------  
Blittable                                    |  114     |  16000
OLD                                          |  548     |  16000
Compressed                                   | 3536     |   4000
BitPacker WithNetworkWriter Length9          | 3743     |   3625
BitPacker WithNetworkWriter Length10         | 3673     |   4000
BitPacker WithBuffer Length9                 | 3690     |   3625
BitPacker WithBuffer Length10                | 3627     |   4000
BitPacker WithBuffer Length9 optimized       | 1954     |   3625
BitPacker WithBuffer Length9 inline          | 1459     |   3625
```