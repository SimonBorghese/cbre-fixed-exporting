CBRE for anything but Containment Breach
======
Why would you even use this over Sledge?

This takes the fork of Sledge known as CBRE, Containment Breach Room Editor, and modifies it so that it can be used to export to anything but SCP - Containment Breach.
It's designed to be able to actually be able to export to Unity, but you could use it for anything (besides SCP: CB as stuff will likely break).

You could use this as a general purpose level creator.

The Editor is licensed under the GNU General Public License, version 2.0.
All other components are licensed under the GNU Lesser General Public License, version 2.1, unless otherwise stated.

You can find the Sledge source code at https://github.com/LogicAndTrick/sledge
You can find the CBRE source code at https://github.com/SCP-CBN/cbre

Todo list:
- [x] Fix exporting of other formats (OBJ, FBX) so that solids are exported as seperate meshes. (Previously, Unity would import these files as one giant mesh)
- [ ] Allow for the embedding of textures loaded by the map
- [ ] Allow for the exporting of more formats
- [ ] Embed and/or export data related to entities either within the 3d models or seperately.


Not planned but may be done within the future:
- [ ] Linux support via Wine
- [ ] Linux support via Mono
