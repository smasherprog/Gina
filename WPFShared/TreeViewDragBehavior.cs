using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace WPFShared
{
    // Token: 0x02000050 RID: 80
    public class TreeViewDragBehavior : Behavior<TreeView>
    {
        // Token: 0x0600021F RID: 543 RVA: 0x000092E0 File Offset: 0x000074E0
        protected override void OnAttached()
        {
            base.OnAttached();
            base.AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
            base.AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
            base.AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
        }

        // Token: 0x06000220 RID: 544 RVA: 0x00009338 File Offset: 0x00007538
        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!_IsDragging && _StartingPoint != null)
            {
                _StartingPoint = null;
            }
        }

        // Token: 0x06000221 RID: 545 RVA: 0x0000935C File Offset: 0x0000755C
        private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_StartingPoint == null)
            {
                return;
            }
            if (e.LeftButton == MouseButtonState.Pressed && !_IsDragging && _DragObject != null)
            {
                var treeView = (TreeView)sender;
                var position = e.GetPosition(treeView);
                if (Math.Pow(position.X - _StartingPoint.Value.X, 2.0) + Math.Pow(position.Y - _StartingPoint.Value.Y, 2.0) > 64.0)
                {
                    var dataObject = new DataObject(_DragObject);
                    dataObject.SetData("DragObject", _DragObject);
                    _ = DragDrop.DoDragDrop(treeView, dataObject, DragDropEffects.Move);
                    _IsDragging = true;
                }
            }
        }

        // Token: 0x06000222 RID: 546 RVA: 0x00009434 File Offset: 0x00007634
        private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeView = (TreeView)sender;
            var treeViewItem = ((DependencyObject)e.OriginalSource).VisualUpwardSearch<TreeViewItem>();
            _DragObject = treeViewItem;
            if (_DragObject != null)
            {
                _StartingPoint = new Point?(e.GetPosition(treeView));
                _IsDragging = false;
            }
        }

        // Token: 0x040000B0 RID: 176
        private Point? _StartingPoint;

        // Token: 0x040000B1 RID: 177
        private bool _IsDragging;

        // Token: 0x040000B2 RID: 178
        private object _DragObject;
    }
}
