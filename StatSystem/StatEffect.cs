//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using MyFuncs;
using System;

namespace MyGame
{
    //A collection of effects applied to the stats of a stat container

    public class StatEffect
    {
        //Data ----------------------------------------------------------------
        private string name;
        public string Name { get { return name; } }

        public ReadOnlyCollection<AffectedStat> AffectedStats { get; private set; }


        //Setup -----------------------------------------------------------------
        public StatEffect(string name, ReadOnlyCollection<AffectedStat> affectedStats)
        {
            this.name = name;
            AffectedStats = affectedStats;
        }
        public StatEffect(XmlReader reader)
        {
            ReadXml(reader);
        }

        private void ReadXml(XmlReader reader)
        {
            List<AffectedStat> f = new List<AffectedStat>();

            reader.MoveToContent();

            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                switch (reader.Name)
                {
                    case "Info":
                        name = reader.GetAttribute("name");
                        break;

                    case "Stat":
                        string statName = reader.GetAttribute("name");
                        float value = reader.ParseFloat("value");

                        ModType modType; //Defaults the modtype for base stats, which is why ParseEnum shouldnt be used here
                        string typeString = reader.GetAttribute("type");
                        if (typeString == null || !Enum.TryParse<ModType>(typeString, out modType))
                        {
                            modType = ModType.Flat;
                        }

                        f.Add(new AffectedStat(statName, value, modType));
                        break;
                }
            }

            AffectedStats = f.AsReadOnly();
        }
    }
}