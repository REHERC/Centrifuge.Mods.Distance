using System;
using System.Collections.Generic;
using System.Text;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using UnityEngine;

namespace ExampleNamespace
{
    [ModEntryPoint(ModID)]
    public class Mod : MonoBehaviour
    {
        public const string ModID = "com.example.someone/ExampleModID";

        public void Initialize(IManager manager)
        {
            // This will run after Awake(), unless 
            // specified otherwise in ModEntryPoint
            // attribute property AwakeAfterInitialize.
        }

        public void Awake()
        {

        }
    }
}