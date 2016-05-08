using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoleViewer
{
    class Atom
    {
        private double m_x, m_y, m_z;
        private int m_res_num;
        private char m_chain;
        private string m_element;
        private string m_residue;
        private bool m_isCA;
        /// <summary>
        /// Radius of covalently bonded nitrogen atom
        /// </summary>
        public const double N_RAD = .75;
        /// <summary>
        /// Radius of covalently bonded oxygen atom
        /// </summary>
        public const double O_RAD = .73;
        /// <summary>
        /// Radius of covalently bonded carbon atom
        /// </summary>
        public const double C_RAD = .77;
        /// <summary>
        /// Default Constructor for Atom type
        /// </summary>
        public Atom()
        {
            m_element = null;
            m_residue = null;
            m_chain = '\0';
            m_res_num = 0;
            m_x = 0;
            m_y = 0;
            m_z = 0;
            m_isCA = false;
        }
        /// <summary>
        /// Constructor for Atom type
        /// </summary>
        /// <param name="a_ele">String for the element of the atom</param>
        /// <param name="a_res">String representing what residue type atom belongs to</param>
        /// <param name="a_cha">Char for representing which chain atom is on</param>
        /// <param name="a_r_num">Int representing what residue number the atom is on</param>
        /// <param name="a_x_in">Double for the x coordinate of the atom in angstroms</param>
        /// <param name="a_y_in">Double for the y coordinate of the atom in angstroms</param>
        /// <param name="a_z_in">Double for the z coordinate of the atom in angstroms</param>
        public Atom(string a_ele, string a_res, char a_cha, int a_r_num, double a_x_in, double a_y_in, double a_z_in)
        {
            m_element = a_ele;
            m_residue = a_res;
            m_chain = a_cha;
            m_res_num = a_r_num;
            m_x = a_x_in;
            m_y = a_y_in;
            m_z = a_z_in;
            if (a_ele == "CA")
            {
                m_isCA = true;
            }
            else
            {
                m_isCA = false;
            }

        }
        /// <summary>
        /// Accesor for what element the atom is
        /// </summary>
        public string Ele
        {
            get
            {
                return m_element;
            }
        }
        /// <summary>
        /// Accesor for the x coordinate of the atom
        /// </summary>
        public double X
        {
            get
            {
                return m_x;
            }
            set
            {
                m_x = value;
            }
        }
        /// <summary>
        /// Accesor for the y coordinate of the atom
        /// </summary>
        public double Y
        {
            get
            {
                return m_y;
            }
            set
            {
                m_y = value;
            }
        }
        /// <summary>
        /// Accesor for the z coordinate of the atom
        /// </summary>
        public double Z
        {
            get
            {
                return m_z;
            }
            set
            {
                m_z = value;
            }
        }
        /// <summary>
        /// Accesor for if the atom is an alpha carbon
        /// </summary>
        public bool CA
        {
            get
            {
                return m_isCA;
            }
        }
        /// <summary>
        /// Accesor for the residue number of the atom
        /// </summary>
        public int Residue_Num
        {
            get
            {
                return m_res_num;
            }
        }
        /// <summary>
        /// Accesor for the chain the atom is on
        /// </summary>
        public char Chain
        {
            get
            {
                return m_chain;
            }
        }
    }
}
