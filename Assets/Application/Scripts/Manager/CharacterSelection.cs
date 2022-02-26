using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;
using Cinemachine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using MoreMountains.FeedbacksForThirdParty;
using System;



namespace HTLibrary.Application
{
    public class CharacterSelection : MonoSingleton<CharacterSelection>
    {
        public CharacterConfigure configure;
        public int randomSelectCount = 2;//随机抽取可转职职业数量

        private Character newFollow;
        public int currentSelectIndex;
        private GameObject currentCharacter; //当前角色

        [Header("转职之后位置的初始化偏移")]
        public Vector3 InitialOffset;

        public Dictionary<int, CharacterUnit> characterUnit=new Dictionary<int, CharacterUnit>();

        public int MaxSelectHeroNumber = 2;

        [HideInInspector]
        //Recorder before transfer maxhealth 
        private int _frontCharacterMaxHealth;
        public int FrontCharacterMaxHealth{
            get{
                return _frontCharacterMaxHealth;
            }set{
                _frontCharacterMaxHealth=value;
            }
        }

        void Awake()
        {
            currentSelectIndex = 1;
            Initial();        
        }

        /// <summary>
        /// 初始化所有职业存入字典
        /// </summary>
        void Initial()
        {
             foreach(var temp in configure.characters)
            {
                if (characterUnit.ContainsKey(temp.id)) return;
                characterUnit.Add(temp.id, temp);
            }
        }

        /// <summary>
        /// 切换角色逻辑
        /// </summary>
        /// <param name="id"></param>
        public void ChangeCharacter(int id)
        {         
                SkillBoxManager.Instance.ImplementSkillBoxs(false);

                CinemachineVirtualCamera caremaFollow= FindObjectOfType<CinemachineVirtualCamera>();
                CharacterManager characterContorl  = CharacterManager.Instance;
                MMAutoFocus autoFocus = FindObjectOfType<MMAutoFocus>();

                currentCharacter = CharacterManager.Instance.GetCharacter("Player1").gameObject;
                CharacterManager.Instance.RemoverCharacter("Player1");

                Health currentHelath = currentCharacter.GetComponent<Health>();
                 _frontCharacterMaxHealth=currentHelath.MaximumHealth; //Recorder front character maxhealth value

                Vector3 newPosition =currentCharacter.transform.position+ 
                (currentCharacter.transform.forward * InitialOffset.z + currentCharacter.transform.right * InitialOffset.x +
                    currentCharacter.transform.up * InitialOffset.y);

                GameObject newCharacter = Instantiate(GetCharacterUnit(id).characterPrefab,
                 newPosition, currentCharacter.transform.rotation);

                newFollow = newCharacter.GetComponent<Character>();

                caremaFollow.Follow = newCharacter.transform;
                autoFocus.FocusTargets[0] = newCharacter.transform;
                characterContorl.AddCharater(newCharacter.GetComponent<Character>().PlayerID, newCharacter.GetComponent<Character>());
   
                currentSelectIndex = id;
                HTLevelManager.Instance.CurrentCharacterID = id;

                Destroy(currentCharacter);              
        }

        /// <summary>
        /// 返回可转职业的ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> GetBindIds(int id)
        {
            return characterUnit.TryGet<int, CharacterUnit>(id).bindIDs;
        }

        //从可转职业的ID中随机抽取2个id
        public List<int> GetRandomCharIds()
        {
            List<int> finalLst = new List<int>();
            List<int> curValdSelectIds = GetBindIds(HTLevelManager.Instance.CurrentCharacterID);
            System.Random rand = new System.Random();
            int getIdCount = curValdSelectIds.Count > randomSelectCount ? randomSelectCount : randomSelectCount / 2;
            int i = 0;
            while(i<getIdCount)
            {
                int randIdIndex = rand.Next(0, curValdSelectIds.Count);
                if(!finalLst.Contains(curValdSelectIds[randIdIndex]))
                {
                    finalLst.Add(curValdSelectIds[randIdIndex]);
                    i++;
                }
            }

            return finalLst;
        }

        /// <summary>
        /// 根据ID 返回职业信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CharacterUnit GetCharacterUnit(int id)
        {
            return characterUnit.TryGet<int, CharacterUnit>(id);
        }

        /// <summary>
        /// 返回当前角色拥有的技能
        /// </summary>
        /// <returns></returns>
        public List<int> ReturnCurrentSkill()
        {
            return GetCharacterUnit(currentSelectIndex).skillIDs;
        }
    }
}