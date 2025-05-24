/* Slot.cs */
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    public int slotIndex;
    public int groupIndex;
    public bool isOpen = false;
    public bool isReserved = false;
    public TMP_Text labelText;
    public Button button;

    private SlotGridManager manager;

    public void Initialize(SlotGridManager mgr, int index, int group)
    {
        manager = mgr;
        slotIndex = index;
        groupIndex = group;
        isOpen = false;
        isReserved = false;
        UpdateVisual();
    }

    public void OpenSlot()
    {
        isOpen = true;
        UpdateVisual();
    }

    public void SetReserved(bool reserved)
    {
        isReserved = reserved;
        UpdateVisual();
    }

    public void SetUnit(string unitName)
    {
        labelText.text = unitName;
    }

    private void UpdateVisual()
    {
        button.interactable = isOpen;

        if (!isOpen)
            labelText.text = "잠김";
        else if (isReserved)
            labelText.text = "★ 예약됨";
        else
            labelText.text = "빈 슬롯";
    }

    public void OnClick()
    {
        if (isOpen)
        {
            Debug.Log($"[Slot Clicked] index: {slotIndex}");
            manager.OnSlotClicked(this);
        }
    }
}