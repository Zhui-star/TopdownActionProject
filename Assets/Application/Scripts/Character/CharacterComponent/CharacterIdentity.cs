using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    public class CharacterIdentity : MonoBehaviour
    {
        public WeaponType heroType;
        public CharacterConfig characterConfigure;

        private float attack;
        private float critRank;
        private float defence;
        private float hp;
        private float dodge;
        private float moveSpeed;
        private float attackSpeed;
        private void Awake()
        {
            foreach (var temp in HTDBManager.Instance.equip)
            {
                AddEquipEvent(temp);
            }
        }

        private void OnEnable()
        {
            HTDBManager.Instance.AddEquipEvent += AddEquipEvent;
            HTDBManager.Instance.RemoveEquipEvent += RemoveEquipEvent;
        }

        private void OnDisable()
        {
            if (HTDBManager.Instance == null) return;
            HTDBManager.Instance.AddEquipEvent -= AddEquipEvent;
            HTDBManager.Instance.RemoveEquipEvent -= RemoveEquipEvent;
        }

        private void OnDestroy()
        {
            ResetData();
        }


        public void AddEquipEvent(int id)
        {
            Item item= InventoryManager.Instance.GetItemById(id);
            characterConfigure.additiveAttack += item.Attack;
            characterConfigure.additiveCritRank += item.Crit;
            characterConfigure.additiveDefence += item.Defence;
            characterConfigure.additiveHP += item.hp;
            characterConfigure.additiveDodge += item.Dodge;
            characterConfigure.additiveMoveSpeed += item.MoveSpeed;
            characterConfigure.characterAddtiveAttackSpeed += item.AttackSpeed;

            attack += item.Attack;
            critRank += item.Crit;
            defence += item.Defence;
            hp += item.hp;
            dodge += item.Dodge;
            moveSpeed += item.MoveSpeed;
            attackSpeed += item.AttackSpeed;
        }

        public void RemoveEquipEvent(int id)
        {
            Item item = InventoryManager.Instance.GetItemById(id);
            characterConfigure.additiveAttack -= item.Attack;
            characterConfigure.additiveCritRank -= item.Crit;
            characterConfigure.additiveDefence -= item.Defence;
            characterConfigure.additiveHP -= item.hp;
            characterConfigure.additiveDodge -= item.Dodge;
            characterConfigure.additiveMoveSpeed -= item.MoveSpeed;
            characterConfigure.characterAddtiveAttackSpeed -= item.AttackSpeed;

            attack -= item.Attack;
            critRank -= item.Crit;
            defence -= item.Defence;
            hp -= item.hp;
            dodge -= item.Dodge;
            moveSpeed -= item.MoveSpeed;
            attackSpeed -= item.AttackSpeed;
        }

        void ResetData()
        {
            characterConfigure.additiveAttack -= attack;
            characterConfigure.additiveCritRank -= critRank;
            characterConfigure.additiveDefence -= defence;
            characterConfigure.additiveHP -= hp;
            characterConfigure.additiveDodge -= dodge;
            characterConfigure.additiveMoveSpeed -= moveSpeed;
            characterConfigure.characterAddtiveAttackSpeed -= attackSpeed;
        }
    }
}

