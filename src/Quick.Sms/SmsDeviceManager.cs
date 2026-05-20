using System;
using System.Collections.Generic;
using System.Linq;

namespace Quick.Sms
{
    public class SmsDeviceManager
    {
        public static SmsDeviceManager Instnce { get; } = new SmsDeviceManager();

        private Dictionary<string, ISmsDevice> deviceDict = new Dictionary<string, ISmsDevice>();
        private Dictionary<string, SmsDeviceTypeInfo> deviceTypeDict = new Dictionary<string, SmsDeviceTypeInfo>();

        private SmsDeviceManager()
        {
            Register<SIMComModems.SIM7600.Device>();
            Register<WavecomModems.CDMA.Device>();
            Register<WavecomModems.Q2403.Device>();
            Register<SiemensModems.MC52i.Device>();
            Register<MeiGModems.SLM320P.Device>();
            Register<HuaweiModems.MC323.Device>();
        }

        public void Register<TDevice>()
            where TDevice : class, ISmsDevice, new()
        {
            Register(new TDevice());
        }

        private void Register(ISmsDevice device)
        {
            var key = device.GetType().FullName;
            deviceDict[key] = device;
            deviceTypeDict[key] = new SmsDeviceTypeInfo()
            {
                Id = key,
                Name = device.Name
            };
        }

        public ISmsDevice[] GetMasterDeviceTypes()
        {
            return deviceDict.Values.ToArray();
        }

        public SmsDeviceTypeInfo[] GetDeviceTypeInfos()
        {
            return deviceTypeDict.Values.ToArray();
        }

        /// <summary>
        /// 创建设备实例
        /// </summary>
        /// <returns></returns>
        public ISmsDevice CreateDeviceInstance(string deviceTypeId, Object settingObj)
        {
            if (!deviceDict.TryGetValue(deviceTypeId, out var masterDevice))
                return null;
            var model = masterDevice.CreateNewInstance();
            model.Init(settingObj);
            return model;
        }

        /// <summary>
        /// 获取设备类型信息
        /// </summary>
        /// <param name="deviceTypeId"></param>
        /// <returns></returns>
        public SmsDeviceTypeInfo GetDeviceTypeInfo(string deviceTypeId)
        {
            if (deviceTypeDict.TryGetValue(deviceTypeId, out var model))
                return model;
            return null;
        }
    }
}
