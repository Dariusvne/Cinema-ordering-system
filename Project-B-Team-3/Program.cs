using System;

namespace ProjectB
{
    interface IStructure
    {
        void FirstRender();
        int Run();
        
    }

    class Program
    {
        // A stuct that tracks the information that is entered while the
        // program is being executed and interacted with. We, for example, need
        // to know what movie the user has selected and what seats got picked.
        public struct Information
        {
            public Movies ChosenFilm { get; set; }    // All these variables are null by default, so: variable == null = true when 
            public Account? Member { get; set; }          // it has never been set
            public int[][]? ChosenSeats { get; set; }
            public string ChosenTime { get; set; }
            public string ChosenDate { get; set; }
            public string VerificationCode { get; set; }
            public string RegistrationEmail { get; set; }
            public string[] AddMovieInfo { get; set; }

            public int SmallPopcornAmount { get; set; }
            public int MediumPopcornAmount { get; set; }
            public int LargePopcornAmount { get; set; }

            public int SmallDrinksAmount { get; set; }
            public int MediumDrinksAmount { get; set; }
            public int LargeDrinksAmount { get; set; }
        }

        public static Information information { get; set; }
        public static SettingsHandler.Settings settings { get; set; }
        public static PricesHandler prices { get; set; }

        // The main function of the system.
        static void Main(string[] args)
        {
            // Settings for the console application
            information = new Information();
            settings = SettingsHandler.Load();
            prices = new PricesHandler();

            Console.CursorVisible = false;

            Checks.CheckSystem();

            Manager game = new Manager();
            game.Start();
        }
    }

    // From here until the end of the file will only be classes that help us
    // make/code the system.



    #region api
    /// <summary>
    /// The class that contains
    public class api
    {
        /// <summary>
        /// Prints text in the middle of the screen at a specified y position
        /// </summary>
        /// <param name="text">text to write in the middle</param>
        /// <param name="y">The y position to write the text at</param>
        public static void PrintCenter(string text, int y, ConsoleColor? background = null, ConsoleColor? foreground = null)
        {
            if (background != null)
            {
                Console.BackgroundColor = background.GetValueOrDefault();
            }
            if (foreground != null)
            {
                Console.ForegroundColor = foreground.GetValueOrDefault();
            }
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, y);
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints something at a specific point on the screen
        /// </summary>
        /// <param name="text">The text to print</param>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position. Aka the line to print on (counted from up to down)</param>
        /// <param name="Background">The background that the text will have. Use System.ConsoleColor.(colorname)</param>
        /// <param name="Foreground">The color the characters will have. Use System.ConsoleColor.(colorname)</param>
        public static void PrintExact(string text, int x, int y, ConsoleColor? background = null, ConsoleColor? foreground = null)
        {
            if (background != null)
            {
                Console.BackgroundColor = background.GetValueOrDefault();
            }
            if (foreground != null)
            {
                Console.ForegroundColor = foreground.GetValueOrDefault();
            }
            Console.SetCursorPosition(x, y);
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Draws a line from start cordinates to the stop cordinates
        /// </summary>
        /// <param name="start">An array with two int in the format (x, y)</param>
        /// <param name="stop">The same as the start parameter but then where the line will stop</param>
        public static void DrawLine(int[] start, int[] stop)
        {

        }


        #region Button
        public class Button : Component
        {
            public string Title { get; init; }
            public int Index { get; init; }
            public int X { get; init; }
            public int Y { get; init; }

            /// <summary>
            /// This button class draw a button on the screen.
            /// </summary>
            /// <param name="title">The text that will apear on the button</param>
            /// <param name="index">This is the index of the button. When the current index (given in the Draw() fuction) is the same as the index, then </param>
            /// <param name="x">The x cordinate where the left side of the button will begin</param>
            /// <param name="y">The y cordinate (line) where the button will be placed</param>
            public Button(string title, int index, int x, int y)
            {
                Title = title;
                Index = index;
                X = x;
                Y = y;
            }

            /// <summary>
            /// Draws the button on the specified x and y coordinates
            /// </summary>
            /// <param name="current_index">It index the user is on at a specific screen</param>
            public override void Display(int current_index)
            {
                if (Index == current_index)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                Console.SetCursorPosition(X, Y);
                Console.WriteLine($" {Title} ");
                Console.ResetColor();
            }

            /// <summary>
            /// Makes a row of buttons in that are centered to the screen
            /// </summary>
            public static List<Button> CreateRow(string[] titles, int gap, int y, int start_index = 0)
            {
                var ButtonRow = new List<Button>();

                int total_length = gap * (titles.Length - 1);

                foreach (string title in titles) { total_length += title.Length + 2; }

                int x = ((Console.WindowWidth - total_length) / 2);

                for (int i = 0; i < titles.Length; i++)
                {
                    ButtonRow.Add(new Button(titles[i], start_index + i, x, y));
                    x += (titles[i].Length + 2) + gap;
                }

                return ButtonRow;
            }
        }
        #endregion

        /* Voor de mensen die na GrandOmega hier naar kijken en denken: M.. m.. maar je zet geen *this.* voor alle variablen dus je verwijst niet
         naar de instance van de class. Jawel, maar in .net (c#) is een variable automatisch een instance variable, dus hoef je dat niet ervoor te
        zetten. Als je een class variable wilt maken moet je er "static" voor stoppen, bijvoorbeeld: public static string ClassVariable*/

        #region Textbox
        public class Textbox : Component
        {
            protected string Placeholder;
            public string Input = "";
            public int Index;
            protected int X;
            protected int Y;
            protected bool Hidden;

            protected List<char> allowed = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-=_+`~{}[]:;'\"\\|<>,./?!@#$%^&*()".ToCharArray().ToList();

            /// <summary>
            /// A textbox class that can accept input and displays it
            /// </summary>
            /// <param name="placeholder">The text that will show when no input has been entered</param>
            /// <param name="index">The unimovie index of the button</param>
            /// <param name="x">The x cordinate where the textbox will be displayed</param>
            /// <param name="y">The y cordiante </param>
            public Textbox(string placeholder, int index, int x, int y, bool space_allowed = false, bool hidden = false)
            {
                Placeholder = placeholder;
                Index = index;
                X = x;
                Y = y;
                Hidden = hidden;

                if (space_allowed)
                {
                    allowed.Add(' ');
                }
            }

            /// <summary>
            /// Adds a character to the input of the textbox
            /// </summary>
            /// <param name="character">Here you pass the char that the user has entered while the textbox is selected</param>
            public virtual void AddLetter(char character)
            {
                if (allowed.Contains(character))
                {
                    Input += character;
                }
            }

            /// <summary>
            /// Removes a character from the input of the textbox
            /// </summary>
            public void Backspace()
            {
                if (Input != "")
                {
                    Input = Input.Remove(Input.Length - 1, 1);
                }
            }

            /// <summary>
            /// Displays the Textbox on the screen at the specified cordinates.
            /// </summary>
            /// <param name="current_index">The current index is the index that the user is currently on in the menu</param>
            public override void Display(int current_index)
            {
                Placeholder = Placeholder.PadRight(20);

                if (Input.PadRight(20) == "                    ")
                {
                    if (Index == current_index)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.BackgroundColor = ConsoleColor.Gray;

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    Console.SetCursorPosition(X, Y);
                    Console.WriteLine(Placeholder);


                }
                else
                {
                    if (Index == current_index)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    if (Hidden)
                    {
                        if (Input.Length < 20)
                        {
                            Console.SetCursorPosition(X, Y);
                            Console.WriteLine(new string('*', Input.Length).PadRight(20));
                        }
                        else
                        {
                            Console.SetCursorPosition(X, Y);
                            Console.WriteLine(new string('*', Input.Length).Remove(0, Input.Length - 20));
                        }
                    }
                    else
                    {
                        if (Input.Length < 20)
                        {
                            Console.SetCursorPosition(X, Y);
                            Console.WriteLine(Input.PadRight(20));
                        }
                        else
                        {
                            Console.SetCursorPosition(X, Y);
                            Console.WriteLine(Input.Remove(0, Input.Length - 20));
                        }
                    }
                }
                Console.ResetColor();
            }
        }

        public class BigTextbox : Component
        {
            protected string Placeholder;
            public string Input = "";
            protected int Length;
            protected int Width;
            protected int Index;
            protected int X;
            protected int Y;

            protected List<char> allowed = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-=_+`~{}[]:;'\"\\|<>,./?!@#$%^&*()".ToCharArray().ToList();

            /// <summary>
            /// A textbox class that can accept input and displays it
            /// </summary>
            /// <param name="placeholder">The text that will show when no input has been entered</param>
            /// <param name="index">The unimovie index of the button</param>
            /// <param name="x">The x cordinate where the textbox will be displayed</param>
            /// <param name="y">The y cordiante </param>
            public BigTextbox(string placeholder, int index, int x, int y, bool space_allowed = false, int width = 40, int length = 3)
            {
                Length = length;
                Placeholder = placeholder.PadRight(80);

                Index = index;
                X = x;
                Y = y;
                Width = width;
                Length = length;

                if (space_allowed)
                {
                    allowed.Add(' ');
                }
            }

            /// <summary>
            /// Adds a character to the input of the textbox
            /// </summary>
            /// <param name="character">Here you pass the char that the user has entered while the textbox is selected</param>
            public virtual void AddLetter(char character)
            {
                if (allowed.Contains(character))
                {
                    Input += character;
                }
            }

            /// <summary>
            /// Removes a character from the input of the textbox
            /// </summary>
            public void Backspace()
            {
                if (Input != "")
                {
                    Input = Input.Remove(Input.Length - 1, 1);
                }
            }

            /// <summary>
            /// Displays the Textbox on the screen at the specified cordinates.
            /// </summary>
            /// <param name="current_index">The current index is the index that the user is currently on in the menu</param>
            public override void Display(int current_index)
            {
                if (Input.Length == 0)
                {
                    // Setting the color for to different colors based on if it is selceted or not
                    if (Index == current_index)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.BackgroundColor = ConsoleColor.Gray;

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    // Drawing all the rows
                    for (int row = 0; row < Length; row++)
                    {
                        Console.SetCursorPosition(X, Y + row);
                        if (row == 0)
                        {
                            Console.WriteLine(Placeholder.PadRight(Width));
                        }
                        else
                        {
                            Console.WriteLine("".PadRight(Width));
                        }
                    }
                }
                else
                {
                    if (Index == current_index)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    string tempInput = Input;
                    List<string> LineList = new List<string>();

                    int Counter = 1;
                    int lastSplitIndex = 0;
                    for (int LetterIndex = 0; LetterIndex < Input.Length; LetterIndex++)
                    {
                        if (LetterIndex == Input.Length - 1)
                        {
                            LineList.Add(tempInput.Substring(lastSplitIndex, LetterIndex - lastSplitIndex + 1));
                        }
                        else
                        {
                            if (Counter > Width - 10)
                            {
                                if (Input.ElementAt(LetterIndex) == ' ')
                                {
                                    LineList.Add(tempInput.Substring(lastSplitIndex, LetterIndex - lastSplitIndex));
                                    lastSplitIndex = LetterIndex + 1;
                                    Counter = 0;
                                }
                                if (Counter == Width)
                                {
                                    LineList.Add(tempInput.Substring(lastSplitIndex, LetterIndex - lastSplitIndex + 1));
                                    lastSplitIndex = LetterIndex + 1;
                                    Counter = 0;
                                }
                            }
                        }
                        Counter++;
                    }

                    if (LineList.Count > 0)
                    {
                        for (int row = 0; row < Length; row++)
                        {
                            Console.SetCursorPosition(X, Y + row);
                            if (row < LineList.Count)
                            {
                                Console.WriteLine(LineList[row].PadRight(Width));
                            }
                            else
                            {
                                Console.WriteLine("".PadRight(Width));
                            }
                        }
                    }
                }
                Console.ResetColor();
            }
        }
        #endregion


        #region ConditionalTextbox
        public class ConditionalTextbox : Textbox
        {
            private int MinInputLength;
            private int MaxInputLength;
            private Func<char, bool> CheckFunction;

            /// <summary>
            /// The conditional textbox is a textbox that works the same as a normal textbox, but has some added tweaking features to it. You can
            /// for example set a maximun amount of characters the input can take. It's also possible to set a minimun amount of characters. If the length
            /// of the input is below the minimum, then the text in the Textbox will be red.
            /// </summary>
            /// <param name="placeholder">The text that is shown in the box when no input has been entered by the user</param>
            /// <param name="index">If the index is equal the the 'current index' in a menu, then it will light up, indicating that is is selected</param>
            /// <param name="x">The x position that the input box will be drawn at</param>
            /// <param name="y">The line (counted from upper to bottom) that the box will be drawn on</param>
            /// <param name="min">If the entered input's length is lowed than this int, the text in the box will be red</param>
            /// <param name="max">The max amount of characters the input will take</param>
            // /// <param name="hidden">If this is set to true, then the entered input will be hidden and replaced with '*' characters</param>
            public ConditionalTextbox(string placeholder,
                      int index,
                      int x, int y,
                      int min = 0, int max = 100,
                      Func<char, bool> AddLetterCheck = null) :
                              base(placeholder, index, x, y)
            {
                MinInputLength = min;
                MaxInputLength = max;

                if (AddLetterCheck == null)
                {
                    CheckFunction = (chr) => true;
                }
                else
                {
                    CheckFunction = AddLetterCheck;
                }
            }

            /// <summary>
            /// Adds a character to the input of the textbox
            /// </summary>
            /// <param name="character">
            /// Here you pass the char that the user has entered while the textbox is selected.
            /// Unlike the Textbox class, this class only adds the character to the Input string variable if it will not exceed
            /// the max MaxInputLength
            /// </param>
            public override void AddLetter(char character)
            {

                if (allowed.Contains(character) && Input.Length < MaxInputLength && CheckFunction(character))
                {
                    Input += character;
                }
            }

            /// <summary>
            /// Displays the Textbox on the screen at the specified cordinates.
            /// </summary>
            /// <param name="current_index">The current index is the index that the user is currently on in the menu</param>
            public override void Display(int current_index)
            {
                Placeholder = Placeholder.PadRight(20);

                if (Input.PadRight(20) == "                    ")
                {
                    if (Index == current_index)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.BackgroundColor = ConsoleColor.Gray;

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    Console.SetCursorPosition(X, Y);
                    Console.WriteLine(Placeholder);


                }
                else
                {
                    if (Index == current_index)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Gray;
                        if (Input.Length < MinInputLength)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        if (Input.Length < MinInputLength)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                    }

                    if (Input.Length < 20)
                    {
                        Console.SetCursorPosition(X, Y);
                        Console.WriteLine(Input.PadRight(20));
                    }
                    else
                    {
                        Console.SetCursorPosition(X, Y);
                        Console.WriteLine(Input.Remove(0, Input.Length - 20));
                    }
                }
                Console.ResetColor();
            }
        }
        #endregion
        
        #region Slider
        public class Slider : Component
        {
            private int _amount;
            public int Amount { get { return _amount; } }
            private int Index;
            private int X;
            private int Y;
            private int Max;

            public Slider(int index, int x, int y, int max)
            {
                X = x;
                Y = y;
                Max = max;
                Index = index;
            }

            public void PlusOne()
            {
                if (Amount < Max)
                {
                    _amount += 1;
                }
            }

            public void MinusOne()
            {
                if (Amount > 0)
                {
                    _amount -= 1;
                }
            }

            public override void Display(int currentIndex)
            {
                if (Index != currentIndex)
                {
                    api.PrintExact($"{Amount}".PadLeft(3).PadRight(5), X, Y, background: ConsoleColor.DarkGray, foreground: ConsoleColor.Black);                    
                }
                else
                {
                    api.PrintExact($"{Amount}".PadLeft(3).PadRight(5), X, Y, background: ConsoleColor.Gray, foreground: ConsoleColor.Black);
                }

                api.PrintExact("<", X - 2, Y);
                api.PrintExact(">", X + 6, Y);
            }
        }
        #endregion

        #region DecimalSlider
        public class DecimalSlider : Component
        {
            private Double _amount;
            public Double Amount { get { return _amount; } }
            private int Index;
            private int X;
            private int Y;
            private Double Max;

            public DecimalSlider(int index, int x, int y, Double max, Double? initialValue = null)
            {
                X = x;
                Y = y;
                Max = max;
                Index = index;

                if (initialValue != null)
                {
                    _amount = initialValue.GetValueOrDefault();
                }
            }

            public void PlusOne()
            {
                if (Amount < Max)
                {
                    _amount += 0.01;
                }
            }

            public void MinusOne()
            {
                if (Amount > 0.11)
                {
                    _amount -= 0.01;
                }
            }

            public override void Display(int currentIndex)
            {
                if (Index != currentIndex)
                {
                    api.PrintExact($"{Amount.ToString("0.00")}".PadLeft(5).PadRight(6), X, Y, background: ConsoleColor.DarkGray, foreground: ConsoleColor.Black);                    
                }
                else
                {
                    api.PrintExact($"{Amount.ToString("0.00")}".PadLeft(5).PadRight(6), X, Y, background: ConsoleColor.Gray, foreground: ConsoleColor.Black);
                }

                api.PrintExact("<", X - 2, Y);
                api.PrintExact(">", X + 7, Y);
            }
        }
        #endregion
        
        /// <summary>
        /// An abstract class for all the screen classes in the program.
        /// </summary>
        public abstract class BaseScreen
        {
            public abstract int Run();
        }

        public abstract class Component
        {
            public abstract void Display(int current_index);
        }
    }
    #endregion
}
