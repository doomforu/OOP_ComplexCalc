using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ComplexCalculator
{
    internal class Complex  //Implementovana trida z cv2
    {
        public double real;
        public double img;

        public static Complex Parse(string s)  //Parsovaci fuknce zatim funguje pro format 1+2j, 1+-2j, -1+-2j atd.
        {
            Regex regex = new Regex(@"([-+]?\d+\.?\d*|[-+]?\d*\.?\d+)" +
            @"\s*" +
           @"\+" +
            @"\s*" +
            @"([-+]?\d+\.?\d*|[-+]?\d*\.?\d+)" +
            @"j");

            Match match = regex.Match(s);
            if (!match.Success)
            {
                return new Complex(0, 0);
                //throw new FormatException("Invalid complex number format");
            }
            return new Complex(double.Parse(match.Groups[1].Value),
                double.Parse(match.Groups[2].Value));         
        }

        public Complex(double real, double img)
        {
            this.real = real;
            this.img = img;
        }

        public static Complex operator +(Complex a, Complex b)
        {
            Complex sum = new Complex(0, 0);
            sum.real = a.real + b.real;
            sum.img = a.img + b.img;

            return sum;
        }

        public static Complex operator -(Complex a, Complex b)
        {
            Complex dif = new Complex(0, 0);
            dif.real = a.real - b.real;
            dif.real = a.real - b.real;

            return dif;
        }

        public static Complex operator *(Complex a, Complex b)
        {
            Complex prod = new Complex(0, 0);
            prod.real = a.real * b.real - a.img * b.img;
            prod.img = a.real * b.img + a.img * b.real;
            return prod;
        }

        public static Complex operator /(Complex a, Complex b)
        {
            Complex frac = new Complex(0, 0);
            double c = Math.Pow(b.real, 2) + Math.Pow(b.img, 2);
            frac.real = (a.real * b.real + a.img * b.img) / c;
            frac.img = (a.real * (-b.img) + a.img * b.real) / c;

            return frac;
        }

        public static bool operator ==(Complex a, Complex b)
        {
            return a.real == b.real && a.img == b.img;
        }

        public static bool operator !=(Complex a, Complex b)
        {
            return !(a == b);
        }

        public  Complex Invert()
        {
            return new Complex(-real, -img);
        }

        public Complex Sdruzene()
        {
            return new Complex(real, -img);
        }

        public double Mod()
        {
            return Math.Sqrt(Math.Pow(real, 2) + Math.Pow(img, 2));
        }

        public double Arg()
        {
            return Math.Atan2(img, real);
        }

        public override string ToString()
        {
            
            if (img < 0)
                return string.Format("{0}-{1}j", real, -img);
            return string.Format("{0}+{1}j", real, img);

        }

    }
}
