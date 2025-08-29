using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace WPFShared
{
    // Token: 0x02000004 RID: 4
    public class ListViewSizeColumnsBehavior : Behavior<ListView>
    {
        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000007 RID: 7 RVA: 0x00002132 File Offset: 0x00000332
        // (set) Token: 0x06000008 RID: 8 RVA: 0x00002144 File Offset: 0x00000344
        public string SizeList
        {
            get => (string)base.GetValue(ListViewSizeColumnsBehavior.SizeListProperty); set => base.SetValue(ListViewSizeColumnsBehavior.SizeListProperty, value);
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000009 RID: 9 RVA: 0x00002152 File Offset: 0x00000352
        private GridView MyGridView
        {
            get
            {
                if (_GridView == null)
                {
                    _GridView = base.AssociatedObject.View as GridView;
                }
                return _GridView;
            }
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002189 File Offset: 0x00000389
        protected override void OnAttached()
        {
            base.OnAttached();
            base.AssociatedObject.SizeChanged += delegate (object o, SizeChangedEventArgs e)
            {
                ResizeColumns();
            };
            base.AssociatedObject.IsVisibleChanged += delegate (object o, DependencyPropertyChangedEventArgs e)
            {
                _SizesSet = false;
            };
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00002234 File Offset: 0x00000434
        private void ResizeColumns()
        {
            if (MyGridView == null || _SizesSet)
            {
                return;
            }
            var i = 0;
            var list = (from o in SizeList.Split(new char[] { '|' })
                        select new Tuple<int, double>(i++, double.Parse(o, CultureInfo.InvariantCulture))).ToList<Tuple<int, double>>();
            var num = base.AssociatedObject.ActualWidth;
            var num2 = 0.0;
            foreach (var tuple in list.Where((Tuple<int, double> o) => o.Item2 > 1.0))
            {
                MyGridView.Columns[tuple.Item1].Width = tuple.Item2;
                num -= tuple.Item2;
                num2 += tuple.Item2;
            }
            foreach (var tuple2 in list.Where((Tuple<int, double> o) => o.Item2 > 0.0 && o.Item2 <= 1.0))
            {
                var num3 = Math.Min(num * tuple2.Item2, base.AssociatedObject.ActualWidth - num2);
                MyGridView.Columns[tuple2.Item1].Width = num3;
                num2 += num3;
            }
            _SizesSet = true;
        }

        // Token: 0x04000002 RID: 2
        public static DependencyProperty SizeListProperty = DependencyProperty.Register("SizeList", typeof(string), typeof(ListViewSizeColumnsBehavior), null);

        // Token: 0x04000003 RID: 3
        private GridView _GridView;

        // Token: 0x04000004 RID: 4
        private bool _SizesSet;
    }
}
