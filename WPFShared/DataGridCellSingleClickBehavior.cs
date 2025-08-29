using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace WPFShared
{
    // Token: 0x02000003 RID: 3
    public class DataGridCellSingleClickBehavior : Behavior<DataGridCell>
    {
        // Token: 0x06000004 RID: 4 RVA: 0x000020FA File Offset: 0x000002FA
        protected override void OnAttached()
        {
            base.OnAttached();
            base.AssociatedObject.PreviewMouseLeftButtonDown += delegate (object o, MouseButtonEventArgs e)
            {
                if (o is DataGridCell dataGridCell && !dataGridCell.IsReadOnly && !dataGridCell.IsEditing)
                {
                    if (!dataGridCell.IsFocused)
                    {
                        _ = dataGridCell.Focus();
                    }
                    var dataGrid = dataGridCell.VisualUpwardSearch<DataGrid>();
                    if (dataGrid != null)
                    {
                        if (dataGrid.SelectionUnit != DataGridSelectionUnit.FullRow)
                        {
                            if (!dataGridCell.IsSelected)
                            {
                                dataGridCell.IsSelected = true;
                                return;
                            }
                        }
                        else
                        {
                            var dataGridRow = dataGridCell.VisualUpwardSearch<DataGridRow>();
                            if (dataGridRow != null && !dataGridRow.IsSelected)
                            {
                                dataGridRow.IsSelected = true;
                            }
                        }
                    }
                }
            };
        }
    }
}
