using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MoleViewer
{
    class Protein
    {
        public Protein()
        {
            prot = new List<Atom>();
        }
        public double CenterX()
        {
            double retval = 0;
            //only execute if prot list of atoms is non empty
            if (!prot.Any())
            {
                double max = prot[0].X;
                double min = prot[0].X;
                for (int i = 1; i < prot.Count(); i++)
                {
                    if (prot[i].X > max) max = prot[i].X;
                    if (prot[i].X < min) min = prot[i].X;
                }
            }
            return retval;
        }
        public double CenterY()
        {
            double retval = 0;
            //only execute if prot list of atoms is non empty
            if (!prot.Any())
            {
                double max = prot[0].Y;
                double min = prot[0].Y;
                for (int i = 1; i < prot.Count(); i++)
                {
                    if (prot[i].Y > max) max = prot[i].Y;
                    if (prot[i].Y < min) min = prot[i].Y;
                }
            }
            return retval;
        }
        public double CenterZ()
        {
            double retval = 0;
            //only execute if prot list of atoms is non empty
            if (!prot.Any())
            {
                double max = prot[0].Z;
                double min = prot[0].Z;
                for (int i = 1; i < prot.Count(); i++)
                {
                    if (prot[i].Z > max) max = prot[i].Z;
                    if (prot[i].Z < min) min = prot[i].Z;
                }
            }
            return retval;
        }
        /*
         * 
         * */
        public int ParseFile(string file)
        {
            //clear any previous atom data
            prot.Clear();
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
                            double x = Convert.ToDouble(elements[10]);
                            double y = Convert.ToDouble(elements[11]);
                            double z = Convert.ToDouble(elements[12]);
                            //create new atom object
                            prot.Add(new Atom(elements[3], elements[5], chain, res, x, y, z));

                        }
                    }
                }
                catch (Exception ex){
                    throw (new Exception("ERROR: File not recognized: " + ex.Message));
                }
                sr.Close();
            }
            return prot.Count();
        }
        /*
         * Member Variables 
         * */
        public List<Atom> prot;
    }
}
