
namespace HTLibrary.Utility
{
    /// <summary>
    /// 物品类型
    /// </summary>
  public enum ItemType
    {
        None,
        Consumable,
        Weapon,
        Armor,
        Shoes,
        Hat
    }

    /// <summary>
    /// 物品品阶
    /// </summary>
    public enum ItemQuality
    {
        Purple=5,
        Red=4,
        Green=3,
        Blue=2,
        White=1,
        None
    }

    public enum ToolTipType
    {
        None,
        ItemEffect
    }


    public enum WeaponType
    {
        None,
        Sworder,
        Archer,
        Magician,
        Shielder,
        Barserker
    }
}

