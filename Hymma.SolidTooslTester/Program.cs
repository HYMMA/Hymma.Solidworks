using  Hymma.SolidTools.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTooslTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var cross =Mathematics.VectorsCross(new double[] { 2, 3, 4 }, new double[] { 5, 6, 7 });
            Console.WriteLine(string.Format("{0},{1},{2}",cross[0],cross[1],cross[2]));
            Console.ReadLine();
        }
    }
}
