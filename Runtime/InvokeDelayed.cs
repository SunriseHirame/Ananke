using UnityEngine;

public class InvokeDelayed : MonoBehaviour
{
    private void Start ()
    {
        for (var i = 0; i < 10000; i++)
        {
            new Hiramesaurus.Ananke.DelayedAction (10, () => { });
        }
    }
}
