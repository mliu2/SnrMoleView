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
        

        private Protein m_prot1;
        private Protein m_prot2;
        //holds distance for paired alpha carbons and records the residue number
        private List<ResNumPair> m_CAPair;
        //holds the pair with the greatest distance
        private ResNumPair m_maxDistPair;
        /// <summary>
        /// Constructor for the backend object. This object holds 2 proteins and contains functions for protein-protein alignment.
        /// Contains functions that allow for the display to retrieve information about the protein.
        /// </summary>
        public Backend()
        {
            m_prot1 = new Protein();
            m_prot2 = new Protein();
            m_CAPair = new List<ResNumPair>();
        }
        /// <summary>
        /// Translate the center of the protein to the center of the axis. 
        /// </summary>
        /// <param name="a_prtn">Protein to be centered</param>
        private void CenterProt(Protein a_prtn)
        {
            a_prtn.Translate(-a_prtn.CenterX(), -a_prtn.CenterY(), -a_prtn.CenterZ());
        }
        
        /// <summary>
        /// Generates a spherical geometry mesh. It divides the sphere into a number of slices and stacks for which triangles can be generated.
        /// Written by Charles Petzold, 2007
        /// FROM ftp://ftp.oreilly.com/pub/examples/9780735623941/3DProgWin/Chapter%206/BeachBallSphere/BeachBallSphere.cs
        /// </summary>
        /// <param name="a_center">3D point of where the center of the sphere is</param>
        /// <param name="a_radius">Radius of the sphere to be generated</param>
        /// <param name="a_slices">Number of horizontal slices that the sphere will have. Increasing this will increase the smoothness of the sphere.</param>
        /// <param name="a_stacks">Number of vertical slices that the sphere will have. Increasing this will increase the smoothness of the sphere.</param>
        /// <returns>The geometric mesh of the sphere.</returns>
        public MeshGeometry3D GenerateSphere(Point3D a_center, double a_radius, int a_slices, int a_stacks)
        {
            // Create the MeshGeometry3D.
            MeshGeometry3D mesh = new MeshGeometry3D();

            // Fill the Position, Normals, and TextureCoordinates collections.
            for (int stack = 0; stack <= a_stacks; stack++)
            {
                double phi = Math.PI / 2 - stack * Math.PI / a_stacks;
                double y = a_radius * Math.Sin(phi);
                double scale = -a_radius * Math.Cos(phi);

                for (int slice = 0; slice <= a_slices; slice++)
                {
                    double theta = slice * 2 * Math.PI / a_slices;
                    double x = scale * Math.Sin(theta);
                    double z = scale * Math.Cos(theta);

                    Vector3D normal = new Vector3D(x, y, z);
                    mesh.Normals.Add(normal);
                    mesh.Positions.Add(normal + a_center);
                    mesh.TextureCoordinates.Add(
                            new Point((double)slice / a_slices,
                                      (double)stack / a_stacks));
                }
            }

            // Fill the TriangleIndices collection.
            for (int stack = 0; stack < a_stacks; stack++)
                for (int slice = 0; slice < a_slices; slice++)
                {
                    int n = a_slices + 1; // Keep the line length down.

                    if (stack != 0)
                    {
                        mesh.TriangleIndices.Add((stack + 0) * n + slice);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice);
                        mesh.TriangleIndices.Add((stack + 0) * n + slice + 1);
                    }
                    if (stack != a_stacks - 1)
                    {
                        mesh.TriangleIndices.Add((stack + 0) * n + slice + 1);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice + 1);
                    }
                }
            return mesh;
        }

        /// <summary>
        /// Parses a file into a protein object as m_prot1.
        /// The protein is then centered, and the list for calculating RMSD is cleared, as a new protein was loaded in.
        /// </summary>
        /// <param name="a_file">Path to the file to be parsed.</param>
        /// <returns>Number of atoms that were parsed.</returns>
        public int Prot1Parse(string a_file)
        {
            int num = m_prot1.ParseFile(a_file);
            CenterProt(Protein1);
            RMSDDictClear();
            return num;
        }
        /// <summary>
        /// Parses a file into a protein object as m_prot2.
        /// The protein is then centered, and the list for calculating RMSD is cleared, as a new protein was loaded in.
        /// </summary>
        /// <param name="a_file">Path to the file to be parsed.</param>
        /// <returns>Number of atoms that were parsed.</returns>
        public int Prot2Parse(string a_file)
        {
            int num = m_prot2.ParseFile(a_file);
            CenterProt(Protein2);
            RMSDDictClear();
            return num;
        }
        /// <summary>
        /// Accessor to get the ResNumPair with the greatest distance.
        /// </summary>
        public ResNumPair MaxDistResidue
        {
            get
            {
                return m_maxDistPair;
            }
        }
        /// <summary>
        /// Accessor for m_prot1
        /// </summary>
        public Protein Protein1
        {
            get
            {
                return m_prot1;
            }
        }
        /// <summary>
        /// Accessor for m_prot2.
        /// </summary>
        public Protein Protein2
        {
            get
            {
                return m_prot2;
            }
        }
        /// <summary>
        /// Performs ICP Alignment using LIBICP 1.4.7.
        /// Checks if the proteins fit the minimum requirements of the alignment. 
        /// Throws if it does not, or if there are less than two proteins loaded.
        /// Extract the alpha carbon matrix from the proteins. Sets up the translation and rotation matrices.
        /// Fits the two matrices using ICP. The rotation matrix is applied to a full atom matrix of Protein2.
        /// Protein2 is then translated. Calls RMSD() to calculate the RMSD between the two proteins.
        /// </summary>
        /// <returns>RMSD value between the two proteins.</returns>
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
            double[,] prot1Mat = m_prot1.CAMatrix();
            double[,] prot2Mat = m_prot2.CAMatrix();
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
            double[,] prot2FullMat = m_prot2.ToMatrix();
            //create objects of Matrix type to perfrom matrix multiplication
            Matrix<double> prot2MatObj = DenseMatrix.OfArray(prot2FullMat);
            Matrix<double> RMatObj = DenseMatrix.OfArray(RMat);
            /*To get the Final dataset B, given dataset A
             * B = RMat * A + TMat;
             * Thus Rotation is applied first
             */
            prot2MatObj = RMatObj * prot2MatObj;
            //sets the protein coordinates to the matrix
            m_prot2.FromMatrix(prot2MatObj);
            //translate
            m_prot2.Translate(TMat[0], TMat[1], TMat[2]);
            return RMSD();
        }
        /// <summary>
        /// Clears the list associated with finding RMSD. Resets the value of the ResNumPair with the greatest distance.
        /// </summary>
        private void RMSDDictClear()
        {
            m_CAPair.Clear();
            m_maxDistPair = new ResNumPair();
        }
        /// <summary>
        /// For an alpha carbon on protein 1, it will find the nearest alpha carbon in protein 2.
        /// It then creates a ResNumPair object to be added to the m_CAPair list for RMSD calculations.
        /// </summary>
        /// <param name="a_CAIndex">Index in the list of proteins for the alpha carbon on protein 1</param>
        /// <returns>Distance between the two alpha carbons</returns>
        private double Nearest(int a_CAIndex)
        {
            double NearDist = double.PositiveInfinity;
            Atom pairedAtom = new Atom();
            if (a_CAIndex < Protein1.Count)
            {
                double templateX = Protein1.Atoms[a_CAIndex].X;
                double templateY = Protein1.Atoms[a_CAIndex].Y;
                double templateZ = Protein1.Atoms[a_CAIndex].Z;

                foreach (Atom atom in Protein2.Atoms)
                {
                    if (atom.CA)
                    {
                        //distance in 3d space
                        double tempDist = Math.Sqrt(Math.Pow(atom.X - templateX, 2) + Math.Pow(atom.Y - templateY, 2) + Math.Pow(atom.Z - templateZ, 2));
                        if (tempDist < NearDist)
                        {
                            NearDist = tempDist;
                            pairedAtom = atom;
                        }
                    }   
                }
            }
            else
            {
                throw (new Exception("Distance calculation error: Template CAIndex out of range."));
            }
            m_CAPair.Add(new ResNumPair(Protein1.Atoms[a_CAIndex].Residue_Num, pairedAtom.Residue_Num, Protein1.Atoms[a_CAIndex].Chain, pairedAtom.Chain,NearDist));
            return NearDist;
        }
        /// <summary>
        /// Calculates the Root-Mean-Squared Deviation(RMSD) between the two proteins using alpha carbons.
        /// The RMSD is the square root of the mean of the distance between a set of points.
        /// The set of points it examines comprise the alpha carbon backbone of the proteins. This exludes the side atoms.
        /// It calls Nearest(int) to populate the list that it uses to determine RMSD and sums the squared of the returned distance.
        /// It divides that by the number of alpha carbons to get the mean, which is the then rooted to get the RMSD.
        /// Also marks the atom pair with the greatest distance.
        /// </summary>
        /// <returns>The RMSD between the two proteins' alpha carbon backbone.</returns>
        private double RMSD()
        {
            RMSDDictClear();
            int count = 0;
            double SumDist = 0;
            for(int i = 0; i < Protein1.Count; i++)
            {
                if (Protein1.Atoms[i].CA)
                {
                    double dist = Nearest(i);
                    SumDist += dist*dist;
                    count++;
                }
            }
            m_maxDistPair = m_CAPair.Max();
            return Math.Sqrt(SumDist/ count);
        }
    }
}
