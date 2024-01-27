using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    public string cardName;
    public int cardType; // 0 = Emotional, 1 = Financial, 2 = Logistical

    public int publicOpinionEffect;
    public int legalTroubleEffect;
}
