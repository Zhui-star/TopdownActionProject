using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    public class RayCastCamToPlayer : MonoBehaviour
    {
        private MeshRenderer meshRenders;
        public float occluedValue;
        private float initialValue;
        private Transform player;
        private void Start()
        {
            GameObject go = GameObject.FindGameObjectWithTag(Tags.Player);
            if (go != null)
            {
                player = go.transform;
            }

            initialValue = 1;
        }

        private void Update()
        {
            if (player == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag(Tags.Player);
                if (go != null)
                {
                    player = go.transform;
                }
            }

            GameObject occludeItem = RayCastCheckUtility.Instance.RasyCastCheckGameObject(transform.position, player.position);

            if (occludeItem == null)
            {
                if (meshRenders != null)
                {

                    foreach (var temp1 in meshRenders.materials)
                    {
                        if (temp1.HasProperty("_Opacity"))
                        {
                            temp1.SetFloat("_Opacity", initialValue);
                        }

                    }
                    meshRenders = null;
                }

            }
            else
            {
                MeshRenderer render = occludeItem.GetComponent<MeshRenderer>();

                if (render != null)
                {
                    if (meshRenders != null && meshRenders != render)
                    {
                        foreach (var temp1 in meshRenders.materials)
                        {
                            if (temp1.HasProperty("_Opacity"))
                            {
                                temp1.SetFloat("_Opacity", initialValue);
                            }

                        }
                    }


                    foreach (var temp in render.materials)
                    {
                        if (temp.HasProperty("_Opacity"))
                        {
                            temp.SetFloat("_Opacity", occluedValue);
                        }
                    }
                    meshRenders = render;




                }

            }
        }
    }

}
