using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SlotGridManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotParent;
    public PathManager pathManager;     // Inspector에 꼭 할당
    public RectTransform slotGridRect;
    public GameObject cardUI;
    public CardDisplay cardDisplay;
    public int totalSlots = 16;

    private List<Slot> slots = new();
    private Slot reservedSlot;

    IEnumerator Start()
    {
        GenerateSlots();
        OpenInitialSlots();
        Debug.Log($"[SlotGridManager] Generated {slots.Count} slots.");

        // Wait for UI layout to complete
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(slotGridRect);
        Debug.Log("[SlotGridManager] Forced rebuild layout on slotGridRect");

        if (pathManager != null)
        {
            Debug.Log("[SlotGridManager] Calling GeneratePathPoints()");
            pathManager.slotGridRect = slotGridRect;
            pathManager.GeneratePathPoints();
        }
        else
        {
            Debug.LogError("[SlotGridManager] pathManager is not assigned!");
        }

        // Hide card UI
        if (cardUI != null)
            cardUI.SetActive(false);
    }

    void GenerateSlots()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            var go = Instantiate(slotPrefab, slotParent);
            var slot = go.GetComponent<Slot>();
            slot.Initialize(this, i, i / 4);
            slots.Add(slot);
        }
    }

    void OpenInitialSlots()
    {
        for (int group = 0; group < 4; group++)
        {
            var groupSlots = slots.FindAll(s => s.groupIndex == group);
            int pick = Random.Range(0, groupSlots.Count);
            groupSlots[pick].OpenSlot();
        }
    }

    public void OnSlotClicked(Slot clicked)
    {
        // 기존 로직...
    }

    public Slot GetReservedSlot() => reservedSlot;
}