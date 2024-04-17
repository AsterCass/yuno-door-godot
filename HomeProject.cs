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
		articleRequest.Request(ServerInfo.ServerBase.KotomiAddress + "/article/list?articleType=1&offset=0&limit=3");

		HttpRequest lifeRequest = GetNode<HttpRequest>("LifeRequest");
		lifeRequest.RequestCompleted += OnLiftReqCompleted;
		lifeRequest.Request(ServerInfo.ServerBase.KotomiAddress + "/article/list?articleType=2&offset=0&limit=3");
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
				var thisLabel = instance.GetNode<Label>("CardContainer/CardBgContainer/Title");
				thisLabel.Text = objDic["articleTitle"].AsString();
				articleContainer.AddChild(instance);
			}
		}
	}
}
