using UnityEngine;
namespace HTLibrary.Application
{
    public class SpawnCharacter : MonoBehaviour
    {
        public GameObject SpawnGo;
        public bool randomPosition=true;
        [HideInInspector]
        public int CorresPondingSkillID{get;set;}
        public void SpawningCharacter()
        {
            GameObject character = GameObject.Instantiate(SpawnGo);
            Vector3 _targetPosition = this.transform.position;

            if(randomPosition)
            {
                _targetPosition = new Vector3(_targetPosition.x + Random.Range(-5, 5), _targetPosition.y, _targetPosition.z + Random.Range(0, 5));
            }

            character.transform.position = _targetPosition;
            character.transform.rotation = this.transform.rotation;

            PatnerController patnerController = character.GetComponent<PatnerController>();
            if(patnerController!=null)
            {
                patnerController.ActivatePatnerController();
                patnerController.CorresPondingSkillID=this.CorresPondingSkillID;
            }

        }
    }

}
