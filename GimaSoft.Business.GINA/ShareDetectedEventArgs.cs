using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GimaSoft.Business.GINA
{
	// Token: 0x0200001A RID: 26
	public class ShareDetectedEventArgs : EventArgs
	{
		// Token: 0x06000107 RID: 263 RVA: 0x000056B8 File Offset: 0x000038B8
		public ShareDetectedEventArgs(PackageShareType type, Guid sessionId, string sharer, string filename = null)
		{
			this.SessionId = sessionId;
			this.Sharer = sharer;
			this.ShareType = type;
			if (!string.IsNullOrWhiteSpace(filename))
			{
				if (type == PackageShareType.GINAPackageFile)
				{
					this.FilePackage = Package.OpenPackage(File.ReadAllBytes(filename));
					return;
				}
				if (type == PackageShareType.GamTextTriggersFile)
				{
					this.FilePackage = Package.OpenPackageFromGamTextTriggers(filename);
				}
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005711 File Offset: 0x00003911
		public ShareDetectedEventArgs(PackageShareType type, Package package, string sharer = null)
		{
			this.SessionId = Guid.Empty;
			this.ShareType = type;
			this.Sharer = sharer;
			this.FilePackage = package;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00005739 File Offset: 0x00003939
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00005741 File Offset: 0x00003941
		public Guid SessionId { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600010B RID: 267 RVA: 0x0000574A File Offset: 0x0000394A
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00005752 File Offset: 0x00003952
		public List<Guid> SessionIds { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600010D RID: 269 RVA: 0x0000575C File Offset: 0x0000395C
		public IEnumerable<Guid> EffectiveSessionIds
		{
			get
			{
				if (this.SessionIds != null && this.SessionIds.Any<Guid>())
				{
					return this.SessionIds;
				}
				return new List<Guid> { this.SessionId };
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00005798 File Offset: 0x00003998
		// (set) Token: 0x0600010F RID: 271 RVA: 0x000057A0 File Offset: 0x000039A0
		public string Sharer { get; internal set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000057A9 File Offset: 0x000039A9
		// (set) Token: 0x06000111 RID: 273 RVA: 0x000057B1 File Offset: 0x000039B1
		public Package FilePackage { get; internal set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000057BA File Offset: 0x000039BA
		// (set) Token: 0x06000113 RID: 275 RVA: 0x000057C2 File Offset: 0x000039C2
		public PackageShareType ShareType { get; internal set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000057D4 File Offset: 0x000039D4
		public bool ProcessInvitation
		{
			get
			{
				if (!ShareDetectedEventArgs.InteractiveShareTypes.Contains(this.ShareType))
				{
					return true;
				}
				if (Configuration.Current.AcceptShareLevel == ShareLevel.Anybody)
				{
					return true;
				}
				if (!string.IsNullOrWhiteSpace(this.Sharer) && Configuration.Current.AcceptShareLevel == ShareLevel.TrustedList)
				{
					return Configuration.Current.ShareWhiteList.Select((string o) => o.ToLower()).Contains(this.Sharer.ToLower());
				}
				return false;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000115 RID: 277 RVA: 0x0000585C File Offset: 0x00003A5C
		public bool PromptForDownload
		{
			get
			{
				return ShareDetectedEventArgs.InteractiveShareTypes.Contains(this.ShareType) && !this.AutoMerge;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00005884 File Offset: 0x00003A84
		public bool AutoMerge
		{
			get
			{
				if (Configuration.Current.AutoMergeShareLevel == ShareLevel.Anybody)
				{
					return true;
				}
				if (!string.IsNullOrWhiteSpace(this.Sharer) && Configuration.Current.AutoMergeShareLevel == ShareLevel.TrustedList)
				{
					return Configuration.Current.ShareWhiteList.Select((string o) => o.ToLower()).Contains(this.Sharer.ToLower());
				}
				return false;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000117 RID: 279 RVA: 0x000058F8 File Offset: 0x00003AF8
		public bool IsFileImport
		{
			get
			{
				return ShareDetectedEventArgs.FileShareTypes.Contains(this.ShareType);
			}
		}

		// Token: 0x04000074 RID: 116
		private static IEnumerable<PackageShareType> InteractiveShareTypes = new List<PackageShareType>
		{
			PackageShareType.GINAShare,
			PackageShareType.GamTextTriggersShare
		};

		// Token: 0x04000075 RID: 117
		private static IEnumerable<PackageShareType> FileShareTypes = new List<PackageShareType>
		{
			PackageShareType.GINAPackageFile,
			PackageShareType.GamTextTriggersFile
		};
	}
}
