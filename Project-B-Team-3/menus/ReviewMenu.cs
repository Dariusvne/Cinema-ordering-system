using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    class ReviewMenu : IStructure
    {
        private int Index;
        private List<api.Button> Buttons = new List<api.Button>();

        public ReviewMenu()
        {
            Buttons = api.Button.CreateRow(new string[] { "Place review", "See review" }, 4, 17);
        }

        private void DrawButtons()
        {
            foreach (api.Button Button in Buttons)
            {
                Button.Display(Index);
            }
        }
        
        public void FirstRender()
        {
            DrawButtons();

            string footer = "ARROW KEYS - Change box  |  ENTER - Confirm  |  ESCAPE - Go back";
	        Console.SetCursorPosition((Console.WindowWidth - footer.Length) / 2, 28);
            Console.WriteLine(footer);
        }
        
        public int Run()
        {
            ConsoleKey keyPressed;
            Console.Clear();
            FirstRender();
            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                api.PrintExact(" ".PadRight(Console.WindowWidth), 0, 8, ConsoleColor.Black, ConsoleColor.DarkRed);
                keyPressed = keyInfo.Key;
                    
                if (keyPressed == ConsoleKey.Enter)
                {
                    if (Index == 0)
                    {
                        if (Program.information.Member == null)
                        {
                            api.PrintCenter("    You haven't logged in to an account yet.    ", 8, foreground: ConsoleColor.DarkRed);
                        }
                        else
                        {
                            return 1;
                        }
                    } else
                    {
                        return 2;
                    }
                }

                if (keyPressed == ConsoleKey.RightArrow)
                {
                    Index++;
                    if (Index > Buttons.Count - 1)
                    {
                        Index = 0;
                    }

                }
                if (keyPressed == ConsoleKey.LeftArrow)
                {
                    Index--;
                    if (Index < 0)
                    {
                        Index = Buttons.Count - 1;
                    }
                }
                DrawButtons();
            }
            while (keyPressed != ConsoleKey.Escape);
            return 0;
        }
    }
}
