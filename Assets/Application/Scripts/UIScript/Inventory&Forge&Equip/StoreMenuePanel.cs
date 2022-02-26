using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using DG.Tweening;
namespace HTLibrary.Application
{
    /// <summary>
    /// 商品Panel UI
    /// </summary>
    public class StoreMenuePanel : BasePanel
    {
        public override void OnEnter()
        {
            base.OnEnter();
            transform.DOScale(1, 0.25f).OnComplete(()=>{HTPauseGame.Instance.PauseGame();}).SetUpdate(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            transform.DOScale(0, 0.25f).OnComplete(()=>{HTPauseGame.Instance.UnPauseGame();}).SetUpdate(true);
        }

        public void CloseClick()
        {
            UIManager.Instance.PopPanel();
        }
    }

}
