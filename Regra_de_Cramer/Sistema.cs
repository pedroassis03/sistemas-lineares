using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;


namespace Regra_de_Cramer
{
    internal class Sistema
    {
        public Sistema(int numEquacoes, int numVariaveis)
        {
            NumEquacoes = numEquacoes;
            NumVariaveis = numVariaveis;
        }

        public int NumEquacoes { get; }

        public int NumVariaveis { get; }


        public MathNet.Numerics.LinearAlgebra.Matrix<double> CriarMatrizDeCoeficientes()
        {
            var A = Matrix<double>.Build.Dense(NumEquacoes, NumVariaveis);
            return A;
        }

        public MathNet.Numerics.LinearAlgebra.Vector<double> CriarVetorDeConstantes()
        {
            var b = Vector<double>.Build.Dense(NumEquacoes);
            return b;
        }

        public string LerEquacao(string equationPattern, int i)
        {
            while (true)
            {
                string equacao = Console.ReadLine();

                if (Regex.IsMatch(equacao, equationPattern)) //verifica se a equação atende ao padrão
                {
                    return equacao;
                }
                else
                {
                    Console.Write($"Formato de equação inválido.\n\nDigite a Equação {i + 1} novamente: ");
                }
            }

        }
    }
}
