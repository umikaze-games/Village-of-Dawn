using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeUI : MonoBehaviour
{
	public Image tradeItemIcon;
	public TextMeshProUGUI tradeItemName;
	public TMP_InputField tradeAmount;
	public Button confirmBtn;
	public Button cancelBtn;

	private ItemDetails itemdetails;
	private bool isSellTrade;

	private void Awake()
	{
		cancelBtn.onClick.AddListener(CancelTrade);
		confirmBtn.onClick.AddListener(Trade);
	}
	public void SetupTradeUI(ItemDetails item, bool isSell)
	{
		itemdetails = item;
		tradeItemIcon.sprite = item.itemIcon;
		tradeItemName.text = item.itemName;
		isSellTrade = isSell;
	}

	public void CancelTrade()
	{ 
		this.gameObject.SetActive(false);
	}

	public void Trade()
	{
		int amount = Convert.ToInt32(tradeAmount.text);
		InventoryManager.Instance.TradeItem(itemdetails, amount, isSellTrade);
		CancelTrade();
	}
}
