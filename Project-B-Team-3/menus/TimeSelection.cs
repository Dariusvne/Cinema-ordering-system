using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    internal class TimeSelection : IStructure
    {
        public int Index;
        public int normalIndex;

        public List<Movies> M;
        public List<List<api.Button>> Buttons = new List<List<api.Button>>();

        public TimeSelection()
        {
            var Movies = new MoviesList();
            Movies.Load();
            M = Movies.Movies;

            int p = 6;
            int tempindex = 0;
            int x = 0;
            //Creates all the buttons 
            for (int i = 0; i < Program.information.ChosenFilm.Dates.Count; i++)
            {
                x = Console.WindowWidth/2-((Program.information.ChosenFilm.Dates[i]["Time"].Count*7)+(Program.information.ChosenFilm.Dates[i]["Time"].Count-1)*3)/2-1;  // Finds middle of the screen for all boxes
                Buttons.Add(new List<api.Button>());
                for (int q = 0; q < Program.information.ChosenFilm.Dates[i]["Time"].Count; q++)
                {
                    Buttons[i].Add(new api.Button(Program.information.ChosenFilm.Dates[i]["Time"][q], tempindex, x, p));
                    x += 10;
                    tempindex++;    
                }
                p += 3;
            }

            bool is_Empty = true;
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (Buttons[i].Count != 0)
                {
                    normalIndex = i;
                    is_Empty = false;
                    break;
                }
            }
            if (is_Empty)
            {
                normalIndex = -1;
            }

        }
        private int DrawButtons()
        {
            int count = 0;
            for(int i = 0; i < Buttons.Count; i++)
            {
                for(int j = 0;j < Buttons[i].Count; j++)
                    {
                        Buttons[i][j].Display(Index);
                        count++;
                    }
            }
            return count;
        }
        public void FirstRender()
        {
            api.PrintCenter("<<*Select the time on which you would like to see your movie*>>", 1);
            api.PrintCenter(Program.information.ChosenFilm.Name, 3, background: ConsoleColor.White, foreground: ConsoleColor.Black);
            api.PrintCenter("ARROW UP/DOWN - Select time | ENTER - Confirm time | ESCAPE - Exit", 28);
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

            if (normalIndex >= 0)
            {
                Index = (Buttons[normalIndex].Count-1)/2;
            }

            var info = Program.information;
            FirstRender();  
            int indexCount = DrawButtons();

            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    //Saves information for the seats screen
                    var temporary = Program.information;

                    int count = 0;
                    string time = "";
                    for (int i = 0; i < Buttons.Count; i++) {
                        for (int j = 0; j < Buttons[i].Count; j++) {
                            if (count == Index) {
                                time = Buttons[i][j].Title;
                                temporary.ChosenDate = Program.information.ChosenFilm.Dates[normalIndex]["Date"][0];
                                temporary.ChosenTime = time;
                                Program.information = temporary;
                                Console.Clear();

                                return 1;
                            }
                            else
                            {
                                count++;
                            }
                        }
                    }
                }
                if (normalIndex != -1)
                {

                
                    //When the index is smaller then the position of the last button, it will add one to the index.
                    if (key.Key == ConsoleKey.RightArrow && Index < Buttons[normalIndex][Buttons[normalIndex].Count - 1].Index)
                    {
                        Index++;
                    }
                    //When the index is greater then the position of the first button, it will add one to the index.
                    else if (key.Key == ConsoleKey.LeftArrow && Index > Buttons[normalIndex][0].Index)
                    {
                        Index--;
                    }
                    //When key is down arrow it will move to the next box in the jagged list of boxes
                    else if (key.Key == ConsoleKey.DownArrow && normalIndex < Buttons.Count - 1)
                    {

                        for (int i = normalIndex + 1; i < Buttons.Count; i++)
                        {
                            if (Buttons[i].Count != 0)
                            {
                                normalIndex = i;
                                Index = Buttons[normalIndex][(Buttons[normalIndex].Count - 1) / 2].Index;
                                break;
                            }
                        }
                    }
                    else if (key.Key == ConsoleKey.UpArrow && normalIndex > 0)
                    {
                        
                        for (int i = normalIndex - 1; i >= 0; i--)
                        {
                            if (Buttons[i].Count != 0)
                            {
                                normalIndex = i;
                                Index = Buttons[normalIndex][(Buttons[normalIndex].Count - 1) / 2].Index;
                                break;
                            }
                        }

                    }
                }
                DrawButtons();
            }
            while (key.Key != ConsoleKey.Escape);
            info.ChosenFilm = M[0];
            Program.information = info;
            
            return 0;

        }
    }
}
