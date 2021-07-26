using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var refe = new Ref();
            var client = new client() { myref = refe };
            refe.MyProperty = 40;

            Console.WriteLine(client.myref.MyProperty) ;
            Console.ReadLine();
        }
    }

    class Ref
    {
        public Ref()
        {
            MyProperty = 2;
        }
        public int MyProperty { get; set; }
    }

   class client
    {
        public client()
        {
            ClientProperty = 3;
        }
        public int ClientProperty { get; set; }
        public Ref myref { get; set; }
    }
}
