using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    internal class SettingsMenu : IStructure
    {
        private int Index = 0;
        private List<api.Component> Components = new List<api.Component>();

        private Dictionary<string, Double> _temporaryDrinksPrices = Program.prices.DrinksPrices.GetDict();
        private Dictionary<string, Double> _temporaryPopcornPrices = Program.prices.PopcornPrices.GetDict();

        private bool _coronaCheck = Program.settings.CoronaCheck;

        public SettingsMenu()
        {
            Components.Add(new api.Button("Toggle Corona", 0, (Console.WindowWidth - 3) / 2, 10));

            // Popcorn prices sliders
            Components.Add(new api.DecimalSlider(1, (Console.WindowWidth + 15) / 2, 12, 99, _temporaryPopcornPrices["Small"]));
            Components.Add(new api.DecimalSlider(2, (Console.WindowWidth + 15) / 2, 13, 99, _temporaryPopcornPrices["Medium"]));
            Components.Add(new api.DecimalSlider(3, (Console.WindowWidth + 15) / 2, 14, 99, _temporaryPopcornPrices["Large"]));

            // Drinks prices sliders
            Components.Add(new api.DecimalSlider(4, (Console.WindowWidth + 15) / 2, 16, 99, _temporaryDrinksPrices["Small"]));
            Components.Add(new api.DecimalSlider(5, (Console.WindowWidth + 15) / 2, 17, 99, _temporaryDrinksPrices["Medium"]));
            Components.Add(new api.DecimalSlider(6, (Console.WindowWidth + 15) / 2, 18, 99, _temporaryDrinksPrices["Large"]));

            Components.Add(new api.Button("Confirm", 7, (Console.WindowWidth - 9) / 2, 20));
        }


        public void FirstRender()
        {
            api.PrintCenter("Here are all the settings for the system", 4);

            api.PrintExact("Small Popcorn Price:", (Console.WindowWidth - 33) / 2, 12);
            api.PrintExact("Medium Popcorn Price:", (Console.WindowWidth - 35) / 2, 13);
            api.PrintExact("Large Popcorn Price:", (Console.WindowWidth - 33) / 2, 14);

            api.PrintExact("Small Drinks Price:", (Console.WindowWidth - 31) / 2, 16);
            api.PrintExact("Medium Drinks Price:", (Console.WindowWidth - 33) / 2, 17);
            api.PrintExact("Large Drinks Price:", (Console.WindowWidth - 31) / 2, 18);

            DrawButtons();
            
            string footer = "ARROW KEYS - select options  | ESCAPE - Cancel all changes | ENTER - Confirm ";
            Console.SetCursorPosition((Console.WindowWidth - footer.Length) / 2, 28);
            Console.WriteLine(footer);
        }
        

        private void DrawButtons()
        {
            foreach (var Component in Components)
            {
                Component.Display(Index);
            }

            if (_coronaCheck == true)
            {
                api.PrintExact("       ", (Console.WindowWidth - 22) / 2, 10);
                api.PrintExact("  ON  ", (Console.WindowWidth - 22) / 2, 10, ConsoleColor.Green, ConsoleColor.White);
            }
            else
            {
                api.PrintExact("  OFF  ", (Console.WindowWidth - 22) / 2, 10, ConsoleColor.Red, ConsoleColor.White);
            }
        }

        public int Run()
        {
            Console.Clear();
            FirstRender();
            ConsoleKey keyPressed;
            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.Enter)
                {
                    if (Index == 0)
                    {
                        _coronaCheck = _coronaCheck ? false : true;
                    }

                    if (Index == 7)
                    {
                        if (Program.settings.CoronaCheck != _coronaCheck) { Program.settings = Program.settings.ToggleCoronaCheck(); }

                        Program.prices.PopcornPrices["Small"] = (Components[1] as api.DecimalSlider).Amount;
                        Program.prices.PopcornPrices["Medium"] = (Components[2] as api.DecimalSlider).Amount;
                        Program.prices.PopcornPrices["Large"] = (Components[3] as api.DecimalSlider).Amount;
                        
                        Program.prices.DrinksPrices["Small"] = (Components[4] as api.DecimalSlider).Amount;;
                        Program.prices.DrinksPrices["Medium"] = (Components[5] as api.DecimalSlider).Amount;
                        Program.prices.DrinksPrices["Large"] = (Components[6] as api.DecimalSlider).Amount;

                        return -1;
                    }
                }

                if (keyPressed == ConsoleKey.DownArrow)
                {
                    Index++;
                    if (Index > Components.Count - 1)
                    {
                        Index = 0;
                    }
                }
                
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    Index--;
                    if (Index < 0)
                    {
                        Index = Components.Count - 1;
                    }
                }

                if (keyPressed == ConsoleKey.LeftArrow)
                {
                    if (Components[Index].GetType() == typeof(api.DecimalSlider))
                    {
                        (Components[Index] as api.DecimalSlider)!.MinusOne();
                    }
                }

                if (keyPressed == ConsoleKey.RightArrow)
                {
                    if (Components[Index].GetType() == typeof(api.DecimalSlider))
                    {
                        (Components[Index] as api.DecimalSlider)!.PlusOne();
                    }
                }
                DrawButtons();

            } while (keyPressed != ConsoleKey.Escape);

            return -1;
        }
    }
}
