using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagProblemWithAG.Solucoes
{
    public class SolucaoMochila
    {
        /// <summary>
        /// probabilidade de gerar ter a mochila na solução
        /// </summary>
        private int probMochila = 30;

        private int numMochilas = 10;

        private int maxPeso = 25;

        private int[] objetos { get; set; }
        private int[] beneficios { get; set; }
        private int[] pesos { get; set; }

        public float Fitness {
            get
            {
                float alpha = beneficios.Sum();
                int temp = beneficios.Zip(objetos, (d1, d2) => d1 * d2).Sum(); // produto interno dos arrays beneficios e objetos
                return temp - alpha * (pesos.Sum() - maxPeso);
            }
        }

        public SolucaoMochila(int numMochilas, int maxPeso)
        {
            this.maxPeso = maxPeso;
            this.numMochilas = numMochilas;

            GerarObjetos();
            GerarBeneficios();
            GerarPesos();
        }

        /// <summary>
        /// Adiciona uma mochila na solução
        /// </summary>
        /// <param name="beneficio"></param>
        /// <param name="peso"></param>
        public void AdicionarMochila(int objeto,int beneficio, int peso)
        {
            objetos[objeto] = 1;
            beneficios[objeto] = beneficio;
            pesos[objeto] = peso;
        }

        /// <summary>
        /// remove uma mochila da solução
        /// </summary>
        /// <param name="pos"></param>
        public void RemoverMochila(int objeto)
        {
            objetos[objeto] = 0;
        }

        /// <summary>
        /// joga as posicoes pares dos arrays objetos/benefiecios em um novo elemento baseado no parametro sm
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        public SolucaoMochila Mesclar(SolucaoMochila sm)
        {
            SolucaoMochila novoMesclado = new SolucaoMochila(numMochilas, maxPeso);

            for(int i = 0; i < numMochilas; i++)
            {
                if(i%2 == 0)
                {
                    novoMesclado.objetos[i] = this.objetos[i];
                    novoMesclado.beneficios[i] = this.beneficios[i];
                    novoMesclado.pesos[i] = this.pesos[i];
                }
                else
                {
                    novoMesclado.objetos[i] = sm.objetos[i];
                    novoMesclado.beneficios[i] = sm.beneficios[i];
                    novoMesclado.pesos[i] = sm.pesos[i];
                }
            }
            return novoMesclado;
        }

        private void GerarObjetos()
        {
            objetos = new int[numMochilas];
            for(int i=0;i<numMochilas;i++)
            {
                 //probabilidade de gerar mochila
                if(StaticRandom.Instance.Next(1, 100) < probMochila)
                    objetos[i] = 1;
                else
                    objetos[i] = 0;
            }
        }

        private void GerarBeneficios()
        {
            beneficios = new int[numMochilas];
            for (int i = 0; i < numMochilas; i++)
            {
                beneficios[i] = StaticRandom.Instance.Next(1, 5);
            }
        }

        private void GerarPesos()
        {
            pesos = new int[numMochilas];
            for (int i = 0; i < numMochilas; i++)
            {
                pesos[i] = StaticRandom.Instance.Next(1, 5);
            }
        }

        public override string ToString()
        {
            string rtn = "";
            //string format = "(objeto:{0}, peso:{1}, benefício:{2}),";
            string format = "{0}&{1}&{2},";
            for (int i = 0; i < objetos.Length; i++)
            {
                rtn += string.Format(format, objetos[i], pesos[i], beneficios[i]);
            }

            rtn = "[" + rtn.Remove(rtn.Length - 1, 1) + "]" + Environment.NewLine;

            return rtn;
        }
    }
}
