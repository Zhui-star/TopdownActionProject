using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using System;
namespace HTLibrary.Application
{
    [Serializable]
    public class CharacterUnit
    {
        public string characterName;

        public int id;

        public GameObject characterPrefab;

        public List<int> bindIDs;

        public List<int> skillIDs;

        public string _characterInfo;

        /// <summary>
        /// 是否包含可转职的职业
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsContainBindIds(int id)
        {
            return bindIDs.Contains(id);
        }
    }

}
