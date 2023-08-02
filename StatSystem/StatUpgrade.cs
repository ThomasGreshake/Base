//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


using System.Collections.Generic;
using System.Xml;
using MyFuncs;

namespace MyGame
{
    //Holds Data that can be used to create statMods with increasing strength. Is used by the statsUpgrader


    public class StatUpgrade
    {
        //Data -----------------------------------------------------------------------
        private string _statName;
        public string StatName { get { return _statName; } }

        private ModType _type;
        public ModType Type { get { return _type; } }

        private readonly List<float> points = new List<float>();
        public int Count { get { return points.Count; } }


        //Setup -----------------------------------------------------------------------
        public StatUpgrade(string statName, ModType type, List<float> points)
        {
            _statName = statName;
            _type = type;
            this.points = points;
        }
        public StatUpgrade(XmlReader reader)
        {
            ReadXml(reader);
        }
        private void ReadXml(XmlReader reader)
        {
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
                        ReadInfo(reader);
                        break;

                    case "Effect":
                        points.Add(reader.ParseFloat("value"));
                        break;
                }
            }
        }
        private void ReadInfo(XmlReader reader)
        {
            _statName = reader.GetAttribute("name");
            _type = reader.ParseEnum<ModType>("type");
        }


        //Getters -----------------------------------------------------------------------
        public float Read(int i)
        {
            if (points.Count == 0)
            {
                return 0;
            }
            if (i < 0)
            {
                return points[0];
            }
            if (i >= points.Count)
            {
                return points[points.Count - 1];
            }
            return points[i];
        }
    }
}