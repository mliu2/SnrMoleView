using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;

namespace MoleViewer
{
    

    class Backend
    {
        public const double N_RAD = .75;
        public const double O_RAD = .73;
        public const double C_RAD = .77;
        public const double H_RAD = .38;

        private Protein prot1;
        private Protein prot2;

        
        //CODE FOR SPHERES FROM ftp://ftp.oreilly.com/pub/examples/9780735623941/3DProgWin/Chapter%206/BeachBallSphere/BeachBallSphere.cs
        public MeshGeometry3D GenerateSphere(Point3D center, double radius, int slices, int stacks)
        {
            // Create the MeshGeometry3D.
            MeshGeometry3D mesh = new MeshGeometry3D();

            // Fill the Position, Normals, and TextureCoordinates collections.
            for (int stack = 0; stack <= stacks; stack++)
            {
                double phi = Math.PI / 2 - stack * Math.PI / stacks;
                double y = radius * Math.Sin(phi);
                double scale = -radius * Math.Cos(phi);

                for (int slice = 0; slice <= slices; slice++)
                {
                    double theta = slice * 2 * Math.PI / slices;
                    double x = scale * Math.Sin(theta);
                    double z = scale * Math.Cos(theta);

                    Vector3D normal = new Vector3D(x, y, z);
                    mesh.Normals.Add(normal);
                    mesh.Positions.Add(normal + center);
                    mesh.TextureCoordinates.Add(
                            new Point((double)slice / slices,
                                      (double)stack / stacks));
                }
            }

            // Fill the TriangleIndices collection.
            for (int stack = 0; stack < stacks; stack++)
                for (int slice = 0; slice < slices; slice++)
                {
                    int n = slices + 1; // Keep the line length down.

                    if (stack != 0)
                    {
                        mesh.TriangleIndices.Add((stack + 0) * n + slice);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice);
                        mesh.TriangleIndices.Add((stack + 0) * n + slice + 1);
                    }
                    if (stack != stacks - 1)
                    {
                        mesh.TriangleIndices.Add((stack + 0) * n + slice + 1);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice + 1);
                    }
                }
            return mesh;
        }
        //generate a sphere


        public Backend()
        {
            prot1 = new Protein();
            prot2 = new Protein();
        }
        public int Prot1Parse(string file)
        {
            return prot1.ParseFile(file);
        }
        public int Prot2Parse(string file)
        {
            return prot2.ParseFile(file);
        }
        public Protein Protein1
        {
            get
            {
                return prot1;
            }
        }
        public Protein Protein2
        {
            get
            {
                return prot2;
            }
        }
    }
}
