namespace ZengGe.StardewValleyMod.BiggerChest;

public sealed class ModConfig
{
	/// <summary>
	/// 总共多少行
	/// </summary>
	public int Rows { get; set; }
	/// <summary>
	/// 总共多少列
	/// </summary>
	public int Columns { get; set; }

	public ModConfig()
	{
		Reset();
	}

	public void Check()
	{
		if (Rows < 1) Rows = 1;
		else if (Rows > 9) Rows = 9;

		if (Columns < 1) Columns = 1;
		else if (Columns > 20) Columns = 20;
	}

	public void Reset()
	{
		Rows = 5;
		Columns = 14;
	}
}
