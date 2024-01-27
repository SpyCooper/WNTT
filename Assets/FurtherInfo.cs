using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FurtherInfo : ScriptableObject
{
    public string eventName;
    public int cardTypeAffected; // 0 = Emotional, 1 = Financial, 2 = Logistical

    public int publicOpinionEffect;
    public int legalTroubleEffect; 
}
