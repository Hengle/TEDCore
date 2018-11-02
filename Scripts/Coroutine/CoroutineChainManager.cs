using System.Collections;

namespace TEDCore.Coroutine
{
    public class CoroutineChainManager : MonoSingleton<CoroutineChainManager>
    {
        public CoroutineChain Create()
        {
            return new CoroutineChain();
        }

        public CoroutineChain Create(IEnumerator coroutine)
        {
            return new CoroutineChain(coroutine);
        }
    }
}