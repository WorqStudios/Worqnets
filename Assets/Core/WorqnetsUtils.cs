﻿using System;
using UnityEngine;

namespace Worq.Worqnets.Core
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
                    throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
            }
        }
    }
}