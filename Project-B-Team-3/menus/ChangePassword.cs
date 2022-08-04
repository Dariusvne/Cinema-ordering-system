using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    internal class ChangePassword : IStructure
    {
        private int Index;
        private List<api.Textbox> Textboxes = new List<api.Textbox>();

        public ChangePassword()
        {
            Index = 0;

            Textboxes.Add(new api.Textbox("Email", 0, (Console.WindowWidth - 20) / 2, 10));
            Textboxes.Add(new api.Textbox("New Password", 1, (Console.WindowWidth - 20) / 2, 13, hidden: true));
            Textboxes.Add(new api.Textbox("Confirm New Password", 2, (Console.WindowWidth - 20) / 2, 16, hidden: true));

        }

        public void FirstRender()
        {
            api.PrintCenter("Email:", 9);
            api.PrintCenter("New Password:", 12);
            api.PrintCenter("Confirm Password:", 15);
            string footer = "ARROW KEYS/TAB - Change box  |  ENTER - Finish  |  ESCAPE - Go back";
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

                if (keyPressed == ConsoleKey.Tab || keyPressed == ConsoleKey.DownArrow)
                {
                    if (Index < Textboxes.Count - 1)
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

                if (key.Key == ConsoleKey.Enter)
                {
                    if (Textboxes[0].Input == "")
                    {
                        api.PrintExact(" ".PadRight(Console.WindowWidth), 0, 4, ConsoleColor.Black, ConsoleColor.DarkRed);
                        api.PrintCenter("ERROR:  Email empty", 4, ConsoleColor.Black, ConsoleColor.DarkRed);
                        Console.Beep(100, 100);
                    }
                    else if (Textboxes[1].Input == "")
                    {
                        api.PrintExact(" ".PadRight(Console.WindowWidth), 0, 4, ConsoleColor.Black, ConsoleColor.DarkRed);
                        api.PrintCenter("ERROR:  New password empty", 4, ConsoleColor.Black, ConsoleColor.DarkRed);
                        Console.Beep(100, 100);
                    }
                    else if (Textboxes[2].Input == "")
                    {
                        api.PrintExact(" ".PadRight(Console.WindowWidth), 0, 4, ConsoleColor.Black, ConsoleColor.DarkRed);
                        api.PrintCenter(" ERROR:  Confirm password empty", 4, ConsoleColor.Black, ConsoleColor.DarkRed);
                        Console.Beep(100, 100);
                    }
                    else
                    {
                        AccountHandler check = new AccountHandler();
                        if (!check.EmailExists(Textboxes[0].Input))
                        {
                            api.PrintExact(" ".PadRight(Console.WindowWidth), 0, 4, ConsoleColor.Black, ConsoleColor.DarkRed);
                            api.PrintCenter(" No account found", 4, ConsoleColor.Black, ConsoleColor.DarkRed);
                            Thread.Sleep(1500);
                            return 0;
                        }
                        else if (SendEmail.IsValidEmail(Textboxes[0].Input) & check.EmailExists(Textboxes[0].Input) & check.PasswordCheck(Textboxes[1].Input, Textboxes[2].Input))
                        {
                            api.PrintExact(" ".PadRight(Console.WindowWidth), 0, 4, ConsoleColor.Black, ConsoleColor.DarkRed);
                            api.PrintCenter("Your password has been changed", 4, ConsoleColor.Black, ConsoleColor.Green);
                            Thread.Sleep(1500);
                            check.ChangePassword(Textboxes[0].Input, Textboxes[1].Input, Textboxes[2].Input);
                            return 0;
                        }
                        else if (!check.PasswordCheck(Textboxes[1].Input, Textboxes[2].Input))
                        {
                            api.PrintExact(" ".PadRight(Console.WindowWidth), 0, 4, ConsoleColor.Black, ConsoleColor.DarkRed);
                            api.PrintCenter(" ERROR:  Passwords do not match", 4, ConsoleColor.Black, ConsoleColor.DarkRed);
                        }
                        else
                        {
                            api.PrintExact(" ".PadRight(Console.WindowWidth), 0, 4, ConsoleColor.Black, ConsoleColor.DarkRed);
                            api.PrintCenter(" ERROR:  Email is wrong", 4, ConsoleColor.Black, ConsoleColor.DarkRed);
                        }
                    }
                }

                DisplayTextboxes();
            }
            while (key.Key != ConsoleKey.Escape);
            return 0;
        }
    }
}
