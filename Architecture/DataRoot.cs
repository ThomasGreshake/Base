//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


//Provides access to some monobehaviour functionalities, which dont exist on my dataClasses, and provides a root for the data hierachy

namespace MyGame
{
    public abstract class RootClass<T> : Singleton<RootClass<T>> where T : DataClass, new()
    {
        //Data -------------------------------------------------------------------------------
        private T data;
        public static T Root { get { return instance.data; } }


        //Protected --------------------------------------------------------------------------------
        protected sealed override void LateAwake()
        {
            data = new();
        }

        //Privates --------------------------------------------------------------------------
        private void Start()
        {
            data.StartTree();
        }
    }
}
