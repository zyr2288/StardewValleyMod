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

	}
}
