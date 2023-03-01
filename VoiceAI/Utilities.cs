using System;
using System.Windows.Forms;
using Microsoft.Win32;
using VoiceAIGui.Properties;

// Token: 0x02000015 RID: 21
internal class Utilities
{
	// Token: 0x0600014C RID: 332 RVA: 0x00008505 File Offset: 0x00006705
	public static Utilities Singleton()
	{
		if (Utilities.instance == null)
		{
			Utilities.instance = new Utilities();
		}
		return Utilities.instance;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x0000851D File Offset: 0x0000671D
	private Utilities()
	{
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00008525 File Offset: 0x00006725
	public bool GetStartOnBoot()
	{
		return Settings.Default.StartOnBoot;
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00008534 File Offset: 0x00006734
	public void SetStartOnBoot(bool startOnBoot)
	{
		if (startOnBoot)
		{
			Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true).SetValue("Voice.ai", Application.ExecutablePath);
		}
		else
		{
			Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true).DeleteValue("Voice.ai", false);
		}
		Settings.Default.StartOnBoot = startOnBoot;
		Settings.Default.Save();
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00008595 File Offset: 0x00006795
	public bool GetMinimizeOnClose()
	{
		return Settings.Default.MinimizeOnClose;
	}

	// Token: 0x06000151 RID: 337 RVA: 0x000085A1 File Offset: 0x000067A1
	public void SetMinimizeOnClose(bool minimizeOnClose)
	{
		Settings.Default.MinimizeOnClose = minimizeOnClose;
		Settings.Default.Save();
	}

	// Token: 0x06000152 RID: 338 RVA: 0x000085B8 File Offset: 0x000067B8
	public bool GetStartInTray()
	{
		return Settings.Default.StartInTray;
	}

	// Token: 0x06000153 RID: 339 RVA: 0x000085C4 File Offset: 0x000067C4
	public void SetStartInTray(bool startInTray)
	{
		Settings.Default.StartInTray = startInTray;
		Settings.Default.Save();
	}

	// Token: 0x06000154 RID: 340 RVA: 0x000085DB File Offset: 0x000067DB
	public bool GetAutoTrain()
	{
		return Settings.Default.AutoTrain;
	}

	// Token: 0x06000155 RID: 341 RVA: 0x000085E7 File Offset: 0x000067E7
	public void SetAutoTrain(bool enabled)
	{
		Settings.Default.AutoTrain = enabled;
		Settings.Default.Save();
	}

	// Token: 0x06000156 RID: 342 RVA: 0x000085FE File Offset: 0x000067FE
	public bool GetPromptForUpdate()
	{
		return Settings.Default.PromptForUpdate;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x0000860A File Offset: 0x0000680A
	public void SetPromptForUpdate(bool enabled)
	{
		Settings.Default.PromptForUpdate = enabled;
		Settings.Default.Save();
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00008621 File Offset: 0x00006821
	public bool GetVolumeBoost()
	{
		return Settings.Default.VolumeBoost;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0000862D File Offset: 0x0000682D
	public void SetVolumeBoost(bool enabled)
	{
		Settings.Default.VolumeBoost = enabled;
		Settings.Default.Save();
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00008644 File Offset: 0x00006844
	public bool GetReduceBreath()
	{
		return Settings.Default.ReduceBreath;
	}

	// Token: 0x0600015B RID: 347 RVA: 0x00008650 File Offset: 0x00006850
	public void SetReduceBreath(bool enabled)
	{
		Settings.Default.ReduceBreath = enabled;
		Settings.Default.Save();
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00008667 File Offset: 0x00006867
	public float GetGateOutputValue()
	{
		return Settings.Default.GateOutputValue;
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00008673 File Offset: 0x00006873
	public void SetGateOuputValue(float value)
	{
		Settings.Default.GateOutputValue = value;
		Settings.Default.Save();
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000868A File Offset: 0x0000688A
	public bool GetNoteSnap()
	{
		return Settings.Default.SnapNote;
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00008696 File Offset: 0x00006896
	public void SetNoteSnap(bool enabled)
	{
		Settings.Default.SnapNote = enabled;
		Settings.Default.Save();
	}

	// Token: 0x06000160 RID: 352 RVA: 0x000086AD File Offset: 0x000068AD
	public float GetSemitoneShift()
	{
		return Settings.Default.SemitoneShift;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x000086B9 File Offset: 0x000068B9
	public void SetSemitoneShift(float value)
	{
		Settings.Default.SemitoneShift = value;
		Settings.Default.Save();
	}

	// Token: 0x06000162 RID: 354 RVA: 0x000086D0 File Offset: 0x000068D0
	public bool GetAudioPassthruEnabled()
	{
		return Settings.Default.PassThruEnabled;
	}

	// Token: 0x06000163 RID: 355 RVA: 0x000086DC File Offset: 0x000068DC
	public void SetAudioPassthruEnabled(bool enabled)
	{
		Settings.Default.PassThruEnabled = enabled;
		Settings.Default.Save();
	}

	// Token: 0x06000164 RID: 356 RVA: 0x000086F4 File Offset: 0x000068F4
	public object SetSettingValue(string name, float value)
	{
		bool flag = value != 0f;
		string text = name.ToLower();
		uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
		if (num <= 1306159328U)
		{
			if (num <= 533801063U)
			{
				if (num != 87818708U)
				{
					if (num != 215202713U)
					{
						if (num == 533801063U)
						{
							if (text == "promptforupdate")
							{
								this.SetPromptForUpdate(flag);
								goto IL_2B4;
							}
						}
					}
					else if (text == "audiopassthru")
					{
						this.SetAudioPassthruEnabled(flag);
						goto IL_2B4;
					}
				}
				else if (text == "startintray")
				{
					this.SetStartInTray(flag);
					goto IL_2B4;
				}
			}
			else if (num <= 677775829U)
			{
				if (num != 543053314U)
				{
					if (num == 677775829U)
					{
						if (text == "snapnote")
						{
							this.SetNoteSnap(flag);
							goto IL_2B4;
						}
					}
				}
				else if (text == "volumeboost")
				{
					this.SetVolumeBoost(flag);
					goto IL_2B4;
				}
			}
			else if (num != 1019237139U)
			{
				if (num == 1306159328U)
				{
					if (text == "voicemean")
					{
						VoiceChanger.singleton(0L).LockVoiceMean(value);
						goto IL_2B4;
					}
				}
			}
			else if (text == "gateoutput")
			{
				this.SetGateOuputValue(value);
				goto IL_2B4;
			}
		}
		else if (num <= 3430357409U)
		{
			if (num != 1635173897U)
			{
				if (num != 2419558806U)
				{
					if (num == 3430357409U)
					{
						if (text == "reducebreath")
						{
							this.SetReduceBreath(flag);
							goto IL_2B4;
						}
					}
				}
				else if (text == "outputvolume")
				{
					Settings.Default.OutputVolume = value;
					VoiceChanger.singleton(0L).SetOutputVolume(value);
					goto IL_2B4;
				}
			}
			else if (text == "keepalive")
			{
				PowerRequester.EnableConstantDisplayAndPower(flag);
				goto IL_2B4;
			}
		}
		else if (num <= 3881872140U)
		{
			if (num != 3847145228U)
			{
				if (num == 3881872140U)
				{
					if (text == "startwithwindows")
					{
						this.SetStartOnBoot(flag);
						goto IL_2B4;
					}
				}
			}
			else if (text == "autotrain")
			{
				this.SetAutoTrain(flag);
				goto IL_2B4;
			}
		}
		else if (num != 3899524373U)
		{
			if (num == 4166805404U)
			{
				if (text == "minimizeonclose")
				{
					this.SetMinimizeOnClose(flag);
					goto IL_2B4;
				}
			}
		}
		else if (text == "semitoneshift")
		{
			this.SetSemitoneShift(value);
			goto IL_2B4;
		}
		return null;
		IL_2B4:
		return true;
	}

	// Token: 0x06000165 RID: 357 RVA: 0x000089BC File Offset: 0x00006BBC
	public static int CompareVersions(string n, string o)
	{
		string[] array = n.Split(new char[] { '.' });
		string[] array2 = o.Split(new char[] { '.' });
		float[] array3 = new float[Math.Max(array2.Length, array.Length)];
		float[] array4 = new float[array3.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array3[i] = float.Parse("0" + array[i].Trim());
		}
		for (int j = 0; j < array2.Length; j++)
		{
			array4[j] = float.Parse("0" + array2[j].Trim());
		}
		for (int k = 0; k < array3.Length; k++)
		{
			if (array3[k] > array4[k])
			{
				return 1;
			}
			if (array3[k] < array4[k])
			{
				return -1;
			}
		}
		return 0;
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00008A90 File Offset: 0x00006C90
	public object GetSettingValue(string name)
	{
		string text = name.ToLower();
		uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
		bool flag;
		if (num <= 1306159328U)
		{
			if (num <= 543053314U)
			{
				if (num != 87818708U)
				{
					if (num != 533801063U)
					{
						if (num == 543053314U)
						{
							if (text == "volumeboost")
							{
								flag = this.GetVolumeBoost();
								goto IL_26F;
							}
						}
					}
					else if (text == "promptforupdate")
					{
						flag = this.GetPromptForUpdate();
						goto IL_26F;
					}
				}
				else if (text == "startintray")
				{
					flag = this.GetStartInTray();
					goto IL_26F;
				}
			}
			else if (num != 677775829U)
			{
				if (num != 1019237139U)
				{
					if (num == 1306159328U)
					{
						if (text == "voicemean")
						{
							return VoiceChanger.singleton(0L).GetLastVoiceMean();
						}
					}
				}
				else if (text == "gateoutput")
				{
					return this.GetGateOutputValue();
				}
			}
			else if (text == "snapnote")
			{
				flag = this.GetNoteSnap();
				goto IL_26F;
			}
		}
		else if (num <= 3430357409U)
		{
			if (num != 1635173897U)
			{
				if (num != 2419558806U)
				{
					if (num == 3430357409U)
					{
						if (text == "reducebreath")
						{
							flag = this.GetReduceBreath();
							goto IL_26F;
						}
					}
				}
				else if (text == "outputvolume")
				{
					return Settings.Default.OutputVolume;
				}
			}
			else if (text == "keepalive")
			{
				flag = PowerRequester.state;
				goto IL_26F;
			}
		}
		else if (num <= 3881872140U)
		{
			if (num != 3847145228U)
			{
				if (num == 3881872140U)
				{
					if (text == "startwithwindows")
					{
						flag = this.GetStartOnBoot();
						goto IL_26F;
					}
				}
			}
			else if (text == "autotrain")
			{
				flag = this.GetAutoTrain();
				goto IL_26F;
			}
		}
		else if (num != 3899524373U)
		{
			if (num == 4166805404U)
			{
				if (text == "minimizeonclose")
				{
					flag = this.GetMinimizeOnClose();
					goto IL_26F;
				}
			}
		}
		else if (text == "semitoneshift")
		{
			return this.GetSemitoneShift();
		}
		return null;
		IL_26F:
		return flag;
	}

	// Token: 0x04000080 RID: 128
	private static Utilities instance;
}
