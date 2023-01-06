using System;
using System.IO;

namespace Challenge_1
{
    public class Program
    {
        public static void Main()
        {
            StreamReader lector = new("info.txt");
            int row = File.ReadLines("info.txt").Count();

            if (row != 0 && (lector.ReadLine()!).Split(" ").Count() == 5)
            {
                string[,] users = new string[row, 5];
                int i = 0, j = 0;
                string linea;
                lector.BaseStream.Position = 0;
                lector.DiscardBufferedData();

                while ((linea = lector.ReadLine()!) != null)
                {
                    string[] words = linea.Split(" ");

                    if (j > 4)
                    {
                        j = 0;
                    }

                    foreach (string word in words)
                    {
                        users[i, j] = word;
                        j++;
                    }
                    i++;
                }
                lector.Close();
                int start = 1;

                do
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(
                    "\nSeleccionar opción: \n" +
                    "1 - Promedio de cantidad de películas vistas.\n" +
                    "2 - Promedio de cantidad de películas vistas por periodo.\n" +
                    "3 - Promedio de Cantidad de películas por edad.\n" +
                    "4 - Promedio de Cantidad de películas por sexo.\n" +
                    "5 - Promedio de cantidad de películas vistas por periodo y sexo.\n" +
                    "0 - Salir\n" +
                    "\nSu Opción: "
                    );

                    string input = Console.ReadLine()!;

                    int option = ParStr(input, 0, 5);

                    switch (option)
                    {
                        case 1:
                            AvgMov(users);
                            break;

                        case 2:
                            AvgPer(users);
                            break;

                        case 3:
                            AvgEdad(users);
                            break;

                        case 4:
                            AvgGen(users);
                            break;

                        case 5:
                            AvgPerGen(users);
                            break;

                        case 0:
                            Console.WriteLine("Cerrando Programa.");
                            start = 0;
                            break;
                    }
                    if (option != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n¿Continuar? (1)Si (0)No" + "\nSu opcion: ");
                        string st_input = Console.ReadLine()!;
                        start = ParStr(st_input, 0, 1);

                        if (start != 1)
                        {
                            start = 0;
                            Console.WriteLine("Cerrando Programa");
                        }
                    }
                } while (start == 1);

            }
            else
            {   
                Console.WriteLine("Archivo con contenido incorrecto.");
            }
        }

        public static int ParStr(string input, int fnum, int snum)
        {
            if (int.TryParse(input, out int num) && num >= fnum && num <= snum)
            {
                return int.Parse(input);
            }
            else
            {
                Console.WriteLine("Por favor ingresar un numero válido.");
                return 0;
            }
        }

        public static void AvgMov(string[,] users)
        {
            int total = 0;
            for (int i = 0; i < users.GetLength(0); i++)
            {
                total += int.Parse(users[i, 4]);
            }

            float avg = (float)total / users.GetLength(0);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Promedio de cantidad de películas vistas.");
            Console.WriteLine(GrapDesign(avg) + Math.Round(avg, 2));
        }

        public static void AvgPer(string[,] users)
        {
            int[,] months = new int[13, 2];
            string[] nameMonths = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

            for (int i = 0; i < users.GetLength(0); i++)
            {
                for (int j = 0; j <= 12; j++)
                {
                    if (j < 10)
                    {
                        if (users[i, 3] == (string)("0" + j + "/2022"))
                        {
                            months[j, 0] += int.Parse(users[i, 4]);
                            months[j, 1] += 1;
                        }
                    }
                    else
                    {
                        if (users[i, 3] == (string)(j + "/2022"))
                        {
                            months[j, 0] += int.Parse(users[i, 4]);
                            months[j, 1] += 1;
                        }
                    }
                }
            }

            for (int i = 0; i < 13; i++)
            {
                if (months[i, 0] != 0)
                {
                    float avg = (float)months[i, 0] / months[i, 1];
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(GrapDesign(avg) + Math.Round(avg, 2) + " " + nameMonths[i - 1]);
                }
            }
        }

        public static void AvgEdad(string[,] users)
        {
            string[] nacSplit = new string[3];
            int[,] CantEdad = new int[users.GetLength(0), 3];
            DateTime nac;
            int edad, ult = 0;

            for (int i = 0; i < users.GetLength(0); i++)
            {
                nacSplit = users[i, 1].Split("/");
                nac = new DateTime(int.Parse(nacSplit[2]), int.Parse(nacSplit[1]), int.Parse(nacSplit[0]));
                edad = DateTime.Today.AddTicks(-nac.Ticks).Year - 1;
                int cont = 0, found = 0, index = 0;

                for (int j = 0; j < CantEdad.GetLength(0); j++)
                {
                    if (edad == CantEdad[j, 0])
                    {
                        found = 1;
                        index = j;
                        cont++;
                        break;
                    }
                    else
                    {
                        found = 0;
                    }
                }

                if (found == 1)
                {
                    CantEdad[index, 1] += int.Parse(users[i, 4]);
                    CantEdad[index, 2] += cont++;
                }
                else
                {
                    cont++;
                    CantEdad[ult, 0] = edad;
                    CantEdad[ult, 1] += int.Parse(users[i, 4]);
                    CantEdad[ult, 2] += cont;
                    ult++;
                }
            }

            for (int i = 0; i < CantEdad.GetLength(0); i++)
            {
                if (CantEdad[i, 0] != 0)
                {
                    float avg = (float)CantEdad[i, 1] / CantEdad[i, 2];
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Edad " + CantEdad[i, 0] + " -> " + GrapDesign(avg) + Math.Round(avg, 2));
                }
            }
        }

        public static void AvgGen(string[,] users)
        {
            string[] Gen = { "M", "F", "X" };
            int[,] CantGen = new int[3, 2];

            for (int i = 0; i < users.GetLength(0); i++)
            {
                if (users[i, 2] == "M")
                {
                    CantGen[0, 0] += int.Parse(users[i, 4]);
                    CantGen[0, 1] += 1;
                }
                else if (users[i, 2] == "F")
                {
                    CantGen[1, 0] += int.Parse(users[i, 4]);
                    CantGen[1, 1] += 1;
                }
                else
                {
                    CantGen[2, 0] += int.Parse(users[i, 4]);
                    CantGen[2, 1] += 1;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                float avg = (float)CantGen[i, 0] / CantGen[i, 1];
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Sexo " + Gen[i] + " -> " + GrapDesign(avg) + Math.Round(avg, 2));
            }
        }

        public static void AvgPerGen(string[,] users)
        {
            int[,] CantPerGen = new int[users.GetLength(0), 2];
            string[,] CantGen = new string[users.GetLength(0), 2];

            int ult = 0;

            for (int i = 0; i < users.GetLength(0); i++)
            {
                int cont = 0, found = 0, index = 0;

                for (int j = 0; j < users.GetLength(0); j++)
                {
                    if (users[i, 3] == CantGen[j, 1] && users[i, 2] == CantGen[j, 0])
                    {
                        found = 1;
                        index = j;
                        cont++;
                        break;
                    }
                    else
                    {
                        found = 0;
                    }
                }

                if (found == 1)
                {
                    CantPerGen[index, 0] += int.Parse(users[i, 4]);
                    CantPerGen[index, 1] += cont++;
                }
                else
                {
                    cont++;
                    CantGen[ult, 0] = users[i, 2];
                    CantGen[ult, 1] = users[i, 3];
                    CantPerGen[ult, 0] += int.Parse(users[i, 4]);
                    CantPerGen[ult, 1] += cont++;
                    ult++;
                }
            }

            for (int i = 0; i < CantPerGen.GetLength(0); i++)
            {
                if (CantPerGen[i, 0] != 0)
                {
                    float avg = (float)CantPerGen[i, 0] / CantPerGen[i, 1];
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Periodo " + CantGen[i, 1] + " Sexo " + CantGen[i, 0] + " -> " + GrapDesign(avg) + Math.Round(avg, 2));
                }
            }
        }

        public static string GrapDesign(float avg)
        {
            string graphic = "";

            for (int i = 0; i < (int)avg; i++)
            {
                graphic += "/";
            }
            return graphic;
        }
    }
}