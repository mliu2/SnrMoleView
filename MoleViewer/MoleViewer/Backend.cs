using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;

using icp_net;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;


namespace MoleViewer
{
    

    class Backend
    {
        public const double N_RAD = .75;
        public const double O_RAD = .73;
        public const double C_RAD = .77;
        public const double H_RAD = .38;

        private Protein _prot1;
        private Protein _prot2;
        //holds distance for CA and records the residue number
        private SortedDictionary<double, int> _CADist;
        //residue number with the largest distance from its pair
        private int _maxResNum = 0;
        public Backend()
        {
            _prot1 = new Protein();
            _prot2 = new Protein();
            _CADist = new SortedDictionary<double, int>();
        }
        private void CenterProt(Protein prtn)
        {
            prtn.Translate(-prtn.CenterX(), -prtn.CenterY(), -prtn.CenterZ());
        }
        
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

        public int Prot1Parse(string file)
        {
            int num = _prot1.ParseFile(file);
            CenterProt(Protein1);
            _maxResNum = 0;
            _CADist.Clear();
            return num;
        }
        public int Prot2Parse(string file)
        {
            int num = _prot2.ParseFile(file);
            CenterProt(Protein2);
            _maxResNum = 0;
            _CADist.Clear();
            return num;
        }
        public int MaxResidue
        {
            get
            {
                return _maxResNum;
            }
        }
        public Protein Protein1
        {
            get
            {
                return _prot1;
            }
        }
        public Protein Protein2
        {
            get
            {
                return _prot2;
            }
        }
        public double ICPAlignment()
        {
            if (Protein1.Count == 0)
            {
                throw (new Exception("Alignment Error: Protein 1 not found."));
            }
            if (Protein2.Count == 0)
            {
                throw (new Exception("Alignment Error: Protein 2 not found."));
            }
            //LIBICP doesnt work with less than 5 points
            if (Protein1.CACount < 5 || Protein2.CACount < 5)
            {
                throw (new Exception("Alignment Error: Not enough atoms for alignment."));
            }
            //aligns by CA backbone
            double[,] prot1Mat = _prot1.CAMatrix();
            double[,] prot2Mat = _prot2.CAMatrix();
            icp_net.ManagedICP align = new icp_net.ManagedICP(prot1Mat, Protein1.CACount, 3);

            //set up return Rotation matrix
            double[,] RMat = new double[3, 3];
            RMat[0, 0] = 1.0;
            RMat[1, 1] = 1.0;
            RMat[2, 2] = 1.0;
            //set up return Translation matrix
            double[] TMat = new double[3];

            //last parameter is inlier distance, if <= 0, uses all points
            align.fit(prot2Mat, Protein2.CACount, RMat, TMat, -1);

            //get full atom matrix to transform
            // LIBICP actually takes in a n x 3 matrix, but we need a 3 x n matrix for the math, so ToMatrix() gives a full atom matrix in that orientation
            double[,] prot2FullMat = _prot2.ToMatrix();
            //create objects of Matrix type to perfrom matrix multiplication
            Matrix<double> prot2MatObj = DenseMatrix.OfArray(prot2FullMat);
            Matrix<double> RMatObj = DenseMatrix.OfArray(RMat);
            /*To get the Final dataset B, given dataset A
             * B = RMat * A + TMat;
             * Thus Rotation is applied first
             */
            prot2MatObj = RMatObj * prot2MatObj;
            //sets the protein coordinates to the matrix
            _prot2.FromMatrix(prot2MatObj);
            //translate
            _prot2.Translate(TMat[0], TMat[1], TMat[2]);
            return RMSD();
        }
        //Finds the nearest CA carbon in protein 2, should input index of a CA carbon on protein 1
        private double Nearest(int CAIndex)
        {
            double NearDist = double.PositiveInfinity;
            if (CAIndex < _prot1.Count)
            {
                double templateX = _prot1.Atoms[CAIndex].X;
                double templateY = _prot1.Atoms[CAIndex].Y;
                double templateZ = _prot1.Atoms[CAIndex].Z;
                
                foreach (Atom atom in _prot2.Atoms)
                {
                    if (atom.CA)
                    {
                        //distance in 3d space
                        double tempDist = Math.Sqrt(Math.Pow(atom.X - templateX, 2) + Math.Pow(atom.Y - templateY, 2) + Math.Pow(atom.Z - templateZ, 2));
                        if (tempDist < NearDist)
                        {
                            NearDist = tempDist;
                        }
                    }   
                }
            }
            else
            {
                throw (new Exception("Distance calculation error: Template CAIndex out of range."));
            }
            return NearDist;
        }
        //determines the root mean square deviation
        //which the square root of the mean of the distance between a set of points
        private double RMSD()
        {
            _CADist.Clear();
            int count = 0;
            double SumDist = 0;
            for(int i = 0; i < _prot1.Count; i++)
            {
                if (_prot1.Atoms[i].CA)
                {
                    double dist = Nearest(i);
                    _CADist.Add(dist, _prot1.Atoms[i].Residue_Num);
                    SumDist += dist;
                    count++;
                }
            }
            _maxResNum = _CADist.Last().Value;
            return Math.Sqrt(SumDist / count);
        }
    }
}
