using System;
using System.Collections.Generic;
using System.Text;

namespace TautologyCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Proszę podać równanie:");
            var finalData = new StringBuilder();

            while (true)
            {
                string entry = Console.ReadLine();
                if (entry == null || Environment.NewLine.Contains(entry))
                {
                    break;
                }
                var IsTautology = new IsTautology(entry);
                IsTautology.ArgumentsCheck();
                IsTautology.GeneratePossibilityMatrix();
                IsTautology.OperatorEvaluation();

                //Dane wyjściowe
                finalData.AppendLine(entry);
                for (int i = 0; i < IsTautology.matrixOut.GetLength(0); i++)
                {
                    for (int n = 0; n < IsTautology.matrixOut.GetLength(1); n++)
                    {
                        finalData.Append(IsTautology.matrixOut[i, n]);
                    }
                    finalData.AppendLine();
                }
                finalData.AppendLine();
            }

            Console.WriteLine("Wynik równania:");
            Console.Write(finalData);
        }     
    }
}