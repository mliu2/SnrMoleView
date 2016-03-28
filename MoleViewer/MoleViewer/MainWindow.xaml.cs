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
            GeometryModel3D myGeometryModel = new GeometryModel3D();
            ModelVisual3D myModelVisual3D = new ModelVisual3D();
            
            // Define the lights cast in the scene. Without light, the 3D object cannot 
            // be seen. Note: to illuminate an object from additional directions, create 
            // additional lights.
            DirectionalLight myDirectionalLight = new DirectionalLight();
            myDirectionalLight.Color = Colors.White;
            myDirectionalLight.Direction = new Vector3D(-0.61, -0.5, -0.61);

            myModel3DGroup.Children.Add(myDirectionalLight);

            // The geometry specifes the shape of the 3D plane. In this sample, a flat sheet 
            // is created.
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            // Create a collection of normal vectors for the MeshGeometry3D.
            Vector3DCollection myNormalCollection = new Vector3DCollection();
            myNormalCollection.Add(new Vector3D(0,0,1));
            myNormalCollection.Add(new Vector3D(0,0,1));
            myNormalCollection.Add(new Vector3D(0,0,1));
            myNormalCollection.Add(new Vector3D(0,0,1));
            myNormalCollection.Add(new Vector3D(0,0,1));
            myNormalCollection.Add(new Vector3D(0,0,1));
            myMeshGeometry3D.Normals = myNormalCollection;

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();
            myPositionCollection.Add(new Point3D(-0.5, -0.5, 0.5));
            myPositionCollection.Add(new Point3D(0.5, -0.5, 0.5));
            myPositionCollection.Add(new Point3D(0.5, 0.5, 0.5));
            myPositionCollection.Add(new Point3D(0.5, 0.5, 0.5));
            myPositionCollection.Add(new Point3D(-0.5, 0.5, 0.5));
            myPositionCollection.Add(new Point3D(-0.5, -0.5, 0.5));
            myMeshGeometry3D.Positions = myPositionCollection;

            // Create a collection of texture coordinates for the MeshGeometry3D.
            PointCollection myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myMeshGeometry3D.TextureCoordinates = myTextureCoordinatesCollection;

            // Create a collection of triangle indices for the MeshGeometry3D.
            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(5);
            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            // The material specifies the material applied to the 3D object. In this sample a  
            // linear gradient covers the surface of the 3D object.

            // Create a horizontal linear gradient with four stops.   
            LinearGradientBrush myHorizontalGradient = new LinearGradientBrush();
            myHorizontalGradient.StartPoint = new Point(0, 0.5);
            myHorizontalGradient.EndPoint = new Point(1, 0.5);
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Blue, 0.75));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.LimeGreen, 1.0));

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(myHorizontalGradient);
            myGeometryModel.Material = myMaterial;
            myGeometryModel.BackMaterial = myMaterial;

            // Apply a transform to the object. In this sample, a rotation transform is applied,  
            // rendering the 3D object rotated.
            RotateTransform3D myRotateTransform3D = new RotateTransform3D();
            AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
            myAxisAngleRotation3d.Axis = new Vector3D(0,3,0);
            myAxisAngleRotation3d.Angle = 40;
            myRotateTransform3D.Rotation = myAxisAngleRotation3d;
            myGeometryModel.Transform = myRotateTransform3D;

            // Add the geometry model to the model group.
            myModel3DGroup.Children.Add(myGeometryModel);

            // Add the group of models to the ModelVisual3d.
            myModelVisual3D.Content = myModel3DGroup;

            // 
            myDisplay.Children.Add(myModelVisual3D);

            myDisplay.Height = display.Height;
            myDisplay.Width = display.Width;
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
        private double TotalDx, TotalDy;
        private bool mMidDown;
        private void Mouse_Wheel(object sender, MouseWheelEventArgs e)
        {
            PCamera.Position = new Point3D(PCamera.Position.X, PCamera.Position.Y, PCamera.Position.Z - e.Delta / 1000D);
        }

        private void Mouse_Up(object sender, MouseButtonEventArgs e)
        {
            mMidDown = false;
            mDown = false;
        }

        private void Mouse_Down(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                mDown = true;
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                mMidDown = true;
            }
            else
            {
                return;
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
                PCamera.Position = new Point3D(PCamera.Position.X - dx / (myDisplay.ActualWidth*5), PCamera.Position.Y - dy / (myDisplay.ActualHeight*5), PCamera.Position.Z);
            }
            if (mDown)
            {
                Point pos = Mouse.GetPosition(myDisplay);
                Point actualPos = new Point(pos.X - myDisplay.ActualWidth / 2, myDisplay.ActualHeight / 2 - pos.Y);
                double dx = actualPos.X - mLastPos.X, dy = actualPos.Y - mLastPos.Y;

                TotalDx += dx; TotalDy += dy;

                double theta = TotalDx / 3;
                double phi = TotalDy / 3;
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
    }
}
