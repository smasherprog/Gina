using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace WPFShared
{
    // Token: 0x02000002 RID: 2
    public class ComboBoxAutoOpenBehavior : Behavior<ComboBox>
    {
        // Token: 0x06000001 RID: 1 RVA: 0x0000205E File Offset: 0x0000025E
        protected override void OnAttached()
        {
            base.OnAttached();
            base.AssociatedObject.Loaded += delegate (object o, RoutedEventArgs e)
            {
                base.AssociatedObject.IsDropDownOpen = true;
            };
        }
    }
}
