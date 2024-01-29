using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class EndOfGame : MonoBehaviour
{
    public static EndOfGame Instance { get; private set; }

    [SerializeField] TextMeshProUGUI POScoreText;
    [SerializeField] Slider POSlider;
    [SerializeField] TextMeshProUGUI LTScoreText;
    [SerializeField] Slider LTSlider;
    [SerializeField] Button mainMenuButton;

    [SerializeField] GameObject EndOfGameScreen;

    private string highPOText = "The public accepts your apology.";
    private string medPOText = "The public doesn't seem to care.";
    private string lowPOText = "The public despises you.";

    private string highLTText = "You're probably going to jail.";
    private string medLTText = "You should probably get a lawyer ready.";
    private string lowLTText = "You're in no immediate threat of legal action.";

    [SerializeField] private Animator animator;
    private string animatorTriggerText = "GameHasEnded";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenu);
        });

        EndOfGameScreen.SetActive(false);
    }

    public void GameEnded()
    {
        POSlider.value = GameManager.Instance.GetPublicOpinionNormalized();
        if (POSlider.value <= .3)
        {
            POScoreText.text = lowPOText;
        }
        else if(POSlider.value <= .7)
        {
            POScoreText.text = medPOText;
        }
        else if (POSlider.value <= 1)
        {
            POScoreText.text = highPOText;
        }

        LTSlider.value = GameManager.Instance.GetLegalTroubleNormalized();
        if (LTSlider.value <= .3)
        {
            LTScoreText.text = lowLTText;
        }
        else if (LTSlider.value <= .7)
        {
            LTScoreText.text = medLTText;
        }
        else if (LTSlider.value <= 1)
        {
            LTScoreText.text = highLTText;
        }

        EndOfGameScreen.SetActive(true);
        animator.SetTrigger(animatorTriggerText);
    }

    public void ShowEOGScreen()
    {
        EndOfGameScreen.SetActive(true);
    }
}
