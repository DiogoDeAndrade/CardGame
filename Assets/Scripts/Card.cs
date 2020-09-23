using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public bool hidden = false;

    [Header("References")]
    public Image    back;

    void Start()
    {
        UpdateCard();
    }

    void Update()
    {
        
    }

    void UpdateCard()
    {
        back.gameObject.SetActive(hidden);
    }
}
