using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NBC.ActionEditor;
using System.Reflection;

namespace CCx
{
    public class CommandProcessor
    {
        private static Dictionary<Type, Type> _processorMap;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            if (_processorMap != null) return;
            _processorMap = new Dictionary<Type, Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    var attribute = type.GetCustomAttribute<CommandProcessorAttribute>();
                    if (attribute != null && typeof(IDirectable).IsAssignableFrom(type))
                    {
                        _processorMap[attribute.CommandType] = type;
                    }
                }
            }
        }
        public static IDirectable GetProcessor(Type commandType)
        {
            if (_processorMap.TryGetValue(commandType, out Type processorType))
            {
                return (IDirectable)Activator.CreateInstance(processorType);
            }
            return null;
        }
    }

}


