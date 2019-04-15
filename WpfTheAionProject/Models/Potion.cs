using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTheAionProject.Models
{
    public class Potion : GameItem
    {
        public int HealthChange { get; set; }
        public int LivesChange { get; set; }

        public Potion (int id, string itemName, int price, int healthChange, int livesChange, string description, int experiencePoints)
            : base(id, itemName, price, description, experiencePoints)
        {
            HealthChange = healthChange;
            LivesChange = livesChange;
        }

        public override string InformationString()
        {
            if (HealthChange != 0)
            {
                return $"{ItemName}: {Description}\nHealth: {HealthChange}";
            }
            else if (LivesChange != 0)
            {
                return $"{ItemName}: {Description}\nLives: {LivesChange}";
            }
            else
            {
                return $"{ItemName}: {Description}";
            }
        }
    }
}
