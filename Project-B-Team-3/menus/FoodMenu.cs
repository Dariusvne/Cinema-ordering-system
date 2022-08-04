namespace ProjectB
{
    internal class FoodMenu : IStructure
    {
        private int Index = 0;
        private List<api.Component> Components = new List<api.Component>();
        private const int OFFSET = 0;

        private int Popcorn_Small = 0;
        private int Popcorn_Medium = 0;
        private int Popcorn_Large = 0;
        private int Drinks_Small = 0;
        private int Drinks_Medium = 0;
        private int Drinks_Large = 0;

        private string Foodasccii = @"
 ______              _
|  ____|            | |
| |__ ___   ___   __| |
|  __/ _ \ / _ \ / _` |
| | | (_) | (_) | (_| |
|_|  \___/ \___/ \__,_|
";

        public FoodMenu()
        {
            //Components.Add(new api.Button("Do you want a small popcorn?",0, 1, 3));
            //Components.Add(new api.Button("Do you want a Medium popcorn?", 0, 1, 6));
        }

        public void FirstRender()
        {
            api.PrintCenter("Do you want food with your reservation?", 3);

            api.PrintCenter($"Small Popcorn:  {Program.prices.PopcornPrices["Small"].ToString("0.00")}$", 6, ConsoleColor.White, ConsoleColor.Black);
            api.PrintCenter($"Medium Popcorn: {Program.prices.PopcornPrices["Medium"].ToString("0.00")}$", 9, ConsoleColor.White, ConsoleColor.Black);
            api.PrintCenter($"Large Popcorn: {Program.prices.PopcornPrices["Large"].ToString("0.00")}$", 12, ConsoleColor.White, ConsoleColor.Black);

            api.PrintCenter($"Small Drink: {Program.prices.DrinksPrices["Small"].ToString("0.00")}$", 15, ConsoleColor.White, ConsoleColor.Black);
            api.PrintCenter($"Medium Drink: {Program.prices.DrinksPrices["Medium"].ToString("0.00")}$", 18, ConsoleColor.White, ConsoleColor.Black);
            api.PrintCenter($"Large Drink: {Program.prices.DrinksPrices["Large"].ToString("0.00")}$", 21, ConsoleColor.White, ConsoleColor.Black);

            DrawButtons();

            string footer = "ARROW KEYS - Change box  |  ENTER - Confirm  |  ESCAPE - Go back";
            Console.SetCursorPosition((Console.WindowWidth - footer.Length) / 2, 28);
            Console.WriteLine(footer);
        }

        public void DrawButtons()
        {
            foreach (var button in Components)
            {
                button.Display(Index);
            }

            // Print Popcorn_Small
            if (Index != 0)
            {
                api.PrintCenter($"{Popcorn_Small}".PadLeft(3).PadRight(5), 7, background: ConsoleColor.DarkGray, foreground: ConsoleColor.Black);
            }
            else
            {
                api.PrintCenter($"{Popcorn_Small}".PadLeft(3).PadRight(5), 7, background: ConsoleColor.Gray, foreground: ConsoleColor.Black);
            }
            
            // Print Popcorn_Medium
            if (Index != 1)
            {
                api.PrintCenter($"{Popcorn_Medium}".PadLeft(3).PadRight(5), 10, background: ConsoleColor.DarkGray, foreground: ConsoleColor.Black);
            }
            else
            {
                api.PrintCenter($"{Popcorn_Medium}".PadLeft(3).PadRight(5), 10, background: ConsoleColor.Gray, foreground: ConsoleColor.Black);
            }
            
            // Print Popcorn_Large
            if (Index != 2)
            {
                api.PrintCenter($"{Popcorn_Large}".PadLeft(3).PadRight(5), 13, background: ConsoleColor.DarkGray, foreground: ConsoleColor.Black);
            }
            else
            {
                api.PrintCenter($"{Popcorn_Large}".PadLeft(3).PadRight(5), 13, background: ConsoleColor.Gray, foreground: ConsoleColor.Black);
            }
            
            // Print Drinks_Small
            if (Index != 3)
            {
                api.PrintCenter($"{Drinks_Small}".PadLeft(3).PadRight(5), 16, background: ConsoleColor.DarkGray, foreground: ConsoleColor.Black);
            }
            else
            {
                api.PrintCenter($"{Drinks_Small}".PadLeft(3).PadRight(5), 16, background: ConsoleColor.Gray, foreground: ConsoleColor.Black);
            }
            
            // Print Drinks_Medium
            if (Index != 4)
            {
                api.PrintCenter($"{Drinks_Medium}".PadLeft(3).PadRight(5), 19, background: ConsoleColor.DarkGray, foreground: ConsoleColor.Black);
            }
            else
            {
                api.PrintCenter($"{Drinks_Medium}".PadLeft(3).PadRight(5), 19, background: ConsoleColor.Gray, foreground: ConsoleColor.Black);
            }
            
            // Print Drinks_Large
            if (Index != 5)
            {
                api.PrintCenter($"{Drinks_Large}".PadLeft(3).PadRight(5), 22, background: ConsoleColor.DarkGray, foreground: ConsoleColor.Black);
            }
            else
            {
                api.PrintCenter($"{Drinks_Large}".PadLeft(3).PadRight(5), 22, background: ConsoleColor.Gray, foreground: ConsoleColor.Black);
            }

            //arrows Popcorn_Small
            api.PrintExact("<", Console.WindowWidth / 2 - 5, 7);
            api.PrintExact(">", Console.WindowWidth / 2 + 3, 7);

            //arrows Popcorn_Medium
            api.PrintExact("<", Console.WindowWidth / 2 - 5, 10);
            api.PrintExact(">", Console.WindowWidth / 2 + 3, 10);

            //arrows Popcorn_Large
            api.PrintExact("<", Console.WindowWidth / 2 - 5, 13);
            api.PrintExact(">", Console.WindowWidth / 2 + 3, 13);

            //arrows Drinks_Small
            api.PrintExact("<", Console.WindowWidth / 2 - 5, 16);
            api.PrintExact(">", Console.WindowWidth / 2 + 3, 16);

            //arrows Drinks_Medium
            api.PrintExact("<", Console.WindowWidth / 2 - 5, 19);
            api.PrintExact(">", Console.WindowWidth / 2 + 3, 19);

            //arrows Drinks_Large
            api.PrintExact("<", Console.WindowWidth / 2 - 5, 22);
            api.PrintExact(">", Console.WindowWidth / 2 + 3, 22);

            // calculate price: movieprice*amountofseats + popcornprice*amountofseats
            double total_price =
                (Popcorn_Small * Program.prices.PopcornPrices["Small"]) +
                (Popcorn_Medium * Program.prices.PopcornPrices["Medium"]) +
                (Popcorn_Large * Program.prices.PopcornPrices["Large"]) +
                (Drinks_Small * Program.prices.DrinksPrices["Small"]) +
                (Drinks_Medium * Program.prices.DrinksPrices["Medium"]) +
                (Drinks_Large * Program.prices.DrinksPrices["Large"]);
            
            api.PrintCenter($"    Total price: {total_price.ToString("0.00")}$    ", 24, foreground: ConsoleColor.Green);
        }

        public int Run()
        {
            Console.Clear();
            FirstRender();
            ConsoleKeyInfo key;
            
            do
            {
                var info = Program.information;
                key = Console.ReadKey(true);
                ConsoleKey keyPressed = key.Key;

                if (keyPressed == ConsoleKey.Enter)
                {
                    info.SmallPopcornAmount = Popcorn_Small;
                    info.MediumPopcornAmount = Popcorn_Medium;
                    info.LargePopcornAmount = Popcorn_Large;

                    info.SmallDrinksAmount = Drinks_Small;
                    info.MediumDrinksAmount = Drinks_Medium;
                    info.LargeDrinksAmount = Drinks_Large;

                    Program.information = info;

                    return 1;
                }

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    if (Index > 0)
                    {
                        Index--;
                    }
                }
                if (keyPressed == ConsoleKey.DownArrow)
                {
                    if (Index < 5)
                    {
                        Index++;
                    }
                }

                if (keyPressed == ConsoleKey.RightArrow)
                {
                    if (Index == 0)
                    {
                        if (Popcorn_Small < 100)
                        {
                            Popcorn_Small++;
                        }
                    }
                    if (Index == 1)
                    {
                        if (Popcorn_Medium < 100)
                        {
                            Popcorn_Medium++;
                        }
                    }
                    if (Index == 2)
                    {
                        if (Popcorn_Large < 100)
                        {
                            Popcorn_Large++;
                        }
                    }
                    if (Index == 3)
                    {
                        if (Drinks_Small < 100)
                        {
                            Drinks_Small++;
                        }
                    }
                    if (Index == 4)
                    {
                        if (Drinks_Medium < 100)
                        {
                            Drinks_Medium++;
                        }
                    }
                    if (Index == 5)
                    {
                        if (Drinks_Large < 100)
                        {
                            Drinks_Large++;
                        }
                    }
                }
                else if (keyPressed == ConsoleKey.LeftArrow)
                {
                    if (Index == 0)
                    {
                        if (Popcorn_Small > 0)
                        {
                            Popcorn_Small--;
                        }
                    }
                    if (Index == 1)
                    {
                        if (Popcorn_Medium > 0)
                        {
                            Popcorn_Medium--;
                        }
                    }
                    if (Index == 2)
                    {
                        if (Popcorn_Large > 0)
                        {
                            Popcorn_Large--;
                        }
                    }
                    if (Index == 3)
                    {
                        if (Drinks_Small > 0)
                        {
                            Drinks_Small--;
                        }
                    }
                    if (Index == 3)
                    {
                        if (Drinks_Small > 0)
                        {
                            Drinks_Small--;
                        }
                    }
                    if (Index == 4)
                    {
                        if (Drinks_Medium > 0)
                        {
                            Drinks_Medium--;
                        }
                    }
                    if (Index == 5)
                    {
                        if (Drinks_Large > 0)
                        {
                            Drinks_Large--;
                        }
                    }
                }
                DrawButtons();
            }
            while (key.Key != ConsoleKey.Escape);

            return -1;
        }
    }
}
