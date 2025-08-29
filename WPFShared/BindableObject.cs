using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace WPFShared
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	public abstract class BindableObject : DependencyObject, INotifyPropertyChanged, IDisposable
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00003658 File Offset: 0x00001858
		protected static void SetDependentProperties(Type type)
		{
			Stack<Type> stack = new Stack<Type>();
			Type type2 = type;
			do
			{
				stack.Push(type2);
				type2 = type2.BaseType;
			}
			while (type2 != typeof(BindableObject));
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

		// Token: 0x0600006B RID: 107 RVA: 0x000036E8 File Offset: 0x000018E8
		private static BindableObject.BindablePropertyInfo GetPropertyField(Type type, string propertyName)
		{
			lock (typeof(BindableObject))
			{
				if (!BindableObject.PropertyFields.ContainsKey(type))
				{
					BindableObject.PropertyFields.Add(type, new Dictionary<string, BindableObject.BindablePropertyInfo>());
					BindableObject.DependentProperties.Add(type, new Dictionary<string, List<string>>());
				}
			}
			BindableObject.BindablePropertyInfo bindablePropertyInfo;
			lock (type)
			{
				if (!BindableObject.PropertyFields[type].ContainsKey(propertyName))
				{
					BindableObject.PropertyFields[type].Add(propertyName, new BindableObject.BindablePropertyInfo
					{
						Index = BindableObject.PropertyFields[type].Keys.Count + 1
					});
				}
				bindablePropertyInfo = BindableObject.PropertyFields[type][propertyName];
			}
			return bindablePropertyInfo;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000037D8 File Offset: 0x000019D8
		private static int GetPropertyFieldIndex(Type type, string propertyName)
		{
			return BindableObject.GetPropertyField(type, propertyName).Index;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003810 File Offset: 0x00001A10
		protected static void RegisterDependentProperty(Type type, string propertyName, string dependeePropertyName, BindableObject.ReferenceLookup lookupMethod)
		{
			BindableObject.BindablePropertyInfo propertyField = BindableObject.GetPropertyField(type, propertyName);
			lock (type)
			{
				BindableObject.DependeePropertyInfo dependeePropertyInfo = propertyField.Dependees.SingleOrDefault((BindableObject.DependeePropertyInfo o) => o.DependeePropertyName == dependeePropertyName);
				if (dependeePropertyInfo == null)
				{
					propertyField.Dependees.Add(dependeePropertyInfo = new BindableObject.DependeePropertyInfo
					{
						DependeePropertyName = dependeePropertyName
					});
					if (!BindableObject.DependentProperties[type].ContainsKey(dependeePropertyName))
					{
						BindableObject.DependentProperties[type][dependeePropertyName] = new List<string> { propertyName };
					}
					else if (!BindableObject.DependentProperties[type][dependeePropertyName].Any((string o) => o == propertyName))
					{
						BindableObject.DependentProperties[type][dependeePropertyName].Add(propertyName);
					}
				}
				dependeePropertyInfo.LookupMethod = lookupMethod;
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003960 File Offset: 0x00001B60
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

		// Token: 0x0600006F RID: 111 RVA: 0x000039B4 File Offset: 0x00001BB4
		private object Get(string propertyName)
		{
			int propertyFieldIndex = BindableObject.GetPropertyFieldIndex(base.GetType(), propertyName);
			this.EnsurePropertyObjectsSize(propertyFieldIndex + 1);
			return this.PropertyObjects[propertyFieldIndex];
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000039E4 File Offset: 0x00001BE4
		protected T Get<T>(string propertyName)
		{
			object obj = this.Get(propertyName);
			if (obj != null)
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003A28 File Offset: 0x00001C28
		protected void Set(string propertyName, object value)
		{
			int propertyFieldIndex = BindableObject.GetPropertyFieldIndex(base.GetType(), propertyName);
			this.EnsurePropertyObjectsSize(propertyFieldIndex + 1);
			this.PropertyObjects[propertyFieldIndex] = value;
			this.RaisePropertyChanged(propertyName);
			if (BindableObject.DependentProperties[base.GetType()].ContainsKey(propertyName))
			{
				foreach (string text in BindableObject.DependentProperties[base.GetType()][propertyName])
				{
					BindableObject.DependeePropertyInfo dependeePropertyInfo = BindableObject.GetPropertyField(base.GetType(), text).Dependees.Single((BindableObject.DependeePropertyInfo o) => o.DependeePropertyName == propertyName);
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

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000072 RID: 114 RVA: 0x00003B44 File Offset: 0x00001D44
		// (remove) Token: 0x06000073 RID: 115 RVA: 0x00003B7C File Offset: 0x00001D7C
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000074 RID: 116 RVA: 0x00003BB4 File Offset: 0x00001DB4
		public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentException("propertyName cannot be null or empty.");
			}
			lock (typeof(BindableObject))
			{
				if (!BindableObject.eventArgCache.ContainsKey(propertyName))
				{
					BindableObject.eventArgCache.Add(propertyName, new PropertyChangedEventArgs(propertyName));
				}
			}
			return BindableObject.eventArgCache[propertyName];
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003C30 File Offset: 0x00001E30
		protected virtual void AfterPropertyChanged(string propertyName)
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003C34 File Offset: 0x00001E34
		protected void RaisePropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, BindableObject.GetPropertyChangedEventArgs(propertyName));
			}
			this.AfterPropertyChanged(propertyName);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003C60 File Offset: 0x00001E60
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

		// Token: 0x06000078 RID: 120 RVA: 0x00003C97 File Offset: 0x00001E97
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003CA6 File Offset: 0x00001EA6
		public virtual void Dispose(bool disposing)
		{
			if (!this._Disposed)
			{
				this._Disposed = true;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003CB8 File Offset: 0x00001EB8
		~BindableObject()
		{
			this.Dispose(false);
		}

		// Token: 0x04000027 RID: 39
		private const string ERROR_MSG = "{0} is not a public property of {1}";

		// Token: 0x04000028 RID: 40
		private static readonly Dictionary<string, PropertyChangedEventArgs> eventArgCache = new Dictionary<string, PropertyChangedEventArgs>();

		// Token: 0x04000029 RID: 41
		private static Dictionary<Type, Dictionary<string, BindableObject.BindablePropertyInfo>> PropertyFields = new Dictionary<Type, Dictionary<string, BindableObject.BindablePropertyInfo>>();

		// Token: 0x0400002A RID: 42
		private static Dictionary<Type, Dictionary<string, List<string>>> DependentProperties = new Dictionary<Type, Dictionary<string, List<string>>>();

		// Token: 0x0400002B RID: 43
		private List<object> PropertyObjects = new List<object>();

		// Token: 0x0400002D RID: 45
		protected bool _Disposed;

		// Token: 0x02000011 RID: 17
		// (Invoke) Token: 0x0600007E RID: 126
		public delegate object ReferenceLookup(object lookupKey, object obj);

		// Token: 0x02000012 RID: 18
		private class DependeePropertyInfo
		{
			// Token: 0x17000010 RID: 16
			// (get) Token: 0x06000081 RID: 129 RVA: 0x00003CFB File Offset: 0x00001EFB
			// (set) Token: 0x06000082 RID: 130 RVA: 0x00003D03 File Offset: 0x00001F03
			public string DependeePropertyName { get; set; }

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x06000083 RID: 131 RVA: 0x00003D0C File Offset: 0x00001F0C
			// (set) Token: 0x06000084 RID: 132 RVA: 0x00003D14 File Offset: 0x00001F14
			public BindableObject.ReferenceLookup LookupMethod { get; set; }
		}

		// Token: 0x02000013 RID: 19
		private class BindablePropertyInfo
		{
			// Token: 0x17000012 RID: 18
			// (get) Token: 0x06000086 RID: 134 RVA: 0x00003D25 File Offset: 0x00001F25
			// (set) Token: 0x06000087 RID: 135 RVA: 0x00003D2D File Offset: 0x00001F2D
			public int Index { get; set; }

			// Token: 0x04000031 RID: 49
			public List<BindableObject.DependeePropertyInfo> Dependees = new List<BindableObject.DependeePropertyInfo>();
		}
	}
}
