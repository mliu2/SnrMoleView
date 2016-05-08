using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;

namespace MoleViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //backend object for handling data
        private Backend m_backend;
        //3d display objects
        private Model3DGroup m_myModel3DGroup;
        private ModelVisual3D m_myModelVisual3D;
        //members for tracking camera movement
        private Transform3DGroup m_transform;
        private AxisAngleRotation3D m_rotation = new AxisAngleRotation3D();
        private Vector3D m_previousPosition3D = new Vector3D(0, 0, 1);
        private TranslateTransform3D m_translate = new TranslateTransform3D(0,0,0);
        //holds mouse state for camera movement handlers
        private bool m_mDown;
        private Point m_mLastPos;
        private bool m_mMidDown;
        //checks what kind of representation
        private bool m_fullAtom = false; 

        /// <summary>
        /// Displays main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            m_backend = new Backend();
            Shape3d();
        }
        /// <summary>
        /// Handler for showing the Help window when the help button is clicked.
        /// </summary>
        /// <param name="sender">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        /// <param name="e">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        private void ShowHelp(object sender, RoutedEventArgs e)
        {
            HelpWindow help = new HelpWindow();
            help.Show();
        }
        /// <summary>
        /// Handler for showing the About window when the about button is clicked.
        /// </summary>
        /// <param name="sender">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        /// <param name="e">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Show();
        }
        /// <summary>
        /// Initiates the Viewport3D content. Adds model object to the display.
        /// Initiates transform object for camera movement.
        /// </summary>
        private void Shape3d()
        {

            // Declare scene objects.
            m_myModel3DGroup = new Model3DGroup();
            m_myModelVisual3D = new ModelVisual3D();

            myDisplay.Children.Add(m_myModelVisual3D);

            m_myModelVisual3D.Content = m_myModel3DGroup;

            m_transform = new Transform3DGroup();
            m_transform.Children.Add(new RotateTransform3D(m_rotation));
        }
        /// <summary>
        /// Creates the 3d atom model. Calls GenerateSphere out of the backend to make the mesh.
        /// Applies a solid color brush and diffuse material to the mesh to texture the model.
        /// Adds the model to the modelGroup
        /// </summary>
        /// <param name="atom">The atom that is being represented</param>
        /// <param name="radius">Radius of the sphere to generate</param>
        /// <param name="color">Color of the sphere to generate</param>
        private void MakeAtom(Atom atom, double radius, Color color)
        {
            MeshGeometry3D mesh = m_backend.GenerateSphere(new Point3D(atom.X, atom.Y, atom.Z), radius, 9, 9);
            GeometryModel3D geomod = new GeometryModel3D();
            geomod.Geometry = mesh;
            SolidColorBrush solidBrush = new SolidColorBrush(color);
            geomod.Material = new DiffuseMaterial(solidBrush);
            m_myModel3DGroup.Children.Add(geomod);
        }
        /// <summary>
        /// Generates a slightly larger transparent sphere to highlight the atom.
        /// Creates a slightly larger sphere using GenerateSphere out of the backend.
        /// Applies a transparent solid color brush and an emissive material to the mesh.
        /// Adds the model to the modelGroup
        /// </summary>
        /// <param name="atom">Atom that is being highlighted</param>
        private void MakeAtomHighlight(Atom atom)
        {
            // .8 angstrom is slightly larger than any of the atoms
            MeshGeometry3D mesh = m_backend.GenerateSphere(new Point3D(atom.X, atom.Y, atom.Z), .8, 9, 9);
            GeometryModel3D geomod = new GeometryModel3D();
            geomod.Geometry = mesh;
            SolidColorBrush solidBrush = new SolidColorBrush(Colors.Yellow);
            solidBrush.Opacity = .5;
            geomod.Material = new EmissiveMaterial(solidBrush);
            m_myModel3DGroup.Children.Add(geomod);
        }
        /// <summary>
        /// Generates the 3d model for protein 1.
        /// Calls MakeAtom(Atom,double,Color) using the correct radii and color scheme.
        /// Calls MakeAtomHighlight(Atom) to highlight atom with greatest distance.
        /// </summary>
        private void MakeProt1()
        {

            foreach (Atom atom in m_backend.Protein1.Atoms)
            {
                if (atom.CA == true)
                {
                    MakeAtom(atom, Atom.C_RAD, Colors.CornflowerBlue);
                }
                else if (atom.Ele == "C" && m_fullAtom)
                {
                    MakeAtom(atom, Atom.C_RAD, Colors.CornflowerBlue);
                }
                else if (atom.Ele == "N" && m_fullAtom)
                {
                    MakeAtom(atom, Atom.N_RAD, Colors.PaleTurquoise);
                }
                else if (atom.Ele == "O" && m_fullAtom)
                {
                    MakeAtom(atom, Atom.O_RAD, Colors.Ivory);
                }

                if (atom.Residue_Num == m_backend.MaxDistResidue.Prot1Num && atom.Chain == m_backend.MaxDistResidue.Chain1)
                {
                    MakeAtomHighlight(atom);
                }
            }
        }
        /// <summary>
        /// Generates the 3d model for protein 2.
        /// Calls MakeAtom(Atom,double,Color) using the correct radii and color scheme.
        /// Calls MakeAtomHighlight(Atom) to highlight atom with greatest distance.
        /// </summary>
        private void MakeProt2()
        {
            foreach (Atom atom in m_backend.Protein2.Atoms)
            {

                if (atom.CA == true)
                {
                    MakeAtom(atom, Atom.C_RAD, Colors.Crimson);
                }
                else if (atom.Ele == "C" && m_fullAtom)
                {
                    MakeAtom(atom, Atom.C_RAD, Colors.Crimson);
                }
                else if (atom.Ele == "N" && m_fullAtom)
                {
                    MakeAtom(atom, Atom.N_RAD, Colors.PaleVioletRed);
                }
                else if (atom.Ele == "O" && m_fullAtom)
                {
                    MakeAtom(atom, Atom.O_RAD, Colors.LightSalmon);
                }

                if (atom.Residue_Num == m_backend.MaxDistResidue.Prot2Num && atom.Chain == m_backend.MaxDistResidue.Chain2)
                {
                    MakeAtomHighlight(atom);
                }
            }
        }
        /// <summary>
        /// Redraws both models. 
        /// Clears the model group and calls MakeProt1() and MakeProt2() to remake the models.
        /// </summary>
        private void redraw()
        {
            m_myModel3DGroup.Children.Clear();
            try
            {
                MakeProt1();
                MakeProt2();
            }
            catch (Exception ex)
            {
                Output.Text += ex.Message + "\r\n";
            }
        }
        /// <summary>
        /// Moves the camera position back to the origin.
        /// </summary>
        private void FocusProt()
        {
            PCamera.Position = new Point3D(0, 0, PCamera.Position.Z);
        }
        /// <summary>
        /// Handler for the Load button of protein 1.
        /// Opens a file browser dialog and gets the filepath. 
        /// Parses the file and reports te number of atoms loaded.
        /// Focuses the camera and redraws.
        /// </summary>
        /// <param name="sender">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        /// <param name="e">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            //file browser window
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "PDBx/mmCIF Files|*.cif";
            openFileDialog1.Title = "Select a PDBx/mmCIF File";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    //display number of atoms loaded and file name
                    int num = m_backend.Prot1Parse(openFileDialog1.FileName);
                    FileName1.Text = openFileDialog1.SafeFileName;
                    Output.Text += "***\r\n" + openFileDialog1.FileName + " : " + num + " atoms loaded \r\n***\r\n";
                    FocusProt();
                    redraw();
                }
                catch (Exception ex)
                {
                    Output.Text += ex.Message + "\r\n";
                }
                
            }
        }
        /// <summary>
        /// Handler for the Load button of protein 2.
        /// Opens a file browser dialog and gets the filepath. 
        /// Parses the file and reports te number of atoms loaded.
        /// Focuses the camera and redraws.
        /// </summary>
        /// <param name="sender">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        /// <param name="e">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            //file browser window
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "PDBx/mmCIF Files|*.cif";
            openFileDialog1.Title = "Select a PDBx/mmCIF File";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    //display number of atoms loaded and file name
                    int num = m_backend.Prot2Parse(openFileDialog1.FileName);
                    FileName2.Text = openFileDialog1.SafeFileName;
                    Output.Text += "***\r\n" + openFileDialog1.FileName + " : " + num + " atoms loaded \r\n***\r\n";
                    FocusProt();
                    redraw();
                }
                catch (Exception ex)
                {
                    Output.Text += ex.Message + "\r\n";
                }
            }
        }
        //HANDLERS FOR MOUSE CAMERA CONTROL
        /// <summary>
        /// Zooms the camera by mouse wheel.
        /// Adjusts the camera position along the Z axis when the mouse wheel is scrolled.
        /// </summary>
        /// <param name="sender">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        /// <param name="e">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        private void Mouse_Wheel(object sender, MouseWheelEventArgs e)
        {
            PCamera.Position = new Point3D(PCamera.Position.X, PCamera.Position.Y, PCamera.Position.Z - e.Delta / 100D);
        }
        /// <summary>
        /// Sets the variables that check if the mouse buttons are held to false
        /// </summary>
        /// <param name="sender">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        /// <param name="e">The mouse event that triggers this handler</param>
        private void Mouse_Up(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                m_mDown = false;
            }
            if (e.MiddleButton == MouseButtonState.Released)
            {
                m_mMidDown = false;
            }
        }
        /// <summary>
        /// Handler for when mouse buttons are pressed in the Viewport3D.
        /// If middle mouse button is clicked, it sets the flag for middle mouse button being held, and remembers the mouse position.
        /// If left mouse button is clicked, it sets the flag for the left mouse button being held, and projects the mouse position to a trackball to calculate perceived depth.
        /// </summary>
        /// <param name="sender">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        /// <param name="e">The mouse event that triggers this handler</param>
        private void Mouse_Down(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed && e.MiddleButton != MouseButtonState.Pressed) return;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                m_mDown = true;
                m_previousPosition3D = ProjectToTrackball(ActualWidth,ActualHeight, e.GetPosition(this));
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                m_mMidDown = true;
            }
            Point pos = Mouse.GetPosition(myDisplay);
            m_mLastPos = new Point(pos.X - myDisplay.ActualWidth / 2, myDisplay.ActualHeight / 2 - pos.Y);
        }
        /// <summary>
        /// Handler for when mouse moves in the Viewport3D
        /// If middle mouse is held and moved, the camera will move along the perceived XY plane based on how much the mouse moved.
        /// The percieved xy plane is calculated by finding the up direction of the camera (y axis) and getting the cross product with the look direction (getting the x axis).
        /// If left mouse is held and moved, the camera and light source will be transformed based on the Track() function.
        /// </summary>
        /// <param name="sender">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        /// <param name="e">The mouse event that trigger this handler</param>
        private void Mouse_Move(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (m_mMidDown)
            {
                int speed = 500;

                Point pos = Mouse.GetPosition(myDisplay);
                Point actualPos = new Point(pos.X - myDisplay.ActualWidth / 2, myDisplay.ActualHeight / 2 - pos.Y);
                double dx = actualPos.X - m_mLastPos.X, dy = actualPos.Y - m_mLastPos.Y;

                //calculate perceived xy plane
                PCamera.Position += (PCamera.UpDirection * dy/speed);
                PCamera.Position += (Vector3D.CrossProduct(PCamera.LookDirection, PCamera.UpDirection) * dx/speed);
            }
            if (m_mDown)
            {
                Track(e.GetPosition(this));
                PCamera.Transform = m_transform;
                myLight.Transform = m_transform;
            }
        }
        /// <summary>
        /// When the mouse leaves the Viewport3D object space, unflags the mouse state booleans.
        /// </summary>
        /// <param name="sender">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        /// <param name="e">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        private void Mouse_Leave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            m_mMidDown = false;
            m_mDown = false;
        }
        /// <summary>
        /// From 3Dtools library 
        /// (c) 2007 
        /// Copyright Microsoft Corporation. Subject to Microsoft Limited Permissive License. 
        /// https://3dtools.codeplex.com/SourceControl/latest#3DTools/3DTools/TrackballDecorator.cs
        /// Projects a point to the trackball which the camera will be transformed along.
        /// </summary>
        /// <param name="width">Width of area to track in</param>
        /// <param name="height">Height of area to track in</param>
        /// <param name="point">Point that needs to be projected.</param>
        /// <returns>A 3d vector holding the x,y,z values of the point that has been projected</returns>
        private Vector3D ProjectToTrackball(double width, double height, Point point)
        {
            double x = point.X / (width / 2);    // Scale so bounds map to [0,0] - [2,2]
            double y = point.Y / (height / 2);

            x = x - 1;                           // Translate 0,0 to the center
            y = 1 - y;                           // Flip so +Y is up instead of down

            double z2 = 1 - x * x - y * y;       // z^2 = 1 - x^2 - y^2
            double z = z2 > 0 ? Math.Sqrt(z2) : 0;

            return new Vector3D(x, y, z);
        }
        /// <summary>
        /// From 3Dtools library 
        /// (c) 2007 
        /// Copyright Microsoft Corporation. Subject to Microsoft Limited Permissive License. 
        /// https://3dtools.codeplex.com/SourceControl/latest#3DTools/3DTools/TrackballDecorator.cs
        /// Recalculates the rotation3d object from the new position of the mouse. 
        /// Axis of rotation is the crossproduct of the previous trackball vector and the current trackball vector.
        /// Angle of rotation is the angle between the previous trackball vector and the current trackball vector.
        /// This new rotation is composed with the previous rotation to generate the new total rotation.
        /// </summary>
        /// <param name="currentPosition">Point representing the current mouse position.</param>
        private void Track(Point currentPosition)
        {
            Vector3D currentPosition3D = ProjectToTrackball(
                ActualWidth, ActualHeight, currentPosition);

            Vector3D axis = Vector3D.CrossProduct(m_previousPosition3D, currentPosition3D);
            double angle = Vector3D.AngleBetween(m_previousPosition3D, currentPosition3D);

            // quaterion will throw if this happens - sometimes we can get 3D positions that
            // are very similar, so we avoid the throw by doing this check and just ignoring
            // the event 
            if (axis.Length == 0) return;

            Quaternion delta = new Quaternion(axis, -angle);

            // Get the current orientantion from the RotateTransform3D
            Quaternion q = new Quaternion(m_rotation.Axis, m_rotation.Angle);

            // Compose the delta with the previous orientation
            q *= delta;

            // Write the new orientation back to the Rotation3D
            m_rotation.Axis = q.Axis;
            m_rotation.Angle = q.Angle;

            m_previousPosition3D = currentPosition3D;
        }
        /// <summary>
        /// Handler for when the Alignm button is clicked.
        /// Calls ICPAlignment() out of the backend object, redraws the models, and reports the RMSD between the two proteins.
        /// </summary>
        /// <param name="sender">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        /// <param name="e">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        private void Align_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double rmsd = m_backend.ICPAlignment();
                redraw();
                Output.Text += "RMSD = " + Math.Round(Convert.ToDecimal(rmsd), 3) + "\r\n";
                Output.Text += "Alignment Complete.\r\n";
            }
            catch (Exception ex)
            {
                Output.Text += ex.Message + "\r\n";
            }
        }

        /// <summary>
        /// Handler for when the fullatom checkbox is toggled.
        /// It will set the flag for full atom representation and redaw the models.
        /// </summary>
        /// <param name="sender">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        /// <param name="e">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        private void FullAtomToggle_Checked(object sender, RoutedEventArgs e)
        {
            m_fullAtom = true;
            try
            {
                redraw();
            }
            catch (Exception ex)
            {
                Output.Text += ex.Message + "\r\n";
            }
        }
        /// <summary>
        /// Handler for when the fullatom checkbox is untoggled.
        /// It will reset the flag for full atom representation and redaw the models.
        /// </summary>
        /// <param name="sender">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        /// <param name="e">Doesn't do anything, but required to delegate RoutedEventHandler</param>
        private void FullAtomToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            m_fullAtom = false;
            redraw();
        }

    }
}
