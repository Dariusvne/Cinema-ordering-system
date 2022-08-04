using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    internal class MovieSelection : IStructure
    {
        private int start;
        private int end;
        private int pagenumber;

        private int Index;
        private List<api.Textbox> FilterBoxes;  
        
        public List<Movies> M;

        public MovieSelection()
        {
            var Movies = new MoviesList();
            Movies.Load();

            M = Movies.Movies;

            FilterBoxes = new List<api.Textbox>();
            FilterBoxes.Add(new api.Textbox("Filter Title", -1, 0, 3, true));
            FilterBoxes.Add(new api.Textbox("Filter Genre", -2, 0, 1, true));
        }

        private void DisplayMenu(int start, int end, int pagenumber)
        {
            Console.Clear();
            api.PrintCenter("<<*Select the movie you want*>>", 1);
            api.PrintCenter("ARROW UP/DOWN - Select movie | ARROW LEFT/RIGHT - Select page | ENTER - Confirm movie | ESCAPE - Exit", 28);
            if (M.Count % 3 != 0 || M.Count == 0)
            {
                api.PrintCenter("Page " + pagenumber + "/" + ((M.Count / 3) + 1), 2);
            }
            else if(M.Count <= 3 || M.Count % 3 == 0)
            {
                api.PrintCenter("Page " + pagenumber + "/" + (M.Count / 3), 2);

            }
            int j = 5;
            for (int i = start; i < end; i++)
            {
                Console.SetCursorPosition(0, j + 1);
                Console.WriteLine("Genre: " + M[i].Genre, j + 1);
                Console.SetCursorPosition(0, j + 2);
                Console.WriteLine("Discription: ", j + 1);
                Console.SetCursorPosition(0, j + 3);
                Console.WriteLine(M[i].Discription);

                j = j + 8;


            }


        }
        public void FirstRender()
        {
            int j = 5;


            if (end > M.Count)

            {
                end = M.Count;
            }

            DisplayMenu(start, end, pagenumber);

            for (int i = start; i < end; i++)
            {
                if (i == Index)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.SetCursorPosition(0, j);
                    Console.WriteLine($"Title: {M[i].Name} ");
                    Console.ResetColor();

                    if (pagenumber == 1)
                    {
                        FilterBoxes[0].Display(Index);
                        FilterBoxes[1].Display(Index);
                    }

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.SetCursorPosition(0, j);
                    Console.WriteLine($"Title: {M[i].Name} ");
                    Console.ResetColor();

                    if (pagenumber == 1)
                    {
                        FilterBoxes[0].Display(Index);
                        FilterBoxes[1].Display(Index);
                    }
                }
                
                

                j = j + 8;
            }
            if(FilterBoxes[0].Input != "" || FilterBoxes[1].Input != "")
            {
                api.PrintCenter("To reset the filters empty the boxes and hit Enter...", 4, foreground: ConsoleColor.Green);
            }
        }


        //uses pages and an index, when the index goes past 3 (3 because the max amount of movies on 1 page is 3) it swaps pages.
        public int Run()
        {
            var info = Program.information;
            
            int page = 0;
            pagenumber = 1;
            start = 0;
            end = 3;
            int maxpage = M.Count % 3 == 0 ? M.Count / 3 : ((M.Count / 3) + 1);
            if(M.Count <= 3)
            {
                maxpage = 1;
            }

            if (end > M.Count)

            {
                end = M.Count;
            }

            Console.Clear();
            FirstRender();
            ConsoleKeyInfo key;
            do
            {
                int p = 0;
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.DownArrow && Index < end - 1)
                {
                    Index++;
                }
                else if (key.Key == ConsoleKey.UpArrow && (Index > page || Index == 0 || Index == -1))
                {
                    Index--;
                }
                //when the left arrow key is pressed it will swap te the previes page or when up arrow is pressed and the index is equal to the start variable.
                else if (key.Key == ConsoleKey.LeftArrow && start > 0 || (key.Key == ConsoleKey.UpArrow && Index == start && pagenumber > 1))
                {
                    Index = Index == start ? Index - 1 : start - 3;
                    start -= 3;
                    pagenumber--;
                    page = page - 3;

                    if (end % 3 == 0)
                    {
                        end = end - 3;
                    }
                    else
                    {
                        end = end - (end % 3);
                    }
                    Console.Clear();
                    FirstRender();
                    if (end > M.Count)

                    {
                        end = M.Count;
                    }
                }
                //when the right arrow key is pressed it will swap te the next page or when down arrow is pressed and the index is equal to the end variable.
                else if ((key.Key == ConsoleKey.RightArrow && end < M.Count) || (key.Key == ConsoleKey.DownArrow && Index == end - 1 && pagenumber < maxpage))
                {
                    page = page + 3;
                    pagenumber++;
                    Index = page;
                    start += 3;
                    end += 3;
                    Console.Clear();
                    FirstRender();
                    if (end > M.Count)

                    {
                        end = M.Count;
                    }
                }
                else if (Index == -1 || Index == -2)
                {
                    
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        if(Index == -1)
                        {
                            FilterBoxes[0].Backspace();
                        }
                        else
                        {
                            FilterBoxes[1].Backspace();
                        }
                    }
                    else
                    {
                        if (Index == -1)
                        {
                            FilterBoxes[0].AddLetter(key.KeyChar);
                        }
                        else
                        {
                            FilterBoxes[1].AddLetter(key.KeyChar);
                        }
                    }
                }



                if (key.Key == ConsoleKey.Enter)
                {
                    if(Index == -1 || Index == -2)
                    {
                        var Movies = new MoviesList();
                        Movies.Load();
                        var tempMoviesList = new List<Movies>();

                        if (FilterBoxes[0].Input == "" && FilterBoxes[1].Input == "")
                        {
                            M = Movies.Movies;
                            maxpage = M.Count % 3 == 0 ? M.Count / 3 : ((M.Count / 3) + 1);
                            start = 0;
                            end = 3;
                            pagenumber = 1;
                            FirstRender();
                            if (end > M.Count)
                            {
                                end = M.Count;
                            }
                            else
                            {
                                end = 3;
                            }
                            continue;
                        }

                        

                        if (FilterBoxes[0].Input != "" && FilterBoxes[1].Input != "")
                        {
                            M = M;
                        }
                        else if(FilterBoxes[0].Input == "" && FilterBoxes[1].Input == "")
                        {
                            M = Movies.Movies;
                        }

                        for(int i = 0; i < M.Count; i++)
                        {
                            if (FilterBoxes[0].Input != "" && FilterBoxes[1].Input != "")
                            {
                                if (M[i].Name.ToLower().Contains(FilterBoxes[0].Input))
                                {
                                    if (M[i].Genre.ToLower().Contains(FilterBoxes[1].Input))
                                    {
                                        tempMoviesList.Add(M[i]);
                                    }
                                }
                            }
                            else if (M[i].Genre.ToLower().Contains(FilterBoxes[1].Input) && FilterBoxes[1].Input != "")
                            {
                                tempMoviesList.Add(M[i]);
                            }
                            else if (M[i].Name.ToLower().Contains(FilterBoxes[0].Input) && FilterBoxes[0].Input != "")
                            {
                                tempMoviesList.Add(M[i]);
                            }
                        }
                        
                        M = tempMoviesList;
                        maxpage = M.Count % 3 == 0 && M.Count != 0 ? M.Count / 3 : ((M.Count / 3) + 1);
                        start = 0;
                        end = 3;
                        pagenumber = 1;
                        FirstRender();


                    }
                    else if (M.Count != 0)
                    {
                        info.ChosenFilm = M[Index];
                        Program.information = info;
                        return 1;
                    }
                }


                int j = 5;

                if (end > M.Count)

                {
                    end = M.Count;
                }
                //draws all de boxes again with the new given index.
                if(end == 0)
                {
                    FilterBoxes[0].Display(Index);
                    FilterBoxes[1].Display(Index);
                    api.PrintCenter("There are no movies that fit your filter", (Console.WindowHeight / 2) );
                    api.PrintCenter("Remove the filter and hit Enter before applying another one", (Console.WindowHeight / 2) + 1);
                }
                for (int i = start; i < end; i++)
                {
                    if (i == Index)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                        Console.SetCursorPosition(0, j);
                        Console.WriteLine($"Title: {M[i].Name} ");
                        Console.ResetColor();

                        if (pagenumber == 1)
                        {
                            FilterBoxes[0].Display(Index);
                            FilterBoxes[1].Display(Index);
                        }

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.SetCursorPosition(0, j);
                        Console.WriteLine($"Title: {M[i].Name} ");
                        Console.ResetColor();


                        if (pagenumber == 1)
                        {
                            FilterBoxes[0].Display(Index);
                            FilterBoxes[1].Display(Index);
                        }
                    }

                    j = j + 8;

                }
                Console.SetCursorPosition(0, p);
            } while (key.Key != ConsoleKey.Escape);

            return 0;
        }
    }
}
