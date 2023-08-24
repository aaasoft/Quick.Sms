﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Quick.Sms.Avalonia.ViewModels
{
    /// <summary>
    /// 通知属性更改的模型基类
    /// </summary>
    public abstract class PropertyNotifyModel : INotifyPropertyChanging, INotifyPropertyChanged
    {
        /// <summary>
        /// 属性正在改变事件
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;
        /// <summary>
        /// 属性已改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 触发属性正在改变事件
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisePropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// 触发属性已改变事件
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
