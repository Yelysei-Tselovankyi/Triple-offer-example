using System;
using System.Collections.Generic;

namespace CodeBase.Services.ProgressService.SerializableTypes
{
    [Serializable]
    public class PlayerProgress
    {
        public Dictionary<string, BaseEventProgress> EventsProgress = new ();
    }
}