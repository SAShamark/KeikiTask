using System.Collections;
using UnityEngine;

namespace Services.Coroutines
{
    public class CoroutineServices : MonoBehaviour, ICoroutineServices
    {
        public Coroutine StartRoutine(IEnumerator enumerator)
        {
            return StartCoroutine(enumerator);
        }

        public void StopRoutine(Coroutine routine)
        {
            if (routine != null)
            {
                StopCoroutine(routine);
            }
        }
    }
}