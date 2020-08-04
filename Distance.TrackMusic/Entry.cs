using Distance.TrackMusic.Models;
using Events;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.TrackMusic
{
    [ModEntryPoint("com.github.corecii/Distance.TrackMusic")]
    public class Mod : MonoBehaviour
    {
        public static Mod Instance { get; set; }

        public IManager Manager { get; set; }

        public ConfigurationLogic Config { get; set; }

        public VariablesLogic Variables { get; set; }

        public SoundPlayerLogic SoundPlayer { get; set; }

        public LevelEditorLogic LevelEditor { get; set; }

        public Log Logger { get; set; }

        public bool Enabled { get; set; } = true;

        public void Initialize(IManager manager)
        {
            Instance = this;
            Manager = manager;

            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

            Config = gameObject.AddComponent<ConfigurationLogic>();
            Variables = gameObject.AddComponent<VariablesLogic>();
            SoundPlayer = gameObject.AddComponent<SoundPlayerLogic>();
            LevelEditor = gameObject.AddComponent<LevelEditorLogic>();
            Logger = LogManager.GetForCurrentAssembly();

            MusicTrack.Info.Register();
            MusicChoice.Info.Register();

            DirectoryEx.CreateIfDoesNotExist("EditorMusic/");

            RuntimePatcher.AutoPatch();
        }

        public void LateInitialize(IManager _)
        {
            Logger.Warning("LateInitialize");
            PatchPostLoad(true);
        }

        public void PatchPostLoad(bool subscribe)
        {
            var audioManager = G.Sys.AudioManager_;

            RemoveParticularSubscriber<Events.Level.PostLoad.Data>(audioManager);

            var list = G.Sys.AudioManager_.subscriberList_;

            var item = new StaticEvent<Events.Level.PostLoad.Data>.Subscriber(new StaticEvent<Events.Level.PostLoad.Data>.Delegate(data =>
            {
                Debug.Log("Running PostLoad");

                G.Sys.AudioManager_.OnEventPostLoad(data);
            }));

            list.Add(item);

            if (subscribe)
            {
                (item as IEventSubscriber).Subscribe();
            }
        }

        public void RemoveParticularSubscriber<T>(AudioManager component)
        {
            SubscriberList subscriberList_ = component?.subscriberList_;

            if (subscriberList_ is null)
            {
                return;
            }

            StaticEvent<T>.Delegate func;

            foreach (IEventSubscriber subscriber in new List<IEventSubscriber>(subscriberList_))
            {
                if (subscriber is StaticEvent<T>.Subscriber eventSubscriber)
                {
                    func = eventSubscriber?.func_;

                    if (func != null)
                    {
                        subscriberList_.Remove(subscriber);
                    }
                }
            }
        }
    }
}