using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WPFShared.Converters
{
    // Token: 0x0200002E RID: 46
    public class InListToVisibilityConverter : IValueConverter
    {
        // Token: 0x060001AB RID: 427 RVA: 0x00007F24 File Offset: 0x00006124
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] array = null;
            var visibility = Visibility.Visible;
            if (parameter != null)
            {
                var text = parameter.ToString();
                if (text.Contains('#'))
                {
                    var array2 = text.Split(new char[] { '#' });
                    text = array2[0];
                    if (array2[1].ToLower() == "collapsed")
                    {
                        visibility = Visibility.Collapsed;
                    }
                }
                array = text.Split(new char[] { '|' });
            }
            var visibility2 = (visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            if (value == null)
            {
                return (array != null && array.Contains("null")) ? visibility : visibility2;
            }
            if (value != null)
            {
                List<string> list;
                if (value is string)
                {
                    list = new List<string>(((string)value).Split(new char[] { '|' }));
                }
                else
                {
                    if (value is IEnumerable)
                    {
                        list = new List<string>();
                        foreach (var obj in (IEnumerable)value)
                        {
                            list.Add(obj.ToString());
                        }
                        goto IL_0130;
                    }
                    list = new List<string>(value.ToString().Split(new char[] { '|' }));
                }
            IL_0130:
                foreach (var text2 in list)
                {
                    if (array.Contains(text2))
                    {
                        return visibility;
                    }
                }
            }
            return visibility2;
        }

        // Token: 0x060001AC RID: 428 RVA: 0x000080C8 File Offset: 0x000062C8
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
