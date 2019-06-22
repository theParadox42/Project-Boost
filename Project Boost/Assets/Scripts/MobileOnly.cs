using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileOnly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_IOS
        #elif UNITY_ANDROID
        #else
        Destroy(gameObject);
        #endif
    }
}
