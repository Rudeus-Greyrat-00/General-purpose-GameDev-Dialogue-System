   M S B A M L   `   `   `  ����R  NGPGameDevDialogueEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullm;clr-namespace:GPGameDevDialogueEditor.DialogueObjectEditors-GPGameDevDialogueEditor.DialogueObjectEditors  5      a5clr-namespace:GPGameDevDialogueEditor.UICustomObjects'GPGameDevDialogueEditor.UICustomObjects  5      >    8GPGameDevDialogueEditor.DialogueObjectEditors.TalkEditor   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Mlocal;clr-namespace:GPGameDevDialogueEditor.DialogueObjectEditors     5      Duc5clr-namespace:GPGameDevDialogueEditor.UICustomObjects     5      .��5
      �� $��0,0,0,0q�6   .+�5   
   ��   J� IsHitTestVisible$
  True��6   .+�5      �� -   5         ��Name$ RectangleContainer��5      $	��341��64   $	��320��6@   
���   ����	#FF2121216o   �� 5      -   5        $��20��5      $��20��6$   
���   �$��2��6>   
������� J� 	AllowDrop$
 True��6d   6u   �� 5       �� Orientation$ 
Horizontal=�6   $��20,0,0,0q�62   .+�5      �� -   5        $��20��5      $	��301��6(   ���	#FF00BBBB6g   �� 5      -   5        
���  ��$��20��5   *   $��20��65   $��Center=�6A   $��Center=�6e   .��6�   X6�   5      �� 5      -   5        $ parseModifierOutcome��5      $��18��66   $��8��6A   
��� � �
 �� Top$ 99��6Y    �� Left$	 303��6i   $
  True��6{    � ToolTip$ No errors found��6�   5      5      �� 5   
   $��20,25q�6   .+�5      �� $ 
Horizontal=�6   .+�5      �� $��
Talk name:��6   �� 5      -   5         ��Name$ TalkTag��5      $	��216��6+   $��20,0,0,0q�67    �� TextWrapping$ NoWrap=�6I   $��For example 'Statement'��6�   
�������$��Center=�6�   $��24��6  5      �� 5      $ 
Horizontal=�6   $��0,4,0,0q�62   .+�5      �� $��Character name:��6   �� 5      -   5        $ CharacterName��5      $	��205��61   $��0,0,0,0q�6=   $ NoWrap=�6N   $#��Name of the talking character��6�   
�������$��Center=�6�   $��24��6  5      �� 5      $��
Talk text:��6   $  False��6)   �� 5       -   5        $ MainTextBox��5       $	��300��6+   $ NoWrap=�67   $
��Left=�6o   $	��100��6�   $��12(�6�   $��1,0,1,1q�6�   
�������	 �� AcceptsReturn$
	 True��6"  .��67  What the character says...5"      �� 5#      $ 
Horizontal=�6   .+�5$      �� $��	130,0,0,0q�6   .��6+   
Next talk:67   9   3GPGameDevDialogueEditor.UICustomObjects.LineStarter  5%      -	   5        
  Name$
 starter��5%   !   $��	10,6,0,15q�62   $��Right=�6E   6c   5'      �� 5(      -
   5         ��Name$ IsConditionalCB��5(      ���	#FFFF7474.��6z   Is Conditional6�   �� 5*      -   5         ��Name$ ConditionContainer��5*      5,      �� 5-      $ 
Horizontal=�6   .+�5.      �� ���	#3300FFFF$	��150��6/   $��0,1,0,2q�6;   $��Actions:��6L   �� 5/      -   5        $��1,1,1,2q�5/   1   $��
Add action��6B   $��69��6W   50      �� 52      -   5        $ ActionStackPanel��52      $��20,0,0,0q�63   54      �� 55      $ 
Horizontal=�6   .+�56      �� ���	#3300FF04$	��150��6/   $��0,2,0,2q�6;   $��Answers:��6L   �� 57      -   5        $
��Left=�57      $��1,1,1,2q�6F   $��Add answer:��6W   58      �� 5:      -   5        $ AnswerStackPanel��5:      $��20,0,0,0q�63   5<      5=      5>      5?      