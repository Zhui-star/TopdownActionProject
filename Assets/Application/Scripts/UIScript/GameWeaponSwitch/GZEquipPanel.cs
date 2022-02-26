using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using HTLibrary.Framework;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    public class GZEquipPanel : BasePanel
    {
        private CanvasGroup canvasGroup;
        public MenueWeaponSlot menueSlot;
        public Transform parent;
        private CharacterIdentity identity;
        public override void OnEnter()
        {
            base.OnEnter();
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }

            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            transform.DOScale(1, 0.25f).OnComplete(() =>
            {
                
            }).SetUpdate(true);

            Knapsack.Instance.LoadInventory();
        }

        public override void OnExit()
        {
            base.OnExit();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            transform.DOScale(0, 0.25f).OnComplete(()=>{HTPauseGame.Instance.UnPauseGame();}).SetUpdate(true);

        }

        void InitialWeapon()
        {
            identity = CharacterManager.Instance.GetCharacter("Player1").GetComponent<CharacterIdentity>();
            List<int> weaponList = HTDBManager.Instance.SelectHeroWeapon(identity.heroType);

            foreach (var temp in weaponList)
            {
                Item item = InventoryManager.Instance.GetItemById(temp);
                GameObject slotGo = GameObject.Instantiate(menueSlot.gameObject, parent);
                MenueWeaponSlot slot = slotGo.GetComponent<MenueWeaponSlot>();
                slot.item = item;
                slot.UpdateUI();
            }
        }
    }

}
