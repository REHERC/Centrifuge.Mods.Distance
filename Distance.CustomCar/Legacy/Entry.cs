#pragma warning disable IDE0052
using Events.MainMenu;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using Reactor.API.Storage;
using System;
using System.Collections.Generic;

namespace CustomCar.Legacy
{
    [ModEntryPoint("com.github.larnin/CustomCar")]
    public class Mod
    {
        private readonly List<Assets> _assets = new List<Assets>();
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        public void Initialize(IManager _)
        {
            try
            {
                RuntimePatcher.AutoPatch();
            }
            catch (Exception e)
            {
                _log.Exception(e);
            }

            Initialized.Subscribe(data =>
            {
                try
                {
                    CarInfos carInfos = new CarInfos();
                    carInfos.CollectInfos();
                    CarBuilder builder = new CarBuilder();
                    builder.CreateCars(carInfos);
                }
                catch (Exception e)
                {
                    ErrorList.Add("An error occured while trying to load cars assets.");
                    _log.Exception(e);
                }

                if (ErrorList.HaveErrors())
                {
                    ErrorList.Show();
                }
            });
        }

        public void Load()
        {
            ModdedCarsColors.LoadAll();
        }
    }
}