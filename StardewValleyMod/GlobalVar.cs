using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiggerChest;

public class GlobalVar
{
	public static IModHelper SMAPIHelper { get; set; }
	public static IMonitor SMAPIMonitor { get; set; }
	public static HarmonyLib.Harmony SMAPIHarmonyLib { get; set; }
}
