   M S B A M L   `   `   `  ����R  NGPGameDevDialogueEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullm;clr-namespace:GPGameDevDialogueEditor.DialogueObjectEditors-GPGameDevDialogueEditor.DialogueObjectEditors  5      a5clr-namespace:GPGameDevDialogueEditor.UICustomObjects'GPGameDevDialogueEditor.UICustomObjects  5      @    :GPGameDevDialogueEditor.DialogueObjectEditors.AnswerEditor   5      W SPresentationCore, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Mlocal;clr-namespace:GPGameDevDialogueEditor.DialogueObjectEditors     5      Duc5clr-namespace:GPGameDevDialogueEditor.UICustomObjects     5      .��5
      �� .+�5   
   �� $	��253��6   $��1��6    
���   �$��0,2,0,2q�68   $
��Left=�6I   6f   �� 5   
     �� Orientation$  
Horizontal=�6   .+�5      �� -   5         ��Name$ IsConditionalCB��5      $��0,2,0,0q�60   ���	#FFFF7474.��6�   Is Conditional6�   �� 5      $��75,0,0,0q�6   .��6&   
Next talk:62   9   3GPGameDevDialogueEditor.UICustomObjects.LineStarter  5      -   5          Name$ starter��5      $��	10,6,0,15q�6.   $��Right=�6A   6_   5      �� 5   
   -   5         ��Name$ AnswerConditionContainer��5      69   �� 5   
   $  
Horizontal=�6   .+�5      �� -   5         ��Name$ 
AnswerText��5      $��22��6*   $	��200��66   $
��Left=�6B   $��1,1,1,1q�6]    �� TextWrapping$
 Wrap=�6n   ���	#1900FF04
�������$��What the player says��6  $��Center=�6!  6E  �� 5      -   5        $��1,1,1,1q�5   (   $��50��69   .��6D   Remove6L   5      �� 5   
   $  
Horizontal=�6   .+�5      �� $��0,0,0,0q�6   ���	#3300FFFF$	��174��6<   $��23��6H   $��12(�6T   .��6b   Answer Actions:6s   �� 5      -   5        $��1,0,1,0q�5      $��78��68   $��
Add action��6C   6Z   5      �� 5   
   -   5        $ AnswerActionContainer��5      $	��270��64   $
��Left=�6@   $��22,1,1,1q�6[   6o   �� 5   
   5!      5"      