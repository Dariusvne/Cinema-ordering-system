using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    internal class EarningsMenu : IStructure
    {
	/* Initializing a new DateTime list that will hold the last 7 dates to pick
	the statistics from. */
        private List<DateTime> _lastDays = new List<DateTime>();
	    private Dictionary<string, Dictionary<string, double>> _datesEarningsVisitors = new Dictionary<string, Dictionary<string, double>>();
        private List<api.Button> _datesRow;

        public EarningsMenu()
        {
            /* First figuring out what the date was 7 days ago. When that is found
	    we add it to the _lastDays list and do the same for the next 6 days. */
            DateTime initialDate = DateTime.Now.AddDays(-7);
            foreach (var _ in Enumerable.Range(0, 7))
            {
                _lastDays.Add(initialDate);
                initialDate = initialDate.AddDays(1);
            }

	    /* Time to fill up the _datesEarningsVisitors dictionary with the last days
	    as strings, holding a the amount of money made that day and the amount of
	    visitors. */
            foreach (DateTime date in _lastDays)
            {
                string dateString = String.Format($"{date:dd-MM-yyyy}");
                if (_datesEarningsVisitors.TryAdd(dateString, new Dictionary<string, double>()))
                {
                    _datesEarningsVisitors[dateString].Add("Earnings", 0.0);
                    _datesEarningsVisitors[dateString].Add("Visitors", 0.0);
                }
            }

            /* Now we can loop through all the reservations that have been made and check if the
            reservation's date is one of the dates in our last 7 days. If so, we add its amount
            of seats to the visitors count, and calculate the price based on the amount of popcorn
            and seats that have been reserved. Like this we only have to iterate over the reservations
            once */
            var reservationHandler = new ReservationsHandler();
            foreach (var member in reservationHandler.ResevationsDict.Keys)
            {
                foreach (var reservationId in reservationHandler.ResevationsDict[member].Keys)
                {
                    Dictionary<string, string> reservation = reservationHandler.ResevationsDict[member][reservationId];
                    if (_datesEarningsVisitors.ContainsKey(reservation["Date"]))
                    {
                        string[] seats = reservation["Seats"].Split('|');
                        _datesEarningsVisitors[reservation["Date"]]["Visitors"] += seats.Length;
                        
                        _datesEarningsVisitors[reservation["Date"]]["Earnings"] += (seats.Length * 11.49) +
                            (Convert.ToInt32(reservation["SmallPopcornAmount"]) * Program.prices.PopcornPrices["Small"]) +
                            (Convert.ToInt32(reservation["MediumPopcornAmount"]) * Program.prices.PopcornPrices["Medium"]) +
                            (Convert.ToInt32(reservation["LargePopcornAmount"]) * Program.prices.PopcornPrices["Large"]) +
                            (Convert.ToInt32(reservation["SmallDrinksAmount"]) * Program.prices.DrinksPrices["Small"]) +
                            (Convert.ToInt32(reservation["MediumDrinksAmount"]) * Program.prices.DrinksPrices["Medium"]) +
                            (Convert.ToInt32(reservation["LargeDrinksAmount"]) * Program.prices.DrinksPrices["Large"]);
                    }
                }
            }

            _datesRow = api.Button.CreateRow(_datesEarningsVisitors.Keys.ToArray(), 2, 8, Int32.MinValue);
        }

        public void FirstRender()
        {
            api.PrintCenter(" Overview of last 7 days ", 5, ConsoleColor.White, ConsoleColor.Black);

            /* Display a table of the last 7 days, and fill them with the data that has been collected
	       about the amount of visitors and revenue. */
            foreach (var date in _datesRow)
            {
                date.Display(date.Index);
                api.PrintExact("Earnings", date.X+1, date.Y + 2, ConsoleColor.White, ConsoleColor.Black);
                api.PrintExact($"{_datesEarningsVisitors[date.Title]["Earnings"].ToString("0.00")}$", date.X + 1, date.Y + 3, foreground: ConsoleColor.Green);

                api.PrintExact("Visitors", date.X+1, date.Y + 5, ConsoleColor.White, ConsoleColor.Black);
                api.PrintExact($"{_datesEarningsVisitors[date.Title]["Visitors"].ToString("0")}", date.X + 1, date.Y + 6, foreground: ConsoleColor.Cyan);
            }

            /* Display the total amount of visitors and revenue of the last 7 days. We go through all the dates and
               add all they stats to a totalVisitors and totalEarnings counter.*/
            api.PrintCenter(" Total of last 7 days ", 18, ConsoleColor.White, ConsoleColor.Black);
            double totalVisitors = 0;
            double totalEarnings = 0;
            foreach (var date in _datesEarningsVisitors.Keys)
            {
                totalVisitors += _datesEarningsVisitors[date]["Visitors"];
                totalEarnings += _datesEarningsVisitors[date]["Earnings"];
            }
            api.PrintCenter("Total earnings", 20, ConsoleColor.White, ConsoleColor.Black);
            api.PrintCenter($"{totalEarnings}$", 21, foreground: ConsoleColor.Green);

            api.PrintCenter("Total visitors", 23, ConsoleColor.White, ConsoleColor.Black);
            api.PrintCenter($"{totalVisitors}", 24, foreground: ConsoleColor.Cyan);

            string footer = "ESCAPE - Exit";
            Console.SetCursorPosition((Console.WindowWidth - footer.Length) / 2, 28);
            Console.WriteLine(footer);
        }
	

        public int Run()
        {
            Console.Clear();
            FirstRender();
	   
            do
            {
                ConsoleKey keyPressed = Console.ReadKey(true).Key;
		
                if (keyPressed == ConsoleKey.Escape)
                {
                    return -1;
                }
            } while (true);
        }

    }
}
