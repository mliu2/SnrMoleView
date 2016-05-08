using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoleViewer
{
    class ResNumPair : IComparable<ResNumPair>
    {
        private int m_Prot1ResNum;
        private int m_Prot2ResNum;
        private double m_Distance;
        private char m_Res1Chain;
        private char m_Res2Chain;
        /// <summary>
        /// Default Constructor for ResNumPair
        /// </summary>
        public ResNumPair()
        {
            m_Prot1ResNum = 0;
            m_Prot2ResNum = 0;
            m_Res1Chain = '\0';
            m_Res2Chain = '\0';
            m_Distance = 0;
        }
        /// <summary>
        /// Constructor for ResNumPair
        /// </summary>
        /// <param name="a_num1">Residue number of Alpha Carbon on first protein</param>
        /// <param name="a_num2">Residue number of Alpha Carbon on second protein</param>
        /// <param name="a_chain1">Chain of Alpha Carbon on first protein</param>
        /// <param name="a_chain2">Chain of Alpha Carbon on second protein</param>
        /// <param name="a_dist">Distance between the two alpha carbons</param>
        public ResNumPair(int a_num1, int a_num2, char a_chain1, char a_chain2, double a_dist)
        {
            m_Prot1ResNum = a_num1;
            m_Prot2ResNum = a_num2;
            m_Res1Chain = a_chain1;
            m_Res2Chain = a_chain2;
            m_Distance = a_dist;
        }
        /// <summary>
        /// Accessor for the residue number of alpha carbon on first protein
        /// </summary>
        public int Prot1Num
        {
            get
            {
                return m_Prot1ResNum;
            }
        }
        /// <summary>
        /// Accessor for the residue number of alpha carbon on second protein
        /// </summary>
        public int Prot2Num
        {
            get
            {
                return m_Prot2ResNum;
            }
        }
        /// <summary>
        /// Accessor for distance between the two atoms
        /// </summary>
        public double Distance
        {
            get
            {
                return m_Distance;
            }
        }
        /// <summary>
        /// Accessor for the chain of the alpha carbon on the first protein
        /// </summary>
        public char Chain1
        {
            get
            {
                return m_Res1Chain;
            }
        }
        /// <summary>
        /// Accessor for the chain of the alpha carbon on the second protein
        /// </summary>
        public char Chain2
        {
            get
            {
                return m_Res2Chain;
            }
        }
        /// <summary>
        /// Required comparison function for child of IComparable
        /// </summary>
        /// <param name="a_other">Other ResNumPair object to compare to by distance</param>
        /// <returns>
        /// Returns 0 if the distances are equal.
        /// Returns -1 if this distance is smaller than distance of a_other.
        /// Returns 1 if this distance is greater than distance of a_other.
        /// </returns>
        public int CompareTo(ResNumPair a_other)
        {
            if (a_other.Distance == Distance){
                return 0;
            }else if(a_other.Distance > Distance){
                return -1;
            } else {
                return 1;
            }
        }
    }
}
