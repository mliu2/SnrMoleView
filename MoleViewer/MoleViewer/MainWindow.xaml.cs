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
        private void MakeProt1()
        {
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
        private void Focus(Protein prot)
        {
            PCamera.Position = new Point3D(prot.CenterX(), prot.CenterY(), prot.CenterZ());
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
                    Focus(backend.Protein1);
                    MakeProt1();
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
                    Focus(backend.Protein2);
                }
                catch (Exception ex)
                {
                    Output.Text += ex.Message + "\r\n";
                }
            }
        }

        //HANDLERS FOR MOUSE CAMERA CONTROL
        //Adapted from:
        //http://www.codeproject.com/Articles/23332/WPF-D-Primer

        private bool mDown;
        private Point mLastPos;
        private double RotTotalDx, RotTotalDy, TransTotalDx, TransTotalDy;
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

                TransTotalDx += dx/500; TransTotalDy += dy/500;
                PCamera.Transform = new TranslateTransform3D(TransTotalDx, TransTotalDy, 0);
            }
            if (mDown)
            {
                Point pos = Mouse.GetPosition(myDisplay);
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

                mLastPos = actualPos;
            }
        }

        private void Mouse_Leave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            mMidDown = false;
            mDown = false;
        }


 
    }
}
