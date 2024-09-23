namespace Reaktor
{
    internal class Program
    {
        static Random rnd = new Random();
        static int temperature = rnd.Next(40, 100);
        static double energy = rnd.Next(1, 5) + rnd.NextDouble();

        static void StartReactor()
        {
            Console.WriteLine("Nyomj meg egy gombot a reaktor indításához!");
            Console.ReadKey();
            Console.Clear();

            Console.Write("Reaktor indítása");
            Thread.Sleep(1000);

            for (int i = 0; i < 3; i++)
            {
                Console.Write('.');
                Thread.Sleep(1000);
            }

            DisplayReactor(temperature, energy);
        }

        static void DisplayReactor(int temperature, double energy)
        {
            Console.Clear();

            Console.WriteLine(" ---------------------");
            Console.WriteLine("|REAKTOR MŰKÖDÉS ALATT |");
            Console.WriteLine(" ---------------------\n\n");

            Console.Write($"Reaktor jelenlegi hőmérséklete:");

            if (temperature < 70)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            else if (temperature >= 70 && temperature < 90)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }

            else if (temperature >= 90)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine($" {temperature}°C");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Reaktor által generált energia: {Math.Round(energy, 2)} GW");

            Console.WriteLine("\nReaktor hűtése (1)");
            Console.WriteLine("Reaktor leállítása (2)");
        }

        static void CoolReactor(int temperature, double energy)
        {
            Console.Clear();

            Console.WriteLine(" ------------------------------");
            Console.WriteLine("|REAKTOR HŰTÉSE FOLYAMATBAN VAN|");
            Console.WriteLine(" ------------------------------\n\n");

            Console.Write($"Reaktor jelenlegi hőmérséklete:");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($" {temperature}°C");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Reaktor által generált energia: {Math.Round(energy, 2)} GW");
        }

        static void Main(string[] args)
        {
            bool startCooling = false;
            bool stopReactor = false;

            Thread keyMonitor = new Thread(() =>
            {
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKey pressedKey = Console.ReadKey().Key;

                        if (pressedKey == ConsoleKey.D1)
                        {
                            startCooling = true;
                        }
                        else if (pressedKey == ConsoleKey.D2)
                        {
                            stopReactor = true;
                        }
                    }
                }
            });

            StartReactor();
            keyMonitor.Start();

            for (int i = temperature; i < 100; i++)
            {

                if (startCooling)
                {
                    while (temperature > 40)
                    {
                        if (temperature - 10 <= 40)
                        {
                            temperature = 40;
                            i = temperature;
                            CoolReactor(temperature, energy);
                            Thread.Sleep(1000);
                        }

                        else
                        {
                            temperature -= 10;
                            CoolReactor(temperature, energy);
                            Thread.Sleep(1000);
                        }
                    }
                    startCooling = false;
                    stopReactor = false;
                }

                else if (stopReactor)
                {
                    if (temperature < 70)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("A reaktor sikeresen leállt.");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(1000);
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\nA reaktor leállításához hűtés szükséges!");
                        Console.ForegroundColor = ConsoleColor.White;
                        stopReactor = false;
                        Thread.Sleep(1000);
                    }
                }

                DisplayReactor(temperature++, energy += rnd.NextDouble());
                Thread.Sleep(1000);
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" ---------------------");
            Console.WriteLine("|A reaktor felrobbant!|");
            Console.WriteLine(" ---------------------");
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(0);
        }
    }
}
