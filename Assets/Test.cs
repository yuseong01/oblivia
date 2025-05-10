using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {

        Invoke("disable", 3f);
        test();
    }

    void disable()
    {
        this.gameObject.SetActive(false);
    }

    void test()
    {
        while (true)
        {
            Debug.Log("¾ßÈ£");
        }
    }
}
