using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFShared
{
	// Token: 0x0200001E RID: 30
	public class TreeList : ItemsControl
	{
		// Token: 0x0600011D RID: 285 RVA: 0x00005EF0 File Offset: 0x000040F0
		static TreeList()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeList), new FrameworkPropertyMetadata(typeof(TreeList)));
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000612C File Offset: 0x0000432C
		public TreeList()
		{
			this.Rows = new TreeCollection();
			base.ItemsSource = this.Rows;
			this.Rows.TreeSelectionChanged += delegate(object o, TreeCollection.TreeSelectionChangedEventArgs e)
			{
				this.OnTreeSelectionChanged(e);
			};
			this.Rows.CollectionChanged += delegate(object o, NotifyCollectionChangedEventArgs e)
			{
				this.OnCollectionChanged(e);
			};
			base.AddHandler(TreeListItem.ExpandedEvent, new RoutedEventHandler(delegate(object o, RoutedEventArgs e)
			{
				TreeList treeList = o as TreeList;
				TreeListItem treeListItem = e.OriginalSource as TreeListItem;
				if (treeList == null || treeListItem == null || treeListItem.Node == null)
				{
					return;
				}
				this.Rows.ExpandNode(treeListItem.Node);
			}));
			base.AddHandler(TreeListItem.CollapsedEvent, new RoutedEventHandler(delegate(object o, RoutedEventArgs e)
			{
				TreeList treeList2 = o as TreeList;
				TreeListItem treeListItem2 = e.OriginalSource as TreeListItem;
				if (treeList2 == null || treeListItem2 == null || treeListItem2.Node == null)
				{
					return;
				}
				this.Rows.CollapseNode(treeListItem2.Node);
			}));
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00006214 File Offset: 0x00004414
		// (set) Token: 0x06000120 RID: 288 RVA: 0x0000621C File Offset: 0x0000441C
		public TreeCollection Rows { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00006225 File Offset: 0x00004425
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00006237 File Offset: 0x00004437
		public ITreeItem RootItem
		{
			get
			{
				return (ITreeItem)base.GetValue(TreeList.RootItemProperty);
			}
			set
			{
				base.SetValue(TreeList.RootItemProperty, value);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00006245 File Offset: 0x00004445
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00006257 File Offset: 0x00004457
		public int Indention
		{
			get
			{
				return (int)base.GetValue(TreeList.IndentionProperty);
			}
			set
			{
				base.SetValue(TreeList.IndentionProperty, value);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000125 RID: 293 RVA: 0x0000626A File Offset: 0x0000446A
		// (set) Token: 0x06000126 RID: 294 RVA: 0x0000627C File Offset: 0x0000447C
		public ImageSource CollapsedImage
		{
			get
			{
				return (ImageSource)base.GetValue(TreeList.CollapsedImageProperty);
			}
			set
			{
				base.SetValue(TreeList.CollapsedImageProperty, value);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000127 RID: 295 RVA: 0x0000628A File Offset: 0x0000448A
		// (set) Token: 0x06000128 RID: 296 RVA: 0x0000629C File Offset: 0x0000449C
		public ImageSource ExpandedImage
		{
			get
			{
				return (ImageSource)base.GetValue(TreeList.ExpandedImageProperty);
			}
			set
			{
				base.SetValue(TreeList.ExpandedImageProperty, value);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000129 RID: 297 RVA: 0x000062AA File Offset: 0x000044AA
		// (set) Token: 0x0600012A RID: 298 RVA: 0x000062BC File Offset: 0x000044BC
		public Thickness ImagePadding
		{
			get
			{
				return (Thickness)base.GetValue(TreeList.ImagePaddingProperty);
			}
			set
			{
				base.SetValue(TreeList.ImagePaddingProperty, value);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600012B RID: 299 RVA: 0x000062CF File Offset: 0x000044CF
		// (set) Token: 0x0600012C RID: 300 RVA: 0x000062E1 File Offset: 0x000044E1
		public int HoverExpandDelay
		{
			get
			{
				return (int)base.GetValue(TreeList.HoverExpandDelayProperty);
			}
			set
			{
				base.SetValue(TreeList.HoverExpandDelayProperty, value);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600012D RID: 301 RVA: 0x000062F4 File Offset: 0x000044F4
		// (set) Token: 0x0600012E RID: 302 RVA: 0x000062FC File Offset: 0x000044FC
		private TreeCollection TreeNodes { get; set; }

		// Token: 0x0600012F RID: 303 RVA: 0x00006305 File Offset: 0x00004505
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == TreeList.RootItemProperty)
			{
				this.Rows.RootItem = this.RootItem;
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000633C File Offset: 0x0000453C
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left && this.SelectionMode == SelectionMode.Extended)
			{
				if (this.NodeWasSelected && this.ModifierKey == Key.LeftCtrl && this.LastSelectedNode != null)
				{
					this.Rows.UnselectNode(this.LastSelectedNode);
					return;
				}
				if (this.ModifierKey == Key.None && this.LastSelectedNode != null)
				{
					this.Rows.UnselectNodes(this.Rows.SelectedNodes.Where((TreeNode o) => o != this.LastSelectedNode));
					this.Rows.SelectNode(this.LastSelectedNode);
				}
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006424 File Offset: 0x00004624
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			TreeListItem treeListItem = ((DependencyObject)e.OriginalSource).VisualUpwardSearch<TreeListItem>();
			if (treeListItem == null)
			{
				if (this.SelectionMode != SelectionMode.Multiple)
				{
					this.Rows.ClearSelection();
					this.LastSelectedNode = null;
				}
			}
			else
			{
				TreeNode node = treeListItem.Node;
				switch (this.SelectionMode)
				{
				case SelectionMode.Single:
					this.Rows.UnselectNodes(this.Rows.SelectedNodes.Where((TreeNode o) => o != node));
					this.Rows.SelectNode(node);
					break;
				case SelectionMode.Multiple:
					if (node.IsSelected)
					{
						this.Rows.UnselectNode(node);
					}
					else
					{
						this.Rows.SelectNode(node);
					}
					break;
				case SelectionMode.Extended:
				{
					bool flag = e.ChangedButton == MouseButton.Left && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift));
					bool flag2 = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
					this.NodeWasSelected = node.IsSelected;
					if (flag2)
					{
						this.ModifierKey = Key.LeftCtrl;
						if (e.ChangedButton == MouseButton.Left)
						{
							if (!node.IsSelected)
							{
								this.Rows.SelectNode(node);
							}
							this.LastSelectedNode = node;
						}
					}
					else if (flag)
					{
						this.ModifierKey = Key.LeftShift;
						if (this.LastSelectedNode == null || this.LastSelectedNode.ParentNode != node.ParentNode)
						{
							this.Rows.UnselectNodes(this.Rows.SelectedNodes.Where((TreeNode o) => o != node));
							this.Rows.SelectNode(node);
							this.LastSelectedNode = node;
						}
						else
						{
							int num = this.Rows.IndexOf(this.LastSelectedNode);
							int num2 = this.Rows.IndexOf(node);
							int num3 = Math.Sign(num2 - num);
							List<TreeNode> nodes = new List<TreeNode>();
							if (num != num2)
							{
								for (int i = 0; i < Math.Abs(num - num2) + 1; i++)
								{
									TreeNode treeNode = this.Rows[num + i * num3];
									if (treeNode.Depth == this.LastSelectedNode.Depth)
									{
										nodes.Add(treeNode);
									}
								}
								this.Rows.UnselectNodes(this.Rows.SelectedNodes.Where((TreeNode o) => !nodes.Contains(o)));
								this.Rows.SelectNodes(nodes);
							}
						}
					}
					else
					{
						this.ModifierKey = Key.None;
						if (e.ChangedButton == MouseButton.Left || !this.Rows.SelectedNodes.Contains(node))
						{
							this.LastSelectedNode = node;
							if (!node.IsSelected)
							{
								this.Rows.UnselectNodes(this.Rows.SelectedNodes.Where((TreeNode o) => o != node).ToList<TreeNode>());
								this.Rows.SelectNode(node);
							}
						}
					}
					break;
				}
				}
			}
			e.Handled = true;
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000132 RID: 306 RVA: 0x000067B8 File Offset: 0x000049B8
		// (remove) Token: 0x06000133 RID: 307 RVA: 0x000067F0 File Offset: 0x000049F0
		public event TreeCollection.TreeSelectionChangedEventHandler TreeSelectionChanged = delegate(object o, TreeCollection.TreeSelectionChangedEventArgs e)
		{
		};

		// Token: 0x06000134 RID: 308 RVA: 0x00006825 File Offset: 0x00004A25
		private void OnTreeSelectionChanged(TreeCollection.TreeSelectionChangedEventArgs e)
		{
			this.TreeSelectionChanged(this, e);
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000135 RID: 309 RVA: 0x00006834 File Offset: 0x00004A34
		// (remove) Token: 0x06000136 RID: 310 RVA: 0x0000686C File Offset: 0x00004A6C
		public event NotifyCollectionChangedEventHandler CollectionChanged = delegate(object o, NotifyCollectionChangedEventArgs e)
		{
		};

		// Token: 0x06000137 RID: 311 RVA: 0x000068A1 File Offset: 0x00004AA1
		private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			this.CollectionChanged(this, e);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000068B0 File Offset: 0x00004AB0
		public bool FocusNextNode(TreeListItem container)
		{
			int num = this.Rows.IndexOf(container.Node);
			if (num < this.Rows.Count - 1)
			{
				container = (TreeListItem)base.ItemContainerGenerator.ContainerFromItem(this.Rows[num + 1]);
				container.Focus();
				return true;
			}
			return false;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000690C File Offset: 0x00004B0C
		public bool FocusPreviousNode(TreeListItem container)
		{
			int num = this.Rows.IndexOf(container.Node);
			if (num > 0)
			{
				container = (TreeListItem)base.ItemContainerGenerator.ContainerFromItem(this.Rows[num - 1]);
				container.Focus();
				return true;
			}
			return false;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006959 File Offset: 0x00004B59
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TreeListItem;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006964 File Offset: 0x00004B64
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TreeListItem();
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000696C File Offset: 0x00004B6C
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			if (element != null && element is TreeListItem && item is TreeNode)
			{
				((TreeListItem)element).Node = (TreeNode)item;
				(item as TreeNode).Collection = this.Rows;
			}
			if (element != null && element is TreeListItem)
			{
				((TreeListItem)element).Tree = this;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000069CD File Offset: 0x00004BCD
		// (set) Token: 0x0600013E RID: 318 RVA: 0x000069DF File Offset: 0x00004BDF
		public SelectionMode SelectionMode
		{
			get
			{
				return (SelectionMode)base.GetValue(TreeList.SelectionModeProperty);
			}
			set
			{
				base.SetValue(TreeList.SelectionModeProperty, value);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600013F RID: 319 RVA: 0x000069F4 File Offset: 0x00004BF4
		// (remove) Token: 0x06000140 RID: 320 RVA: 0x00006A2C File Offset: 0x00004C2C
		public event TreeList.QueryCanDropHandler QueryCanDrop;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000141 RID: 321 RVA: 0x00006A64 File Offset: 0x00004C64
		// (remove) Token: 0x06000142 RID: 322 RVA: 0x00006A9C File Offset: 0x00004C9C
		public event TreeList.DropItemsHandler DropItems;

		// Token: 0x06000143 RID: 323 RVA: 0x00006AD4 File Offset: 0x00004CD4
		protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this._StartingItem = null;
				this._StartingPoint = null;
				TreeListItem treeListItem = ((DependencyObject)e.OriginalSource).VisualUpwardSearch<TreeListItem>();
				if (treeListItem != null)
				{
					Path path = ((DependencyObject)e.OriginalSource).VisualUpwardSearch<Path>();
					if (path == null || path.Name != "DrillPath")
					{
						this._StartingPoint = new Point?(e.GetPosition(this));
						this._StartingItem = treeListItem;
					}
				}
				this._IsDragging = false;
				this._DragHoverTarget = null;
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00006B5D File Offset: 0x00004D5D
		protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
		{
			this._StartingPoint = null;
			base.OnPreviewMouseUp(e);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006B74 File Offset: 0x00004D74
		protected override void OnPreviewMouseMove(MouseEventArgs e)
		{
			if (this._StartingPoint == null)
			{
				return;
			}
			if (e.LeftButton == MouseButtonState.Pressed && !this._IsDragging)
			{
				TreeListItem treeListItem = ((DependencyObject)e.OriginalSource).VisualUpwardSearch<TreeListItem>();
				Point position = e.GetPosition(this);
				double num = Math.Pow(position.X - this._StartingPoint.Value.X, 2.0) + Math.Pow(position.Y - this._StartingPoint.Value.Y, 2.0);
				if (treeListItem != null && num > 256.0)
				{
					DataObject dataObject = new DataObject(this.Rows);
					dataObject.SetData("DragObject", this.Rows);
					DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Move);
					this._IsDragging = true;
				}
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006C54 File Offset: 0x00004E54
		private TreeList.DropItemsEventArgs GetDropArgs(DragEventArgs e)
		{
			DependencyObject dependencyObject = base.InputHitTest(e.GetPosition(this)) as DependencyObject;
			TreeListItem treeListItem = ((dependencyObject == null) ? null : dependencyObject.VisualUpwardSearch<TreeListItem>());
			TreeNode treeNode = ((treeListItem != null) ? treeListItem.Node : null);
			TreeCollection treeCollection = ((DataObject)e.Data).GetData("DragObject") as TreeCollection;
			if (treeCollection == null)
			{
				return null;
			}
			return new TreeList.DropItemsEventArgs(treeCollection, treeNode);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006CB8 File Offset: 0x00004EB8
		protected override void OnDragOver(DragEventArgs e)
		{
			e.Handled = true;
			ScrollViewer scrollViewer = this.FindVisualChild<ScrollViewer>();
			double num = 25.0;
			double y = e.GetPosition(this).Y;
			double num2 = 25.0;
			if (y < num && y > 0.0)
			{
				double verticalOffset = scrollViewer.VerticalOffset;
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - num2);
				if (scrollViewer.VerticalOffset != verticalOffset)
				{
					return;
				}
			}
			else if (y > base.ActualHeight - num && scrollViewer.VerticalOffset != scrollViewer.ScrollableHeight)
			{
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + num2);
				return;
			}
			TreeList.DropItemsEventArgs dropArgs = this.GetDropArgs(e);
			if (dropArgs == null)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			TreeList.QueryCanDropItemsEventArgs queryCanDropItemsEventArgs = new TreeList.QueryCanDropItemsEventArgs(dropArgs);
			if (queryCanDropItemsEventArgs.DraggedItems.Contains(queryCanDropItemsEventArgs.DropItem))
			{
				e.Effects = DragDropEffects.None;
			}
			else if (this.QueryCanDrop != null)
			{
				this.QueryCanDrop(this, queryCanDropItemsEventArgs);
				e.Effects = (queryCanDropItemsEventArgs.CanDrop ? DragDropEffects.Move : DragDropEffects.None);
			}
			else
			{
				e.Effects = DragDropEffects.None;
			}
			if (queryCanDropItemsEventArgs.DropNode != null && !queryCanDropItemsEventArgs.DropNode.IsExpanded && queryCanDropItemsEventArgs.DropNode == this._DragHoverTarget)
			{
				if ((DateTime.Now - this._DragHoverTime).TotalMilliseconds > (double)this.HoverExpandDelay)
				{
					queryCanDropItemsEventArgs.DropNode.IsExpanded = true;
					this._DragHoverTarget = null;
					return;
				}
			}
			else
			{
				this._DragHoverTarget = queryCanDropItemsEventArgs.DropNode;
				this._DragHoverTime = DateTime.Now;
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006E34 File Offset: 0x00005034
		protected override void OnDrop(DragEventArgs e)
		{
			TreeList.DropItemsEventArgs dropArgs = this.GetDropArgs(e);
			if (this.DropItems != null)
			{
				this.DropItems(this, dropArgs);
			}
			e.Handled = true;
		}

		// Token: 0x04000062 RID: 98
		public static readonly DependencyProperty RootItemProperty = DependencyProperty.Register("RootItem", typeof(ITreeItem), typeof(TreeList), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

		// Token: 0x04000063 RID: 99
		public static readonly DependencyProperty IndentionProperty = DependencyProperty.Register("Indention", typeof(int), typeof(TreeList), new FrameworkPropertyMetadata(20, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

		// Token: 0x04000064 RID: 100
		public static readonly DependencyProperty CollapsedImageProperty = DependencyProperty.Register("CollapsedImage", typeof(ImageSource), typeof(TreeList), new FrameworkPropertyMetadata(new BitmapImage(new Uri("pack://application:,,,/WPFShared;component/Resources/SpinnerDown.png"))));

		// Token: 0x04000065 RID: 101
		public static readonly DependencyProperty ExpandedImageProperty = DependencyProperty.Register("ExpandedImage", typeof(ImageSource), typeof(TreeList), new FrameworkPropertyMetadata(new BitmapImage(new Uri("pack://application:,,,/WPFShared;component/Resources/SpinnerUp.png"))));

		// Token: 0x04000066 RID: 102
		public static readonly DependencyProperty ImagePaddingProperty = DependencyProperty.Register("ImagePadding", typeof(Thickness), typeof(TreeList), new FrameworkPropertyMetadata(new Thickness(0.0)));

		// Token: 0x04000067 RID: 103
		public static readonly DependencyProperty HoverExpandDelayProperty = DependencyProperty.Register("HoverExpandDelay", typeof(int), typeof(TreeList), new FrameworkPropertyMetadata(750, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

		// Token: 0x0400006A RID: 106
		public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(TreeList), new FrameworkPropertyMetadata(SelectionMode.Single));

		// Token: 0x0400006B RID: 107
		private TreeNode LastSelectedNode;

		// Token: 0x0400006C RID: 108
		private bool NodeWasSelected;

		// Token: 0x0400006D RID: 109
		private Key ModifierKey;

		// Token: 0x0400006E RID: 110
		private Point? _StartingPoint;

		// Token: 0x0400006F RID: 111
		private TreeListItem _StartingItem;

		// Token: 0x04000070 RID: 112
		private bool _IsDragging;

		// Token: 0x04000071 RID: 113
		private TreeNode _DragHoverTarget;

		// Token: 0x04000072 RID: 114
		private DateTime _DragHoverTime;

		// Token: 0x0200001F RID: 31
		public class DropItemsEventArgs : EventArgs
		{
			// Token: 0x06000150 RID: 336 RVA: 0x00006E65 File Offset: 0x00005065
			public DropItemsEventArgs(TreeCollection collection, TreeNode targetNode)
			{
				this.DraggedNodes = ((collection != null) ? collection.SelectedNodes.ToList<TreeNode>() : null);
				this.DropNode = targetNode;
			}

			// Token: 0x06000151 RID: 337 RVA: 0x00006E8B File Offset: 0x0000508B
			public DropItemsEventArgs()
			{
			}

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x06000152 RID: 338 RVA: 0x00006E93 File Offset: 0x00005093
			// (set) Token: 0x06000153 RID: 339 RVA: 0x00006E9B File Offset: 0x0000509B
			public IEnumerable<TreeNode> DraggedNodes { get; set; }

			// Token: 0x17000047 RID: 71
			// (get) Token: 0x06000154 RID: 340 RVA: 0x00006EA4 File Offset: 0x000050A4
			// (set) Token: 0x06000155 RID: 341 RVA: 0x00006EAC File Offset: 0x000050AC
			public TreeNode DropNode { get; set; }

			// Token: 0x17000048 RID: 72
			// (get) Token: 0x06000156 RID: 342 RVA: 0x00007050 File Offset: 0x00005250
			public IEnumerable<ITreeItem> DraggedItems
			{
				get
				{
					foreach (TreeNode node in this.DraggedNodes)
					{
						yield return node.DataItem;
					}
					yield break;
				}
			}

			// Token: 0x17000049 RID: 73
			// (get) Token: 0x06000157 RID: 343 RVA: 0x0000706D File Offset: 0x0000526D
			public ITreeItem DropItem
			{
				get
				{
					if (this.DropNode == null)
					{
						return null;
					}
					return this.DropNode.DataItem;
				}
			}
		}

		// Token: 0x02000020 RID: 32
		public class QueryCanDropItemsEventArgs : TreeList.DropItemsEventArgs
		{
			// Token: 0x06000158 RID: 344 RVA: 0x00007084 File Offset: 0x00005284
			public QueryCanDropItemsEventArgs(TreeList.DropItemsEventArgs dropArgs)
			{
				base.DraggedNodes = dropArgs.DraggedNodes;
				base.DropNode = dropArgs.DropNode;
				this.CanDrop = false;
			}

			// Token: 0x1700004A RID: 74
			// (get) Token: 0x06000159 RID: 345 RVA: 0x000070AB File Offset: 0x000052AB
			// (set) Token: 0x0600015A RID: 346 RVA: 0x000070B3 File Offset: 0x000052B3
			public bool CanDrop { get; set; }
		}

		// Token: 0x02000021 RID: 33
		// (Invoke) Token: 0x0600015C RID: 348
		public delegate void QueryCanDropHandler(object sender, TreeList.QueryCanDropItemsEventArgs e);

		// Token: 0x02000022 RID: 34
		// (Invoke) Token: 0x06000160 RID: 352
		public delegate void DropItemsHandler(object sender, TreeList.DropItemsEventArgs e);
	}
}
