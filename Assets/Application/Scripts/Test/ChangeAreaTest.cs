using System.Collections;
using UnityEngine;
using HTLibrary.Application;
using HTLibrary.Utility;
using NaughtyAttributes;
namespace HTLibrary.Test
{
    public class ChangeAreaTest : MonoBehaviour
    {
        bool canChange = true;

        [ReorderableList]
        public int[] _characterIDs;
        private int _index;

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == Tags.Player&&canChange)
            {
                ++_index;
                _index = _index % _characterIDs.Length;
                SkillBoxManager.Instance.ImplementSkillBoxs(false);
                CharacterSelection.Instance.ChangeCharacter(_characterIDs[_index]);
                SkillBoxManager.Instance.ImplementSkillBoxs(true);

                canChange = false;           
                StartCoroutine(IChangeCharacter());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == Tags.Player)
            {
                canChange = true;
            }
        }

        IEnumerator IChangeCharacter()
        {
            yield return new WaitForSeconds(5.0f);
            canChange = true;
        }

       
    }
}