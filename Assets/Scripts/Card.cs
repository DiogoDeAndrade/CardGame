using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public delegate void ClickEvent(Card card);

    public bool     hidden = false;
    public CardDesc desc;

    ClickEvent OnClick;

    [Header("References")]
    public Image            pane;
    public Image            back;
    public TextMeshProUGUI  nameDisplay;
    public Image            image;
    public TextMeshProUGUI  flavourDisplay;
    public TextMeshProUGUI  costDisplay;

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

            if (desc.cost > 0)
            {
                costDisplay.text = "" + desc.cost;
                costDisplay.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                costDisplay.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void SetEventHandler(ClickEvent handler)
    {
        if (handler != null)
        {
            OnClick = handler;
            pane.raycastTarget = true;
        }
        else
        {
            OnClick = null;
            pane.raycastTarget = false;
        }
    }

    public void ClickedCard()
    {
        OnClick?.Invoke(this);
    }
}
