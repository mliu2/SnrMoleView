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

        public ResNumPair()
        {
            m_Prot1ResNum = 0;
            m_Prot2ResNum = 0;
            m_Res1Chain = '\0';
            m_Res2Chain = '\0';
            m_Distance = 0;
        }
        public ResNumPair(int a_num1, int a_num2, char a_chain1, char a_chain2, double a_dist)
        {
            m_Prot1ResNum = a_num1;
            m_Prot2ResNum = a_num2;
            m_Res1Chain = a_chain1;
            m_Res2Chain = a_chain2;
            m_Distance = a_dist;
        }
        public int Prot1Num
        {
            get
            {
                return m_Prot1ResNum;
            }
        }
        public int Prot2Num
        {
            get
            {
                return m_Prot2ResNum;
            }
        }
        public double Distance
        {
            get
            {
                return m_Distance;
            }
        }
        public char Chain1
        {
            get
            {
                return m_Res1Chain;
            }
        }
        public char Chain2
        {
            get
            {
                return m_Res2Chain;
            }
        }
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
