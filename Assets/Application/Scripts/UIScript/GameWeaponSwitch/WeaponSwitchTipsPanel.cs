using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
namespace HTLibrary.Application
{
    /// <summary>
    /// 武器切换面板
    /// </summary>
    public class WeaponSwitchTipsPanel : BasePanel
    {
        private CharacterIdentity characterIdentity;
        public AudioClip EquipSound;

        bool _selfSwitch=false;
        public override void OnEnter()
        {
            base.OnEnter();
            _selfSwitch=false;


            transform.DOScale(1, 0.25f).SetUpdate(true);

            if (CharacterManager.Instance != null)
            {
                characterIdentity = CharacterManager.Instance.GetCharacter("Player1").GetComponent<CharacterIdentity>();
            }

        }

        public override void OnExit()
        {
            base.OnExit();

            transform.DOScale(0, 0.25f).OnComplete(()=>
            {
                if(!_selfSwitch)
                {
                    HTPauseGame.Instance.UnPauseGame();
                }
              
            }).SetUpdate(true);

            if (EquipSound != null)
            {
                SoundManager.Instance.PlaySound(EquipSound, transform.localPosition, false);
            }


        }

        public void OnSwitchClick()
        {
            if (HTDBManager.Instance.ReturnCurrentWeapon() != null)
            {
                HTDBManager.Instance.RemoveEquip(HTDBManager.Instance.ReturnCurrentWeapon().itemID);
            }

            List<int> weaponList = HTDBManager.Instance.SelectHeroWeapon(characterIdentity.heroType);
            int bestId = HTDBManager.Instance.SelectBestWepaon(weaponList);
            HTDBManager.Instance.AddEquip(bestId);
            UIManager.Instance.PopPanel();

        }

        public void OnSelfSwitchClick()
        {
            _selfSwitch=true;
            UIManager.Instance.PopPanel();
            UIManager.Instance.PushPanel(UIPanelType.GSEquipPanel);
        }
    }

}
