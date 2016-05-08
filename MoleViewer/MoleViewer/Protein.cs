using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;


namespace MoleViewer
{
    class Protein
    {
        /*
         * Member variables
         * */
        private List<Atom> m_prot;
        /// <summary>
        /// Default constructor for Protein class
        /// </summary>
        public Protein()
        {
            m_prot = new List<Atom>();
        }
        /// <summary>
        /// Accessor for list of atoms
        /// </summary>
        public List<Atom> Atoms
        {
            get
            {
                return m_prot;
            }
        }
        /// <summary>
        /// Accessor for number of atoms in protein
        /// </summary>
        public int Count
        {
            get
            {
                return m_prot.Count;
            }
        }
        /// <summary>
        /// Accessor for the number of alpha carbons in the protein
        /// </summary>
        public int CACount
        {
            get
            {
                int total = 0;
                foreach (Atom atom in m_prot)
                {
                    if (atom.CA) total++;
                }
                return total;
            }
        }
        /// <summary>
        /// Gets the x coordinate of the center of the protein.
        /// It will get the center by finding the average of the largest and smallest x value.
        /// </summary>
        /// <returns>Returns the x coordinate of the center of the protein</returns>
        public double CenterX()
        {
            double retval = 0;
            //only execute if prot list of atoms is non empty
            if (m_prot.Any())
            {
                double max = m_prot[0].X;
                double min = m_prot[0].X;
                for (int i = 1; i < Count; i++)
                {
                    if (m_prot[i].X > max) max = m_prot[i].X;
                    if (m_prot[i].X < min) min = m_prot[i].X;
                }
                retval = (max + min) / 2;
            }
            return retval;
        }
        /// <summary>
        /// Gets the y coordinate of the center of the protein.
        /// It will get the center by finding the average of the largest and smallest y value.
        /// </summary>
        /// <returns>Returns the y coordinate of the center of the protein</returns>
        public double CenterY()
        {
            double retval = 0;
            //only execute if prot list of atoms is non empty
            if (m_prot.Any())
            {
                double max = m_prot[0].Y;
                double min = m_prot[0].Y;
                for (int i = 1; i < Count; i++)
                {
                    if (m_prot[i].Y > max) max = m_prot[i].Y;
                    if (m_prot[i].Y < min) min = m_prot[i].Y;
                }
                retval = (max + min) / 2;
            }
            return retval;
        }
        /// <summary>
        /// Gets the z coordinate of the center of the protein.
        /// It will get the center by finding the average of the largest and smallest z value.
        /// </summary>
        /// <returns>Returns the z coordinate of the center of the protein</returns>
        public double CenterZ()
        {
            double retval = 0;
            //only execute if prot list of atoms is non empty
            if (m_prot.Any())
            {
                double max = m_prot[0].Z;
                double min = m_prot[0].Z;
                for (int i = 1; i < Count; i++)
                {
                    if (m_prot[i].Z > max) max = m_prot[i].Z;
                    if (m_prot[i].Z < min) min = m_prot[i].Z;
                }
                retval = (max + min) / 2;
            }
            return retval;
        }
        /// <summary>
        /// Translates the protein by the input values
        /// </summary>
        /// <param name="a_x">Amount to translate protein in the x axis in angstroms</param>
        /// <param name="a_y">Amount to translate protein in the y axis in angstroms</param>
        /// <param name="a_z">Amount to translate protein in the z axis in angstroms</param>
        public void Translate(double a_x, double a_y, double a_z)
        {
            foreach (Atom atom in m_prot)
            {
                atom.X = atom.X + a_x;
                atom.Y = atom.Y + a_y;
                atom.Z = atom.Z + a_z;
            }
        }
        /// <summary>
        /// Creates a horizontal matrix of all of the atoms' coordinates and returns it.
        /// This matrix is used for the matrix multiplication to transform the protein.
        /// </summary>
        /// <returns>Returns full atom matrix, of 3 rows, Count number of columns</returns>
        public double[,] ToMatrix()
        {
            double[,] AtomMatrix = new double[3, Count];
            for (int i = 0; i < Count; i++)
            {
                AtomMatrix[0, i] = m_prot[i].X;
                AtomMatrix[1, i] = m_prot[i].Y;
                AtomMatrix[2, i] = m_prot[i].Z;
            }
            return AtomMatrix;
        }
        /// <summary>
        /// Creates a vertical matrix of alpha carbon coordinates and returns it.
        /// This matrix is used for the LIBICP input to determine the transform matrix.
        /// </summary>
        /// <returns>Returns alpha carbon matrix of CACount number of rows, 3 columns</returns>
        public double[,] CAMatrix()
        {
            double[,] AtomMatrix = new double[CACount, 3];
            int i = 0;
            for (int atomnum = 0; atomnum < Count; atomnum++)
            {
                if (m_prot[atomnum].CA)
                {
                    AtomMatrix[i, 0] = m_prot[atomnum].X;
                    AtomMatrix[i, 1] = m_prot[atomnum].Y;
                    AtomMatrix[i, 2] = m_prot[atomnum].Z;
                    i++;
                }
            }
            return AtomMatrix;
        }
        /// <summary>
        /// Takes in a horizontal matrix and repositions each atom to its corresponding set of coordinates in the matrix.
        /// </summary>
        /// <param name="input">Horizontal matrix produced by multiplying the rotation matrix with the matrix from ToMatrix()</param>
        public void FromMatrix(Matrix<double> input)
        {
            for (int i = 0; i < Count; i++)
            {
                m_prot[i].X = input[0, i];
                m_prot[i].Y = input[1, i];
                m_prot[i].Z = input[2, i];
            }
        }
        /// <summary>
        /// Opens a file from a filpath and reads in data. For each line, it checks if it is an atom record.
        /// If it is an atom record, it will parse it by splitting the string according to the PDBx/mmCIF file format.
        /// If it does not work, it will throw an exception stating that the file cannot be parsed.
        /// </summary>
        /// <param name="file">String for the path to the file to be parsed</param>
        /// <returns>Int for the number of atoms that were parsed</returns>
        public int ParseFile(string file)
        {
            //clear any previous atom data
            m_prot.Clear();
            string path = file;
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                String line;
                try
                {
                    //read file line by line, checking for correct format
                    while ((line = sr.ReadLine()) != null)
                    {
                        //split the elements of the line
                        String[] elements = line.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
                        if (elements.Length > 11 && elements[0] == "ATOM")
                        {
                            //parse each element
                            char chain = elements[6].ToCharArray()[0];
                            int res;
                            if (!Int32.TryParse(elements[8], out res))
                            {
                                throw new FormatException("Unable to parse residue number");
                            }
                            string elem = elements[3];
                            if (elem != "CA")
                            {
                                elem = elements[2];
                            }
                            double x = Convert.ToDouble(elements[10]);
                            double y = Convert.ToDouble(elements[11]);
                            double z = Convert.ToDouble(elements[12]);
                            //create new atom object
                            m_prot.Add(new Atom(elem, elements[5], chain, res, x, y, z));

                        }
                    }
                }
                catch (Exception ex){
                    throw (new Exception("ERROR: File not recognized: " + ex.Message));
                }
                sr.Close();
            }
            return m_prot.Count();
        }
        
    }
}
