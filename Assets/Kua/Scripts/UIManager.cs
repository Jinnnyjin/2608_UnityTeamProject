using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //public Slider HpSlider;
    //public Slider XpSlider;
    public TextMeshProUGUI TimerText;

    // 인벤토리 슬롯 안의 'Icon' 이미지 컴포넌트들을 순서대로 넣어줄 배열
    public Image[] ItemIcons;

    private float m_gameTime = 0f;
    private int m_nextEmptySlotIndex = 0; // 다음에 아이템이 들어갈 슬롯 번호

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
    public void AcquireItem(Sprite _itemSprite)
    {
        // 인벤토리 칸이 가득 찼다면 무시
        if (m_nextEmptySlotIndex >= ItemIcons.Length) return;

        // 현재 빈 슬롯의 아이콘 이미지를 활성화하고 스프라이트 교체
        ItemIcons[m_nextEmptySlotIndex].sprite = _itemSprite;
        ItemIcons[m_nextEmptySlotIndex].gameObject.SetActive(true);

        // 다음 칸을 가리키도록 인덱스 증가
        m_nextEmptySlotIndex++;
    }
}
