   M S B A M L   `   `   `  ���� R  NGPGameDevDialogueEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullm;clr-namespace:GPGameDevDialogueEditor.DialogueObjectEditors-GPGameDevDialogueEditor.DialogueObjectEditors  a5clr-namespace:GPGameDevDialogueEditor.UICustomObjects'GPGameDevDialogueEditor.UICustomObjects  >    8GPGameDevDialogueEditor.DialogueObjectEditors.TalkEditor   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Mlocal;clr-namespace:GPGameDevDialogueEditor.DialogueObjectEditors     Duc5clr-namespace:GPGameDevDialogueEditor.UICustomObjects     .���� $��0,0,0,0q�.+���   J� IsHitTestVisible$
  True��.+��� -    ��Name$ RectangleContainer��$	��341��$	��320��
���   ����	#FF212121�� -   $��20��$��20��
���   �$��2��
������� J� 	AllowDrop$
 True����  �� Orientation$ 
Horizontal=�$��20,0,0,0q�.+��� -   $��20��$	��301�����	#FF00BBBB�� -   
���  ��$��20��$��20��$��Center=�$��Center=�.��X�� -   $ parseModifierOutcome��$��18��$��8��
��� � �
 �� Top$ 99�� �� Left$	 303��$
  True�� � ToolTip$ No errors found���� $��20,25q�.+��� $ 
Horizontal=�.+��� $��
Talk name:���� -    ��Name$ TalkTag��$	��216��$��20,0,0,0q� �� TextWrapping$ NoWrap=�$��For example 'Statement'��
�������$��Center=�$��24���� $ 
Horizontal=�$��0,4,0,0q�.+��� $��Character name:���� -   $ CharacterName��$	��205��$��0,0,0,0q�$ NoWrap=�$#��Name of the talking character��
�������$��Center=�$��24���� $��
Talk text:��$  False���� -   $ MainTextBox��$	��300��$ NoWrap=�$
��Left=�$	��100��$��12(�$��1,0,1,1q�
�������	 �� AcceptsReturn$
	 True��.��What the character says...�� $ 
Horizontal=�.+��� $��	130,0,0,0q�.��
Next talk:9   3GPGameDevDialogueEditor.UICustomObjects.LineStarter  -	   
  Name$
 starter��$��	10,6,0,15q�$��Right=��� -
    ��Name$ IsConditionalCB�����	#FFFF7474.��Is Conditional�� -    ��Name$ ConditionContainer���� $ 
Horizontal=�.+��� ���	#3300FFFF$	��150��$��0,1,0,2q�$��Actions:���� -   $��1,1,1,2q�$��
Add action��$��69���� -   $ ActionStackPanel��$��20,0,0,0q��� $ 
Horizontal=�.+��� ���	#3300FF04$	��150��$��0,2,0,2q�$��Answers:���� -   $
��Left=�$��1,1,1,2q�$��Add answer:���� -   $ AnswerStackPanel��$��20,0,0,0q�