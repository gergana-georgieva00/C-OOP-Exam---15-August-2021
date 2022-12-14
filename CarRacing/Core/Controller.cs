using CarRacing.Core.Contracts;
using CarRacing.Models.Cars;
using CarRacing.Models.Cars.Contracts;
using CarRacing.Models.Maps;
using CarRacing.Models.Maps.Contracts;
using CarRacing.Models.Racers;
using CarRacing.Models.Racers.Contracts;
using CarRacing.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarRacing.Core
{
    class Controller : IController
    {
        private CarRepository cars;
        private RacerRepository racers;
        private Map map;
        public Controller()
        {
            cars = new CarRepository();
            racers = new RacerRepository();
            map = new Map();
        }

        public string AddCar(string type, string make, string model, string VIN, int horsePower)
        {
            ICar car;
            switch (type)
            {
                case "SuperCar":
                    car = new SuperCar(make, model, VIN, horsePower);
                    break;
                case "TunedCar":
                    car = new TunedCar(make, model, VIN, horsePower);
                    break;
                default:
                    throw new ArgumentException("Invalid car type!");
            }

            cars.Add(car);
            return $"Successfully added car {make} {model} ({VIN}).";
        }

        public string AddRacer(string type, string username, string carVIN)
        {
            if (type != "ProfessionalRacer" && type != "StreetRacer")
            {
                throw new ArgumentException("Invalid racer type!");
            }

            var car = cars.FindBy(carVIN);
            if (car is null)
            {
                throw new ArgumentException("Car cannot be found!");
            }

            IRacer racer;
            switch (type)
            {
                case "ProfessionalRacer":
                    racer = new ProfessionalRacer(username, car);
                    racers.Add(racer);
                    break;
                case "StreetRacer":
                    racer = new StreetRacer(username, car);
                    racers.Add(racer);
                    break;
            }

            return $"Successfully added racer {username}.";
        }

        public string BeginRace(string racerOneUsername, string racerTwoUsername)
        {
            if (racers.FindBy(racerOneUsername) is null || racers.FindBy(racerTwoUsername) is null)
            {
                throw new ArgumentException($"Racer " +
                    $"{(racers.FindBy(racerOneUsername) is null ? racerOneUsername : racerTwoUsername)} cannot be found!");
            }

            var racer1 = racers.FindBy(racerOneUsername);
            var racer2 = racers.FindBy(racerTwoUsername);

            return map.StartRace(racer1, racer2);
        }

        public string Report()
        {
            var sb = new StringBuilder();
            foreach (var racer in racers.Models.OrderByDescending(m => m.DrivingExperience).ThenBy(m => m.Username))
            {
                sb.AppendLine(racer.ToString());
            }

            return sb.ToString().Trim();
        }
    }
}
