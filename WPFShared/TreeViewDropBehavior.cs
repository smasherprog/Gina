using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace WPFShared
{
    // Token: 0x02000005 RID: 5
    public class TreeViewDropBehavior : Behavior<TreeView>
    {
        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000012 RID: 18 RVA: 0x00002406 File Offset: 0x00000606
        // (set) Token: 0x06000013 RID: 19 RVA: 0x00002418 File Offset: 0x00000618
        public GenericCommand IsDropAllowedCommand
        {
            get => (GenericCommand)base.GetValue(TreeViewDropBehavior.IsDropAllowedCommandProperty); set => base.SetValue(TreeViewDropBehavior.IsDropAllowedCommandProperty, value);
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000014 RID: 20 RVA: 0x00002426 File Offset: 0x00000626
        // (set) Token: 0x06000015 RID: 21 RVA: 0x00002438 File Offset: 0x00000638
        public GenericCommand DropCommand
        {
            get => (GenericCommand)base.GetValue(TreeViewDropBehavior.DropCommandProperty); set => base.SetValue(TreeViewDropBehavior.DropCommandProperty, value);
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002448 File Offset: 0x00000648
        protected override void OnAttached()
        {
            base.OnAttached();
            base.AssociatedObject.AllowDrop = true;
            base.AssociatedObject.DragOver += AssociatedObject_DragOver;
            base.AssociatedObject.Drop += AssociatedObject_Drop;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00002498 File Offset: 0x00000698
        private void SetArgs(DragEventArgs e)
        {
            _Args.SourceTreeViewItem = ((DataObject)e.Data).GetData("DragObject") as TreeViewItem;
            _Args.SourceObject = _Args.SourceTreeViewItem?.DataContext;
            _ = e.GetPosition(base.AssociatedObject);
            var dependencyObject = base.AssociatedObject.InputHitTest(e.GetPosition(base.AssociatedObject)) as DependencyObject;
            _Args.DestinationTreeViewItem = dependencyObject?.VisualUpwardSearch<TreeViewItem>();
            _Args.DestinationObject = _Args.DestinationTreeViewItem?.DataContext;
            _Args.Effects = e.Effects;
        }

        // Token: 0x06000018 RID: 24 RVA: 0x00002574 File Offset: 0x00000774
        private void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            SetArgs(e);
            if (_Args.SourceTreeViewItem == _Args.DestinationTreeViewItem)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
            if (IsDropAllowedCommand != null)
            {
                IsDropAllowedCommand.Execute(_Args);
                e.Effects = _Args.Effects;
                e.Handled = true;
                return;
            }
            e.Effects = DragDropEffects.All;
            e.Handled = true;
        }

        // Token: 0x06000019 RID: 25 RVA: 0x000025F8 File Offset: 0x000007F8
        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            SetArgs(e);
            DropCommand?.Execute(_Args);
        }

        // Token: 0x04000007 RID: 7
        public static readonly DependencyProperty IsDropAllowedCommandProperty = DependencyProperty.Register("IsDropAllowedCommand", typeof(GenericCommand), typeof(TreeViewDropBehavior), null);

        // Token: 0x04000008 RID: 8
        public static readonly DependencyProperty DropCommandProperty = DependencyProperty.Register("DropCommand", typeof(GenericCommand), typeof(TreeViewDropBehavior), null);

        // Token: 0x04000009 RID: 9
        private readonly TreeViewDropBehavior.CanDropEventArgs _Args = new TreeViewDropBehavior.CanDropEventArgs();

        // Token: 0x02000006 RID: 6
        public class CanDropEventArgs : EventArgs
        {
            // Token: 0x17000005 RID: 5
            // (get) Token: 0x0600001C RID: 28 RVA: 0x00002688 File Offset: 0x00000888
            // (set) Token: 0x0600001D RID: 29 RVA: 0x00002690 File Offset: 0x00000890
            public TreeViewItem SourceTreeViewItem { get; set; }

            // Token: 0x17000006 RID: 6
            // (get) Token: 0x0600001E RID: 30 RVA: 0x00002699 File Offset: 0x00000899
            // (set) Token: 0x0600001F RID: 31 RVA: 0x000026A1 File Offset: 0x000008A1
            public TreeViewItem DestinationTreeViewItem { get; set; }

            // Token: 0x17000007 RID: 7
            // (get) Token: 0x06000020 RID: 32 RVA: 0x000026AA File Offset: 0x000008AA
            // (set) Token: 0x06000021 RID: 33 RVA: 0x000026B2 File Offset: 0x000008B2
            public object SourceObject { get; set; }

            // Token: 0x17000008 RID: 8
            // (get) Token: 0x06000022 RID: 34 RVA: 0x000026BB File Offset: 0x000008BB
            // (set) Token: 0x06000023 RID: 35 RVA: 0x000026C3 File Offset: 0x000008C3
            public object DestinationObject { get; set; }

            // Token: 0x17000009 RID: 9
            // (get) Token: 0x06000024 RID: 36 RVA: 0x000026CC File Offset: 0x000008CC
            // (set) Token: 0x06000025 RID: 37 RVA: 0x000026D4 File Offset: 0x000008D4
            public DragDropEffects Effects { get; set; }
        }
    }
}
