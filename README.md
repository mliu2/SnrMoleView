# SnrMoleView
Molecular Viewer for Senior Project

Installation

To install the program, download the program from the GitHub repository found at https://github.com/mliu2/SnrMoleView. To use the program, open the MoleViewer.exe which should be found in the /MoleViewer/MoleViewer/bin/x64/Release/ directory. MoleViewer requires a 64 bit Windows operating system and the Microsoft .NET Framework.

User Manual

In order to load in a protein model, click the load button beside the corresponding slot. A dialog box will prompt for the file, which should be a PDBx/mmCIF file from www.rcsb.org. The protein model will be displayed in the gray area to the left. Use the mouse to move the camera. Left click and drag inside of the gray area to rotate the camera. Middle click and drag inside of the gray area to pan the camera. With the mouse cursor in the gray area, scroll the mouse wheel to zoom in or out. At the top left is a Help button. This will display another window that explains the camera controls, the alignment algorithm, as well as the color key for the protein model. To the right is an About button, which will give more information about the source code of the the project. Just bellow the Load buttons is an Align button. When two proteins are loaded, this will align the second protein to the first, highlight the residue on the first protein that is most distance from the nearest alpha carbon on the second protein, and provide the RMSD. The RMSD is shown in the Message Feed, the text display found in the bottom right of the screen. The Full Atom Model checkbox will toggle between displaying only the alpha carbons and displaying all of the atoms. 