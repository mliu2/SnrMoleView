   M S B A M L   `   `   `  ����  
MoleViewer    MoleViewer.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
MoleViewer��5   	   $	��547��6   $	��767��6)    � 
ResizeMode$ CanMinimize=�65   .��5      � .+�5   
   �� 
���   �$��1q�6%   $
��Left=�69   $��40��6T   $	��Top=�6`   $	��284��6x   $��
465,34,0,0q�6�   ._�5      �� -   5         ��Name$ Load1��5      $
��Load��6$   $
��Left=�63   $	��Top=�6N   $��75��6f   $��	198,9,0,0q�6�    � ToolTip$" Browse for .cif file to load��6�   5      �� 5	   
   
���   �$��1q�6%   $
��Left=�69   $��40��6T   $	��Top=�6`   $	��284��6x   $��
465,73,0,0q�6�   ._�5
      �� -   5        $ Load2��5
      $
��Load��6$   $
��Left=�63   $	��Top=�6N   $��75��6f   $��	198,9,0,0q�6�   $" Browse for .cif file to load��6�   5      �� 5   
   -   5         ��Name$ 	FileName1��5      $
��Left=�6%   $��23��6@    �� TextWrapping$
 Wrap=�6L   $��	Protein 1��6`   $	��Top=�6q   $	��174��6�   $��
477,45,0,0q�6�    �� 
IsReadOnly . $ Protein 1 (Reference)��6�   �� 5   
   -   5        $ 	FileName2��5      $
��Left=�6%   $��23��6@   $
 Wrap=�6L   $��	Protein 2��6`   $	��Top=�6q   $	��174��6�   $��
477,81,0,0q�6�    . $ Protein 2 (Source)��6�   �� 5   
   -   5        $ Output��5      $
��Left=�6"   $	��351��6=   $
 Wrap=�6J   $	��Top=�6^   $	��284��6v   $��465,158,0,0q�6�    . " �� VerticalScrollBarVisibility$
 Auto=�6�   $ Output Feed��6�   �� 5   
   -   5         ��Name$ display��5      $
��Left=�6"   $	��475��6=   $	��Top=�6J   $	��450��6b   	 J� RenderTransformOrigin$	 0.945,0.924&�6n   $��	10,34,0,0q�6�   #��� ��6�   $_ YClick and Drag to Rotate.
Mouse Wheel to Zoom.
Click Middle Mouse Button and Drag to Pan.��6\  .+�5      '� -   5        
 '�Name$
 	myDisplay��5      $
��Left=�6,   $	��475��6G   $	��Top=�6T   $	��450��6l   x�5      g� 5       g� Content 5      t� 5      -   5        myLight�� �� Color$ #FFFFFF��5   A    t� 	Direction$ -0.612372,-0.5,-0.6123725�6Q   5       '� Camera 5      =� 5      -	   5        PCamera�� � Position$ 0,0,301�5   9    � LookDirection$ 0,0,-15�6K    =� FieldOfView$ 60Q�6b   5      5      �� 5   
   -
   5        $ Align��5      $��Align��6    $
��Left=�60   $��665,118,0,0q�6K   $	��Top=�6`   $��75��6x   $� �Aligns Protein 2 to Protein 1 and reports the RMSD.
The residue of first chain farthest away from an alpha carbon on the opposite chain
and the residue of the opposite alpha carbon are highlighted. ��6�   �� 5   
   -   5         ��Name$ FullAtomToggle��5      $��Full Atom Model��6+   $
��Left=�6E   $��477,123,0,0q�6`   $	��Top=�6u   $@ :Toggles between Alpha Carbon Only and Full Atom rendering.��6�   c� 5   
   $
��Left=�6   $	��Top=�6-   $	��761��6E   $��27��6Q   ���	#FFE7F2FF.@�5       �� -   5        $
 help��5       $
��Help��6#   $��22��62   $	��Top=�6>   $��75��6V   6a   �� 5!      -   5        $ about��5!      $��About��6$   $��22��64   $	��Top=�6@   $��75��6X   6c   5"      5#      5$      