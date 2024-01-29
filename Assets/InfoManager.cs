using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Animations;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public static InfoManager Instance { get; private set; }

    [SerializeField] Animator animator;

    [SerializeField] private GameObject topicObject;
    private string topicAddedTriggerName = "TopicAdded";
    [SerializeField] private TextMeshProUGUI topicText;

    [SerializeField] private GameObject newInfo1Object;
    private string newInfo1AddedTriggerName = "NewInfo1Added";
    [SerializeField] private TextMeshProUGUI info1Text;

    [SerializeField] private GameObject newInfo2Object;
    private string newInfo2AddedTriggerName = "NewInfo2Added";
    [SerializeField] private TextMeshProUGUI info2Text;

    [SerializeField] private GameObject newInfo3Object;
    private string newInfo3AddedTriggerName = "NewInfo3Added";
    [SerializeField] private TextMeshProUGUI info3Text;

    [SerializeField] private GameObject newInfo4Object;
    private string newInfo4AddedTriggerName = "NewInfo4Added";
    [SerializeField] private TextMeshProUGUI info4Text;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        topicObject.SetActive(false);
        newInfo1Object.SetActive(false);
        newInfo2Object.SetActive(false);
        newInfo3Object.SetActive(false);
        newInfo4Object.SetActive(false);
    }

    public void AddTopic(string text)
    {
        //Debug.Log(text);
        topicObject.SetActive(true);
        topicText.text = text;
        animator.SetTrigger(topicAddedTriggerName);
    }

    public void AddNewInfo1(string text)
    {
        Debug.Log(text);
        newInfo1Object.SetActive(true);
        info1Text.text = text;
        animator.SetTrigger(newInfo1AddedTriggerName);
    }

    public void AddNewInfo2(string text)
    {
        info2Text.text = text;
        newInfo2Object.SetActive(true);
        animator.SetTrigger(newInfo2AddedTriggerName);
    }

    public void AddNewInfo3(string text)
    {
        info3Text.text = text;
        newInfo3Object.SetActive(true);
        animator.SetTrigger(newInfo3AddedTriggerName);
    }

    public void AddNewInfo4(string text)
    {
        info4Text.text = text;
        newInfo4Object.SetActive(true);
        animator.SetTrigger(newInfo4AddedTriggerName);
    }
}
