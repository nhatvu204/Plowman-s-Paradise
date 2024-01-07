using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopListingManager : ListingManager<ItemData>
{
    //The Shop Listing Entry prefab to instantiate
    public GameObject shopListing;
    //The transform of the grid to instantiate the entries on
    public Transform listingGrid;

    //Keep track of what player is going to purchase
    ItemData itemToBuy;
    int quantity;

    [Header("Confirmation Screen")]
    public GameObject confirmationScreen;
    public Text confirmationPrompt;
    public Text quantityText;
    public Text costCalculationText;
    public Button purchaseButton;

    protected override void DisplayListing(ItemData listingItem, GameObject listingGameObject)
    {
        listingGameObject.GetComponent<ShopListing>().Display(listingItem);
    }

    public void OpenConfirmationScreen(ItemData item)
    {
        itemToBuy = item;
        quantity = 1;
        RenderConfirmationScreen();
    }

    public void RenderConfirmationScreen()
    {
        confirmationScreen.SetActive(true);
        confirmationPrompt.text = $"Buy {itemToBuy.name} ?";
        quantityText.text = "x" + quantity;

        int cost = itemToBuy.cost * quantity;
        int playerMoneyLeft = PlayerStats.Money - cost;

        //Player can't buy if dont have enough money
        if(playerMoneyLeft < 0)
        {
            costCalculationText.text = "Insufficient funds";
            purchaseButton.interactable = false;
            return;
        }
        purchaseButton.interactable = true;

        costCalculationText.text = $"{PlayerStats.Money} > {playerMoneyLeft}";
    }

    public void AddQuantity()
    {
        quantity++;
        RenderConfirmationScreen();
    }

    public void SubtractQuantity()
    {
        if(quantity > 1)
        {
            quantity--;
        }
        RenderConfirmationScreen() ;
    }

    //Purchase and close confirmation screen
    public void ConfirmPurchase()
    {
        Shop.Purchase(itemToBuy, quantity);
        confirmationScreen.SetActive(false);
    }

    public void CancelPurchase()
    {
        confirmationScreen.SetActive(false);
    }
}
