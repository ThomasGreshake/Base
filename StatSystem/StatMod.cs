//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


namespace MyGame
{
    /*
    StatValues can be modified using mods.
    */
    
    public enum ModType { Flat, Add, Mult, Min, Max }

    public class StatMod
    {
        public readonly string modName;
        public readonly float value;
        public readonly ModType type;

        public StatMod(string modName, float value, ModType type)
        {
            this.modName = modName;
            this.value = value;
            this.type = type;
        }
    }
}
