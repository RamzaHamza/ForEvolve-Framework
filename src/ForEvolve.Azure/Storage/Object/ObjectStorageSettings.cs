﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ForEvolve.Azure.Storage.Object
{
    public class ObjectStorageSettings : StorageSettings, IObjectStorageSettings
    {
        public string ContainerName { get; set; }
    }
}
