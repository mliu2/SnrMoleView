   M S B A M L   `   `   `  ����  
MoleViewer    MoleViewer.HelpWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$
  Help��5   	   $��476.016��6   $��451.883��6'    � 
ResizeMode$ CanMinimize=�67   .��5      � .+�5   
   �� $	��Key��6   $
��Left=�6   $	��Top=�69    J� RenderTransformOrigin$ 1.441,2.138&�6Q   $��10,0,0,0q�6u   $	��181��6�   $��16(�6�   � 5   
   $
��Left=�6   $	��112��6*   $	��Top=�67   $	��181��6O   $��	10,28,0,0q�6[    � ColumnDefinitions 5      �� 5	      $	��10* �6#   �� 5
      $	��80* �6#   �� 5      $	��91* �6#    � RowDefinitions 5      �� 5      $��2* �6    �� 5      $��2* �6    �� 5      $��2* �6    �� 5      $��2* �6    .+�5      �� $
��Left=�6   $��28��61    �� TextWrapping$
 Wrap=�6=   $��	Protein 1��6Q   $	��Top=�6b   $��90��6z    �� 
IsReadOnly . $��14(�6�    �� TextAlignment$ Center=�6�   $��0��6�   {�.  $
��Bold"�6�    �� TextDecorations$ 	Underline{�6�   $��2��6	  �� 5      $
��Left=�6   $��28��61   $
 Wrap=�6=   $��Carbon��6Q   $	��Top=�6_   $��90��6w    . $��14(�6�   $ Center=�6�   $��1��6�   {�.  ���	#FF6495ED$
��Bold"�6�   $��2��6  �� 5      $
��Left=�6   $��28��61   $
 Wrap=�6=   $��Nitrogen��6Q   $	��Top=�6a   $��90��6y    . $��14(�6�   $ Center=�6�   $��2��6�   {�.  ���	#FFAFEEEE$
��Bold"�6�   $��2��6  �� 5      $
��Left=�6   $��28��61   $
 Wrap=�6=   $��Oxygen��6Q   $	��Top=�6_   $��90��6w    . $��14(�6�   $ Center=�6�   $��3��6�   {�.  ���	#FFFFFFF0$
��Bold"�6�   $��2��6  �� 5      $
��Left=�6   $��28��61   $
 Wrap=�6=   $��	Protein 2��6Q   $	��Top=�6b   $��90��6z    . $��14(�6�   $ Center=�6�   $��0��6�   $��2��6�   {�.  $
��Bold"�6�   $ 	Underline{�6�   �� 5      $
��Left=�6   $��28��61   $
 Wrap=�6=   $��Carbon��6Q   $	��Top=�6_   $��90��6w    . $��14(�6�   $ Center=�6�   $��1��6�   $��2��6�   {�.  ���	#FFDC143C$
��Bold"�6�   �� 5      $
��Left=�6   $��28��61   $
 Wrap=�6=   $��Nitrogen��6Q   $	��Top=�6a   $��90��6y    . $��14(�6�   $ Center=�6�   $��2��6�   $��2��6�   {�.  ���	#FFDB7093$
��Bold"�6  �� 5      $
��Left=�6   $��28��61   $
 Wrap=�6=   $��Oxygen��6Q   $	��Top=�6_   $��90��6w    . $��14(�6�   $ Center=�6�   $��3��6�   $��2��6�   {�.  ���	#FFFFA07A$
��Bold"�6�   5      �� 5   
   $��Alignment Algorithm��6   $
��Left=�6.   $	��Top=�6I   $ 1.441,2.138&�6a   $��
10,142,0,0q�6�   $	��181��6�   $��16(�6�   �� 5   
   $
��Left=�6   $
��Wrap=�6/   $	��Top=�6C   $��
10,173,0,0q�6[   $	��264��6o   $	��426��6|   �� 6�   $���MoleViewer uses an Iterative Closest Point (ICP) algorithm to align alpha carbon positions in proteins. The residue of first chain farthest away from an alpha carbon on the opposite chain and the residue of the opposite alpha carbon are highlighted.��6�   �� 6�  �� 6�  �� 6�  �� 6�  $F�@ICP is used to minimize the difference between two point clouds:��6�  �� 6�  �� 6  $O�I1. For each point in the source, find the closest point in the reference.��6  �� 6_  �� 6k  $g�a2. Estimate the rotation and translation matrix by minimizing error between all associated points��6o  �� 6�  �� 6�  $$�3. Transform the source points��6�  �� 6  �� 6  $3�-4. Iterate and Revise transformation matrices��6"  �� 6Y  �� 6e  �� 6k  �� 6w  $$�For more information, consult:��6{  �� 6�  �� 6�  $���Pomerleau, François, Francis Colas, and Roland Siegwart. "A Review of Point Cloud Registration Algorithms for Mobile Robotics." FNT in Robotics Foundations and Trends in Robotics 4.1 (2015): 1-104.��6�  6�  �� 5   
   $
��Left=�6   $
��Wrap=�6/   $	��Top=�6C   $��
219,28,0,0q�6[   $	��112��6o   $	��217��6|   �� 6�   $&�  - Left Click and Drag to Rotate��6�   �� 6�   �� 6�   $+�% - Middle Mouse Click and Drag to Pan��6�   �� 6�   �� 6  $'�! - Scroll the Mouse Wheel to Zoom��6  62  �� 5    
   $��Camera Controls��6   $
��Left=�6*   $	��Top=�6E   $ 1.441,2.138&�6]   $��	219,0,0,0q�6�   $	��181��6�   $��16(�6�   5"      5#      