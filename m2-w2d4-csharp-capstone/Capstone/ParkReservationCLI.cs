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
            //    Console.WriteLine(@" _    _  _____ ______  _     ______     ______   ___   _____   ___  ______   ___   _____  _____ ");
            //    Console.WriteLine(@"| |  | ||  _  || ___ \| |    |  _  \    |  _  \ / _ \ |_   _| / _ \ | ___ \ / _ \ /  ___||  ___|");
            //    Console.WriteLine(@"| |  | || | | || |_/ /| |    | | | |    | | | |/ /_\ \  | |  / /_\ \| |_/ // /_\ \\ `--. | |__  ");
            //    Console.WriteLine(@"| |/\| || | | ||    / | |    | | | |    | | | ||  _  |  | |  |  _  || ___ \|  _  | `--. \|  __| ");
            //    Console.WriteLine(@"\  /\  /\ \_/ /| |\ \ | |____| |/ /     | |/ / | | | |  | |  | | | || |_/ /| | | |/\__/ /| |___ ");
            //    Console.WriteLine(@" \/  \/  \___/ \_| \_|\_____/|___/      |___/  \_| |_/  \_/  \_| |_/\____/ \_| |_/\____/ \____/ ");
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
                    GetCampsiteByParkID();
                    break;

                case "2":
                    GetDates();
                    break;

                case "s":
                    //start over;
                    break;

                case "q":
                    Console.WriteLine("Thank you for using the Park Reservation cli app");
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

        private void GetCampsiteByParkID()
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
                campsites = dal.GetCampsitesByParkId(newCampsite.ParkId);
                DisplayCampsiteAndReservations(campsites);
            }
            else if (userChoice == "c")
            {
                DisplayCampgrounds();

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
                    Console.WriteLine("What is the start date for your travel? (dd/mm/yy)");
                    newReservation.FromDate = Convert.ToDateTime(Console.ReadLine());

                    Console.WriteLine("What is the end date for your travel? (dd/mm/yy)");
                    newReservation.ToDate = Convert.ToDateTime(Console.ReadLine());
                }

                resDal.AddNewReservation(newReservation);

                Console.WriteLine(newReservation);
            }
        }

        public void DisplayCampgrounds()
        {
            CampgroundSQLDAL dal = new CampgroundSQLDAL(connectionString);
            List<Campground> campgrounds = dal.GetAllCampgroundsParkID(newCampsite.ParkId);

            foreach (var campground in campgrounds)
            {
                Console.WriteLine(campground);
            }
        }

        public void GetDates()
        {
            Console.WriteLine("What is the start date for your travel? (dd/mm/yy)");
            newReservation.FromDate = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine("What is the end date for your travel? (dd/mm/yy)");
            newReservation.ToDate = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine("Do you wish to display campsites by (a)ll parks or a (s)pecific park?");
            string userChoice = Console.ReadLine().ToUpper();

            if (userChoice == "A")
            {

            }
            else if (userChoice == "S")
            {

            }
            else
            {
                Console.WriteLine("Invalid Input. Please Start Again...");
            }



        }
    }

}