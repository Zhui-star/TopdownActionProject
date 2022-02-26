using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    public float speed=150;//测试用
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1*Time.deltaTime*speed, 0));
    }
}
