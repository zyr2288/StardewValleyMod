using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiggerChest;

public class Config
{
	/// <summary>
	/// 总共多少行
	/// </summary>
	public static int Rows { get; set; } = 5;
	/// <summary>
	/// 总共多少列
	/// </summary>
	public static int Columns { get; set; } = 16;

	public void LoadConfig()
	{

	}
}
