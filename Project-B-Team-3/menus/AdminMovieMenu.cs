using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    internal class AdminMovie : IStructure
    {
        private int Index;
        private string Prompt;
        private List<api.Button> Buttons = new List<api.Button>();

        public AdminMovie()
        {
            Index = 0;

            int x = Console.WindowWidth / 2 - ((19 + 10) / 2);
            Buttons.Add(new api.Button("Edit Movie", 0, x, 17));
            Buttons.Add(new api.Button("Add Movie", 1, x + 20, 17));

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

            string footer = "ARROW KEYS - select options  |  ENTER - Confirm  |  ESCAPE - Exit";
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

                if (keyPressed == ConsoleKey.Escape)
                {
                    return 0;
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

            } while (keyPressed != ConsoleKey.Enter);

            return Index+1;
        }

    }
}
