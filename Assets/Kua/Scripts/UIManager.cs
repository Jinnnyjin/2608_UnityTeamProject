using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    //public Slider HpSlider;
    //public Slider XpSlider;
    public TextMeshProUGUI TimerText;

    [SerializeField] private List<SkillSlot> m_listSlot;

   
    private float m_gameTime = 0f;
   

    public static UIManager m_Instance { get; private set; }

    private void Awake()
    {
        m_Instance = this;
    }

    public void Update()
    {
        m_gameTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(m_gameTime / 60F);
        int seconds = Mathf.FloorToInt(m_gameTime % 60F);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    //public void UpdateHP(float _currentItem, float _maxItem)
    //{
    //    if (HpSlider != null) HpSlider.value = _currentItem / _maxItem;
    //}
    //
    //public void UpdateXP(float _currentItem, float _maxItem)
    //{
    //    if (XpSlider != null) XpSlider.value = _currentItem / _maxItem;
    //}

    // 아이템을 먹었을 때 호출될 함수 (매개변수 _ 규칙 준수)
    //public void AcquireItem(Sprite _itemSprite, SkillType _skillType)
    //{
    //    int skillIdx = (int)_skillType;
    //    if(skillIdx >= m_listSlot.Count)
    //    {
    //        Debug.LogError("스킬 타입이 이상한 값이 들어옴");
    //        return;
    //    }
    //
    //    SkillSlot skillSlot = m_listSlot[skillIdx];
    //
    //    var icons = skillSlot.Icons;
    //    int nextIdx = skillSlot.NextEmptySlotIndex;
    //
    //    // 인벤토리 칸이 가득 찼다면 무시
    //    if (nextIdx >= icons.Length) return;
    //
    //    // 현재 빈 슬롯의 아이콘 이미지를 활성화하고 스프라이트 교체
    //    icons[nextIdx].sprite = _itemSprite;
    //    icons[nextIdx].gameObject.SetActive(true);
    //
    //    // 다음 칸을 가리키도록 인덱스 증가
    //    skillSlot.NextEmptySlotIndex++;
    //}
}
