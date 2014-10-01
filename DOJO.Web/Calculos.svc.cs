using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace DOJO.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Calculos
    {
        private char[,] teste { get; set; }
        [OperationContract]
        public void DoWork()
        {
            // Add your operation implementation here
            return;
        }

        private static char[,] BiDimensionalMatriz(string resultado)
        {

            char[,] rollback = new char[10, 10];
            var rows = resultado.Split(';');

            for (int i = 0; i < 10; i++)
            {
                var columns = rows[i].Split('|');

                for (int j = 0; j < 10; j++)
                {
                    Char.TryParse(columns[j].ToString(), out rollback[i, j]);
                }
            }
            return rollback;

        }

        private int ContarAeroportos(char[,] Mapa)
        {
            int cont = 0;

            for (int i = 0; i < Mapa.GetLength(0); i++)
            {
                for (int j = 0; j < Mapa.GetLength(1); j++)
                {
                    if (Mapa[i, j] == 'A')
                    {
                        cont += 1;
                    }
                }
            }

            return cont;
        }


        private static char[,] Expandir(char[,] Mapa)
        {
            char[,] quadrado = new char[10, 10];
            for (int i = 0; i < Mapa.GetLength(0); i++)
            {
                for (int j = 0; j < Mapa.GetLength(1); j++)
                {
                    if (Mapa[i, j] == ' ')
                    {
                        quadrado[i, j] = ' ';
                    }
                    if (Mapa[i, j] == 'A')
                    {
                        quadrado[i, j] = 'A';
                    }
                    if (Mapa[i, j] == '*')
                    {
                        quadrado[i, j] = '*';
                        if (j > 0)
                            quadrado[i, j - 1] = '*';
                        if (j < 9)
                            quadrado[i, j + 1] = '*';
                        if (i > 0)
                            quadrado[i - 1, j] = '*';
                        if (i < 9)
                            quadrado[i + 1, j] = '*';
                    }
                }
            }

            return quadrado;


        }

        private static string CharToString(char[,] quadrado)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    sb.Append(quadrado[i, j].ToString());
                    sb.Append("|");
                }
                sb.Append(";");
            }
            return sb.ToString();
        }

        [OperationContract]
        public string PrimeiroDia()
        {
            Mapa();
            return CharToString(teste);
        }

        private void Mapa()
        {
            teste = new char[,]{
                {' ', ' ', ' ', ' ', ' ',' ', ' ', ' ', ' ', ' '},
                {' ', '*', ' ', ' ', ' ',' ', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', ' ',' ', ' ', ' ', ' ', '*'},
                {' ', ' ', ' ', ' ', ' ',' ', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', 'A', ' ',' ', ' ', 'A', ' ', ' '},
                {' ', ' ', ' ', ' ', ' ',' ', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', 'A',' ', ' ', ' ', ' ', '*'},
                {' ', ' ', ' ', ' ', ' ',' ', ' ', ' ', ' ', ' '},
                {' ', ' ', '*', ' ', ' ',' ', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', ' ',' ', ' ', ' ', ' ', ' '}
            };
        }


        [OperationContract]
        public string PrimeiroAeroporto()
        {
            Mapa();
            while (true)
            {
                teste = Expandir(teste);
                int aeroportos = ContarAeroportos(teste);
                if (aeroportos <= 2)
                    break;
            }
            return CharToString(teste);
        }


        [OperationContract]
        public string TodosAeroportos()
        {
            Mapa();
            while (true)
            {
                teste = Expandir(teste);
                int aeroportos = ContarAeroportos(teste);
                if (aeroportos <= 0)
                    break;
            }
            return CharToString(teste);
        }

        [OperationContract]
        public int DiasTodosAeroportos()
        {
            int count = 0;
            Mapa();
            while (true)
            {
                count++;
                teste = Expandir(teste);
                int aeroportos = ContarAeroportos(teste);
                if (aeroportos <= 0)
                    break;
            }

            return count;
        }

        [OperationContract]
        public int DiasPrimeiroAeroporto()
        {
            int count = 0;
            Mapa();
            while (true)
            {
                count++;
                teste = Expandir(teste);
                int aeroportos = ContarAeroportos(teste);
                if (aeroportos <= 2)
                    break;
            }

            return count;
        }
    }
}
