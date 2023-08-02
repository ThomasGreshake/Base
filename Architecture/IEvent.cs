//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


using System;

namespace MyGame
{
    /*
    This is single interface is basically my event system.

    Specify events using different interfaces that derive from IEvent<T>. For example, if you need an event that 
    gets called if a property x of the class "example" changes, create an interface "Xchanged" deriving from IEvent<Xchanged>,
    with a public method GetClass(), and let "example" implement that interface.
    One can ofc implement additional getters associated with the event to get all the info needed, and easily add more things later

    Use Xchanged.Call(this) on that class everytime x changes, or Xchanged.Call(new OnEventClass),
    where OnEventClass implements the interface and stores additional one time info
    Listeners can register themselves and get the specific instance where x changed using the getters on the interface

    While listeners could in theory use a comparison to find out if they handle that specific class, a register with a static 
    constructor that uses a manager class to find changes on other classes is really easy and works better imo

    This means this simple system can handle any possbile type of event without any type conversions on the listener side and
    does not use reflection or any other akward implementations of event systems I have seen.
    I have never seen an event system implemented like this, but its mine and it hasnt caused issues yet!
    */


    public interface IEvent<T> where T : IEvent<T>
    {
        //Data ----------------------------------------
        private static event Action<T> onEvent;


        //Publics ----------------------------------------------------------
        public static void Register(Action<T> listener)
        {
            onEvent -= listener; //Lazy way to prevent double registers
            onEvent += listener;
        }
        public static void Unregister(Action<T> listener)
        {
            onEvent -= listener;
        }
        public static void Call(T eventInterface)
        {
            onEvent?.Invoke(eventInterface);
        }


        //Constructor -----------------------------------------------------
        static IEvent()
        {
            /*
            While T doesnt have to be an interface technically, events might get confused with one another
            So, just to be safe, error out
            */

            if (!typeof(T).IsInterface)
            {
                throw new Exception("The event must be implemented using an interface");
            }
        }
    }
}
