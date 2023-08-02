//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


using System.Collections;
using UnityEngine;
using System;


namespace MyGame
{
    //Just a normal singleton with some additional things


    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        //Data -----------------------------------------------------------------------------------------
        protected static T instance { get; private set; }

        private Action<float> update;


        //Setup ------------------------------------------------------------------------------------------
        private void Awake()
        {
            if (instance != null)
            {
                Debug.Log("Multiple instances of the same Singleton");
                Destroy(gameObject);
                return;
            }

            instance = this as T;

            LateAwake();
        }

        protected virtual void LateAwake(){}


        //Publics ------------------------------------------------------------------------
        public static void StartRoutine(IEnumerator i)
        {
            instance.StartCoroutine(i);
        }
        public static void StopRoutine(IEnumerator i)
        {
            instance.StopCoroutine(i);
        }
        public static void RegisterUpdate(Action<float> u)
        {
            instance.update += u;
        }
        public static void UnregisterUpdate(Action<float> u)
        {
            instance.update -= u;
        }
        public static Transform GetTransform()
        {
            return instance.transform;
        }


        //Privates -----------------------------------------------------------------------
        private void Update()
        {
            update?.Invoke(Time.deltaTime);
        }
    }
}
