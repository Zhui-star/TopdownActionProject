
using HTLibrary.Framework;
using DG.Tweening;
namespace HTLibrary.Application
{
    /// <summary>
    /// 神明控制器
    /// </summary>
    public class GodsController : MonoBehaviourSimplify
    {
        private SkillReleaseTrigger _skillReleseTrigger;
        public bool InitialUseSkill = true;
        public int InitialSkillIndex = 2;
        private void Awake()
        {
            _skillReleseTrigger = GetComponent<SkillReleaseTrigger>();
        }

     
        private void OnEnable()
        {
            EventTypeManager.AddListener<int>(HTEventType.GodGolemController, TriggerSkill);

            if(InitialUseSkill)
            {
                TriggerSkill(InitialSkillIndex);
            }  
        }

        private void OnDisable()
        {
            EventTypeManager.RemoveListener<int>(HTEventType.GodGolemController, TriggerSkill);
        }

        /// <summary>
        /// 触发神明技能
        /// </summary>
        /// <param name="index"></param>
        protected virtual void TriggerSkill(int index)
        {
            _skillReleseTrigger.TriggerSkill(index);
        }

        protected override void OnBeforeDestroy()
        {
        }
    }

}
