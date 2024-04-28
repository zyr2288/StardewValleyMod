using StardewModdingAPI.Events;
using StardewValley.Objects;
using StardewValley;
using StardewValley.Menus;

namespace BiggerChest;

public class BiggerChest : ItemGrabMenu
{
	public BiggerChest(IList<Item> inventory, object context = null) : base(inventory, context)
	{
	}

	public BiggerChest(IList<Item> inventory, bool reverseGrab, bool showReceivingMenu, InventoryMenu.highlightThisItem highlightFunction, behaviorOnItemSelect behaviorOnItemSelectFunction, string message, behaviorOnItemSelect behaviorOnItemGrab = null, bool snapToBottom = false, bool canBeExitedWithKey = false, bool playRightClickSound = true, bool allowRightClick = true, bool showOrganizeButton = false, int source = 0, Item sourceItem = null, int whichSpecialButton = -1, object context = null, ItemExitBehavior heldItemExitBehavior = ItemExitBehavior.ReturnToPlayer, bool allowExitWithHeldItem = false) : 
		base(inventory, reverseGrab, showReceivingMenu, highlightFunction, behaviorOnItemSelectFunction, message, behaviorOnItemGrab, snapToBottom, canBeExitedWithKey, playRightClickSound, allowRightClick, showOrganizeButton, source, sourceItem, whichSpecialButton, context, heldItemExitBehavior, allowExitWithHeldItem)
	{
	}
}
