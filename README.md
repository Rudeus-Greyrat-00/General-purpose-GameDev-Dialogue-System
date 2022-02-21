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

### 3.1: Dialogue structure and component meaning

Before starting to explain how a Dialogue class object is composed and how it should be easily used, i have to explain the meanings of all of the classes that you will work with when using GPGD Dialogue System.

I think looking at the editor it is a great way to understand how an object of Dialogue class work and how it should be used, so here there is a screenshot from the editor.

![Image 1](https://user-images.githubusercontent.com/96582680/154964295-db75e4ab-428b-4300-a70a-9d497f548fa7.png)

So there is a lot to cover. What you are looking at is a simple dialogue where an NPC says "Today is a beautiful day" then ask to the player "Isn't it?" The player can choose between "Yes, it is a beauiful day" or "No it is a bad day" and then the Dialogue ends with the NPC sayng "I'm glad that you agree with me" or "Nooo why do you whink it is a bad day?" depending of wich answer the player choosed.

Let's start from the ground up.
This:

![image](https://user-images.githubusercontent.com/96582680/154965037-ef12d4aa-3381-46fb-a4c1-818acc52eb05.png)

Is called a "Talk". It represent basically what happen in a single text bubble inside your game. A Dialogue class object contains a "FirstTalk" (in the example, it would be the one called "First Statement"). Every talk could contain a link to another talk. A Talk has a "Name" or a "Tag", it is a string that can contains the name of the talk or basically whatever you want, it is just an utility string. If you don't fill it, it will be empty space. Different talks can have the same Tag.

Next you have a space where you can specify a "Character Name", even this is optional (for example if you want to create a dialogue like reading a sign). Basically everything is optional. If you want, you can even export a dialogue with all empty talks.

Next you have the main talk text. Here you can write what you want to appear in the text bubble in your game. It could be what is written on a sign, or what an NPC says.

Clicking on "Add Answer" an answer will appear. You can add unlimited amount of answers. As you can see each answer contains a link to another talk. They are all optional.

There are a couple of things that I have not explained yet. These are "Action", "Conditions", "Runtime Variables" and "Modifiers". I'm going to briefely explain what they represent, i will explain how to use all this stuff in the next section.

