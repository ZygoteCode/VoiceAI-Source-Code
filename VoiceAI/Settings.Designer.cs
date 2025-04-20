using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VoiceAIGui.Properties
{
	// Token: 0x02000018 RID: 24
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.0.3.0")]
	public sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x000097A2 File Offset: 0x000079A2
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x000097A9 File Offset: 0x000079A9
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x000097BB File Offset: 0x000079BB
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		public bool MinimizeOnClose
		{
			get
			{
				return (bool)this["MinimizeOnClose"];
			}
			set
			{
				this["MinimizeOnClose"] = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x000097CE File Offset: 0x000079CE
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x000097E0 File Offset: 0x000079E0
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		public bool StartOnBoot
		{
			get
			{
				return (bool)this["StartOnBoot"];
			}
			set
			{
				this["StartOnBoot"] = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x000097F3 File Offset: 0x000079F3
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x00009805 File Offset: 0x00007A05
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		public bool StartInTray
		{
			get
			{
				return (bool)this["StartInTray"];
			}
			set
			{
				this["StartInTray"] = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00009818 File Offset: 0x00007A18
		// (set) Token: 0x060001BB RID: 443 RVA: 0x0000982A File Offset: 0x00007A2A
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("False")]
		public bool PromptForUpdate
		{
			get
			{
				return (bool)this["PromptForUpdate"];
			}
			set
			{
				this["PromptForUpdate"] = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000983D File Offset: 0x00007A3D
		// (set) Token: 0x060001BD RID: 445 RVA: 0x0000984F File Offset: 0x00007A4F
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("False")]
		public bool AutoTrain
		{
			get
			{
				return (bool)this["AutoTrain"];
			}
			set
			{
				this["AutoTrain"] = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00009862 File Offset: 0x00007A62
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00009874 File Offset: 0x00007A74
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		public bool VolumeBoost
		{
			get
			{
				return (bool)this["VolumeBoost"];
			}
			set
			{
				this["VolumeBoost"] = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00009887 File Offset: 0x00007A87
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00009899 File Offset: 0x00007A99
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		public bool ReduceBreath
		{
			get
			{
				return (bool)this["ReduceBreath"];
			}
			set
			{
				this["ReduceBreath"] = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x000098AC File Offset: 0x00007AAC
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x000098BE File Offset: 0x00007ABE
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("0")]
		public float SemitoneShift
		{
			get
			{
				return (float)this["SemitoneShift"];
			}
			set
			{
				this["SemitoneShift"] = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x000098D1 File Offset: 0x00007AD1
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x000098E3 File Offset: 0x00007AE3
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("False")]
		public bool SnapNote
		{
			get
			{
				return (bool)this["SnapNote"];
			}
			set
			{
				this["SnapNote"] = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x000098F6 File Offset: 0x00007AF6
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x00009908 File Offset: 0x00007B08
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("-45")]
		public float GateOutputValue
		{
			get
			{
				return (float)this["GateOutputValue"];
			}
			set
			{
				this["GateOutputValue"] = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000991B File Offset: 0x00007B1B
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000992D File Offset: 0x00007B2D
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("1")]
		public float OutputVolume
		{
			get
			{
				return (float)this["OutputVolume"];
			}
			set
			{
				this["OutputVolume"] = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00009940 File Offset: 0x00007B40
		// (set) Token: 0x060001CB RID: 459 RVA: 0x00009952 File Offset: 0x00007B52
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		public bool PassThruEnabled
		{
			get
			{
				return (bool)this["PassThruEnabled"];
			}
			set
			{
				this["PassThruEnabled"] = value;
			}
		}

		// Token: 0x04000089 RID: 137
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
