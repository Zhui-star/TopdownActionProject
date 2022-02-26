using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    /// <summary>
    /// 伙伴管理器
    /// </summary>
    public class PatnerDataManager : MonoSingleton<PatnerDataManager>
    {
        public PatnerDataConfigure patnerDataTable;
        Dictionary<int, PatnerUnit> patnerDicts = new Dictionary<int, PatnerUnit>();
        Dictionary<string, int> _patnerHealth = new Dictionary<string, int>();
        List<int> followPatner = new List<int>();

        private void Awake()
        {
            InitialData();
        }

        private void Start()
        {

        }

        /// <summary>
        /// 初始化数据表
        /// </summary>
        void InitialData()
        {
            List<PatnerUnit> patnerDataList = patnerDataTable.patnerDatabase;
            foreach (var temp in patnerDataList)
            {
                if (patnerDicts.ContainsKey(temp.ID)) continue;
                patnerDicts.Add(temp.ID, temp);
            }
        }

        /// <summary>
        /// 添加随行伙伴
        /// </summary>
        /// <param name="id"></param>
        public void AddFollowPatner(int id)
        {
            followPatner.Add(id);
        }

        /// <summary>
        /// 得到所有随行伙伴
        /// </summary>
        /// <returns></returns>
        public List<GameObject> GetFollowPatners()
        {
            List<GameObject> patnerGoList = new List<GameObject>();
            foreach (var temp in followPatner)
            {
                patnerGoList.Add(GetPartnerById(temp));
            }
            return patnerGoList;
        }


        /// <summary>
        /// 移除随行伙伴
        /// </summary>
        /// <param name="id"></param>
        public void RemoveFollowPatner(int id)
        {
            followPatner.Remove(id);
        }

        /// <summary>
        /// 通过ID得到单个伙伴
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private GameObject GetPartnerById(int id)
        {
            GameObject patnerGo = patnerDicts.TryGet<int,PatnerUnit>(id)._GameObject;

            if (patnerGo != null)
            {
                return patnerGo;
            }

            return null;
        }

        /// <summary>
        /// 检查是否是随行伙伴
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckFollowPatner(int id)
        {
            return followPatner.Contains(id);
        }


        /// <summary>
        /// 清空随行伙伴
        /// </summary>
        public void ClearFollowPatnerList()
        {
            followPatner.Clear();
            ClearPatnerHealth();
        }



        /// <summary>
        /// 通过ID 得到伙伴单位数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PatnerUnit GetPatnerById(int id)
        {
            return patnerDicts.TryGet<int, PatnerUnit>(id);
        }

        /// <summary>
        /// 得到全部伙伴ID
        /// </summary>
        /// <returns></returns>
        public List<int> GetFollowPatnerIds()
        {
            return followPatner;
        }

        /// <summary>
        /// 添加伙伴血量存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddPatnerHealthDicts(string key, int value)
        {
            if(!_patnerHealth.ContainsKey(key))
            {
                _patnerHealth.Add(key, value);

            }
            else
            {
                _patnerHealth[key] = value;
            }
        }

        /// <summary>
        /// 移除特定伙伴血量存储
        /// </summary>
        /// <param name="key"></param>
        public void RemovePatnerHealthDicts(string key)
        {
            if(_patnerHealth.ContainsKey(key))
            {
                _patnerHealth.Remove(key);
            }
        }

        /// <summary>
        /// 清除伙伴血量数据
        /// </summary>
        public void ClearPatnerHealth()
        {
            _patnerHealth.Clear();
        }

        /// <summary>
        /// 得到伙伴血量通过Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public  int GetPatnerHealthByKey(string key)
        {
            if (_patnerHealth.ContainsKey(key))
            {
                return _patnerHealth.TryGet<string, int>(key);
            }

            return int.MaxValue;
        }

        /// <summary>
        /// 是否包含血量数据 (key)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool CheckPatnerHealthKey(string key)
        {
            return _patnerHealth.ContainsKey(key);
        }

    }

}
