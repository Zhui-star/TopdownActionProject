using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    public class EnterSceneController :HTLibrary.Framework. Controller
    {
        public override void Execute(object data)
        {
            SceneArgs e = data as SceneArgs;
            switch(e.scenesIndex)
            {             
                case 0:
          
                    HTDBManager.Instance.SaveKnapsack();
                    TalentSystemManager.Instance.SaveLearnedTalents();
                    SaveManager.Instance.SaveGameTime();
                    SaveManager.Instance.SaveDialogPersistantData();
                    
                    //Test 重置数据
                    LevelUnitManager.Instance.CurrentLevelIndex = 1;
                    HTLevelManager.Instance.ResetData();
                    break;
                case 6:
                  //TODO 测试打包 HTLevelManager.Instance.ResetData();
                    break;
            }
        }
    }


}
