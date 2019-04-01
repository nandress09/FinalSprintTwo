﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTheAionProject.Models;

namespace WpfTheAionProject.DataLayer
{
    /// <summary>
    /// static class to store the game data set
    /// </summary>
    public static class GameData
    {
        public static Player PlayerData()
        {
            return new Player()
            {
                Id = 1,
                Name = "Laura",
                Age = 23,
                JobTitle = Player.JobTitleName.Wolf,
                Race = Character.ParentType.Mother,
                Health = 100,
                Lives = 3,
                ExperiencePoints = 10,
                LocationId = 0
            };
        }

        public static Map GameMap()
        {
            Map gameMap = new Map();

            gameMap.Locations.Add
                (new Location()
                {
                    Id = 1,
                    Name = "Church",
                    Description = "A location to worship.",
                    Accessible = true,
                    ModifiyExperiencePoints = 10,
                    Message = "\tYou have now in the foyer of the Church. This is where all data will be brought to find out who has the baby"
                }
                );

            gameMap.Locations.Add
                (new Location()
                {
                    Id = 2,
                    Name = "The Bakery",
                    Description = "/t The bakery has no need for you or your baby, he simply is a baker,  though he does send his regards" ,
                    Accessible = true,
                    ModifiyExperiencePoints = 10,
                    Message = "To find more clues about your lost baby, please continue to explore the different buildings in the village."
                }
                );

            gameMap.Locations.Add
                (new Location()
                {
                    Id = 3,
                    Name = "The Squires Castle",
                    Description = "The squires keep is full of weapons, both clean and recently used. Will we find any sign of the baby?",
                    Accessible = true,
                    ModifiyExperiencePoints = 10
                }
                );

            gameMap.Locations.Add
                (new Location()
                {
                    Id = 4,
                    Name = "The kings Keep",
                    Description = "Who else to want your baby dead than that of the Queen? She always looked so mean toward animals",
                    Accessible = false,
                    ModifiyExperiencePoints = 50,
                    ModifyLives = -1,
                    RequiredExperiencePoints = 40
                }
                );

            gameMap.Locations.Add
                (new Location()
                {
                   Id = 5,
                    Name = "The Leather Market",
                    Description = "The Leather market reeks of dead babes.. best be on gaurd",
                    Accessible = false,
                    ModifiyExperiencePoints = 20,
                    ModifyHealth = 50,
                    Message = "Boy, best keep moving, you're kind aint welcomed here"
                }
                );

            gameMap.Locations.Add
                (new Location()
                {
                    Id = 6,
                    Name = "Animal Hospital",
                    Description = "Here, all animals are safe. This might be the best place to hear whispers of the lost babe",
                    Accessible = true,
                    ModifiyExperiencePoints = 10
                }
                );

            
            // set the initial location for the player
            
            gameMap.CurrentLocation = gameMap.Locations.FirstOrDefault(l => l.Id == 1);

            return gameMap;
        }
    }
}
