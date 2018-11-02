using System;
using System.Collections;
using System.Collections.Generic;

namespace TEDCore.Coroutine
{
    public class CoroutineChain
    {
        private Queue<IEnumerator> m_coroutines;

        public CoroutineChain()
        {
            m_coroutines = new Queue<IEnumerator>();
        }

        public CoroutineChain(IEnumerator coroutine)
        {
            m_coroutines = new Queue<IEnumerator>();
            m_coroutines.Enqueue(coroutine);
        }

        public CoroutineChain Enqueue(IEnumerator coroutine)
        {
            m_coroutines.Enqueue(coroutine);
            return this;
        }

        public CoroutineChain Enqueue(Action action)
        {
            m_coroutines.Enqueue(CreateEnumerator(action));
            return this;
        }

        public CoroutineChain Enqueue<T>(Action<T> action, T data)
        {
            m_coroutines.Enqueue(CreateEnumerator(action, data));
            return this;
        }

        private IEnumerator CreateEnumerator(Action action)
        {
            yield return null;

            if (action != null)
            {
                action();
            }
        }

        private IEnumerator CreateEnumerator<T>(Action<T> action, T data)
        {
            yield return null;

            if (action != null)
            {
                action(data);
            }
        }

        public void RunCoroutine()
        {
            CoroutineManager.Instance.RunCoroutine(RunEnemerators());
        }

        private IEnumerator RunEnemerators()
        {
            while (m_coroutines.Count > 0)
            {
                yield return CoroutineManager.Instance.RunCoroutine(m_coroutines.Dequeue());
            }
        }
    }
}