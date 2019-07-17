using UnityEngine;

namespace Hiramesaurus.Ananke
{
    public struct DelayedAction : IDelayedAction
    {
        private readonly System.Action callback;

        public DelayedAction (float delay, System.Action callback)
        {
            this.callback = callback;
            var time = Time.time + delay;
            
            AnankeManager.Instance.RegisterDelayedAction (time, this);
        }

        public void Invoke ()
        {
            callback?.Invoke ();
        }
    }
    
    public struct DelayedAction<T> : IDelayedAction
    {
        private readonly T data;
        private readonly System.Action<T> callback;

        public DelayedAction (float delay, T data, System.Action<T> callback)
        {
            this.data = data;
            this.callback = callback;
            var time = Time.time + delay;
            
            AnankeManager.Instance.RegisterDelayedAction (time, this);
        }
        
        public void Invoke ()
        {
            callback?.Invoke (data);
        }
    }

    public interface IDelayedAction
    {
        void Invoke ();
    }
}