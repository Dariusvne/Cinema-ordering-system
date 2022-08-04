using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    internal class Welcome : api.BaseScreen, IStructure
    {
        private int Index;
        private string Prompt;
        private List<api.Button> Buttons;

        public Welcome(string prompt, int index)
        {
            Prompt = prompt;
            Index = index;

            string[] buttontitles = { "Choose Film", "Login", "Register", "Reviews" };
            Buttons = api.Button.CreateRow(buttontitles, 3, 17);
        }

        public void FirstRender()
        {

            Console.SetCursorPosition(0, 8);

            int y = 8;
            foreach (var line in Prompt.Split('\n'))
	    {
                api.PrintCenter(line, y);
                y++;
            }
            // Console.WriteLine(Prompt);

            DrawButtons();

            string footer = "ARROW KEYS - select options  |  ENTER - Confirm  |  ESCAPE - Exit";
            Console.SetCursorPosition((Console.WindowWidth - footer.Length) / 2, 28);
            Console.WriteLine(footer);
        }

        private void DrawButtons()
        {
            foreach (api.Button Button in Buttons)
            {  
                Button.Display(Index);
            }
        }


        public override int Run()
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
                    return 4;
                }
                if (keyPressed == ConsoleKey.F12)
                {
                    return 42069;
                }

                if (keyPressed == ConsoleKey.RightArrow)
                {
                    Index++;
                    if (Index > Buttons.Count-1)
                    {
                        Index = 0;
                    }

                }
                if (keyPressed == ConsoleKey.LeftArrow)
                {
                    Index--;
                    if (Index < 0)
                    {
                        Index = Buttons.Count-1;
                    }
                }
                DrawButtons();

            } while (keyPressed != ConsoleKey.Enter);

            return Index;
        }
    }
}
