using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB
{
    internal class ConfirmCode : IStructure
    {
        private int Index;
        private List<api.Textbox> codeBox = new List<api.Textbox>();

        public ConfirmCode()
        {
            codeBox.Add(new api.ConditionalTextbox("Verification Code: ", 0, (Console.WindowWidth - 20) / 2, 15, 6, 6));
        }

        private void DrawTextBoxes()
        {
            foreach (api.Textbox box in codeBox)
            {
                box.Display(Index);
            }
        }

        public void FirstRender()
        {
            api.PrintCenter("Verification Code:", 14);

            DrawTextBoxes();

            string footer = "ENTER - Finish  |  ESCAPE - Cancel";
            Console.SetCursorPosition((Console.WindowWidth - footer.Length) / 2, 28);
            Console.WriteLine(footer);
        }

        public static bool CheckCode(string input, string check)
        {
            if (input == check)
            {
                return true;
            }
            return false;
        }

        public int Run()
        {
            Console.Clear();
            ConsoleKey keyPressed;
            FirstRender();
            do
            {
                var info = Program.information;
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.Backspace)
                {
                    codeBox[Index].Backspace();
                    DrawTextBoxes();
                }

                if (keyPressed == ConsoleKey.Escape)
                {
                    var cancelRegistration = new AccountHandler();
                    cancelRegistration.Remove(Program.information.RegistrationEmail);
                    info.VerificationCode = null;
                    info.RegistrationEmail = null;
                    Program.information = info;

                    return 0;
                }

                if (Index < codeBox.Count)
                {
                    if (Index == 0)
                    {
                        if (int.TryParse(keyInfo.KeyChar.ToString(), out int number))
                        {
                            codeBox[Index].AddLetter(keyInfo.KeyChar);
                        }
                    }
                    else
                    {
                        codeBox[Index].AddLetter(keyInfo.KeyChar);
                    }
                }

                if (keyPressed == ConsoleKey.Enter)
                {
                    if (!CheckCode(codeBox[0].Input, Program.information.VerificationCode))
                    {
                        api.PrintCenter("ERROR:  Verification code is incorrect", 12, ConsoleColor.Black, ConsoleColor.DarkRed);
                    }
                    else
                    {
                        api.PrintExact(" ".PadRight(Console.WindowWidth), 0, 12, ConsoleColor.Black, ConsoleColor.DarkRed);
                        api.PrintCenter("Verification completed", 12, ConsoleColor.Black, ConsoleColor.Green);
                        Thread.Sleep(1500);

                        return 0;
                    }
                }

                DrawTextBoxes();
            }
            while(keyPressed != ConsoleKey.Escape);
            return 0;
        }
    }
}
