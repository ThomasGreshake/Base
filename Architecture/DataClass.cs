//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


using System.Collections.Generic;

namespace MyGame
{
    /* These classes provide the basis for a simple data hierachy
    Add children to any DataClass, which implement the functions OnStart and OnStop
    */

    public abstract class DataClass
    {
        //Data -------------------------------------------------------------------
        private readonly List<DataClass> dataChildren = new();

        private bool _started = false;
        public bool Started { get { return _started; } }


        //Setup --------------------------------------------------------------------
        protected DataClass() { }
        protected DataClass(DataClass parent)
        {
            parent.AddChild(this);
        }


        //Protected ----------------------------------------------------------------------
        protected virtual void OnStart() { }
        protected virtual void OnStop() { }



        //Protected Internal -------------------------------------------------------------
        protected internal virtual void StartTree()
        {
            if (!_started)
            {
                _started = true;
                OnStart();
            }

            for (int i = 0; i < dataChildren.Count; i++)
            {
                dataChildren[i].StartTree();
            }
        }

        protected internal virtual void StopTree()
        {
            if (_started)
            {
                _started = false;
                OnStop();
            }

            for (int i = 0; i < dataChildren.Count; i++)
            {
                dataChildren[i].StopTree();
            }
        }


        //Privates -----------------------------------------------
        private void AddChild(DataClass child)
        {
            dataChildren.Add(child);
        }
    }

    //Just a normal dataclass with a parent reference
    public abstract class DataChild<T> : DataClass where T : DataClass
    {
        public readonly T Parent;

        protected DataChild(T parent) : base(parent)
        {
            this.Parent = parent;
        }
    }
}