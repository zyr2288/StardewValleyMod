global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using StardewModdingAPI;
global using StardewValley;
global using HarmonyLib;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley.Menus;
using StardewValley.Objects;

namespace ZengGe.StardewValleyMod.BiggerChest;

public class ModEntry : Mod
{
	public override void Entry(IModHelper helper)
	{
		GlobalVar.SMAPIHelper = helper;
		GlobalVar.SMAPIMonitor = Monitor;
		GlobalVar.SMAPIHarmonyLib = new Harmony(ModManifest.UniqueID);

		GlobalVar.Config = helper.ReadConfig<ModConfig>();
		GlobalVar.Config.Check();

		BiggerChest.Install();

		// 游戏载入，注册Mod设置功能
		helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
	}

	#region 注册Mod设置功能
	/// <summary>
	/// 游戏载入，注册Mod设置功能
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void GameLoop_GameLaunched(object? sender, GameLaunchedEventArgs e)
	{
		var configMenu = GlobalVar.SMAPIHelper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
		if (configMenu == null)
			return;

		configMenu.Register(ModManifest, GlobalVar.Config.Reset,
			() => {
				Helper.WriteConfig(GlobalVar.Config);
			}
		);

		configMenu.AddNumberOption(ModManifest,
			() => GlobalVar.Config.Rows,
			(int value) => {
				if (value < 1) value = 1;
				else if (value > 9) value = 9;

				GlobalVar.Config.Rows = value;
			},
			() => GlobalVar.SMAPIHelper.Translation.Get("config.chestRows"),
			min: 1, max: 9
		);

		configMenu.AddNumberOption(ModManifest,
			() => GlobalVar.Config.Columns,
			(int value) => {
				if (value < 1) value = 1;
				else if (value > 20) value = 20;

				GlobalVar.Config.Columns = value;
			},
			() => GlobalVar.SMAPIHelper.Translation.Get("config.chestColumns"),
			min: 1, max: 20
		);
	}
	#endregion 注册Mod设置功能

}