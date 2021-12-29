using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MasterDetailTemplate.Models;
using MasterDetailTemplate.Util;
using Xamarin.Forms;

namespace MasterDetailTemplate.Converters
{
    public class ItemTappedEventArgsToQuestionCategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture) =>
            (value as ItemTappedEventArgs)?.Item as QuestionCategory;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new DoNotCallThisExcpetion();
        }
    }
}
