using HTLibrary.Framework;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// 奖励面板
    /// </summary>
    public class PrizePanel : MonoBehaviour
    {
        public PrizeUnitUI _prizeUnitUI;
        private HTDBManager _htDBManager;
        private InventoryManager _inventoryManager;

        private void Awake()
        {
            _htDBManager = HTDBManager.Instance;
            _inventoryManager = InventoryManager.Instance;
        }

        private void Start()
        {
            UpdatePanel();
        }

        /// <summary>
        /// 更新Panel
        /// </summary>
        void UpdatePanel()
        {
            //Get acuqir monemy and show the item UI
            uint acuireqMoeny=HTLevelManager.Instance.GetAcquiredMoney((uint)HTDBManager.Instance.GetCoins());

            Debugs.LogInformation("Acuire money:" + acuireqMoeny, Color.gray);

            if(acuireqMoeny>0)
            {
               PrizeUnitUI moneyUI=GameObject.Instantiate(_prizeUnitUI,this.transform);
               moneyUI.UpdateSpritialSourceUI(acuireqMoeny);
            }
       
            List<int> itemIDs = _htDBManager.GetPickUpEquipList();

            foreach(var itemID in itemIDs)
            {
               PrizeUnitUI unitUI = GameObject.Instantiate(_prizeUnitUI, this.transform);
               Item item = _inventoryManager.GetItemById(itemID);
               unitUI.UpdateEquipUI(item);
            }
           
        }
    }

}
