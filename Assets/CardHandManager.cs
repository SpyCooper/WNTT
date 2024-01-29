using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandManager : MonoBehaviour
{

    [SerializeField] Button card1Button;
    [SerializeField] Button card2Button;
    [SerializeField] Button card3Button;
    [SerializeField] Button card4Button;

    private void Start()
    {
        card1Button.onClick.AddListener(() => {
            GameManager.Instance.SelectCard(0);
        });
        card2Button.onClick.AddListener(() => {
            GameManager.Instance.SelectCard(1);
        });
        card3Button.onClick.AddListener(() => {
            GameManager.Instance.SelectCard(2);
        });
        card4Button.onClick.AddListener(() => {
            GameManager.Instance.SelectCard(3);
        });
    }
}
