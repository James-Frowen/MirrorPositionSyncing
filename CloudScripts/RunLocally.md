
Run inline
```
.\mirror_server.exe -client -address localhost
```

Run one
```
invoke-expression 'cmd /c start powershell -Command { .\mirror_server.exe -client -address localhost }'
```


Run many

```
1..2 | % { invoke-expression 'cmd /c start powershell -Command { .\mirror_server.exe -client -address localhost }' }
```


Client and server

```
invoke-expression 'cmd /c start powershell -Command { .\mirror_server.exe -server }'
1..5 | % { invoke-expression 'cmd /c start powershell -Command { .\mirror_server.exe -client -address localhost }' }
```