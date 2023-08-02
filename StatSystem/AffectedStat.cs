//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


namespace MyGame
{
    //Represents the way a stat of a statContainer is affected by a statEffect. 

    public class AffectedStat
    {
        public readonly string StatName; //Name of the stat affected
        public readonly float Value; //How strong is the effect
        public readonly ModType Type; //Which type of effect is it

        public AffectedStat(string statName, float value, ModType type)
        {
            StatName = statName;
            Value = value;
            Type = type;
        }
    }
}
