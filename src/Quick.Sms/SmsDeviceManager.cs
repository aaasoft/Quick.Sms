using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms
{
    public class SmsDeviceManager
    {
        public static SmsDeviceManager Instnce { get; } = new SmsDeviceManager();

        private Dictionary<string, ISmsDevice> deviceDict = new Dictionary<string, ISmsDevice>();
        private Dictionary<string, SmsDeviceTypeInfo> deviceTypeDict = new Dictionary<string, SmsDeviceTypeInfo>();

        private SmsDeviceManager()
        {
            Register(this.GetType().Assembly);
        }

        public void Register<TDevice>()
            where TDevice : ISmsDevice
        {
            Register(typeof(TDevice));
        }

        public void Register(Type deviceType)
        {
            var device = (ISmsDevice)Activator.CreateInstance(deviceType);
            Register(device);
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

        public void Register(params Assembly[] assemblys)
        {
            foreach (var assembly in assemblys)
                foreach (var type in assembly.GetTypes())
                    if (type.IsPublic && type.IsClass && !type.IsAbstract && typeof(ISmsDevice).IsAssignableFrom(type))
                        Register(type);
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
            var model = (ISmsDevice)Activator.CreateInstance(masterDevice.GetType());
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
