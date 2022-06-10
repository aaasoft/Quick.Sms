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
            deviceDict[key] = (ISmsDevice)device;
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
            return deviceDict.Select(t => new SmsDeviceTypeInfo()
            {
                Id = t.Key,
                Name = t.Value.Name
            }).ToArray();
        }

        /// <summary>
        /// 创建设备实例
        /// </summary>
        /// <returns></returns>
        public ISmsDevice CreateDeviceInstance(string deviceTypeId, Object settingObj)
        {
            if (!deviceDict.ContainsKey(deviceTypeId))
                return null;
            var masterDevice = deviceDict[deviceTypeId];
            var model = (ISmsDevice)Activator.CreateInstance(masterDevice.GetType());
            model.Init(settingObj);
            return model;
        }
    }
}
