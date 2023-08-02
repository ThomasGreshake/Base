//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------



using System.Collections.Generic;
using System.Linq;

namespace MyGame
{
    //Creates a bunch of statmods from a list of statUpgrades and applies them to a statContainer.
    //This could represent leveling up, increasing in tier, etc

    public class StatsUpgrader
    {
        //Data ------------------------------------------------------
        private readonly string name;
        private List<StatUpgrade> upgrades;
        private readonly StatContainer container;

        private int _currentLevel = -1;
        public int CurrentLevel { get { return _currentLevel; } }


        //Setup -----------------------------------------------------
        public StatsUpgrader(string name, StatContainer container, IList<StatUpgrade> upgrades)
        {
            this.name = name;
            this.container = container;
            if (upgrades != null)
            {
                this.upgrades = upgrades.ToList();
            }
        }


        //Publics --------------------------------------------------
        public void UpgradeTo(int level)
        {
            if (_currentLevel == level)
            {
                return;
            }
            _currentLevel = level;

            if (upgrades == null)
            {
                return;
            }
            foreach (StatUpgrade u in upgrades)
            {
                ApplyUpgrade(u);
            }
        }
        public void RemoveAllEffects()
        {
            _currentLevel = -1;
            if (upgrades == null)
            {
                return;
            }

            foreach (StatUpgrade u in upgrades)
            {
                container.GetStat(u.StatName).RemoveMod(name);
            }
        }
        public float ReadAt(string statName, int i)
        {
            foreach (var item in upgrades)
            {
                if (item.StatName == statName)
                {
                    return item.Read(i);
                }
            }
            return -1;
        }


        //Privates ------------------------------------------------------
        private void ApplyUpgrade(StatUpgrade u)
        {
            container.GetStat(u.StatName).AddMod(new StatMod(name, u.Read(_currentLevel), u.Type));
        }
    }
}
