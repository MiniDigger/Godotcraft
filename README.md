# Godotcraft

Minecraft, but using the godot engine

# Assets

If you want to run it yourself, you need to download the minecraft assets and put them into the mcassets folder into the user folder.  
Its located at `C:\Users\Martin\AppData\Roaming\Godot\app_userdata\Godotcraft` for me.  
InventiveTalent has a repo with all the stuff: https://api.github.com/repos/InventivetalentDev/minecraft-assets/zipball/1.15.2

# Status

09.04.2020: More rendering, better culling, texture atlas!
08.04.2020: Random experiments with voxel rendering, including trying to do culling  
04.04.2020: first pass on adding a console/chat view, had to rewrite stuff to c#...  
03.04.2020: reworked the server list, its actually working now  
02.04.2020: worked on a better protocol impl the last few days, got joining working, chat working, no real ui yet  
30.03.2020: Initial setup, a few menus, half assed version of a server list ping  

# Acknowledgments

Thanks to the following resources:

* ORelio for Minecraft-Console-Client, Godotcraft uses a modified version of their implementation of the datatypes in the mc protocol: https://github.com/ORelio/Minecraft-Console-Client/blob/master/MinecraftClient/Protocol/Handlers/DataTypes.cs
* QuentinCaffeino for godot-console, Godotcraft uses a modified (and ported to c#) version of that project https://github.com/QuentinCaffeino/godot-console
