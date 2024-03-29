using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.TransformUtils;

namespace Utils
{
    public static class Log
    {
        public delegate string DescribeDelegate<T>(T sender);

        private delegate string DescribeDelegate(object sender);

        private static Dictionary<Type, DescribeDelegate> _senderDescribersByType;

        public static void Info(object sender, string message)
        {
            DescribeSender(sender, ref message);
            Info(sender.GetType(), message);
        }

        public static void Info(Type senderType, string message)
        {
            Info(senderType.Name, message);
        }

        public static void Info(string senderName, string message)
        {
            Debug.Log($"[{senderName}] {message}");
        }

        public static void Warning(object sender, string message)
        {
            DescribeSender(sender, ref message);
            Warning(sender.GetType(), message);
        }

        public static void Warning(Type senderType, string message)
        {
            Warning(senderType.Name, message);
        }

        public static void Warning(string senderName, string message)
        {
            Debug.LogWarning($"[{senderName}] {message}");
        }

        public static void Error(object sender, string message)
        {
            DescribeSender(sender, ref message);
            Error(sender.GetType(), message);
        }

        public static void Error(Type senderType, string message)
        {
            Error(senderType.Name, message);
        }

        public static void Error(string senderName, string message)
        {
            Debug.LogError($"[{senderName}] {message}");
        }

        private static void DescribeSender(object sender, ref string message)
        {
            CheckDescribers();

            var senderType = sender.GetType();
            foreach (var entry in _senderDescribersByType)
            {
                if (senderType.IsSubclassOf(entry.Key))
                {
                    message += entry.Value.Invoke(sender);
                    return;
                }
            }
        }

        private static void CheckDescribers()
        {
            if (_senderDescribersByType != null)
                return;

            _senderDescribersByType = new Dictionary<Type, DescribeDelegate>();

            RegisterSenderDescriber<UnityEngine.MonoBehaviour>(sender =>
            {
                var path = sender.gameObject.scene.path;
                if (!string.IsNullOrEmpty(path))
                    path += "/";

                return $"\nat {path}{sender.transform.GetPath()}";
            });

            RegisterSenderDescriber<ScriptableObject>(sender => $"\nat {sender.name}");
        }

        public static void RegisterSenderDescriber<T>(DescribeDelegate<T> describer)
        {
            _senderDescribersByType.Add(typeof(T), sender => describer.Invoke((T) sender));
        }
    }
}