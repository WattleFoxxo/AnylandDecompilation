using System;

// Token: 0x0200024F RID: 591
public static class ServerURLs
{
	// Token: 0x04001366 RID: 4966
	public static readonly string StartAuthenticatedSession = "/auth/start";

	// Token: 0x04001367 RID: 4967
	public static readonly string EndAuthenticatedSession = "/auth/end";

	// Token: 0x04001368 RID: 4968
	public static readonly string PollServer = "/p";

	// Token: 0x04001369 RID: 4969
	public static readonly string GetPersonInfo = "/person/info";

	// Token: 0x0400136A RID: 4970
	public static readonly string GetPersonInfoBasic = "/person/infobasic";

	// Token: 0x0400136B RID: 4971
	public static readonly string AddFriend = "/person/addfriend";

	// Token: 0x0400136C RID: 4972
	public static readonly string RemoveFriend = "/person/removefriend";

	// Token: 0x0400136D RID: 4973
	public static readonly string IncreaseFriendshipStrength = "/person/incfriendstrength";

	// Token: 0x0400136E RID: 4974
	public static readonly string GetFriends = "/person/friends";

	// Token: 0x0400136F RID: 4975
	public static readonly string GetFriendsByStrength = "/person/friendsbystr";

	// Token: 0x04001370 RID: 4976
	public static readonly string UpdatePersonalSetting = "/person/updatesetting";

	// Token: 0x04001371 RID: 4977
	public static readonly string UpdateAttachment = "/person/updateattachment";

	// Token: 0x04001372 RID: 4978
	public static readonly string SetHandColor = "/person/sethandcolor";

	// Token: 0x04001373 RID: 4979
	public static readonly string GetPersonFlagStatus = "/person/getflag";

	// Token: 0x04001374 RID: 4980
	public static readonly string TogglePersonFlag = "/person/toggleflag";

	// Token: 0x04001375 RID: 4981
	public static readonly string PingPerson = "/person/ping";

	// Token: 0x04001376 RID: 4982
	public static readonly string RegisterUsageMode = "/person/registerusagemode";

	// Token: 0x04001377 RID: 4983
	public static readonly string RegisterHold = "/person/registerhold";

	// Token: 0x04001378 RID: 4984
	public static readonly string GetHoldGeometry = "/person/getholdgeometry";

	// Token: 0x04001379 RID: 4985
	public static readonly string GetQuickEquipList = "/person/getquickequiplist";

	// Token: 0x0400137A RID: 4986
	public static readonly string RequestWelcome = "/person/requestwelcome";

	// Token: 0x0400137B RID: 4987
	public static readonly string SetCustomSearchWords = "/person/setcustomsearchwords";

	// Token: 0x0400137C RID: 4988
	public static readonly string GetThing = "/thing";

	// Token: 0x0400137D RID: 4989
	public static readonly string GetThingDefinition = "/sl/tdef";

	// Token: 0x0400137E RID: 4990
	public static readonly string GetThingDefinition_CDN = "http://d6ccx151yatz6.cloudfront.net";

	// Token: 0x0400137F RID: 4991
	public static readonly string GetThingDefinitionAreaBundle = "/sl/tdefbdl";

	// Token: 0x04001380 RID: 4992
	public static readonly string GetThingDefinitionAreaBundle_CDN = "http://d26e4xubm8adxu.cloudfront.net";

	// Token: 0x04001381 RID: 4993
	public static readonly string SaveThing = "/thing";

	// Token: 0x04001382 RID: 4994
	public static readonly string GetRecentlyDeletedThingIds = "/thing/recentlydeletedids";

	// Token: 0x04001383 RID: 4995
	public static readonly string GetTopThingIdsCreatedByPerson = "/thing/topby";

	// Token: 0x04001384 RID: 4996
	public static readonly string GetThingInfo = "/thing/info";

	// Token: 0x04001385 RID: 4997
	public static readonly string GetThingFlagStatus = "/thing/getflag";

	// Token: 0x04001386 RID: 4998
	public static readonly string ToggleThingFlag = "/thing/toggleflag";

	// Token: 0x04001387 RID: 4999
	public static readonly string SearchThings = "/thing/search";

	// Token: 0x04001388 RID: 5000
	public static readonly string ReportMissingThing = "/thing/reportmissing";

	// Token: 0x04001389 RID: 5001
	public static readonly string SetThingTags = "/thing/settags";

	// Token: 0x0400138A RID: 5002
	public static readonly string GetThingTags = "/thing/gettags";

	// Token: 0x0400138B RID: 5003
	public static readonly string SetThingUnlisted = "/thing/setunlisted";

	// Token: 0x0400138C RID: 5004
	public static readonly string NewPlacement = "/placement/new";

	// Token: 0x0400138D RID: 5005
	public static readonly string UpdatePlacement = "/placement/update";

	// Token: 0x0400138E RID: 5006
	public static readonly string DeletePlacement = "/placement/delete";

	// Token: 0x0400138F RID: 5007
	public static readonly string DeleteAllPlacements = "/placement/deleteall";

	// Token: 0x04001390 RID: 5008
	public static readonly string ReplaceAllOccurrencesOfThing = "/placement/replacething";

	// Token: 0x04001391 RID: 5009
	public static readonly string GetPlacementInfo = "/placement/info";

	// Token: 0x04001392 RID: 5010
	public static readonly string SetPlacementAttribute = "/placement/setattr";

	// Token: 0x04001393 RID: 5011
	public static readonly string ClearPlacementAttribute = "/placement/clearattr";

	// Token: 0x04001394 RID: 5012
	public static readonly string GetThingIdsInAreaByOtherCreators = "/placement/getthingidsinareabyothercreators";

	// Token: 0x04001395 RID: 5013
	public static readonly string CopyPlacements = "/placement/copyall";

	// Token: 0x04001396 RID: 5014
	public static readonly string LoadInventory = "/inventory";

	// Token: 0x04001397 RID: 5015
	public static readonly string SaveInventoryItem = "/inventory/save";

	// Token: 0x04001398 RID: 5016
	public static readonly string UpdateInventoryItem = "/inventory/update";

	// Token: 0x04001399 RID: 5017
	public static readonly string DeleteInventoryItem = "/inventory/delete";

	// Token: 0x0400139A RID: 5018
	public static readonly string LoadArea = "/area/load";

	// Token: 0x0400139B RID: 5019
	public static readonly string CreateArea = "/area";

	// Token: 0x0400139C RID: 5020
	public static readonly string RenameArea = "/area/rename";

	// Token: 0x0400139D RID: 5021
	public static readonly string UpdateAreaSettings = "/area/updatesettings";

	// Token: 0x0400139E RID: 5022
	public static readonly string SetParentArea = "/area/setparentarea";

	// Token: 0x0400139F RID: 5023
	public static readonly string GetSubAreas = "/area/getsubareas";

	// Token: 0x040013A0 RID: 5024
	public static readonly string SetAreaEditor = "/area/seteditor";

	// Token: 0x040013A1 RID: 5025
	public static readonly string SetAreaListEditor = "/area/setlisteditor";

	// Token: 0x040013A2 RID: 5026
	public static readonly string LockPersonFromArea = "/area/lockperson";

	// Token: 0x040013A3 RID: 5027
	public static readonly string UnlockPersonFromArea = "/area/unlockperson";

	// Token: 0x040013A4 RID: 5028
	public static readonly string GetAreaLists = "/area/lists";

	// Token: 0x040013A5 RID: 5029
	public static readonly string SearchAreas = "/area/search";

	// Token: 0x040013A6 RID: 5030
	public static readonly string GetAreaFlagStatus = "/area/getflag";

	// Token: 0x040013A7 RID: 5031
	public static readonly string ToggleAreaFlag = "/area/toggleflag";

	// Token: 0x040013A8 RID: 5032
	public static readonly string GetAreaInfo = "/area/info";

	// Token: 0x040013A9 RID: 5033
	public static readonly string GetRandomArea = "/area/random";

	// Token: 0x040013AA RID: 5034
	public static readonly string SetFavoriteArea = "/area/setfavorite";

	// Token: 0x040013AB RID: 5035
	public static readonly string SetHomeArea = "/area/sethome";

	// Token: 0x040013AC RID: 5036
	public static readonly string RegisterAchievement = "/ach/reg";

	// Token: 0x040013AD RID: 5037
	public static readonly string CreateForum = "/forum/forum/new";

	// Token: 0x040013AE RID: 5038
	public static readonly string GetForum = "/forum/forum";

	// Token: 0x040013AF RID: 5039
	public static readonly string GetForumInfo = "/forum/foruminfo";

	// Token: 0x040013B0 RID: 5040
	public static readonly string EditForumInfo = "/forum/forum/edit";

	// Token: 0x040013B1 RID: 5041
	public static readonly string GetForumIdFromName = "/forum/forumid";

	// Token: 0x040013B2 RID: 5042
	public static readonly string GetForumThread = "/forum/thread";

	// Token: 0x040013B3 RID: 5043
	public static readonly string GetForumThreadInfo = "/forum/threadinfo";

	// Token: 0x040013B4 RID: 5044
	public static readonly string AddForumThread = "/forum/thread/new";

	// Token: 0x040013B5 RID: 5045
	public static readonly string AddForumComment = "/forum/comment/new";

	// Token: 0x040013B6 RID: 5046
	public static readonly string EditForumComment = "/forum/comment/edit";

	// Token: 0x040013B7 RID: 5047
	public static readonly string RemoveForumComment = "/forum/comment/remove";

	// Token: 0x040013B8 RID: 5048
	public static readonly string LikeForumComment = "/forum/comment/like";

	// Token: 0x040013B9 RID: 5049
	public static readonly string ToggleForumThreadSticky = "/forum/thread/togglesticky";

	// Token: 0x040013BA RID: 5050
	public static readonly string ToggleForumThreadLocked = "/forum/thread/togglelocked";

	// Token: 0x040013BB RID: 5051
	public static readonly string ClarifyForumThreadTitle = "/forum/thread/clarifytitle";

	// Token: 0x040013BC RID: 5052
	public static readonly string RemoveForumThread = "/forum/thread/remove";

	// Token: 0x040013BD RID: 5053
	public static readonly string GetFavoriteForums = "/forum/favorites";

	// Token: 0x040013BE RID: 5054
	public static readonly string ToggleFavoriteForum = "/forum/forum/togglefavorite";

	// Token: 0x040013BF RID: 5055
	public static readonly string SetForumProtectionLevel = "/forum/forum/setprotection";

	// Token: 0x040013C0 RID: 5056
	public static readonly string SetForumDialog = "/forum/forum/setdialog";

	// Token: 0x040013C1 RID: 5057
	public static readonly string SearchForums = "/forum/search";

	// Token: 0x040013C2 RID: 5058
	public static readonly string SearchForumThreads = "/forum/searchthreads";

	// Token: 0x040013C3 RID: 5059
	public static readonly string SubmitGift = "/gift/submit";

	// Token: 0x040013C4 RID: 5060
	public static readonly string GetReceivedGifts = "/gift/getreceived";

	// Token: 0x040013C5 RID: 5061
	public static readonly string ToggleGiftPrivacy = "/gift/toggleprivacy";

	// Token: 0x040013C6 RID: 5062
	public static readonly string MarkGiftSeen = "/gift/markseen";

	// Token: 0x040013C7 RID: 5063
	public static readonly string StartEditToolsTrial = "/extras/startedittoolstrial";

	// Token: 0x040013C8 RID: 5064
	public static readonly string StartEditToolsPurchase = "/extras/startedittoolspurchase";

	// Token: 0x040013C9 RID: 5065
	public static readonly string StartPurchase = "/extras/startpurchase";

	// Token: 0x040013CA RID: 5066
	public static readonly string CompletePurchase = "/extras/completepurchase";

	// Token: 0x040013CB RID: 5067
	public static readonly string CancelPurchase = "/extras/cancelpurchase";
}
