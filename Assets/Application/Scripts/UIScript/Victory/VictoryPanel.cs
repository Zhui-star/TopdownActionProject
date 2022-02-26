using HTLibrary.Framework;
using UnityEngine;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// Vicotry panel 
    /// </summary>
    public class VictoryPanel : BasePanel
    {
        [Header("Animation")]
        public float _animTime=1;
        float _animTimer;
        public override void OnEnter()
        {
            base.OnEnter();
            transform.localScale = Vector3.one;
            HTPauseGame.Instance.PauseGame();
            _animTimer=0;
        }

        public override void OnExit()
        {
            base.OnExit();
            HTPauseGame.Instance.UnPauseGame();
            transform.localScale = Vector3.zero;
        }


        private void Update()
        {
            _animTimer+=Time.unscaledTime;
            
            if (MoreMountains.TopDownEngine.InputManager.Instance.InteractButton.State.CurrentState
            ==MoreMountains.Tools.MMInput.ButtonStates.ButtonDown
            &&_animTimer>_animTime)
            {
                ProcessAnyButtonDownClick();
            }
        }

        /// <summary>
        /// Process any key down click
        /// </summary>
        void ProcessAnyButtonDownClick()
        {
            //TODO Enter to the next level
            LevelUnitManager.Instance.LoadMenueScenes();
            UIManager.Instance.ClearStackPanel();
        }
    }

}
