# Netduino_IP_Stacks

This project contains the source code for the managed IP stacks for the Netduino family of microcontroller boards.

The entire Netduino.IP stack, from link layer interface to the networking protocols, is written in C#. It works with the standard System.Net classes (Socket, HttpWebRequest, etc.) for backwards compatibility, so existing NETMF projects should work out of the box.

Netduino.IP has been designed to use as little as 4KB of RAM (vs 10s of KBs for lwIP). Plus the new stack only consumes RAM when networking is used; garbage collection is minimized by design.

The new Netduino.IP stack is designed for modularity and for use across many communication mediums and many different networking chips.

## Netduino.IP

Netduino.IP contains the core Netduino.IP stack offering features such as TCP and UDP sockets, DNS etc.

The Netduino.IP project contains the code for the Netduino Plus 2 and the Netduino 3 Ethernet boards.

## Netduino.IP_CC3100

Netduino.IP_CC3100 contains the Netduino.IP stack for the CC3100 chip as found on the Netduino 3 WiFi board.

## Debugging Netduino.IP

One of the advantages of the Netduino.IP is the fact that it is written in C# and can be [debugged from within Visual Studio](Documentation/DebugProjects.md).

## Building a Release

Once debugged, any changes to the Netduino.IP code can be [built into the firmware](BuildingForFirmware) and deployed to the board.  More information about building the firmware can be found in the [Netduino_SDK github repository](https://github.com/WildernessLabs/Netduino_SDK).

## License

Copyright 2017-2018, Wilderness Labs Inc.
    
    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at
    
      http://www.apache.org/licenses/LICENSE-2.0
    
    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.