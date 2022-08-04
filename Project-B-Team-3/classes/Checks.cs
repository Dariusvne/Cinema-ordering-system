using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace ProjectB
{
    // This class will be used to load and hold the list of accounts on the system
    static internal class Checks
    {
        /// <summary>
        /// This function runs all the checks that are made for the system. This includes thing like: Making sure there are
        /// no duplicate movies, dates, times, etc... 
        /// </summary>
        public static void CheckSystem()
        {
            var movies = new MoviesList();
            movies.Load();
            DuplicateMoviesCheck(movies);
            DuplicateDatesCheck(movies);
            DuplicateTimesCheck(movies);
            DateFormatsCheck(movies);
            DateOverdueCheck(movies);
            AutoAddDates(movies);
        }

        private static void DuplicateMoviesCheck(MoviesList movies)
        {
            // Removing duplicate movies from the movies.json file.
            {
                var grouped_movies = movies.Movies.GroupBy(x => x.Name);
                foreach (var movie in grouped_movies)
                {
                    if (movie.Count() > 1)
                    {
                        for (int x = 0; x < movie.Count() - 1; x++)
                        {
                            movies.Remove(movie.Key);
                        }
                    }
                }
            }
        }

        private static void DuplicateDatesCheck(MoviesList movies)
        {
            // Removing duplicate times and dates for movies.
            foreach (var movie in movies.Movies)
            {
                var grouped_dates = movie.Dates.GroupBy(x => x["Date"][0]);
                int movieIndex = movies.Movies.IndexOf(movie);

                foreach (var date in grouped_dates)
                {
                    if (date.Count() > 1)
                    {
                        for (int x = 0; x < date.Count() - 1; x++)
                        {
                            foreach (var dateDict in movies.Movies[movieIndex].Dates)
                            {
                                if (dateDict["Date"][0] == date.Key)
                                {
                                    movies.RemoveDate(movieIndex, movies.Movies[movieIndex].Dates.IndexOf(dateDict), date.Key);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void DuplicateTimesCheck(MoviesList movies)
        {
            for (int movieIndex = 0; movieIndex < movies.Movies.Count; movieIndex++)
            {
                for (int dateIndex = 0; dateIndex < movies.Movies[movieIndex].Dates.Count; dateIndex++)
                {
                    var grouped_times = movies.Movies[movieIndex].Dates[dateIndex]["Time"].GroupBy(x => x);
                                                                                                             
                    foreach (var time in grouped_times)
                    {
                        if (time.Count() <= 1) { continue; }

                        for (int count = 0; count < time.Count() - 1; count++)
                        {
                            movies.RemoveTime(movieIndex, dateIndex, time.Key);
                        }
                    }
                }
            }
        }
        
        private static void DateFormatsCheck(MoviesList movies)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

            for(int i = 0; i < movies.Movies.Count; i++)
            {
                int DateCount = movies.Movies[i].Dates.Count;
                for (int j = 0; j < DateCount; j++)
                {
                    if (check_Time(movies.Movies[i].Dates[j]["Date"][0]) == false)
                    {
                        DateTime date;
                        date = DateTime.Parse(movies.Movies[i].Dates[j]["Date"][0]);
                        movies.Movies[i].Dates[j]["Date"][0] = date.ToString("dd-MM-yyyy");
                    }
                }
                
            }
            movies.Save();
        }

        private static void DateOverdueCheck(MoviesList movies)
        {
            // A check that will make all the passed dates for movies into comming dates.
            for(int i = 0; i < movies.Movies.Count; i++)
            {
                for(int j = movies.Movies[i].Dates.Count - 1; j >= 0; j--)
                {

                    DateTime FilmDate = DateTime.ParseExact(movies.Movies[i].Dates[j]["Date"][0], "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    int check = FilmDate.CompareTo(DateTime.Today);
                    if (check < 0)
                    {
                        movies.RemoveDate(i, j, movies.Movies[i].Dates[j]["Date"][0]);
                    }
                }
            }


        }

        private static void AutoAddDates(MoviesList movies) //if there are no dates in a movie, then they will be automatically generated
        {
            for (int i = 0; i < movies.Movies.Count; i++)
            {
                if (movies.Movies[i].Dates.Count == 0)
                {
                    var DateIndex = 0;
                    var addDays = 14;
                    for(int j = 0; j < 7; j++)
                    {
                        //12:00 | 17:00 | 21:00
                        DateTime today = DateTime.Today;
                        DateTime addedDays = today.AddDays(addDays);
                        var tempDict = new Dictionary<string, List<string>>() { { "Date", new List<string>() }, { "Time", new List<string>() } };

                        movies.Movies[i].Dates.Add(tempDict);
                        movies.Movies[i].Dates[DateIndex]["Date"].Add(addedDays.ToString("dd-MM-yyyy"));

                        //standerd times are added
                        if(DateIndex < 5)
                        {
                            movies.Movies[i].Dates[DateIndex]["Time"].Add("09:00");
                            movies.Movies[i].Dates[DateIndex]["Time"].Add("17:00");
                            movies.Movies[i].Dates[DateIndex]["Time"].Add("21:00");
                        }
                        else
                        {
                            movies.Movies[i].Dates[DateIndex]["Time"].Add("09:00");
                            movies.Movies[i].Dates[DateIndex]["Time"].Add("17:00");
                            movies.Movies[i].Dates[DateIndex]["Time"].Add("21:00");
                            movies.Movies[i].Dates[DateIndex]["Time"].Add("23:00");
                        }
                        DateIndex++;
                        addDays++;
                    }
                }
            }
            movies.Save();
        }
        private static bool check_Time(string readAddMeeting)
        {
            var dateFormats = new[] { "dd-MM-yyyy" };
            DateTime scheduleDate;
            bool validDate = DateTime.TryParseExact(
                readAddMeeting,
                dateFormats,
                DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.None,
                out scheduleDate);


            return validDate;
        }
    }

}
