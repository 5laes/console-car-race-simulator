using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace labb3carrace
{
    internal class Program
    {
        static List<Car> carList = new List<Car>();
        static bool showStatus = false;
        
        // Main method
        static void Main(string[] args)
        {
            AddCar();
            CarRaceStart();
            Console.ReadLine();
        }

        // Start all the threads
        public static void CarRaceStart()
        {
            StartCars();
            Thread statusSwitch = new Thread(SwitchStatus);
            statusSwitch.Start();
            GetCarDistance();
        }

        // Adds a car with input from user
        public static void AddCar()
        {
            char choice;
            do
            {
                Console.Clear();
                Console.Write("\n\tCar name: ");
                string carName = Console.ReadLine();

                Console.Write("\n\tCar top speed: ");
                decimal.TryParse(Console.ReadLine(), out decimal topSpeed);

                Console.Write("\n\tTime to top speed: ");
                decimal.TryParse(Console.ReadLine(), out decimal timeToTopSpeed);

                Car newCar = new Car(carName, topSpeed, timeToTopSpeed);
                carList.Add(newCar);

                choice = AddAnotherCar();
                
            } while (choice != 'N');
        }

        // Asks the user if they want to add another car
        public static char AddAnotherCar()
        {
            Console.Write("\n\tDo you want to add another car?" +
                    "\n\t[1]Yes" +
                    "\n\t[2]No" +
                    "\n\t:");
            int.TryParse(Console.ReadLine(), out int choice);
            switch (choice)
            {
                case 1:
                    return 'Y';
                case 2:
                    return 'N';
                default:
                    Console.Write("\n\tERROR");
                    AddAnotherCar();
                    return 'K';
            }
        }

        // Goes through every car in the list and starts a separate thread for it
        public static void StartCars()
        {
            foreach (Car car in carList)
            {
                Thread carTread = new Thread(car.DriveCar);
                carTread.Start();
            }
        }

        // Runs in the main thread while any car has not reached the finnish line
        public static void GetCarDistance() 
        {
            do
            {
                Thread.Sleep(50);
                Console.Clear();
                if (showStatus == true)
                {
                    PrintCarsDistances();
                }
                if (showStatus == false)
                {
                    PrintCarsDistances();
                    PrintRaceInfo();
                }   
                // Will return false as long as no car has reached the finnish line
            } while (CheckCarsDistances() == false);
        }

        // Prints out statistics of each car
        public static void PrintRaceInfo()
        {
            foreach (Car car in carList)
            {
                Console.Write($"\nCar: {car._name}" +
                    $"\nCurrent speed: {car._currentSpeed}km/h, Top speed {car._topSpeed}km/h, Time to top speed {car._timeToTopSpeed}s, Distance: {car._totalDistanceTravelled}m, Time spent driving: {car._time}s, Problem: {car._problem}");
                Console.WriteLine();
            }
        }

        // Prints out the "graphic" parts `o##o>
        public static void PrintCarsDistances() 
        {
            foreach (Car car in carList)
            {
                car.PrintDistance();
                Console.Write($"{car._name}, Problem: {car._problem}");
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        // Checks all the cars in the list if they have reach the goal
        // Will return false as long as no car has reached the finnish line and the true
        public static bool CheckCarsDistances()
        {
            bool hasCarFinnished = false;

            foreach (Car car in carList)
            {
                //                                  vvv This number changes the distance of the race
                if (car._totalDistanceTravelled > 100000)
                {
                    PrintWinner(car);
                    hasCarFinnished = true;
                    break;
                }
            }
            return hasCarFinnished;
        }

        // Changes what gets printed to console depending user input
        public static void SwitchStatus()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "hide")
                {
                    showStatus = true;
                }
                if (input == "show")
                {
                    showStatus = false;
                }
            }            
        }

        // Prints the winner
        public static void PrintWinner(Car winner)
        {
            Console.Write($"\n\t{winner._name} WON!!");
        }        
    }
}