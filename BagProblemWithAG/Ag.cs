using BagProblemWithAG.Solucoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagProblemWithAG
{
    public class Ag
    {
        private int numMochilas = 10;
        private int numMaxGeracoes = 100;
        private int maxPeso = 25;
        private float mutacaoProp = 0.04f;
        private float cossProp = 0.6f;

        public Ag(int numMochilas, int maxPeso, int numMaxGeracoes)
        {
            this.numMochilas = numMochilas;
            this.maxPeso = maxPeso;
            this.numMaxGeracoes = numMaxGeracoes;
        }

        public List<SolucaoMochila> populacaoAtual { get; set; }

        /// <summary>
        /// Gera a população
        /// </summary>
        /// <param name="n">Tamanho da população</param>
        public List<SolucaoMochila> geraPopulacaoInicial(int n)
        {
            List<SolucaoMochila>  populacao = new List<SolucaoMochila>();

            for(int i =0; i < n; i++)
            {
                populacao.Add(new SolucaoMochila(numMochilas, maxPeso));            
            }

            populacaoAtual = populacao;

            return populacao;
        }

        public void avaliaPopulacao(List<SolucaoMochila> populacao)
        {
            //metodo desnecessario o calculo do fitness eh feito em cada individuo (SolucaoMochila)
        }

        /// <summary>
        /// gera uma nova geração de indivíduos a partir da população P.
        /// </summary>
        /// <param name="populacao">População</param>
        /// <param name="numGeracao">numGeracao representa o número da geração, para controlar o critério de parada.</param>
        /// <param name="elitismo">elitismo é um parâmetro booleano que informa se será aplicado o elitismo, isto é, se o melhor indivíduo da população P será copiado sem modificações para a nova geração.</param>
        /// <returns></returns>
        public List<SolucaoMochila> novaGeracao(List<SolucaoMochila> populacao, int numGeracao, bool elitismo)
        {
            if (numGeracao > numMaxGeracoes)
                return populacao;

            List<SolucaoMochila> novaPopulacao = new List<SolucaoMochila>();

            if (elitismo)
                novaPopulacao.Add(melhorIndividuo(populacao));

            for (int i = 0; i < populacao.Count / 2; i++)
            {
                List<SolucaoMochila> indsCross = crossover(sorteiaIndividuoRoleta(populacao), sorteiaIndividuoRoleta(populacao), cossProp);

                if (indsCross != null) // ocorreu crossover
                    novaPopulacao.AddRange(indsCross);

                mutacao(sorteiaIndividuo(novaPopulacao), mutacaoProp);
            }

            return novaPopulacao;
        }

        /// <summary>
        /// realiza o crossover entre os indivíduos i1 e i2 de acordo com a probabilidade prob.
        /// </summary>
        /// <param name="i1">Individuo 1</param>
        /// <param name="i2">Individuo 2</param>
        /// <param name="probabilidade">entre 0 e 1</param>
        /// <returns>Retorna os dois filhos gerados.</returns>
        public List<SolucaoMochila> crossover(SolucaoMochila i1, SolucaoMochila i2, float probabilidade)
        {
            return new List<SolucaoMochila> { i1.Mesclar(i2), i2.Mesclar(i1) };
        }

        /// <summary>
        /// realiza a mutação no indivíduo i de acordo com a probabilidade prob.
        /// </summary>
        /// <param name="i">Indivíduo</param>
        /// <param name="probabilidade">Probabilidade (entre 0 e 1)</param>
        /// <returns>Retorna o indivíduo modificado.</returns>
        public void mutacao(SolucaoMochila i, float probabilidade)
        {
            if (StaticRandom.Instance.Next(0, 100) < probabilidade * 100)
            {
                i.RemoverMochila(StaticRandom.Instance.Next(0, numMochilas));
            }
        }

        public void imprimirPopulacao(int geracao, List<SolucaoMochila> populacao)
        {
            SolucaoMochila melhor = melhorIndividuo(populacao);
            float sumFitness = populacao.Sum(s => s.Fitness);
            float avgFitness = sumFitness / (float)populacao.Count;

            Console.WriteLine("Geração " + geracao + ":");
            Console.WriteLine("\tMelhor solução:" + melhor);
            Console.WriteLine("\tSoma dos fitness:" + sumFitness);
            Console.WriteLine("\tFitness médio:" + avgFitness);
            Console.WriteLine(Environment.NewLine);
        }

        public float fitnessMedio(List<SolucaoMochila> populacao)
        {
            float sumFitness = populacao.Sum(s => s.Fitness);
            float avgFitness = sumFitness / (float)populacao.Count;

            return avgFitness;
        }

        private SolucaoMochila sorteiaIndividuo(List<SolucaoMochila> populacao)
        {
            return populacao[StaticRandom.Instance.Next(0, populacao.Count)];
        }

        private SolucaoMochila sorteiaIndividuoRoleta(List<SolucaoMochila> populacao)
        {
            float sumFitness = populacao.Sum(s => s.Fitness);

            List<SolucaoMochila> ordenadoPelaAptidao = populacao.OrderBy(o => o.Fitness / sumFitness).ToList();

            float sorteio = StaticRandom.Instance.Next(0, Math.Abs((int)sumFitness));
            int count = 0;
            while (sorteio > 0)
            {
                sorteio -= ordenadoPelaAptidao[count].Fitness;
                count++;
            }

            if (count >= populacao.Count)
                return ordenadoPelaAptidao[ordenadoPelaAptidao.Count - 1];
            return ordenadoPelaAptidao[count];
        }

        private SolucaoMochila melhorIndividuo(List<SolucaoMochila> populacao)
        {
            SolucaoMochila melhor = populacao[0];
            foreach(SolucaoMochila s in populacao)
            {
                if (s.Fitness > melhor.Fitness)
                    melhor = s;
            }

            return melhor;
        }

    }
}
