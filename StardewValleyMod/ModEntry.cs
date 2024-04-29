global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using StardewModdingAPI;
global using StardewValley;
global using HarmonyLib;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley.Menus;
using StardewValley.Objects;

namespace BiggerChest;

public class ModEntry : Mod
{
	public override void Entry(IModHelper helper)
	{
		GlobalVar.SMAPIHelper = helper;
		GlobalVar.SMAPIMonitor = Monitor;
		GlobalVar.SMAPIHarmonyLib = new Harmony(ModManifest.UniqueID);

		BiggerChest.Install();

		//GlobalVar.SMAPIHarmonyLib.Patch
		//(
		//	original: AccessTools.Method(typeof(Chest), "ShowMenu"),
		//	prefix: new HarmonyMethod(typeof(BiggerChest), nameof(BiggerChest))
		//);

		// var info = helper.Reflection.GetField<Chest>(typeof(Chest), "capacity");


		//info.SetValue(typeof(Chest), 70);

		// helper.Events.Display.RenderedHud += Display_RenderedHud;
	}

	private void Display_RenderedHud(object? sender, RenderedHudEventArgs e)
	{
		var menu = Game1.activeClickableMenu;
		if (menu is not ItemGrabMenu)
			return;

		var chestMenu = (ItemGrabMenu)menu;
		var chest = (Chest)chestMenu.context;
		if (chest is BiggerChest)
			return;

		//var bigger = new BiggerChest
		//{

		//};

		//bigger.Items.AddRange(chest.Items);

		//chestMenu.context = bigger;

		////var chest = (Chest)chestMenu.context;
		////chest.Items.CountItemStacks();

		//var biggerMenu = new BiggerChestMenu(
		//	chestMenu.ItemsToGrabMenu.xPositionOnScreen,
		//	chestMenu.ItemsToGrabMenu.yPositionOnScreen,
		//	false,
		//	chestMenu.ItemsToGrabMenu.actualInventory,
		//	capacity: BiggerChest.TotalCapacity,
		//	rows: BiggerChest.Rows
		//);

		//////if (chestMenu.ItemsToGrabMenu.rows <= 3)
		//////{
		//////	chestMenu.ItemsToGrabMenu.capacity = 60;
		//////	chestMenu.ItemsToGrabMenu.rows = 5;
		//////}

		//if (BiggerChest.Rows > 3)
		//{
		//	var pos = (BiggerChest.Rows - 3) * 24;
		//	chestMenu.inventory.movePosition(0, pos);

		//	//	//for (var i = bigger.allClickableComponents.Count; i < capacity; i++)
		//	//	//{
		//	//	//	var rec = new Rectangle();
		//	//	//	var com = new ClickableComponent(rec, i.ToString());
		//	//	//	bigger.allClickableComponents.Insert(0, com);
		//	//	//}
		//}
		//chestMenu.ItemsToGrabMenu = biggerMenu;
	}

	private void Display_Rendering(object? sender, RenderingEventArgs e)
	{

	}
}