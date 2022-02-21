# DOCUMENTATION WORK IN PROGRESS

# General Purpose Game Development Dialogue System (AKA GPGD Dialogue System)
General purpose C# libraries (and editor) to create NPC dialogues to be used in games in any C# based game engine

[Download last editor relase](https://github.com/VulcanRedEngineer1701/General-purpose-GameDev-Dialogue-System/releases/tag/v1.0)

Index:
1. What is GPGD Dialogue System
2. How to include the libraries in your games
3. How to use it in your games
   
   3.1 - Dialogue structure and components meaning
  
   3.2 - How to use in your games
   
   3.3 - Editor overview
   
   3.4 - Best practices

## 1: What is GPGD Dialogue System
GPGD Dialogue System is a project composed of two sub-project, a library (wich I will refer to as "library" or "the library" or "the libraries") wich can create and manage NPC dialogues in games at runtime, and an editor wich can be used to create dialogue json files wich can be converted by the library into a Dialogue class object at runtime in your game with one simple function.
The dialogue class provide some useful function to navigate easily into the dialogue as you will see into sections 3.1 and 3.2. 

The editor is a completely stand-alone project, it actually uses the library to export the dialogue json file and for some feature (for example it contains a section where you can simulate the behavior of the dialogue that you are writing) but you don't need to link the editor project to your game in any ways at all.

 ## 2: How to include the libaries on your game
 (please note that I tried to describe that in a way that even begineer in game development or in programming (like me) can understand)
 #### First: Download all the code as a zip file, then extract it. Inside you should see 2 folders, 'EditorSource' and 'Libraries'.
 
 _____________________________________________________________________________________________________________________________________
 
 #### Unity: 
 
Open the library folder > GPGameDevDialogueSystemLib >  GPGameDevDialogueSystemLib > bin > relase > GPGameDevDialogueSystemLib.dll. Simply drag that .dll file into the Unity editor, into the "Assets" area. You did it!
 _____________________________________________________________________________________________________________________________________
 
 #### Godot (it also applies to pretty much any C# project, even the ones without engines, it doesn't work for Unity): 
 
 0 - Copy the 'Libraries' folder and put it in a 'stable' place, when you won't move or touch unintentionally (for example: desktop = bad place, somewhere inside your game project folder = good place)
 
 -1 Open the .sln file of your project with visual studio (I used Visual Studio 2022), if you are using godot, it should be inside the project folder.
 
 -2 Go to the 'Solution Explorer' on the right of the screen (if you use the default Visual Studio layout), left click on 'Solution [name of your solution]' > Add > Existing Project then navigate to the .csproj file inside the 'Libraries' folder (it is inside Libraries > GPGameDevDialogueSystemLib > GPGameDevDialogueSystemLib and it should have a green 'C#' icon) and add it. You are almost done.
 
 -3 Under Solition [name of your solution] you should now see two projects. Under [your project] left click on 'Dependencies' > Add Project Reference, tick 'GPGameDevDialogueSystemLib'. You are done!
_____________________________________________________________________________________________________________________________________

Of course in both cases to create/use object of class Dialogue you need a using directive, so in any .cs file when you want to use the dialogue system add "using DialogueSystem"
## 3: How to use it in your games
      
