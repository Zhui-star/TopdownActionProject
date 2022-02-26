using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlParticlesSpawner : MonoBehaviour {

    public ParticleSystem cps;
    public string bulletTag = "SineBullet";

    private Collider _collider;

     private void Start() {
        // _collider=GetComponent<Collider>();
        // _collider.isTrigger=true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(bulletTag) == true)
        {
            Destroy(collision.gameObject);
            cps.transform.position = collision.transform.position;
            cps.Emit(1);
        }
    }
}
