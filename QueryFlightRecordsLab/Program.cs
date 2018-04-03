using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/* A nifty little console application that can perform various queries on an external file containing a large collection of flight information */

namespace QueryFlightRecordsLab
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Program Introduction
                Console.WriteLine("*****Welcome to Flight Record Manager*****\n**Type exit at any time to end the application**");
                string exit = "";

                while (exit != "exit")
                {
                    Console.WriteLine("\n(1) Display X number of flight records");
                    Console.WriteLine("\n(2) Display arrival delays from X (origin) to Y (destination)");
                    Console.WriteLine("\n(3) Display all airports with direct flights to X (destination)");
                    Console.WriteLine("\n(4) Display origin and destination of X number of flights\nwith largest arrival delay");
                    Console.WriteLine("\n(5) Display destination city for the first X number flights");
                    Console.WriteLine("\n(6) Display average arrival delay of all flights from X (origin) to Y (destination)");
                    Console.WriteLine("\n(7) Display X number of the shortest/longest flights");
                    Console.WriteLine("\n(8) Display the longest/shortest flights out of X (origin) city");

                    Console.WriteLine("\nSelect the function you would like to perform\nby entering the appropriate number: ");

                    //Read user input
                    int selection = 0;
                    string input = Console.ReadLine();
                    if (input.ToUpper().Trim().Replace(" ", "") == "EXIT")
                        exitApp();
                    else
                        int.TryParse(input, out selection);

                    //Determine function selection
                    switch (selection)
                    {
                        case 1:
                            //Displays X number of flights
                            Console.WriteLine("\nEnter the number of flights you would like to display: ");
                            string input2 = Console.ReadLine();
                            int count = 0;
                            if (input2.ToUpper().Trim().Replace(" ", "") == "EXIT")
                                exitApp();
                            else
                                int.TryParse(input2, out count);

                            display_Flights(count);
                            break;
                        case 2:
                            //Displays arrival delays from X to Y
                            Console.WriteLine("Enter your origin city: ");
                            string origin = Console.ReadLine();
                            if (origin.ToUpper().Trim().Replace(" ", "") == "EXIT")
                                Environment.Exit(1);
                            Console.WriteLine("\nEnter your destination city: ");
                            string destination = Console.ReadLine();
                            if (destination.ToUpper().Trim().Replace(" ", "") == "EXIT")
                                exitApp();
                            arrivalDelay(origin, destination);
                            break;
                        case 3:
                            //Display all direct flights to X
                            Console.WriteLine("Enter your destination city: ");
                            string destination2 = Console.ReadLine();
                            if (destination2.ToUpper().Trim().Replace(" ", "") == "EXIT")
                                exitApp();
                            directFlight(destination2);
                            break;
                        case 4:
                            //Sort by largest arrival delay and display X #
                            Console.WriteLine("Enter how many flights you would like to display: ");
                            string countInput = Console.ReadLine();
                            int myCount = 0;
                            if (countInput.ToUpper().Trim().Replace(" ", "") == "EXIT")
                                exitApp();
                            else
                                int.TryParse(countInput, out myCount);
                            largestArrivalDelay(myCount);
                            break;
                        case 5:
                            //Display first X # flight destinations
                            Console.WriteLine("Enter how many destinations you would like to display: ");
                            string destInput = Console.ReadLine();
                            int destOutput = 0;
                            if (destInput.ToUpper().Trim().Replace(" ", "") == "EXIT")
                                exitApp();
                            else
                                int.TryParse(destInput, out destOutput);
                            firstNumbFlights(destOutput);
                            break;
                        case 6:
                            Console.WriteLine("Enter your origin city: ");
                            string originCity = Console.ReadLine();
                            if (originCity.ToUpper().Trim().Replace(" ", "") == "EXIT")
                                exitApp();
                            Console.WriteLine("Enter your destination city: ");
                            string destinationCity = Console.ReadLine();
                            if (originCity.ToUpper().Trim().Replace(" ", "") == "EXIT")
                                exitApp();
                            averageDelay(originCity, destinationCity);
                            break;
                        case 7:
                            Console.WriteLine("Would you like to display shortest or longest flights: ");
                            string userInput = Console.ReadLine();
                            if (userInput.ToUpper().Trim().Replace(" ", "") == "EXIT")
                                exitApp();
                            Console.WriteLine("How many flights would you like to display: ");
                            string userInputCount = Console.ReadLine();
                            int userCount;
                            int.TryParse(userInputCount, out userCount);
                            shortest_LongestFlight(userInput, userCount);
                            break;
                        case 8:
                            Console.WriteLine("Would you like to display shortest or longest flights: ");
                            string userInput2 = Console.ReadLine();
                            if (userInput2.ToUpper().Trim().Replace(" ", "") == "EXIT")
                                exitApp();
                            Console.WriteLine("Enter your origin city: ");
                            string originCity2 = Console.ReadLine();
                            if (originCity2.ToUpper().Trim().Replace(" ", "") == "EXIT")
                                exitApp();
                            long_ShortFrom(userInput2, originCity2);
                            break;
                        default:
                            Console.WriteLine("Sorry, either TryParse has failed, or your input was invalid\nPress any key to exit: ");
                            Console.ReadKey();
                            exitApp();
                            break;
                    }
                }
                Console.WriteLine("Press any key to exit: ");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Attempting to run Program.Main() has produced the following error: \n{0}", e.Message);
                Console.WriteLine("\nPress any key to exit application: ");
                Console.ReadKey();
                exitApp();
            }
        }
        public static void exitApp()
        {
            Console.WriteLine("Exiting application now...");
            Environment.Exit(1);
        }

        private static void display_Flights(int count)
        {
            try
            {
                //Displays all info from X # of flights
                var flights = FlightInfo.ReadFlightsFromFile("airline.csv");
                for (int i = 0; i < count; i++)
                {
                    string myFlight = flights.ElementAt(i).ToString();
                    Console.WriteLine(myFlight);
                }
                Console.WriteLine("Press any key to continue: ");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Attempting to run Program.display_Flights() has produced the following error: \n{0}", e.Message);
                Console.WriteLine("\nPress any key to exit application: ");
                Console.ReadKey();
                exitApp();
            }
        }

        private static void arrivalDelay(string origin, string destination)
        {
            try
            {
                //Displays all arrival delay times from X_origin to Y_destination
                if (origin.ToUpper().Trim().Replace(" ", "") == "EXIT" || destination.ToUpper().Trim().Replace(" ", "") == "EXIT")
                    exitApp();
                var flights = FlightInfo.ReadFlightsFromFile("airline.csv");
                int i = 1;
                Console.WriteLine("The arrival delays from {0} to {1} are as follows: ", origin, destination);
                var query = from item in flights
                            where item.Origin.ToUpper().Replace(" ", "").Trim().Equals(origin.ToUpper().Replace(" ", "").Trim())
                            && item.Destination.ToUpper().Replace(" ", "").Trim().Equals(destination.ToUpper().Replace(" ", "").Trim())
                            select item;
                foreach (var item in query)
                    Console.WriteLine("({0}){1} minutes", i++, item.ArrivalDelay);
                if (query.Count() == 0)
                    Console.WriteLine("Sorry, the query you entered was not recognize.\nBe sure to enter both the city, and state abr. Ex: Los Angeles CA");
                Console.WriteLine("Press any key to continue: ");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Attempting to run Program.arrivalDelay() has produced the following error: \n{0}", e.Message);
                Console.WriteLine("\nPress any key to exit application: ");
                Console.ReadKey();
                exitApp();
            }
        }

        private static void directFlight(string destination)
        {
            try
            {
                //Displays # of flights from each origin to X_destination
                if (destination.ToUpper().Trim().Replace(" ", "") == "EXIT")
                    exitApp();
                var flights = FlightInfo.ReadFlightsFromFile("airline.csv");
                List<FlightInfo> queryList = new List<FlightInfo>();
                Console.WriteLine("The following airports have direct flights to {0}", destination);
                var query = from item in flights
                            where item.Destination.ToUpper().Replace(" ", "").Trim().Equals(destination.ToUpper().Replace(" ", "").Trim())
                            select item;
                int myCount = 0;
                foreach (var item in query)
                    queryList.Add(item);
                while (queryList.Count != 0)
                {
                    myCount = queryList.Count();
                    var myQuery = from item in queryList
                                  where item.Origin.Equals(queryList.ElementAt((myCount - 1)).Origin)
                                  select item;

                    Console.WriteLine("{0} flights from {1}", myQuery.Count(), myQuery.ElementAt(0).Origin);
                    queryList.RemoveAll(x => myQuery.ElementAt(0).Origin.Contains(x.Origin));
                }
                Console.WriteLine("Press any key to continue: ");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Attempting to run Program.directFlight() has produced the following error: \n{0}", e.Message);
                Console.WriteLine("\nPress any key to exit application: ");
                Console.ReadKey();
                exitApp();
            }
        }

        private static void shortest_LongestFlight(string input, int count)
        {
            try
            {
                if (input.ToUpper().Trim().Replace(" ", "") == "EXIT")
                    exitApp();
                var flights = FlightInfo.ReadFlightsFromFile("airline.csv");
                int i = 1;
                List<FlightInfo> newFlights = new List<FlightInfo>();
                Console.WriteLine("This may take a moment to process...\n");
                if (input.ToUpper().Trim().Replace(" ", "") == "SHORTEST" || input.ToUpper().Trim().Replace(" ", "") == "SHORT")
                {
                    Console.WriteLine("The shortest flights are as follows: ");
                    while (flights.Count() != 0)
                    {
                        newFlights.Add(flights.ElementAt(flights.Count() - 1));
                        flights.RemoveAll(o => o.Distance.Equals(newFlights.ElementAt(newFlights.Count() - 1).Distance));
                    }
                    var shortFlight = newFlights.OrderBy(o => o.Distance).Take(count);
                    foreach (var item in shortFlight)
                    {
                        Console.WriteLine("({0}) [{1}] to [{2}] at a distance of ({3}) miles", i++, item.Origin, item.Destination, item.Distance);
                    }
                }
                else if (input.ToUpper().Trim().Replace(" ", "") == "LONGEST" || input.ToUpper().Trim().Replace(" ", "") == "LONG")
                {
                    Console.WriteLine("The longest flights are as follows: ");
                    while (flights.Count() != 0)
                    {
                        newFlights.Add(flights.ElementAt(flights.Count() - 1));
                        flights.RemoveAll(o => o.Distance.Equals(newFlights.ElementAt(newFlights.Count() - 1).Distance));
                    }
                    var longFlight = newFlights.OrderByDescending(o => o.Distance).Take(count);
                    foreach (var item in longFlight)
                    {
                        Console.WriteLine("({0}) [{1}] to [{2}] at a distance of ({3}) miles", i++, item.Origin, item.Destination, item.Distance);
                    }
                }
                else
                    Console.WriteLine("Sorry, the variable you entered was not recognized.");
                Console.WriteLine("\nPress any key to continue: ");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Attempting to run Program.shortest_LongestFlight() has produced the following error: \n{0}", e.Message);
                Console.WriteLine("\nPress any key to exit application: ");
                Console.ReadKey();
                exitApp();
            }
        }

        private static void largestArrivalDelay(int count)
        {
            try
            {
                //Sort flights by largest arrival delay and display X #
                var flights = FlightInfo.ReadFlightsFromFile("airline.csv");
                int i = 1;
                var newList = flights.OrderBy(o => o.ArrivalDelay);
                var myList = newList.Take(count);
                foreach (var item in myList)
                    Console.WriteLine("({3})[{0}] to [{1}], arrival delay: ({2}) minutes", item.Origin, item.Destination, item.ArrivalDelay, i++);

                Console.WriteLine("Press any key to continue: ");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Attempting to run Program.largestArrivalDelay() has produced the following error: \n{0}", e.Message);
                Console.WriteLine("\nPress any key to exit application: ");
                Console.ReadKey();
                exitApp();
            }
        }

        private static void firstNumbFlights(int count)
        {
            try
            {
                var flights = FlightInfo.ReadFlightsFromFile("airline.csv");
                var myList = flights.Take(count);
                int i = 0;
                foreach (var item in myList)
                    Console.WriteLine("({0}) {1}", i++, item.Destination);

                Console.WriteLine("Press any key to continue: ");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Attempting to run Program.firstNumbFlight() has produced the following error: \n{0}", e.Message);
                Console.WriteLine("\nPress any key to exit application: ");
                Console.ReadKey();
                exitApp();
            }
        }

        private static void averageDelay(string origin, string destination)
        {
            try
            {
                if (origin.ToUpper().Trim().Replace(" ", "") == "EXIT" || destination.ToUpper().Trim().Replace(" ", "") == "EXIT")
                    exitApp();
                var flights = FlightInfo.ReadFlightsFromFile("airline.csv");

                var query = from itemX in flights
                            where itemX.Origin.ToUpper().Trim().Replace(" ", "") == origin.ToUpper().Trim().Replace(" ", "")
                            && itemX.Destination.ToUpper().Trim().Replace(" ", "") == destination.ToUpper().Trim().Replace(" ", "")
                            select itemX;
                double avg = query.Average(o => o.ArrivalDelay);
                Console.WriteLine("The average arrival delay of all flights from {0} to {1} is({2}) minutes", origin.ToUpper(), destination.ToUpper(), avg);
                if (query.Count() == 0)
                    Console.WriteLine("Sorry, the query you entered was not recognize.\nBe sure to enter both the city, and state abr. Ex: Los Angeles CA");
                Console.WriteLine("Press any key to continue: ");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Attempting to run Program.averageDelay() has produced the following error: \n{0}", e.Message);
                Console.WriteLine("\nPress any key to exit application: ");
                Console.ReadKey();
                exitApp();
            }
        }

        private static void long_ShortFrom(string input, string origin)
        {
            try
            {
                //if (origin.ToUpper().Trim().Replace(" ", "") == "EXIT" || input.ToUpper().Trim().Replace(" ", "") == "EXIT")
                    //exitApp();
                int i = 1;
                List<FlightInfo> shortFlight = new List<FlightInfo>();
                List<FlightInfo> longFlight = new List<FlightInfo >();
                List<FlightInfo> intermediate = new List<FlightInfo>();
                var flights = FlightInfo.ReadFlightsFromFile("airline.csv");
                if (input.ToUpper().Trim().Replace(" ", "") == "SHORT" || input.ToUpper().Trim().Replace(" ", "") == "SHORTEST")
                {
                    Console.WriteLine("The shortest flights out of {0} were as follows: ", origin.ToUpper());
                    var query = from item in flights
                                where item.Origin.ToUpper().Replace(" ", "").Trim().Equals(origin.ToUpper().Replace(" ", "").Trim())
                                select item;
                    intermediate = query.ToList();
                    while (intermediate.Count() != 0)
                    {
                        shortFlight.Add(intermediate.ElementAt(intermediate.Count() - 1));
                        intermediate.RemoveAll(o => o.Distance.Equals(shortFlight.ElementAt(shortFlight.Count() - 1).Distance));
                    }
                    foreach (var item in shortFlight.OrderBy(o => o.Distance).Take(shortFlight.Count() / 4))
                        Console.WriteLine("({0}) [{1}] to [{2}] at a distance of ({3}) miles", i++, item.Origin, item.Destination, item.Distance);
                }
                else if (input.ToUpper().Trim().Replace(" ", "") == "LONG" || input.ToUpper().Trim().Replace(" ", "") == "LONGEST")
                {
                    Console.WriteLine("The longest flights out of {0} were as follows: ", origin.ToUpper());
                    var myList = from item in flights
                                 where item.Origin.ToUpper().Trim().Replace(" ", "") == origin.ToUpper().Trim().Replace(" ", "")
                                 select item;
                    //var longFlight = myList.OrderByDescending(o => o.Origin).Take(10);
                    foreach (var item in longFlight)
                        Console.WriteLine("({0}) [{1}] to [{2}] at a distance of ({3}) miles", i++, item.Origin, item.Destination, item.Distance);
                }
                else
                    Console.WriteLine("Sorry, the variable you entered was not recognized.");

                if (shortFlight.Count() == 0 && longFlight.Count() == 0)
                    Console.WriteLine("Sorry, the query you entered was not recognize.\nBe sure to enter both the city, and state abr. Ex: Los Angeles CA");
                Console.WriteLine("\nPress any key to continue: ");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Attempting to run Program.long_ShortFrom() has produced the following error: \n{0}", e.Message);
                Console.WriteLine("\nPress any key to exit application: ");
                Console.ReadKey();
                exitApp();
            }
        }
    }
    class FlightInfo
    {
        public string Airline { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int DepartureDelay { get; set; }
        public int ArrivalDelay { get; set; }
        public bool Cancelled { get; set; }
        public int Distance { get; set; }


        public FlightInfo(string[] fields)
        {
            this.Airline = fields[0];
            this.Origin = fields[1];
            this.Destination = fields[2];
            this.DepartureDelay = int.Parse(fields[3]);
            this.ArrivalDelay = int.Parse(fields[4]);
            this.Cancelled = int.Parse(fields[5]) == 1;
            this.Distance = int.Parse(fields[6]);
        }

        public override string ToString()
        {
            return String.Format("{0} flight from {1} to {2} ({3} miles) departure delay: {4} minutes, arrival delay: {5} minutes"
                , this.Airline, this.Origin, this.Destination, this.Distance, this.DepartureDelay, this.ArrivalDelay);
        }

        public static List<FlightInfo> ReadFlightsFromFile(string fileName)
        {
            try {
                List<FlightInfo> flights = new List<FlightInfo>();
                bool first = true;
                using (var reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (first)
                        {
                            first = false;
                            continue;
                        }

                        string[] parts = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length != 7)
                            continue;

                        flights.Add(new FlightInfo(parts));
                    }
                }
                return flights;
            }
            catch (Exception e)
            {
                Console.WriteLine("Attempting to run FlightInfo.ReadFlightsFromFile() has produced the following error: \n{0}", e.Message);
                Console.WriteLine("\nPress any key to exit application: ");
                Console.ReadKey();
                Program.exitApp();
                return null;
            }
        }
    }
}
