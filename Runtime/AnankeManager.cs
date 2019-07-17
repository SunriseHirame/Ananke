using System.Collections.Generic;
using UnityEngine;

namespace Hiramesaurus.Ananke
{
    public sealed class AnankeManager : MonoBehaviour
    {
        public static AnankeManager Instance { get; private set; }

        private readonly HashSet<System.Type> isIncludedType = new HashSet<System.Type> ();
        private readonly List<DelayCallBase> delayedCalls = new List<DelayCallBase> ();

        internal void RegisterDelayedAction<T> (float time, T action) where T : IDelayedAction
        {
            DelayedCall<T>.Add (time, action);
            if (isIncludedType.Contains (typeof (T)))
                return;
            
            delayedCalls.Add (DelayedCall<T>.GetInstance ());
            isIncludedType.Add (typeof (T));
        }
        
        private void Update ()
        {
            var time = Time.time;

            foreach (var call in delayedCalls)
            {
                call.CheckInvoke (time);
            }
        }

        [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init ()
        {
            Instance = new GameObject(string.Empty).AddComponent<AnankeManager> ();
            Instance.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        private sealed class DelayedCall<T> : DelayCallBase where T : IDelayedAction
        {
            private static readonly List<T> invokedActions = new List<T> ();
            
            public static void Add (float delay, in T action)
            {
                invokeTimes.Add (delay);
                invokedActions.Add (action);             
            }

            public static DelayCallBase GetInstance ()
            {
                return new DelayedCall<T> ();
            }

            protected override void InvokeAndRemove (int index)
            {
                invokedActions[index].Invoke ();
                invokedActions.RemoveAt (index);
            }
        }

        private abstract class DelayCallBase
        {
            protected static readonly List<float> invokeTimes = new List<float> ();
            
            internal void CheckInvoke (float time)
            {
                var last = invokeTimes.Count - 1;
                for (var i = last; i >= 0; i--)
                {
                    if (time < invokeTimes[i]) 
                        continue;
                    
                    invokeTimes.RemoveAt (i);
                    InvokeAndRemove (i);
                }
            }

            protected abstract void InvokeAndRemove (int index);
        }
    }

}