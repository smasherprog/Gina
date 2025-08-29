using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using GimaSoft.Business.GINA;
using GimaSoft.GINA.Properties;
using Microsoft.Windows.Controls.Ribbon;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000026 RID: 38
	public partial class MainWindow : RibbonWindow
	{
		// Token: 0x060003CE RID: 974 RVA: 0x0000D240 File Offset: 0x0000B440
		public MainWindow()
		{
			this.InitializeComponent();
			this.SysTrayIcon.Click += delegate(object o, EventArgs e)
			{
				if (Configuration.Current.MinimizeToSystemTray)
				{
					base.Show();
					base.WindowState = WindowState.Normal;
					this.BringToTop();
				}
			};
			Configuration.Current.PropertyChanged += delegate(object o, PropertyChangedEventArgs e)
			{
				if (e.PropertyName == "MinimizeToSystemTray")
				{
					this.SysTrayIcon.Visible = Configuration.Current.MinimizeToSystemTray;
					if (base.WindowState == WindowState.Minimized)
					{
						if (this.SysTrayIcon.Visible)
						{
							base.Hide();
							return;
						}
						base.Show();
					}
				}
			};
			((MainWindowViewModel)base.DataContext).Window = this;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000D2D3 File Offset: 0x0000B4D3
		protected override void OnStateChanged(EventArgs e)
		{
			if (base.WindowState == WindowState.Minimized && Configuration.Current.MinimizeToSystemTray)
			{
				base.Hide();
			}
			base.OnStateChanged(e);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000D2F8 File Offset: 0x0000B4F8
		private void Window_Closing(object sender, CancelEventArgs e)
		{
			this.SysTrayIcon.Visible = false;
			foreach (KeyValuePair<BehaviorGroup, TextWindow> keyValuePair in GINAData.Current.TextBehaviorWindows)
			{
				keyValuePair.Value.Close();
			}
			foreach (KeyValuePair<BehaviorGroup, TimerWindow> keyValuePair2 in GINAData.Current.TimerBehaviorWindows)
			{
				keyValuePair2.Value.Close();
			}
			Settings.Default.MainWindowPlacement = this.GetPlacement();
			Settings.Default.Save();
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000D3C8 File Offset: 0x0000B5C8
		private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(Settings.Default.MainWindowPlacement))
			{
				try
				{
					this.SetPlacement(Settings.Default.MainWindowPlacement);
				}
				catch
				{
				}
			}
			base.Title = global::System.Windows.Forms.Application.ProductName;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000D418 File Offset: 0x0000B618
		private void CharacterListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this._CharacterDragStartLoc = e.GetPosition(null);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000D428 File Offset: 0x0000B628
		private void CharacterListView_PreviewMouseMove(object sender, global::System.Windows.Input.MouseEventArgs e)
		{
			Vector vector = this._CharacterDragStartLoc - e.GetPosition(null);
			if (e.LeftButton == MouseButtonState.Pressed && (Math.Abs(vector.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(vector.Y) > SystemParameters.MinimumVerticalDragDistance))
			{
				DependencyObject dependencyObject = (DependencyObject)e.OriginalSource;
				if (dependencyObject == null)
				{
					return;
				}
				global::System.Windows.Controls.ListViewItem listViewItem = dependencyObject.VisualUpwardSearch<global::System.Windows.Controls.ListViewItem>();
				if (listViewItem == null || listViewItem.DataContext == null)
				{
					return;
				}
				DragDrop.DoDragDrop(listViewItem, new global::System.Windows.DataObject("Character", ((CharacterViewModel)listViewItem.DataContext).Character), global::System.Windows.DragDropEffects.Move);
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000D4BC File Offset: 0x0000B6BC
		private void CharacterListView_DragEnter(object sender, global::System.Windows.DragEventArgs e)
		{
			if (sender == e.Source || !e.Data.GetDataPresent("Character"))
			{
				e.Effects = global::System.Windows.DragDropEffects.None;
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000D4E0 File Offset: 0x0000B6E0
		private void CharacterListView_Drop(object sender, global::System.Windows.DragEventArgs e)
		{
			if (e.Data.GetDataPresent("Character"))
			{
				GINACharacter ginacharacter = e.Data.GetData("Character") as GINACharacter;
				if (ginacharacter == null)
				{
					return;
				}
				int num = GINACharacter.All.IndexOf(ginacharacter);
				global::System.Windows.Controls.ListViewItem listViewItem = ((DependencyObject)this.CharacterListView.InputHitTest(e.GetPosition(this.CharacterListView))).VisualUpwardSearch<global::System.Windows.Controls.ListViewItem>();
				int num2;
				if (listViewItem != null)
				{
					GINACharacter character = ((CharacterViewModel)listViewItem.DataContext).Character;
					if (character == null)
					{
						return;
					}
					num2 = GINACharacter.All.IndexOf(character);
				}
				else
				{
					num2 = GINACharacter.All.Count - 1;
				}
				if (num == -1 || num2 == -1)
				{
					return;
				}
				GINACharacter.All.Move(num, num2);
				e.Handled = true;
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000D5A0 File Offset: 0x0000B7A0
		private void CharacterListView_DragOver(object sender, global::System.Windows.DragEventArgs e)
		{
			e.Effects = (e.Data.GetDataPresent("Character") ? global::System.Windows.DragDropEffects.Move : global::System.Windows.DragDropEffects.None);
			e.Handled = true;
		}

		// Token: 0x040000D7 RID: 215
		private NotifyIcon SysTrayIcon = new NotifyIcon
		{
			Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("GimaSoft.GINA.Resources.GinaTray.ico")),
			Visible = false
		};

		// Token: 0x040000D8 RID: 216
		private global::System.Windows.Point _CharacterDragStartLoc;
	}
}
