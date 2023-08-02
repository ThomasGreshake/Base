//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------



using System;


namespace MyGame
{
    //A class that handles updates, mainly meant for data classes
    //Can set a timeDelta multiplier for all registered update functions
    //Also provides a rareUpdate that doesnt get called every frame for more intense tasks


    public interface ITimeClass
    {
        public float GetTimeMult();
        public void RegisterUpdate(Action<float> update);
        public void RegisterRareUpdate(Action<float> update);
        public void UnregisterUpdate(Action<float> update);
        public void UnregisterRareUpdate(Action<float> update);
    }

    public abstract class TimeClass<T> : DataChild<T>, ITimeClass where T : DataClass
    {
        //Data -------------------------------------------------------
        private float _rareTimer, _timeMult;
        private readonly float _rareUpdateTimer;
        public float TimeMult { get { return _timeMult; } }

        private Action<float> update, rareUpdate;


        //Setup -----------------------------------------------------
        protected TimeClass(T parent, float rareUpdateTimer = 0.1f) : base(parent)
        {
            _timeMult = 1;
            _rareUpdateTimer = rareUpdateTimer;
            _rareTimer = UnityEngine.Random.Range(0, rareUpdateTimer);
        }


        //Publics ------------------------------------------
        public float GetTimeMult()
        {
            return _timeMult;
        }
        public void RegisterUpdate(Action<float> update)
        {
            this.update -= update;
            this.update += update;
        }
        public void RegisterRareUpdate(Action<float> update)
        {
            rareUpdate -= update;
            rareUpdate += update;
        }
        public void UnregisterUpdate(Action<float> update)
        {
            this.update -= update;
        }
        public void UnregisterRareUpdate(Action<float> update)
        {
            rareUpdate -= update;
        }


        //Protected ----------------------------------------
        protected void Update(float delta) //Register this !
        {
            TimeClassUpdateNoTimeMult(delta);

            delta *= _timeMult;
            update?.Invoke(delta);
            _rareTimer += delta;
            if (_rareTimer >= _rareUpdateTimer)
            {
                rareUpdate?.Invoke(_rareTimer);
                _rareTimer = 0;
            }
        }
        protected void SetTimeMult(float mult)
        {
            _timeMult = mult;
        }
        protected virtual void TimeClassUpdateNoTimeMult(float delta) { }
    }
}
