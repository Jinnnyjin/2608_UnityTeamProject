using System;
using UnityEngine;


/*///////////////////////////////////////
 *              SOITemData
 *기능 : 아이템의 기능을 담당하는 것이 아닌, 아이템의 시각적 데이터 , 아이템 별 데이터를 관리
 *///////////////////////////////////////

public enum WorldItemType
{
    EXP,
    END,
}

/*
 가장 기본이 되는 아이템 능력치
 최종 적용 능력치 = 기본 능력치 + 아이템 동적 능력치
 */
//[Serializable]
public class ItemInfo
{
    public WorldItemType Type;
    public float Value;
}

public class SOItemData : SOData
{
    [SerializeField] private Sprite m_icon;
    public Sprite Icon => m_icon;

    [SerializeField] private WorldItemType m_itemType;
    public WorldItemType ItemType => m_itemType;
    //[SerializeField] private ItemInfo m_baseItemInfo;
    //public ItemInfo BaseItemInfo => m_baseItemInfo;

}
