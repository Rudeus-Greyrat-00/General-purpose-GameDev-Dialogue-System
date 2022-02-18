# DOCUMENTATION WORK IN PROGRESS

# General Purpose Game Development Dialogue System (AKA GPGD Dialogue System)
General purpose C# libraries (and editor) to create dialogues to be used in games in any C# based game engine

[Download last editor relase](https://github.com/VulcanRedEngineer1701/General-purpose-GameDev-Dialogue-System/releases/tag/v0.0.1)

Index:
1. What is GPGD Dialogue System
2. How to include the libraries in your games
3. How to use in your games
   
   3.1 - Dialogue structure and components meaning
  
   3.2 - How to use in your games
   
   3.3 - Editor overview
   
   3.4 - Best practices

## 1: What is GPGD Dialogue System
GPGD Dialogue System is a project composed of two sub-project, a library (wich I will refer to as "library") wich can create and manage NPC dialogues in games at runtime, and an editor wich can be used to create dialogue files wich can be converted by the library into a Dialogue class object at runtime in your game with one simple function.
The dialogue class provide some useful function to navigate easily into the dialogue as you will see into sections 3.1 and 3.2. 

The editor is a completely stand-alone project, it actually uses the library to export the json file and for some feature (for example it contains a section where you can simulate the behavior of the dialogue that you are making) but you don't need to link the editor project to your game in any ways at all.

 ## 2: How to include the libaries on your game
