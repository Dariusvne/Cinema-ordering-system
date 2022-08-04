using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace ProjectB
{
    internal class ChangeTime : IStructure
    {
        public int Index;
        public List<Movies> M;
        public List<List<api.Button>> Buttons = new List<List<api.Button>>();
        private List<api.Textbox> TextBox = new List<api.Textbox>(); //Keeps count of the times change textboxes
        private List<api.Textbox> TextBox2 = new List<api.Textbox>(); //Keeps count of the Dates change textboxes

        private MoviesList Movies;
        public ChangeTime()
        {
            Movies = new MoviesList();
            Movies.Load();
            M = Movies.Movies;

            int p = 6;
            int tempindex = 1000;
            int DatesIndex = Program.information.ChosenFilm.Dates.Count;
            int Timesindex = 0;
            int x = 0;
            //Creates all the buttons and textboxes
            for (int i = 0; i < Program.information.ChosenFilm.Dates.Count; i++)
            {
                TextBox.Add(new api.Textbox("Add/Delete time", Timesindex, 2, p));
                TextBox2.Add(new api.Textbox("Edit/Delete Date", DatesIndex, 95, p - 1));
                Timesindex++;
                DatesIndex++;
                x = Console.WindowWidth / 2 - ((Program.information.ChosenFilm.Dates[i]["Time"].Count * 7) + (Program.information.ChosenFilm.Dates[i]["Time"].Count - 1) * 3) / 2 - 1;  // Finds middle of the screen for all boxes
                Buttons.Add(new List<api.Button>());
                for (int q = 0; q < Program.information.ChosenFilm.Dates[i]["Time"].Count; q++)
                {
                    Buttons[i].Add(new api.Button(Program.information.ChosenFilm.Dates[i]["Time"][q], tempindex, x, p));
                    x += 10;
                    tempindex++;
                }
                p += 3;
            }

            

            //When the Date textboxes are not at their max(7), there will be added one extra so a new Date can be added.
            if (Program.information.ChosenFilm.Dates.Count < 7)
            {
                TextBox2.Add(new api.Textbox("Add/Delete Date", DatesIndex, 95, p - 1));
            }
        }
        //this function can be used to check if a date is in a correct format.
        private bool check_Time(string readAddMeeting)
        {
            var dateFormats = new[] {"dd-MM-yyyy"};
            DateTime scheduleDate;
            bool validDate = DateTime.TryParseExact(
                readAddMeeting,
                dateFormats,
                DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.None,
                out scheduleDate);
            

            return validDate;   
        }

        private int DrawButtons()
        {
            int count = 0;
            //draws all the times 
            for (int i = 0; i < Buttons.Count; i++)
            {
                for (int j = 0; j < Buttons[i].Count; j++)
                {
                    {
                        Buttons[i][j].Display(Index);
                        count++;

                    }
                }
            }

            //draws all the textboxes
            for (int p = 0; p < TextBox.Count; p++)
            {
                TextBox[p].Display(Index);
                TextBox2[p].Display(Index);
            }
            if (TextBox.Count < 7)
            {
                TextBox2[TextBox2.Count - 1].Display(Index);
            }


            return count;
        }
        public void FirstRender()
        {
            api.PrintCenter("<<*Select the time on which you would like to see your movie*>>", 1);
            api.PrintCenter("Retype a Time/Date to delete it or type a different Time/Date to edit it, formats: HH:MM || DD-MM-YY", 26, foreground: ConsoleColor.Green);
            api.PrintCenter(Program.information.ChosenFilm.Name, 3, background: ConsoleColor.White, foreground: ConsoleColor.Black);
            api.PrintCenter("ARROW UP/DOWN - Select Boxes | ENTER - Comfirm | ESCAPE - Exit", 28);
            int j = 5;
            //Draws all the dates
            for (int i = 0; i < Program.information.ChosenFilm.Dates.Count; i++)
            {
                api.PrintCenter(Program.information.ChosenFilm.Dates[i]["Date"][0], j);
                j += 3;
            }
        }



        //Buttons is a jagged list of buttons, every list stands for a row of times on the screen.
        //The user uses the arrow keys to change the value of the index if the index is out of bound
        //it will go to the next list inside of buttons.
        public int Run()
        {
            Console.Clear();
            int normalIndex = 0;
            var info = Program.information;
            FirstRender();
            DrawButtons();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                api.PrintCenter("                                                          ", 4, foreground: ConsoleColor.DarkRed);

                if (TextBox2.Count == 1)
                {
                    if (TextBox2[normalIndex].Index == Index)
                    {
                        if (key.Key == ConsoleKey.Backspace)
                        {
                            TextBox2[normalIndex].Backspace();
                        }
                        else
                        {
                            TextBox2[normalIndex].AddLetter(key.KeyChar);
                        }

                    }
                }

                else if (Index <= TextBox[TextBox.Count - 1].Index)
                {

                    if (TextBox[normalIndex].Index == Index)
                    {
                        if (key.Key == ConsoleKey.Backspace)
                        {
                            TextBox[normalIndex].Backspace();
                        }
                        else
                        {
                            TextBox[normalIndex].AddLetter(key.KeyChar);
                        }

                    }

                }
                else if (Index > TextBox[TextBox.Count - 1].Index && Index <= TextBox2[TextBox2.Count - 1].Index)
                {
                    if (TextBox2[normalIndex].Index == Index)
                    {
                        if (key.Key == ConsoleKey.Backspace)
                        {
                            TextBox2[normalIndex].Backspace();
                        }
                        else
                        {
                            TextBox2[normalIndex].AddLetter(key.KeyChar);
                        }

                    }
                }


                //When you press enter there can be 2 scenarios, either the Index is lower then the count of the list of Time textboxes or higher (so the person is either on the right or left side of the textboxes).
                //when its lower it will run the first if statement which handles changing or adding times.
                //when its higher it will run the second statement which handles adding, deleting and changing dates. When you add a new date you will be able to add times to it. 

                if (key.Key == ConsoleKey.Enter)
                {
                    //load json file
                    var Movies123 = new MoviesList();
                    Movies123.Load();
                    var M = Movies123.Movies;
                    

                    if(TextBox2.Count != 0)
                    {
                        

                        if (Index < TextBox.Count)
                        { 
                        
                            DateTime time;
                            int index = 0;
                            if (DateTime.TryParse(TextBox[normalIndex].Input, out time))
                            {
                                //searches in the list of movies to find at what index it is
                                for (int i = 0; i < M.Count; i++)
                                {
                                    if (M[i].Name == info.ChosenFilm.Name)
                                    {
                                        index = i;
                                        break;
                                    }
                                }
                                //Find the given time in the list of times and removes it

                                bool inTimeList = false;
                                for (int i = 0; i < M[index].Dates[normalIndex]["Time"].Count; i++)
                                {

                                    if (TextBox[normalIndex].Input == M[index].Dates[normalIndex]["Time"][i])
                                    {
                                        inTimeList = true;
                                        Movies123.RemoveTime(index, normalIndex, TextBox[normalIndex].Input);
                                        break;
                                    }
                                }

                                if (inTimeList == false && Buttons[normalIndex].Count < 7)
                                {
                                    M[index].Dates[normalIndex]["Time"].Add(TextBox[normalIndex].Input);
                                    Movies123.Save();
                                }

                                bool Contains = false;
                                for (int i = 0; i < Buttons[normalIndex].Count; i++)
                                {
                                    if (Buttons[normalIndex][i].Title == TextBox[normalIndex].Input)
                                    {
                                        Contains = true;
                                    }
                                }
                                if (Buttons[normalIndex].Count == 7 && !Contains)
                                {
                                    api.PrintCenter("You are at your max of 7 times!", 4, foreground: ConsoleColor.DarkRed);
                                }

                                info.ChosenFilm = Movies123.Movies[index];
                                Program.information = info;

                               
                                int p = 6;
                                int tempindex = 1000;
                                int realindex = 0;
                                int x = 0;

                                //We need to redraw the boxes and buttons, so we are able to show the added or romoved time.
                                Buttons = new List<List<api.Button>>();
                                TextBox = new List<api.Textbox>();
                                for (int i = 0; i < Program.information.ChosenFilm.Dates.Count; i++)
                                {
                                    TextBox.Add(new api.Textbox("Add/Delete time", realindex, 2, p));
                                    Buttons.Add(new List<api.Button>());
                                    tempindex++;
                                    realindex++;
                                    x = Console.WindowWidth / 2 - ((Program.information.ChosenFilm.Dates[i]["Time"].Count * 7) + (Program.information.ChosenFilm.Dates[i]["Time"].Count - 1) * 3) / 2 - 1;  // Finds middle of the screen for all boxes
                                    api.PrintExact("                                                                                    ", x - 15, p); //need to remove the previous time boxes before drawing the new ones.
                                    for (int q = 0; q < Program.information.ChosenFilm.Dates[i]["Time"].Count; q++)
                                    {
                                        Buttons[i].Add(new api.Button(Program.information.ChosenFilm.Dates[i]["Time"][q], tempindex, x, p));
                                        x += 10;
                                        tempindex++;
                                    }
                                    p += 3;

                                }
                                
                            }

                            else
                            {
                                api.PrintCenter("You did not type the time correctly", 4, foreground: ConsoleColor.DarkRed);

                            }
                        
                        }

                        else
                        {

                            if (Index >= TextBox2[0].Index && Index <= TextBox2[TextBox2.Count-1].Index)
                            {
                                DateTime Date;
                                int index = 0;
                                int textBox2Index = 0; 


                                if (check_Time(TextBox2[normalIndex].Input))
                                {
                                    //searches in the list of movies to find at what index it is
                                    for (int i = 0; i < M.Count; i++)
                                    {
                                        if (M[i].Name == info.ChosenFilm.Name)
                                        {
                                            index = i;
                                            break;
                                        }
                                    }



                                    if (TextBox.Count != TextBox2.Count)
                                    {
                                        //Adds new date when the index is at the last one of the boxes on the right side.
                                        if(TextBox2[TextBox2.Count-1].Index == Index)
                                        {
                                            List<string> tempDate = new List<string>();
                                            List<string> tempTime = new List<string>();

                                            DateTime FilmDate = DateTime.ParseExact(TextBox2[normalIndex].Input, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                            int compare = FilmDate.CompareTo(DateTime.Today);
                                            if (compare > 0)
                                            {


                                                if (dateNotUsed(index, normalIndex))
                                                {
                                                    tempDate.Add(TextBox2[normalIndex].Input);

                                                    var tempdict = new Dictionary<string, List<string>>() {

                                                        { "Date", tempDate},
                                                        { "Time", tempTime}
                                                    };

                                                    M[index].Dates.Add(tempdict);
                                                    Movies123.Save();
                                                    textBox2Index = TextBox.Count + 1;

                                                    Index = TextBox2.Count == 7 && (TextBox2.Count != TextBox.Count) ? Index + 1 : Index + 2;
                                                    normalIndex = TextBox2.Count == 7 && (TextBox2.Count != TextBox.Count) ? normalIndex : normalIndex + 1;
                                                }
                                                else
                                                {
                                                    textBox2Index = TextBox.Count;
                                                    api.PrintCenter("This date already exist!", 4, foreground: ConsoleColor.DarkRed);
                                                }
                                            }
                                            else
                                            {
                                                textBox2Index = TextBox.Count;
                                                if (compare == 0)
                                                {
                                                    api.PrintCenter("This is today", 4, foreground: ConsoleColor.DarkRed);
                                                }
                                                else
                                                {
                                                    api.PrintCenter("This date is in the past", 4, foreground: ConsoleColor.DarkRed);
                                                }
                                            }

                                        }
                                        //removes the date when the date is equal to the given one.
                                        else if(TextBox2[normalIndex].Input == M[index].Dates[normalIndex]["Date"][0])
                                        {
                                            M[index].Dates.RemoveAt(normalIndex);
                                            Movies123.Save();
                                            Console.Clear();
                                            textBox2Index = TextBox.Count - 1;
                                            Index = normalIndex == 0 ? Index - 1 : Index - 2;
                                            normalIndex = normalIndex == 0 ? normalIndex : normalIndex - 1;


                                        }
                                        //Changes the date into another date
                                        else if (TextBox2[normalIndex].Input != M[index].Dates[normalIndex]["Date"][0])
                                        {
                                            DateTime FilmDate = DateTime.ParseExact(TextBox2[normalIndex].Input, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                            int compare = FilmDate.CompareTo(DateTime.Today);
                                            if (compare > 0)
                                                if (dateNotUsed(index, normalIndex))
                                                {
                                                    M[index].Dates[normalIndex]["Date"][0] = TextBox2[normalIndex].Input;
                                                    Movies123.Save();
                                                    textBox2Index = TextBox.Count;
                                                }
                                                else
                                                { 
                                                    textBox2Index = TextBox.Count;

                                                    api.PrintCenter("This date already exist!", 4, foreground: ConsoleColor.DarkRed);
                                                }
                                            else
                                            {
                                                textBox2Index = TextBox.Count;
                                                if (compare == 0)
                                                {
                                                    api.PrintCenter("This is today", 4, foreground: ConsoleColor.DarkRed);
                                                }
                                                else
                                                {
                                                    api.PrintCenter("This date is in the past", 4, foreground: ConsoleColor.DarkRed);
                                                }
                                            }
                                        }
                                    }
                                    else if(TextBox.Count == TextBox2.Count)
                                    {
                                        if(TextBox2[normalIndex].Input == M[index].Dates[normalIndex]["Date"][0])
                                        {
                                            M[index].Dates.RemoveAt(normalIndex);
                                            Movies123.Save();
                                            Console.Clear();
                                            textBox2Index = TextBox.Count - 1;

                                            Index = normalIndex == 0 ? Index - 1 : Index - 2;
                                            normalIndex = normalIndex == 0 ? normalIndex : --normalIndex;


                                        }

                                        else if (TextBox2[normalIndex].Input != M[index].Dates[normalIndex]["Date"][0])
                                        {
                                            DateTime FilmDate = DateTime.ParseExact(TextBox2[normalIndex].Input, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                            int compare = FilmDate.CompareTo(DateTime.Today);
                                            if (compare > 0)
                                                if (dateNotUsed(index, normalIndex))
                                                {
                                                    M[index].Dates[normalIndex]["Date"][0] = TextBox2[normalIndex].Input;
                                                    Movies123.Save();
                                                    textBox2Index = TextBox.Count;
                                                }
                                                else
                                                {
                                                    textBox2Index = TextBox.Count;

                                                    api.PrintCenter("This date already exist!", 4, foreground: ConsoleColor.DarkRed);
                                                }
                                            else
                                            {
                                                textBox2Index = TextBox.Count;
                                                if (compare == 0)
                                                {
                                                    api.PrintCenter("This is today", 4, foreground: ConsoleColor.DarkRed);
                                                }
                                                else
                                                {
                                                    api.PrintCenter("This date is in the past", 4, foreground: ConsoleColor.DarkRed);
                                                }
                                            }

                                        }
                                    }

                                    info.ChosenFilm = Movies123.Movies[index];
                                    Program.information = info;

                                    
                                    
                                    int p = 6;
                                    int tempindex = 1000;
                                    int realindex = 0;
                                    int x = 0;
                                    int j = 5;

                                    Buttons = new List<List<api.Button>>();
                                    TextBox = new List<api.Textbox>();
                                    TextBox2 = new List<api.Textbox>();

                                    //redraws all the new boxes and textboxes with the new information (either a deleted box or a added box)
                                    for (int i = 0; i < Program.information.ChosenFilm.Dates.Count; i++)
                                    {
                                        TextBox.Add(new api.Textbox("Add/Delete time", realindex, 2, p));
                                        TextBox2.Add(new api.Textbox("Edit/Delete Date", textBox2Index, 95, p - 1));
                                        Buttons.Add(new List<api.Button>());
                                        api.PrintCenter(Program.information.ChosenFilm.Dates[i]["Date"][0], j);

                                        tempindex++;
                                        realindex++;
                                        textBox2Index++;
                                        x = Console.WindowWidth / 2 - ((Program.information.ChosenFilm.Dates[i]["Time"].Count * 7) + (Program.information.ChosenFilm.Dates[i]["Time"].Count - 1) * 3) / 2 - 1;  // Finds middle of the screen for all boxes
                                        api.PrintExact("                                                                                    ", x - 15, p); //need to remove the previous time boxes before drawing the new ones.
                                        for (int q = 0; q < Program.information.ChosenFilm.Dates[i]["Time"].Count; q++)
                                        {
                                            Buttons[i].Add(new api.Button(Program.information.ChosenFilm.Dates[i]["Time"][q], tempindex, x, p));
                                            x += 10;
                                            tempindex++;
                                        }
                                        j += 3;
                                        p += 3;

                                    }
                                    bool is_Even = TextBox.Count == TextBox2.Count ? true : false;
                                    if (is_Even == false || TextBox2.Count < 7)
                                    {
                                        TextBox2.Add(new api.Textbox("Add Date", textBox2Index, 95, p - 1));
                                    }
                                }
                                else
                                {
                                    api.PrintCenter("You did not type the Date correctly", 4, foreground: ConsoleColor.DarkRed);

                                }
                            }
                        }
                    }

                }
                if (key.Key == ConsoleKey.RightArrow)
                {

                    if (Index < TextBox2[0].Index)
                    {
                        Index += TextBox.Count;
                    }


                }
                else
                //When the index is greater then the position of the first button, it will add one to the index.
                if (key.Key == ConsoleKey.LeftArrow && Index - TextBox.Count >= 0 && TextBox2.Count > 1)
                {
                    if (Index == TextBox2[TextBox2.Count - 1].Index && TextBox.Count != TextBox2.Count)
                    {
                        Index = TextBox.Count - 1;
                        normalIndex--;
                    }
                    else
                    {
                        Index -= TextBox.Count;
                    }
                }

                //When key is down arrow it will move to the next box in the jagged list of boxes
                if (key.Key == ConsoleKey.DownArrow && TextBox2.Count > 1)
                {
                    //checks if next row is empty, if it is the row will be skipped.
                    if (Index == TextBox2[TextBox2.Count - 1].Index)
                    {
                        Index = 0;
                        normalIndex = 0;
                    }
                    else if (Index == TextBox[TextBox.Count - 1].Index && Index < TextBox.Count)
                    {
                        Index++;
                        normalIndex = 0;
                    }
                    else if(Index < TextBox2[TextBox2.Count-1].Index && Index >= 0)
                    {
                        Index++;
                        normalIndex++;
                    }

                }
                else if (key.Key == ConsoleKey.UpArrow && TextBox2.Count > 1)
                {
                    if (Index == TextBox2[0].Index)
                    {
                        Index--;
                        normalIndex = TextBox.Count - 1;
                    }
                    else if (Index > 0)
                    {
                        normalIndex--;
                        Index--;
                    }else if(Index == 0)
                    {
                        normalIndex = TextBox2.Count-1;
                        Index = TextBox2[TextBox2.Count - 1].Index;
                    }

                }
                Console.SetCursorPosition(0, 0);
                DrawButtons();
                FirstRender();
            }
        while (key.Key != ConsoleKey.Escape);

            if (Program.information.AddMovieInfo[3].Equals("add"))
            {
                return 2;
            }
            else
            {
                return 0;
            }
            
        }
        private bool dateNotUsed(int index, int normalIndex)
        {
            bool exist = false;
            foreach (var date in M[index].Dates)
            {
                if (TextBox2[normalIndex].Input == date["Date"][0])
                {
                    return false;
                }
            }
            return true;
        }
    }
}



