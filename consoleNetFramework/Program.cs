using System;
using System.Text.RegularExpressions;
namespace consoleNetFramework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var optsWithPasswordHidden = new Regex(@"(?x)                    #ignore pattern white space so we can leave comments
                (?i)                    #ignore case
                    (?<=/p\s+)          #positive look behind for /p that is followed by white space(s)
                    .*?                 #get everything lazy way 
                    (?=\s+)             #positive look ahead for white space(s) 
                ").Replace("signtool sign /f MyCert.pfx /p 1@1234%%%%ThisIsPassword%%%% /p7 otherOptions /pg yetAnotherPolicy /fd SHA256 MyFile.exe", "/p ********");
            Console.WriteLine(optsWithPasswordHidden);
            Console.ReadLine();
        }
    }
}
