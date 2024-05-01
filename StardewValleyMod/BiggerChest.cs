using StardewValley.Menus;
using StardewValley.Objects;
using static StardewValley.Menus.InventoryMenu;
using static StardewValley.Menus.ItemGrabMenu;

namespace ZengGe.StardewValleyMod.BiggerChest;

#region 大箱子类
/// <summary>
/// 大箱子类
/// </summary>
public class BiggerChest
{
	private const int BlockWidth = 64;
	private const int StartYPos = 40;
	private static Vector2 PreDrawBoxPos = new();

	[HarmonyPostfix]
	public static void GetCapacity(Chest __instance, ref int __result)
	{
		__result = GlobalVar.Config.Rows * GlobalVar.Config.Columns;
	}

	[HarmonyFinalizer]
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

		// 匹配创建箱子
		GlobalVar.SMAPIHarmonyLib.Patch(
			original: AccessTools.Constructor(typeof(ItemGrabMenu), types),
			finalizer: new HarmonyMethod(AccessTools.Method(typeof(BiggerChest), nameof(CreateChestMenu)))
		);

		// 匹配箱子抓取道具
		GlobalVar.SMAPIHarmonyLib.Patch(
			original: AccessTools.Method(typeof(ItemGrabMenu), nameof(ItemGrabMenu.RepositionSideButtons)),
			finalizer: new HarmonyMethod(AccessTools.Method(typeof(BiggerChest), nameof(UpdateUIPosition)))
		);

		GlobalVar.SMAPIHarmonyLib.Patch(
			original: AccessTools.Method(typeof(ItemGrabMenu), nameof(ItemGrabMenu.draw), new Type[] { typeof(SpriteBatch) }),
			prefix: new HarmonyMethod(AccessTools.Method(typeof(BiggerChest), nameof(PredrawChest))),
			finalizer: new HarmonyMethod(AccessTools.Method(typeof(BiggerChest), nameof(AfterdrawChest)))
		);

		// 匹配箱子最大格数
		GlobalVar.SMAPIHarmonyLib.Patch(
			original: AccessTools.Method(typeof(Chest), nameof(Chest.GetActualCapacity)),
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
		var width = BlockWidth * GlobalVar.Config.Columns;
		var xPos = Game1.uiViewport.Width / 2 - width / 2;

		UpdateUIPosition(menu);

		menu.ItemsToGrabMenu = new InventoryMenu(
			xPos, StartYPos + 150, menu.ItemsToGrabMenu.playerInventory,
			menu.ItemsToGrabMenu.actualInventory, menu.ItemsToGrabMenu.highlightMethod,
			GlobalVar.Config.Rows * GlobalVar.Config.Columns, GlobalVar.Config.Rows,
			menu.ItemsToGrabMenu.horizontalGap, menu.ItemsToGrabMenu.verticalGap, menu.ItemsToGrabMenu.drawSlots
		);
		menu.ItemsToGrabMenu.capacity = GlobalVar.Config.Rows * GlobalVar.Config.Columns;

		UpdateUIPosition(menu);

		menu.storageSpaceTopBorderOffset = 20;

		// 这里是主菜单的位置
		menu.yPositionOnScreen = 200;
		menu.inventory.SetPosition(menu.inventory.xPositionOnScreen, GlobalVar.Config.Rows * BlockWidth + 263);
	}
	#endregion 更新箱子Menu

	#region 更新选择颜色工具位置
	[HarmonyFinalizer]
	public static void UpdateUIPosition(ItemGrabMenu __instance)
	{
		var rightPos = Game1.uiViewport.Width / 2 + (BlockWidth * GlobalVar.Config.Columns) / 2 + BlockWidth;
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
		yPos += BlockWidth * 2;

		__instance.trashCan?.setPosition(rightPos, yPos);
		yPos += BlockWidth * 3;

		__instance.okButton?.setPosition(rightPos, yPos);
	}
	#endregion 更新选择颜色工具位置

	#region 绘制箱子菜单前
	[HarmonyPrefix]
	public static void PredrawChest(ItemGrabMenu __instance, SpriteBatch b)
	{
		PreDrawBoxPos.Y = __instance.yPositionOnScreen;
		__instance.yPositionOnScreen = GlobalVar.Config.Rows * BlockWidth - 50;
	}
	#endregion 绘制箱子菜单前

	#region 绘制箱子菜单后
	[HarmonyFinalizer]
	public static void AfterdrawChest(ItemGrabMenu __instance, SpriteBatch b)
	{
		__instance.yPositionOnScreen = (int)PreDrawBoxPos.Y;
	}
	#endregion 绘制箱子菜单后

}

#endregion 大箱子类