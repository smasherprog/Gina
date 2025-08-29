using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace WPFShared
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public abstract class BindableControl : Control, INotifyPropertyChanged
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00002828 File Offset: 0x00000A28
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

		// Token: 0x06000031 RID: 49 RVA: 0x000028B8 File Offset: 0x00000AB8
		public BindableControl()
		{
			if (BindableControl.PropertyFields.ContainsKey(base.GetType()))
			{
				this.PropertyObjects = new List<object>(BindableControl.PropertyFields[base.GetType()].Keys.Count);
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002910 File Offset: 0x00000B10
		private static BindableControl.BindablePropertyInfo GetPropertyField(Type type, string propertyName)
		{
			lock (typeof(BindableObject))
			{
				if (!BindableControl.PropertyFields.ContainsKey(type))
				{
					BindableControl.PropertyFields.Add(type, new Dictionary<string, BindableControl.BindablePropertyInfo>());
					BindableControl.DependentProperties.Add(type, new Dictionary<string, List<string>>());
				}
			}
			BindableControl.BindablePropertyInfo bindablePropertyInfo;
			lock (type)
			{
				if (!BindableControl.PropertyFields[type].ContainsKey(propertyName))
				{
					BindableControl.PropertyFields[type].Add(propertyName, new BindableControl.BindablePropertyInfo
					{
						Index = BindableControl.PropertyFields[type].Keys.Count + 1
					});
				}
				bindablePropertyInfo = BindableControl.PropertyFields[type][propertyName];
			}
			return bindablePropertyInfo;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002A00 File Offset: 0x00000C00
		private static int GetPropertyFieldIndex(Type type, string propertyName)
		{
			return BindableControl.GetPropertyField(type, propertyName).Index;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002A38 File Offset: 0x00000C38
		protected static void RegisterDependentProperty(Type type, string propertyName, string dependeePropertyName, BindableControl.ReferenceLookup lookupMethod)
		{
			BindableControl.BindablePropertyInfo propertyField = BindableControl.GetPropertyField(type, propertyName);
			lock (type)
			{
				BindableControl.DependeePropertyInfo dependeePropertyInfo = propertyField.Dependees.SingleOrDefault((BindableControl.DependeePropertyInfo o) => o.DependeePropertyName == dependeePropertyName);
				if (dependeePropertyInfo == null)
				{
					propertyField.Dependees.Add(dependeePropertyInfo = new BindableControl.DependeePropertyInfo
					{
						DependeePropertyName = dependeePropertyName
					});
					if (!BindableControl.DependentProperties[type].ContainsKey(dependeePropertyName))
					{
						BindableControl.DependentProperties[type][dependeePropertyName] = new List<string> { propertyName };
					}
					else if (!BindableControl.DependentProperties[type][dependeePropertyName].Any((string o) => o == propertyName))
					{
						BindableControl.DependentProperties[type][dependeePropertyName].Add(propertyName);
					}
				}
				dependeePropertyInfo.LookupMethod = lookupMethod;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002B88 File Offset: 0x00000D88
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

		// Token: 0x06000036 RID: 54 RVA: 0x00002BDC File Offset: 0x00000DDC
		private object Get(string propertyName)
		{
			int propertyFieldIndex = BindableControl.GetPropertyFieldIndex(base.GetType(), propertyName);
			this.EnsurePropertyObjectsSize(propertyFieldIndex + 1);
			return this.PropertyObjects[propertyFieldIndex];
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002C0C File Offset: 0x00000E0C
		protected T Get<T>(string propertyName)
		{
			object obj = this.Get(propertyName);
			if (obj != null)
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002C50 File Offset: 0x00000E50
		public void Set(string propertyName, object value)
		{
			int propertyFieldIndex = BindableControl.GetPropertyFieldIndex(base.GetType(), propertyName);
			this.EnsurePropertyObjectsSize(propertyFieldIndex + 1);
			this.PropertyObjects[propertyFieldIndex] = value;
			this.RaisePropertyChanged(propertyName);
			if (BindableControl.DependentProperties[base.GetType()].ContainsKey(propertyName))
			{
				foreach (string text in BindableControl.DependentProperties[base.GetType()][propertyName])
				{
					BindableControl.DependeePropertyInfo dependeePropertyInfo = BindableControl.GetPropertyField(base.GetType(), text).Dependees.Single((BindableControl.DependeePropertyInfo o) => o.DependeePropertyName == propertyName);
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

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000039 RID: 57 RVA: 0x00002D6C File Offset: 0x00000F6C
		// (remove) Token: 0x0600003A RID: 58 RVA: 0x00002DA4 File Offset: 0x00000FA4
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600003B RID: 59 RVA: 0x00002DDC File Offset: 0x00000FDC
		public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentException("propertyName cannot be null or empty.");
			}
			lock (typeof(BindableControl))
			{
				if (!BindableControl.eventArgCache.ContainsKey(propertyName))
				{
					BindableControl.eventArgCache.Add(propertyName, new PropertyChangedEventArgs(propertyName));
				}
			}
			return BindableControl.eventArgCache[propertyName];
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002E58 File Offset: 0x00001058
		protected virtual void AfterPropertyChanged(string propertyName)
		{
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002E5C File Offset: 0x0000105C
		protected void RaisePropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, BindableControl.GetPropertyChangedEventArgs(propertyName));
			}
			this.AfterPropertyChanged(propertyName);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002E88 File Offset: 0x00001088
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

		// Token: 0x04000011 RID: 17
		private const string ERROR_MSG = "{0} is not a public property of {1}";

		// Token: 0x04000012 RID: 18
		private static readonly Dictionary<string, PropertyChangedEventArgs> eventArgCache = new Dictionary<string, PropertyChangedEventArgs>();

		// Token: 0x04000013 RID: 19
		private static Dictionary<Type, Dictionary<string, BindableControl.BindablePropertyInfo>> PropertyFields = new Dictionary<Type, Dictionary<string, BindableControl.BindablePropertyInfo>>();

		// Token: 0x04000014 RID: 20
		private static Dictionary<Type, Dictionary<string, List<string>>> DependentProperties = new Dictionary<Type, Dictionary<string, List<string>>>();

		// Token: 0x04000015 RID: 21
		private List<object> PropertyObjects = new List<object>();

		// Token: 0x02000009 RID: 9
		// (Invoke) Token: 0x06000041 RID: 65
		public delegate object ReferenceLookup(object lookupKey, object obj);

		// Token: 0x0200000A RID: 10
		private class DependeePropertyInfo
		{
			// Token: 0x1700000A RID: 10
			// (get) Token: 0x06000044 RID: 68 RVA: 0x00002EBF File Offset: 0x000010BF
			// (set) Token: 0x06000045 RID: 69 RVA: 0x00002EC7 File Offset: 0x000010C7
			public string DependeePropertyName { get; set; }

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x06000046 RID: 70 RVA: 0x00002ED0 File Offset: 0x000010D0
			// (set) Token: 0x06000047 RID: 71 RVA: 0x00002ED8 File Offset: 0x000010D8
			public BindableControl.ReferenceLookup LookupMethod { get; set; }
		}

		// Token: 0x0200000B RID: 11
		private class BindablePropertyInfo
		{
			// Token: 0x1700000C RID: 12
			// (get) Token: 0x06000049 RID: 73 RVA: 0x00002EE9 File Offset: 0x000010E9
			// (set) Token: 0x0600004A RID: 74 RVA: 0x00002EF1 File Offset: 0x000010F1
			public int Index { get; set; }

			// Token: 0x0400001A RID: 26
			public List<BindableControl.DependeePropertyInfo> Dependees = new List<BindableControl.DependeePropertyInfo>();
		}
	}
}
