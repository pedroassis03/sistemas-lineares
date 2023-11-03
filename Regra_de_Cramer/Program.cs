using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra; //biblioteca de métodos de equações lineares

namespace Regra_de_Cramer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int numEquacoes = 0;
            int numVariaveis = 0;
            Console.WriteLine("Informe o número de equações (linhas) do sistema: ");

            while (true)
            {
                try
                {
                    numEquacoes = int.Parse(Console.ReadLine());

                    while (numEquacoes <= 0)
                    {
                        Console.WriteLine("O número de equações deve ser maior que zero. Digite novamente");
                        numEquacoes = int.Parse(Console.ReadLine());
                    }

                    numVariaveis = numEquacoes; //o número de equações e variáveis deve ser igual para que o sistema tenha solução
                }
                catch (FormatException)
                {
                    Console.WriteLine("O formato digitado é incorreto. Digite novamente: ");
                    continue;
                }
                break;
            }

            Sistema sistema = new Sistema(numEquacoes, numVariaveis);

            // Criar a matriz de coeficientes (A) e o vetor de constantes (b)
            Matrix<double> A = sistema.CriarMatrizDeCoeficientes();
            Vector<double> b = sistema.CriarVetorDeConstantes();

            Console.WriteLine("Preparando o ambiente...");
            Thread.Sleep(2000);
            Console.Clear();
            Console.WriteLine("Informe as equações no formato 'i1 + i2 + i3...  = d':");

            for (int i = 0; i < sistema.NumEquacoes; i++)
            {
                string equationPattern = @"^\s*(?:-?\d+(?:\.\d+)?\s*[^=\d]+)+\s*=\s*-?\d+(?:\.\d+)?$"; //cria um padrão de inserção para as equações

                Console.Write($"\nEquação {i + 1}: ");
                string equacao = sistema.LerEquacao(equationPattern, i);

                // Analisar a equação para extrair os coeficientes e o termo constante
                string[] termos = equacao.Split(new string[] { "i1", "i2", "i3", "i4", "i5", "i6", "i7", "i8", "i9", "i10", "=" }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < sistema.NumVariaveis; j++)
                {
                    Console.Write($"Confirme o coeficiente de i{j + 1}: ");
                    while (true)
                    {
                        try
                        {
                            double coeficiente = double.Parse(Console.ReadLine().Trim());
                            double comparision = double.Parse(termos[j].Replace(" ", ""));

                            while (coeficiente != Convert.ToDouble(termos[j].Replace(" ", "")))
                            {
                                Console.Write($"O coeficiente digitado não condiz com a equação. Digite o coeficiente correto de i{j + 1}: ");
                                coeficiente = double.Parse((Console.ReadLine()).Trim());
                            }

                            Console.WriteLine("");
                            A[i, j] = coeficiente;

                        }
                        catch (FormatException)
                        {
                            Console.Write("O formato digitado é incorreto. Digite novamente: ");
                            continue;
                        }
                        break;
                    }
                }

                // O último termo é o termo constante
                b[i] = double.Parse(termos[termos.Length - 1]);
            }

            // Resolver o sistema de equações
            var solucao = A.Solve(b);

            // Exibir a solução
            Console.WriteLine("\nSolução:");
            for (int i = 0; i < numVariaveis; i++)
            {
                Console.WriteLine($"i{i + 1} = {solucao[i]:N3}A");
            }

            //Calcula i3 mesmo em sistemas de 2 equações
            if (numVariaveis == 2)
            {
                if (solucao[0] > solucao[1])
                {
                    Console.WriteLine($"i3 = {(solucao[0] - solucao[1]):N3}");
                }
                else if (solucao[0] < solucao[1])
                {
                    Console.WriteLine($"i3 = {(solucao[1] - solucao[0]):N3}");
                }
                else
                {
                    Console.WriteLine("i3 = 0");
                }
            }

            Console.ReadLine();
        }
    }
}

/*
 * ^ - Starts with
 * $ - Ends with
 * [] - Range
 * () - Group
 * . - Single character once
 * + - one or more characters in a row
 * ? - optional preceding character match
 * \ - escape character
 * \n - New line
 * \d - Digit
 * \D - Non-digit
 * \s - White space
 * \S - non-white space
 * \w - alphanumeric/underscore character (word chars)
 * \W - non-word characters
 * {x,y} - Repeat low (x) to high (y) (no "y" means at least x, no ",y" means that many)
 * (x|y) - Alternative - x or y
 * 
 * [^x] - Anything but x (where x is whatever character you want)
 */
