using System.Threading;
using System.Windows;

namespace WPFShared
{
    // Token: 0x02000014 RID: 20
    public class Clipboard
    {
        // Token: 0x06000089 RID: 137 RVA: 0x00003D4C File Offset: 0x00001F4C
        public static bool SetText(string text)
        {
            for (var i = 0; i < 10; i++)
            {
                try
                {
                    System.Windows.Clipboard.SetData(DataFormats.Text, text);
                    return true;
                }
                catch
                {
                    Thread.Sleep(50);
                }
            }
            return false;
        }
    }
}
