using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotMobile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_IOS
            Destroy(gameObject);
        #elif UNITY_ANDROID
            Destroy(gameObject);
        #endif
    }
}
