using GimaSoft.Business.GINA;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.Windows;
using WPFShared;

namespace GimaSoft.GINA
{
    // Token: 0x02000002 RID: 2
    public class ApplicationUpdateControl : BindableControl
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        static ApplicationUpdateControl()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ApplicationUpdateControl), new FrameworkPropertyMetadata(typeof(ApplicationUpdateControl)));
        }

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000002 RID: 2 RVA: 0x000020A4 File Offset: 0x000002A4
        // (set) Token: 0x06000003 RID: 3 RVA: 0x000020B1 File Offset: 0x000002B1
        public string Message
        {
            get => base.Get<string>("Message"); set => base.Set("Message", value);
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000004 RID: 4 RVA: 0x000020BF File Offset: 0x000002BF
        // (set) Token: 0x06000005 RID: 5 RVA: 0x000020CC File Offset: 0x000002CC
        public string ErrorMessage
        {
            get => base.Get<string>("ErrorMessage"); set => base.Set("ErrorMessage", value);
        }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000006 RID: 6 RVA: 0x000020DA File Offset: 0x000002DA
        // (set) Token: 0x06000007 RID: 7 RVA: 0x000020E7 File Offset: 0x000002E7
        public int Status
        {
            get => base.Get<int>("Status"); set => base.Set("Status", value);
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000008 RID: 8 RVA: 0x000020FA File Offset: 0x000002FA
        // (set) Token: 0x06000009 RID: 9 RVA: 0x0000210C File Offset: 0x0000030C
        public bool UseAutoMode
        {
            get => (bool)base.GetValue(ApplicationUpdateControl.UseAutoModeProperty); set => base.SetValue(ApplicationUpdateControl.UseAutoModeProperty, value);
        }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600000A RID: 10 RVA: 0x0000211F File Offset: 0x0000031F
        // (set) Token: 0x0600000B RID: 11 RVA: 0x0000212C File Offset: 0x0000032C
        public string CurrentVersion
        {
            get => base.Get<string>("CurrentVersion"); set => base.Set("CurrentVersion", value);
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x0600000C RID: 12 RVA: 0x0000213A File Offset: 0x0000033A
        // (set) Token: 0x0600000D RID: 13 RVA: 0x00002147 File Offset: 0x00000347
        public string NewVersion
        {
            get => base.Get<string>("NewVersion"); set => base.Set("NewVersion", value);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002168 File Offset: 0x00000368
        private void DoUpdate()
        {
            Status = 11;
            Configuration.SaveConfiguration(false);
            Configuration.SaveConfiguration(true);
            ((App)global::System.Windows.Application.Current).Restarting = true;
            ApplicationDeployment.CurrentDeployment.UpdateCompleted += delegate (object oi, AsyncCompletedEventArgs ei)
            {
                global::System.Windows.Application.Current.Shutdown();
                global::System.Windows.Forms.Application.Restart();
            };
            ApplicationDeployment.CurrentDeployment.UpdateAsync();
        }

        // Token: 0x0600000F RID: 15 RVA: 0x00002248 File Offset: 0x00000448
        private void CheckUpdate()
        {
            Status = 0;
            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                Status = 1;
                return;
            }
            Configuration.SaveConfiguration(false);
            Status = 10;
            ApplicationDeployment.CurrentDeployment.CheckForUpdateCompleted += delegate (object o, CheckForUpdateCompletedEventArgs e)
            {
                if (e.Error != null)
                {
                    Status = 2;
                    ErrorMessage = e.Error.Message;
                    return;
                }
                if (!e.UpdateAvailable)
                {
                    Status = 3;
                    if (UseAutoMode)
                    {
                        base.Visibility = Visibility.Collapsed;
                    }
                    return;
                }
                NewVersion = e.AvailableVersion.ToString();
                CurrentVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                Status = 4;
            };
            ApplicationDeployment.CurrentDeployment.CheckForUpdateAsync();
        }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000010 RID: 16 RVA: 0x000022AC File Offset: 0x000004AC
        public GenericCommand VisibilityChangedCommand => new GenericCommand
        {
            Execute = delegate (object p)
            {
                if ((Visibility)p == Visibility.Visible)
                {
                    CheckUpdate();
                }
            }
        };

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000011 RID: 17 RVA: 0x000022DB File Offset: 0x000004DB
        public GenericCommand CloseCommand => new GenericCommand(delegate (object p)
                                                           {
                                                               base.Visibility = Visibility.Collapsed;
                                                           });

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x06000012 RID: 18 RVA: 0x000022F6 File Offset: 0x000004F6
        public GenericCommand DownloadCommand => new GenericCommand(delegate (object p)
                                                              {
                                                                  DoUpdate();
                                                              });

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x06000013 RID: 19 RVA: 0x00002316 File Offset: 0x00000516
        public GenericCommand OpenChangeLogCommand => new GenericCommand(delegate (object p)
                                                                   {
                                                                       _ = Process.Start(Configuration.GINAChangeLogURL);
                                                                   });

        // Token: 0x04000001 RID: 1
        private const int NoStatus = 0;

        // Token: 0x04000002 RID: 2
        private const int NotNetworkApp = 1;

        // Token: 0x04000003 RID: 3
        private const int Error = 2;

        // Token: 0x04000004 RID: 4
        private const int UpToDate = 3;

        // Token: 0x04000005 RID: 5
        private const int PromptToUpdate = 4;

        // Token: 0x04000006 RID: 6
        private const int Checking = 10;

        // Token: 0x04000007 RID: 7
        private const int Updating = 11;

        // Token: 0x04000008 RID: 8
        public static readonly DependencyProperty UseAutoModeProperty = DependencyProperty.Register("UseAutoMode", typeof(bool), typeof(ApplicationUpdateControl), null);
    }
}
