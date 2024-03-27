using System;
using System.IO;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000280 RID: 640
public class TogglableFeatures
{
	// Token: 0x06001846 RID: 6214 RVA: 0x000DFCF7 File Offset: 0x000DE0F7
	public void Init()
	{
		this.jsonFilePath = Application.persistentDataPath + "/features.json";
		if (File.Exists(this.jsonFilePath))
		{
			this.LoadValuesFromJsonFile();
		}
		else
		{
			this.SaveJsonFileWithCurrentSettings();
		}
	}

	// Token: 0x06001847 RID: 6215 RVA: 0x000DFD30 File Offset: 0x000DE130
	private void LoadValuesFromJsonFile()
	{
		string text = File.ReadAllText(this.jsonFilePath);
		try
		{
			JSONNode jsonnode = JSON.Parse(text);
			if (jsonnode != null)
			{
				this.ringMenu = this.GetBool(jsonnode, "ringMenu", this.ringMenu);
				this.createThings = this.GetBool(jsonnode, "createThings", this.createThings);
				this.changeThings = this.GetBool(jsonnode, "changeThings", this.changeThings);
				this.areaDialog = this.GetBool(jsonnode, "areaDialog", this.areaDialog);
				this.forums = this.GetBool(jsonnode, "boards", this.forums);
				this.friendsDialog = this.GetBool(jsonnode, "friendsDialog", this.friendsDialog);
				this.areasDialog = this.GetBool(jsonnode, "areasDialog", this.areasDialog);
				this.ownProfileDialog = this.GetBool(jsonnode, "meDialog", this.ownProfileDialog);
				this.profileDialog = this.GetBool(jsonnode, "profileDialog", this.profileDialog);
				this.quitButton = this.GetBool(jsonnode, "quitButton", this.quitButton);
				this.pingAlerts = this.GetBool(jsonnode, "pingAlerts", this.pingAlerts);
				this.newbornAlerts = this.GetBool(jsonnode, "newbornAlerts", this.newbornAlerts);
				this.teleportLaser = this.GetBool(jsonnode, "teleportLaser", this.teleportLaser);
				this.contextLaser = this.GetBool(jsonnode, "contextLaser", this.contextLaser);
				this.homeButton = this.GetBool(jsonnode, "homeButton", this.homeButton);
				this.helpDialog = this.GetBool(jsonnode, "helpDialog", this.helpDialog);
				this.createArea = this.GetBool(jsonnode, "createArea", this.createArea);
				this.visitedAreasList = this.GetBool(jsonnode, "visitedAreasList", this.visitedAreasList);
				this.createdAreasList = this.GetBool(jsonnode, "createdAreasList", this.createdAreasList);
				this.favoritedAreasList = this.GetBool(jsonnode, "favoritedAreasList", this.favoritedAreasList);
				this.webBrowsing = this.GetBool(jsonnode, "webBrowsing", this.webBrowsing);
				this.hapticPulse = this.GetBool(jsonnode, "hapticPulse", this.webBrowsing);
				this.scriptsAsEditor = this.GetBool(jsonnode, "scriptsAsEditor", this.scriptsAsEditor);
				if (this.foundMissingProperties)
				{
					this.SaveJsonFileWithCurrentSettings();
				}
			}
		}
		catch (Exception ex)
		{
			Log.Debug("Togglable features JSON file threw an exception.");
		}
	}

	// Token: 0x06001848 RID: 6216 RVA: 0x000DFFC8 File Offset: 0x000DE3C8
	private bool GetBool(JSONNode data, string name, bool defaultValue)
	{
		bool flag;
		if (data[name] == null)
		{
			this.foundMissingProperties = true;
			flag = defaultValue;
		}
		else
		{
			flag = data[name].AsBool;
		}
		return flag;
	}

	// Token: 0x06001849 RID: 6217 RVA: 0x000E0008 File Offset: 0x000DE408
	public void SaveJsonFileWithCurrentSettings()
	{
		string text = string.Concat(new string[]
		{
			"{",
			Environment.NewLine,
			"    \"ringMenu\": ",
			JsonHelper.GetJson(this.ringMenu),
			",",
			Environment.NewLine,
			"    \"createThings\": ",
			JsonHelper.GetJson(this.createThings),
			",",
			Environment.NewLine,
			"    \"changeThings\": ",
			JsonHelper.GetJson(this.changeThings),
			",",
			Environment.NewLine,
			"    \"areaDialog\": ",
			JsonHelper.GetJson(this.areaDialog),
			",",
			Environment.NewLine,
			"    \"boards\": ",
			JsonHelper.GetJson(this.forums),
			",",
			Environment.NewLine,
			"    \"friendsDialog\": ",
			JsonHelper.GetJson(this.friendsDialog),
			",",
			Environment.NewLine,
			"    \"areasDialog\": ",
			JsonHelper.GetJson(this.areasDialog),
			",",
			Environment.NewLine,
			"    \"meDialog\": ",
			JsonHelper.GetJson(this.ownProfileDialog),
			",",
			Environment.NewLine,
			"    \"profileDialog\": ",
			JsonHelper.GetJson(this.profileDialog),
			",",
			Environment.NewLine,
			"    \"quitButton\": ",
			JsonHelper.GetJson(this.quitButton),
			",",
			Environment.NewLine,
			"    \"pingAlerts\": ",
			JsonHelper.GetJson(this.pingAlerts),
			",",
			Environment.NewLine,
			"    \"newbornAlerts\": ",
			JsonHelper.GetJson(this.newbornAlerts),
			",",
			Environment.NewLine,
			"    \"teleportLaser\": ",
			JsonHelper.GetJson(this.teleportLaser),
			",",
			Environment.NewLine,
			"    \"contextLaser\": ",
			JsonHelper.GetJson(this.contextLaser),
			",",
			Environment.NewLine,
			"    \"homeButton\": ",
			JsonHelper.GetJson(this.homeButton),
			",",
			Environment.NewLine,
			"    \"helpDialog\": ",
			JsonHelper.GetJson(this.helpDialog),
			",",
			Environment.NewLine,
			"    \"createArea\": ",
			JsonHelper.GetJson(this.createArea),
			",",
			Environment.NewLine,
			"    \"visitedAreasList\": ",
			JsonHelper.GetJson(this.visitedAreasList),
			",",
			Environment.NewLine,
			"    \"createdAreasList\": ",
			JsonHelper.GetJson(this.createdAreasList),
			",",
			Environment.NewLine,
			"    \"favoritedAreasList\": ",
			JsonHelper.GetJson(this.favoritedAreasList),
			",",
			Environment.NewLine,
			"    \"webBrowsing\": ",
			JsonHelper.GetJson(this.webBrowsing),
			",",
			Environment.NewLine,
			"    \"hapticPulse\": ",
			JsonHelper.GetJson(this.hapticPulse),
			",",
			Environment.NewLine,
			"    \"scriptsAsEditor\": ",
			JsonHelper.GetJson(this.scriptsAsEditor),
			",",
			Environment.NewLine,
			"}",
			Environment.NewLine
		});
		File.WriteAllText(this.jsonFilePath, text);
	}

	// Token: 0x0400168B RID: 5771
	public bool ringMenu = true;

	// Token: 0x0400168C RID: 5772
	public bool createThings = true;

	// Token: 0x0400168D RID: 5773
	public bool changeThings = true;

	// Token: 0x0400168E RID: 5774
	public bool areaDialog = true;

	// Token: 0x0400168F RID: 5775
	public bool forums = true;

	// Token: 0x04001690 RID: 5776
	public bool friendsDialog = true;

	// Token: 0x04001691 RID: 5777
	public bool areasDialog = true;

	// Token: 0x04001692 RID: 5778
	public bool ownProfileDialog = true;

	// Token: 0x04001693 RID: 5779
	public bool profileDialog = true;

	// Token: 0x04001694 RID: 5780
	public bool quitButton = true;

	// Token: 0x04001695 RID: 5781
	public bool pingAlerts = true;

	// Token: 0x04001696 RID: 5782
	public bool newbornAlerts = true;

	// Token: 0x04001697 RID: 5783
	public bool teleportLaser = true;

	// Token: 0x04001698 RID: 5784
	public bool contextLaser = true;

	// Token: 0x04001699 RID: 5785
	public bool homeButton = true;

	// Token: 0x0400169A RID: 5786
	public bool helpDialog = true;

	// Token: 0x0400169B RID: 5787
	public bool createArea = true;

	// Token: 0x0400169C RID: 5788
	public bool visitedAreasList = true;

	// Token: 0x0400169D RID: 5789
	public bool createdAreasList = true;

	// Token: 0x0400169E RID: 5790
	public bool favoritedAreasList = true;

	// Token: 0x0400169F RID: 5791
	public bool webBrowsing = true;

	// Token: 0x040016A0 RID: 5792
	public bool hapticPulse = true;

	// Token: 0x040016A1 RID: 5793
	public bool scriptsAsEditor = true;

	// Token: 0x040016A2 RID: 5794
	private string jsonFilePath = string.Empty;

	// Token: 0x040016A3 RID: 5795
	private bool foundMissingProperties;
}
