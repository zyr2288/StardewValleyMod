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

namespace BiggerChest;

public class BiggerChest
{
	public const int Rows = 6;
	public const int Columns = 14;

	[HarmonyPostfix]
	public static void ShowMenu(Chest __instance)
	{
		if (Game1.activeClickableMenu is ItemGrabMenu menu)
		{
			menu.ItemsToGrabMenu.capacity = Rows * Columns;
			menu.ItemsToGrabMenu.rows = Rows;
		}
	}

	[HarmonyPostfix]
	public static void GetCapacity(Chest __instance, ref int __result)
	{
		__result = Rows * Columns;
	}

	public static void Install()
	{
		var tempChest = new Chest();

		GlobalVar.SMAPIHarmonyLib.Patch(
			original: AccessTools.Method(typeof(Chest), nameof(tempChest.ShowMenu)),
			postfix: new HarmonyMethod(AccessTools.Method(typeof(BiggerChest), nameof(ShowMenu)))
		);

		GlobalVar.SMAPIHarmonyLib.Patch(
			original: AccessTools.Method(typeof(Chest), nameof(tempChest.GetActualCapacity)),
			postfix: new HarmonyMethod(AccessTools.Method(typeof(BiggerChest), nameof(GetCapacity)))
		);
	}
	
}

public class BiggerChestMenu : InventoryMenu
{
	public BiggerChestMenu(InventoryMenu baseMenu) :
		base(baseMenu.xPositionOnScreen, yPosition, playerInventory, actualInventory, highlightMethod, capacity, rows, horizontalGap, verticalGap, drawSlots)
	{
		if (source == 1 && sourceItem is Chest chest3 && chest3.GetActualCapacity() != 36)
		{
			int actualCapacity = chest3.GetActualCapacity();
			int num = ((actualCapacity >= 70) ? 5 : 3);
			if (actualCapacity < 9)
			{
				num = 1;
			}

			int num2 = 64 * (actualCapacity / num);
			ItemsToGrabMenu = new InventoryMenu(Game1.uiViewport.Width / 2 - num2 / 2, yPositionOnScreen + ((actualCapacity < 70) ? 64 : (-21)), playerInventory: false, inventory, highlightFunction, actualCapacity, num);
			if (chest3.SpecialChestType == Chest.SpecialChestTypes.MiniShippingBin)
			{
				base.inventory.moveItemSound = "Ship";
			}

			if (num > 3)
			{
				yPositionOnScreen += 42;
				base.inventory.SetPosition(base.inventory.xPositionOnScreen, base.inventory.yPositionOnScreen + 38 + 4);
				ItemsToGrabMenu.SetPosition(ItemsToGrabMenu.xPositionOnScreen - 32 + 8, ItemsToGrabMenu.yPositionOnScreen);
				storageSpaceTopBorderOffset = 20;
				trashCan.bounds.X = ItemsToGrabMenu.width + ItemsToGrabMenu.xPositionOnScreen + IClickableMenu.borderWidth * 2;
				okButton.bounds.X = ItemsToGrabMenu.width + ItemsToGrabMenu.xPositionOnScreen + IClickableMenu.borderWidth * 2;
			}
		}
		else
		{
			ItemsToGrabMenu = new InventoryMenu(xPositionOnScreen + 32, yPositionOnScreen, playerInventory: false, inventory, highlightFunction);
		}
	}
}