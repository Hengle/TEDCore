using TEDCore.UnitTesting;
using TEDCore.Resource;
using UnityEngine;

public class UnitTesting_Resource : BaseUnitTesting
{
    [TestButton]
    public void LoadAsyncAssetTenTimes()
    {
        ResourceManager.Instance.LoadAsync<GameObject>("EmptyAsset", delegate(GameObject obj)
            {
                for (int i = 0; i < 10; i++)
                {
                    GameObject.Instantiate(obj);
                }

                ResourceManager.Instance.Unload<GameObject>("EmptyAsset");
            });
    }


    [TestButton]
    public void UnloadAsyncAssetTenTimes()
    {
        for (int i = 0; i < 10; i++)
        {
            ResourceManager.Instance.LoadAsync<GameObject>("EmptyAsset", delegate(GameObject obj)
                {
                    GameObject.Instantiate(obj);
                    ResourceManager.Instance.Unload<GameObject>("EmptyAsset");
                });
        }
    }


    [TestButton]
    public void LoadAsyncAssetBundleTenTimes()
    {
        ResourceManager.Instance.LoadAsync<GameObject>("EmptyAssetBundle", delegate(GameObject obj)
            {
                for (int i = 0; i < 10; i++)
                {
                    GameObject.Instantiate(obj);
                }

                ResourceManager.Instance.Unload<GameObject>("EmptyAssetBundle");
            });
    }


    [TestButton]
    public void UnloadAsyncAssetBundleTenTimes()
    {
        for (int i = 0; i < 10; i++)
        {
            ResourceManager.Instance.LoadAsync<GameObject>("EmptyAssetBundle", delegate(GameObject obj)
                {
                    GameObject.Instantiate(obj);
                    ResourceManager.Instance.Unload<GameObject>("EmptyAssetBundle");
                });
        }
    }


    [TestButton]
    public void LoadAsyncAssetTexture()
    {
        ResourceManager.Instance.LoadAsync<Texture>("AssetTexture", delegate(Texture obj)
            {
                
            });
    }


    [TestButton]
    public void UnloadAssetTexture()
    {
        ResourceManager.Instance.Unload<Texture>("AssetTexture");
    }


    [TestButton]
    public void LoadAsyncAssetBundleTexture()
    {
        ResourceManager.Instance.LoadAsync<Texture>("AssetBundleTexture", delegate(Texture obj)
            {
                
            });
    }


    [TestButton]
    public void UnloadAssetBundleTexture()
    {
        ResourceManager.Instance.Unload<Texture>("AssetBundleTexture");
    }
}
