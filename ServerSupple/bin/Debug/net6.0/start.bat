@echo off
chcp 65001
cls
:srat
echo Server starting...

RustDedicated.exe -batchmode ^
+server.hostname "MERCURY WINDOWS" ^
+server.identity "winserver" ^
+server.port 28015 ^
+server.maxplayers 5 ^
+server.seed 1111 ^
+server.worldsize 2500 ^
+server.seed 329193 ^
-logFile "output.txt" ^
+rcon.port 28015
+rcon.password "123" ^
goto start