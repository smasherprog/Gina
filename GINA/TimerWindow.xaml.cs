using GimaSoft.Business.GINA;
using Microsoft.Windows.Controls;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFShared;

namespace GimaSoft.GINA
{
    // Token: 0x02000017 RID: 23
    public partial class TimerWindow : Window
    {
        // Token: 0x060002FA RID: 762 RVA: 0x0000A060 File Offset: 0x00008260
        public static TimerWindow NewWindow(BehaviorGroup behavior)
        {
            var timerWindow = (TimerWindow)Application.LoadComponent(new Uri("TimerWindow.xaml", UriKind.Relative));
            timerWindow.DataContext = new TimerWindowViewModel(behavior)
            {
                Window = timerWindow
            };
            return timerWindow;
        }

        // Token: 0x060002FB RID: 763 RVA: 0x0000A099 File Offset: 0x00008299
        public TimerWindow()
        {
            InitializeComponent();
            base.SourceInitialized += InitializeWindowSource;
        }

        // Token: 0x060002FC RID: 764
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        // Token: 0x060002FD RID: 765
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        // Token: 0x060002FE RID: 766 RVA: 0x0000A0B9 File Offset: 0x000082B9
        private void InitializeWindowSource(object sender, EventArgs e)
        {
            hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
        }

        // Token: 0x060002FF RID: 767 RVA: 0x0000A0D1 File Offset: 0x000082D1
        private void ResizeWindow(TimerWindow.ResizeDirection direction)
        {
            _ = TimerWindow.SendMessage(hwndSource.Handle, 274U, (IntPtr)(long)(61440 + direction), IntPtr.Zero);
        }

        // Token: 0x06000300 RID: 768 RVA: 0x0000A0FB File Offset: 0x000082FB
        private void ResetCursor(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                base.Cursor = Cursors.Arrow;
            }
        }

        // Token: 0x06000301 RID: 769 RVA: 0x0000A110 File Offset: 0x00008310
        private void Resize(object sender, MouseButtonEventArgs e)
        {
            var rectangle = sender as Rectangle;
            switch (_ = rectangle.Name)
            {
                case "top":
                    base.Cursor = Cursors.SizeNS;
                    ResizeWindow(TimerWindow.ResizeDirection.Top);
                    return;
                case "bottom":
                    base.Cursor = Cursors.SizeNS;
                    ResizeWindow(TimerWindow.ResizeDirection.Bottom);
                    return;
                case "left":
                    base.Cursor = Cursors.SizeWE;
                    ResizeWindow(TimerWindow.ResizeDirection.Left);
                    return;
                case "right":
                    base.Cursor = Cursors.SizeWE;
                    ResizeWindow(TimerWindow.ResizeDirection.Right);
                    return;
                case "topLeft":
                    base.Cursor = Cursors.SizeNWSE;
                    ResizeWindow(TimerWindow.ResizeDirection.TopLeft);
                    return;
                case "topRight":
                    base.Cursor = Cursors.SizeNESW;
                    ResizeWindow(TimerWindow.ResizeDirection.TopRight);
                    return;
                case "bottomLeft":
                    base.Cursor = Cursors.SizeNESW;
                    ResizeWindow(TimerWindow.ResizeDirection.BottomLeft);
                    return;
                case "bottomRight":
                    base.Cursor = Cursors.SizeNWSE;
                    ResizeWindow(TimerWindow.ResizeDirection.BottomRight);
                    break;
            }
        }

        // Token: 0x06000302 RID: 770 RVA: 0x0000A27C File Offset: 0x0000847C
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

        // Token: 0x06000303 RID: 771 RVA: 0x0000A3AD File Offset: 0x000085AD
        private void TextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _ = TimerWindow.ReleaseCapture();
            _ = TimerWindow.SendMessage(hwndSource.Handle, 161U, (IntPtr)2, IntPtr.Zero);
        }

        // Token: 0x06000304 RID: 772 RVA: 0x0000A3D8 File Offset: 0x000085D8
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var timerWindowViewModel = base.DataContext as TimerWindowViewModel;
            timerWindowViewModel.PropertyChanged += tv_PropertyChanged;
            this.SetPlacement(timerWindowViewModel.Behavior.WindowLayout);
            this.HideFromAltTab();
            if (timerWindowViewModel.ShowOpaqueWindow)
            {
                this.ClearOverlay();
                return;
            }
            this.MakeOverlay();
        }

        // Token: 0x06000305 RID: 773 RVA: 0x0000A430 File Offset: 0x00008630
        private void tv_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _ = new ColorPicker();
            if (e.PropertyName == "ShowOpaqueWindow")
            {
                var timerWindowViewModel = base.DataContext as TimerWindowViewModel;
                if (timerWindowViewModel.ShowOpaqueWindow)
                {
                    this.ClearOverlay();
                    this.BringToTop();
                    return;
                }
                this.MakeOverlay();
            }
        }

        // Token: 0x0400007E RID: 126
        private const int WM_SYSCOMMAND = 274;

        // Token: 0x0400007F RID: 127
        private const int WM_NCLBUTTONDOWN = 161;

        // Token: 0x04000080 RID: 128
        private const int HT_CAPTION = 2;

        // Token: 0x04000081 RID: 129
        private HwndSource hwndSource;

        // Token: 0x02000018 RID: 24
        public enum ResizeDirection
        {
            // Token: 0x0400008D RID: 141
            Left = 1,
            // Token: 0x0400008E RID: 142
            Right,
            // Token: 0x0400008F RID: 143
            Top,
            // Token: 0x04000090 RID: 144
            TopLeft,
            // Token: 0x04000091 RID: 145
            TopRight,
            // Token: 0x04000092 RID: 146
            Bottom,
            // Token: 0x04000093 RID: 147
            BottomLeft,
            // Token: 0x04000094 RID: 148
            BottomRight
        }
    }
}
