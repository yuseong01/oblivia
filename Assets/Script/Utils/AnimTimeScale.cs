using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTimeScale : MonoBehaviour
{
    [SerializeField] float timeScale = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
