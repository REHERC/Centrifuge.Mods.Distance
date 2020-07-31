using Distance.TrackMusic.Models;
using Events;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Distance.TrackMusic
{
    [ModEntryPoint("com.github.corecii/Distance.TrackMusic")]
    public class Mod : MonoBehaviour
    {
        public static Mod Instance { get; set; }

        public IManager manager_;

        public ConfigurationLogic config_;

        public VariablesLogic variables_;

        public SoundPlayerLogic soundPlayer_;

        public LevelEditorLogic levelEditor_;

        public Log log_;

        public bool Enabled { get; set; } = true;

        public void Initialize(IManager manager)
        {
            Instance = this;
            manager_ = manager;

            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

            config_ = gameObject.AddComponent<ConfigurationLogic>();
            variables_ = gameObject.AddComponent<VariablesLogic>();
            soundPlayer_ = gameObject.AddComponent<SoundPlayerLogic>();
            levelEditor_ = gameObject.AddComponent<LevelEditorLogic>();
            log_ = LogManager.GetForCurrentAssembly();

            MusicTrack.Info.Register();
            MusicChoice.Info.Register();

            DirectoryEx.CreateIfDoesNotExist("EditorMusic/");

            RuntimePatcher.AutoPatch();
        }

        public void LateInitialize(IManager _)
        {
            log_.Warning("LateInitialize");
            PatchPostLoad(true);
        }

        public void PatchPostLoad(bool subscribe)
        {
            var audioManager = G.Sys.AudioManager_;

            RemoveParticularSubscriber<Events.Level.PostLoad.Data>(audioManager);

            //var list = (SubscriberList)typeof(AudioManager).GetField("subscriberList_", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(audioManager);

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

        public void /*StaticEvent<T>.Delegate*/ RemoveParticularSubscriber<T>(AudioManager component)
        {
            // Made code specific to audiomanager since it's only referenced once

            //const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            /*FieldInfo subscriberListField_ = component.GetType().GetField("subscriberList_", bindingFlags);

            if (subscriberListField_ == null)
            {
                return null;
            }

            SubscriberList subscriberList_ = subscriberListField_.GetValue(component) as SubscriberList;*/

            SubscriberList subscriberList_ = component?.subscriberList_;

            if (subscriberList_ is null)
            {
                return;
            }

            StaticEvent<T>.Delegate func;

            //int index = 0;

            foreach (IEventSubscriber subscriber in new List<IEventSubscriber>(subscriberList_))
            {
                if (subscriber is StaticEvent<T>.Subscriber eventSubscriber)
                {
                    //FieldInfo funcField_ = subscriber.GetType().GetField("func_", bindingFlags);

                    func = eventSubscriber?.func_;

                    if (func != null)
                    {
                        subscriberList_.Remove(subscriber);
                    }

                    /*if (funcField_ != null)
                    {
                        func = funcField_.GetValue(subscriber) as StaticEvent<T>.Delegate;
                        break;
                    }*/
                }
                //index++;
            }

            /*if (func != null)
            {
                subscriberList_.RemoveAt(index);
            }*/

            //return func;
        }
    }
}