using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTheAionProject.Models
{
    public class Treasure: GameItem
    {
        Coin,
        Jewel
    }
    public TreasureType Type { get; set; }

    public Treasure(int id, string name, int value, TreasureType, string description, int experiencePoints)
        : base(id, name, value, description, experiencePoints)
    {
        Type = type;
    }

    public override string InformationString()
    {
        return $"{Name}: {Description}\nValue: {Value}";
    }


}
