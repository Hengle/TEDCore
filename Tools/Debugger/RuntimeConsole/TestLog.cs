using UnityEngine;

public class TestLog : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1))
        {
            Debug.Log("This is log.");
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            Debug.LogWarning("This is log warning.");
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            Debug.LogError("This is log error.");
        }

        if (Input.GetKey(KeyCode.Alpha4))
        {
            Debug.LogException(new System.Exception("This is log exception."));
        }

        if (Input.GetKey(KeyCode.Alpha5))
        {
            GameObject test = null;
            test.SetActive(false);
        }

        if (Input.GetKey(KeyCode.Alpha6))
        {
            int[] array = {0, 1};
            array[2] = 2;
        }
    }
}
