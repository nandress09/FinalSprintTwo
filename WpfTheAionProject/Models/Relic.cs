using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTheAionProject.Models
{
    public enum Relic:GameItem
    {
    public enum UseActionType
    {
        OPENLOCATION,
        KILLPLAYER
    }

    public UseActionType UseAction { get; set; }

    public Relic(int id, string itemName, int price, string description, int experiencePoints, string townsMenThoughts, UseActionType useAction)
        : base(id, itemName, price, description, experiencePoints, townsMenThoughts)
    {
        UseAction = useAction;
    }

    public override string InformationString()
    {
        return $"{ItemName}: {Description}\nValue: {Price}";
    }
}
}
