public static class ClientPlayerInfoExtensions
{
	public static string GetChatName(this ClientPlayerInfo playerInfo, bool closeColorTag)
	{
		return $"{ClientPlayerInfo.GetColorPrefix(playerInfo.index_)}{playerInfo.username_}{(closeColorTag ? "[-]" : string.Empty)}";
	}
}
