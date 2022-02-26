using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleControl : MonoBehaviour
{
    private ParticleSystem blackhole;
    public float time = 4; //消失时间
    
    // Start is called before the first frame update
    void Start()
    {
        blackhole = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("EndParticle", time);
    }

    public void EndParticle()
    {
        if (blackhole.transform.localScale.x >= 0 && blackhole.transform.localScale.y >= 0 && blackhole.transform.localScale.z >= 0)
        {
            blackhole.transform.localScale = new Vector3(blackhole.transform.localScale.x - 0.01f, blackhole.transform.localScale.y - 0.01f, blackhole.transform.localScale.z - 0.01f);
        }
    }

   
}
