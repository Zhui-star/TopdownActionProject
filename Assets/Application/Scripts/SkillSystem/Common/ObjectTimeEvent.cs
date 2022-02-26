using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using HTLibrary.Framework;
public class ObjectTimeEvent : MonoBehaviour
{
    public float Time = 0.9f;

    [Header("物体一定时间后是否会孵化新的物体")]
    public string objectPoolName;
    public Vector3 spawnerOffset;

    [Header("物体一定时间后反馈")]
    public MMFeedbacks spawnFeedbacks;

    private void Awake()
    {
        spawnFeedbacks?.Initialization(this.gameObject);
    }

    private void OnEnable()
    {
        Invoke("SpawnObject", Time);
        Invoke("PlayFeedBacks", Time);
    }

    /// <summary>
    /// 隐藏物体之后孵化出新的物体
    /// </summary>
    void SpawnObject()
    {
        if (!string.IsNullOrEmpty(objectPoolName))
        {
            GameObject _object = PoolManagerV2.Instance.GetInst(objectPoolName);
            _object.transform.position = this.transform.position+transform.forward*spawnerOffset.z+transform.up*spawnerOffset.y+transform.right*spawnerOffset.x;
            _object.transform.rotation = this.transform.rotation;
         
        }
    }

    /// <summary>
    /// 反馈激活
    /// </summary>
    void PlayFeedBacks()
    {

        spawnFeedbacks?.PlayFeedbacks();
    }

    /// <summary>
    /// 停止计时操作
    /// </summary>
    public void StopInvoke()
    {
        CancelInvoke("SpawnObject");
        CancelInvoke("PlayFeedBacks");
    }

}
