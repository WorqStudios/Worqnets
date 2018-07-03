using System;
using UnityEngine;

namespace DefaultNamespace
{
    public static class WorqnetsUtils
    {
        public enum MessageType
        {
            Warning, Error
        }
        
        public static void PrintMessage(string message, MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Warning:
                    Debug.Log(message);
                    break;
                case MessageType.Error:
                    Debug.LogError(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("messageType", messageType, null);
            }
        }
    }
}