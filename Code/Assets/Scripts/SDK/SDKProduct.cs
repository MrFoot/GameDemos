using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SDKProduct {

	public class ProductData {
		public string Key;
		public double Price;
		public string Name;
		public int Amount;

		public ProductData(string key, double price, string name, int amount) {
			this.Key = key;
			this.Price = price;
			this.Name = name;
			this.Amount = amount;
		}
	}

	public const int GiftVip = 700010;

	public const int EngryPotion = 400101;
	public const int HungerPotion = 400102;
	public const int HealingPotion = 400103;

	public const int GiftNew = 700012;
	public const int GiftYear = 700013;
	public const int GiftGrow = 700014;
	public const int GiftFlower = 700015;
	public const int GiftEgg = 700016;
	public const int GiftCoin = 700017;
	public const int GiftPolit = 700018;
	public const int GiftSnow = 700019;

    public const int Diamond50 = 700006;
    public const int Diamond150 = 700007;
    public const int Diamond500 = 700008;

	public const int ARBig = 1001;
	public const int ARTwo = 2001;
	public const int ARQiang = 3001;

	public static Dictionary<int, ProductData> Products = new Dictionary<int, ProductData> () {
		{GiftVip, new ProductData("104", 30, "至尊VIP", 1)},
		{Diamond50, new ProductData("101", 5, "250钻石", 250)},
		{Diamond150, new ProductData("102", 15, "1000钻石", 1000)},
		{Diamond500, new ProductData("103", 30, "2500钻石", 2500)},
		{EngryPotion, new ProductData("105", 1, "能量果汁", 1)},
		{HungerPotion, new ProductData("106", 1, "饥饿果汁", 1)},
		{HealingPotion, new ProductData("107", 2, "复原果汁", 1)},
		#if FOURTHREE
		{GiftNew, new ProductData("108", 1, "新手大礼包", 1)},
		#else
		{GiftNew, new ProductData("108", 0.1, "新手大礼包", 1)},
		#endif
		{GiftYear, new ProductData("402", 29, "新年大礼包", 1)},
		{GiftGrow, new ProductData("109", 18, "成长大礼包", 1)},
		{GiftFlower, new ProductData("404", 8, "鲜花大礼包", 1)},
		{GiftEgg, new ProductData("405", 8, "鸡蛋大礼包", 1)},
		{GiftCoin, new ProductData("406", 29, "金币大礼包", 1)},
		{GiftPolit, new ProductData("407", 18, "飞行员大礼包", 1)},
		{GiftSnow, new ProductData("408", 28, "雪地大礼包", 1)},
		{ARBig, new ProductData("110", 5, "AR拍照", 1)},
		{ARTwo, new ProductData("110", 5, "AR拍照", 1)},
		{ARQiang, new ProductData("110", 5, "AR拍照", 1)},
	};
}
