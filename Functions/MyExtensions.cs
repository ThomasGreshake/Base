//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


using System.Collections.Generic;
using System;
using System.Xml;

namespace MyFuncs
{
    //A collection of various extensions for IEnumerables (mostly in the spirit of LINQ) and easier XML attribute parsing

    public static class MyExtensions
    {
        //Returns the item that maxes the func
        public static T MaxBy<T>(this IEnumerable<T> list, Func<T, float> func)
        {
            T max = default(T);
            float maxValue = float.MinValue;
            foreach (var item in list)
            {
                float value = func(item);
                if (value > maxValue)
                {
                    maxValue = value;
                    max = item;
                }
            }
            return max;
        }

        //Returns the item that mins the func
        public static T MinBy<T>(this IEnumerable<T> list, Func<T, float> func)
        {
            T min = default(T);
            float minValue = float.MaxValue;
            foreach (var item in list)
            {
                float value = func(item);
                if (value < minValue)
                {
                    minValue = value;
                    min = item;
                }
            }
            return min;
        }

        //Looks if it can find a true condition, doesnt care about what item actually is true
        public static bool FindTrue<T>(this IEnumerable<T> list, Func<T, bool> func)
        {
            foreach (var item in list)
            {
                if (func(item))
                {
                    return true;
                }
            }
            return false;
        }

        //Reads and parses xml attributes
        public static bool ParseBool(this XmlReader reader, string attribute, bool defaultOnFailure = false)
        {
            bool b;
            if (!bool.TryParse(reader.GetAttribute(attribute), out b))
            {
                if (defaultOnFailure)
                {
                    return false;
                }
                throw new Exception("Could not parse attribute " + attribute + " to a bool. Read value: " + reader.GetAttribute(attribute) + " at " + reader.Name);
            }
            return b;
        }
        public static int ParseInt(this XmlReader reader, string attribute, bool defaultOnFailure = false)
        {
            int i;
            if (!int.TryParse(reader.GetAttribute(attribute), out i))
            {
                if (defaultOnFailure)
                {
                    return 0;
                }
                throw new Exception("Could not parse attribute " + attribute + " to an int. Read value: " + reader.GetAttribute(attribute) + " at " + reader.Name);
            }
            return i;
        }
        public static float ParseFloat(this XmlReader reader, string attribute, bool defaultOnFailure = false)
        {
            float f;
            if (!float.TryParse(reader.GetAttribute(attribute), out f))
            {
                if (defaultOnFailure)
                {
                    return 0;
                }
                throw new Exception("Could not parse attribute " + attribute + " to a float. Read value: " + reader.GetAttribute(attribute) + " at " + reader.Name);
            }
            return f;
        }
        public static T ParseEnum<T>(this XmlReader reader, string attribute, bool defaultOnFailure = false) where T : struct, Enum
        {
            T e;
            if (!Enum.TryParse(reader.GetAttribute(attribute), out e))
            {
                if (defaultOnFailure)
                {
                    return default(T);
                }
                throw new Exception("Could not parse attribute " + attribute + " to an enum of type: " + typeof(T).ToString() + ". Read value: " + reader.GetAttribute(attribute));
            }
            return e;
        }
    }
}


