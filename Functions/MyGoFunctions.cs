//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


using UnityEngine;

namespace MyGoFuncs
{
    public static class MyGoFunctions
    {
        public static void ChangeAllLayersRecursive(Transform transform, int layer)
        {
            transform.gameObject.layer = layer;

            foreach (Transform child in transform)
            {
                if (child == null)
                {
                    continue;
                }
                ChangeAllLayersRecursive(child, layer);
            }
        }
        public static Transform FindRecursive(Transform transform, string childName)
        {
            foreach (Transform child in transform)
            {
                if (child.name == childName)
                {
                    return child;
                }

                Transform foundInChild = FindRecursive(child, childName);

                if (foundInChild != null)
                {
                    return foundInChild;
                }
            }
            return null;
        }

        public static string RotateAndAddOn(SpriteRenderer sr, bool n, bool e, bool s, bool w)
        {
            if (n && e && s && w)
            {
                sr.transform.localEulerAngles = Vector3.zero;
                return "_NESW";
            }
            if (n && !w)
            {
                sr.transform.localEulerAngles = Vector3.zero;
                return GetName(n, e, s);
            }
            if (e && !n)
            {
                sr.transform.localEulerAngles = new Vector3(0, 0, -90);
                return GetName(e, s, w);
            }
            if (s && !e)
            {
                sr.transform.localEulerAngles = new Vector3(0, 0, 180);
                return GetName(s, w, n);
            }
            if (w && !s)
            {
                sr.transform.localEulerAngles = new Vector3(0, 0, 90);
                return GetName(w, n, e);
            }
            sr.transform.localEulerAngles = Vector3.zero;
            return "_";
        }
        public static string FlipAndAddOn(SpriteRenderer sr, bool n, bool e, bool s, bool w)
        {
            if (s && !n)
            {
                sr.flipY = true;
                return GetNameAddon(s, e, n, w);
            }
            return GetNameAddon(n, e, s, w);
        }
        public static string FlipAndAddOn(SpriteRenderer sr, bool n, bool s)
        {
            if (s && !n)
            {
                sr.flipY = true;
                return GetName(s, n);
            }
            return GetName(n, s);
        }
        public static string RotateAndAddOn(Transform t, bool n, bool s)
        {
            if (s && !n)
            {
                t.rotation *= Quaternion.Euler(0, 0, 180);
                return GetName(s, n);
            }
            return GetName(n, s);
        }

        private static string GetName(bool n, bool s)
        {
            string name = "_";
            name = n ? name + "N" : name;
            name = s ? name + "S" : name;
            return name;
        }
        private static string GetName(bool n, bool e, bool s)
        {
            string name = "_";
            name = n ? name + "N" : name;
            name = e ? name + "E" : name;
            name = s ? name + "S" : name;
            return name;
        }
        public static string GetNameAddon(bool n, bool e, bool s, bool w)
        {
            string name = "_";
            name = n ? name + "N" : name;
            name = e ? name + "E" : name;
            name = s ? name + "S" : name;
            name = w ? name + "W" : name;
            return name;
        }
    }
}