# DOCUMENTATION WORK IN PROGRESS

# General Purpose Game Development Dialogue System (AKA GPGD Dialogue System)
General purpose C# libraries (and editor) to create NPC dialogues to be used in games in any C# based game engine

[Download last editor relase](https://github.com/VulcanRedEngineer1701/General-purpose-GameDev-Dialogue-System/releases/tag/v1.0)

Index:
1. What is GPGD Dialogue System
2. How to include the libraries in your games
3. How to use it in your games
   
   3.1 - Dialogue structure and components meaning
  
   3.2 - How to use it in your games
   
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

Before starting to explain how a Dialogue class object is composed and how it should be easily used, i have to explain the meanings of all of the classes you will work with when using GPGD Dialogue System.

I think looking at the editor it is a great way to understand how an object of Dialogue class work and how it should be used, so here there is a screenshot from the editor.

![Image 1](https://user-images.githubusercontent.com/96582680/154964295-db75e4ab-428b-4300-a70a-9d497f548fa7.png)

So there is a lot to cover. What you are looking at is a simple dialogue where an NPC says "Today is a beautiful day" then asks to the player "Isn't it?" The player can choose between "Yes, it is a beauiful day" or "No it is a bad day" and then the Dialogue ends with the NPC saying "I'm glad that you agree with me" or "Nooo why do you whink it is a bad day?" depending of wich answer the player choosed.

Let's start from the ground up.
This:

![image](https://user-images.githubusercontent.com/96582680/154965037-ef12d4aa-3381-46fb-a4c1-818acc52eb05.png)

Is called a "Talk". It represent basically what happen in a single text bubble inside your game. A Dialogue class object contains a "FirstTalk" (in the example, it would be the one called "First Statement"). Every talk could contain a link to another talk. A Talk has a "Name" or a "Tag", it is a string that can contains the name of the talk or basically whatever you want, it is just an utility string. If you don't fill it, it will be empty space. Different talks can have the same Tag.

Next you have a space where you can specify a "Character Name", even this is optional (for example if you want to create a dialogue like reading a sign). Basically everything is optional. If you want, you can even export a dialogue with all empty talks.

Next you have the main talk text. Here you can write what you want to appear in the text bubble in your game. It could be what is written on a sign, or what an NPC says.

Clicking on "Add Answer" an answer will appear. You can add unlimited amount of answers. As you can see each answer contains a link to another talk. They are all optional.

_________________________________________________________________________________________________________________________

There are a couple of things that I have not explained yet. These are "Action", "Conditions", "Runtime Variables" and "Modifiers". I'm going to briefely explain what they represent, i will explain how to use all this stuff in the next section.

"Action" are object of class GenericEngineHandledAction (Inherit from DialogueAction class). They have two fields, "name" and "value". They are information containers, i will explain better how to use they in the next section.

Now to explain "Condition" let's say that I want to create a dialogue where if a condition in game is verified a character says something, else it says something different. It could be done this way:

![image](https://user-images.githubusercontent.com/96582680/154970026-bff481f6-831f-48d6-b941-3e3fb306fc0c.png)

Let's focus on an empty condition:

![image](https://user-images.githubusercontent.com/96582680/154970444-80f8b4e1-1af7-465b-806d-9f00e47d8243.png)

As you can see, a "Condition" can contains a Name, an Argument, actions that are performed if the outcome is true, and actions that are performed if the outcome is false. And of course, it contains links to a path to follow if the outcome is true, or if it false. In a Dialogue class object, the property "dialogueConditions" returns all the conditions fetching them from all the talks of the dialogue.

_________________________________________________________________________________________________________________________

And finally, let's say that you want to edit some part of a talk text a runtime in your game. For example, let's say that in your game the player has an inventory and can equip hats. Let's assume that every hat has a different name. You want that an NPC says something like "Oh, i see that you have a very nice" and then the name of the hat, for example "Oh, i see that you have a very nice mexican hat". 

All the fields and property of Talk class are public so you can edit the text how you want, but actually there is a simpler way to do it, for example this can be done using "Runtime Variables" and "Modifiers". Object of class "Runtime Variables" contains a "name" and a "value". 

In the editor you can chose only the name, because the value is supposed to be filled at runtime by your game. How it depends by your implementation, i will suggest some ways in the next section. 

In addition to a Runtime Variables, you need a "Load Runtime Variable Modifier" in the text. In the text, you can add modifier like this {modName,modArgument}. The name of the load runtime variable modifier is "loadRV". So you dialogue in the editor will appear like this

![image](https://user-images.githubusercontent.com/96582680/154974301-961e361f-8b3a-49e1-be3c-e74494f1a337.png)

When you instance a new dialogue class object readed from a dialogue file you need to fill all the runtime variables values and all the condition outcomes. 

Then the value that you assigned to the "CurrentHat" RuntimeVariable object will be automatically placed inside the Talk text instead of {loadRV,...}. I will explain this better in the next section. If you are afraid to made mistakes check the small rectangle in the upper-right corner of the Talk text textbox, if it is green, all your modifier will work. If it is red like in the following example, hover with the mouse over the rectangle to get information about what is wrong. In this example, i will write the name of the runtime variable wrong:

![image](https://user-images.githubusercontent.com/96582680/154975501-0ac0fd1f-5fba-4acb-9d79-39581e450f2e.png)

Now you should have a basic grasp about what is a Dialogue, a Talk, a Condition, a RuntimeVariable, an Action and a Modifier

## 3.2: How to use GPGD Dialogue System in your games

Ok, let's say that you just created a dialogue in the editor (an editor overview / tutorial is in the next section) and you want to use it in your game. 
First of all you need to export it. Press Ctrl + E or go un File > Export. Put the file somewhere inside your game project folder. 

Now, to get a Dialogue class object from that file you need first of all to get the content of that file and put in in a string. You can do this in many ways, it depents on your engine (if it has some features for interacting with game files) and you have to know how to do that, but it is a pretty easy task. Then you can convert that string into a dialogue object calling DialogueManager.JSONParse(yourstring) if you are working in a NON NetFramework4.x environment, otherwise you call DialogueManager.JSONParseNetFr4x(yourstring). If you don't know wich one use, in Unity and Godot you have to use DialogueManager.JSONParseNetFr4x. In general, if JSONParse/JOSNParseNetFr4x return an error, try switching the function. An example could be the following

![image](https://user-images.githubusercontent.com/96582680/155179835-e2fead5d-c289-4a81-a04b-d755a3627610.png)

Now you got your dialogue!

______________________________________________________________________________________________________________________

Now, every time you get a dialogue the first thing to do is to fill the outcomes of all the conditions, and fill all the values of all the runtime variables. How to do it it really depends on you and on your implementation, a possible way could be the following:

![image](https://user-images.githubusercontent.com/96582680/155018140-d5a2e906-4cc2-4762-ba81-aa00cdca15ea.png)

And in that switch you can define every condition you want inside your game and then add it in your dialogue, and eventually when you need to define a new kind of condition, you just add in the switch. You can do a pretty similar thing with the RuntimeVariables. It doesn't matter how you do it, its all up to you, but both Condition outcomes AND RuntimeVariables values should be filled before starting the dialogue itself.

You "start" the dialogue and continue into it witch only one function, NextStep() wich has an overload to use if the dialogue has answers, NextStep(answerIndex).
For example, the simplest implementation possibile is the following:

![image](https://user-images.githubusercontent.com/96582680/155181499-4e13199b-dfc4-4b92-afdc-0fb59b0e7f0b.png)

To test the dialogue library initially i created a console applications with this code:

![image](https://user-images.githubusercontent.com/96582680/155181774-27c1c49c-8d81-4383-aa04-c1a1fff72bfb.png)







