using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GalaSoft.MvvmLight.Command
{
	// Token: 0x02000003 RID: 3
	public class EventToCommand : TriggerAction<FrameworkElement>
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002158 File Offset: 0x00000358
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000217A File Offset: 0x0000037A
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(EventToCommand.CommandProperty);
			}
			set
			{
				base.SetValue(EventToCommand.CommandProperty, value);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000218C File Offset: 0x0000038C
		// (set) Token: 0x06000008 RID: 8 RVA: 0x000021A9 File Offset: 0x000003A9
		public object CommandParameter
		{
			get
			{
				return base.GetValue(EventToCommand.CommandParameterProperty);
			}
			set
			{
				base.SetValue(EventToCommand.CommandParameterProperty, value);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021BC File Offset: 0x000003BC
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000021DE File Offset: 0x000003DE
		public object CommandParameterValue
		{
			get
			{
				return this._commandParameterValue ?? this.CommandParameter;
			}
			set
			{
				this._commandParameterValue = value;
				this.EnableDisableElement();
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000021F0 File Offset: 0x000003F0
		// (set) Token: 0x0600000C RID: 12 RVA: 0x00002212 File Offset: 0x00000412
		public bool MustToggleIsEnabled
		{
			get
			{
				return (bool)base.GetValue(EventToCommand.MustToggleIsEnabledProperty);
			}
			set
			{
				base.SetValue(EventToCommand.MustToggleIsEnabledProperty, value);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002228 File Offset: 0x00000428
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0000225A File Offset: 0x0000045A
		public bool MustToggleIsEnabledValue
		{
			get
			{
				return (this._mustToggleValue == null) ? this.MustToggleIsEnabled : this._mustToggleValue.Value;
			}
			set
			{
				this._mustToggleValue = new bool?(value);
				this.EnableDisableElement();
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002270 File Offset: 0x00000470
		protected override void OnAttached()
		{
			base.OnAttached();
			this.EnableDisableElement();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002284 File Offset: 0x00000484
		private FrameworkElement GetAssociatedObject()
		{
			return base.AssociatedObject;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000229C File Offset: 0x0000049C
		private ICommand GetCommand()
		{
			return this.Command;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000022B4 File Offset: 0x000004B4
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000022CB File Offset: 0x000004CB
		public bool PassEventArgsToCommand { get; set; }

		// Token: 0x06000014 RID: 20 RVA: 0x000022D4 File Offset: 0x000004D4
		public void Invoke()
		{
			this.Invoke(null);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022E0 File Offset: 0x000004E0
		protected override void Invoke(object parameter)
		{
			if (!this.AssociatedElementIsDisabled())
			{
				ICommand command = this.GetCommand();
				object obj = this.CommandParameterValue;
				if (obj == null && this.PassEventArgsToCommand)
				{
					obj = parameter;
				}
				if (command != null && command.CanExecute(obj))
				{
					command.Execute(obj);
				}
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002344 File Offset: 0x00000544
		private static void OnCommandChanged(EventToCommand element, DependencyPropertyChangedEventArgs e)
		{
			if (element != null)
			{
				if (e.OldValue != null)
				{
					((ICommand)e.OldValue).CanExecuteChanged -= element.OnCommandCanExecuteChanged;
				}
				ICommand command = (ICommand)e.NewValue;
				if (command != null)
				{
					command.CanExecuteChanged += element.OnCommandCanExecuteChanged;
				}
				element.EnableDisableElement();
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000023C0 File Offset: 0x000005C0
		private bool AssociatedElementIsDisabled()
		{
			FrameworkElement associatedObject = this.GetAssociatedObject();
			return associatedObject != null && !associatedObject.IsEnabled;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000023E8 File Offset: 0x000005E8
		private void EnableDisableElement()
		{
			FrameworkElement associatedObject = this.GetAssociatedObject();
			if (associatedObject != null)
			{
				ICommand command = this.GetCommand();
				if (this.MustToggleIsEnabledValue && command != null)
				{
					associatedObject.IsEnabled = command.CanExecute(this.CommandParameterValue);
				}
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000243A File Offset: 0x0000063A
		private void OnCommandCanExecuteChanged(object sender, EventArgs e)
		{
			this.EnableDisableElement();
		}

		// Token: 0x04000002 RID: 2
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventToCommand), new PropertyMetadata(null, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			EventToCommand eventToCommand = s as EventToCommand;
			if (eventToCommand != null)
			{
				if (eventToCommand.AssociatedObject != null)
				{
					eventToCommand.EnableDisableElement();
				}
			}
		}));

		// Token: 0x04000003 RID: 3
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommand), new PropertyMetadata(null, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			EventToCommand.OnCommandChanged(s as EventToCommand, e);
		}));

		// Token: 0x04000004 RID: 4
		public static readonly DependencyProperty MustToggleIsEnabledProperty = DependencyProperty.Register("MustToggleIsEnabled", typeof(bool), typeof(EventToCommand), new PropertyMetadata(false, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			EventToCommand eventToCommand2 = s as EventToCommand;
			if (eventToCommand2 != null)
			{
				if (eventToCommand2.AssociatedObject != null)
				{
					eventToCommand2.EnableDisableElement();
				}
			}
		}));

		// Token: 0x04000005 RID: 5
		private object _commandParameterValue;

		// Token: 0x04000006 RID: 6
		private bool? _mustToggleValue;
	}
}
