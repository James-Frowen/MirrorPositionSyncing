client numbers +1 for editor,
ping measured using editor
cpu/mem measured using `top`

Linux e2-medium (2vCPU, 4GB memory)

## With Networktransform position only

```
Telepathy
clients, ping, cpu%, memory%, notes
40, 135, 64, 5
80, 144, 166, 5.1
90, 260, 180, 5.1
100, 260, 185, 5.1, looks stable, some small movement spikes
110, 330, 190, 5.2
120, 300, 190, 5.2
160, 540, 190, 5.5
200, 600, 190, 6.0
150, 530, 189, 6.1, ramping down
100, 282, 186, 5.8
100, 282, 186, 5.8, after 5 mins at 100
0, 127, 10, 6.0

Libuv2k-1
clients, ping, cpu%, memory%, notes
0, 135, 10, 5.2
40, 190, 30, 8.0
80, 160, 56, 16.8
120, 160, 86.7, 24.2
160, 200, 105.3, 50.8, some visual spikes
200, 380, 103, 76.5
220, 350, 110, 90.8
240, NA, NA, NA, Server crashed (out of memory)

Libuv2k-2
clients, ping, cpu%, memory%, notes
200, 360, 110, 76.0, restart and added 200 clients
220, 390, 108, 90.7
240, NA, NA, NA, Server crashed (out of memory)

Libuv2k-3
clients, ping, cpu%, memory%, notes
200, 300, 107, 76.0, restart and added 200 clients
150, , , , client shows as 183 connected, reconnecting shows 200
0, , , , client shows as 50 connected, reconnecting shows 148

Apathy
clients, ping, cpu%, memory%, notes
0, 124, 11, 5.2
40, 147, 33.6, 5.9
80, 130, 67.4, 6.5
120, 140, 108, 7.1
160, 260, 110, 7.7, visual lag spikes
180, 350, 112, 7.9
200, 380, 107, 8.3 
220, 450, 103, 8.7
240, 500, 103, 9.0
280, 600, 110, 9.7
160, 300, 107, 10.3, ramping down
100, 220, 998, 10.5, still visual lag
50, 130, 42.2, 7.5, looks smooth


Asio
clients, ping, cpu%, memory%, notes
0, 138, 11, 5.2
40, 135, 36.9, 5.6
80, 133, 66.0, 5.9
120, 131, 100.0, 6.3
160, 140, 128, 6.6
180, 145, 161.7, 6.9
200, 161, 164.5, 7.1, visual lag spikes
220, 300, 185, 7.5,
240, 420, 196, 7.8
280, 2000+, 197, 10.3
140, 180, 140, 10.2, ramping down, still a lot of visual lag spikes
70, 150, 55, 9.8, smooth movement again
```

## With Position system
```
Telepathy
clients, ping, cpu%, memory%, notes
0, 135, 11.3, 5.1
40, 130, 35.0, 5.6
80, 128, 52, 6.0
120, 130, 86.7, 6.4
160, 135, 93.7, 6.9
200, 150, 120.7, 7.5, large ping spike but settles back to 150
240, 140, 137, 8.1, now ping spike looks smooth
280, 190, 142, 8.5, visual lag spikes movement jumpy
320, 170, 134, 9.1
360, 207, 140, 9.7
400, 300, 136, 10.3
```

### With Position system (isKinematic for non-local player)
isKinematic to stop players being moved by gravity

500 max connections in NetMan

```
Telepathy 
clients, ping, cpu%, memory%, notes
0, 135, 11, 5.1
40, 127, 25, 5.6
80, 130, 40, 6.1
120, 136, 61, 6.5
160, 140, 75, 6.9
200, 135, 86, 7.4
240, 140, 99, 7.8
280, 144, 122, 8.2
320, 210, 130, 8.8, some visual lag
360, 226, 130, 9.4
400, 168, 154, 9.8
440, 190, 144, 10.8
480, 214, 150, 11.5, still stable
500, 230, 150, 11.5


Libuv2k
clients, ping, cpu%, memory%, notes
0, 144, 13, 5.1
40, 250, 23, 5.8, visual lag
80, 216, 30, 6.5
120, 224, 35, 7.1
160, 229, 48, 7.9
200, 266, 63, 8.6
240, 180, 69, 10.0, movement seems smooth?? why did it get better
280, 194, 101, 11.0
320, 192, 91.3, 11.7, visual lag again
360, 190, 113, 12.4
400, 200, 112, 16.7
440, 210, 112, 17.9
480, 216, 118, 19.5
500, 237, 118, 19.9

Apathy
clients, ping, cpu%, memory%, notes
0, 130, 13, 5.1
40, 135, 21, 5.7
80, 125, 35.6, 6.1
120, 130, 43, 6.8
160, 128, 50, 7.5
200, 138, 63, 8.2
240, 136, 69, 8.8
280, 150, 88, 9.4
320, 144, 99, 10
360, 143, 110, 10.6, still smooth movement
400, 145, 117, 11.3
440, 180, 116, 12.3, some visual lag
480, 208, 122, 13.1
500, 225, 126, 13.6

Asio (4 threads)
clients, ping, cpu%, memory%, notes
0, 137, 12, 5.1
40, 137, 35, 5.4
80, 150, 65, 5.7
120, 155, 74, 6.0
160, 177, 91, 6.4
200, 171, 107, 6.8
240, 148, 121, 7.1
280, 150, 134, 7.5
320, 165, 162, 7.8 
360, 172, 163, 8.1, small visual lag
400, 185, 163, 8.7, some visual lag
440, 210, 165, 9.2
480, 201, 188, 9.8
500, 214, 170, 9.9, some visual lag but mostly smooth/stable 
```

### Bigger server
Linux e2-highcpu-4 (4vCPU, 4GB memory)

2000 max connections in NetMan
994 seems like max (file limit problem on linux)

```
Telepathy
clients, ping, cpu%, memory%, notes
0, 130, 18.6, 5.0
100, 140, 93, 6.2
200, 142, 135, 7.4
300, 150, 188.4, 8.6
400, 150, 199, 10
500, 160, 217, 11.7
600, 173, 220, 13.2
700, 203, 232, 14.8
800, 259, 232, 16.8
900, 270, 238, 18.4
994, 280, 237, 21.4

Libuv2k
clients, ping, cpu%, memory%, notes
0, 200, 19, 5.1
100, 214, 65.3, 6.7, visual lag
200, 200, 91.7, 8.5, spikes
300, 162, 143, 10.6
400, 202, 155, 13.4 
500, 164, 150, 19.6, objects jumping
stopping test early, visual performance isnt good  

Apathy
clients, ping, cpu%, memory%, notes
0, 120, 19.3, 5.1
100, 132, 74.8, 6.6
225, 133, 115.9, 8.6
300, 137, 152.7, 9.7
400, 145, 153, 11.4
500, 160, 161, 13.5, small visual lag
600, 161, 167, 14.9
700, 170, 168, 16.5
800, 202, 173, 18.0, visual lag
900, 213, 177, 19.9
994, 217, 178, 23.1

Asio (4 threads)
clients, ping, cpu%, memory%, notes
0, 135, 20, 5
100, 140, 167.3, 5.8
200, 135, 276, 6.7
300, 143, 346, 7.6
400, 162, 349, 8.5
500, 169, 366, 9.8
600, 213, 370, 10.7, small visual lag
700, 206, 369, 12.0
800, , , , all disconnected (max message receive queue) while connecting new clients
```


### Windows server

windows t3.xlarge (4vCPUm 16gb memory)
* on aws so vcpu may not compare to number above

```
Telepathy
clients, ping, cpu%, memory%, notes
0, 170, , 2.8, 124, 
100, 175, , 14.5, 162, 
200, 170, 60, 27, 210, 
300, 177, 55, 37, 260, 
400, 193, 60, 50, 310, 
500, 203, 70, 50, 390, Spikes to 200ms, stabilized down


Libuv2k
clients, ping, cpu%, memory%, notes
Didnt run dll not found

Apathy
clients, ping-uk, ping-USwest, cpu%, memory%, notes
0, 174, 50, 3, 129
100, 165, 55, 12, 187
200, 167, 50, 20, 248
300, 177, 55, 33, 310
400, 180, 55, 33, 375
500, 193, 65, 35, 450 

Asio
clients, ping, cpu%, memory%, notes
Didnt run dll not found
```