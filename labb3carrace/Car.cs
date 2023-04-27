using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace labb3carrace
{
    internal class Car
    {
        public string _name { get; set; }
        public decimal _currentSpeed { get; set; }
        public decimal _topSpeed { get; set; }
        public decimal _timeToTopSpeed { get; set; }
        public decimal _travelledDistance { get; set; }
        public decimal _totalDistanceTravelled { get; set; }
        public string  _problem { get; set; }
        public decimal _time { get; set; }
        public decimal _timeSpentAccelerating { get; set; }

        public Car(string name, decimal topSpeed, decimal timeToTopSpeed)
        {
            _name = name;
            _topSpeed = topSpeed;
            _timeToTopSpeed = timeToTopSpeed;
            _time = 0.05m;
            _problem = "None";
        }

        // Start all the calculations of distance, speed and acceleration
        public void DriveCar()
        {
            while (true)
            {
                // Every 30 seconds a car have spent driving it could generate an accident 
                if (_time % 30 == 0)
                {
                    GenerateAccident();
                    _time += 0.05m;
                }
                else
                {
                    AccelerateCar();

                    // The distance travelled in the last 0,05 second
                    _travelledDistance = _currentSpeed * 0.05m;

                    Thread.Sleep(50);
                    _time += 0.05m;

                    // Adding the travelled distance to the total distance
                    _totalDistanceTravelled += _travelledDistance;
                }
            }
        }

        // Generates a random accident and resets the cars speed and acceleration time to 0
        public void GenerateAccident()
        {
            Random random = new Random();
            int num = random.Next(1, 51);

            if (num == 5 || num == 10 || num == 15 || num == 20 || num == 25 || num == 30 || num == 35 || num == 40 || num == 45 || num == 50)
            {
                _problem = "Engine failure, topspeed permanently lowered by 1";
                _currentSpeed -= 1;
                _topSpeed-= 1;
            }

            if (num == 9 || num == 19 || num == 29 || num == 39 || num == 49)
            {
                _currentSpeed = 0;
                _timeSpentAccelerating = 0;
                for (int i = 10; i > 0; i--)
                {
                    _problem = $"Bird on window, wait {i} sec";
                    Thread.Sleep(1000);
                }
                _problem = "None";
            }

            if (num == 1 || num == 24)
            {
                _currentSpeed = 0;
                _timeSpentAccelerating = 0;
                for (int i = 10; i > 0; i--)
                {
                    _problem = $"Change tires, wait {i} sec";
                    Thread.Sleep(1000);
                }
                _problem = "None";
            }

            if (num == 48)
            {
                _currentSpeed = 0;
                _timeSpentAccelerating = 0;
                for (int i = 30; i > 0; i--)
                {
                    _problem = $"Refill fuel, wait {i} sec";
                    Thread.Sleep(1000);
                }
                _problem = "None";
            }
        }

        // Calculates the acceleration curve of the car
        // Is currently done with a cosinus curve
        public void AccelerateCar()
        {
            // Once _currentSpeed reaches _topSpeed number it will no longer accelerate
            if (_currentSpeed == _topSpeed)
            {
                // Do nothing here
            }
            else
            {
                _timeSpentAccelerating += 0.05m;

                // this calculation is what simulates the acceleration of the car
                double currentSpeed = Math.Cos(((double)_timeSpentAccelerating / ((double)_timeToTopSpeed / Math.PI)) - Math.PI) * ((double)_topSpeed / 2) + ((double)_topSpeed / 2);

                currentSpeed = Math.Round(currentSpeed);
                _currentSpeed = (decimal)currentSpeed;
            }
        }

        // Prints the "graphical" distance of the car
        public void PrintDistance()
        {
            Console.Write("Start");
            for (int i = 0; i < 100; i++)
            {//                                          vvv Change this number to always have 2 less 0s than the distance of the race
                if (Math.Round(_totalDistanceTravelled / 1000) == i)
                {
                    Console.Write("`o##o>");
                }
                else
                {
                    Console.Write("_");
                }
            }
            Console.WriteLine("Finnish");
        }
    }
}   