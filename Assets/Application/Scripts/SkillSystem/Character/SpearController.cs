using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearController : MonoBehaviour
{
    //public float speed = 10;
    private Vector3 newposition;
    // Start is called before the first frame update
    void Start()
    {
        newposition = new Vector3(0, 0.5f, 0);
        //transform.Translate(new Vector3(0, Mathf.Lerp(0,0.5f,20*Time.deltaTime), 0));
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3(Mathf.Lerp(transform.position.x, newposition.x, Time.deltaTime), 
        //    Mathf.Lerp(transform.position.y, newposition.y, Time.deltaTime), Mathf.Lerp(transform.position.z, newposition.z, Time.deltaTime));
        //
        transform.position = Vector3.MoveTowards(transform.position, newposition, Time.deltaTime*0.5f );
    }
}
