using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ProjectB
{
    internal class Manager
    {
        public int MainMenu_LastIndex = 0;
        public void Start()
        {
            RunStartingMenu();
        }

        private void RunStartingMenu()
        {
            string prompt = @"
__          __  _                            
\ \        / / | |                           
 \ \  /\  / /__| | ___ ___  _ __ ___   ___   
  \ \/  \/ / _ \ |/ __/ _ \| '_ ` _ \ / _ \  
   \  /\  /  __/ | (_| (_) | | | | | |  __/  
    \/  \/ \___|_|\___\___/|_| |_| |_|\___|  
";
            string[] options = { "Choose Film", "Login", "Register", "Reviews" };
            Welcome mainMenu = new Welcome(prompt, MainMenu_LastIndex);
            int selectedIndex = mainMenu.Run();

            MainMenu_LastIndex = selectedIndex;

            switch (selectedIndex)
            {
                case 0:
                    MoviesMenu();
                    break;
                case 1:
                    LoginMenu();
                    break;
                case 2:
                    RegisterMenu();
                    break;
                case 3:
                    ReviewsMenu();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
            }
        }

        private void LoginMenu()
        {
            LoginMenu Login = new LoginMenu(true);
            int index = Login.Run();
            switch (index)
            {
                case 0:
                    RunStartingMenu();
                    break;
                case 1:
                    RunLoggedInMenu();
                    break;
                case 2:
                    RunAdminMenu();
                    break;
                case 3:
                    RunChangePasswordMenu();
                    break;
                case 4:
                    CateringMenu();
                    break;
                    
            }
        }

        private void MoviesMenu()
        {
            MovieSelection Movie = new MovieSelection();
            int index = Movie.Run();
            switch (index)
            {
                case 0:
                    if (Program.information.Member == null)
                    {
                        RunStartingMenu();
                    }
                    else
                    {
                        RunLoggedInMenu();
                    }
                    break;
                case 1:
                    TimeSelectionMenu();
                    break;
            }
        }
	
        private void TimeSelectionMenu()
        {
            TimeSelection Time = new TimeSelection();
            int index = Time.Run();
            switch (index)
            {
                case 0:
                    MoviesMenu();
                    break;
                case 1:
                    SeatsChoosingMenu();
                    break;
            }

        }

        private void RegisterMenu()
        {
            Register register = new Register();
            int index  = register.Run();
            switch (index)
            {
                case 0:
                    RunStartingMenu();
                    break;
                case 1:
                    ConfirmCodeMenu();
                    break;
            }
        }

        private void CateringMenu()
        {
            CateringMenu cartering = new CateringMenu();
            switch (cartering.Run())
            {    
                case 1:
                    FoodOverviewMenu();
                    break;
                case -1:
                    RunStartingMenu();
                    break;
            }
        }

        private void FoodChosingMenu()
        {
            FoodMenu foodMenu = new FoodMenu();
            switch (foodMenu.Run())
            {
                case 1:
                    RunOverviewMenu();
                    break;
                case -1:
                    SeatsChoosingMenu();
                    break;
            }
        }

        private void FoodOverviewMenu()
        {
            FoodOverviewMenu foodoverview = new FoodOverviewMenu();
            switch (foodoverview.Run()) {
                case -1:
                    CateringMenu();
                    break;
            }
        }

        private void ConfirmCodeMenu()
        {
            ConfirmCode confirmCode = new ConfirmCode();
            int index = confirmCode.Run();
            switch(index)
            {
                case 0:
                    RunStartingMenu();
                    break;
            }
        }

        private void ReviewsMenu()
        {
            ReviewMenu ReviewMenu = new ReviewMenu();
            int index = ReviewMenu.Run();
            switch (index)
            {
                case 0:
                    if (Program.information.Member == null)
                    {
                        RunStartingMenu();
                        break;
                    }
                    else
                    {
                        RunLoggedInMenu();
                        break;
                    }
                case 1:
                    RunCreateReviewMenu();
                    break;
                case 2:
                    RunReviewOverviewMenu();
                    break;
            }
        }

        private void SeatsChoosingMenu()
        {
	    SeatsMenu seatsmenu = new SeatsMenu();
	    int index = seatsmenu.Run();
	    switch (index)
	    {
		case 0:
		    TimeSelectionMenu();
		    break;
		case 1:
		    FoodChosingMenu();
		    break;
	    }
        }

        private void RunOverviewMenu()
        { 
            OverviewMenu overviewmenu = new OverviewMenu();
            int index = overviewmenu.Run();
	        switch (index) {
                    case -1:
                        FoodChosingMenu();
                        break;
                    case 1:
                        RunConfirmreservationMenu();
                        break;
            }
        }

	
        private void RunLoggedInMenu()
        {
            LoggedInMenu loggedIn = new LoggedInMenu(); 
            int index = loggedIn.Run();
            switch (index) {
		        case -1:
                    var info = Program.information;
                    info.Member = null;
                    Program.information = info;
                    RunStartingMenu();
                    break;
                case 0:
                    MoviesMenu();
                    break;
                case 1:
                    RunReservationOverviewMenu();  
                    break;
                case 2:
                    ReviewsMenu();
                    break;
                case 3:
                    RunStartingMenu();
                    break;
            }

        }

        private void RunReservationOverviewMenu()
        {
            ReservationOverviewMenu reservationOverviewMenu = new ReservationOverviewMenu();
            int index = reservationOverviewMenu.Run();
            switch (index) {
                case -1:
                    RunLoggedInMenu();
                    break;
                case 0:
                    RunConfirmDecisionMenu();
                    break;

            }

        }

        private void RunConfirmDecisionMenu()
        {
            ConfirmDecisionMenu confirmDecisionMenu = new ConfirmDecisionMenu(); 
            int index = confirmDecisionMenu.Run();
            switch (index)
            {
                case -1:
                    RunReservationOverviewMenu();
                    break;
                case 0:
                    RunLoggedInMenu();
                    break;

            }

        }
        private void RunAdminMenu()
        {
            AdminMenu adminMenu = new AdminMenu();
            int index = adminMenu.Run();
            switch (index) {
                case 0:
                    RunStartingMenu();
                    break;
                case 1:
                    RunAdminMovieMenu();
                    break;
                case 2:
                    RunReviewOverviewMenu();
                    break;
                case 3:
                    RunEarningsMenu();
                    break;
                case 4:
                    RunSettingsMenu();
                    break;
            }
        }

        private void RunSettingsMenu()
        {
            SettingsMenu settingsMenu = new SettingsMenu();
            switch (settingsMenu.Run())
            {
                case -1:
                    RunAdminMenu();
                    break;
            }
        }

        private void RunAdminMovieMenu()
        {
            AdminMovie adminmoviemenu = new AdminMovie();
            int index = adminmoviemenu.Run();
            switch (index)
            {
                case 0:
                    RunAdminMenu();
                    break;
                case 1:
                    RunChooseFilmToEditMenu();
                    break;
                case 2:
                    RunAddMovieMenu();
                    break;
            }
        }

        private void RunChooseFilmToEditMenu()
        {
            ChooseFilmToEditMenu ChooseFilmToEdit = new ChooseFilmToEditMenu();
            int index = ChooseFilmToEdit.Run();
            switch (index) 
            {
                case 0:
                    RunAdminMovieMenu();
                    break;
                case 1:
                    RunEditMovieMenu();
                    break;
               
            }       


        }

        private void RunEditMovieMenu()
        {
            EditMovie editMovieMenu = new EditMovie();
            int index = editMovieMenu.Run();
            switch (index)
            {
                case 0:
                    RunChooseFilmToEditMenu();
                    break;
                case 1:
                    RunChangeTimeMenu();
                    break;
                case 2:
                    RunAdminMenu();
                    break;

            }
        }

        private void RunChangeTimeMenu()
        {
            ChangeTime changeTimeMenu = new ChangeTime();  
            int index = changeTimeMenu.Run();
            switch (index)
            {
                case 0:
                    RunEditMovieMenu();
                    break;
                case 1:
                    RunAdminMovieMenu();
                    break;
                case 2:
                    RunAddMovieMenu();
                    break;
                
            }

        }
        private void RunAddMovieMenu()
        {
            AddMovieMenu AddMovieMenu = new AddMovieMenu();
            int index = AddMovieMenu.Run();
            switch (index)
            {
                case 0:
                    RunAdminMenu();
                    break;
                case 1:
                    RunChangeTimeMenu();
                    break;
                case 2:
                    RunAdminMovieMenu();
                    break;
            }
        }
        private void RunChangePasswordMenu()
        {
            ChangePassword changePassword = new ChangePassword();
            int index = changePassword.Run();
            switch (index)
            {
                case 0:
                    LoginMenu();
                    break;
            }
        }

        private void RunEarningsMenu()
	{
            EarningsMenu earningsMenu = new EarningsMenu();
	    switch (earningsMenu.Run())
	    {
		case -1:
                    RunAdminMenu();
                    break;
            }
        }

        private void RunCreateReviewMenu()
        {
            CreateReview createReview = new CreateReview();
            int index = createReview.Run();
            switch (index)
            {
                case 0:
                    ReviewsMenu();
                    break;
            }
        }

        private void RunReviewOverviewMenu()
        {
            ReviewOverview reviewlist = new ReviewOverview();
            int index = reviewlist.Run();
            switch (index)
            {
                case -1:
                    RunReviewOverviewMenu();
                    break;
                case 0:
                    ReviewsMenu();
                    break;
                case 1:
                    RunAdminMenu();
                    break;
            }
        }

        private void RunConfirmreservationMenu()
        {
            ConfirmReservation confirmreservation = new ConfirmReservation();
            int index = confirmreservation.Run();
            switch (index)
            {
                case 0:
                    RunLoggedInMenu();
                    break;
            }
        }
    }
}
