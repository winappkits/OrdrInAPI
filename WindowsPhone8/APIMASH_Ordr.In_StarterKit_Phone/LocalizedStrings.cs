﻿using APIMASH_Ordr.In_StarterKit_Phone.Resources;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings
    {
        private static AppResources _localizedResources = new AppResources();

        public AppResources LocalizedResources { get { return _localizedResources; } }
    }
}