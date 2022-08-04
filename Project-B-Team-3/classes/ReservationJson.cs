using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectB
{

    /// <summary>
    /// This class handles anything related to storing picked seats and accessing them
    /// </summary>
    internal class ReservationsHandler
    {
        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> _reservationsDict;
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> ResevationsDict
        {
            get => _reservationsDict;
        }

        private string SeatsJsonName = @$"Data{Path.DirectorySeparatorChar}Reservations.json";

        public ReservationsHandler()
        {
            if (File.Exists(SeatsJsonName))
            {
                try
                {
                    Load();
                }
                catch (Exception err)
                {
                    Console.WriteLine($"Error while trying to load json file: {err.Message}");
                    Console.WriteLine("Press any key to continue.....");
                    Console.Read();
                    _reservationsDict = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
                }
            }
            else
            {
                File.Create(SeatsJsonName);
                _reservationsDict = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
            }
        }

        /// <summary>
        /// Adds all the information of the reservation to the json
        /// </summary>
        /// <param name="info">The information object in Program. All the information will be taken from this</param>
	/// <param name="popcornAmount">The popcorn amount of </param>
        public void Add(Program.Information info)
        {
            string ID = Guid.NewGuid().ToString();

	    // Add the user's email to the dictionary
            if (!_reservationsDict.ContainsKey(info.Member!.Email))
            {
                _reservationsDict.Add(info.Member!.Email, new Dictionary<string, Dictionary<string, string>>());
            }

	    // Make an ID until it is unique and add it to the emails dictionary
            do
            {
                ID = Guid.NewGuid().ToString();
            } 
            while (_reservationsDict[info.Member.Email].ContainsKey(ID));
            _reservationsDict[info.Member.Email].Add(ID, new Dictionary<string, string>());

            // Add all the useful information to the
            int[][] seats = info.ChosenSeats;
            string SeatsStr = $"R{seats.First()[0]}S{seats.First()[1]}";
            foreach (int[] seat in seats)
            {
                if (seat != seats.First())
                {
                    SeatsStr += $"|R{seat[0]}S{seat[1]}";
                }
            }

            _reservationsDict[info.Member.Email][ID].Add("MovieName", info.ChosenFilm.Name);
            _reservationsDict[info.Member.Email][ID].Add("Date", info.ChosenDate);
            _reservationsDict[info.Member.Email][ID].Add("Time", info.ChosenTime);
            _reservationsDict[info.Member.Email][ID].Add("Seats", SeatsStr);
            _reservationsDict[info.Member.Email][ID].Add("SmallPopcornAmount", info.SmallPopcornAmount.ToString());
            _reservationsDict[info.Member.Email][ID].Add("MediumPopcornAmount", info.MediumPopcornAmount.ToString());
            _reservationsDict[info.Member.Email][ID].Add("LargePopcornAmount", info.LargePopcornAmount.ToString());
            _reservationsDict[info.Member.Email][ID].Add("SmallDrinksAmount", info.SmallDrinksAmount.ToString());
            _reservationsDict[info.Member.Email][ID].Add("MediumDrinksAmount", info.MediumDrinksAmount.ToString());
            _reservationsDict[info.Member.Email][ID].Add("LargeDrinksAmount", info.LargeDrinksAmount.ToString());

            Save();
        }

	// /// <summary>
	// /// Removes given indexes from the picked seats in the json and in the seats dictionary
	// /// </summary>
        // /// <param name="moviename">The movie name that the seats are selected for</param>
        // /// <param name="date">What date the seats are picked for</param>
        // /// <param name="time">The time that the seats were picked for</param>
        // /// <param name="chosen_seats">An array of int arrays that hold the row and seat of the seats that want to be removed</param>
        // public void Remove(string moviename, string date, string time, int[][] removing_seats)
        // {
	//     // Turning the int[][] into a list and then using its Remove method to
	//     // remove the given seats if they exist
        //     List<int[]> currently_picked = _seatsDict[moviename][date][time].ToList();
        //     foreach (int[] seat in removing_seats) {
	// 	if (currently_picked.Contains(seat)) {
        //             currently_picked.Remove(seat);
        //         }
        //     }

	//     // Turning the new list into an int[][] array and setting it as the
	//     // new picked seats. After that is instantly saved in the json
        //     _seatsDict[moviename][date][time] = currently_picked.ToArray();
        //     Save();
        // }

	// /// <summary>
	// /// Returns the dictionary for READ PURPOSES ONLY. Only add and remove seats via
	// /// the methods Remove, and Add!
	// /// </summary>
        // public Dictionary<string, Dictionary<string, Dictionary<string, int[][]>>> GetDict()
        // {
        //     return _seatsDict;
        // }

        /// <summary>
        /// Loads what is currently stored in the json file into the SeatsDict dictionary
        /// </summary>
        private void Load()
        {
            string json = File.ReadAllText(SeatsJsonName);
            _reservationsDict = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(json);
        }

        /// <summary>
	/// Saves any changes to the SeatsDict variable to the json file
	/// </summary>
        private void Save()
        {
	    var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            File.WriteAllText(SeatsJsonName, JsonSerializer.Serialize(_reservationsDict, options: options));
        }
        public void RemoveReservation(string email, string orderId)
        {
            ResevationsDict[email].Remove(orderId);
            Save();
        }
    }

}
