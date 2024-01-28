using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    int UILayer;
    Vector2 yes;

    private void Start()
    {
        yes = transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.position = new Vector2(transform.position.x, yes.y + 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.position = yes;
    }
}