using System.Collections;

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

        public UnityEngine.Coroutine RunCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }
    }
}