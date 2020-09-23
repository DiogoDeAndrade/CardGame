using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Card : MonoBehaviour
{
    public delegate void ClickEvent(Card card);

    public bool     hidden = false;
    public bool     tapped = false;
    public CardDesc desc;
    public int      defense = 0;

    ClickEvent OnClick;

    [Header("References")]
    public Image            pane;
    public Image            back;
    public TextMeshProUGUI  nameDisplay;
    public Image            image;
    public TextMeshProUGUI  flavourDisplay;
    public TextMeshProUGUI  costDisplay;
    public TextMeshProUGUI  attackDisplay;
    public TextMeshProUGUI  defenseDisplay;

    void Start()
    {
        UpdateCard();
    }    

    public void Set(CardDesc card)
    {
        desc = card;
        defense = desc.defensePower;
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

            if (desc.type == CardDesc.Type.Creature)
            {
                attackDisplay.transform.parent.gameObject.SetActive(true);
                attackDisplay.text = "" + GetAttackPower();
                defenseDisplay.text = "" + defense;
            }
            else
            {
                attackDisplay.transform.parent.gameObject.SetActive(false);
            }
        }
        if (tapped)
        {
            transform.rotation = Quaternion.Euler(0, 0, -10);
        }
        else
        {
            transform.rotation = Quaternion.identity;
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
        // Check if we're on a targeting mode
        Player activePlayer = GameMng.GetActivePlayer();
        if (activePlayer.IsTargeting())
        {
            activePlayer.RunAction(this);
            return;
        }

        OnClick?.Invoke(this);
    }

    public void Tap()
    {
        tapped = true;
        UpdateCard();
    }

    public void Untap()
    {
        tapped = false;
        UpdateCard();
    }

    public int GetAttackPower()
    {
        if (desc.type != CardDesc.Type.Creature) return 0;

        return desc.attackPower;
    }

    public int DealDamage(int inDamage)
    {
        if (defense >= inDamage)
        {
            defense -= inDamage;
            UpdateCard();
            return 0;
        }
        else
        {
            int ret = inDamage - defense;
            defense = 0;
            UpdateCard();
            return ret;
        }
    }

    public void Heal(int inHealing)
    {
        defense += inHealing;

        if (defense > desc.defensePower) defense = desc.defensePower;

        UpdateCard();
    }
}
