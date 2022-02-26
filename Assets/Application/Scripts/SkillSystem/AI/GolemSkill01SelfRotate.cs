using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSkill01SelfRotate : MonoBehaviour
{
    public float speed = 150;//测试用

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(1 * Time.deltaTime * speed,0 , 0));
    }
}
