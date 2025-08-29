using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace GalaSoft.MvvmLight
{
	// Token: 0x02000015 RID: 21
	public abstract class ViewModelBase : INotifyPropertyChanged, ICleanup, IDisposable
	{
		// Token: 0x06000077 RID: 119 RVA: 0x0000315A File Offset: 0x0000135A
		protected ViewModelBase()
			: this(null)
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003166 File Offset: 0x00001366
		protected ViewModelBase(IMessenger messenger)
		{
			this.MessengerInstance = messenger;
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000079 RID: 121 RVA: 0x0000317C File Offset: 0x0000137C
		// (remove) Token: 0x0600007A RID: 122 RVA: 0x000031B8 File Offset: 0x000013B8
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000031F4 File Offset: 0x000013F4
		public static bool IsInDesignModeStatic
		{
			get
			{
				if (ViewModelBase._isInDesignMode == null)
				{
					DependencyProperty isInDesignModeProperty = DesignerProperties.IsInDesignModeProperty;
					ViewModelBase._isInDesignMode = new bool?((bool)DependencyPropertyDescriptor.FromProperty(isInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue);
					if (!ViewModelBase._isInDesignMode.Value && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal))
					{
						ViewModelBase._isInDesignMode = new bool?(true);
					}
				}
				return ViewModelBase._isInDesignMode.Value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003288 File Offset: 0x00001488
		public bool IsInDesignMode
		{
			get
			{
				return ViewModelBase.IsInDesignModeStatic;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000032A0 File Offset: 0x000014A0
		// (set) Token: 0x0600007E RID: 126 RVA: 0x000032B7 File Offset: 0x000014B7
		protected IMessenger MessengerInstance { get; set; }

		// Token: 0x0600007F RID: 127 RVA: 0x000032C0 File Offset: 0x000014C0
		protected virtual void Broadcast<T>(T oldValue, T newValue, string propertyName)
		{
			PropertyChangedMessage<T> propertyChangedMessage = new PropertyChangedMessage<T>(this, oldValue, newValue, propertyName);
			if (this.MessengerInstance != null)
			{
				this.MessengerInstance.Send<PropertyChangedMessage<T>>(propertyChangedMessage);
			}
			else
			{
				Messenger.Default.Send<PropertyChangedMessage<T>>(propertyChangedMessage);
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003304 File Offset: 0x00001504
		[Obsolete("This interface will be removed from ViewModelBase in a future version, use ICleanup.Cleanup instead.")]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Cleanup();
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003323 File Offset: 0x00001523
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000332E File Offset: 0x0000152E
		public virtual void Cleanup()
		{
			Messenger.Default.Unregister(this);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003340 File Offset: 0x00001540
		protected virtual void RaisePropertyChanged<T>(string propertyName, T oldValue, T newValue, bool broadcast)
		{
			this.RaisePropertyChanged(propertyName);
			if (broadcast)
			{
				this.Broadcast<T>(oldValue, newValue, propertyName);
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000336C File Offset: 0x0000156C
		protected virtual void RaisePropertyChanged(string propertyName)
		{
			this.VerifyPropertyName(propertyName);
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000033A4 File Offset: 0x000015A4
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public void VerifyPropertyName(string propertyName)
		{
			Type type = base.GetType();
			if (type.GetProperty(propertyName) == null)
			{
				throw new ArgumentException("Property not found", propertyName);
			}
		}

		// Token: 0x0400001C RID: 28
		private static bool? _isInDesignMode;
	}
}
