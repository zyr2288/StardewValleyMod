global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using StardewModdingAPI;
global using StardewValley;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley.Menus;

namespace BiggerChest;

public class ModEntry : Mod
{
	public override void Entry(IModHelper helper)
	{
		GlobalVar.SMAPIHelper = helper;
		GlobalVar.SMAPIMonitor = Monitor;

		helper.Events.Display.RenderingActiveMenu += Display_RenderingActiveMenu;
	}

	private void Display_RenderingActiveMenu(object? sender, RenderingActiveMenuEventArgs e)
	{
		var menu = Game1.activeClickableMenu;
		if (menu is not ItemGrabMenu) 
			return;

		var chestMenu = (ItemGrabMenu)menu;
		if (chestMenu.ItemsToGrabMenu is BiggerChestMenu)
			return;

		chestMenu.ItemsToGrabMenu = new BiggerChestMenu(
			chestMenu.xPositionOnScreen,
			chestMenu.yPositionOnScreen,
			true,
			chestMenu.ItemsToGrabMenu.actualInventory,
			capacity: 48,
			rows: 4
		);

		//if (chestMenu.ItemsToGrabMenu.rows <= 3)
		//{
		//	chestMenu.ItemsToGrabMenu.capacity = 60;
		//	chestMenu.ItemsToGrabMenu.rows = 5;
		//}


		// chestMenu.ItemsToGrabMenu.rows
		//menu = new BiggerChestMenu(chestMenu.sourceItem)
		//menu.draw(e.SpriteBatch);
	}
}
