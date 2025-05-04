using System.Globalization;
using System.Text.RegularExpressions;


namespace ComplexCalculator
{
    internal class Complex  //Implementovana trida z cv2
    {
        public double real;
        public double img;
        public double r;
        public double exponentReal;
        public double exponentImag;


        public static Complex Parse(string s)
        {
            //s = s.Trim();

            // Standard formats: a+bj, a-bj, bj, a
            Regex standardPattern = new Regex(
            @"^([-+]?\d*[.,]?\d+)([-+]\d*[.,]?\d*)j$|" +  // a+bj
            @"^([-+]?\d*[.,]?\d+)j$|" +                   // bj
            @"^([-+]?\d*[.,]?\d+)$|" +                    // a
            @"^([-+]?)j$",                                // j, +j, -j
            RegexOptions.Compiled);

            // Exponential format: re^(a+bj) or r e^(a+bj)
            Regex exponentialPattern = new Regex(
    @"^(?<base>[-+]?\d+(?:[.,]\d+)?)\s*e\^\(" +  // Base number
    @"(?:" +
    @"(?<real>[-+]?\d+(?:[.,]\d+)?)\s*" +       // Real part
    @"(?<imaginary>[-+]\d+(?:[.,]\d+)?)j" +     // Imaginary part
    @"|" +
    @"(?<realOnly>[-+]?\d+(?:[.,]\d+)?)\s*" +   // Just real part
    @"|" +
    @"(?<imagOnly>[-+]?\d+(?:[.,]\d+)?)j" +     // Just imaginary part
    @"|" +
    @"(?<pureImaginary>[+-]?j)" +               // Pure imaginary part (e.g., e^(j) or e^(-j))
    @")\)$",
    RegexOptions.Compiled);


            Match standardMatch = standardPattern.Match(s);
            if (standardMatch.Success)
            {
                double real = 0;
                double imaginary = 0;

                // Handle a+bj format
                if (!string.IsNullOrEmpty(standardMatch.Groups[1].Value))
                    real = ParseNumber(standardMatch.Groups[1].Value);

                if (!string.IsNullOrEmpty(standardMatch.Groups[2].Value))
                    imaginary = ParseNumber(standardMatch.Groups[2].Value);

                // Handle bj format
                else if (!string.IsNullOrEmpty(standardMatch.Groups[3].Value))
                    imaginary = ParseNumber(standardMatch.Groups[3].Value);

                // Handle pure real number
                else if (!string.IsNullOrEmpty(standardMatch.Groups[4].Value))
                    real = ParseNumber(standardMatch.Groups[4].Value);

                // Handle j, +j, -j cases
                else if (standardMatch.Groups[5].Success)
                {
                    string sign = standardMatch.Groups[5].Value;
                    imaginary = sign switch
                    {
                        "+" => 1,
                        "-" => -1,
                        _ => 1  // no sign means positive
                    };
                }

                return new Complex(real, imaginary, 0, 0, 0);
            }

            Match exponentialMatch = exponentialPattern.Match(s);
            if (exponentialMatch.Success)
            {
                double r = ParseNumber(exponentialMatch.Groups["base"].Value);
                double exponentReal = 0;
                double exponentImag = 0;

                // Case 1: re^(a+bj) or variations
                if (exponentialMatch.Groups["real"].Success ||
                    exponentialMatch.Groups["imagSign"].Success ||
                    exponentialMatch.Groups["imagValue"].Success)
                {
                    if (exponentialMatch.Groups["real"].Success)
                    {
                        exponentReal = ParseNumber(exponentialMatch.Groups["real"].Value);
                    }

                    if (exponentialMatch.Groups["imagSign"].Success ||
                        exponentialMatch.Groups["imagValue"].Success)
                    {
                        string imagSign = exponentialMatch.Groups["imagSign"].Success ?
                            exponentialMatch.Groups["imagSign"].Value : "+";
                        string imagValue = exponentialMatch.Groups["imagValue"].Value;

                        if (string.IsNullOrEmpty(imagValue))
                            imagValue = "1";  // Handle cases like e^(j) or e^(-j)

                        exponentImag = ParseNumber(imagSign + imagValue);
                    }
                }
                // Case 2: re^(a)
                else if (exponentialMatch.Groups["realOnly"].Success)
                {
                    exponentReal = ParseNumber(exponentialMatch.Groups["realOnly"].Value);
                }
                // Case 3: re^(bj)
                else if (exponentialMatch.Groups["imagOnly"].Success)
                {
                    string imagOnly = exponentialMatch.Groups["imagOnly"].Value;
                    if (string.IsNullOrEmpty(imagOnly))
                        imagOnly = "1";  // Handle e^(j)
                    else if (imagOnly == "+")
                        imagOnly = "1";
                    else if (imagOnly == "-")
                        imagOnly = "-1";
                    else if (exponentialMatch.Groups["pureImaginary"].Success)
                    {
                        exponentImag = exponentialMatch.Groups["pureImaginary"].Value == "-j" ? -1 : 1;
                    }
                    exponentImag = ParseNumber(imagOnly);
                }

                return new Complex(0, 0, r, exponentReal, exponentImag);
            }

            throw new FormatException($"Invalid complex number format: '{s}'");
        }

        private static double ParseNumber(string s)
        {
            return double.Parse(s.Replace(',', '.'),
                CultureInfo.InvariantCulture);
        }

        public Complex(double real, double img, double r, double exponentReal, double exponentImag)
        {
            if (real == 0 && img == 0)
            {
                this.real = r * Math.Exp(exponentReal) * Math.Cos(exponentImag);
                this.img = r * Math.Exp(exponentReal) * Math.Sin(exponentImag);
                this.r = r;
                this.exponentReal = exponentReal;
                this.exponentImag = exponentImag;
            }
            else if(r == 0 & exponentReal == 0 & exponentImag == 0)
            {
                this.real = real;
                this.img = img;
                this.r = Math.Sqrt(Math.Pow(real, 2) + Math.Pow(img, 2));
                //this.exponentReal = Math.Log10(r);
                this.exponentImag = Math.Atan2(img,real);
            }
        }


        public static Complex operator +(Complex a, Complex b)
        {
            double real = a.real + b.real;
            double img = a.img + b.img;
            return new Complex(real,img,0 ,0 ,0);
        }

        public static Complex operator -(Complex a, Complex b)
        {
            double real = a.real - b.real;
            double img = a.img - b.img;

            return new Complex(real, img, 0, 0, 0);
        }

        public static Complex operator *(Complex a, Complex b)
        {
            double real = a.real * b.real - a.img * b.img;
            double img = a.real * b.img + a.img * b.real;
            return new Complex(real, img, 0, 0, 0);
        }

        public static Complex operator /(Complex a, Complex b)
        {
            double c = Math.Pow(b.real, 2) + Math.Pow(b.img, 2);
            double real = (a.real * b.real + a.img * b.img) / c;
            double img = (a.real * (-b.img) + a.img * b.real) / c;

            return new Complex(real, img, 0, 0, 0);
        }

        public static bool operator ==(Complex a, Complex b)
        {
            return a.real == b.real && a.img == b.img;
        }

        public static bool operator !=(Complex a, Complex b)
        {
            return !(a == b);
        }

        public Complex Invert()
        {
            return new Complex(-real, -img, 0, 0, 0);
        }

        public Complex Sdruzene()
        {
            return new Complex(real, -img,0 ,0 ,0);
        }

        public double Mod()
        {
            return Math.Sqrt(Math.Pow(real, 2) + Math.Pow(img, 2));
        }

        public double Arg()
        {
            return Math.Atan2(img, real);
        }

        public string ToString(string format)
        {
            if(format == "exponential")
            {
                return string.Format("{0:0.0000}e^({1:0.0000}j)", r, exponentImag);
                //return string.Format("{0:0.0000}e^({1:0.0000}+{2:0.0000}j)", r, exponentReal ,exponentImag);
            }
            else if (format == "algebraic")
            {
                if (img < 0)
                    return string.Format("{0:0.0000}-{1:0.0000}j", real, -img);
                return string.Format("{0:0.0000}+{1:0.0000}j", real, img);
            }

            return string.Format("{0:0.0000}+{1:0.0000}j", real, img);
        }
    }
}
