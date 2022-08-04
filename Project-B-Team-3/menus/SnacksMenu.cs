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
    internal class SnacksMenu : IStructure
	{
        private int[] Index = new int[] {1, 0};
        private List<api.Component> Components;
        private const int OFFSET = 0;


        private ReservationsHandler accounthandler = new ReservationsHandler();

        private Dictionary<int, List<int>> _seats = new Dictionary<int, List<int>>();
        private int _popcornAmount = 0;

        public SnacksMenu()
        {
            Components.AddRange(api.Button.CreateRow(new string[] { "Go Back", "Confirm" }, 3, 25));
            Components.Add(new api.Slider(3, 3, 2, 3));
        }

        public void FirstRender()
        {
            api.PrintCenter("Do you want to order any snacks for during the movie?", 5);
            
            
            
            string footer = "ARROW KEYS - Change box  |  ENTER - Confirm  |  ESCAPE - Go back";
            Console.SetCursorPosition((Console.WindowWidth - footer.Length) / 2, 28);
            Console.WriteLine(footer);
        }

        public void DrawButtons()
        {
            foreach (var button in Components)
            {
                if (Index[0] == 1)
                {
                    button.Display(Index[1]);
                }
                else
                {
                    button.Display(-1);
                }
            }
        }

       
        public int Run()
        {
            Console.Clear();
            FirstRender();
            ConsoleKeyInfo key;

             do
            {
                var info = Program.information;
                key = Console.ReadKey(true);
                ConsoleKey keyPressed = key.Key;

		if (keyPressed == ConsoleKey.Enter)
		{
		    if (Index[1] == 0)
		    {
                        return -1;
                    }
		    if (Index[1] == 1)
		    {
			return 1;
                    }
		}
		if (keyPressed == ConsoleKey.UpArrow)
		{
		    if (Index[0] == 1)
		    {
			Index[0]--;
		    }
		}
		if (keyPressed == ConsoleKey.DownArrow)
		{
		    if (Index[0] == 0)
		    {
			Index[0]++;
		    }
		}
		    
                if (keyPressed == ConsoleKey.RightArrow)
                {
		    if (Index[0] == 0)
		    {
			if (_popcornAmount < 100)
			{
			    _popcornAmount++;
			}
		    }
		    else
		    {
			if (Index[1] < Components.Count - 1)
			{
			    Index[1]++;
			}
			else
			{
			    Index[1] = 0;
			}
		    }
                }
                else if (keyPressed == ConsoleKey.LeftArrow)
                {
		    if (Index[0] == 0)
		    {
			if (_popcornAmount > 0)
			{
			    _popcornAmount--;
			}
		    }
		    else
		    {			
			if (Index[1] > 0)
			{
			    Index[1]--;
			}
		    }
                }
                DrawButtons();
            }
            while (key.Key != ConsoleKey.Escape);
	    
            return 1;
        }
    }
}
