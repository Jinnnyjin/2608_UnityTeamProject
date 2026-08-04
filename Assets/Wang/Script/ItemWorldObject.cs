using System;
using UnityEngine;


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
    public void Execute()
    {
        var itemData = m_itemData.ItemInfo;
        
        
        switch (itemData.Type)
        {
            case WorldItemType.EXP:
                {
                    
                }
                break;

        }

        onExecute?.Invoke(this);
    }

    public void Init(SOItemData _itemData, Action<ItemWorldObject> _callExecuteBack)
    {
        m_itemData = _itemData;
        onExecute = _callExecuteBack;
        m_ItemSpriteRenderer.sprite = m_itemData.Icon;
    }
}
