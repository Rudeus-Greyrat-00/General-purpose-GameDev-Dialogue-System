   M S B A M L   `   `   `  ����R  NGPGameDevDialogueEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:GPGameDevDialogueEditorGPGameDevDialogueEditor  5   	   m;clr-namespace:GPGameDevDialogueEditor.DialogueObjectEditors-GPGameDevDialogueEditor.DialogueObjectEditors  5   	   a5clr-namespace:GPGameDevDialogueEditor.UICustomObjects'GPGameDevDialogueEditor.UICustomObjects  5   	   (    "GPGameDevDialogueEditor.MainWindow   5   	   W SPresentationCore, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:GPGameDevDialogueEditor     5   	   Juc;clr-namespace:GPGameDevDialogueEditor.DialogueObjectEditors     5   	   Dco5clr-namespace:GPGameDevDialogueEditor.UICustomObjects     5   	   -   5          � Title$  GPEditor��5   	   $
��1150��6   $
��1800��6(   .��5      �  � RowDefinitions 5   
   �� 5      $��20��6   $��* �6+   68   �� 5      6    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0��6.   .+�5      o� .@�5      m� $
��File��6   .@�5      m� -   5        $	��New��5      6,   m� 5      -   5        $
��Open��5      6-   m� 5      -   5        $��Save (Ctrl + S)��5      68   �� 5      6"   m� 5      -   5        $��Save as��5      6A   �� 5      m� 5      -   5        $��Export (Ctrl + E)��5      6L   �� 5      6"   m� 5       $
��Exit��6   5!      5"      o� 5#      .@�5$      m� $��_Edit��6   .@�5%      m� -   5        $��New talk (Ctrl + T)��5%      6<   m� 5&      -   5        $��Test dialogue (F5)��5&      6;   5'      5(      o� 5)      .@�5*      m� $��_View��6   .@�5+      m� -	   5        $ ��Initial position and scale��5+      6C   m� 5,      -
   5        $!��Dialogue simulation console��5,      6D   5-      5.      5/      �� 50   
   -   5         ��Name$ MainScrollViewer��50      $��1��61   $��2��6>   $��Hidden=�6R   $��Hidden=�6w   .��51      �� -   5         ��Name$ 
MainCanvas��51      $��100000��52   =   $��100000��6M   ��53      !� 54       j� TileMode$
 Tile=�6"    j� Stretch$ Uniform=�62    j� Viewport$ 25,25,25,25�6D   	 j� ViewportUnits$	 Absolute=�6[   
 !� Visual
 55      �� 56      $��40��6(   $��40��63   
�������
�������$	��0.1��6Z   58      z�5:      u� 5;      6(   .+�5=      =   7GPGameDevDialogueEditor.UICustomObjects.DialogueStarter  -   5          Name$ MainStarter��5=   %    �� Left$ 10010��6:   
 �� Top$ 10010��6N   6c   5>      5?      � 5@   
   $��1��6   $��1��6   .+�5A      B   <GPGameDevDialogueEditor.UICustomObjects.DialogueSimulationUI  -   5          Name$! DialogueSimulationUIElement��5A   &   $y�Hidden=�6K   6a   5B      5D      5E      