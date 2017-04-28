using UnityEngine;
using System.Collections;
using Umeng;
using System.Collections.Generic;

public class UmengController {

	public static string AppKey = "584fd326b27b0a250b000061";

	public static string defaultPack = "com.soulgame.bear";

	public static string Free = "FREE";
	public static string MM = "MM";
	public static string Jidi = "JIDI";
	public static string WoShop = "WOSHOP";
	public static string Egame = "EGAME";
	public static string FourThree = "FOURTHREE";

	public static Dictionary<string, PublishAttr> Channels = new Dictionary<string, PublishAttr>() {
		{Free, new PublishAttr () {Symbols = Free, Name = "无计费", }},
		{MM, new PublishAttr {Symbols = MM, Name = "MM单网",umeng_channel = "mm",soulgame_channel = "mm"}},
		{Jidi, new PublishAttr {Symbols = Jidi, Name = "基地单网",umeng_channel = "jidi",soulgame_channel = "jidi"}},
		{WoShop, new PublishAttr {Symbols = WoShop, Name = "联通三网",umeng_channel = "woshop",soulgame_channel = "woshop"}},
		{Egame, new PublishAttr {Symbols = Egame, Name = "爱游戏三网",Pack = defaultPack + ".egame",umeng_channel = "egame",soulgame_channel = "egame",egame_channel = "10000000",wx_id = "wxc698cf72046210a2",wx_secret = "bc3443e3dd3a212cd66b37303e6daeb8"}},
		{FourThree, new PublishAttr {Symbols = FourThree, Name = "4399",Pack = defaultPack + ".m4399",umeng_channel = "4399",soulgame_channel = "4399",
				wx_id = "wx2b36f94d85d370e4", wx_secret = "72305dbfa498d11d6c7e5434ef17030e" }},
	};

	public static PublishAttr ChannelItem;

	static UmengController() {
		#if FREE
		ChannelItem = Channels[Free];
		#elif MM
		ChannelItem = Channels[MM];
		#elif JIDI
		ChannelItem = Channels[Jidi];
		#elif WOSHOP
		ChannelItem = Channels[WoShop];
		#elif EGAME
		ChannelItem = Channels[Egame];
		#elif FOURTHREE
		ChannelItem = Channels[FourThree];
		#else
		ChannelItem = Channels[Free];
		#endif

		GA.StartWithAppKeyAndChannelId (AppKey , ChannelItem.umeng_channel);
	}

	public UmengController() {
//		string appkey = Main.Instance.SDKManager.GetMetaValue ("UMENG_APPKEY");
//		string appchannel = Main.Instance.SDKManager.GetMetaValue ("UMENG_CHANNEL");
//		Debug.Log ("umeng " + appkey + " channel " + appchannel);
//		GA.StartWithAppKeyAndChannelId (appkey , appchannel);
	}

	public void Event(string ev) {
		GA.Event (ev);
	}

	#region event
	public static string ev_select_big_1001 = "1001";
	public static string ev_select_two_1002 = "1002";

	public static string ev_guide_mo_1003 = "1003";
	public static string ev_guide_kitchen_1004 = "1004";
	public static string ev_guide_eat_1005 = "1005";
	public static string ev_guide_wc_1006 = "1006";
	public static string ev_guide_bed_1007 = "1007";
	public static string ev_guide_close_1008 = "1008";
	public static string ev_guide_open_1009 = "1009";
	public static string ev_guide_pet_1010 = "1010";
	public static string ev_guide_farmget_1011 = "1011";
	public static string ev_guide_bo_1012 = "1012";
	public static string ev_guide_jiao_1013 = "1013";
	public static string ev_guide_mu_1014 = "1014";
	public static string ev_guide_muwei_1015 = "1015";
	public static string ev_guide_you_1016 = "1016";
	public static string ev_guide_yousuc_1017 = "1017";
	public static string ev_guide_jiasu_1018 = "1018";
	public static string ev_guide_renwu_1019 = "1019";
	public static string ev_guide_xiao_1020 = "1020";
	public static string ev_guide_xinjiao_1021 = "1021";
	public static string ev_guide_libao_1022 = "1022";
	public static string ev_guide_xinxin_1023 = "1023";

	public static string ev_level_1024 = "1024";
	public static string ev_top_1025 = "1025";
	public static string ev_topxian_1026 = "1026";
	public static string ev_topdan_1027 = "1027";
	public static string ev_mail_1028 = "1028";
	public static string ev_setting_1029 = "1029";
	public static string ev_nick_1030 = "1030";
	public static string ev_headchange_1031 = "1031";
	public static string ev_friend_1032 = "1032";
	public static string ev_findfriend_1033 = "1033";
	public static string ev_addfriend_1034 = "1034";

    public static string ev_wardrobe_1035= "1035";
    public static string ev_wardrobe_eye_1036 = "1036";
    public static string ev_wardrobe_raw_1037 = "1037";
    public static string ev_wardrobe_heat_1038 = "1038";
    public static string ev_wardrobe_cloth_1039 = "1039";
    public static string ev_wardrobe_tao_1040 = "1040";

    public static string ev_ar_1041 = "1041";
    public static string ev_parrot_1042 = "1042";
    public static string ev_share_1043 = "1043";
    public static string ev_sharebtn_1044 = "1044";
    public static string ev_tarrace_1045 = "1045";
    public static string ev_kitchen_1046 = "1046";
    public static string ev_washroom_1047 = "1047";
    public static string ev_bedroom_1048 = "1048";
    public static string ev_petbtn_1049 = "1049";
    public static string ev_matong_1050 = "1050";
    public static string ev_xishu_1051 = "1051";
    public static string ev_feizao_1052 = "1052";
    public static string ev_pengtou_1053 = "1053";
    public static string ev_taideng_1054 = "1054";

    public static string ev_visitxian_1055 = "1055";
    public static string ev_visitegg_1056 = "1056";
    public static string ev_visitshang_1057 = "1057";
    public static string ev_visitxia_1058 = "1058";
    public static string ev_petcangku_1059 = "1059";
    public static string ev_petxinxin_1060 = "1060";

	public static string ev_mu_ji_1061 = "1061";
    public static string ev_mu_niu_1062 = "1062";
    public static string ev_mu_zhu_1063 = "1063";
    public static string ev_mu_yang_1064 = "1064";
    public static string ev_mu_er_1065 = "1065";
    public static string ev_mu_feng_1066 = "1066";
    public static string ev_jiao_done_1067 = "1067";
    public static string ev_zhaohuan_1068 = "1068";
    public static string ev_cangmai_1069 = "1069";
    public static string ev_tou_1070 = "1070";
	#endregion

	#region level

    public void StartLevel(int level)
    {
        GA.StartLevel(LevelName(level));
	}

    public void FinishLevel(int level)
    {
        GA.FinishLevel(LevelName(level));
	}

    public void FailLevel(int level)
    {
        GA.FailLevel(LevelName(level));
	}

    public string LevelName(int level)
    {
        return level + "";
	}
	#endregion

	#region 支付，购买，消费
	public void Pay(double cash, int diamond) {
		GA.Pay (cash, GA.PaySource.Source9, diamond);
	}

	public void Pay(double cash, string item, int amount) {
		GA.Pay (cash, GA.PaySource.Source9, item, amount, 0);
	}

    public void Buy(string item, int amount, double price)
    {
        GA.Buy(item, amount, price);
    }

	public void Use(string item, int amount, double price) {
		GA.Use(item, amount, price);
	}
	#endregion
}
