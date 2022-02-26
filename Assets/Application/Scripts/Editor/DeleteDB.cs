using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HTLibrary.Application;
using HTLibrary.Framework;
using HTLibrary.Utility;

namespace HTLibrary.Editor
{
    public class DeleteDB : MonoBehaviour
    {
        [MenuItem("HTLibrary/DelelteDB",false,-10)]
        static void DeleteDataBase()
        {
            for(int i=1;i<4;i++)
            {
                PlayerPrefs.DeleteKey(Consts.Knapsack + i);

                PlayerPrefs.DeleteKey(Consts.EquipPanel + i);

                PlayerPrefs.DeleteKey(Consts.archerWeapon + i);

                PlayerPrefs.DeleteKey(Consts.shieldWeapon + i);

                PlayerPrefs.DeleteKey(Consts.sowrdWeapon + i);

                PlayerPrefs.DeleteKey(Consts.magicianWeapon + i);
            }
          

            PlayerPrefs.Save();


        }
    }

}
