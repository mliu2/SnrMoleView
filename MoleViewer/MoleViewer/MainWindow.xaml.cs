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

        public MainWindow()
        {
            InitializeComponent();
            backend = new Backend();
            Shape3d();
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
        private void CenterProt(Protein prtn)
        {
            prtn.Translate(-prtn.CenterX(), -prtn.CenterY(), -prtn.CenterZ());
        }
        private void MakeProt1()
        {
            CenterProt(backend.Protein1);
            foreach (Atom atom in backend.Protein1.prot)
            {
                if (atom.CA == true)
                {
                    MakeAtom(atom, Backend.C_RAD, Colors.CornflowerBlue);
                }
                else if (atom.Ele == "C")
                {
                    MakeAtom(atom, Backend.C_RAD, Colors.CornflowerBlue);
                }
                else if (atom.Ele == "N")
                {
                    MakeAtom(atom, Backend.N_RAD, Colors.PaleVioletRed);
                }
                else if (atom.Ele == "O")
                {
                    MakeAtom(atom, Backend.O_RAD, Colors.Ivory);
                }
            }
        }
        private void MakeProt2()
        {
            CenterProt(backend.Protein2);
            foreach (Atom atom in backend.Protein2.prot)
            {
                if (atom.CA == true)
                {
                    MakeAtom(atom, Backend.C_RAD, Colors.Crimson);
                }
                /*else if (atom.Ele == "C")
                {
                    MakeAtom(atom, Backend.C_RAD, Colors.Crimson);
                }
                else if (atom.Ele == "N")
                {
                    MakeAtom(atom, Backend.N_RAD, Colors.PaleVioletRed);
                }
                else if (atom.Ele == "O")
                {
                    MakeAtom(atom, Backend.O_RAD, Colors.Ivory);
                }*/
            }
        }
        private void redraw()
        {
            myModel3DGroup.Children.Clear();
            MakeProt1();
            MakeProt2();
        }
        private void Focus()
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
                    Output.Text += openFileDialog1.FileName + " : " + num + " atoms loaded \r\n";
                    Focus();
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
                    Output.Text += openFileDialog1.FileName + " : " + num + " atoms loaded \r\n";
                    Focus();
                    redraw();
                }
                catch (Exception ex)
                {
                    Output.Text += ex.Message + "\r\n";
                }
            }
        }

        //HANDLERS FOR MOUSE CAMERA CONTROL
        //Adapted from:
        //https://3dtools.codeplex.com/SourceControl/latest#3DTools/3DTools/TrackballDecorator.cs

        private bool mDown;
        private Point mLastPos;
        private double TransTotalDx, TransTotalDy;
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

                TransTotalDx += dx/5000; TransTotalDy += dy/5000;
                PCamera.Position += (PCamera.UpDirection * dy/500);
                PCamera.Position += (Vector3D.CrossProduct(PCamera.LookDirection, PCamera.UpDirection) * dx/500);
                /*Vector3D tempY = (PCamera.UpDirection * dy/500);
                Vector3D tempX = (Vector3D.CrossProduct(PCamera.LookDirection, PCamera.UpDirection) * dx/500);*/
                /*_translate.OffsetX += tempY.X + tempX.X;
                _translate.OffsetY += tempY.Y + tempX.Y;
                _translate.OffsetZ += tempY.Z + tempX.Z;*/
                /*Transform3DGroup group = new Transform3DGroup();
                group.Children.Clear();
                group.Children.Add(_translate);
                myModel3DGroup.Transform = group;*/
                //PCamera.Transform = new TranslateTransform3D(TransTotalDx, TransTotalDy, 0);
                /*Transform3DGroup group = new Transform3DGroup();
                group.Children.Clear();
                group.Children.Add(_transform);
                group.Children.Add(_translate);
                PCamera.Transform = group;*/
            }
            if (mDown)
            {
                Track(e.GetPosition(this));
                PCamera.Transform = _transform;
                myLight.Transform = _transform;

                
                //Track2(e.GetPosition(this));
                //myModel3DGroup.Transform = _transform;
                /*Point pos = Mouse.GetPosition(myDisplay);
                Point actualPos = new Point(pos.X - myDisplay.ActualWidth / 2, myDisplay.ActualHeight / 2 - pos.Y);
                double dx = actualPos.X - mLastPos.X, dy = actualPos.Y - mLastPos.Y;

                RotTotalDx += dx; RotTotalDy += dy;

                double theta = RotTotalDx / 3;
                double phi = RotTotalDy / 3;
                Vector3D thetaAxis = new Vector3D(0, 1, 0);
                Vector3D phiAxis = new Vector3D(-1, 0, 0);

                Transform3DGroup group = new Transform3DGroup();
                group.Children.Clear();
                QuaternionRotation3D r;
                r = new QuaternionRotation3D(new Quaternion(thetaAxis, theta));
                group.Children.Add(new RotateTransform3D(r));
                r = new QuaternionRotation3D(new Quaternion(phiAxis, phi));
                group.Children.Add(new RotateTransform3D(r));
              
                myModel3DGroup.Transform = group;

                mLastPos = actualPos;*/
            }
        }
        private void Mouse_Leave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            mMidDown = false;
            mDown = false;
        }
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
        private void Track2(Point currentPosition)
        {
            Vector3D currentPosition3D = ProjectToTrackball(
                ActualWidth, ActualHeight, currentPosition);

            double dx = currentPosition3D.X - _previousPosition3D.X;
            double dy = currentPosition3D.Y - _previousPosition3D.Y;

            _rotationY.Angle += dy * 90;

            //Quaternion delta = new Quaternion(new Vector3D(0, Math.Cos(_rotationY.Angle * Math.PI / 180), Math.Sin(_rotationY.Angle * Math.PI / 180)), dx * 90);
            //Quaternion q = new Quaternion(_rotationX.Axis, _rotationX.Angle);
            //q *= delta;
            //_rotationX.Axis = q.Axis;
            //_rotationX.Angle = q.Angle;

            _rotationY.Angle += Math.Cos(_rotationAboutZ.Angle * Math.PI / 180) * dx * 90;
            _rotationX.Angle += Math.Cos(_rotationY.Angle * Math.PI / 180) * dx * 90;
            _rotationAboutZ.Angle += Math.Sin(_rotationY.Angle * Math.PI / 180) * dx * 90;

            _previousPosition3D = currentPosition3D;
        }

 
    }
}
