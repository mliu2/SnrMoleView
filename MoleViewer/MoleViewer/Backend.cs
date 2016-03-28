using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoleViewer
{
    class Backend
    {
        private Protein prot1;
        private Protein prot2;
        
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
