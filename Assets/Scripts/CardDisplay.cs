/* CardDisplay.cs */
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Suit { Spade, Heart, Diamond, Club }

public class Card
{
    public Suit suit;
    public int number;
    public Card(Suit s, int n) { suit = s; number = n; }
}

public class CardDisplay : MonoBehaviour
{
    public TMP_Text[] cardTexts;
    public TMP_Text resultText;
    public TMP_Text generatedUnitText;
    public SlotGridManager slotGridManager;

    private List<Card> hand = new List<Card>(5);
    private Dictionary<string, string> unitMap = new()
    {
        { "No Pair", "Militia" },
        { "One Pair", "Archer" },
        { "Two Pair", "Gunner" },
        { "Three of a Kind", "Missile Drone" },
        { "Straight", "Sniper" },
        { "Flush", "Helicopter" },
        { "Full House", "Wizard" },
        { "Four of a Kind", "Gatling Tank" },
        { "Straight Flush", "Dragon" },
        { "Royal Flush", "Legendary Phoenix" }
    };

    public void InitHand()
    {
        List<Card> deck = GenerateDeck(); Shuffle(deck);
        hand.Clear();
        for (int i = 0; i < 5; i++)
        {
            Card c = deck[i]; hand.Add(c);
            cardTexts[i].text = $"{SuitToSymbol(c.suit)} {CardValueToString(c.number)}";
        }
        // 실시간 족보 표시
        string realTimeResult = EvaluateHand();
        resultText.text = realTimeResult;
    }

    public void RerollCardAt(int index)
    {
        List<Card> deck = GenerateDeck(); Shuffle(deck);
        Card newCard = deck[Random.Range(0, deck.Count)];
        hand[index] = newCard;
        cardTexts[index].text = $"{SuitToSymbol(newCard.suit)} {CardValueToString(newCard.number)}";
        // 실시간 족보 표시
        string realTimeResult = EvaluateHand();
        resultText.text = realTimeResult;
    }

    public void RerollCard0() => RerollCardAt(0);
    public void RerollCard1() => RerollCardAt(1);
    public void RerollCard2() => RerollCardAt(2);
    public void RerollCard3() => RerollCardAt(3);
    public void RerollCard4() => RerollCardAt(4);

    public void ConfirmSelection()
    {
        string result = EvaluateHand();
        // 결과 최종 표시 (필요시 UI 유지)  
        Debug.Log($"[확정된 족보] {result}");
        string unitName = unitMap.ContainsKey(result) ? unitMap[result] : unitMap["No Pair"];
        generatedUnitText.text = $"Generated Unit: {unitName}";
        Debug.Log($"[슬롯 배치 완료] {unitName} → 슬롯 {slotGridManager.GetReservedSlot()?.slotIndex}");
        // 슬롯에 유닛 배치
        Slot reserved = slotGridManager.GetReservedSlot();
        if (reserved != null)
            reserved.SetUnit(unitName);
        // 카드 UI 숨김
        slotGridManager.cardUI.SetActive(false);
    }

    private string EvaluateHand()
    {
        var numCounts = new Dictionary<int, int>();
        var suitCounts = new Dictionary<Suit, int>();
        List<int> numbers = new();
        foreach (var card in hand)
        {
            if (!numCounts.ContainsKey(card.number)) numCounts[card.number] = 0;
            numCounts[card.number]++;
            if (!suitCounts.ContainsKey(card.suit)) suitCounts[card.suit] = 0;
            suitCounts[card.suit]++;
            numbers.Add(card.number);
        }
        numbers.Sort();
        bool isFlush = suitCounts.ContainsValue(5);
        bool isStraight = IsStraight(numbers);
        bool isRoyal = isStraight && numbers[0] == 10;
        string result = "No Pair";
        if (isRoyal && isFlush) result = "Royal Flush";
        else if (isStraight && isFlush) result = "Straight Flush";
        else if (numCounts.ContainsValue(4)) result = "Four of a Kind";
        else if (numCounts.ContainsValue(3) && numCounts.ContainsValue(2)) result = "Full House";
        else if (isFlush) result = "Flush";
        else if (isStraight) result = "Straight";
        else if (numCounts.ContainsValue(3)) result = "Three of a Kind";
        else if (CountPairs(numCounts) == 2) result = "Two Pair";
        else if (CountPairs(numCounts) == 1) result = "One Pair";
        return result;
    }

    private bool IsStraight(List<int> numbers)
    {
        for (int i = 1; i < numbers.Count; i++)
            if (numbers[i] != numbers[i - 1] + 1)
                return false;
        return true;
    }

    private int CountPairs(Dictionary<int, int> counts)
    {
        int pairs = 0;
        foreach (var v in counts.Values) if (v == 2) pairs++;
        return pairs;
    }

    private List<Card> GenerateDeck()
    {
        var deck = new List<Card>();
        for (int i = 7; i <= 14; i++)
            foreach (Suit s in System.Enum.GetValues(typeof(Suit)))
                deck.Add(new Card(s, i));
        return deck;
    }

    private void Shuffle(List<Card> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            var tmp = list[i]; list[i] = list[r]; list[r] = tmp;
        }
    }

    private string CardValueToString(int v) => v switch
    {
        11 => "J",
        12 => "Q",
        13 => "K",
        14 => "A",
        _ => v.ToString()
    };

    private string SuitToSymbol(Suit s) => s switch
    {
        Suit.Spade => "♠",
        Suit.Heart => "♥",
        Suit.Diamond => "♦",
        Suit.Club => "♣",
        _ => "?"
    };
}
