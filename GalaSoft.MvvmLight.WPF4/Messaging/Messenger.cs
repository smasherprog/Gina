using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Helpers;

namespace GalaSoft.MvvmLight.Messaging
{
	// Token: 0x02000006 RID: 6
	public class Messenger : IMessenger
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000021A0 File Offset: 0x000003A0
		public static Messenger Default
		{
			get
			{
				if (Messenger._defaultInstance == null)
				{
					Messenger._defaultInstance = new Messenger();
				}
				return Messenger._defaultInstance;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000021D2 File Offset: 0x000003D2
		public static void OverrideDefault(Messenger newMessenger)
		{
			Messenger._defaultInstance = newMessenger;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000021DB File Offset: 0x000003DB
		public static void Reset()
		{
			Messenger._defaultInstance = null;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000021E4 File Offset: 0x000003E4
		public virtual void Register<TMessage>(object recipient, Action<TMessage> action)
		{
			this.Register<TMessage>(recipient, null, false, action);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000021F2 File Offset: 0x000003F2
		public virtual void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action)
		{
			this.Register<TMessage>(recipient, null, receiveDerivedMessagesToo, action);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002200 File Offset: 0x00000400
		public virtual void Register<TMessage>(object recipient, object token, Action<TMessage> action)
		{
			this.Register<TMessage>(recipient, token, false, action);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002210 File Offset: 0x00000410
		public virtual void Register<TMessage>(object recipient, object token, bool receiveDerivedMessagesToo, Action<TMessage> action)
		{
			Type typeFromHandle = typeof(TMessage);
			Dictionary<Type, List<Messenger.WeakActionAndToken>> dictionary;
			if (receiveDerivedMessagesToo)
			{
				if (this._recipientsOfSubclassesAction == null)
				{
					this._recipientsOfSubclassesAction = new Dictionary<Type, List<Messenger.WeakActionAndToken>>();
				}
				dictionary = this._recipientsOfSubclassesAction;
			}
			else
			{
				if (this._recipientsStrictAction == null)
				{
					this._recipientsStrictAction = new Dictionary<Type, List<Messenger.WeakActionAndToken>>();
				}
				dictionary = this._recipientsStrictAction;
			}
			List<Messenger.WeakActionAndToken> list;
			if (!dictionary.ContainsKey(typeFromHandle))
			{
				list = new List<Messenger.WeakActionAndToken>();
				dictionary.Add(typeFromHandle, list);
			}
			else
			{
				list = dictionary[typeFromHandle];
			}
			WeakAction<TMessage> weakAction = new WeakAction<TMessage>(recipient, action);
			Messenger.WeakActionAndToken weakActionAndToken = new Messenger.WeakActionAndToken
			{
				Action = weakAction,
				Token = token
			};
			list.Add(weakActionAndToken);
			this.Cleanup();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000022E4 File Offset: 0x000004E4
		public virtual void Send<TMessage>(TMessage message)
		{
			this.SendToTargetOrType<TMessage>(message, null, null);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000022F1 File Offset: 0x000004F1
		public virtual void Send<TMessage, TTarget>(TMessage message)
		{
			this.SendToTargetOrType<TMessage>(message, typeof(TTarget), null);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002307 File Offset: 0x00000507
		public virtual void Send<TMessage>(TMessage message, object token)
		{
			this.SendToTargetOrType<TMessage>(message, null, token);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002314 File Offset: 0x00000514
		public virtual void Unregister(object recipient)
		{
			Messenger.UnregisterFromLists(recipient, this._recipientsOfSubclassesAction);
			Messenger.UnregisterFromLists(recipient, this._recipientsStrictAction);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002331 File Offset: 0x00000531
		public virtual void Unregister<TMessage>(object recipient)
		{
			this.Unregister<TMessage>(recipient, null);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000233D File Offset: 0x0000053D
		public virtual void Unregister<TMessage>(object recipient, Action<TMessage> action)
		{
			Messenger.UnregisterFromLists<TMessage>(recipient, action, this._recipientsStrictAction);
			Messenger.UnregisterFromLists<TMessage>(recipient, action, this._recipientsOfSubclassesAction);
			this.Cleanup();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002364 File Offset: 0x00000564
		private static void CleanupList(IDictionary<Type, List<Messenger.WeakActionAndToken>> lists)
		{
			if (lists != null)
			{
				List<Type> list = new List<Type>();
				foreach (KeyValuePair<Type, List<Messenger.WeakActionAndToken>> keyValuePair in lists)
				{
					List<Messenger.WeakActionAndToken> list2 = new List<Messenger.WeakActionAndToken>();
					foreach (Messenger.WeakActionAndToken weakActionAndToken in keyValuePair.Value)
					{
						if (weakActionAndToken.Action == null || !weakActionAndToken.Action.IsAlive)
						{
							list2.Add(weakActionAndToken);
						}
					}
					foreach (Messenger.WeakActionAndToken weakActionAndToken2 in list2)
					{
						keyValuePair.Value.Remove(weakActionAndToken2);
					}
					if (keyValuePair.Value.Count == 0)
					{
						list.Add(keyValuePair.Key);
					}
				}
				foreach (Type type in list)
				{
					lists.Remove(type);
				}
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002510 File Offset: 0x00000710
		private static bool Implements(Type instanceType, Type interfaceType)
		{
			bool flag;
			if (interfaceType == null || instanceType == null)
			{
				flag = false;
			}
			else
			{
				Type[] interfaces = instanceType.GetInterfaces();
				foreach (Type type in interfaces)
				{
					if (type == interfaceType)
					{
						return true;
					}
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002580 File Offset: 0x00000780
		private static void SendToList<TMessage>(TMessage message, IEnumerable<Messenger.WeakActionAndToken> list, Type messageTargetType, object token)
		{
			if (list != null)
			{
				List<Messenger.WeakActionAndToken> list2 = list.Take(list.Count<Messenger.WeakActionAndToken>()).ToList<Messenger.WeakActionAndToken>();
				foreach (Messenger.WeakActionAndToken weakActionAndToken in list2)
				{
					IExecuteWithObject executeWithObject = weakActionAndToken.Action as IExecuteWithObject;
					if (executeWithObject != null && weakActionAndToken.Action.IsAlive && weakActionAndToken.Action.Target != null && (messageTargetType == null || weakActionAndToken.Action.Target.GetType() == messageTargetType || Messenger.Implements(weakActionAndToken.Action.Target.GetType(), messageTargetType)) && ((weakActionAndToken.Token == null && token == null) || (weakActionAndToken.Token != null && weakActionAndToken.Token.Equals(token))))
					{
						executeWithObject.ExecuteWithObject(message);
					}
				}
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000026A0 File Offset: 0x000008A0
		private static void UnregisterFromLists(object recipient, Dictionary<Type, List<Messenger.WeakActionAndToken>> lists)
		{
			if (recipient != null && lists != null && lists.Count != 0)
			{
				lock (lists)
				{
					foreach (Type type in lists.Keys)
					{
						foreach (Messenger.WeakActionAndToken weakActionAndToken in lists[type])
						{
							WeakAction action = weakActionAndToken.Action;
							if (action != null && recipient == action.Target)
							{
								action.MarkForDeletion();
							}
						}
					}
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000027BC File Offset: 0x000009BC
		private static void UnregisterFromLists<TMessage>(object recipient, Action<TMessage> action, Dictionary<Type, List<Messenger.WeakActionAndToken>> lists)
		{
			Type typeFromHandle = typeof(TMessage);
			if (recipient != null && lists != null && lists.Count != 0 && lists.ContainsKey(typeFromHandle))
			{
				lock (lists)
				{
					foreach (Messenger.WeakActionAndToken weakActionAndToken in lists[typeFromHandle])
					{
						WeakAction<TMessage> weakAction = weakActionAndToken.Action as WeakAction<TMessage>;
						if (weakAction != null && recipient == weakAction.Target && (action == null || action == weakAction.Action))
						{
							weakActionAndToken.Action.MarkForDeletion();
						}
					}
				}
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000028BC File Offset: 0x00000ABC
		private void Cleanup()
		{
			Messenger.CleanupList(this._recipientsOfSubclassesAction);
			Messenger.CleanupList(this._recipientsStrictAction);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000028D8 File Offset: 0x00000AD8
		private void SendToTargetOrType<TMessage>(TMessage message, Type messageTargetType, object token)
		{
			Type typeFromHandle = typeof(TMessage);
			if (this._recipientsOfSubclassesAction != null)
			{
				List<Type> list = this._recipientsOfSubclassesAction.Keys.Take(this._recipientsOfSubclassesAction.Count<KeyValuePair<Type, List<Messenger.WeakActionAndToken>>>()).ToList<Type>();
				foreach (Type type in list)
				{
					List<Messenger.WeakActionAndToken> list2 = null;
					if (typeFromHandle == type || typeFromHandle.IsSubclassOf(type) || Messenger.Implements(typeFromHandle, type))
					{
						list2 = this._recipientsOfSubclassesAction[type];
					}
					Messenger.SendToList<TMessage>(message, list2, messageTargetType, token);
				}
			}
			if (this._recipientsStrictAction != null)
			{
				if (this._recipientsStrictAction.ContainsKey(typeFromHandle))
				{
					List<Messenger.WeakActionAndToken> list2 = this._recipientsStrictAction[typeFromHandle];
					Messenger.SendToList<TMessage>(message, list2, messageTargetType, token);
				}
			}
			this.Cleanup();
		}

		// Token: 0x04000004 RID: 4
		private static Messenger _defaultInstance;

		// Token: 0x04000005 RID: 5
		private Dictionary<Type, List<Messenger.WeakActionAndToken>> _recipientsOfSubclassesAction;

		// Token: 0x04000006 RID: 6
		private Dictionary<Type, List<Messenger.WeakActionAndToken>> _recipientsStrictAction;

		// Token: 0x02000007 RID: 7
		private struct WeakActionAndToken
		{
			// Token: 0x04000007 RID: 7
			public WeakAction Action;

			// Token: 0x04000008 RID: 8
			public object Token;
		}
	}
}
