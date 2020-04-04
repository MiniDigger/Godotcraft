# Godotcraft

Minecraft, but using the godot engine

# Status

04.04.2020: first pass on adding a console/chat view, had to rewrite stuff to c#...  
03.04.2020: reworked the server list, its actually working now  
02.04.2020: worked on a better protocol impl the last few days, got joining working, chat working, no real ui yet  
30.03.2020: Initial setup, a few menus, half assed version of a server list ping  

# Acknowledgments

Thanks to the following resources:

* ORelio for Minecraft-Console-Client, Godotcraft uses a modified version of their implementation of the datatypes in the mc protocol: https://github.com/ORelio/Minecraft-Console-Client/blob/master/MinecraftClient/Protocol/Handlers/DataTypes.cs
* QuentinCaffeino for godot-console, Godotcraft uses a modified (and ported to c#) version of that project https://github.com/QuentinCaffeino/godot-console
