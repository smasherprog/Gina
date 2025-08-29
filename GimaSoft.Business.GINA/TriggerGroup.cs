using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using WPFShared;

namespace GimaSoft.Business.GINA
{
	// Token: 0x02000040 RID: 64
	public class TriggerGroup : GINABusinessObject, ITriggerLibraryEntry
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00010FA1 File Offset: 0x0000F1A1
		public static TriggerGroup RootGroup
		{
			get
			{
				return TriggerGroup._RootGroup;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00010FB0 File Offset: 0x0000F1B0
		public static IEnumerable<TriggerGroup> All
		{
			get
			{
				return TriggerGroup.RootGroup.Groups.SelectMany((TriggerGroup o) => o.DescendantTree);
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00010FE0 File Offset: 0x0000F1E0
		public static void Init()
		{
			if (!TriggerGroup.RootGroup.Groups.Any<TriggerGroup>())
			{
				BehaviorGroup.Init();
				TriggerGroup.RootGroup.AddGroup("Common", null);
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00011024 File Offset: 0x0000F224
		public static void Clear()
		{
			List<TriggerGroup> list = TriggerGroup.RootGroup.Groups.SelectMany((TriggerGroup o) => o.DescendantTree).Distinct<TriggerGroup>().ToList<TriggerGroup>();
			TriggerGroup.RootGroup.Groups.Clear();
			foreach (TriggerGroup triggerGroup in list)
			{
				triggerGroup.Dispose();
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x000110CC File Offset: 0x0000F2CC
		public static int GetNextGroupId()
		{
			List<int> list = (from o in TriggerGroup.All
				where o.GroupId > -1
				select o.GroupId).ToList<int>();
			if (!list.Any<int>())
			{
				return 1;
			}
			return list.Max() + 1;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00011164 File Offset: 0x0000F364
		public static TriggerGroup FindGroup(IEnumerable<string> names)
		{
			if (names == null || !names.Any<string>())
			{
				return null;
			}
			Queue<string> queue = new Queue<string>(names.Select((string o) => o.ToLower()));
			ObservableCollection<TriggerGroup> observableCollection = TriggerGroup.RootGroup.Groups;
			TriggerGroup triggerGroup = null;
			while (queue.Any<string>())
			{
				string path = queue.Dequeue();
				triggerGroup = observableCollection.SingleOrDefault((TriggerGroup o) => o.Name.ToLower() == path);
				if (triggerGroup == null)
				{
					return null;
				}
				observableCollection = triggerGroup.Groups;
			}
			return triggerGroup;
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000390 RID: 912 RVA: 0x000111F0 File Offset: 0x0000F3F0
		// (remove) Token: 0x06000391 RID: 913 RVA: 0x00011224 File Offset: 0x0000F424
		public static event TriggerGroup.GroupsChangedHandler GroupsChanged;

		// Token: 0x06000392 RID: 914 RVA: 0x00011257 File Offset: 0x0000F457
		private static void OnGroupsChanged()
		{
			TriggerGroup.GroupsChanged();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00011263 File Offset: 0x0000F463
		public TriggerGroup()
		{
			this.GroupId = -100;
			this.NeedsMerge = true;
			this.Triggers = new ObservableCollection<Trigger>();
			this.Groups = new ObservableCollection<TriggerGroup>();
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00011290 File Offset: 0x0000F490
		public TriggerGroup(bool isRoot)
		{
			this.GroupId = (isRoot ? (-500) : (-100));
			this.NeedsMerge = !isRoot;
			this.Triggers = new ObservableCollection<Trigger>();
			this.Groups = new ObservableCollection<TriggerGroup>();
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000395 RID: 917 RVA: 0x000112CA File Offset: 0x0000F4CA
		// (set) Token: 0x06000396 RID: 918 RVA: 0x0001130C File Offset: 0x0000F50C
		public string Name
		{
			get
			{
				return base.Get<string>("Name");
			}
			set
			{
				string name = this.Name;
				string newName = value;
				if (!string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(newName))
				{
					base.RaisePropertyChanged("Name");
					return;
				}
				ObservableCollection<TriggerGroup> observableCollection = ((this.ParentGroup != null) ? this.ParentGroup.Groups : (TriggerGroup.RootGroup.Groups.Contains(this) ? TriggerGroup.RootGroup.Groups : null));
				if (observableCollection == null || !observableCollection.Any((TriggerGroup o) => o.Name.ToLower() == newName.ToLower() && o != this))
				{
					base.Set("Name", value);
					return;
				}
				base.RaisePropertyChanged("Name");
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000397 RID: 919 RVA: 0x000113B9 File Offset: 0x0000F5B9
		// (set) Token: 0x06000398 RID: 920 RVA: 0x000113C6 File Offset: 0x0000F5C6
		public string Comments
		{
			get
			{
				return base.Get<string>("Comments");
			}
			set
			{
				base.Set("Comments", value);
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000399 RID: 921 RVA: 0x000113D4 File Offset: 0x0000F5D4
		// (set) Token: 0x0600039A RID: 922 RVA: 0x000113E1 File Offset: 0x0000F5E1
		public bool SelfCommented
		{
			get
			{
				return base.Get<bool>("SelfCommented");
			}
			set
			{
				base.Set("SelfCommented", value);
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600039B RID: 923 RVA: 0x000113F4 File Offset: 0x0000F5F4
		// (set) Token: 0x0600039C RID: 924 RVA: 0x00011401 File Offset: 0x0000F601
		public ObservableCollection<TriggerGroup> Groups
		{
			get
			{
				return base.Get<ObservableCollection<TriggerGroup>>("Groups");
			}
			set
			{
				base.Set("Groups", value);
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600039D RID: 925 RVA: 0x0001140F File Offset: 0x0000F60F
		// (set) Token: 0x0600039E RID: 926 RVA: 0x0001141C File Offset: 0x0000F61C
		public ObservableCollection<Trigger> Triggers
		{
			get
			{
				return base.Get<ObservableCollection<Trigger>>("Triggers");
			}
			set
			{
				base.Set("Triggers", value);
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600039F RID: 927 RVA: 0x0001142A File Offset: 0x0000F62A
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x00011437 File Offset: 0x0000F637
		public int GroupId
		{
			get
			{
				return base.Get<int>("GroupId");
			}
			set
			{
				base.Set("GroupId", value);
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0001144A File Offset: 0x0000F64A
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x00011457 File Offset: 0x0000F657
		public bool EnableByDefault
		{
			get
			{
				return base.Get<bool>("EnableByDefault");
			}
			set
			{
				base.Set("EnableByDefault", value);
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0001146A File Offset: 0x0000F66A
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x00011477 File Offset: 0x0000F677
		public bool NeedsMerge
		{
			get
			{
				return base.Get<bool>("NeedsMerge");
			}
			set
			{
				base.Set("NeedsMerge", value);
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0001148A File Offset: 0x0000F68A
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00011497 File Offset: 0x0000F697
		public TriggerGroup ParentGroup
		{
			get
			{
				return base.Get<TriggerGroup>("ParentGroup");
			}
			set
			{
				base.Set("ParentGroup", value);
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x000114B0 File Offset: 0x0000F6B0
		public IEnumerable<TriggerGroup> DescendantTree
		{
			get
			{
				return new List<TriggerGroup> { this }.Union(this.Groups.SelectMany((TriggerGroup o) => o.DescendantTree).ToList<TriggerGroup>());
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x000114FD File Offset: 0x0000F6FD
		public bool IsUnattached
		{
			get
			{
				return this.GroupId == -100;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x0001151F File Offset: 0x0000F71F
		public IEnumerable<GINACharacter> EnabledCharacters
		{
			get
			{
				return (from o in GINACharacter.All
					where o.TriggerGroups.Contains(this)
					orderby o.DisplayName
					select o).ToList<GINACharacter>();
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00011560 File Offset: 0x0000F760
		public TriggerGroup AddGroup(string name, int? index = null)
		{
			TriggerGroup triggerGroup = new TriggerGroup
			{
				Name = name,
				ParentGroup = this
			};
			return this.AddGroup(triggerGroup, index);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x000115DC File Offset: 0x0000F7DC
		public TriggerGroup AddGroup(TriggerGroup group, int? index = null)
		{
			if (group.ParentGroup != null)
			{
				group.ParentGroup.Groups.Remove(group);
			}
			if (!this.IsUnattached && group.IsUnattached)
			{
				group.GroupId = TriggerGroup.GetNextGroupId();
			}
			if (!this.IsUnattached && group.NeedsMerge)
			{
				group.NeedsMerge = false;
			}
			if (!this.IsUnattached && TriggerGroup.All.Any((TriggerGroup o) => o.GroupId == group.GroupId))
			{
				throw new Exception("Duplicate GroupId");
			}
			group.ParentGroup = this;
			string name = group.Name;
			int num = 1;
			while (group.ParentGroup.Groups.Any((TriggerGroup o) => o.Name.ToLower() == group.Name.ToLower() && o != group))
			{
				group.Name = string.Format("{0} #{1}", name, num);
			}
			if (index == null)
			{
				this.Groups.Add(group);
			}
			else
			{
				this.Groups.Insert(index.Value, group);
			}
			if (!this.IsUnattached)
			{
				TriggerGroup.OnGroupsChanged();
			}
			return group;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00011750 File Offset: 0x0000F950
		public void Move(TriggerGroup group, int index = -1)
		{
			if (group == this.ParentGroup)
			{
				return;
			}
			TriggerGroup triggerGroup = group.Groups.SingleOrDefault((TriggerGroup o) => o.Name.ToLower() == this.Name.ToLower());
			if (triggerGroup != null)
			{
				this.Merge(group, true);
				this.ParentGroup.Groups.Remove(this);
			}
			else
			{
				this.ParentGroup.Groups.Remove(this);
				group.AddGroup(this, null);
			}
			if (!this.IsUnattached)
			{
				TriggerGroup.OnGroupsChanged();
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000117D0 File Offset: 0x0000F9D0
		public void Remove()
		{
			foreach (TriggerGroup triggerGroup in this.Groups.ToList<TriggerGroup>())
			{
				triggerGroup.Remove();
			}
			foreach (Trigger trigger in this.Triggers.ToList<Trigger>())
			{
				trigger.Remove();
			}
			this.ParentGroup.Groups.Remove(this);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00011880 File Offset: 0x0000FA80
		public Trigger AddTrigger(string name)
		{
			Trigger trigger = new Trigger
			{
				Name = name,
				ParentGroup = this
			};
			return this.AddTrigger(trigger, null);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x000118E0 File Offset: 0x0000FAE0
		public Trigger AddTrigger(Trigger trigger, int? index = null)
		{
			if (trigger.ParentGroup != null)
			{
				trigger.ParentGroup.Triggers.Remove(trigger);
			}
			trigger.ParentGroup = this;
			if (!this.IsUnattached)
			{
				if (!string.IsNullOrWhiteSpace(trigger.SuggestedCategory) && trigger.Category.Name.ToLower() != trigger.SuggestedCategory.ToLower())
				{
					trigger.Category = TriggerCategory.Find(trigger.SuggestedCategory, true);
				}
				trigger.NeedsMerge = false;
				trigger.MediaFileId = null;
				trigger.SuggestedCategory = null;
			}
			Trigger trigger2 = this.Triggers.SingleOrDefault((Trigger o) => o.Name.ToLower() == trigger.Name.ToLower());
			if (index == null)
			{
				this.Triggers.Add(trigger);
			}
			else
			{
				this.Triggers.Insert(Math.Min(index.Value, this.Triggers.Count), trigger);
			}
			if (trigger2 != null)
			{
				this.Triggers.Remove(trigger2);
			}
			return trigger;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00011A68 File Offset: 0x0000FC68
		public string GetUniqueTriggerName(Trigger trigger)
		{
			string name = trigger.Name;
			string newName = trigger.Name;
			int num = 1;
			while (this.Triggers.Any((Trigger o) => o.Name.ToLower() == newName.ToLower() && trigger != o))
			{
				newName = string.Format("{0} #{1}", name, num++);
			}
			return newName;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00011AE0 File Offset: 0x0000FCE0
		public TriggerGroup CloneShallow()
		{
			return new TriggerGroup
			{
				Name = this.Name,
				Comments = this.Comments,
				GroupId = -100
			};
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00011B14 File Offset: 0x0000FD14
		public TriggerGroup Clone()
		{
			TriggerGroup triggerGroup = new TriggerGroup();
			triggerGroup.CopyFrom(this);
			foreach (TriggerGroup triggerGroup2 in this.Groups)
			{
				triggerGroup.AddGroup(triggerGroup2.Clone(), null);
			}
			foreach (Trigger trigger in this.Triggers)
			{
				triggerGroup.AddTrigger(trigger.Clone(), null);
			}
			return triggerGroup;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00011BD0 File Offset: 0x0000FDD0
		public void CopyFrom(TriggerGroup group)
		{
			this.Name = group.Name;
			if (!this.SelfCommented)
			{
				this.Comments = group.Comments;
			}
			this.NeedsMerge = group.NeedsMerge && this.IsUnattached;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00011C6C File Offset: 0x0000FE6C
		public void Merge(TriggerGroup targetParentGroup, bool forceMerge = false)
		{
			TriggerGroup triggerGroup = targetParentGroup.Groups.SingleOrDefault((TriggerGroup o) => o.Name.ToLower() == this.Name.ToLower());
			if (this.NeedsMerge || forceMerge)
			{
				if (triggerGroup != null)
				{
					triggerGroup.CopyFrom(this);
				}
				else
				{
					triggerGroup = targetParentGroup.AddGroup(this.CloneShallow(), null);
					triggerGroup.EnableByDefault = targetParentGroup.EnableByDefault;
					if (triggerGroup.EnableByDefault)
					{
						foreach (GINACharacter ginacharacter in GINACharacter.All)
						{
							ginacharacter.TriggerGroups.Add(triggerGroup);
						}
					}
				}
			}
			if (triggerGroup != null)
			{
				foreach (TriggerGroup triggerGroup2 in this.Groups)
				{
					triggerGroup2.Merge(triggerGroup, forceMerge);
				}
				using (List<Trigger>.Enumerator enumerator3 = this.Triggers.Where((Trigger o) => o.NeedsMerge || forceMerge).ToList<Trigger>().GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Trigger trigger = enumerator3.Current;
						Trigger trigger2 = triggerGroup.Triggers.SingleOrDefault((Trigger o) => o.Name.ToLower() == trigger.Name.ToLower());
						if (trigger2 != null)
						{
							trigger2.CopyFrom(trigger);
						}
						else
						{
							trigger2 = triggerGroup.AddTrigger(trigger, null);
						}
						trigger2.Modified = DateTime.Now;
					}
				}
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00011E54 File Offset: 0x00010054
		public override void Dispose(bool disposing)
		{
			if (!this._Disposed)
			{
				this._Disposed = true;
				if (disposing)
				{
					foreach (Trigger trigger in this.Triggers)
					{
						trigger.Dispose();
					}
					foreach (TriggerGroup triggerGroup in this.Groups)
					{
						triggerGroup.Dispose();
					}
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00011EF4 File Offset: 0x000100F4
		public static TriggerGroup LoadFromXml(XmlElement element, TriggerGroup parentGroup, bool recurse = true, IEnumerable<Package.PrerecordedFile> files = null)
		{
			TriggerGroup triggerGroup = new TriggerGroup();
			int elementValue = element.GetElementValue("GroupId", 0);
			if (elementValue > 0)
			{
				triggerGroup.GroupId = elementValue;
			}
			triggerGroup.Name = element.GetElementValue("Name", null);
			triggerGroup.Comments = element.GetElementValue("Comments", null);
			triggerGroup.SelfCommented = element.GetElementValue("SelfCommented", false);
			triggerGroup.EnableByDefault = element.GetElementValue("EnableByDefault", false);
			if (parentGroup != null)
			{
				parentGroup.AddGroup(triggerGroup, null);
			}
			if (recurse)
			{
				XmlElement xmlElement = element.SelectSingleNode("TriggerGroups") as XmlElement;
				if (xmlElement != null)
				{
					List<XmlElement> list = xmlElement.SelectNodes("TriggerGroup").Cast<XmlElement>().ToList<XmlElement>();
					foreach (XmlElement xmlElement2 in list)
					{
						TriggerGroup.LoadFromXml(xmlElement2, triggerGroup, recurse, files);
					}
				}
			}
			XmlElement xmlElement3 = element.SelectSingleNode("Triggers") as XmlElement;
			if (xmlElement3 != null)
			{
				List<XmlElement> list2 = xmlElement3.SelectNodes("Trigger").Cast<XmlElement>().ToList<XmlElement>();
				foreach (XmlElement xmlElement4 in list2)
				{
					triggerGroup.AddTrigger(Trigger.LoadFromXml(xmlElement4, files), null);
				}
			}
			return triggerGroup;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00012074 File Offset: 0x00010274
		public XmlElement SaveToXml(XmlElement element, bool recurse = true, bool includeTriggers = true, bool includeGroupId = true, IEnumerable<Package.PrerecordedFile> files = null)
		{
			XmlDocument ownerDocument = element.OwnerDocument;
			XmlElement xmlElement = ownerDocument.CreateElement("TriggerGroup");
			element.AppendChild(xmlElement);
			xmlElement.AppendChild(ownerDocument.NewElement("Name", this.Name));
			xmlElement.AppendChild(ownerDocument.NewElement("Comments", this.Comments ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("SelfCommented", this.SelfCommented));
			xmlElement.AppendChild(ownerDocument.NewElement("GroupId", includeGroupId ? this.GroupId : 0));
			xmlElement.AppendChild(ownerDocument.NewElement("EnableByDefault", this.EnableByDefault));
			if (recurse && this.Groups.Any<TriggerGroup>())
			{
				XmlElement xmlElement2 = ownerDocument.CreateElement("TriggerGroups");
				xmlElement.AppendChild(xmlElement2);
				foreach (TriggerGroup triggerGroup in this.Groups)
				{
					triggerGroup.SaveToXml(xmlElement2, recurse, includeTriggers, includeGroupId, files);
				}
			}
			if (includeTriggers && this.Triggers.Any<Trigger>())
			{
				XmlElement xmlElement3 = ownerDocument.CreateElement("Triggers");
				xmlElement.AppendChild(xmlElement3);
				foreach (Trigger trigger in this.Triggers)
				{
					trigger.SaveToXml(xmlElement3, files);
				}
			}
			return xmlElement;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00012212 File Offset: 0x00010412
		// Note: this type is marked as 'beforefieldinit'.
		static TriggerGroup()
		{
			TriggerGroup.GroupsChanged = delegate
			{
			};
		}

		// Token: 0x04000154 RID: 340
		public const int RootGroupId = -500;

		// Token: 0x04000155 RID: 341
		public const int UnattachedGroupId = -100;

		// Token: 0x04000156 RID: 342
		private static TriggerGroup _RootGroup = new TriggerGroup(true);

		// Token: 0x02000041 RID: 65
		// (Invoke) Token: 0x060003C5 RID: 965
		public delegate void GroupsChangedHandler();
	}
}
