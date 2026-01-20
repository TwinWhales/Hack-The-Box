using System;
using System.Collections.Generic;

namespace DInjector
{
	// Token: 0x02000017 RID: 23
	internal class ArgumentParser
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00005254 File Offset: 0x00003454
		public static Dictionary<string, string> Parse(IEnumerable<string> argv)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (string text in argv)
			{
				int num = text.IndexOf(':');
				if (num > 0)
				{
					dictionary[text.Substring(0, num)] = text.Substring(num + 1);
				}
				else
				{
					num = text.IndexOf('=');
					if (num > 0)
					{
						dictionary[text.Substring(0, num)] = text.Substring(num + 1);
					}
					else
					{
						dictionary[text] = string.Empty;
					}
				}
			}
			return dictionary;
		}
	}
}
