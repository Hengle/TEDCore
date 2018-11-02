using System.Collections;

namespace TEDCore.Coroutine
{
    public class CoroutineManager : MonoSingleton<CoroutineManager>
    {
        public CoroutineChain CreateCoroutine()
        {
            return new CoroutineChain();
        }

        public CoroutineChain Create(IEnumerator coroutine)
        {
            return new CoroutineChain(coroutine);
        }
    }
}