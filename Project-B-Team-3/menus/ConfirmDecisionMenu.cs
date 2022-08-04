using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    internal class ConfirmDecisionMenu : IStructure
    {
        private int index;
        private api.Button[] areyousurebuttons;
        public ConfirmDecisionMenu()
        {
            
            
            areyousurebuttons = new api.Button[2];
            areyousurebuttons[0] = new api.Button("Confirm", 0, 45, 15);
            areyousurebuttons[1] = new api.Button("Deny", 1, 65, 15);
            
        }

        private void Draw_Buttons()
        {
            foreach(api.Button button in areyousurebuttons)
            {
                button.Display(index);
            }
        }

        public void FirstRender()
        {

            api.PrintCenter("| ARROW LEFT/RIGHT - Button | ENTER - Confirm | ESCAPE - Exit", 28);
            Draw_Buttons();
        }

        public int Run()
        {
            ConsoleKey keyPressed;
            Console.Clear();
            FirstRender();
            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.RightArrow)
                {
                    if (index == 0)
                    {
                        index++;
                       
                    }
                    else
                    {
                        index--;
                    }
                }
                else if (keyPressed == ConsoleKey.LeftArrow)
                {
                    if (index == 1)
                    {
                        index--;

                    }
                    else
                    {
                        index++;
                    }
                }
                else if (keyPressed == ConsoleKey.Enter)
                {
                    if (index == 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                Draw_Buttons();
            }
            while (keyPressed != ConsoleKey.Escape);
            return -1;
        }
    }
}
