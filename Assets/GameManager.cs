using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public int turnNumber = 1;

    public int publicOpinionScore = 15;
    public int legalTroubleScore = 15;

    // Index corresponds to affected card type. 0 = Emotional, 1 = Financial, 2 = Logistical
    int[] publicOpinionModifier = new int[3];
    int[] legalTroubleModifier = new int[3];

    // Editor-accesible "master lists"
    public List<FurtherInfo> furtherInfoIndex;
    public List<Card> cardIndex;

    List<FurtherInfo> activeFurtherInfo = new List<FurtherInfo>();

    List<Card> deck;
    List<Card> discard = new List<Card>();
    List<Card> hand = new List<Card>();
    List<Card> cardsSelected = new List<Card>(); // Which cards are selected in Main Phase, 1 or 2

    Transform[] cardObjects = new Transform[4]; // References to the UI Buttons representing cards

    Image publicOpinionMeter;
    Image legalTroubleMeter;

    void Start()
    {
        gameState = GameState.InfoPhase;
        Debug.Log("Now in InfoPhase, turn " + turnNumber);

        // Object References
        {
            for (int i = 0; i < 4; i++)
            {
                cardObjects[i] = transform.GetChild(4).GetChild(1).GetChild(i);

            }
            publicOpinionMeter = transform.GetChild(3).GetChild(0).GetChild(0).GetComponentInChildren<Image>();
            legalTroubleMeter = transform.GetChild(3).GetChild(1).GetChild(0).GetComponentInChildren<Image>();
        }

        deck = cardIndex;
        ShuffleDeck();

        NextPhase();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateMeters()
    {
        publicOpinionMeter.fillAmount = publicOpinionScore / 30;
        legalTroubleMeter.fillAmount = legalTroubleScore / 30;

        Debug.Log("Meters Updated.");
    }

    void ShuffleDeck()
    {
        List<Card> bufferDeck = new List<Card>();

        for (int i = 0; i < deck.Count; i++)
        {
            bufferDeck.Insert(Random.Range(0, bufferDeck.Count), deck[i]);
        }

        deck = bufferDeck;
    }

    void NextPhase()
    {
        switch (gameState)
        {
            case GameState.InfoPhase:
                {
                    gameState = GameState.DrawPhase;
                    Debug.Log("Now in DrawPhase");
                    DrawFour();
                    break;
                }
            case GameState.DrawPhase:
                {
                    gameState = GameState.MainPhase;
                    Debug.Log("Now in MainPhase");
                    break;
                }
            case GameState.MainPhase:
                {
                    gameState = GameState.EndPhase;
                    Debug.Log("Now in EndPhase");
                    EvaluateCards();
                    break;
                }
            case GameState.EndPhase:
                {
                    if (turnNumber == 5)
                    {
                        EndGame();
                        break;
                    }
                    else
                    {
                        turnNumber += 1;

                        gameState = GameState.InfoPhase;
                        Debug.Log("Now in InfoPhase, turn " + turnNumber);
                        PullEvent();
                        break;
                    }
                }
        }
    }

    void DrawFour()
    {
        discard.AddRange(hand);
        hand.Clear();

        foreach (Transform currentCardObject in cardObjects)
        {
            // currentCardObject.GetComponentInChildren<TMP_Text>().text = deck[0].cardName;
            
            discard.Insert(0, deck[0]);
            hand.Add(deck[0]);
            deck.RemoveAt(0);

            Debug.Log(hand[hand.Count - 1] + " drawn.");
        }

        NextPhase();
    }

    void EvaluateCards()
    {
        foreach(Card currentCard in cardsSelected)
        {
            // Change score with event modifiers
            publicOpinionScore += currentCard.publicOpinionEffect + publicOpinionModifier[currentCard.cardType];
            legalTroubleScore += currentCard.legalTroubleEffect + legalTroubleModifier[currentCard.cardType];

            Debug.Log(currentCard.cardName + " played: " + currentCard.publicOpinionEffect + " / " + currentCard.legalTroubleEffect);
        }

        cardsSelected.Clear();
        UpdateMeters();

        NextPhase();
    }

    void PullEvent()
    {
        FurtherInfo newInfo = furtherInfoIndex[Random.Range(0, furtherInfoIndex.Count - 1)];

        activeFurtherInfo.Add(newInfo);
        furtherInfoIndex.Remove(newInfo); // Prevent duplicates

        // Adjust Modifiers
        publicOpinionModifier[newInfo.cardTypeAffected] += newInfo.publicOpinionEffect;
        legalTroubleModifier[newInfo.cardTypeAffected] += newInfo.legalTroubleEffect;

        Debug.Log("Further Info: " + newInfo.eventName + "!");

        NextPhase();
    }

    void EndGame()
    {
        Debug.Log("Game end.");
    }

    public void CardSelected(int cardIndex) // Card index read R to L in UI (3, 2, 1, 0)
    {
        if (gameState != GameState.MainPhase) return; // Only possible in Main Phase

        if (cardsSelected.Contains(hand[cardIndex])) // Deselection
        {
            cardsSelected.Remove(hand[cardIndex]);
            Debug.Log(hand[cardIndex].cardName + " deselected.");
        }
        else if (cardsSelected.Count < 2) // Max 2
        {
            cardsSelected.Add(hand[cardIndex]);
            Debug.Log(hand[cardIndex].cardName + " selected.");
        }
    }

    public void NextTurnButton()
    {
        if (gameState != GameState.MainPhase) return; // Main Phase only
        if (cardsSelected.Count < 1) return; // At least 1 card

        NextPhase();
    }

    public enum GameState
    {
        InfoPhase,
        DrawPhase,
        MainPhase,
        EndPhase
    }
}
