using System;
using BRE;

namespace BRE.Start
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length < 1)
			{
				Console.WriteLine("Needing argument!");
			}
			else 
			{
				var engine = new BRE();

				var result = engine.EvaluateSentence(args[0], new System.Collections.Generic.Dictionary<string, double>());

				Console.WriteLine(result);
			}         
        }
    }
}
