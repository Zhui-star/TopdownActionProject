using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
   // public ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        //particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("Dispear", 1.2f);
    }

    public void Dispear()
    {
        Destroy(gameObject);
    }
}
