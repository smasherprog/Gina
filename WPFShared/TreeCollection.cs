using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace WPFShared
{
	// Token: 0x02000019 RID: 25
	public class TreeCollection : BindableObject, INotifyCollectionChanged, ICollection<TreeNode>, IEnumerable<TreeNode>, ICollection, IEnumerable
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004552 File Offset: 0x00002752
		// (set) Token: 0x060000BF RID: 191 RVA: 0x0000455A File Offset: 0x0000275A
		private List<TreeNode> Nodes { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004563 File Offset: 0x00002763
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x0000456B File Offset: 0x0000276B
		private Dictionary<ITreeItem, TreeCollection.DataItemInfo> ItemInfo { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004574 File Offset: 0x00002774
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x0000457C File Offset: 0x0000277C
		public ITreeItem RootItem
		{
			get
			{
				return this._RootItem;
			}
			set
			{
				this.Nodes.Clear();
				this.ItemInfo.Clear();
				this._RegisterHandler.Clear();
				this._RootItem = value;
				if (this._RootItem != null)
				{
					this.ItemInfo.Add(this._RootItem, new TreeCollection.DataItemInfo(null, new TreeNode(this._RootItem, -1)
					{
						IsExpanded = true
					})
					{
						IsRoot = true
					});
					this.SetupDataNode(this._RootItem, null);
					this.ExpandNode(this.ItemInfo[this._RootItem].Node);
				}
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004628 File Offset: 0x00002828
		public TreeCollection()
		{
			this.Nodes = new List<TreeNode>();
			this.ItemInfo = new Dictionary<ITreeItem, TreeCollection.DataItemInfo>();
			this._SelectedNodes = new List<TreeNode>();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000046C8 File Offset: 0x000028C8
		public TreeCollection(ITreeItem rootItem)
		{
			this.Nodes = new List<TreeNode>();
			this.ItemInfo = new Dictionary<ITreeItem, TreeCollection.DataItemInfo>();
			this._SelectedNodes = new List<TreeNode>();
			this.RootItem = rootItem;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000048D0 File Offset: 0x00002AD0
		IEnumerator<TreeNode> IEnumerable<TreeNode>.GetEnumerator()
		{
			foreach (TreeNode node in this.Nodes.Where((TreeNode o) => o.Depth > -1))
			{
				yield return node;
			}
			yield break;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004A50 File Offset: 0x00002C50
		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (TreeNode node in this.Nodes.Where((TreeNode o) => o.Depth > -1))
			{
				yield return node;
			}
			yield break;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004A6C File Offset: 0x00002C6C
		public int Count
		{
			get
			{
				if (this.Nodes != null)
				{
					return this.Nodes.Count;
				}
				return 0;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00004A83 File Offset: 0x00002C83
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004A86 File Offset: 0x00002C86
		public void Add(TreeNode node)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004A8D File Offset: 0x00002C8D
		public void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004A94 File Offset: 0x00002C94
		public bool Contains(TreeNode node)
		{
			return this.Nodes.Contains(node);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004AA4 File Offset: 0x00002CA4
		public void CopyTo(TreeNode[] array, int index)
		{
			foreach (TreeNode treeNode in this.Nodes)
			{
				array.SetValue(treeNode, index++);
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004B00 File Offset: 0x00002D00
		public void CopyTo(Array array, int index)
		{
			foreach (TreeNode treeNode in this.Nodes)
			{
				array.SetValue(treeNode, index++);
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004B5C File Offset: 0x00002D5C
		public bool Remove(TreeNode node)
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00004B63 File Offset: 0x00002D63
		public bool IsSynchronized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00004B66 File Offset: 0x00002D66
		public object SyncRoot
		{
			get
			{
				return this._SyncObject;
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000D2 RID: 210 RVA: 0x00004B70 File Offset: 0x00002D70
		// (remove) Token: 0x060000D3 RID: 211 RVA: 0x00004BA8 File Offset: 0x00002DA8
		public event NotifyCollectionChangedEventHandler CollectionChanged = delegate(object o, NotifyCollectionChangedEventArgs e)
		{
		};

		// Token: 0x060000D4 RID: 212 RVA: 0x00004BDD File Offset: 0x00002DDD
		private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			this.CollectionChanged(this, e);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004C08 File Offset: 0x00002E08
		private TreeNode SetupDataNode(ITreeItem dataItem, ITreeItem parentDataItem)
		{
			TreeCollection.DataItemInfo dataItemInfo = null;
			lock (this._SyncObject)
			{
				try
				{
					if (!this.ItemInfo.ContainsKey(dataItem))
					{
						this.ItemInfo.Add(dataItem, new TreeCollection.DataItemInfo(parentDataItem, new TreeNode(dataItem, -1)));
					}
				}
				catch
				{
					throw new Exception("Error accessing ItemInfo");
				}
				try
				{
					dataItemInfo = this.ItemInfo[dataItem];
				}
				catch
				{
					throw new Exception("Error retrieving dataInfo");
				}
				try
				{
					dataItemInfo.ParentDataItem = parentDataItem;
				}
				catch
				{
					throw new Exception("Error setting dataInfo.ParentDataItem");
				}
				TreeCollection.DataItemInfo dataItemInfo2 = null;
				int num = -1;
				try
				{
					dataItemInfo2 = ((parentDataItem != null && this.ItemInfo.ContainsKey(parentDataItem)) ? this.ItemInfo[parentDataItem] : null);
				}
				catch
				{
					throw new Exception("Error retreiving parentInfo");
				}
				try
				{
					num = ((dataItemInfo2 != null && dataItemInfo2.Node != null) ? (dataItemInfo2.Node.Depth + 1) : (-1));
				}
				catch
				{
					throw new Exception("Error retrieving depth");
				}
				try
				{
					dataItemInfo.Node.Depth = num;
				}
				catch
				{
					throw new Exception("Error setting depth");
				}
				try
				{
					if (dataItemInfo.Node == null || dataItemInfo.Node.IsExpanded)
					{
						foreach (ITreeItem treeItem in dataItem.GetChildren())
						{
							this.SetupDataNode(treeItem, dataItem);
						}
					}
				}
				catch
				{
					throw new Exception("Error setting up dataItem.GetChildren()");
				}
				try
				{
					if (dataItem.TreeChildren != null)
					{
						try
						{
							if (!this._RegisterHandler.ContainsKey(dataItem))
							{
								this._RegisterHandler.Add(dataItem, delegate(object o, NotifyCollectionChangedEventArgs e)
								{
									this.DataItemChildren_CollectionChanged(dataItem, e);
								});
							}
						}
						catch
						{
							throw new Exception("Error adding handler");
						}
						try
						{
							dataItem.TreeChildren.CollectionChanged -= this._RegisterHandler[dataItem];
						}
						catch
						{
							throw new Exception("Error unregistering handler");
						}
						try
						{
							dataItem.TreeChildren.CollectionChanged += this._RegisterHandler[dataItem];
						}
						catch
						{
							throw new Exception("Error registering handler");
						}
					}
				}
				catch
				{
					throw new Exception("Error working with datItem.TreeChildren");
				}
			}
			return dataItemInfo.Node;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004FBC File Offset: 0x000031BC
		private void DataItemChildren_CollectionChanged(ITreeItem sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			{
				IEnumerable<Tuple<int, ITreeItem>> enumerable = this.RemapAddedItemIndexes(sender, e.NewItems.Cast<ITreeItem>());
				using (IEnumerator<Tuple<int, ITreeItem>> enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Tuple<int, ITreeItem> tuple = enumerator.Current;
						this.AddItemNodes(sender, new ITreeItem[] { tuple.Item2 }, tuple.Item1);
					}
					return;
				}
				break;
			}
			case NotifyCollectionChangedAction.Remove:
				break;
			default:
				this.ResetItemNode(sender);
				return;
			}
			this.RemoveItemNodes(sender, e.OldItems.Cast<ITreeItem>(), true);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000506C File Offset: 0x0000326C
		private IEnumerable<Tuple<int, ITreeItem>> RemapAddedItemIndexes(ITreeItem parent, IEnumerable<ITreeItem> added)
		{
			List<Tuple<int, ITreeItem>> list = new List<Tuple<int, ITreeItem>>();
			if (parent == null)
			{
				return list;
			}
			List<ITreeItem> children = parent.GetChildren();
			if (children == null || !children.Any<ITreeItem>())
			{
				return list;
			}
			for (int i = 0; i < children.Count; i++)
			{
				if (added.Contains(children[i]))
				{
					list.Add(new Tuple<int, ITreeItem>(i, children[i]));
				}
			}
			return list.OrderBy((Tuple<int, ITreeItem> o) => o.Item1);
		}

		// Token: 0x1700002E RID: 46
		public TreeNode this[int index]
		{
			get
			{
				return this.Nodes[index];
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000050FC File Offset: 0x000032FC
		public TreeNode GetParentNode(TreeNode node)
		{
			if (node == null || node.DataItem == null || !this.ItemInfo.ContainsKey(node.DataItem))
			{
				return null;
			}
			ITreeItem parentDataItem = this.ItemInfo[node.DataItem].ParentDataItem;
			if (parentDataItem == null || !this.ItemInfo.ContainsKey(parentDataItem))
			{
				return null;
			}
			return this.ItemInfo[parentDataItem].Node;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005164 File Offset: 0x00003364
		public int IndexOf(TreeNode node)
		{
			return this.Nodes.IndexOf(node);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005174 File Offset: 0x00003374
		private bool NodeIsVisible(TreeNode node)
		{
			ITreeItem dataItem = node.DataItem;
			if (!this.ItemInfo.ContainsKey(dataItem))
			{
				return false;
			}
			TreeCollection.DataItemInfo dataItemInfo = this.ItemInfo[dataItem];
			if (dataItemInfo.IsRoot)
			{
				return true;
			}
			if (dataItemInfo.ParentDataItem == null || !this.ItemInfo.ContainsKey(dataItemInfo.ParentDataItem))
			{
				return false;
			}
			TreeNode node2 = this.ItemInfo[dataItemInfo.ParentDataItem].Node;
			return node2.IsExpanded && this.NodeIsVisible(node2);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000051F4 File Offset: 0x000033F4
		private bool DataItemChildrenAreVisible(ITreeItem dataItem)
		{
			if (!this.ItemInfo.ContainsKey(dataItem))
			{
				return false;
			}
			TreeNode node = this.ItemInfo[dataItem].Node;
			return node.IsExpanded && this.NodeIsVisible(node);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005234 File Offset: 0x00003434
		private void ResetItemNode(ITreeItem dataItem)
		{
			this.RemoveItemNodes(dataItem, (IEnumerable<ITreeItem>)dataItem.TreeChildren, false);
			if (!this.ItemInfo.ContainsKey(dataItem))
			{
				return;
			}
			this.SetupDataNode(dataItem, this.ItemInfo[dataItem].ParentDataItem);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000052A0 File Offset: 0x000034A0
		private TreeCollection.ManipulationRange GetAffectedRemoveRange(ITreeItem dataItem, IEnumerable<ITreeItem> removeItems)
		{
			if (!this.ItemInfo.ContainsKey(dataItem) || removeItems == null || !removeItems.Any<ITreeItem>())
			{
				return null;
			}
			IEnumerable<int> enumerable = from o in removeItems
				where this.ItemInfo.ContainsKey(o)
				select this.Nodes.IndexOf(this.ItemInfo[o].Node);
			if (!enumerable.Any<int>())
			{
				return null;
			}
			int num = enumerable.Min();
			int num2 = enumerable.Max();
			if (num == -1)
			{
				return null;
			}
			TreeNode treeNode = this.Nodes[num2];
			while (num2 < this.Nodes.Count && this.Nodes[num2].Depth > treeNode.Depth)
			{
				num2++;
			}
			if (num2 == this.Nodes.Count)
			{
				num2--;
			}
			return new TreeCollection.ManipulationRange
			{
				StartIndex = new int?(num),
				EndIndex = new int?(num2),
				Count = new int?(num2 - num + 1)
			};
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000538C File Offset: 0x0000358C
		private int IndexOfNextSiblingNode(ITreeItem dataItem)
		{
			if (dataItem == this.RootItem)
			{
				return this.Nodes.Count;
			}
			ITreeItem parentDataItem = this.ItemInfo[dataItem].ParentDataItem;
			List<ITreeItem> children = parentDataItem.GetChildren();
			int num = children.IndexOf(dataItem);
			if (num == children.Count - 1)
			{
				return this.IndexOfNextSiblingNode(parentDataItem);
			}
			ITreeItem treeItem = children[num + 1];
			return this.Nodes.IndexOf(this.ItemInfo[treeItem].Node);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005408 File Offset: 0x00003608
		private TreeCollection.ManipulationRange GetAffectedAddRange(ITreeItem dataItem, int collIndex, int collCount)
		{
			if (!this.DataItemChildrenAreVisible(dataItem) || !this.ItemInfo.ContainsKey(dataItem))
			{
				return null;
			}
			List<ITreeItem> children = dataItem.GetChildren();
			if (collIndex + collCount == children.Count)
			{
				return new TreeCollection.ManipulationRange
				{
					StartIndex = new int?(this.IndexOfNextSiblingNode(dataItem)),
					Count = new int?(collCount)
				};
			}
			TreeCollection.DataItemInfo dataItemInfo = this.ItemInfo[children[collIndex + 1]];
			if (dataItemInfo == null || dataItemInfo.Node == null)
			{
				return new TreeCollection.ManipulationRange
				{
					StartIndex = new int?(this.Nodes.Count),
					Count = new int?(collCount)
				};
			}
			int num = this.Nodes.IndexOf(dataItemInfo.Node);
			return new TreeCollection.ManipulationRange
			{
				StartIndex = new int?((num >= 0) ? num : this.Nodes.Count),
				Count = new int?(collCount)
			};
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005518 File Offset: 0x00003718
		private void AddItemNodes(ITreeItem dataItem, IEnumerable<ITreeItem> addItems, int startIndex)
		{
			lock (this._SyncObject)
			{
				TreeCollection.ManipulationRange affectedAddRange = this.GetAffectedAddRange(dataItem, startIndex, addItems.Count<ITreeItem>());
				if (affectedAddRange != null)
				{
					List<TreeNode> list = addItems.Select((ITreeItem o) => this.SetupDataNode(o, dataItem)).ToList<TreeNode>();
					this.Nodes.InsertRange(affectedAddRange.StartIndex.Value, list);
					this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list, affectedAddRange.StartIndex.Value));
					foreach (ITreeItem treeItem in addItems)
					{
						if (treeItem.HasTreeChildren && this.ItemInfo.ContainsKey(treeItem) && this.DataItemChildrenAreVisible(treeItem))
						{
							this.AddItemNodes(treeItem, treeItem.GetChildren(), 0);
						}
					}
				}
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005648 File Offset: 0x00003848
		private void RemoveItemNodes(ITreeItem dataItem, IEnumerable<ITreeItem> removeItems, bool deleteItem = false)
		{
			lock (this._SyncObject)
			{
				foreach (ITreeItem treeItem in removeItems)
				{
					if (treeItem.HasTreeChildren && this.ItemInfo.ContainsKey(treeItem) && this.DataItemChildrenAreVisible(treeItem))
					{
						this.RemoveItemNodes(treeItem, treeItem.GetChildren(), deleteItem);
					}
				}
				TreeCollection.ManipulationRange affectedRemoveRange = this.GetAffectedRemoveRange(dataItem, removeItems);
				if (affectedRemoveRange != null)
				{
					List<TreeNode> range = this.Nodes.GetRange(affectedRemoveRange.StartIndex.Value, affectedRemoveRange.Count.Value);
					this.Nodes.RemoveRange(affectedRemoveRange.StartIndex.Value, affectedRemoveRange.Count.Value);
					this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, range, affectedRemoveRange.StartIndex.Value));
				}
				if (deleteItem)
				{
					foreach (ITreeItem treeItem2 in removeItems)
					{
						if (this.ItemInfo.ContainsKey(treeItem2))
						{
							this.ItemInfo.Remove(treeItem2);
						}
						this._RegisterHandler.Remove(treeItem2);
					}
				}
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000057E8 File Offset: 0x000039E8
		private void Ensure(ITreeItem dataItem)
		{
			lock (this._SyncObject)
			{
				if (!this.ItemInfo.ContainsKey(dataItem))
				{
					this.ItemInfo.Add(dataItem, new TreeCollection.DataItemInfo(null, new TreeNode(dataItem, -1)));
				}
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000584C File Offset: 0x00003A4C
		public void ExpandNode(TreeNode node)
		{
			int num = this.Nodes.IndexOf(node);
			if (node.DataItem.HasTreeChildren && (num == this.Nodes.Count - 1 || this.Nodes[num + 1].Depth <= node.Depth))
			{
				this.AddItemNodes(node.DataItem, node.DataItem.GetChildren(), 0);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000058C4 File Offset: 0x00003AC4
		public void CollapseNode(TreeNode node)
		{
			int num = this.Nodes.IndexOf(node);
			if (node.DataItem.HasTreeChildren && num != this.Nodes.Count - 1 && this.Nodes[num + 1].Depth > node.Depth)
			{
				foreach (ITreeItem treeItem in from o in node.DataItem.GetChildren()
					where this.ItemInfo.ContainsKey(o)
					select o)
				{
					TreeNode node2 = this.ItemInfo[treeItem].Node;
					if (node2 != null && node2.IsExpanded)
					{
						this.CollapseNode(node2);
					}
				}
				this.RemoveItemNodes(node.DataItem, node.DataItem.GetChildren(), false);
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000E6 RID: 230 RVA: 0x000059B8 File Offset: 0x00003BB8
		// (remove) Token: 0x060000E7 RID: 231 RVA: 0x000059F0 File Offset: 0x00003BF0
		public event TreeCollection.TreeSelectionChangedEventHandler TreeSelectionChanged = delegate(object o, TreeCollection.TreeSelectionChangedEventArgs e)
		{
		};

		// Token: 0x060000E8 RID: 232 RVA: 0x00005A25 File Offset: 0x00003C25
		private void OnTreeSelectionChanged(IEnumerable<TreeNode> newItems, IEnumerable<TreeNode> removedItems)
		{
			this.TreeSelectionChanged(this, new TreeCollection.TreeSelectionChangedEventArgs(this.SelectedNodes, newItems, removedItems));
			base.RaisePropertyChanged("SelectedItems");
			base.RaisePropertyChanged("SelectedNodes");
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005A56 File Offset: 0x00003C56
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00005A88 File Offset: 0x00003C88
		public IEnumerable<TreeNode> SelectedNodes
		{
			get
			{
				return this._SelectedNodes;
			}
			set
			{
				List<TreeNode> list = new List<TreeNode>();
				List<TreeNode> list2 = new List<TreeNode>();
				List<TreeNode> currentNodes = this.SelectedNodes.ToList<TreeNode>();
				if (value == null)
				{
					list2 = currentNodes;
				}
				else
				{
					list = value.Where((TreeNode o) => !currentNodes.Contains(o)).ToList<TreeNode>();
					list2 = currentNodes.Where((TreeNode o) => !value.Contains(o)).ToList<TreeNode>();
				}
				this._SelectedNodes.Clear();
				if (value != null)
				{
					this._SelectedNodes.AddRange(value);
				}
				this.OnTreeSelectionChanged(list, list2);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00005B57 File Offset: 0x00003D57
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00005B84 File Offset: 0x00003D84
		public IEnumerable<ITreeItem> SelectedItems
		{
			get
			{
				return this.SelectedNodes.Select((TreeNode o) => o.DataItem);
			}
			set
			{
				List<TreeNode> list = new List<TreeNode>();
				if (value != null)
				{
					foreach (ITreeItem treeItem in value)
					{
						this.Ensure(treeItem);
						if (this.ItemInfo.ContainsKey(treeItem))
						{
							list.Add(this.ItemInfo[treeItem].Node);
						}
					}
				}
				this.SelectedNodes = list;
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005C04 File Offset: 0x00003E04
		public void ClearSelection()
		{
			this.SelectedNodes = new List<TreeNode>();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005C14 File Offset: 0x00003E14
		public void UnselectNodes(IEnumerable<TreeNode> nodes)
		{
			if (nodes != null)
			{
				foreach (TreeNode treeNode in nodes.ToList<TreeNode>())
				{
					this.UnselectNode(treeNode);
				}
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005C6C File Offset: 0x00003E6C
		public void UnselectNode(TreeNode node)
		{
			if (this._SelectedNodes.Contains(node))
			{
				this._SelectedNodes.Remove(node);
				this.OnTreeSelectionChanged(null, new List<TreeNode> { node });
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005CAC File Offset: 0x00003EAC
		public void SelectNodes(IEnumerable<TreeNode> nodes)
		{
			if (nodes != null)
			{
				foreach (TreeNode treeNode in nodes.ToList<TreeNode>())
				{
					this.SelectNode(treeNode);
				}
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005D04 File Offset: 0x00003F04
		public void SelectNode(TreeNode node)
		{
			if (!this._SelectedNodes.Contains(node))
			{
				this._SelectedNodes.Add(node);
				this.OnTreeSelectionChanged(new List<TreeNode> { node }, null);
			}
		}

		// Token: 0x04000045 RID: 69
		private ITreeItem _RootItem;

		// Token: 0x04000046 RID: 70
		private object _SyncObject = new object();

		// Token: 0x04000048 RID: 72
		private Dictionary<ITreeItem, NotifyCollectionChangedEventHandler> _RegisterHandler = new Dictionary<ITreeItem, NotifyCollectionChangedEventHandler>();

		// Token: 0x04000049 RID: 73
		private static List<TreeNode> EmptyTreeNode = new List<TreeNode>();

		// Token: 0x0400004B RID: 75
		private List<TreeNode> _SelectedNodes = new List<TreeNode>();

		// Token: 0x0200001A RID: 26
		private class DataItemInfo
		{
			// Token: 0x17000031 RID: 49
			// (get) Token: 0x060000FE RID: 254 RVA: 0x00005D4C File Offset: 0x00003F4C
			// (set) Token: 0x060000FF RID: 255 RVA: 0x00005D54 File Offset: 0x00003F54
			public ITreeItem ParentDataItem { get; set; }

			// Token: 0x17000032 RID: 50
			// (get) Token: 0x06000100 RID: 256 RVA: 0x00005D5D File Offset: 0x00003F5D
			// (set) Token: 0x06000101 RID: 257 RVA: 0x00005D65 File Offset: 0x00003F65
			public TreeNode Node { get; set; }

			// Token: 0x17000033 RID: 51
			// (get) Token: 0x06000102 RID: 258 RVA: 0x00005D6E File Offset: 0x00003F6E
			// (set) Token: 0x06000103 RID: 259 RVA: 0x00005D76 File Offset: 0x00003F76
			public bool IsRoot { get; set; }

			// Token: 0x06000104 RID: 260 RVA: 0x00005D7F File Offset: 0x00003F7F
			public DataItemInfo(ITreeItem parentDataItem, TreeNode node)
			{
				this.Node = node;
				this.ParentDataItem = parentDataItem;
			}
		}

		// Token: 0x0200001B RID: 27
		private class ManipulationRange
		{
			// Token: 0x17000034 RID: 52
			// (get) Token: 0x06000105 RID: 261 RVA: 0x00005D95 File Offset: 0x00003F95
			// (set) Token: 0x06000106 RID: 262 RVA: 0x00005D9D File Offset: 0x00003F9D
			public int? StartIndex { get; set; }

			// Token: 0x17000035 RID: 53
			// (get) Token: 0x06000107 RID: 263 RVA: 0x00005DA6 File Offset: 0x00003FA6
			// (set) Token: 0x06000108 RID: 264 RVA: 0x00005DAE File Offset: 0x00003FAE
			public int? EndIndex { get; set; }

			// Token: 0x17000036 RID: 54
			// (get) Token: 0x06000109 RID: 265 RVA: 0x00005DB7 File Offset: 0x00003FB7
			// (set) Token: 0x0600010A RID: 266 RVA: 0x00005DBF File Offset: 0x00003FBF
			public int? Count { get; set; }
		}

		// Token: 0x0200001C RID: 28
		public class TreeSelectionChangedEventArgs : EventArgs
		{
			// Token: 0x0600010C RID: 268 RVA: 0x00005DD0 File Offset: 0x00003FD0
			public TreeSelectionChangedEventArgs(IEnumerable<TreeNode> selectedNodes, IEnumerable<TreeNode> addedNodes, IEnumerable<TreeNode> removedNodes)
			{
				this.SelectedNodes = selectedNodes ?? TreeCollection.EmptyTreeNode;
				this.AddedNodes = addedNodes ?? TreeCollection.EmptyTreeNode;
				this.RemovedNodes = removedNodes ?? TreeCollection.EmptyTreeNode;
			}

			// Token: 0x17000037 RID: 55
			// (get) Token: 0x0600010D RID: 269 RVA: 0x00005E08 File Offset: 0x00004008
			// (set) Token: 0x0600010E RID: 270 RVA: 0x00005E10 File Offset: 0x00004010
			public IEnumerable<TreeNode> SelectedNodes { get; set; }

			// Token: 0x17000038 RID: 56
			// (get) Token: 0x0600010F RID: 271 RVA: 0x00005E19 File Offset: 0x00004019
			// (set) Token: 0x06000110 RID: 272 RVA: 0x00005E21 File Offset: 0x00004021
			public IEnumerable<TreeNode> AddedNodes { get; set; }

			// Token: 0x17000039 RID: 57
			// (get) Token: 0x06000111 RID: 273 RVA: 0x00005E2A File Offset: 0x0000402A
			// (set) Token: 0x06000112 RID: 274 RVA: 0x00005E32 File Offset: 0x00004032
			public IEnumerable<TreeNode> RemovedNodes { get; set; }

			// Token: 0x1700003A RID: 58
			// (get) Token: 0x06000113 RID: 275 RVA: 0x00005E43 File Offset: 0x00004043
			public IEnumerable<ITreeItem> SelectedItems
			{
				get
				{
					if (this.SelectedNodes == null)
					{
						return null;
					}
					return this.SelectedNodes.Select((TreeNode o) => o.DataItem);
				}
			}

			// Token: 0x1700003B RID: 59
			// (get) Token: 0x06000114 RID: 276 RVA: 0x00005E7F File Offset: 0x0000407F
			public IEnumerable<ITreeItem> AddedItems
			{
				get
				{
					if (this.AddedNodes == null)
					{
						return null;
					}
					return this.AddedNodes.Select((TreeNode o) => o.DataItem);
				}
			}

			// Token: 0x1700003C RID: 60
			// (get) Token: 0x06000115 RID: 277 RVA: 0x00005EBB File Offset: 0x000040BB
			public IEnumerable<ITreeItem> RemovedItems
			{
				get
				{
					if (this.RemovedNodes == null)
					{
						return null;
					}
					return this.RemovedNodes.Select((TreeNode o) => o.DataItem);
				}
			}
		}

		// Token: 0x0200001D RID: 29
		// (Invoke) Token: 0x0600011A RID: 282
		public delegate void TreeSelectionChangedEventHandler(object sender, TreeCollection.TreeSelectionChangedEventArgs e);
	}
}
