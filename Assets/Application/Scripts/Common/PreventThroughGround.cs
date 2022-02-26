using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// 保护角色穿越地面
    /// </summary>
    public class PreventThroughGround : MonoBehaviour
    {
        [HideInInspector]
        public CharacterController characterController;
        //public TopDownController3D controller;
        CharacterDash3D dash;
        public LayerMask layerMask;
        public float distance = 0.2f;
        //public float stickOffset = 0.03f;
        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            //controller = GetComponent<TopDownController3D>();
            dash = GetComponent<CharacterDash3D>();
        }
        private void FixedUpdate()
        {
            Debug.DrawRay(transform.position + characterController.center * transform.localScale.y, Vector2.down * distance, Color.blue);

            RaycastHit hit;
            if(Physics.Raycast(transform.position + characterController.center * transform.localScale.y, Vector2.down,out hit, distance))
            {
                if(MMLayers.LayerInLayerMask(hit.collider.gameObject.layer,layerMask))
                {
                    dash.DashStop();
                }
            }
        }
    }

}
