/* SlotGridManager.cs */
using UnityEngine;
using System.Collections.Generic;

public class SlotGridManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotParent;
    public GameObject cardUI;
    public CardDisplay cardDisplay;

    public int totalSlots = 16;

    private List<Slot> slots = new();
    private Slot reservedSlot;

    void Start()
    {
        GenerateSlots();
        OpenInitialSlots();
        if (cardUI != null)
            cardUI.SetActive(false);
        else
            Debug.LogError("cardUI is not assigned in SlotGridManager");
    }

    void GenerateSlots()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotParent);
            Slot slot = go.GetComponent<Slot>();
            int group = i / 4;
            slot.Initialize(this, i, group);
            slots.Add(slot);
        }
    }

    void OpenInitialSlots()
    {
        for (int group = 0; group < 4; group++)
        {
            List<Slot> groupSlots = slots.FindAll(s => s.groupIndex == group);
            int pick = Random.Range(0, groupSlots.Count);
            groupSlots[pick].OpenSlot();
        }
    }

    public void OnSlotClicked(Slot clicked)
    {
        if (reservedSlot != null)
            reservedSlot.SetReserved(false);

        reservedSlot = clicked;
        reservedSlot.SetReserved(true);

        Debug.Log($"[예약 슬롯] index: {clicked.slotIndex}, group: {clicked.groupIndex}");

        if (cardDisplay != null && cardUI != null)
        {
            cardUI.SetActive(true);
            cardDisplay.InitHand();
        }
    }

    public Slot GetReservedSlot() => reservedSlot;
}