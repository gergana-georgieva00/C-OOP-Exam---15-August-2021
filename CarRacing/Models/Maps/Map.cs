using CarRacing.Models.Maps.Contracts;
using CarRacing.Models.Racers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRacing.Models.Maps
{
    public class Map : IMap
    {
        public string StartRace(IRacer racerOne, IRacer racerTwo)
        {
            if (!racerOne.IsAvailable() && !racerTwo.IsAvailable())
            {
                return "Race cannot be completed because both racers are not available!";
            }
            if (!racerOne.IsAvailable() && racerTwo.IsAvailable())
            {
                return $"{racerTwo.Username} wins the race! {racerOne.Username} was not available to race!";
            }
            if (racerOne.IsAvailable() && !racerTwo.IsAvailable())
            {
                return $"{racerOne.Username} wins the race! {racerTwo.Username} was not available to race!";
            }

            racerOne.Race();
            racerTwo.Race();

            var multiplier = 0.0;
            if (racerOne.RacingBehavior == "strict")
            {
                multiplier = 1.2;
            }
            else
            {
                multiplier = 1.1;
            }

            var car1ChanceOfWinning = racerOne.Car.HorsePower * racerOne.DrivingExperience * multiplier;
            var car2ChanceOfWinning = racerTwo.Car.HorsePower * racerTwo.DrivingExperience * multiplier;

            return $"{racerOne.Username} has just raced against {racerTwo.Username}! {(car1ChanceOfWinning > car2ChanceOfWinning ? racerOne.Username : racerTwo.Username)} is the winner!";
        }
    }
}
