using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Application;
using HTLibrary.Utility;

namespace HTLibrary.Test
{
    public class AddExperienceTest : MonoBehaviour
    {
        public float getExp = 2;

        private float timer;
        public float time = 0.2f;
        private void OnTriggerStay(Collider other)
        {
            if(other.tag==Tags.Player)
            {
                timer += Time.deltaTime;
                if(timer>time)
                {
                    other.GetComponent<CharacterXP>().AddExperience(getExp);
                    timer = 0;

                }

               
            }
        }
    }

}
