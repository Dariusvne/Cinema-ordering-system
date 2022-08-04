using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ProjectB
{
    internal class OverviewMenu : IStructure
	{
        private int Index = 0;
        private List<api.Button> Buttons;
        private const int OFFSET = 0;

        private string _moviename = Program.information.ChosenFilm.Name;
        private string _date = Program.information.ChosenDate;
        private string _time = Program.information.ChosenTime;
        private int[][] seats = Program.information.ChosenSeats;
        private ReservationsHandler reservationsHandler = new ReservationsHandler();

        private Dictionary<int, List<int>> _seats = new Dictionary<int, List<int>>();
        private int _popcornAmount = 0;

        public OverviewMenu()
        {
            Buttons = api.Button.CreateRow(new string[] { "Go Back", "Confirm" }, 3, 25);

            // Make seats into a dict that holds seats for every row
            foreach (int[] seat in seats)
            {
                if (_seats.ContainsKey(seat[0]))
                {
                    _seats[seat[0]].Add(seat[1]);
                }
                else
                {
                    _seats.Add(seat[0], new List<int>());
                    _seats[seat[0]].Add(seat[1]);
                }
            }
        }

        public void FirstRender()
	{
            api.PrintCenter("Here is the overview. Do you want to continue?", 4);

            api.PrintCenter("Movie", 7, ConsoleColor.White, ConsoleColor.Black);
            api.PrintCenter($"{_moviename}", 8);

            api.PrintCenter("Date and time", 10, ConsoleColor.White, ConsoleColor.Black);
            api.PrintCenter($"{_date}", 11);
            api.PrintCenter($"{_time}", 12);

            api.PrintCenter("Seats", 14, ConsoleColor.White, ConsoleColor.Black);

            int y = 15;
            foreach (var row in _seats.Keys)
            {
                foreach (var seat in _seats[row])
                {
		    if (y < 18)
		    {
			api.PrintCenter($"row {row}, seat {seat}", y);			
		    }
		    else if (y == 18)
		    {
			api.PrintCenter($"more...", y);
		    }
                    y++;
                }
	    }

            DrawButtons();

            string footer = "ARROW KEYS - Change box  |  ENTER - Confirm  |  ESCAPE - Go back";
	    Console.SetCursorPosition((Console.WindowWidth - footer.Length) / 2, 28);
            Console.WriteLine(footer);
        }

	public void DrawButtons()
	{
	    foreach (var button in Buttons)
	    {
                button.Display(Index);
            }

            // calculate price: movieprice*amountofseats + popcornprice*amountofseats
            double seats_price = 11.49 * seats.Length;
            api.PrintExact($"Seats price: {seats_price.ToString("0.00")}$", (Console.WindowWidth-20) / 2, 20, foreground: ConsoleColor.Green);

            double food_price =
                (Program.information.SmallPopcornAmount * Program.prices.PopcornPrices["Small"]) +
                (Program.information.MediumPopcornAmount * Program.prices.PopcornPrices["Medium"]) +
                (Program.information.LargePopcornAmount * Program.prices.PopcornPrices["Large"]) +
                (Program.information.SmallDrinksAmount * Program.prices.DrinksPrices["Small"]) +
                (Program.information.MediumDrinksAmount * Program.prices.DrinksPrices["Medium"]) +
                (Program.information.LargeDrinksAmount * Program.prices.DrinksPrices["Large"]);
            
            api.PrintExact($"Foods price: {food_price.ToString("0.00")}$", (Console.WindowWidth - 20) / 2, 21, foreground: ConsoleColor.Green);
            api.PrintExact($"------------------- +", (Console.WindowWidth - 20) / 2, 22, foreground: ConsoleColor.White);

            double total_price = seats_price + food_price;
            api.PrintExact($"Total price: {total_price.ToString("0.00")}$", (Console.WindowWidth - 20) / 2, 23, foreground: ConsoleColor.Green);
        }

       
        public int Run()
        {
            Console.Clear();
            FirstRender();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                ConsoleKey keyPressed = key.Key;

		if (keyPressed == ConsoleKey.Enter)
		{
		    if (Index == 0)
		    {
                        return -1;
                    }
		    if (Index == 1)
		    {
                        reservationsHandler.Add(Program.information);
                        var seatshandler = new SeatsHandler();
                        seatshandler.Add(Program.information.ChosenFilm.Name, Program.information.ChosenDate, Program.information.ChosenTime, Program.information.ChosenSeats!);
			return 1;
                    }
		}

                if (keyPressed == ConsoleKey.LeftArrow)
                {
                    if (Index > 0)
                    {
                        Index--;
                    }
                }

                if (keyPressed == ConsoleKey.RightArrow)
                {
                    if (Index < Buttons.Count - 1)
                    {
                        Index++;
                    }
                }
                
                DrawButtons();
            }
            while (key.Key != ConsoleKey.Escape);
	    
            return -1;
        }
    }
}
