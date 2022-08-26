using System;
using System.Collections.Generic;
using Parser.Models;

namespace Generator
{
    public class Program
    {
        public static void Main()
        {
            List<MethodDto> methods = Parser.Methods.Parser.GetMethods();
            Console.WriteLine(methods.Count);
        }
    }
}