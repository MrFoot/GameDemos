using System;
using FootStudio.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumeType
{
    None = 0,
    Coin = 1,
    RMB = 2,
    Diamond = 3,
}

public class PurchaseManager
{

	public UserManager UserManager;

	public EventBus EventBus;

	public void Init() {

	}

	//测试
	public void BuyCoin(int coin) {
		this.UserManager.ChangeCoin (coin);
		this.EventBus.FireEvent (EventId.COIN_CHANGE);
	}

	public void ConsumeCoin(int coin) {
		//NGUIDebug.Log (coin);
		this.BuyCoin (-coin);
	}

	public void BuyDiamond(int diamond) {
		this.UserManager.ChangeDiamond (diamond);
		this.EventBus.FireEvent (EventId.DIAMOND_CHANGE);
	}

	public void ConsumeDiamond(int diamond) {
		this.BuyDiamond (-diamond);
	}

    public void ConsumeMoney(ConsumeType type, int amount)
    {
        switch (type)
        {
            case ConsumeType.Coin:
                ConsumeCoin(amount);
                break;
            case ConsumeType.Diamond:
                ConsumeDiamond(amount);
                break;
            case ConsumeType.RMB:
                
                break;
            default:
                // Never Be Here
                break;
        }
    }

	public bool HasEnoughCoinToSpend(int amount) {
		return this.UserManager.GetCoin () >= amount;
	}

	public bool HasEnoughDiamondToSpend(int amount) {
		return this.UserManager.GetDiamond () >= amount;
	}

    public bool HasEnoughMoneyToSpend(ConsumeType type, int amount)
    {
        bool isEnough = false;
        switch (type)
        {
            case ConsumeType.Coin:
                isEnough = HasEnoughCoinToSpend(amount);
                break;
            case ConsumeType.Diamond:
                isEnough = HasEnoughDiamondToSpend(amount);
                break;
            case ConsumeType.RMB:
                //break;
            default:
                isEnough = true;  //默认返回true
                break;
        }

        return isEnough;
    }

	public static string GetMoneyIcon(int type) {
		if (type == 1) {
			return "icon_01";
		} else if (type == 3) {
			return "icon_02";
		} else {
			return "";
		}
	}
}


