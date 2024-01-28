using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    int UILayer;
    Vector2 yes;
    Animator animator;
    private void Start()
    {
        yes = transform.position;
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.Play("Card up");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.Play("Card down");
    }
}