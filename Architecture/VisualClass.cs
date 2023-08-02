//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------



using UnityEngine;


namespace MyGame
{
    //Basically a normal monobehaviour that holds a link to the data it represents. Not much here

    public abstract class VisualClass<T> : MonoBehaviour where T : class
    {
        //Data ----------------------------------------------------------
        private T _data;
        public T Data { get { return _data; } }


        //Publics -----------------------------------------------------------
        public void StartRepresentation(T data)
        {
            if (_data == data)
            {
                return;
            }
            if (_data != null)
            {
                OnStop();
            }

            _data = data;
            OnStart();
        }
        public void ClearData()
        {
            OnStop();
            _data = null;
        }


        //Protected --------------------------------------------------------------
        protected virtual void OnStart() { }
        protected virtual void OnStop() { }
    }
}