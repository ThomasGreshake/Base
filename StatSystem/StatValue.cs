//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

namespace MyGame
{
    //A stat is a value that can be modified using mods.

    public class StatValue
    {
        //Data -------------------------------------------------------------------------------------------
        public readonly string StatName;
        public readonly float BaseValue;
        private float _value;
        public float Value { get { return _value; } }
        public int ValueAsInt { get { return Mathf.RoundToInt(_value); } }
        public bool ValueAsBool { get { return _value > 0.5f; } }

        private readonly List<StatMod> mods = new();
        public readonly ReadOnlyCollection<StatMod> Mods;

        public static implicit operator float(StatValue s) => s._value;


        //Setup ---------------------------------------------------------------------------------
        public StatValue(string statName, float baseValue)
        {
            StatName = statName;
            BaseValue = baseValue;
            _value = baseValue;
            Mods = mods.AsReadOnly();
        }


        //Publics ------------------------------------------------------------------------
        public void AddMod(StatMod mod)
        {
            RemoveMod(mod.modName, false);
            mods.Add(mod);
            CalculateValue();
        }
        public bool RemoveMod(StatMod mod)
        {
            if (mods.Remove(mod))
            {
                CalculateValue();
                return true;
            }
            return false;
        }
        public bool RemoveMod(string modName, bool recalculate = true)
        {
            StatMod m = GetMod(modName);
            if (m == null)
            {
                return false;
            }

            mods.Remove(m);
            if (recalculate)
            {
                CalculateValue();
            }
            return true;
        }
        public StatMod GetMod(string modName)
        {
            foreach (StatMod m in mods)
            {
                if (m.modName == modName)
                {
                    return m;
                }
            }
            return null;
        }

        //Privates ------------------------------------------------------
        private void CalculateValue()
        {
            float flat = BaseValue;
            float add = 0;
            float mult = 1;
            float min = float.MinValue;
            float max = float.MaxValue;

            for (int i = 0; i < mods.Count; i++)
            {
                StatMod mod = mods[i];

                switch (mod.type)
                {
                    case ModType.Flat:
                        flat += mod.value;
                        break;

                    case ModType.Add:
                        add += mod.value;
                        break;

                    case ModType.Mult:
                        mult *= 1 + mod.value;
                        break;

                    case ModType.Min:
                        min = Mathf.Max(mod.value, min);
                        break;

                    case ModType.Max:
                        max = Mathf.Min(mod.value, max);
                        break;
                }
            }

            if (min > max)
            {
                _value = (min - max) * 0.5f;
                return;
            }

            _value = Mathf.Clamp(flat * (1 + add) * mult, min, max);
        }
    }
}