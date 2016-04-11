﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoleViewer
{
    class Atom
    {
        public Atom(string ele, string res, char cha, int r_num, double x_in, double y_in, double z_in)
        {
            element = ele;
            residue = res;
            chain = cha;
            res_num = r_num;
            x = x_in;
            y = y_in;
            z = z_in;
            if (ele == "CA")
            {
                isCA = true;
            }
            else
            {
                isCA = false;
            }

        }
        public string Ele
        {
            get
            {
                return element;
            }
        }
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        public double Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
            }
        }
        public bool CA
        {
            get
            {
                return isCA;
            }
        }
        private double x, y, z;
        private int res_num;
        private char chain;
        private string element;
        private string residue;
        private bool isCA;
    }
}
