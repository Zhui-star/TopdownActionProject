using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 技能孵化器
    /// </summary>
    public class SkillSpawner :MonoSingleton<SkillSpawner>
    {
        public List<SkillSlot> skillSlotList = new List<SkillSlot>();

        public GameObject skillSlot;

        public Transform slotParent;

        public bool StoreSkillUnit(int id)
        {
          SkillUnit skillUnit=  SkillSystemManager.Instance.GetSkillById(id);
           return StoreSkillUnit(skillUnit);
        }

        public bool StoreSkillUnit(SkillUnit skillUnit)
        {
            GameObject slotGo = GameObject.Instantiate(skillSlot, slotParent);
            SkillSlot slot = slotGo.GetComponent<SkillSlot>();
            if (slot!=null)
            {
                slot.StoreSkillUnit(skillUnit);
                skillSlotList.Add(slot);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ClearSkillSpawner()
        {
            if (skillSlotList.Count <= 0) return;
            skillSlotList.Clear();
            for(int i=0;i<slotParent.childCount;i++)
            {
                Destroy(slotParent.GetChild(i).gameObject);
            }
        }

        private void OnDisable()
        {
            ClearSkillSpawner();
        }
    }

}
