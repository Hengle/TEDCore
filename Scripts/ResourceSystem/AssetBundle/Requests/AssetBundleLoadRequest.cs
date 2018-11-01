using System.Collections;

namespace TEDCore.AssetBundle
{
    public abstract class AssetBundleLoadRequest : IEnumerator
    {
        public object Current
        {
            get
            {
                return null;
            }
        }

        public bool MoveNext()
        {
            return !IsDone();
        }

        public void Reset()
        {
            
        }

        public abstract bool Update();
        public abstract bool IsDone();
    }

}