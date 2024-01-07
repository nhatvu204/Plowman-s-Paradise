using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCRelationshipListing : MonoBehaviour
{
    [Header("Sprite")]
    public Sprite emptyHeart, fullHeart;

    [Header("UI Elements")]
    public Image portraitImage;
    public Text nameText;
    public Image[] hearts;
    
    public void Display(CharacterData characterData, NPCRelationshipStats relationship)
    {
        portraitImage.sprite = characterData.portrait;
        nameText.text = relationship.name;

        DisplayHearts(relationship.Hearts());
    }

    void DisplayHearts(float number)
    {
        //Set everything to empty
        foreach (Image heart in hearts)
        {
            heart.sprite = emptyHeart;
        }

        //Set the full hearts according to the number given
        for (int i = 0; i < number; i++)
        {
            hearts[i].sprite = fullHeart;
        }
    }
}
