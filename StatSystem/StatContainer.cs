//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MyGame
{
    /*
    A StatContainer holds stats and provides access to them and their modifications using the stat names
    */
    public class StatContainer
    {
        //Data ---------------------------------------------------------------------------------------
        private readonly Dictionary<string, StatValue> stats;
        private readonly List<StatEffect> appliedEffects = new();


        //Setup ----------------------------------------------------------------------------------------
        public StatContainer(StatEffect baseSettings, params StatEffect[] baseEffects) : this(baseSettings, baseEffects.ToList()) { }
        public StatContainer(StatEffect baseSettings, List<StatEffect> baseEffects)
        {
            stats = new Dictionary<string, StatValue>(baseSettings.AffectedStats.Count);
            foreach (AffectedStat effect in baseSettings.AffectedStats)
            {
                StatValue stat = new(effect.StatName, effect.Value);
                stats.TryAdd(stat.StatName, stat);
            }

            foreach (StatEffect effect in baseEffects)
            {
                ApplyEffect(effect);
            }
        }


        //Publics ------------------------------------------------------------------------------------------------------
        public StatValue GetStat(string statName)
        {
            StatValue stat;
            if (stats.TryGetValue(statName, out stat))
            {
                return stat;
            }
            Debug.LogError("Could not find stat with name: " + statName);
            return null;
        }
        public void ApplyEffect(StatEffect effect)
        {
            StatEffect current = GetEffect(effect.Name);
            if (current != null)
            {
                RemoveEffect(current);
            }
            appliedEffects.Add(effect);

            foreach (AffectedStat a in effect.AffectedStats)
            {
                StatMod mod = new StatMod(effect.Name, a.Value, a.Type);
                GetStat(a.StatName)?.AddMod(mod);
            }
        }
        public void RemoveEffect(StatEffect effect)
        {
            if (!appliedEffects.Remove(effect))
            {
                return;
            }

            foreach (AffectedStat a in effect.AffectedStats)
            {
                GetStat(a.StatName)?.RemoveMod(effect.Name);
            }
        }
        public StatEffect GetEffect(string name)
        {
            foreach (StatEffect effect in appliedEffects)
            {
                if (effect.Name == name)
                {
                    return effect;
                }
            }
            return null;
        }
        public bool RemoveEffect(string effectName)
        {
            StatEffect effect = GetEffect(effectName);
            if (effect == null)
            {
                return false;
            }

            RemoveEffect(effect);
            return true;
        }
    }
}