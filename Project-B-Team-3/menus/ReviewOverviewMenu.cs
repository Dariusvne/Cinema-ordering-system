using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    internal class ReviewOverview : IStructure
    {
        private int start;
        private int end;
        private int pagenumber;
        public int Index;
        public List<Review> R;

        public ReviewOverview()
        {
            var Reviews = new ReviewsList();
            R = Reviews._reviews;
        }

        private void DisplayMenu(int start, int end, int pagenumber)
        {
            Console.Clear();
            api.PrintCenter("<<*See all reviews*>>", 1);
            if (Program.information.Member == null || (Program.information.Member.Email != "admin" && Program.information.Member.Password != "admin"))
            {
                api.PrintCenter("ARROW UP/DOWN - Scroll reviews | ARROW LEFT/RIGHT - Select page | ESCAPE - Exit", 28);
            }
            else
            {
                api.PrintCenter("ARROW UP/DOWN - Scroll reviews | ARROW LEFT/RIGHT - Select page | ENTER - Delete review | ESCAPE - Exit", 28);
            }

            if (R.Count % 3 != 0 || R.Count == 0)
            {
                api.PrintCenter("Page " + pagenumber + "/" + ((R.Count / 3) + 1), 2);
            }
            else if (R.Count <= 3 || R.Count % 3 == 0)
            {
                api.PrintCenter("Page " + pagenumber + "/" + (R.Count / 3), 2);

            }

        }

        public void FirstRender()
        {
            int j = 5;


            if (end > R.Count)

            {
                end = R.Count;
            }

            DisplayMenu(start, end, pagenumber);

            for (int i = start; i < end; i++)
            {
                if (i == Index)
                {
                    
                    int starXposition = 7;
                    for (int p = 0; p < 5; p++)
                    {
                        Console.SetCursorPosition(starXposition, j + 1);
                        if (p < R[i].Rating)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("*", j + 1);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine("*", j + 1);
                        }
                        starXposition++;
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(0, j + 1);
                    Console.WriteLine("Rating:", j + 1);
                    Console.SetCursorPosition(0, j);
                    Console.WriteLine($"Name: {R[i].Name} ");
                    Console.SetCursorPosition(0, j + 2);
                    Console.WriteLine("Review: ", j + 1);
                    Console.SetCursorPosition(0, j + 3);
                    Console.WriteLine(R[i].Text);
                    Console.ResetColor();
                }
                else
                {
                  
                    int starXposition = 7;
                    for (int p = 0; p < 5; p++)
                    {
                        Console.SetCursorPosition(starXposition, j + 1);
                        if (p < R[i].Rating)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("*", j + 1);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("*", j + 1);
                            Console.ResetColor();
                        }
                        starXposition++;
                    }
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.SetCursorPosition(0, j + 1);
                    Console.WriteLine("Rating:", j + 1);
                    Console.SetCursorPosition(0, j);
                    Console.WriteLine($"Name: {R[i].Name} ");
                    Console.SetCursorPosition(0, j + 2);
                    Console.WriteLine("Review: ", j + 1);
                    Console.SetCursorPosition(0, j + 3);
                    Console.WriteLine(R[i].Text);
                    Console.ResetColor();
                }

                j = j + 8;

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
            int maxpage = R.Count % 3 == 0 ? R.Count / 3 : ((R.Count / 3) + 1);
            if (R.Count <= 3)
            {
                maxpage = 1;
            }

            if (end > R.Count)

            {
                end = R.Count;
            }

            Console.Clear();
            ConsoleKey keyPressed;
            FirstRender();
            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;
                int p = 0;
                if (keyPressed == ConsoleKey.DownArrow && Index < end - 1)
                {
                    Index++;
                }
                else if (keyPressed == ConsoleKey.UpArrow && Index > page)
                {
                    Index--;
                }
                //when the left arrow key is pressed it will swap te the previes page or when up arrow is pressed and the index is equal to the start variable.
                else if (keyPressed == ConsoleKey.LeftArrow && start > 0 || (keyPressed == ConsoleKey.UpArrow && Index == start && pagenumber > 1))
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
                    if (end > R.Count)

                    {
                        end = R.Count;
                    }
                }
                //when the right arrow key is pressed it will swap te the next page or when down arrow is pressed and the index is equal to the end variable.
                else if ((keyPressed == ConsoleKey.RightArrow && end < R.Count) || (keyPressed == ConsoleKey.DownArrow && Index == end - 1 && pagenumber < maxpage))
                {
                    page = page + 3;
                    pagenumber++;
                    Index = page;
                    start += 3;
                    end += 3;
                    Console.Clear();
                    FirstRender();
                    if (end > R.Count)

                    {
                        end = R.Count;
                    }
                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    if (Program.information.Member != null && R.Count != 0 && Program.information.Member.Email == "admin" && Program.information.Member.Password == "admin")
                    {
                        var deleteReview = new ReviewsList();
                        var ConfirmDecision = new ConfirmDecisionMenu();
                        int outcome = ConfirmDecision.Run();
                        if (outcome == -1)
                        {
                            deleteReview.Remove(Index);
                            return -1;
                        }
                        return -1;
                    }
                }

                int j = 5;
                //draws als de boxes again with the new given index.
                for (
                    int i = start; i < end; i++)
                {
                    if (i == Index)
                    {
                        int starXposition = 7;
                        for (int q = 0; q < 5; q++)
                        {
                            Console.SetCursorPosition(starXposition, j + 1);
                            if (q < R[i].Rating)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("*", j + 1);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine("*", j + 1);
                            }
                            starXposition++;
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(0, j + 1);
                        Console.WriteLine("Rating:", j + 1);
                        Console.SetCursorPosition(0, j);
                        Console.WriteLine($"Name: {R[i].Name} ");
                        Console.SetCursorPosition(0, j + 2);
                        Console.WriteLine("Review: ", j + 1);
                        Console.SetCursorPosition(0, j + 3);
                        Console.WriteLine(R[i].Text);
                        Console.ResetColor();
                    }
                    else
                    {
                        int starXposition = 7;
                        for (int q = 0; q < 5; q++)
                        {
                            Console.SetCursorPosition(starXposition, j + 1);
                            if (q < R[i].Rating)
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine("*", j + 1);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine("*", j + 1);
                                Console.ResetColor();
                            }
                            starXposition++;
                        }
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.SetCursorPosition(0, j + 1);
                        Console.WriteLine("Rating:", j + 1);
                        Console.SetCursorPosition(0, j);
                        Console.WriteLine($"Name: {R[i].Name} ");
                        Console.SetCursorPosition(0, j + 2);
                        Console.WriteLine("Review: ", j + 1);
                        Console.SetCursorPosition(0, j + 3);
                        Console.WriteLine(R[i].Text);
                        Console.ResetColor();
                    }

                    j = j + 8;

                }
                Console.SetCursorPosition(0, p);
            } while (keyPressed != ConsoleKey.Escape);
            if (Program.information.Member == null || (Program.information.Member.Email != "admin" && Program.information.Member.Password != "admin"))
            {
                return 0;
            }
            return 1;
        }
    }
}