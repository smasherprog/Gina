using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace WPFShared
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public abstract class BindableItemsControl : ItemsControl, INotifyPropertyChanged
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00002F40 File Offset: 0x00001140
		protected static void SetDependentProperties(Type type)
		{
			Stack<Type> stack = new Stack<Type>();
			Type type2 = type;
			do
			{
				stack.Push(type2);
				type2 = type2.BaseType;
			}
			while (type2 != typeof(BindableControl));
			while (stack.Count > 0)
			{
				type2 = stack.Pop();
				MethodInfo methodInfo = type2.GetMethods(BindingFlags.Static | BindingFlags.Public).SingleOrDefault((MethodInfo o) => o.Name == "RegisterDependentProperties");
				if (methodInfo != null)
				{
					methodInfo.Invoke(null, new object[] { type });
				}
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002FD0 File Offset: 0x000011D0
		public BindableItemsControl()
		{
			if (BindableItemsControl.PropertyFields.ContainsKey(base.GetType()))
			{
				this.PropertyObjects = new List<object>(BindableItemsControl.PropertyFields[base.GetType()].Keys.Count);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003028 File Offset: 0x00001228
		private static BindableItemsControl.BindablePropertyInfo GetPropertyField(Type type, string propertyName)
		{
			lock (typeof(BindableObject))
			{
				if (!BindableItemsControl.PropertyFields.ContainsKey(type))
				{
					BindableItemsControl.PropertyFields.Add(type, new Dictionary<string, BindableItemsControl.BindablePropertyInfo>());
					BindableItemsControl.DependentProperties.Add(type, new Dictionary<string, List<string>>());
				}
			}
			BindableItemsControl.BindablePropertyInfo bindablePropertyInfo;
			lock (type)
			{
				if (!BindableItemsControl.PropertyFields[type].ContainsKey(propertyName))
				{
					BindableItemsControl.PropertyFields[type].Add(propertyName, new BindableItemsControl.BindablePropertyInfo
					{
						Index = BindableItemsControl.PropertyFields[type].Keys.Count + 1
					});
				}
				bindablePropertyInfo = BindableItemsControl.PropertyFields[type][propertyName];
			}
			return bindablePropertyInfo;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003118 File Offset: 0x00001318
		private static int GetPropertyFieldIndex(Type type, string propertyName)
		{
			return BindableItemsControl.GetPropertyField(type, propertyName).Index;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003150 File Offset: 0x00001350
		protected static void RegisterDependentProperty(Type type, string propertyName, string dependeePropertyName, BindableItemsControl.ReferenceLookup lookupMethod)
		{
			BindableItemsControl.BindablePropertyInfo propertyField = BindableItemsControl.GetPropertyField(type, propertyName);
			lock (type)
			{
				BindableItemsControl.DependeePropertyInfo dependeePropertyInfo = propertyField.Dependees.SingleOrDefault((BindableItemsControl.DependeePropertyInfo o) => o.DependeePropertyName == dependeePropertyName);
				if (dependeePropertyInfo == null)
				{
					propertyField.Dependees.Add(dependeePropertyInfo = new BindableItemsControl.DependeePropertyInfo
					{
						DependeePropertyName = dependeePropertyName
					});
					if (!BindableItemsControl.DependentProperties[type].ContainsKey(dependeePropertyName))
					{
						BindableItemsControl.DependentProperties[type][dependeePropertyName] = new List<string> { propertyName };
					}
					else if (!BindableItemsControl.DependentProperties[type][dependeePropertyName].Any((string o) => o == propertyName))
					{
						BindableItemsControl.DependentProperties[type][dependeePropertyName].Add(propertyName);
					}
				}
				dependeePropertyInfo.LookupMethod = lookupMethod;
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000032A0 File Offset: 0x000014A0
		private void EnsurePropertyObjectsSize(int size)
		{
			lock (this)
			{
				while (size > this.PropertyObjects.Count)
				{
					this.PropertyObjects.Add(null);
				}
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000032F4 File Offset: 0x000014F4
		private object Get(string propertyName)
		{
			int propertyFieldIndex = BindableItemsControl.GetPropertyFieldIndex(base.GetType(), propertyName);
			this.EnsurePropertyObjectsSize(propertyFieldIndex + 1);
			return this.PropertyObjects[propertyFieldIndex];
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003324 File Offset: 0x00001524
		protected T Get<T>(string propertyName)
		{
			object obj = this.Get(propertyName);
			if (obj != null)
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003368 File Offset: 0x00001568
		public void Set(string propertyName, object value)
		{
			int propertyFieldIndex = BindableItemsControl.GetPropertyFieldIndex(base.GetType(), propertyName);
			this.EnsurePropertyObjectsSize(propertyFieldIndex + 1);
			this.PropertyObjects[propertyFieldIndex] = value;
			this.RaisePropertyChanged(propertyName);
			if (BindableItemsControl.DependentProperties[base.GetType()].ContainsKey(propertyName))
			{
				foreach (string text in BindableItemsControl.DependentProperties[base.GetType()][propertyName])
				{
					BindableItemsControl.DependeePropertyInfo dependeePropertyInfo = BindableItemsControl.GetPropertyField(base.GetType(), text).Dependees.Single((BindableItemsControl.DependeePropertyInfo o) => o.DependeePropertyName == propertyName);
					if (dependeePropertyInfo.LookupMethod != null)
					{
						this.Set(text, dependeePropertyInfo.LookupMethod(value, this));
					}
					else
					{
						this.RaisePropertyChanged(text);
					}
				}
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000056 RID: 86 RVA: 0x00003484 File Offset: 0x00001684
		// (remove) Token: 0x06000057 RID: 87 RVA: 0x000034BC File Offset: 0x000016BC
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000058 RID: 88 RVA: 0x000034F4 File Offset: 0x000016F4
		public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentException("propertyName cannot be null or empty.");
			}
			lock (typeof(BindableControl))
			{
				if (!BindableItemsControl.eventArgCache.ContainsKey(propertyName))
				{
					BindableItemsControl.eventArgCache.Add(propertyName, new PropertyChangedEventArgs(propertyName));
				}
			}
			return BindableItemsControl.eventArgCache[propertyName];
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003570 File Offset: 0x00001770
		protected virtual void AfterPropertyChanged(string propertyName)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003574 File Offset: 0x00001774
		protected void RaisePropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, BindableItemsControl.GetPropertyChangedEventArgs(propertyName));
			}
			this.AfterPropertyChanged(propertyName);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000035A0 File Offset: 0x000017A0
		[Conditional("DEBUG")]
		private void VerifyProperty(string propertyName)
		{
			Type type = base.GetType();
			PropertyInfo property = type.GetProperty(propertyName);
			if (property == null)
			{
				string.Format("{0} is not a public property of {1}", propertyName, type.FullName);
			}
		}

		// Token: 0x0400001C RID: 28
		private const string ERROR_MSG = "{0} is not a public property of {1}";

		// Token: 0x0400001D RID: 29
		private static readonly Dictionary<string, PropertyChangedEventArgs> eventArgCache = new Dictionary<string, PropertyChangedEventArgs>();

		// Token: 0x0400001E RID: 30
		private static Dictionary<Type, Dictionary<string, BindableItemsControl.BindablePropertyInfo>> PropertyFields = new Dictionary<Type, Dictionary<string, BindableItemsControl.BindablePropertyInfo>>();

		// Token: 0x0400001F RID: 31
		private static Dictionary<Type, Dictionary<string, List<string>>> DependentProperties = new Dictionary<Type, Dictionary<string, List<string>>>();

		// Token: 0x04000020 RID: 32
		private List<object> PropertyObjects = new List<object>();

		// Token: 0x0200000D RID: 13
		// (Invoke) Token: 0x0600005E RID: 94
		public delegate object ReferenceLookup(object lookupKey, object obj);

		// Token: 0x0200000E RID: 14
		private class DependeePropertyInfo
		{
			// Token: 0x1700000D RID: 13
			// (get) Token: 0x06000061 RID: 97 RVA: 0x000035D7 File Offset: 0x000017D7
			// (set) Token: 0x06000062 RID: 98 RVA: 0x000035DF File Offset: 0x000017DF
			public string DependeePropertyName { get; set; }

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x06000063 RID: 99 RVA: 0x000035E8 File Offset: 0x000017E8
			// (set) Token: 0x06000064 RID: 100 RVA: 0x000035F0 File Offset: 0x000017F0
			public BindableItemsControl.ReferenceLookup LookupMethod { get; set; }
		}

		// Token: 0x0200000F RID: 15
		private class BindablePropertyInfo
		{
			// Token: 0x1700000F RID: 15
			// (get) Token: 0x06000066 RID: 102 RVA: 0x00003601 File Offset: 0x00001801
			// (set) Token: 0x06000067 RID: 103 RVA: 0x00003609 File Offset: 0x00001809
			public int Index { get; set; }

			// Token: 0x04000025 RID: 37
			public List<BindableItemsControl.DependeePropertyInfo> Dependees = new List<BindableItemsControl.DependeePropertyInfo>();
		}
	}
}
