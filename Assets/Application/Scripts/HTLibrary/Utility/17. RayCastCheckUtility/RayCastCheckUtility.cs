using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 射线检测工具
    /// 1. 检查2个位置是否有其他障碍物
    /// </summary>
    public class RayCastCheckUtility : Singleton<RayCastCheckUtility>
    {
        public GameObject RasyCastCheckGameObject(Vector3 position_1,Vector3 position_2)
        {
            RaycastHit hitinfo;

            Debug.DrawLine(position_1, position_2, Color.red);
          /*  if (Physics.Raycast(position_1,position_1- position_2, out hitinfo))
            {
                if (hitinfo.collider.tag != Tags.Player&&hitinfo.collider.tag!=Tags.Enemies)
                {
                    return hitinfo.collider.gameObject;
                }
         
            }*/

            if(Physics.Linecast(position_1,position_2,out hitinfo))
            {
                if (hitinfo.collider.tag != Tags.Player && hitinfo.collider.tag != Tags.Enemies)
                {
                    return hitinfo.collider.gameObject;
                }
            }

            return null;
        }

     

        public string RayCastCheckFromMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if(Physics.Raycast(ray,out hitInfo))
            {
                return hitInfo.collider.tag;
            }

            return "";
        }

        public Vector3 RayCastPoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            
            bool isCollider = Physics.Raycast(ray, out hitInfo,1000,LayerMask.GetMask("Ground"));

           if(isCollider)
            {
                return hitInfo.point;
            }
            return Vector3.zero;
            

        }

        public bool RasyCastDirection(Vector3 position,Vector3 direction,float maxDistance)
        {

            return Physics.Raycast(position, direction, maxDistance);
           
        }

       public bool RayCastDirection(Vector3 position, Vector3 direction,float maxDistance,string layerMaskName)
        {
            Ray ray = new Ray(position, direction);
            Debug.DrawLine(position,position+ direction,Color.red);

            return Physics.Raycast(ray, maxDistance, LayerMask.NameToLayer(layerMaskName));
        }

        /// <summary>
        /// 返回射线检测的游戏物体
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <param name="maxDistance"></param>
        /// <returns></returns>
        public GameObject RayCastDirection(Vector3 position,Vector3 direction,float maxDistance)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit raycasthit;
           if(Physics.Raycast(ray, out raycasthit, maxDistance))
            {
                return raycasthit.collider.gameObject;
            }

            return null;
        }

        /// <summary>
        /// 返回射线检测 结果hit
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <param name="maxDistance"></param>
        /// <returns></returns>
        public RaycastHit RayCastDiretion(Vector3 position,Vector3 direction,float maxDistance)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit hit;
            Physics.Raycast(ray,out hit, maxDistance);
            return hit;
        }

        /// <summary>
        /// 返回Box射线检测
        /// </summary>
        /// <param name="center"></param>
        /// <param name="extensionHalf"></param>
        /// <param name="direction"></param>
        /// <param name="maxDistance"></param>
        /// <returns></returns>

        public RaycastHit BoxCast(Vector3 center,Vector3 extensionHalf,Vector3 direction,float maxDistance)
        {
            RaycastHit raycastHit;

           if(Physics.BoxCast(center, extensionHalf, direction,out raycastHit, Quaternion.LookRotation(direction),maxDistance))
            {
;
            }

            return raycastHit;
        }
   
    }

}
