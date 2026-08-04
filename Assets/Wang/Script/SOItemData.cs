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

[Serializable]
public struct ItemInfo
{
    public WorldItemType Type;
    public float Value;
}
public class SOItemData : SOData
{
    [SerializeField] private Sprite m_icon;
    public Sprite Icon => m_icon;

    [SerializeField] private ItemInfo m_itemInfo;
    public ItemInfo ItemInfo => m_itemInfo;

}
