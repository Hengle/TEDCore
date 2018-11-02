using System;
using System.Collections;
using UnityEngine;

namespace TEDCore.Coroutine
{
    public static class CoroutineUtils
    {
        public static IEnumerator WaitForSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        public static IEnumerator WaitForEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
        }

        public static IEnumerator WaitUntil(Func<bool> predicate)
        {
            yield return new WaitUntil(predicate);
        }

        public static IEnumerator WaitWhile(Func<bool> predicate)
        {
            yield return new WaitWhile(predicate);
        }
    }
}