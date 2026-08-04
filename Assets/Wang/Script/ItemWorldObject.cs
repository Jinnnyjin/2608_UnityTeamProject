using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


/*////////////////////////////////////////
 *            ItemWorldObject
 *기능 : SO(아이템 데이터)를 기반으로 아이템의 기능을 담당하는 스크립트
 *///////////////////////////////////////

[RequireComponent(typeof(PoolObject))]

public class ItemWorldObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_ItemSpriteRenderer;
    private SOItemData m_itemData;

    private Action<ItemWorldObject> onExecute;

    private ItemInfo m_itemInfo = new ItemInfo();
    public void Execute()
    {
        
        switch (m_itemData.ItemType)
        {
            case WorldItemType.EXP:
                {
                    
                }
                break;

        }

        onExecute?.Invoke(this);
    }

    public void Init(SOItemData _itemData, float _itemValue, Action<ItemWorldObject> _callExecuteBack)
    {
        m_itemData = _itemData;
        onExecute = _callExecuteBack;
        m_ItemSpriteRenderer.sprite = m_itemData.Icon;

        m_itemInfo.Value = _itemValue;
        m_itemInfo.Type = m_itemData.ItemType;
    }
}
