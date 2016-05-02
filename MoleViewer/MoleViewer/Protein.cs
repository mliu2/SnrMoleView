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
         * Member Variables 
         * */
        private List<Atom> _prot;
        public Protein()
        {
            _prot = new List<Atom>();
        }
        public List<Atom> Atoms
        {
            get
            {
                return _prot;
            }
        }
        public int Count
        {
            get
            {
                return _prot.Count;
            }
        }
        public int CACount
        {
            get
            {
                int total = 0;
                foreach (Atom atom in _prot)
                {
                    if (atom.CA) total++;
                }
                return total;
            }
        }
        public double CenterX()
        {
            double retval = 0;
            //only execute if prot list of atoms is non empty
            if (_prot.Any())
            {
                double max = _prot[0].X;
                double min = _prot[0].X;
                for (int i = 1; i < Count; i++)
                {
                    if (_prot[i].X > max) max = _prot[i].X;
                    if (_prot[i].X < min) min = _prot[i].X;
                }
                retval = (max + min) / 2;
            }
            return retval;
        }
        public double CenterY()
        {
            double retval = 0;
            //only execute if prot list of atoms is non empty
            if (_prot.Any())
            {
                double max = _prot[0].Y;
                double min = _prot[0].Y;
                for (int i = 1; i < Count; i++)
                {
                    if (_prot[i].Y > max) max = _prot[i].Y;
                    if (_prot[i].Y < min) min = _prot[i].Y;
                }
                retval = (max + min) / 2;
            }
            return retval;
        }
        public double CenterZ()
        {
            double retval = 0;
            //only execute if prot list of atoms is non empty
            if (_prot.Any())
            {
                double max = _prot[0].Z;
                double min = _prot[0].Z;
                for (int i = 1; i < Count; i++)
                {
                    if (_prot[i].Z > max) max = _prot[i].Z;
                    if (_prot[i].Z < min) min = _prot[i].Z;
                }
                retval = (max + min) / 2;
            }
            return retval;
        }
        public void Translate(double x, double y, double z)
        {
            foreach (Atom atom in _prot)
            {
                atom.X = atom.X + x;
                atom.Y = atom.Y + y;
                atom.Z = atom.Z + z;
            }
        }
        //Returns full atom matrix, of 3 rows, Count columns
        public double[,] ToMatrix()
        {
            double[,] AtomMatrix = new double[3, Count];
            for (int i = 0; i < Count; i++)
            {
                AtomMatrix[0, i] = _prot[i].X;
                AtomMatrix[1, i] = _prot[i].Y;
                AtomMatrix[2, i] = _prot[i].Z;
            }
            return AtomMatrix;
        }
        // returns CA matrix of CACount rows, 3 columns
        public double[,] CAMatrix()
        {
            double[,] AtomMatrix = new double[CACount, 3];
            int i = 0;
            for (int atomnum = 0; atomnum < Count; atomnum++)
            {
                if (_prot[atomnum].CA)
                {
                    AtomMatrix[i, 0] = _prot[atomnum].X;
                    AtomMatrix[i, 1] = _prot[atomnum].Y;
                    AtomMatrix[i, 2] = _prot[atomnum].Z;
                    i++;
                }
            }
            return AtomMatrix;
        }
        //takes in matrix of 3 rows, Count columns
        public void FromMatrix(Matrix<double> input)
        {
            for (int i = 0; i < Count; i++)
            {
                _prot[i].X = input[0, i];
                _prot[i].Y = input[1, i];
                _prot[i].Z = input[2, i];
            }
        }
        /*
         * 
         * */
        public int ParseFile(string file)
        {
            //clear any previous atom data
            _prot.Clear();
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
                            _prot.Add(new Atom(elem, elements[5], chain, res, x, y, z));

                        }
                    }
                }
                catch (Exception ex){
                    throw (new Exception("ERROR: File not recognized: " + ex.Message));
                }
                sr.Close();
            }
            return _prot.Count();
        }
        
    }
}
