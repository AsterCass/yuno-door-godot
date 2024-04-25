using Godot;
using System;
using System.Text;
using System.Text.Encodings.Web;

public partial class HomeProject : VBoxContainer
{
	public override void _Ready()
	{
		HttpRequest articleRequest = GetNode<HttpRequest>("ArticleRequest");
		articleRequest.RequestCompleted += OnArticleReqCompleted;
		articleRequest.Request(ServerInfo.ServerBase.KotomiAddress + "/article/list?articleType=1&offset=0&limit=10");

		HttpRequest lifeRequest = GetNode<HttpRequest>("LifeRequest");
		lifeRequest.RequestCompleted += OnLiftReqCompleted;
		lifeRequest.Request(ServerInfo.ServerBase.KotomiAddress + "/article/list?articleType=2&offset=0&limit=10");
	}

	private void OnArticleReqCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		rentCard(body, "HomeArticleContainer");
	}

	private void OnLiftReqCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		rentCard(body, "HomeLifeContainer");
	}

	private void rentCard(byte[] body, string containerName)
	{
		Godot.Collections.Dictionary json = Json.ParseString(Encoding.UTF8.GetString(body)).AsGodotDictionary();
		var articleList = json["data"].AsGodotArray();
		if (articleList.Count > 0)
		{
			var articleContainer = GetNode<HFlowContainer>(containerName);
			var scene = GD.Load<PackedScene>("res://ArticleSimple.tscn");
			for (int count = 0; count < articleList.Count; count++)
			{
				var objDic = articleList[count].AsGodotDictionary();
				var instance = scene.Instantiate();
				var thisLabel = instance.GetNode<Label>("Card/CardContainer/CardBgContainer/Title");
				thisLabel.Text = objDic["articleTitle"].AsString();
				var thisContent = instance.GetNode<Label>("Card/CardContentContainer/CardContent");
				thisContent.Text = objDic["articleBrief"].AsString().Replace("\n", "") + "...\n";
				var thisBtn = instance.GetNode<Button>("Card/CardContentContainer/Go");
				thisBtn.Pressed += () => onPressToReadMore(objDic["id"].AsString());
				articleContainer.AddChild(instance);
			}
		}
	}

	private void onPressToReadMore(string articleId)
	{
		GD.Print(articleId);
		OS.Alert("不支持当前功能，请移步astercasc.com体验完整功能");
	}
}
