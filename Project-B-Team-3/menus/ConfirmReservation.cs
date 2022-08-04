using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using ProjectB;

namespace ProjectB
{
    internal class ConfirmReservation : IStructure
    {
        public void FirstRender()
        {
            api.PrintCenter(Program.information.Member.Firstname + " " + Program.information.Member.Lastname + ", thank you for your reservation!", 13);
            api.PrintCenter("An confirmation email has been sent.", 14);
            api.PrintCenter("Enjoy!", 15);
            api.PrintCenter("press Enter to confirm...", 17);
        }

        public int Run()
        {
            Console.Clear();
            ConsoleKey keyPressed;
            FirstRender();
            do
            {
                var info = Program.information;
                ConsoleKeyInfo key = Console.ReadKey(true);
                keyPressed = key.Key;

                if (keyPressed == ConsoleKey.Enter)
                {
                    int[][] seats = info.ChosenSeats;
                    string SeatsStr = $"R{seats.First()[0]}S{seats.First()[1]}";
                    foreach (int[] seat in seats)
                    {
                        if (seat != seats.First())
                        {
                            SeatsStr += $"|R{seat[0]}S{seat[1]}";
                        }
                    }
                    var Dict = new Dictionary<string, string>() {
                        { "{{Username}}", info.Member.Email                         },
                        { "{{MovieDate}}", info.ChosenDate                          },
                        { "{{MovieTime}}", info.ChosenTime                          },
                        { "{{MovieTitle}}", info.ChosenFilm.Name                    },
                        { "{{Seats}}", SeatsStr                                     },
                        { "{{Popcorn_Small}}", info.SmallPopcornAmount.ToString()   },
                        { "{{Popcorn_Medium}}", info.MediumPopcornAmount.ToString() },
                        { "{{Popcorn_Large}}", info.LargePopcornAmount.ToString()   },
                        { "{{Drinks_Small}}", info.SmallDrinksAmount.ToString()     },
                        { "{{Drinks_Medium}}", info.MediumDrinksAmount.ToString()   },
                        { "{{Drinks_Large}}", info.LargeDrinksAmount.ToString()    }}; // Defined vars to be replaced in mail template
                    
                    SendEmail.SendReservationEmail(Program.information.Member.Email, "htmlReservation.txt", Dict); // Sends the email

                    var temp = Program.information;
                    temp.ChosenSeats = null;
                    Program.information = temp;

                    return 0;
                }

            } while (true);
            return 0;
        }
    }
}
