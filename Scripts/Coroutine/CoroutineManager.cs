using System;
using System.Collections;
using UnityEngine;

namespace TEDCore.Coroutine
{
    public class CoroutineManager : MonoSingleton<CoroutineManager>
    {
        public CoroutineChain Create()
        {
            return new CoroutineChain();
        }

        public CoroutineChain Create(IEnumerator coroutine)
        {
            return new CoroutineChain(coroutine);
        }

        public IEnumerator WaitForSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        public IEnumerator WaitForEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
        }

        public IEnumerator WaitUntil(Func<bool> predicate)
        {
            yield return new WaitUntil(predicate);
        }

        public IEnumerator WaitWhile(Func<bool> predicate)
        {
            yield return new WaitWhile(predicate);
        }

        public UnityEngine.Coroutine RunCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }
    }
}