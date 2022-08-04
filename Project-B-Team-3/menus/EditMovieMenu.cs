using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    internal class EditMovie : IStructure
    {
        private int Index;
        private List<api.Textbox> Textboxes = new List<api.Textbox>();
        private api.BigTextbox BigTextbox;
        private api.Button deleteButton;   
        private api.Button EditTime;
        private api.Button ApplyButton;
        public List<Movies> M;
        private MoviesList Movies;

        public EditMovie()
        {

            Movies = new MoviesList();
            Movies.Load();
            M = Movies.Movies;

            Textboxes.Add(new api.Textbox("Title", 0, (Console.WindowWidth - 20) / 2, 5, space_allowed: true));
            Textboxes.Add(new api.Textbox("Genre", 1, (Console.WindowWidth - 20) / 2, 7, space_allowed: true));
            BigTextbox = new api.BigTextbox("Discription", 2, (Console.WindowWidth - 90) / 2, 9, length : 3, width : 90, space_allowed: true);

            EditTime = new api.Button("Edit Time", 3, (Console.WindowWidth - 11) / 2, 13);
            ApplyButton = new api.Button("Apply", 4, (Console.WindowWidth - 7) / 2, 15);
            deleteButton = new api.Button("Delete Movie", 5, (Console.WindowWidth - 14) / 2, 17);


        }

        public void FirstRender()
        {
            api.PrintCenter("<<*Edit Movie*>>", 1);
            api.PrintCenter("When a box is left empty the specified item is kept the same", 2);
            string footer = "ARROW KEYS / TAB - Change box  |  ENTER - Finish  |  ESCAPE - Go back";
            Console.SetCursorPosition((Console.WindowWidth - footer.Length) / 2, 28);
            Console.WriteLine(footer);
            DisplayTextboxes();
        }

        public void DisplayTextboxes()
        {
            foreach (var textbox in Textboxes)
            {
                textbox.Display(Index);
            }
            BigTextbox.Display(Index);
            deleteButton.Display(Index);
            EditTime.Display(Index);
            ApplyButton.Display(Index);
        }

        public int Run()
        {
            var info = Program.information;

            //we have to check if any info is saved of the textboxes, if it is we assign the info to the textboxes.
            //if we dont save the input and someone goes to the editTimeScreen the previously filled in data will be lost
            if (info.AddMovieInfo != null)
            {
                Textboxes[0].Input = info.AddMovieInfo[0];
                Textboxes[1].Input = info.AddMovieInfo[1];
                BigTextbox.Input = info.AddMovieInfo[2];

            }

            Console.Clear();
            FirstRender();
            ConsoleKeyInfo key;
            int index = 0;
            //searches in the list of movies to find at what index it is
            for (int i = 0; i < M.Count; i++)
            {
                if (M[i].Name == info.ChosenFilm.Name)
                {
                    index = i;
                    break;
                }
            }


            do
            {
                key = Console.ReadKey(true);
                ConsoleKey keyPressed = key.Key;

                if (keyPressed == ConsoleKey.Tab || keyPressed == ConsoleKey.DownArrow)
                {
                    if (Index < 5)
                    {
                        Index++;
                    }
                    else
                    {
                        Index = 0;
                    }
                }
                else if (keyPressed == ConsoleKey.UpArrow)
                {
                    if (Index > 0)
                    {
                        Index--;
                    }
                    else
                    {
                        Index = Textboxes.Count + 3;
                    }
                }

                if (Index < Textboxes.Count)
                {
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        Textboxes[Index].Backspace();
                    }
                    else
                    {
                        Textboxes[Index].AddLetter(key.KeyChar);
                    }
                }
                if (Index == 2)
                {
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        BigTextbox.Backspace();
                    }
                    else
                    {
                        if (BigTextbox.Input.Length < 209)
                        {
                            BigTextbox.AddLetter(key.KeyChar);
                        }
                    }
                }

                if (key.Key == ConsoleKey.Enter)
                {
                    if(Index == 3)
                    {
                        info.AddMovieInfo = new string[4] { Textboxes[0].Input, Textboxes[1].Input, BigTextbox.Input, "edit" };
                        Program.information = info;
                        return 1;
                    }
                    if(Index == 4)
                    {

                        char[] CharArrayDiscription = BigTextbox.Input.ToCharArray();
                        int count = 0;
                        for(int i = 0; i < CharArrayDiscription.Length; i++)
                        {
                            if (CharArrayDiscription[i] == ' ' && count >= 70)
                            {
                                CharArrayDiscription[i] = char.Parse("\n");
                                count = 0;
                            }
                            count++;
                        }
                        BigTextbox.Input = new string(CharArrayDiscription);

                        info.ChosenFilm.Name = Textboxes[0].Input.Length != 0 ? Textboxes[0].Input : info.ChosenFilm.Name;
                        info.ChosenFilm.Genre = Textboxes[1].Input.Length != 0 ? Textboxes[1].Input : info.ChosenFilm.Genre;
                        info.ChosenFilm.Discription = BigTextbox.Input.Length != 0 ? BigTextbox.Input : info.ChosenFilm.Discription;


                        M[index] = info.ChosenFilm;
                        info.AddMovieInfo = null;
                        info.ChosenFilm = null;

                        Program.information = info;
                        Movies.Save();
                        return 0;
                    }
                    if(Index == 5)
                    {
                        var deleteMovie = new MoviesList();
                        deleteMovie.Remove(Program.information.ChosenFilm.Name);
                        return 0;
                    }
                }
                DisplayTextboxes();
            }
            while (key.Key != ConsoleKey.Escape);
            info.AddMovieInfo = null;
            info.ChosenFilm = null;
            Program.information = info;
            return 0;
        }
    }
}
