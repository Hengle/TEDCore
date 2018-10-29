using TEDCore.UnitTesting;
using TEDCore.PlayerData;
using System.Collections.Generic;

public class UnitTesting_PlayerData : BaseUnitTesting
{
    public class TestPlayerData : PlayerData
    {
        public override string FileName
        {
            get
            {
                return "TestPlayerData.dat";
            }
        }

        public int id;
        public string name;
        public int[] exp;
        public List<int> atk;
        public List<WeaponData> weaponDatas;
    }

    [System.Serializable]
    public class WeaponData
    {
        public int id;
        public string name;
        public int atk;

        public WeaponData(int id, string name, int atk)
        {
            this.id = id;
            this.name = name;
            this.atk = atk;
        }
    }

    [TestButton]
    public void SaveData()
    {
        TestPlayerData testData = new TestPlayerData();
        testData.id = 1;
        testData.name = "Ted";
        testData.exp = new int[]{0, 1, 2, 3, 4};
        testData.atk = new List<int>(){10, 20, 30};
        testData.weaponDatas = new List<WeaponData>() {
            new WeaponData(1, "Weapon_1", 10),
            new WeaponData(2, "Weapon_2", 20)
        };

        PlayerDataUtils.Save(testData);

        UnityEngine.Debug.Log("SaveData");
    }

    [TestButton]
    public void LoadData()
    {
        TestPlayerData testData = PlayerDataUtils.Load<TestPlayerData>();
        UnityEngine.Debug.Log("id = " + testData.id);
        UnityEngine.Debug.Log("name = " + testData.name);

        if(testData.exp != null)
        {
            for (int i = 0; i < testData.exp.Length; i++)
            {
                UnityEngine.Debug.LogFormat("exp[{0}] = {1}", i, testData.exp[i]);
            }
        }

        if (testData.atk != null)
        {
            for (int i = 0; i < testData.atk.Count; i++)
            {
                UnityEngine.Debug.LogFormat("atk[{0}] = {1}", i, testData.atk[i]);
            }
        }

        if (testData.weaponDatas != null)
        {
            for (int i = 0; i < testData.weaponDatas.Count; i++)
            {
                UnityEngine.Debug.LogFormat("weaponDatas[{0}].id = {1}", i, testData.weaponDatas[i].id);
                UnityEngine.Debug.LogFormat("weaponDatas[{0}].name = {1}", i, testData.weaponDatas[i].name);
                UnityEngine.Debug.LogFormat("weaponDatas[{0}].atk = {1}", i, testData.weaponDatas[i].atk);
            }
        }
    }
}
