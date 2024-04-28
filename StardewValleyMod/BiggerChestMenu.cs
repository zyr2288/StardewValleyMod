using StardewModdingAPI.Events;
using StardewValley.Objects;
using StardewValley;
using StardewValley.Menus;

namespace BiggerChest;

public class BiggerChestMenu : InventoryMenu
{
	public BiggerChestMenu(int xPosition, int yPosition, bool playerInventory, IList<Item> actualInventory = null, highlightThisItem highlightMethod = null, int capacity = -1, int rows = 3, int horizontalGap = 0, int verticalGap = 0, bool drawSlots = true) : 
		base(xPosition, yPosition, playerInventory, actualInventory, highlightMethod, capacity, rows, horizontalGap, verticalGap, drawSlots)
	{
		for (int i = 0; i < rows; i++) { }
	}

	public override void releaseLeftClick(int x, int y)
	{
		base.releaseLeftClick(x, y);
	}

	public override void leftClickHeld(int x, int y)
	{
		base.leftClickHeld(x, y);
	}

	public override void clickAway()
	{
		base.clickAway();
	}

	public Item tryToAddItem(Item toPlace, string sound = "coin")
	{
		if (toPlace == null)
		{
			return null;
		}

		int stack = toPlace.Stack;
		foreach (ClickableComponent item in inventory)
		{
			int num = Convert.ToInt32(item.name);
			if (num >= actualInventory.Count || actualInventory[num] == null || !highlightMethod(actualInventory[num]) || !actualInventory[num].canStackWith(toPlace))
			{
				continue;
			}

			toPlace.Stack = actualInventory[num].addToStack(toPlace);
			if (toPlace.Stack <= 0)
			{
				try
				{
					Game1.playSound(sound);
					onAddItem?.Invoke(toPlace, playerInventory ? Game1.player : null);
				}
				catch (Exception)
				{
				}

				return null;
			}
		}

		foreach (ClickableComponent item2 in inventory)
		{
			int num2 = Convert.ToInt32(item2.name);
			if (num2 >= actualInventory.Count || (actualInventory[num2] != null && !highlightMethod(actualInventory[num2])) || actualInventory[num2] != null)
			{
				continue;
			}

			if (!string.IsNullOrEmpty(sound))
			{
				try
				{
					Game1.playSound(sound);
				}
				catch (Exception)
				{
				}
			}

			return Utility.addItemToInventory(toPlace, num2, actualInventory, onAddItem);
		}

		if (toPlace.Stack < stack)
		{
			Game1.playSound(sound);
		}

		return toPlace;
	}

	public Item leftClick(int x, int y, Item toPlace, bool playSound = true)
	{
		foreach (ClickableComponent item in inventory)
		{
			if (!item.containsPoint(x, y))
			{
				continue;
			}

			int num = Convert.ToInt32(item.name);
			if (num >= actualInventory.Count || (actualInventory[num] != null && !highlightMethod(actualInventory[num]) && !actualInventory[num].canStackWith(toPlace)))
			{
				continue;
			}

			if (actualInventory[num] != null)
			{
				if (toPlace != null)
				{
					if (playSound)
					{
						Game1.playSound("stoneStep");
					}

					return Utility.addItemToInventory(toPlace, num, actualInventory, onAddItem);
				}

				if (playSound)
				{
					Game1.playSound(moveItemSound);
				}

				return Utility.removeItemFromInventory(num, actualInventory);
			}

			if (toPlace != null)
			{
				if (playSound)
				{
					Game1.playSound("stoneStep");
				}

				return Utility.addItemToInventory(toPlace, num, actualInventory, onAddItem);
			}
		}

		return toPlace;
	}
}
