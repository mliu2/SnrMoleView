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
        private Backend backend;
        private Model3DGroup myModel3DGroup;
        private ModelVisual3D myModelVisual3D;

        private Transform3DGroup _transform;
        private AxisAngleRotation3D _rotation = new AxisAngleRotation3D();
        private Vector3D _previousPosition3D = new Vector3D(0, 0, 1);
        private TranslateTransform3D _translate = new TranslateTransform3D(0,0,0);
        private AxisAngleRotation3D _rotationX = new AxisAngleRotation3D(new Vector3D(0, 1, 0) , 0);
        private AxisAngleRotation3D _rotationY = new AxisAngleRotation3D(new Vector3D(-1, 0, 0) , 0);
        private AxisAngleRotation3D _rotationAboutZ = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0);

        private bool _fullAtom = false; 

        public MainWindow()
        {
            InitializeComponent();
            backend = new Backend();
            Shape3d();
        }
        private void InitializeToolbar()
        {
            ToolBarButton toolBarButton1 = new ToolBarButton("Help");
            ToolBarButton toolBarButton2 = new ToolBarButton("About");
        }
        private void ShowHelp(object sender, RoutedEventArgs e)
        {
            HelpWindow help = new HelpWindow();
            help.Show();
        }
        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Show();
        }
        private void Shape3d()
        {

            // Declare scene objects.
            myModel3DGroup = new Model3DGroup();
            myModelVisual3D = new ModelVisual3D();
         
            myDisplay.Children.Add(myModelVisual3D);

            myModelVisual3D.Content = myModel3DGroup;

            _transform = new Transform3DGroup();
            _transform.Children.Add(new RotateTransform3D(_rotation));
            _transform.Children.Add(new RotateTransform3D(_rotationX));
            _transform.Children.Add(new RotateTransform3D(_rotationY));
        }
        private void MakeAtom(Atom atom, double radius, Color color)
        {
            MeshGeometry3D mesh = backend.GenerateSphere(new Point3D(atom.X, atom.Y, atom.Z), radius, 9, 9);
            GeometryModel3D geomod = new GeometryModel3D();
            geomod.Geometry = mesh;
            SolidColorBrush solidBrush = new SolidColorBrush(color);
            geomod.Material = new DiffuseMaterial(solidBrush);
            myModel3DGroup.Children.Add(geomod);
        }
        private void MakeAtomHighlight(Atom atom)
        {
            MeshGeometry3D mesh = backend.GenerateSphere(new Point3D(atom.X, atom.Y, atom.Z), .8, 9, 9);
            GeometryModel3D geomod = new GeometryModel3D();
            geomod.Geometry = mesh;
            SolidColorBrush solidBrush = new SolidColorBrush(Colors.Yellow);
            solidBrush.Opacity = .5;
            geomod.Material = new EmissiveMaterial(solidBrush);
            myModel3DGroup.Children.Add(geomod);
        }
        private void MakeProt1()
        {
            
            foreach (Atom atom in backend.Protein1.Atoms)
            {
                if (atom.CA == true)
                {
                    MakeAtom(atom, Backend.C_RAD, Colors.CornflowerBlue);
                }
                else if (atom.Ele == "C" && _fullAtom)
                {
                    MakeAtom(atom, Backend.C_RAD, Colors.CornflowerBlue);
                }
                else if (atom.Ele == "N" && _fullAtom)
                {
                    MakeAtom(atom, Backend.N_RAD, Colors.PaleTurquoise);
                }
                else if (atom.Ele == "O" && _fullAtom)
                {
                    MakeAtom(atom, Backend.O_RAD, Colors.Ivory);
                }

                if (atom.Residue_Num == backend.MaxResidue1)
                {
                    MakeAtomHighlight(atom);
                }
            }
        }
        private void MakeProt2()
        {  
            foreach (Atom atom in backend.Protein2.Atoms)
            {

                if (atom.CA == true)
                {
                    MakeAtom(atom, Backend.C_RAD, Colors.Crimson);
                }
                else if (atom.Ele == "C" && _fullAtom)
                {
                    MakeAtom(atom, Backend.C_RAD, Colors.Crimson);
                }
                else if (atom.Ele == "N" && _fullAtom)
                {
                    MakeAtom(atom, Backend.N_RAD, Colors.PaleVioletRed);
                }
                else if (atom.Ele == "O" && _fullAtom)
                {
                    MakeAtom(atom, Backend.O_RAD, Colors.LightSalmon);
                }

                if (atom.Residue_Num == backend.MaxResidue2)
                {
                    MakeAtomHighlight(atom);
                }
            }
        }
        private void redraw()
        {
            myModel3DGroup.Children.Clear();
            MakeProt1();
            MakeProt2();
        }
        private void FocusProt()
        {
            PCamera.Position = new Point3D(0, 0, PCamera.Position.Z);
        }
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
                    int num = backend.Prot1Parse(openFileDialog1.FileName);
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
                    int num = backend.Prot2Parse(openFileDialog1.FileName);
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
        

        private bool mDown;
        private Point mLastPos;
        private bool mMidDown;
        //Vector3D tsltVector;
        private void Mouse_Wheel(object sender, MouseWheelEventArgs e)
        {
            PCamera.Position = new Point3D(PCamera.Position.X, PCamera.Position.Y, PCamera.Position.Z - e.Delta / 100D);
        }

        private void Mouse_Up(object sender, MouseButtonEventArgs e)
        {
            mMidDown = false;
            mDown = false;
        }

        private void Mouse_Down(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed && e.MiddleButton != MouseButtonState.Pressed) return;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                mDown = true;
                _previousPosition3D = ProjectToTrackball(ActualWidth,ActualHeight, e.GetPosition(this));
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                mMidDown = true;
            }
            Point pos = Mouse.GetPosition(myDisplay);
            mLastPos = new Point(pos.X - myDisplay.ActualWidth / 2, myDisplay.ActualHeight / 2 - pos.Y);
        }

        private void Mouse_Move(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mMidDown)
            {
                Point pos = Mouse.GetPosition(myDisplay);
                Point actualPos = new Point(pos.X - myDisplay.ActualWidth / 2, myDisplay.ActualHeight / 2 - pos.Y);
                double dx = actualPos.X - mLastPos.X, dy = actualPos.Y - mLastPos.Y;

                PCamera.Position += (PCamera.UpDirection * dy/500);
                PCamera.Position += (Vector3D.CrossProduct(PCamera.LookDirection, PCamera.UpDirection) * dx/500);
            }
            if (mDown)
            {
                Track(e.GetPosition(this));
                PCamera.Transform = _transform;
                myLight.Transform = _transform;
            }
        }
        private void Mouse_Leave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            mMidDown = false;
            mDown = false;
        }
        //Adapted from:
        //https://3dtools.codeplex.com/SourceControl/latest#3DTools/3DTools/TrackballDecorator.cs
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
        private void Track(Point currentPosition)
        {
            Vector3D currentPosition3D = ProjectToTrackball(
                ActualWidth, ActualHeight, currentPosition);

            Vector3D axis = Vector3D.CrossProduct(_previousPosition3D, currentPosition3D);
            double angle = Vector3D.AngleBetween(_previousPosition3D, currentPosition3D);

            // quaterion will throw if this happens - sometimes we can get 3D positions that
            // are very similar, so we avoid the throw by doing this check and just ignoring
            // the event 
            if (axis.Length == 0) return;

            Quaternion delta = new Quaternion(axis, -angle);

            // Get the current orientantion from the RotateTransform3D
            AxisAngleRotation3D r = _rotation;
            Quaternion q = new Quaternion(_rotation.Axis, _rotation.Angle);

            // Compose the delta with the previous orientation
            q *= delta;

            // Write the new orientation back to the Rotation3D
            _rotation.Axis = q.Axis;
            _rotation.Angle = q.Angle;

            _previousPosition3D = currentPosition3D;
        }
        private void Align_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double rmsd = backend.ICPAlignment();
                redraw();
                Output.Text += "RMSD = " + Math.Round(Convert.ToDecimal(rmsd), 3) + "\r\n";
                Output.Text += "Alignment Complete.\r\n";
            }
            catch (Exception ex)
            {
                Output.Text += ex.Message + "\r\n";
            }
        }

        private void FullAtomToggle_Checked(object sender, RoutedEventArgs e)
        {
            _fullAtom = true;
            redraw();
        }

        private void FullAtomToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            _fullAtom = false;
            redraw();
        }

    }
}
