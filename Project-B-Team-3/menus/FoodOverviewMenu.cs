using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json;

namespace ProjectB
{
    internal class FoodOverviewMenu : IStructure
    {
        private int Index = 0;

        private List<DateTime> _comingDays = new List<DateTime>();
        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> _datesPopcornDrinks= new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
        private List<api.Button> _datesRow = new List<api.Button>();

        private List<api.Component> Components = new List<api.Component>();

        // Testing variables
        private int _totalSmallPopcorn = 32;
        private int _totalMediumPopcorn = 74;
        private int _totalLargePopcorn = 54;

        private int _totalSmallDrinks = 35;
        private int _totalMediumDrinks = 87;
        private int _totalLargeDrinks = 59;
        
        // In the reservations json is stored for each reserveration how much popcorn and
        // how many drinks they have ordered
        private ReservationsHandler reservationsHandler = new ReservationsHandler();

        
        public FoodOverviewMenu()
        {
            /* First figuring out what the date was 7 days ago. When that is found
               we add it to the _lastDays list and do the same for the next 6 days. */
            DateTime initialDate = DateTime.Now;
            foreach (var _ in Enumerable.Range(0, 7))
            {
                _comingDays.Add(initialDate);
                initialDate = initialDate.AddDays(1);
            }

	    /* Time to fill up the _datesPopcornDrinksDrinks dictionary with the last days
	    as strings, holding a the amount of money made that day and the amount of
	    visitors. */
	    foreach (DateTime date in _comingDays)
	    {
                string dateString = String.Format($"{date:dd-MM-yyyy}");
                if (_datesPopcornDrinks.TryAdd(dateString, new Dictionary<string, Dictionary<string, int>>()))
                {
                    string[] snacks = { "Popcorn", "Drinks" };

                    foreach (var snack in snacks)
                    {
                        if (_datesPopcornDrinks[dateString].TryAdd(snack, new Dictionary<string, int>()))
                        {
                            _datesPopcornDrinks[dateString][snack].Add("Small", 0);
                            _datesPopcornDrinks[dateString][snack].Add("Medium", 0);
                            _datesPopcornDrinks[dateString][snack].Add("Large", 0);
                        }
                    }
                }
            }

            var reservationHandler = new ReservationsHandler();
            foreach (var member in reservationHandler.ResevationsDict.Keys)
            {
                foreach (var reservationId in reservationHandler.ResevationsDict[member].Keys)
                {
                    Dictionary<string, string> reservation = reservationHandler.ResevationsDict[member][reservationId];
                    if (_datesPopcornDrinks.ContainsKey(reservation["Date"]))
                    {
                        _datesPopcornDrinks[reservation["Date"]]["Popcorn"]["Small"] += Convert.ToInt32(reservation["SmallPopcornAmount"]);
                        _datesPopcornDrinks[reservation["Date"]]["Popcorn"]["Medium"] += Convert.ToInt32(reservation["MediumPopcornAmount"]);
                        _datesPopcornDrinks[reservation["Date"]]["Popcorn"]["Large"] += Convert.ToInt32(reservation["LargePopcornAmount"]);

                        _datesPopcornDrinks[reservation["Date"]]["Drinks"]["Small"] += Convert.ToInt32(reservation["SmallDrinksAmount"]);
                        _datesPopcornDrinks[reservation["Date"]]["Drinks"]["Medium"] += Convert.ToInt32(reservation["MediumDrinksAmount"]);
                        _datesPopcornDrinks[reservation["Date"]]["Drinks"]["Large"] += Convert.ToInt32(reservation["LargeDrinksAmount"]);
                    }
                }
            }

            _datesRow = api.Button.CreateRow(_datesPopcornDrinks.Keys.ToArray(), 1, 8, Int32.MinValue);
        }

        public void FirstRender()
        {
            api.PrintCenter("Here is an overview of all the ordered snacks!", 5);
            
            bool first = true;                
            foreach (var date in _datesRow)
            {
                int y = 2;
                date.Display(date.Index);
                foreach (string snack in _datesPopcornDrinks[date.Title].Keys)
                {
                    api.PrintExact(snack, date.X+1, date.Y + y, ConsoleColor.White, ConsoleColor.Black);
                    y++;
                    foreach (string size in _datesPopcornDrinks[date.Title][snack].Keys)
                    {
                        api.PrintExact($"{_datesPopcornDrinks[date.Title][snack][size]}", date.X + 1, date.Y + y, foreground: ConsoleColor.Cyan);
                        if (first)
                        {
                            api.PrintExact(size, date.X - 10, date.Y + y, foreground: ConsoleColor.DarkCyan);
                        }
                        y++;
                    }
                    
                    y++;
                }
                if (first)
                {
                    first = false;
                }
            }

            
            DrawButtons();

            string footer = "ESCAPE - Go back";
	    Console.SetCursorPosition((Console.WindowWidth - footer.Length) / 2, 28);
            Console.WriteLine(footer);
        }

	    public void DrawButtons()
	    {
	        foreach (var component in Components)
	        {
                    if (Index == 1)
                    {
                        component.Display(Index);
                    }
		    else
		    {
                        component.Display(-1);
                    }
                }
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
                    return -1;
                    DrawButtons();
                }
            }
            while (key.Key != ConsoleKey.Escape);
	    
            return -1;
        }
    }
}
