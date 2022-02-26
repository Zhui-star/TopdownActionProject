using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;

namespace HTLibrary.Application
{
    public class ShieldLaunchControl : MonoBehaviour
    {
        [Header("移动速度")]
        public float moveSpeed; //移动速度
        [Header("弹射速度")]
        public float LaunchSpeed; //弹射速度
        [Header("射线检测范围")]
        public int radius; //射线检测范围
        [Header("技能消失时间（需与AutoHide的time保持一致）")]
        public int clearTime; //Autohide的time会变成0.5
        private bool isTrigger; //是否碰撞检测到
        private bool isCount; //是否开始存储位置
        private bool isStartClear; //是否开始清理存储位置
        private Rigidbody _rigidBody;
        Vector3 _movement;

        private int index; //用于计数
        private float timer;

        private Collider[] colliders;
        public List<Transform> currentTrans;


        private void OnEnable()
        {
            isTrigger = false;
            isCount = false;
            isStartClear = false;
            index = 0;
            timer = 0;
            Debug.Log("Start Killing!");
            ClearCurrentTrans();
        }

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            
        }

        // Update is called once per frame
        void Update()
        {
            if (isTrigger == false)
            {
                _movement = transform.forward * (moveSpeed / 10) * Time.deltaTime;

                if (_rigidBody != null)
                {
                    _rigidBody.MovePosition(this.transform.position + _movement);
                }
                
            }

            if(isTrigger==true&&index< currentTrans.Count)
            {
                if (currentTrans[index].gameObject.activeSelf == false)
                {
                    currentTrans.Remove(currentTrans[index]);
                    index++;
                    //Debug.Log("Current gameObject is false, Remove!!");
                    if (currentTrans.Count<=1)
                        this.gameObject.SetActive(false);
                }
                else
                {
                    this.gameObject.transform.LookAt(currentTrans[index].position);
                    this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, currentTrans[index].position, LaunchSpeed * Time.deltaTime);
                    if (this.gameObject.transform.position == currentTrans[index].position)
                    {
                        index++;
                        //Debug.Log("Current index is" + index);
                    }
                }

                if (index > currentTrans.Count - 1)
                {
                    index = 0;
                }
                timer += Time.deltaTime;
            }
            
            
            if (timer >= clearTime)
            {
                ClearCurrentTrans();
                timer = 0;
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isTrigger == false)
            {      
                if (other.tag == "Enemy")
                {
                    colliders = Physics.OverlapSphere(this.transform.position, radius);

                    if (colliders.Length > 0 && isCount==false)
                    {
                        isCount = true;
                        string currentName = colliders[0].gameObject.name;

                        for (int i = 0; i < colliders.Length; i++)
                        {
                            if (colliders[i].tag == "Enemy")
                            {
                                if(i==0)
                                {
                                    currentTrans.Add(colliders[i].gameObject.transform);
                                }
                                else if (currentName != colliders[i].gameObject.name)
                                {
                                    currentName = colliders[i].gameObject.name;
                                    currentTrans.Add(colliders[i].gameObject.transform);
                                    Debug.Log(colliders[i].gameObject.name);
                                }
                            }
                        }
                        Debug.Log("Current Count is" + currentTrans.Count);
                        //foreach (Transform name in currentTrans)
                        //    Debug.Log("Final Name is " + name);
                        if(currentTrans.Count>1)
                            isTrigger = true;
                    }
                }
                
            }
            
        }

        private void OnTriggerStay(Collider other)
        {
            if (isStartClear==false&&currentTrans.Count < 2)
            {
                Debug.Log("Start Clear!!");
                ClearCurrentTrans();
                isStartClear = true;
            }
        }

        private void ClearCurrentTrans()
        {
            currentTrans.Clear();
            index = 0;
            Debug.Log("After clear is" + currentTrans.Count);
        }
       
    }

}


