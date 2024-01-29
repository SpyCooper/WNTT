using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState gameState;
    public int turnNumber;

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

    Slider publicOpinionMeter;
    Slider legalTroubleMeter;

    [SerializeField] private Animator deckAnimator;
    private string drawCardsTriggerName = "PullCards";

    [SerializeField] private Animator handAnimator;
    private string drawCardsInHandTriggerName = "PullUpHandCards";
    private string removeCardsfromHandTriggerName = "RemoveCards";
    [SerializeField] private GameObject card1;
    [SerializeField] private GameObject card2;
    [SerializeField] private GameObject card3;
    [SerializeField] private GameObject card4;
    private int selectedCards;
    private const int maxSelectedCards = 2;
    bool nextTurnPressed = false;

    [SerializeField] private Button nextTurnButton;
    private int waitTime = 3;
    private int MaxTurns = 5;


    [SerializeField] private GameObject characterNeutral;
    [SerializeField] private GameObject characterCrying;
    [SerializeField] private GameObject characterShocked;
    [SerializeField] private GameObject characterTalking;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //gameState = GameState.EndPhase;;

        // Object References
        {
            for (int i = 0; i < 4; i++)
            {
                cardObjects[i] = transform.GetChild(4).GetChild(1).GetChild(i);

            }
            publicOpinionMeter = transform.GetChild(3).GetChild(0).GetComponentInChildren<Slider>();
            legalTroubleMeter = transform.GetChild(3).GetChild(1).GetComponentInChildren<Slider>();
        }

        nextTurnButton.onClick.AddListener(() => {
            if (selectedCards > 0)
                {
                    nextTurnPressed = true;
                    //NextPhase();
                }
        });

        UpdateMeters();
        deck = cardIndex;
        ShuffleDeck();


        // Starts and runs the game
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        for(int i = 0; i < 5; i++)
        {
            DeselectCard(i);
        }
        ChangeCharacter(CharacterSprite.neutral);
        selectedCards = 0;
        turnNumber = 0;

        //NextPhase();

        //StartInfoAnimation();
        yield return new WaitForSeconds(1);

        for(; turnNumber < MaxTurns; turnNumber++)
        {
            gameState = GameState.InfoPhase;
            Debug.Log("Now in InfoPhase, turn " + turnNumber);
            if (turnNumber != 0)
            {
                GetNewInfo();
            }
            StartInfoAnimation();
            yield return new WaitForSeconds(waitTime);

            //draw phase
            gameState = GameState.DrawPhase;
            Debug.Log("Now in DrawPhase");
            DrawFour();
            yield return new WaitForSeconds(1);
            deckAnimator.ResetTrigger(drawCardsTriggerName);

            //main phase
            gameState = GameState.MainPhase;
            Debug.Log("Now in MainPhase");
            while(!nextTurnPressed)
            {
                yield return new WaitForSeconds(1);
            }
            nextTurnPressed = false;
            Card temp = cardsSelected[0];
            Debug.Log(temp.name); ;

            //end phase
            gameState = GameState.EndPhase;
            Debug.Log("Now in EndPhase");
            
            EvaluateCards();
            handAnimator.SetTrigger(removeCardsfromHandTriggerName);
            yield return new WaitForSeconds(1);

            ChangeCharacter(temp.characterSpriteChange);
            yield return new WaitForSeconds(waitTime);

            for (int i = 0; i < 4; i++)
            {
                DeselectCard(i);
            }
        }

        EndGame();
    }

    void UpdateMeters()
    {
        publicOpinionMeter.value = publicOpinionScore;
        legalTroubleMeter.value = legalTroubleScore;

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

    //void NextPhase()
    //{
    //    switch (gameState)
    //    {
    //        case GameState.InfoPhase:
    //            {
    //                gameState = GameState.DrawPhase;
    //                Debug.Log("Now in DrawPhase");
    //                DrawFour();
    //                break;
    //            }
    //        case GameState.DrawPhase:
    //            {
    //                gameState = GameState.MainPhase;
    //                Debug.Log("Now in MainPhase");
    //                break;
    //            }
    //        case GameState.MainPhase:
    //            {
    //                gameState = GameState.EndPhase;
    //                Debug.Log("Now in EndPhase");
    //                EvaluateCards();
    //                handAnimator.SetTrigger(removeCardsfromHandTriggerName);
    //                break;
    //            }
    //        case GameState.EndPhase:
    //            {
    //                if (turnNumber == 5)
    //                {
    //                    EndGame();
    //                }
    //                else
    //                {
    //                    gameState = GameState.InfoPhase;

    //                    Debug.Log("Now in InfoPhase, turn " + turnNumber);
    //                    GetNewInfo();
    //                    StartInfoAnimation();
    //                    NextPhase();
    //                    turnNumber += 1;
    //                }
    //                break;
    //            }
    //    }
    //}

    void DrawFour()
    {
        deckAnimator.SetTrigger(drawCardsTriggerName);

        discard.AddRange(hand);
        hand.Clear();

        int iteration = 0;
        foreach (Transform currentCardObject in cardObjects)
        {
            // currentCardObject.GetComponentInChildren<TMP_Text>().text = deck[0].cardName;
            
            discard.Insert(0, deck[0]);
            hand.Add(deck[0]);
            deck.RemoveAt(0);
            
            currentCardObject.GetChild(1).GetComponent<Image>().sprite = hand[iteration].cardSprite;

            Debug.Log(hand[hand.Count - 1] + " drawn.");
            iteration++;
        }


        handAnimator.SetTrigger(drawCardsInHandTriggerName);

        //NextPhase();
    }

    void EvaluateCards()
    {
        foreach (Card currentCard in cardsSelected)
        {
            // Change score with event modifiers
            publicOpinionScore += currentCard.publicOpinionEffect + publicOpinionModifier[currentCard.cardType];
            legalTroubleScore += currentCard.legalTroubleEffect + legalTroubleModifier[currentCard.cardType];

            Debug.Log(currentCard.cardName + " played: " + currentCard.publicOpinionEffect + " / " + currentCard.legalTroubleEffect);
        }

        cardsSelected.Clear();
        UpdateMeters();

        //NextPhase();
    }

    void GetNewInfo()
    {
        FurtherInfo newInfo = furtherInfoIndex[Random.Range(0, furtherInfoIndex.Count - 1)];
        Debug.Log(newInfo.name);

        activeFurtherInfo.Add(newInfo);
        furtherInfoIndex.Remove(newInfo); // Prevent duplicates

        // Adjust Modifiers
        publicOpinionModifier[newInfo.cardTypeAffected] += newInfo.publicOpinionEffect;
        legalTroubleModifier[newInfo.cardTypeAffected] += newInfo.legalTroubleEffect;

        Debug.Log("Further Info: " + newInfo.eventName + "!");
    }

    void EndGame()
    {
        EndOfGame.Instance.GameEnded();
    }

    public void CardSelected(int cardIndex)
    {
        //if (gameState != GameState.MainPhase) return; // Only possible in Main Phase

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

    public float GetPublicOpinionNormalized()
    {
        return publicOpinionScore / publicOpinionMeter.maxValue;
    }
    public float GetLegalTroubleNormalized()
    {
        return legalTroubleScore / legalTroubleMeter.maxValue;
    }

    public void SelectCard(int pos)
    {
            if (pos == 0)
            {
                if (card1.GetComponent<Outline>().enabled == true)
                {
                    card1.GetComponent<Outline>().enabled = false;
                    selectedCards--;
                }
                else
                {
                    if (selectedCards < maxSelectedCards)
                    {
                        card1.GetComponent<Outline>().enabled = true;
                        selectedCards++;
                    CardSelected(pos);
                    }

                }   
            }
            else if (pos == 1)
            {
                if (card2.GetComponent<Outline>().enabled == true)
                {
                    card2.GetComponent<Outline>().enabled = false;
                    selectedCards--;
                }
                else
                {
                    if (selectedCards < maxSelectedCards)
                    {
                        card2.GetComponent<Outline>().enabled = true;
                        selectedCards++;
                    }
                }
            }
            else if (pos == 2)
            {
                if (card3.GetComponent<Outline>().enabled == true)
                {
                    card3.GetComponent<Outline>().enabled = false;
                    selectedCards--;
                }
                else
                {
                    if (selectedCards < maxSelectedCards)
                    {
                        card3.GetComponent<Outline>().enabled = true;
                        selectedCards++;
                    }
                }
            }
            else if (pos == 3)
            {
                if (card4.GetComponent<Outline>().enabled == true)
                {
                    card4.GetComponent<Outline>().enabled = false;
                    selectedCards--;
                }
                else
                {
                    if(selectedCards < maxSelectedCards)
                    {
                        card4.GetComponent<Outline>().enabled = true;
                        selectedCards++;
                    }
                    
                }
            }
    }


    public void DeselectCard(int pos)
    {
        if (pos == 0)
        {
            if (card1.GetComponent<Outline>().enabled == true)
            {
                card1.GetComponent<Outline>().enabled = false;
                selectedCards--;
            }
        }
        else if (pos == 1)
        {
            if (card2.GetComponent<Outline>().enabled == true)
            {
                card2.GetComponent<Outline>().enabled = false;
                selectedCards--;
            }
        }
        else if (pos == 2)
        {
            if (card3.GetComponent<Outline>().enabled == true)
            {
                card3.GetComponent<Outline>().enabled = false;
                selectedCards--;
            }
        }
        else if (pos == 3)
        {
            if (card4.GetComponent<Outline>().enabled == true)
            {
                card4.GetComponent<Outline>().enabled = false;
                selectedCards--;
            }
        }
    }

    private void StartInfoAnimation()
    {
        switch(turnNumber)
        {
            case 0:
                InfoManager.Instance.AddTopic("Fraud");
                break;
            case 1:
                InfoManager.Instance.AddNewInfo1(furtherInfoIndex[0].eventName);
                break;
            case 2:
                InfoManager.Instance.AddNewInfo2(furtherInfoIndex[1].eventName);
                break;
            case 3:
                InfoManager.Instance.AddNewInfo3(furtherInfoIndex[2].eventName);
                break;
            case 4:
                InfoManager.Instance.AddNewInfo4(furtherInfoIndex[3].eventName);
                break;
            default:
                break;
        }

        
    }
    private void ChangeCharacter(CharacterSprite character)
    {
        if(character == CharacterSprite.talking)
        {
            characterCrying.SetActive(false);
            characterNeutral.SetActive(false);
            characterShocked.SetActive(false);
            characterTalking.SetActive(true);
        }
        else if (character == CharacterSprite.neutral)
        {
            characterCrying.SetActive(false);
            characterNeutral.SetActive(true);
            characterShocked.SetActive(false);
            characterTalking.SetActive(false);
        }
        else if (character == CharacterSprite.shocked)
        {
            characterCrying.SetActive(false);
            characterNeutral.SetActive(false);
            characterShocked.SetActive(true);
            characterTalking.SetActive(false);
        }
        else if (character == CharacterSprite.crying)
        {
            characterCrying.SetActive(true);
            characterNeutral.SetActive(false);
            characterShocked.SetActive(false);
            characterTalking.SetActive(false);
        }
    }

    public enum GameState
    {
        InfoPhase,
        DrawPhase,
        MainPhase,
        EndPhase,
        Paused,
        StartPhase,
    }
}
