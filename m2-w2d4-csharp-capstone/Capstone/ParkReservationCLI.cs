using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.Models;
using System.Configuration;

namespace Capstone
{
    class ParkReservationCLI
    {

        const string Command_ViewAllParks = "1";
        const string Command_ChooseACampsite = "2";
        const string Command_ViewReservations = "3";
        const string Command_Quit = "q";
        const string Command_StartOver = "s";
        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;
        public Reservation newReservation = new Reservation();
        public Campsite newCampsite = new Campsite();

        public void RunCLI()
        {
            PrintHeader();
            PrintMainMenu();

            while (true)
            {
                string command = Console.ReadLine();

                Console.Clear();

                switch (command.ToLower())
                {
                    case Command_ViewAllParks:
                        GetAllParksLongDisplay();
                        break;

                    case Command_ChooseACampsite:
                        DisplayCampsiteMenu();
                        break;

                    case Command_ViewReservations:
                        GetReservations();
                        break;

                    case Command_Quit:
                        Console.WriteLine("Thank you for using the Park Reservation cli app");
                        return;

                    default:
                        Console.WriteLine("The command provided was not a valid command, please try again.");
                        break;
                }

                PrintMainMenu();
            }
        }

        private void PrintHeader()
        {
            Console.WriteLine(@" _   _       _   _                   _  ______          _");
            Console.WriteLine(@"| \ | |     | | (_)                 | | | ___ \        | |");
            Console.WriteLine(@"|  \| | __ _| |_ _  ___  _ __   __ _| | | |_/ /_ _ _ __| |"); 
            Console.WriteLine(@"| . ` |/ _` | __| |/ _ \| '_ \ / _` | | |  __/ _` | '__| |/ /");
            Console.WriteLine(@"| |\  | (_| | |_| | (_) | | | | (_| | | | | | (_| | |  |   <");
            Console.WriteLine(@"| | \_/\__,_|\__|_|\___/|_| |_|\__,_|_| \_|  \__,_|_|  |_|\_\");                                                           
            Console.WriteLine(@"______                          _   _");
            Console.WriteLine(@"| ___ \                        | | (_)");
            Console.WriteLine(@"| |_/ /___  ___  ___ _ ____   _| |_ _  ___  _ __  ___");
            Console.WriteLine(@"|    // _ \/ __|/ _ \ '__\ \ / / __| |/ _ \| '_ \/ __|");
            Console.WriteLine(@"| |\ \  __/\__ \  __/ |   \ V /| |_| | (_) | | | \__ \");
            Console.WriteLine(@"\_| \_\___||___/\___|_|    \_/  \__|_|\___/|_| |_|___/");     
        }

        private void PrintMainMenu()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Main-Menu Type in a command");
            Console.WriteLine(" 1 - View All Parks");
            Console.WriteLine(" 2 - Choose A Campsite");
            Console.WriteLine(" 3 - View Exiting Reservations");
            Console.WriteLine(" Q - Quit");
        }

        private void DisplayCampsiteMenu()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Choose a Campsite - Type in a command");
            Console.WriteLine(" 1 - Choose A Campsite By Park Id");
            Console.WriteLine(" 2 - Choose A Campsite By Available Dates");
            Console.WriteLine(" S - Start Over");
            Console.WriteLine(" Q - Quit");

            string command = Console.ReadLine();

            Console.Clear();

            switch (command.ToLower())
            {
                case "1":
                    DisplayParkforParkID();
                    break;

                case "2":
                    GetReservationDates();
                    break;

                case "s":
                    //start over;
                    break;

                case "q":
                    Console.WriteLine("Thank you for using the National Park Reservation cli app");
                    return;

                default:
                    Console.WriteLine("The command provided was not a valid command, please try again.");
                    break;
            }
        }

        private void GetReservations()
        {
            ReservationSQLDAL dal = new ReservationSQLDAL(connectionString);

            List<Reservation> reservations = dal.GetAllReservations();

            Console.WriteLine();

            foreach (var reservation in reservations)
            {
                Console.WriteLine();
                Console.WriteLine(reservation);
            }
        }

        private void GetAllParksLongDisplay()
        {
            ParkSQLDAL dal = new ParkSQLDAL(connectionString);
            List<Park> parks = dal.GetAllParksWithFullDescription();

            foreach (var park in parks)
            {
                Console.WriteLine(park);
            }
        }

        private void GetAllParksShortDisplay()
        {
            ParkSQLDAL dal = new ParkSQLDAL(connectionString);
            List<Park> parks = dal.GetAllParksShortened();

            Console.WriteLine();
            Console.WriteLine();

            foreach (var park in parks)
            {
                Console.WriteLine(park.ParkID + "- " + park.Name);
            }
        }

        private void DisplayParkforParkID()
        {
            GetAllParksShortDisplay();

            Console.WriteLine();
            Console.WriteLine("Please Enter the Park Id...");
            newCampsite.ParkId = int.Parse(Console.ReadLine());

            Console.WriteLine("Do you wish to search the whole (p)ark or a specific (c)ampground? ");
            string userChoice = Console.ReadLine();

            CampsiteSQLDAL dal = new CampsiteSQLDAL(connectionString, false);
            List<Campsite> campsites = new List<Campsite>();

            if (userChoice == "p")
            {
                OfferDateRangeByParkID();
            }
            else if (userChoice == "c")
            {
                DisplayCampgrounds(false);

                Console.WriteLine("Please enter a choice for campground...");

                newCampsite.CampgroundId = int.Parse(Console.ReadLine());

                Console.WriteLine("Do you want to choose a date range? (Y/N)");
                string dateChoice = Console.ReadLine().ToUpper();

                if (dateChoice == "N")
                {
                    campsites = dal.GetCampsitesByParkIdAndCampgroundId(newCampsite.ParkId, newCampsite.CampgroundId);
                }
                else if (dateChoice == "Y")
                {
                    AskForTravelDates();

                    campsites = dal.GetCampsitesByParkCampgroundAndAvailableDates(newCampsite.ParkId, newCampsite.CampgroundId, newReservation.FromDate, newReservation.ToDate);
                }

                DisplayCampsiteAndReservations(campsites);
            }
            else
            {
                Console.WriteLine("Invalid Input. Please Start Again...");
            }

        }

        public void DisplayCampsiteAndReservations(List<Campsite> campsites)
        {
            foreach (var campsite in campsites)
            {
                Console.WriteLine(campsite);
            }

            Console.WriteLine();
            Console.WriteLine("Do you want to make a reservation?");

            if (Console.ReadLine().ToUpper() == "Y")
            {
                ReservationSQLDAL resDal = new ReservationSQLDAL(connectionString);

                Console.WriteLine("Please Enter the Campsite Id...");
                newReservation.SiteId = int.Parse(Console.ReadLine());

                Console.WriteLine("What is the last name for the reservation?");
                newReservation.Name = Console.ReadLine();

                if (newReservation.ToDate == DateTime.MinValue)
                {
                    AskForTravelDates();
                }

                resDal.AddNewReservation(newReservation);

                if (newReservation.ReservationId > 0)
                {
                    Console.WriteLine(newReservation);
                }
                else
                {
                    Console.WriteLine("Invalid Date Range.");
                }
            }
        }

        public void DisplayCampgrounds(bool datesAreGiven)
        {
            CampgroundSQLDAL dal = new CampgroundSQLDAL(connectionString);
            List<Campground> campgrounds = new List<Campground>();

            if (datesAreGiven)
            {
                campgrounds = dal.GetAllCampgroundsParkIDDate(newCampsite.ParkId,newReservation.FromDate, newReservation.ToDate);
            }
            else
            {
                campgrounds = dal.GetAllCampgroundsParkID(newCampsite.ParkId);
            }

            foreach (var campground in campgrounds)
            {
                Console.WriteLine(campground.CampID + "- " + campground.Name);
            }
        }

        public void GetReservationDates()
        {
            CampsiteSQLDAL dal = new CampsiteSQLDAL(connectionString, false);
            List<Campsite> campsites = new List<Campsite>();

            AskForTravelDates();

            Console.WriteLine("Do you wish to display campsites by (a)ll parks or a (s)pecific park?");
            string userChoice = Console.ReadLine().ToUpper();

            if (userChoice == "A")
            {
                campsites = dal.GetCampsitesByAvailableDates(newReservation.FromDate, newReservation.ToDate);
                DisplayCampsiteAndReservations(campsites);
            }
            else if (userChoice == "S")
            {
                DisplayParkforParkIDAndDate();
            }
            else
            {
                Console.WriteLine("Invalid Input. Please Start Again...");
            }

        }
        public void DisplayParkforParkIDAndDate()
        {
            GetAllParksShortDisplay();

            Console.WriteLine();
            Console.WriteLine("Please Enter the Park Id...");
            newCampsite.ParkId = int.Parse(Console.ReadLine());

            Console.WriteLine("Do you wish to search the whole (p)ark or a specific (c)ampground? ");
            string userChoice = Console.ReadLine();

            CampsiteSQLDAL dal = new CampsiteSQLDAL(connectionString, false);
            List<Campsite> campsites = new List<Campsite>();

            if (userChoice == "p")
            {
                campsites = dal.GetCampsitesByParkIdAndAvailableDates(newCampsite.ParkId, newReservation.FromDate, newReservation.ToDate);
                DisplayCampsiteAndReservations(campsites);
            }
            else if (userChoice == "c")
            {
                DisplayCampgrounds(true);

                Console.WriteLine("Please enter a choice for campground...");

                newCampsite.CampgroundId = int.Parse(Console.ReadLine());
                campsites = dal.GetCampsitesByParkIdAndCampgroundId(newCampsite.ParkId, newCampsite.CampgroundId);

                DisplayCampsiteAndReservations(campsites);
            }
            else
            {
                Console.WriteLine("Invalid Input. Please Start Again...");
            }

        }

        public void OfferDateRangeByParkID()
        {
            CampsiteSQLDAL dal = new CampsiteSQLDAL(connectionString, false);
            List<Campsite> campsites = new List<Campsite>();

            Console.WriteLine("Do you want to choose a date range? (Y/N)");
            string dateChoice = Console.ReadLine().ToUpper();

            if (dateChoice == "N")
            {
                campsites = dal.GetCampsitesByParkId(newCampsite.ParkId);
                DisplayCampsiteAndReservations(campsites);
            }
            else if (dateChoice == "Y")
            {
                AskForTravelDates();

                campsites = dal.GetCampsitesByParkIdAndAvailableDates(newCampsite.ParkId, newReservation.FromDate, newReservation.ToDate);
                DisplayCampsiteAndReservations(campsites);
            }
            else
            {
                Console.WriteLine("Invalid Input...");
            }
        }

        public void AskForTravelDates()
        {
                Console.WriteLine("What is the start date for your travel? (mm/dd/yy)");
                newReservation.FromDate = Convert.ToDateTime(Console.ReadLine());

                Console.WriteLine("What is the end date for your travel? (mm/dd/yy)");
                newReservation.ToDate = Convert.ToDateTime(Console.ReadLine());
        }
    }

}