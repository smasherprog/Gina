using GimaSoft.Business.GINA;
using Microsoft.Windows.Controls;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPFShared;

namespace GimaSoft.GINA
{
    // Token: 0x0200001C RID: 28
    public partial class TextWindow : Window
    {
        // Token: 0x0600034A RID: 842 RVA: 0x0000B250 File Offset: 0x00009450
        public static TextWindow NewWindow(BehaviorGroup behavior)
        {
            var textWindow = (TextWindow)Application.LoadComponent(new Uri("TextWindow.xaml", UriKind.Relative));
            textWindow.DataContext = new TextWindowViewModel(behavior)
            {
                Window = textWindow
            };
            return textWindow;
        }

        // Token: 0x0600034B RID: 843 RVA: 0x0000B289 File Offset: 0x00009489
        public TextWindow()
        {
            InitializeComponent();
            base.SourceInitialized += InitializeWindowSource;
        }

        // Token: 0x0600034C RID: 844
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        // Token: 0x0600034D RID: 845
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        // Token: 0x0600034E RID: 846 RVA: 0x0000B2A9 File Offset: 0x000094A9
        private void InitializeWindowSource(object sender, EventArgs e)
        {
            hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
        }

        // Token: 0x0600034F RID: 847 RVA: 0x0000B2C1 File Offset: 0x000094C1
        private void ResizeWindow(TextWindow.ResizeDirection direction)
        {
            _ = TextWindow.SendMessage(hwndSource.Handle, 274U, (IntPtr)(long)(61440 + direction), IntPtr.Zero);
        }

        // Token: 0x06000350 RID: 848 RVA: 0x0000B2EB File Offset: 0x000094EB
        private void ResetCursor(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                base.Cursor = Cursors.Arrow;
            }
        }

        // Token: 0x06000351 RID: 849 RVA: 0x0000B300 File Offset: 0x00009500
        private void Resize(object sender, MouseButtonEventArgs e)
        {
            var rectangle = sender as Rectangle;
            switch (_ = rectangle.Name)
            {
                case "top":
                    base.Cursor = Cursors.SizeNS;
                    ResizeWindow(TextWindow.ResizeDirection.Top);
                    return;
                case "bottom":
                    base.Cursor = Cursors.SizeNS;
                    ResizeWindow(TextWindow.ResizeDirection.Bottom);
                    return;
                case "left":
                    base.Cursor = Cursors.SizeWE;
                    ResizeWindow(TextWindow.ResizeDirection.Left);
                    return;
                case "right":
                    base.Cursor = Cursors.SizeWE;
                    ResizeWindow(TextWindow.ResizeDirection.Right);
                    return;
                case "topLeft":
                    base.Cursor = Cursors.SizeNWSE;
                    ResizeWindow(TextWindow.ResizeDirection.TopLeft);
                    return;
                case "topRight":
                    base.Cursor = Cursors.SizeNESW;
                    ResizeWindow(TextWindow.ResizeDirection.TopRight);
                    return;
                case "bottomLeft":
                    base.Cursor = Cursors.SizeNESW;
                    ResizeWindow(TextWindow.ResizeDirection.BottomLeft);
                    return;
                case "bottomRight":
                    base.Cursor = Cursors.SizeNWSE;
                    ResizeWindow(TextWindow.ResizeDirection.BottomRight);
                    break;
            }
        }

        // Token: 0x06000352 RID: 850 RVA: 0x0000B46C File Offset: 0x0000966C
        private void DisplayResizeCursor(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;
            switch (_ = rectangle.Name)
            {
                case "top":
                    base.Cursor = Cursors.SizeNS;
                    return;
                case "bottom":
                    base.Cursor = Cursors.SizeNS;
                    return;
                case "left":
                    base.Cursor = Cursors.SizeWE;
                    return;
                case "right":
                    base.Cursor = Cursors.SizeWE;
                    return;
                case "topLeft":
                    base.Cursor = Cursors.SizeNWSE;
                    return;
                case "topRight":
                    base.Cursor = Cursors.SizeNESW;
                    return;
                case "bottomLeft":
                    base.Cursor = Cursors.SizeNESW;
                    return;
                case "bottomRight":
                    base.Cursor = Cursors.SizeNWSE;
                    break;
            }
        }

        // Token: 0x06000353 RID: 851 RVA: 0x0000B59D File Offset: 0x0000979D
        private void TextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _ = TextWindow.ReleaseCapture();
            _ = TextWindow.SendMessage(hwndSource.Handle, 161U, (IntPtr)2, IntPtr.Zero);
        }

        // Token: 0x06000354 RID: 852 RVA: 0x0000B650 File Offset: 0x00009850
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var textWindowViewModel = base.DataContext as TextWindowViewModel;
            textWindowViewModel.PropertyChanged += tv_PropertyChanged;
            this.SetPlacement(textWindowViewModel.Behavior.WindowLayout);
            this.HideFromAltTab();
            if (textWindowViewModel.ShowOpaqueWindow)
            {
                this.ClearOverlay();
            }
            else
            {
                this.MakeOverlay();
            }
            ((INotifyCollectionChanged)icTexts.Items).CollectionChanged += delegate (object oi, NotifyCollectionChangedEventArgs ei)
            {
                if (ei.NewItems != null && ei.NewItems.Count > 0)
                {
                    _ = base.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                    {
                        icTexts.ScrollIntoView(ei.NewItems[0]);
                    }));
                }
            };
        }

        // Token: 0x06000355 RID: 853 RVA: 0x0000B6C4 File Offset: 0x000098C4
        private void tv_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _ = new ColorPicker();
            if (e.PropertyName == "ShowOpaqueWindow")
            {
                var textWindowViewModel = base.DataContext as TextWindowViewModel;
                if (textWindowViewModel.ShowOpaqueWindow)
                {
                    this.ClearOverlay();
                    this.BringToTop();
                    return;
                }
                this.MakeOverlay();
            }
        }

        // Token: 0x040000AA RID: 170
        private const int WM_SYSCOMMAND = 274;

        // Token: 0x040000AB RID: 171
        private const int WM_NCLBUTTONDOWN = 161;

        // Token: 0x040000AC RID: 172
        private const int HT_CAPTION = 2;

        // Token: 0x040000AD RID: 173
        private HwndSource hwndSource;

        // Token: 0x0200001D RID: 29
        public enum ResizeDirection
        {
            // Token: 0x040000BA RID: 186
            Left = 1,
            // Token: 0x040000BB RID: 187
            Right,
            // Token: 0x040000BC RID: 188
            Top,
            // Token: 0x040000BD RID: 189
            TopLeft,
            // Token: 0x040000BE RID: 190
            TopRight,
            // Token: 0x040000BF RID: 191
            Bottom,
            // Token: 0x040000C0 RID: 192
            BottomLeft,
            // Token: 0x040000C1 RID: 193
            BottomRight
        }
    }
}
