using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    internal class CateringMenu : IStructure
    {
        private int Index;
        private string Prompt;
        private List<api.Button> Buttons = new List<api.Button>();

        public CateringMenu()
        {
            Index = 0;
           
            Buttons = api.Button.CreateRow(new string[] { "Overview", "Logout" }, 5, 17);
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
            api.PrintCenter("Welcome back! What do you wish to do?", 11);

            DrawButtons();

            string footer = "ARROW KEYS - select options  |  ENTER - Confirm";
            Console.SetCursorPosition((Console.WindowWidth - footer.Length) / 2, 28);
            Console.WriteLine(footer);
        }

        public int Run()
        {
            Console.Clear();
            FirstRender();
            ConsoleKey keyPressed;
            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

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

            } while (keyPressed != ConsoleKey.Enter);

            if (Index == 1) {
                var info = Program.information;
                info.Member = null;
                Program.information = info;
                return -1;
            }
            
            return Index + 1;
        }

    }
}

