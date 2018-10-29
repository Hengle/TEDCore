using TEDCore.UnitTesting;
using TEDCore.Resource;
using UnityEngine;

public class UnitTesting_Resource : BaseUnitTesting
{
    [TestButton]
    public void LoadAsyncAssetTenTimes()
    {
        ResourceSystem.Instance.LoadAsync<GameObject>("EmptyAsset", delegate(GameObject obj)
            {
                for (int i = 0; i < 10; i++)
                {
                    GameObject.Instantiate(obj);
                }

                ResourceSystem.Instance.Unload<GameObject>("EmptyAsset");
            });
    }


    [TestButton]
    public void UnloadAsyncAssetTenTimes()
    {
        for (int i = 0; i < 10; i++)
        {
            ResourceSystem.Instance.LoadAsync<GameObject>("EmptyAsset", delegate(GameObject obj)
                {
                    GameObject.Instantiate(obj);
                    ResourceSystem.Instance.Unload<GameObject>("EmptyAsset");
                });
        }
    }


    [TestButton]
    public void LoadAsyncAssetBundleTenTimes()
    {
        ResourceSystem.Instance.LoadAsync<GameObject>("EmptyAssetBundle", delegate(GameObject obj)
            {
                for (int i = 0; i < 10; i++)
                {
                    GameObject.Instantiate(obj);
                }

                ResourceSystem.Instance.Unload<GameObject>("EmptyAssetBundle");
            });
    }


    [TestButton]
    public void UnloadAsyncAssetBundleTenTimes()
    {
        for (int i = 0; i < 10; i++)
        {
            ResourceSystem.Instance.LoadAsync<GameObject>("EmptyAssetBundle", delegate(GameObject obj)
                {
                    GameObject.Instantiate(obj);
                    ResourceSystem.Instance.Unload<GameObject>("EmptyAssetBundle");
                });
        }
    }


    [TestButton]
    public void LoadAsyncAssetTexture()
    {
        ResourceSystem.Instance.LoadAsync<Texture>("AssetTexture", delegate(Texture obj)
            {
                
            });
    }


    [TestButton]
    public void UnloadAssetTexture()
    {
        ResourceSystem.Instance.Unload<Texture>("AssetTexture");
    }


    [TestButton]
    public void LoadAsyncAssetBundleTexture()
    {
        ResourceSystem.Instance.LoadAsync<Texture>("AssetBundleTexture", delegate(Texture obj)
            {
                
            });
    }


    [TestButton]
    public void UnloadAssetBundleTexture()
    {
        ResourceSystem.Instance.Unload<Texture>("AssetBundleTexture");
    }
}
