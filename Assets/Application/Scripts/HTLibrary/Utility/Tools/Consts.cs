using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 存放一些常量
    /// </summary>
    public class Consts
    {
        public const string PoolParent = "PoolParent";

        public const string Knapsack = "Knapsack";

        public const string EquipPanel = "EquipPanel";

        //MVC
        public const string EnterScenesController = "EnterScensController";
        public const string StartUpGameController = "StartUpGameController";

        //音乐声音大小
        public const string Music = "Music";
        public const string Sfx = "Sfx";
        public const string VSycnCount = "VSycnCount";

        public const string GameLevel = "GameLevel";
        public const string GameExp = "GameExp";
        public const string GameCurrentHP = "GameCurrentHP";

        //初始给角色几个武器
        public const string magicianWeapon = "MagicianWeapon";
        public const string sowrdWeapon = "SwordWeapon";
        public const string archerWeapon = "ArcherWeapon";
        public const string shieldWeapon = "ShiledGuardWeapon";

        //天赋系统
        public const string LearnedTalent = "LearnedTalent";
        public const string GameStateUnits = "GameStateUnits";

        //相机切换场景保存视距
        public const string CameraDistance = "CameraDistance";

        //玩家数据
        public const string Name = "Name";
        public const string GameTime = "GameTime";
        public const string Coin = "Coin"; //Player's money

        //伙伴
        public const string LeagionKnightPassiveSkill = "LeagionKinghtsPassiveSkill";

        public const string PatnerData = "PatnerData";

        //Dialog
        public const string DialogData="DialogData";
    }

    /// <summary>
    /// 行动条回合状态
    /// </summary>
    public enum ActionBarRoundState
    {
        Computer,
        Player,
        Enemy
    }

    /// <summary>
    /// 队伍角色标签
    /// </summary>
    public enum CharacterType_Round
    {
        Player_1,
        Player_2
    }


    /// <summary>
    /// UI Panel类型
    /// </summary>
    public enum UIPanelType
    {
        None,
        GameMenuePanel,
        CharacterSelectPanel,
        HTSkillSelectPanel,
        SettingPanel,
        WeaponSwitchTipsPanel,
        GSEquipPanel,
        InventoryMenuePanel,
        TalentSystemPanel,
        StoreMenuePanel,
        GameOverPanel,
        GamePausePanel,
        TabStatePanel,
        VictoryPanel

    }

    /// <summary>
    /// 游玩状态
    /// </summary>
    public enum RPGPlayerState
    {
        Idle,
        Move,
        Attack,
        Death,
        TakeDamage
    }

    public enum SkillBoxType
    {
        Dodge,
        Skill1,
        Skill2
    }

    /// <summary>
    /// 武器蓄力效果
    /// </summary>
    public enum WeaponAccumlateResult
    {
        None,
        MultipleScale

    }

}
