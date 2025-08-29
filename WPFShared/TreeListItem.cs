using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace WPFShared
{
    // Token: 0x02000023 RID: 35
    public class TreeListItem : ContentControl
    {
        // Token: 0x1700004B RID: 75
        // (get) Token: 0x06000164 RID: 356 RVA: 0x000070C4 File Offset: 0x000052C4
        // (set) Token: 0x06000165 RID: 357 RVA: 0x000070D6 File Offset: 0x000052D6
        public TreeList Tree
        {
            get => (TreeList)base.GetValue(TreeListItem.TreeProperty); set => base.SetValue(TreeListItem.TreeProperty, value);
        }

        // Token: 0x1700004C RID: 76
        // (get) Token: 0x06000166 RID: 358 RVA: 0x000070E4 File Offset: 0x000052E4
        // (set) Token: 0x06000167 RID: 359 RVA: 0x000070F6 File Offset: 0x000052F6
        public TreeNode Node
        {
            get => (TreeNode)base.GetValue(TreeListItem.NodeProperty); set => base.SetValue(TreeListItem.NodeProperty, value);
        }

        // Token: 0x1700004D RID: 77
        // (get) Token: 0x06000168 RID: 360 RVA: 0x00007104 File Offset: 0x00005304
        public Thickness Indention
        {
            get
            {
                if (Tree == null || Node == null)
                {
                    return new Thickness(0.0);
                }
                var imagePadding = Tree.ImagePadding;
                return new Thickness((Tree.Indention * Node.Depth) + imagePadding.Left, imagePadding.Top, imagePadding.Right, imagePadding.Bottom);
            }
        }

        // Token: 0x1700004E RID: 78
        // (get) Token: 0x06000169 RID: 361 RVA: 0x00007176 File Offset: 0x00005376
        private Grid ExpanderButton
        {
            get
            {
                if (base.Template == null)
                {
                    return null;
                }
                if (_ExpanderButton == null)
                {
                    _ExpanderButton = base.Template.FindName("ExpanderButton", this) as Grid;
                }
                return _ExpanderButton;
            }
        }

        // Token: 0x1700004F RID: 79
        // (get) Token: 0x0600016A RID: 362 RVA: 0x000071AC File Offset: 0x000053AC
        private Path DrillPath
        {
            get
            {
                if (ExpanderButton == null)
                {
                    return null;
                }
                if (_DrillPath == null)
                {
                    _DrillPath = base.Template.FindName("DrillPath", this) as Path;
                }
                return _DrillPath;
            }
        }

        // Token: 0x0600016B RID: 363 RVA: 0x000071E4 File Offset: 0x000053E4
        private void SetDrillPath()
        {
            if (Node == null || DrillPath == null)
            {
                return;
            }
            DrillPath.Style = (Style)base.Template.Resources[Node.IsExpanded ? "ExpandedNodePath" : "CollapsedNodePath"];
        }

        // Token: 0x1400000A RID: 10
        // (add) Token: 0x0600016C RID: 364 RVA: 0x0000723B File Offset: 0x0000543B
        // (remove) Token: 0x0600016D RID: 365 RVA: 0x00007249 File Offset: 0x00005449
        public event RoutedEventHandler Collapsed
        {
            add => base.AddHandler(TreeListItem.CollapsedEvent, value); remove => base.RemoveHandler(TreeListItem.CollapsedEvent, value);
        }

        // Token: 0x1400000B RID: 11
        // (add) Token: 0x0600016E RID: 366 RVA: 0x00007257 File Offset: 0x00005457
        // (remove) Token: 0x0600016F RID: 367 RVA: 0x00007265 File Offset: 0x00005465
        public event RoutedEventHandler Expanded
        {
            add => base.AddHandler(TreeListItem.ExpandedEvent, value); remove => base.RemoveHandler(TreeListItem.ExpandedEvent, value);
        }

        // Token: 0x06000170 RID: 368 RVA: 0x00007274 File Offset: 0x00005474
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Node != null)
            {
                var key = e.Key;
                switch (key)
                {
                    case Key.Left:
                        goto IL_0071;
                    case Key.Up:
                        _ = Tree.FocusPreviousNode(this);
                        e.Handled = true;
                        goto IL_00BD;
                    case Key.Right:
                        break;
                    case Key.Down:
                        _ = Tree.FocusNextNode(this);
                        e.Handled = true;
                        goto IL_00BD;
                    default:
                        switch (key)
                        {
                            case Key.Add:
                                break;
                            case Key.Separator:
                                goto IL_00BD;
                            case Key.Subtract:
                                goto IL_0071;
                            default:
                                goto IL_00BD;
                        }
                        break;
                }
                if (!Node.IsExpanded && Node.IsExpandable)
                {
                    Node.IsExpanded = true;
                }
                e.Handled = true;
                goto IL_00BD;
            IL_0071:
                if (Node.IsExpanded)
                {
                    Node.IsExpanded = false;
                }
                e.Handled = true;
            }
        IL_00BD:
            if (!e.Handled)
            {
                base.OnKeyDown(e);
            }
        }

        // Token: 0x06000171 RID: 369 RVA: 0x00007398 File Offset: 0x00005598
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == TreeListItem.NodeProperty)
            {
                TreeListItem.ExpandedDescriptor.AddValueChanged(Node, delegate (object o, EventArgs ie)
                {
                    if (!(o is TreeNode treeNode))
                    {
                        return;
                    }
                    SetDrillPath();
                    if (treeNode.IsExpanded)
                    {
                        base.RaiseEvent(new RoutedEventArgs(TreeListItem.ExpandedEvent));
                        return;
                    }
                    base.RaiseEvent(new RoutedEventArgs(TreeListItem.CollapsedEvent));
                });
                if (Node != null)
                {
                    SetDrillPath();
                }
            }
        }

        // Token: 0x06000172 RID: 370 RVA: 0x00007448 File Offset: 0x00005648
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var expanderButton = ExpanderButton;
            if (expanderButton != null)
            {
                SetDrillPath();
                expanderButton.PreviewMouseDown += delegate (object o, MouseButtonEventArgs e)
                {
                    if (Node != null)
                    {
                        Node.IsExpanded = !Node.IsExpanded;
                        SetDrillPath();
                        e.Handled = true;
                    }
                };
            }
            base.PreviewMouseDown += delegate (object o, MouseButtonEventArgs e)
            {
                var contentPresenter = base.Template.FindName("cp", this) as ContentPresenter;
                _ = contentPresenter.Focus();
            };
        }

        // Token: 0x06000176 RID: 374 RVA: 0x00007498 File Offset: 0x00005698
        // Note: this type is marked as 'beforefieldinit'.
        static TreeListItem()
        {
            TreeListItem.CollapsedEvent = EventManager.RegisterRoutedEvent("Collapsed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeListItem));
            TreeListItem.ExpandedEvent = EventManager.RegisterRoutedEvent("Expanded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeListItem));
        }

        public static readonly RoutedEvent CollapsedEvent;
        public static readonly RoutedEvent ExpandedEvent;
        // Token: 0x0400007C RID: 124 

        // Token: 0x0400007C RID: 124
        private static readonly DependencyPropertyDescriptor ExpandedDescriptor = DependencyPropertyDescriptor.FromProperty(TreeNode.IsExpandedProperty, typeof(TreeNode));

        // Token: 0x0400007D RID: 125
        public static readonly DependencyProperty TreeProperty = DependencyProperty.Register("Tree", typeof(TreeList), typeof(TreeListItem), null);

        // Token: 0x0400007E RID: 126
        public static readonly DependencyProperty NodeProperty = DependencyProperty.Register("Node", typeof(TreeNode), typeof(TreeListItem), null);

        // Token: 0x0400007F RID: 127
        private Grid _ExpanderButton;

        // Token: 0x04000080 RID: 128
        private Path _DrillPath;
    }
}
