using Avalonia.Metadata;
using System;

namespace Quick.Sms.Desktop.Converters
{
    //[System.Windows.Data.ValueConversion(typeof(Boolean?), typeof(Boolean?))]
    public class BooleanReverseConverter : BooleanToTConverter<Boolean?>
    {
        public BooleanReverseConverter()
        {
            //默认值
            NullValue = null;
            TrueValue = false;
            FalseValue = true;
        }
    }
}
