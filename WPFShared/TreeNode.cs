using System.Linq;
using System.Windows;

namespace WPFShared
{
	// Token: 0x02000024 RID: 36
	public class TreeNode : BindableObject
	{
		// Token: 0x06000177 RID: 375 RVA: 0x0000754E File Offset: 0x0000574E
		public TreeNode(ITreeItem dataItem, int depth)
		{
			this.DataItem = dataItem;
			this.Depth = depth;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00007564 File Offset: 0x00005764
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00007576 File Offset: 0x00005776
		public bool IsExpanded
		{
			get
			{
				return (bool)base.GetValue(TreeNode.IsExpandedProperty);
			}
			set
			{
				base.SetValue(TreeNode.IsExpandedProperty, value);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00007589 File Offset: 0x00005789
		// (set) Token: 0x0600017B RID: 379 RVA: 0x0000759B File Offset: 0x0000579B
		public ITreeItem DataItem
		{
			get
			{
				return (ITreeItem)base.GetValue(TreeNode.DataItemProperty);
			}
			set
			{
				base.SetValue(TreeNode.DataItemProperty, value);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000075A9 File Offset: 0x000057A9
		// (set) Token: 0x0600017D RID: 381 RVA: 0x000075BB File Offset: 0x000057BB
		public int Depth
		{
			get
			{
				return (int)base.GetValue(TreeNode.DepthProperty);
			}
			set
			{
				base.SetValue(TreeNode.DepthProperty, value);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600017E RID: 382 RVA: 0x000075CE File Offset: 0x000057CE
		// (set) Token: 0x0600017F RID: 383 RVA: 0x000075E0 File Offset: 0x000057E0
		public TreeCollection Collection
		{
			get
			{
				return (TreeCollection)base.GetValue(TreeNode.CollectionProperty);
			}
			set
			{
				base.SetValue(TreeNode.CollectionProperty, value);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000180 RID: 384 RVA: 0x000075EE File Offset: 0x000057EE
		public bool IsSelected
		{
			get
			{
				return this.Collection != null && this.Collection.SelectedNodes.Contains(this);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0000760B File Offset: 0x0000580B
		public bool IsExpandable
		{
			get
			{
				return this.DataItem.HasTreeChildren;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00007618 File Offset: 0x00005818
		public TreeNode ParentNode
		{
			get
			{
				return this.Collection.GetParentNode(this);
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007650 File Offset: 0x00005850
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == TreeNode.CollectionProperty && this.Collection != null)
			{
				this.Collection.TreeSelectionChanged += delegate(object oi, TreeCollection.TreeSelectionChangedEventArgs ei)
				{
					if (ei.AddedNodes.Contains(this) || ei.RemovedNodes.Contains(this))
					{
						base.RaisePropertyChanged("IsSelected");
					}
				};
			}
		}

		// Token: 0x04000083 RID: 131
		public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(TreeNode), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

		// Token: 0x04000084 RID: 132
		public static readonly DependencyProperty DataItemProperty = DependencyProperty.Register("DataItem", typeof(ITreeItem), typeof(TreeNode), null);

		// Token: 0x04000085 RID: 133
		public static readonly DependencyProperty DepthProperty = DependencyProperty.Register("Depth", typeof(int), typeof(TreeNode), null);

		// Token: 0x04000086 RID: 134
		public static readonly DependencyProperty CollectionProperty = DependencyProperty.Register("Collection", typeof(TreeCollection), typeof(TreeNode), null);
	}
}
