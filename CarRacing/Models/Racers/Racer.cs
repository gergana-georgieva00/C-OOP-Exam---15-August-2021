using CarRacing.Models.Cars.Contracts;
using CarRacing.Models.Racers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRacing.Models.Racers
{
    public class Racer : IRacer
    {
        private string username;
        private string racingBehaviour;
        private int drivingExperience;
        private ICar car;

        public Racer(string username, string racingBehavior, int drivingExperience, ICar car)
        {
            this.Username = username;
            this.RacingBehavior = racingBehavior;
            this.DrivingExperience = drivingExperience;
            this.Car = car;
        }

        public string Username 
        {
            get => username;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Username cannot be null or empty.");
                }

                username = value;
            }
        }

        public string RacingBehavior
        {
            get => racingBehaviour;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Racing behavior cannot be null or empty.");
                }

                racingBehaviour = value;
            }
        }

        public int DrivingExperience
        {
            get => drivingExperience;
            protected set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentException("Racer driving experience must be between 0 and 100.");
                }

                drivingExperience = value;
            }
        }

        public ICar Car
        {
            get => car;
            private set
            {
                if (value is null)
                {
                    throw new ArgumentException("Car cannot be null or empty.");
                }

                car = value;
            }
        }

        public bool IsAvailable()
        {
            if (this.Car.FuelAvailable - this.Car.FuelConsumptionPerRace >= 0 && this.Car.GetType().Name != "TunedCar")
            {
                return true;
            }
            else if (this.Car.FuelAvailable - this.Car.FuelConsumptionPerRace >= 0 && this.Car.GetType().Name == "TunedCar")
            {
                if ((int)Math.Round(this.Car.HorsePower - (Car.HorsePower * 0.03)) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Race()
        {
            Car.Drive();
            if (this.GetType().Name == "ProfessionalRacer")
            {
                this.DrivingExperience += 10;
            }
            else
            {
                this.DrivingExperience += 5;
            }
        }

        public override string ToString()
            => $"{this.GetType().Name}: {this.Username}" + Environment.NewLine
                + $"--Driving behavior: {this.RacingBehavior}" + Environment.NewLine
                + $"--Driving experience: {this.DrivingExperience}" + Environment.NewLine
                + $"--Car: {this.Car.Make} {this.Car.Model} ({this.Car.VIN})";
    }
}
