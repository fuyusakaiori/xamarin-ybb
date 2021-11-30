using System;
using System.Globalization;
using MasterDetailTemplate.Models;
using MasterDetailTemplate.Util;
using Xamarin.Forms;

namespace MasterDetailTemplate.Converters {
    /// <summary>
    /// ItemTappedEventArgs到错题的转换器。
    /// </summary>
    public class ItemTappedEventArgsToQuestionConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture) =>
            (value as ItemTappedEventArgs)?.Item as Question;

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture) {
            throw new DoNotCallThisExcpetion();
        }
    }
}