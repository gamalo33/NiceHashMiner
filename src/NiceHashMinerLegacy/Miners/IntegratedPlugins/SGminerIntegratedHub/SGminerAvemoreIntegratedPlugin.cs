﻿using MinerPlugin;
using NiceHashMinerLegacy.Common.Algorithm;
using NiceHashMinerLegacy.Common.Device;
using NiceHashMinerLegacy.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners.IntegratedPlugins
{
    class SGminerAvemoreIntegratedPlugin : SGMinerPluginBase
    {
        const string _pluginUUIDName = "SGminerAvemore";

        public override string PluginUUID => _pluginUUIDName;

        public override Version Version => new Version(1, 0);

        public override string Name => _pluginUUIDName;


        public override IMiner CreateMiner()
        {
            return new SGminerAvemoreIntegratedMiner(PluginUUID, AMDDevice.OpenCLPlatformID)
            {
                MinerOptionsPackage = _minerOptionsPackage,
                MinerSystemEnvironmentVariables = _minerSystemEnvironmentVariables
            };
        }

        public override Dictionary<BaseDevice, IReadOnlyList<Algorithm>> GetSupportedAlgorithms(IEnumerable<BaseDevice> devices)
        {
            var supported = new Dictionary<BaseDevice, IReadOnlyList<Algorithm>>();
            var amdGpus = devices
                .Where(dev => dev is AMDDevice)
                .Cast<AMDDevice>();

            foreach (var gpu in amdGpus)
            {
                var algorithms = new List<Algorithm> {
                    new Algorithm(PluginUUID, AlgorithmType.X16R)
                    {
                        ExtraLaunchParameters = "-X 256"
                    },
                };
                supported.Add(gpu, algorithms);
            }

            return supported;
        }
    }
}
