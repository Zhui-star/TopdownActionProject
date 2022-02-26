using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using MoreMountains.Feedbacks;
namespace HTLibrary.Application
{

    /// <summary>
    /// 角色掉入凹陷地形
    /// </summary>
    public class CharacterFallDownHole3D : CharacterAbility
    {

        /// <summary>
        /// 检测凹陷地形
        /// </summary>
        protected virtual void CheckForHole()
        {
            if(_controller3D.OverHole&&!_controller3D.Grounded)
            {
                if(_movement.CurrentState != CharacterStates.MovementStates.Dashing&&_condition.CurrentState!=CharacterStates.CharacterConditions.Dead)
                {
                    _movement.ChangeState(CharacterStates.MovementStates.FallingDownHole);

                    _controller3D.ResetToLastGroundNormal();

                    _health.Damage(20, this.gameObject, 0.5f, 0.5f, null,false);

                    PlayAbilityStartFeedbacks();
                }
            }
        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();
            CheckForHole();

        }


    }

}
