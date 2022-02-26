using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 作用类型
    /// </summary>
    public enum SkillType
    {
        None,
        Single,
        Multiple
    }

    public enum WhileAttack
    {
        MeleeFwd,
        Immobile,
        FreeWalk
    } 

    public enum SkillSpawn
    {
        FromPlayer,
        AtMouse,
        SlefSpawn
    }

    public enum ChangePosition
    {
        None,
        Dash
    }

    /// <summary>
    /// 作用属性
    /// </summary>
    public enum SkillProperty
    {
        None,
        Attack
    }

    /// <summary>
    /// 释放类型
    /// </summary>
    public enum ReleaseType
    {
        None,
        Enemy,
        Position
    }

}
