using HarmonyLib;
using StardewModdingAPI;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.Menus.CharacterCustomization;
using static StardewValley.Menus.InventoryMenu;

namespace BiggerChest;



#region 大箱子类
/// <summary>
/// 大箱子类
/// </summary>
public class BiggerChest
{
	public const int Rows = 6;
	public const int Columns = 16;
	public const int Capacity = Rows * Columns;

	[HarmonyPostfix]
	public static void GetCapacity(Chest __instance, ref int __result)
	{
		__result = Capacity;
	}

	[HarmonyFinalizer]
	public static void CreateChestMenu(InventoryMenu __instance, int xPosition, int yPosition, bool playerInventory, IList<Item> actualInventory)
	{
		Console.WriteLine(__instance);

		// 如果是玩家库存
		if (__instance.playerInventory)
			return;

		__instance.rows = Rows;
		__instance.capacity = Capacity;
	}

	public static void Install()
	{
		// int xPosition, int yPosition, bool playerInventory, IList< Item > actualInventory = null, highlightThisItem highlightMethod = null,
		// int capacity = -1, int rows = 3, int horizontalGap = 0, int verticalGap = 0, bool drawSlots = true

		// 不加不能匹配 InventoryMenu
		var types = new Type[]
		{
			typeof(int), typeof(int), typeof(bool), typeof(IList<Item>), typeof(highlightThisItem),
			typeof(int), typeof(int), typeof(int), typeof(int), typeof(bool)
		};

		GlobalVar.SMAPIHarmonyLib.Patch(
			original: AccessTools.Constructor(typeof(InventoryMenu), types),
			finalizer: new HarmonyMethod(AccessTools.Method(typeof(BiggerChest), nameof(CreateChestMenu)))
		);


		//GlobalVar.SMAPIHarmonyLib.Patch(
		//	original: AccessTools.Method(typeof(Chest), nameof(tempChest.ShowMenu)),
		//	postfix: new HarmonyMethod(AccessTools.Method(typeof(BiggerChest), nameof(ShowMenu)))
		//);

		var tempChest = new Chest();
		GlobalVar.SMAPIHarmonyLib.Patch(
			original: AccessTools.Method(typeof(Chest), nameof(tempChest.GetActualCapacity)),
			postfix: new HarmonyMethod(AccessTools.Method(typeof(BiggerChest), nameof(GetCapacity)))
		);
	}
	
}
#endregion 大箱子类

//public class BiggerChestMenu : InventoryMenu
//{
//	public const int BlockWidth = 64;

//	public BiggerChestMenu(ItemGrabMenu menu) :
//		base(menu.ItemsToGrabMenu.xPositionOnScreen, menu.ItemsToGrabMenu.yPositionOnScreen,
//		menu.ItemsToGrabMenu.playerInventory, menu.ItemsToGrabMenu.actualInventory, menu.ItemsToGrabMenu.highlightMethod,
//		menu.ItemsToGrabMenu.capacity, menu.ItemsToGrabMenu.rows, 
//		menu.ItemsToGrabMenu.horizontalGap, menu.ItemsToGrabMenu.verticalGap, menu.ItemsToGrabMenu.drawSlots)
//	{
//		int width = BlockWidth * BiggerChest.Columns;
//		// xPositionOnScreen = Game1.uiViewport.Width / 2 - width / 2;
//		// yPositionOnScreen = yPositionOnScreen + ((actualCapacity < 70) ? 64 : (-21);

//		// menu.SetPosition(menu.inventory.xPositionOnScreen, menu.inventory.yPositionOnScreen);
//		// SetPosition(xPositionOnScreen, yPositionOnScreen + 38 + 4);
//		// menu.ItemsToGrabMenu.SetPosition(menu.ItemsToGrabMenu.xPositionOnScreen, menu.ItemsToGrabMenu.yPositionOnScreen);
//		menu.storageSpaceTopBorderOffset = 20;

//		var xPos = width + menu.xPositionOnScreen;
//		menu.trashCan.bounds.X = xPos;
//		menu.okButton.bounds.X = xPos;
//	}
//}