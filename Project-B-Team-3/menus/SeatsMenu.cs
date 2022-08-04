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
    internal class SeatsMenu : IStructure
    {
        private int[] Index;
        private int ButtonIndex;
        private string[][] TakenSeats;
        private List<api.Button> Buttons = new List<api.Button>();
        private SeatsHandler seatshandler = new SeatsHandler();

        private const int ROWS = 5;
        private const int AMOUNT = 10;

        public SeatsMenu()
	{
            Index = new int[] { ROWS/2, AMOUNT/2 };
            ButtonIndex = 1;
            Buttons.Add(new api.Button("Go Back", ROWS * AMOUNT, Console.WindowWidth / 2 - 17, 21));
            Buttons.Add(new api.Button("Confirm", ROWS * AMOUNT+1, Console.WindowWidth / 2 + 8, 21));

            TakenSeats = new string[ROWS][];
            for (int rownumber = 0; rownumber < ROWS; rownumber++)
            {
                TakenSeats[rownumber] = new string[AMOUNT];
            }

            if (Program.information.ChosenSeats != null)
            {
                foreach (int[] picked_seat in Program.information.ChosenSeats)
                {
                    TakenSeats[picked_seat[0]][picked_seat[1]] = "chosen";
                }
            }
            
            if (seatshandler.SeatsDict.ContainsKey(Program.information.ChosenFilm.Name)){
                if (seatshandler.SeatsDict[Program.information.ChosenFilm.Name].ContainsKey(Program.information.ChosenDate)) {
                    if (seatshandler.SeatsDict[Program.information.ChosenFilm.Name][Program.information.ChosenDate].ContainsKey(Program.information.ChosenTime)) {
                        foreach (int[] already_picked_seat in seatshandler.SeatsDict[Program.information.ChosenFilm.Name][Program.information.ChosenDate][Program.information.ChosenTime])
                        {
                            TakenSeats[already_picked_seat[0]][already_picked_seat[1]] = "taken";
			    if (Program.settings.CoronaCheck)
			    {
                                if (already_picked_seat[0] > 0)
                                {
                                    if (TakenSeats[already_picked_seat[0] - 1][already_picked_seat[1]] != "taken")
                                    {
                                        TakenSeats[already_picked_seat[0] - 1][already_picked_seat[1]] = "taken";
                                    }
                                }
                                if (already_picked_seat[0] < ROWS-1)
                                {
                                    if (TakenSeats[already_picked_seat[0] + 1][already_picked_seat[1]] != "taken")
                                    {
                                        TakenSeats[already_picked_seat[0] + 1][already_picked_seat[1]] = "taken";
                                    }
                                }
				if (already_picked_seat[1] > 0)
				{
				    if (TakenSeats[already_picked_seat[0]][already_picked_seat[1] - 1] != "taken")
				    {
                                        TakenSeats[already_picked_seat[0]][already_picked_seat[1] - 1] = "taken";
                                    }
				}
                                if (already_picked_seat[1] < AMOUNT-1)
                                {
                                    if (TakenSeats[already_picked_seat[0]][already_picked_seat[1] + 1] != "taken")
                                    {
                                        TakenSeats[already_picked_seat[0]][already_picked_seat[1] + 1] = "taken";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void FirstRender()
	    {
            api.PrintCenter(Program.information.ChosenTime, 5);
            api.PrintCenter(Program.information.ChosenDate, 6);
            api.PrintCenter("  screen  ", 20, background: ConsoleColor.White, foreground: ConsoleColor.Black);

            DrawSeats();
            DrawButtons();

            api.PrintCenter("Go to bottom to confirm or go back", 27);
            string footer = "ARROW KEYS - Change Position  |  ENTER - Select Chair  |  ESCAPE - Go back" ;
            Console.SetCursorPosition((Console.WindowWidth - footer.Length) / 2, 28);
            Console.WriteLine(footer);
        }

        private void DrawSeats()
        {
	    for (int x = 0; x < ROWS; x++)
            {
		Console.SetCursorPosition(Console.WindowWidth / 2 - "                                       ".Length / 2, 10+x*2);
                for (int i = 0; i < AMOUNT; i++)
                {
                    DrawSeat(TakenSeats[x][i], Index[0] == x && Index[1] == i);
                    Console.Write(' ');
                }
                Console.Write("\n\n");
            }
        }

        private void DrawButtons()
        {
	    foreach (api.Button button in Buttons)
            {
                button.Display(Index[0] * AMOUNT + ButtonIndex);
            }
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

		// This piece handles turning a free chair into a chosen chair when
		// the user pressed enter and is not on one of the buttons. Also makes
		// the chair free if it was chosen by the user before. It also handles
		// what to return when enter is pressed on one of the buttons
                if (keyPressed == ConsoleKey.Enter)
                {
		    if (Index[0] >= ROWS)
		    {
                        if (ButtonIndex == 0)
                        {
                            var info = Program.information;
                            info.ChosenSeats = null;
                            Program.information = info;

                            return 0; // Go back to the movie time screen
                        }
			else if (ButtonIndex == 1)
                        {
			    // Making a list of indexes of all the seats that were
			    // chosen by the user
                            List<int[]> ChosenSeatsIndexes = new List<int[]>();
                            for (int row = 0; row < ROWS; row++)
                            {
                                for (int seat = 0; seat < AMOUNT; seat++)
                                {
                                    if (TakenSeats[row][seat] == "chosen")
                                    {
                                        ChosenSeatsIndexes.Add(new int[] { row, seat });
                                    }
                                }
                            }
			    // If the list contains a seat than resume, otherwise throw as
			    // error message telling the user the have to select a seat
                            if (ChosenSeatsIndexes.Count > 0)
                            {
                                if (Program.information.Member == null)
                                {
                                    var loginmenu = new LoginMenu(false);
                                    loginmenu.Run();
                                }

				if (Program.information.Member != null)
				{
                                    //seatshandler.Add(Program.information.ChosenFilm.Name, Program.information.ChosenDate, Program.information.ChosenTime, ChosenSeatsIndexes.ToArray());
                                    var info = Program.information;
                                    info.ChosenSeats = ChosenSeatsIndexes.ToArray();
                                    Program.information = info;
                                    return 1; // Go on to overwiew screen | or login screen if not logged in
				}
                            }
                            else
                            {
                                api.PrintCenter("        You haven't select any seats        ", 8, foreground: ConsoleColor.DarkRed);
                            }
                        }
                    }
                    else
                    {
                        if (TakenSeats[Index[0]][Index[1]] == null)
                        {
                            if (TakenSeats[Index[0]][Index[1]] != "taken")
                            {
				TakenSeats[Index[0]][Index[1]] = "chosen"; // Change chair to chosen if it is not taken
                            }
                        }
                        else if (TakenSeats[Index[0]][Index[1]] == "chosen")
                        {
                            TakenSeats[Index[0]][Index[1]] = null; // Change chair to free if it was chosen before
                        }
                    }
                }

		
                if (keyPressed == ConsoleKey.DownArrow)
                {
                    if (Index[0] <= ROWS)
                    {
                        Index[0]++;
                        if (Index[0] > ROWS)
                        {
                            Index[0] = 0;
                        }
                        if (Index[0] >= ROWS)
                        {
                            if (Index[1] < AMOUNT / 2)
                            {
                                ButtonIndex = 0;
                            }
                            else
                            {
                                ButtonIndex = 1;
                            }
                        }
                        else
                        {
			    if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
			    {
				while (TakenSeats[Index[0]][Index[1]] == "taken")
				{
				    Index[0]++;
				    if (Index[0] >= ROWS)
				    {
					Index[0]--;
					while (TakenSeats[Index[0]][Index[1]] == "taken")
					{
					    Index[0]--;
					}
				    }
				}
			    }
			}
		    }
                }
		
		else if (keyPressed == ConsoleKey.UpArrow)
                {
                    if (Index[0] >= 0)
                    {
                        Index[0]--;
                        if (Index[0] < 0)
                        {
                            Index[0] = ROWS - 1;
                        }
                        if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
                        {
                            while (TakenSeats[Index[0]][Index[1]] == "taken")
                            {
                                Index[0]--;
                                if (Index[0] < 0)
                                {
                                    Index[0]++;
                                    while (TakenSeats[Index[0]][Index[1]] == "taken")
                                    {
                                        Index[0]++;
                                    }
                                }
                            }
                        }
                    }
                }
		
		else if (keyPressed == ConsoleKey.RightArrow)
                {
                    if (Index[1] < AMOUNT)
                    {
                        Index[1]++;
                        if (Index[1] >= AMOUNT)
                        {
                            Index[1] = 0;
                        }
                        if (Index[0] >= ROWS)
                        {
                            if (ButtonIndex < Buttons.Count-1)
                            {
                                ButtonIndex++;
                            }
                        }
                        else
                        {
                            if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
                            {
                                while (TakenSeats[Index[0]][Index[1]] == "taken")
                                {
                                    Index[1]++;
                                    if (Index[1] >= AMOUNT - 1)
                                    {
                                        Index[1]--;
                                        while (TakenSeats[Index[0]][Index[1]] == "taken")
                                        {
                                            Index[1]--;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
		else if (keyPressed == ConsoleKey.LeftArrow)
                {
                    if (Index[1] >= 0)
                    {
                        Index[1]--;
                        if (Index[1] < 0)
                        {
                            Index[1] = AMOUNT - 1;
                        }
			if (Index[0] >= ROWS)
			{
                            if (ButtonIndex > 0)
                            {
                                ButtonIndex--;
                            }
                        }
                        else
                        {
                            if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
                            {
                                while (TakenSeats[Index[0]][Index[1]] == "taken")
                                {
                                    Index[1]--;
                                    if (Index[1] < 0)
                                    {
                                        Index[1]++;
                                        while (TakenSeats[Index[0]][Index[1]] == "taken")
                                        {
                                            Index[1]++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                DrawSeats();
                DrawButtons();
            }
            while (key.Key != ConsoleKey.Escape);

            var temp = Program.information;
            temp.ChosenSeats = null;
            Program.information = temp;
            
            return 0;
        }


	// This function handles drawing the seats, if the seat is taken
	// it will draw a red 'x' between the brackets, otherwise it will
	// have have a space between them
        public void DrawSeat(string status, bool selected)
        {
            if (status == "taken")
            {
                if (selected)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.Write("[ ]");
                }

                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write("[ ]");
                }
            }
	    else if (status == "chosen")
            {
                if (selected)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("x");
                    Console.ResetColor();
                    Console.BackgroundColor = ConsoleColor.White;
		    Console.Write("]");
                }
                else
                {
		    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("x");
                    Console.ResetColor();
		    Console.Write("]");
                }
            }
	    else if (status == null)
            {
                if (selected)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("[ ]");
                }
                else
                {
		    Console.Write("[ ]");
                }
            }
            Console.ResetColor();
        }
    }
}
