using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using HTLibrary.Framework;
namespace HTLibrary.Application
{
    /// <summary>
    /// 天赋系统UI
    /// </summary>
    public class TalentSystemPanel : BasePanel
    {
        public override void OnEnter()
        {
            base.OnEnter();
            transform.DOScale(1, .25f).OnComplete(()=>{HTPauseGame.Instance.PauseGame();}).SetUpdate(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            transform.DOScale(0, .25f).OnComplete(()=>{HTPauseGame.Instance.UnPauseGame();}).SetUpdate(true);
            
        }

        public void ClosePanelClick()
        {
            UIManager.Instance.PopPanel();
        }
    }

}
