using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using DG.Tweening;
using HTLibrary.Utility;
using System;
namespace HTLibrary.Application
{
    /// <summary>
    /// 背包操作面板
    /// </summary>
    public class InventoryMenuePanel : BasePanel
    {
        public GameObject ForgePanelGo;
        public GameObject EquipPanelGo;

        public override void OnEnter()
        {
            base.OnEnter();
            transform.DOScale(1, 0.25f).OnComplete(()=>{
                HTPauseGame.Instance.PauseGame();
            }).SetUpdate(true);

            ForgePanelGo.SetActive(InventoryManager.Instance.invetoryType == InventoryType.Forge);

            Knapsack.Instance.OnEnterCallBack();
        }

        public override void OnExit()
        {
            base.OnExit();
            HTPauseGame.Instance.UnPauseGame();
            transform.DOScale(0, 0.25f).SetUpdate(true);

            switch(InventoryManager.Instance.invetoryType)
            {
                case InventoryType.Forge:
                    ForgePanel.Instance.ClearExistItemWhenClosePanel();
                    break;
            }

            Knapsack.Instance.OnExitCallBack();
        }

    }

}
