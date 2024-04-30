using HarmonyLib;
using StardewModdingAPI;
using StardewValley.Inventories;
using StardewValley.Menus;
using StardewValley.Network;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.Menus.InventoryMenu;
using static StardewValley.Menus.ItemGrabMenu;

namespace BiggerChest;


#region 大箱子类
/// <summary>
/// 大箱子类
/// </summary>
public class BiggerChest
{
	private const int BlockWidth = 64;
	private const int StartYPos = 30;

	[HarmonyPostfix]
	public static void GetCapacity(Chest __instance, ref int __result)
	{
		__result = Config.Rows * Config.Columns;
	}

	[HarmonyPostfix]
	public static void CreateChestMenu(ItemGrabMenu __instance)
	{
		UpdateMenu(__instance);
	}

	public static void Install()
	{
		// IList<Item> inventory, bool reverseGrab, bool showReceivingMenu, InventoryMenu.highlightThisItem highlightFunction,
		// behaviorOnItemSelect behaviorOnItemSelectFunction, string message, behaviorOnItemSelect behaviorOnItemGrab = null,
		// bool snapToBottom = false, bool canBeExitedWithKey = false, bool playRightClickSound = true, bool allowRightClick = true,
		// bool showOrganizeButton = false, int source = 0, Item sourceItem = null, int whichSpecialButton = -1, object context = null,
		// ItemExitBehavior heldItemExitBehavior = ItemExitBehavior.ReturnToPlayer, bool allowExitWithHeldItem = false

		// 不加不能匹配 InventoryMenu
		var types = new Type[]
		{
			typeof(IList<Item>), typeof(bool), typeof(bool), typeof(highlightThisItem), typeof(behaviorOnItemSelect),
			typeof(string), typeof(behaviorOnItemSelect), typeof(bool), typeof(bool), typeof(bool), typeof(bool),
			typeof(bool), typeof(int), typeof(Item), typeof(int), typeof(object), typeof(ItemExitBehavior), typeof(bool)
		};

		GlobalVar.SMAPIHarmonyLib.Patch(
			original: AccessTools.Constructor(typeof(ItemGrabMenu), types),
			postfix: new HarmonyMethod(AccessTools.Method(typeof(BiggerChest), nameof(CreateChestMenu)))
		);

		GlobalVar.SMAPIHarmonyLib.Patch(
			original: AccessTools.Method(typeof(ItemGrabMenu), nameof(ItemGrabMenu.setSourceItem)),
			postfix: new HarmonyMethod(AccessTools.Method(typeof(BiggerChest), nameof(UpdateUIPosition)))
		);

		// 匹配箱子最大格数
		var tempChest = new Chest();
		GlobalVar.SMAPIHarmonyLib.Patch(
			original: AccessTools.Method(typeof(Chest), nameof(tempChest.GetActualCapacity)),
			postfix: new HarmonyMethod(AccessTools.Method(typeof(BiggerChest), nameof(GetCapacity)))
		);
	}

	#region 更新箱子Menu
	/// <summary>
	/// 更新箱子Menu
	/// </summary>
	/// <param name="menu"></param>
	private static void UpdateMenu(ItemGrabMenu menu)
	{
		var width = BlockWidth * Config.Columns;
		var xPos = Game1.uiViewport.Width / 2 - width / 2;

		UpdateUIPosition(menu);

		menu.ItemsToGrabMenu = new InventoryMenu(
			xPos, StartYPos + BlockWidth * 2, menu.ItemsToGrabMenu.playerInventory,
			menu.ItemsToGrabMenu.actualInventory, menu.ItemsToGrabMenu.highlightMethod,
			Config.Rows * Config.Columns, Config.Rows,
			menu.ItemsToGrabMenu.horizontalGap, menu.ItemsToGrabMenu.verticalGap, menu.ItemsToGrabMenu.drawSlots
		);

		// __instance.ItemsToGrabMenu.xPositionOnScreen = Game1.uiViewport.Width / 2 - width / 2;
		UpdateUIPosition(menu);

		menu.storageSpaceTopBorderOffset = 20;
		menu.yPositionOnScreen = (Config.Rows - 1) * BlockWidth;
		menu.inventory.SetPosition(menu.inventory.xPositionOnScreen, menu.yPositionOnScreen + BlockWidth * 5 - 7);
	}
	#endregion 更新箱子Menu

	#region 更新选择颜色工具位置
	[HarmonyPostfix]
	public static void UpdateUIPosition(ItemGrabMenu __instance)
	{
		var rightPos = Game1.uiViewport.Width / 2 + (BlockWidth * Config.Columns) / 2 + BlockWidth;
		var yPos = StartYPos + BlockWidth * 2;

		void UpdateY() { yPos += BlockWidth + BlockWidth / 4; };

		if (__instance.chestColorPicker != null)
		{
			__instance.chestColorPicker.yPositionOnScreen = StartYPos;
		}

		__instance.colorPickerToggleButton?.setPosition(rightPos, yPos);
		UpdateY();
		__instance.fillStacksButton?.setPosition(rightPos, yPos);
		UpdateY();
		__instance.organizeButton?.setPosition(rightPos, yPos);
		UpdateY();
		__instance.trashCan?.setPosition(rightPos, yPos);
		yPos += BlockWidth * 2;
		__instance.okButton?.setPosition(rightPos, yPos);
	}
	#endregion 更新选择颜色工具位置

}

#endregion 大箱子类