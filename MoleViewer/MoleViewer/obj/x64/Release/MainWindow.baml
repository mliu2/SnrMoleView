   M S B A M L   `   `   `  ����   
MoleViewer    MoleViewer.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  
MoleViewer��$	��547��$	��767�� � 
ResizeMode$ CanMinimize=�.��� .+��� 
���   �$��1q�$
��Left=�$��40��$	��Top=�$	��284��$��
465,34,0,0q�._��� -    ��Name$ Load1��$
��Load��$
��Left=�$	��Top=�$��75��$��	198,9,0,0q� � ToolTip$" Browse for .cif file to load���� 
���   �$��1q�$
��Left=�$��40��$	��Top=�$	��284��$��
465,73,0,0q�._��� -   $ Load2��$
��Load��$
��Left=�$	��Top=�$��75��$��	198,9,0,0q�$" Browse for .cif file to load���� -    ��Name$ 	FileName1��$
��Left=�$��23�� �� TextWrapping$
 Wrap=�$��	Protein 1��$	��Top=�$	��174��$��
477,45,0,0q� �� 
IsReadOnly . $ Protein 1 (Reference)���� -   $ 	FileName2��$
��Left=�$��23��$
 Wrap=�$��	Protein 2��$	��Top=�$	��174��$��
477,81,0,0q� . $ Protein 2 (Source)���� -   $ Output��$
��Left=�$	��351��$
 Wrap=�$	��Top=�$	��284��$��465,158,0,0q� . " �� VerticalScrollBarVisibility$
 Auto=�$ Output Feed���� -    ��Name$ display��$
��Left=�$	��475��$	��Top=�$	��450��	 J� RenderTransformOrigin$	 0.945,0.924&�$��	10,34,0,0q�#��� ��$_ YClick and Drag to Rotate.
Mouse Wheel to Zoom.
Click Middle Mouse Button and Drag to Pan.��.+�'� -   
 '�Name$
 	myDisplay��$
��Left=�$	��475��$	��Top=�$	��450��x�g�  g� Content t� -   myLight�� �� Color$ #FFFFFF�� t� 	Direction$ -0.612372,-0.5,-0.6123725� '� Camera =� -	   PCamera�� � Position$ 0,0,301� � LookDirection$ 0,0,-15� =� FieldOfView$ 60Q��� -
   $ Align��$��Align��$
��Left=�$��665,118,0,0q�$	��Top=�$��75��$� �Aligns Protein 2 to Protein 1 and reports the RMSD.
The residue of first chain farthest away from an alpha carbon on the opposite chain
and the residue of the opposite alpha carbon are highlighted. ���� -    ��Name$ FullAtomToggle��$��Full Atom Model��$
��Left=�$��477,123,0,0q�$	��Top=�$@ :Toggles between Alpha Carbon Only and Full Atom rendering.��c� $
��Left=�$	��Top=�$	��761��$��27�����	#FFE7F2FF.@��� -   $
 help��$
��Help��$��22��$	��Top=�$��75���� -   $ about��$��About��$��22��$	��Top=�$��75��