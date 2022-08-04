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
    internal class LoginMenu : IStructure
    {
        private int Index;
        private bool changePasswordButton;
        private List<api.Component> Boxes = new List<api.Component>();
        public List<Account> accountList;
        AccountHandler Accounts = new AccountHandler();

        public LoginMenu(bool changepasswordButton)
        {
            Index = 0;
            changePasswordButton = changepasswordButton;
            Boxes.Add(new api.Textbox("E-mail", 0, (Console.WindowWidth - 20) / 2, 14));
            Boxes.Add(new api.Textbox("Password", 1, (Console.WindowWidth - 20) / 2, 16, hidden: true));

            if (changepasswordButton)
            {
                Boxes.Add(new api.Button("Change password", 2, (Console.WindowWidth - 17) / 2, 18));
            }
        }

        public void FirstRender()
        {
            api.PrintCenter("Login", 12);
            string footer = "ARROW KEYS/TAB - Change box  |  ENTER - Finish  |  ESCAPE - Go back";
            Console.SetCursorPosition((Console.WindowWidth - footer.Length) / 2, 28);
            Console.WriteLine(footer);
            DisplayTextboxes();
        }

        public void DisplayTextboxes()
        {
            foreach (var box in Boxes)
            {
                if (box is api.Textbox)
                {
                    box.Display(Index);
                }
                else if (box is api.Button && changePasswordButton)
                {
                    box.Display(Index);
                }
            }
        }

        public int Run()
        {
            Console.Clear();
            FirstRender();
            var info = Program.information;
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                ConsoleKey keyPressed = key.Key;

                
		
                if (keyPressed == ConsoleKey.Tab || keyPressed == ConsoleKey.DownArrow)
                {
                    if (Index < Boxes.Count-1)
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
                        Index = Boxes.Count-1;
                    }
                }

                if (Index < Boxes.Count-1)
                {
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        (Boxes[Index] as api.Textbox).Backspace();
                    }
                    else
                    {
                        (Boxes[Index] as api.Textbox).AddLetter(key.KeyChar);
                    }
                }
                else if(Index <= Boxes.Count-1 && Boxes.Count == 2)
                {
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        (Boxes[Index] as api.Textbox).Backspace();
                    }
                    else
                    {
                        (Boxes[Index] as api.Textbox).AddLetter(key.KeyChar);
                    }
                }


                if (key.Key == ConsoleKey.Enter)
                {
                    var account = Accounts.Exists((Boxes[0] as api.Textbox)!.Input.ToLower(), (Boxes[1] as api.Textbox)!.Input);
                    if (Index == 2)
                    {
                        return 3;
                    }

                    if (account != null)
                    {
                        if ((Boxes[0] as api.Textbox)!.Input.Equals("admin") && (Boxes[1] as api.Textbox)!.Input.Equals("admin"))
                        {
                            if (changePasswordButton)
                            {
                                info.Member = account;
                                Program.information = info;
                                return 2;
                            }
                            else
                            {
                                if (changePasswordButton)
                                {
                                    api.PrintCenter("Invalid email or password!", 20, foreground: ConsoleColor.DarkRed);
                                }
                                else
                                {
                                    api.PrintCenter("Invalid email or password!", 18, foreground: ConsoleColor.DarkRed);
                                }
                            }
                        }
                        else if ((Boxes[0] as api.Textbox)!.Input.Equals("catering") && (Boxes[1] as api.Textbox)!.Input.Equals("catering"))
                        {
                            if (changePasswordButton)
                            {
                                info.Member = account;
                                Program.information = info;
                                return 4;
                            }
                        }
                        else
                        {
                            info.Member = account;
                            Program.information = info;
                            return 1;
                        }
                    }
                    else
                    {
                        if (changePasswordButton)
                        { 
                            api.PrintCenter("Invalid email or password!", 20, foreground: ConsoleColor.DarkRed);
                        }
                        else
                        {
                            api.PrintCenter("Invalid email or password!", 18, foreground: ConsoleColor.DarkRed);
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
