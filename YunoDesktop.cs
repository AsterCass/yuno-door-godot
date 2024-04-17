using Godot;
using System;

public partial class YunoDesktop : Node
{
}

namespace ServerInfo
{
	public static class ServerBase
	{
		public static string Address { get; } = "https://api.astercasc.com/";

		public static string KotomiAddress { get; } = Address + "kotomi/";
	}
}
