using BagProblemWithAG.Solucoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagProblemWithAG
{
    class Program
    {
        static void Main(string[] args)
        {
            List<float> avgFitness = new List<float>();

            Ag ag = new Ag(10, 25,10000);
            List<SolucaoMochila> pop = ag.geraPopulacaoInicial(200);

            for (int i = 0; i < 100; i++)
            {
                pop = ag.novaGeracao(pop, 0, false);
                ag.imprimirPopulacao(i,pop);

                avgFitness.Add((int)ag.fitnessMedio(pop));
            }

            Console.WriteLine(String.Join(",", avgFitness));
            Console.ReadKey();
        }
    }
}
