using System.Collections;
using UnityEngine;

namespace Services.Coroutines
{
    public interface ICoroutineServices
    {
        Coroutine StartRoutine(IEnumerator enumerator);
        void StopRoutine(Coroutine routine);
    }
}