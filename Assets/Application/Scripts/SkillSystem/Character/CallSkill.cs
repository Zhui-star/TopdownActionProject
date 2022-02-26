using UnityEngine;
namespace HTLibrary.Application
{
    public class CallSkill : MonoBehaviour
    {
        public SpawnCharacter spawn;
        public float time;

        private int _corrsPondingSKillID;
        public int CorresPondingSkillID
        {
            get
            {
                return _corrsPondingSKillID;
            }
            set
            {
                spawn.CorresPondingSkillID = value;

                Debugs.LogInformation("Spawn skill corres ponding skill index from skill slot:"+value,Color.blue);
                
                _corrsPondingSKillID = value;
            }
        }

        private void OnEnable()
        {
            Invoke("SpawnCall", time);
        }

        void SpawnCall()
        {
            if (spawn != null)
                spawn.SpawningCharacter();
        }
    }

}
