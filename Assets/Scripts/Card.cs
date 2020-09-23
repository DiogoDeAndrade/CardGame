using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public bool     hidden = false;
    public CardDesc desc;

    [Header("References")]
    public Image            back;
    public TextMeshProUGUI  nameDisplay;
    public Image            image;
    public TextMeshProUGUI  flavourDisplay;

    void Start()
    {
        UpdateCard();
    }    

    public void Set(CardDesc card)
    {
        desc = card;
        UpdateCard();
    }

    void UpdateCard()
    {
        back.gameObject.SetActive(hidden);
        if (desc)
        {
            nameDisplay.text = desc.cardName;
            image.sprite = desc.image;
            image.enabled = (image.sprite != null);            
            flavourDisplay.text = desc.flavourText;
        }
    }
}
