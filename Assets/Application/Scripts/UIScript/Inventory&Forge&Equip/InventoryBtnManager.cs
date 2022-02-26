using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace HTLibrary.Utility
{
    public class InventoryBtnManager : MonoBehaviour
    {
        private static InventoryBtnManager _instance;
        private List<Button> btnGroup = new List<Button>();

        public event Action<bool,ItemType> InventoryButtonClickEvent;
        public event Action<bool,string> BtnClickAnimChangeEvent;

        [HideInInspector]
        public ItemType currentBtnType = ItemType.Weapon;

        //单例模式
        public static InventoryBtnManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<InventoryBtnManager>();
                }
                return _instance;
            }
        }

        void Awake()
        {
            btnGroup.Add(transform.Find("Weapon_Btn").GetComponent<Button>());
            btnGroup.Add(transform.Find("Hat_Btn").GetComponent<Button>());
            btnGroup.Add(transform.Find("Armor_Btn").GetComponent<Button>());
            btnGroup.Add(transform.Find("Shoes_Btn").GetComponent<Button>());
            foreach(Button btn in btnGroup)
            {
                btn.onClick.AddListener(delegate {
                    switch(btn.name){
                        case "Weapon_Btn":
                            InventoryButtonOnClick(btn,ItemType.Weapon);
                            break;
                        case "Hat_Btn":
                            InventoryButtonOnClick(btn,ItemType.Hat);
                            break;
                        case "Armor_Btn":
                            InventoryButtonOnClick(btn, ItemType.Armor);
                            break;
                        case "Shoes_Btn":
                            InventoryButtonOnClick(btn, ItemType.Shoes);
                            break;
                    }
                });
            }
        }

        private void InventoryButtonOnClick(Button btn, ItemType itemType)
        {
            Animator btnAnim = btn.GetComponent<Animator>();
            btnAnim.SetBool("isChose", true);
            BtnClickAnimChangeEvent?.Invoke(false,btn.name);
            currentBtnType = itemType;
            InventoryButtonClickEvent?.Invoke(true, itemType);
        }
    }
}
