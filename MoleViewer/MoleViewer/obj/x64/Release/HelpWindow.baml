   M S B A M L   `   `   `  ����   
MoleViewer    MoleViewer.HelpWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$
  Help��$��476.016��$��451.883�� � 
ResizeMode$ CanMinimize=�.��� .+��� $	��Key��$
��Left=�$	��Top=� J� RenderTransformOrigin$ 1.441,2.138&�$��10,0,0,0q�$	��181��$��16(�� $
��Left=�$	��112��$	��Top=�$	��181��$��	10,28,0,0q� � ColumnDefinitions �� $	��10* ��� $	��80* ��� $	��91* � � RowDefinitions �� $��2* ��� $��2* ��� $��2* ��� $��2* �.+��� $
��Left=�$��28�� �� TextWrapping$
 Wrap=�$��	Protein 1��$	��Top=�$��90�� �� 
IsReadOnly . $��14(� �� TextAlignment$ Center=�$��0��{�.  $
��Bold"� �� TextDecorations$ 	Underline{�$��2���� $
��Left=�$��28��$
 Wrap=�$��Carbon��$	��Top=�$��90�� . $��14(�$ Center=�$��1��{�.  ���	#FF6495ED$
��Bold"�$��2���� $
��Left=�$��28��$
 Wrap=�$��Nitrogen��$	��Top=�$��90�� . $��14(�$ Center=�$��2��{�.  ���	#FFAFEEEE$
��Bold"�$��2���� $
��Left=�$��28��$
 Wrap=�$��Oxygen��$	��Top=�$��90�� . $��14(�$ Center=�$��3��{�.  ���	#FFFFFFF0$
��Bold"�$��2���� $
��Left=�$��28��$
 Wrap=�$��	Protein 2��$	��Top=�$��90�� . $��14(�$ Center=�$��0��$��2��{�.  $
��Bold"�$ 	Underline{��� $
��Left=�$��28��$
 Wrap=�$��Carbon��$	��Top=�$��90�� . $��14(�$ Center=�$��1��$��2��{�.  ���	#FFDC143C$
��Bold"��� $
��Left=�$��28��$
 Wrap=�$��Nitrogen��$	��Top=�$��90�� . $��14(�$ Center=�$��2��$��2��{�.  ���	#FFDB7093$
��Bold"��� $
��Left=�$��28��$
 Wrap=�$��Oxygen��$	��Top=�$��90�� . $��14(�$ Center=�$��3��$��2��{�.  ���	#FFFFA07A$
��Bold"��� $��Alignment Algorithm��$
��Left=�$	��Top=�$ 1.441,2.138&�$��
10,142,0,0q�$	��181��$��16(��� $
��Left=�$
��Wrap=�$	��Top=�$��
10,173,0,0q�$	��264��$	��426���� $���MoleViewer uses an Iterative Closest Point (ICP) algorithm to align alpha carbon positions in proteins. The residue of first chain farthest away from an alpha carbon on the opposite chain and the residue of the opposite alpha carbon are highlighted.���� �� �� �� $F�@ICP is used to minimize the difference between two point clouds:���� �� $O�I1. For each point in the source, find the closest point in the reference.���� �� $g�a2. Estimate the rotation and translation matrix by minimizing error between all associated points���� �� $$�3. Transform the source points���� �� $3�-4. Iterate and Revise transformation matrices���� �� �� �� $$�For more information, consult:���� �� $���Pomerleau, François, Francis Colas, and Roland Siegwart. "A Review of Point Cloud Registration Algorithms for Mobile Robotics." FNT in Robotics Foundations and Trends in Robotics 4.1 (2015): 1-104.���� $
��Left=�$
��Wrap=�$	��Top=�$��
219,28,0,0q�$	��112��$	��217���� $&�  - Left Click and Drag to Rotate���� �� $+�% - Middle Mouse Click and Drag to Pan���� �� $'�! - Scroll the Mouse Wheel to Zoom���� $��Camera Controls��$
��Left=�$	��Top=�$ 1.441,2.138&�$��	219,0,0,0q�$	��181��$��16(�