using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoleViewer
{
    class ResNumPair
    {
        private int m_Prot1ResNum;
        private int m_Prot2ResNum;

        public ResNumPair()
        {
            m_Prot1ResNum = 0;
            m_Prot2ResNum = 0;
        }
        public ResNumPair(int a_num1, int a_num2)
        {
            m_Prot1ResNum = a_num1;
            m_Prot2ResNum = a_num2;
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
    }
}
