using System;

namespace Plugin.NFCWriterFeature
{
    /// <summary>
    /// Cross NFCWriterFeature
    /// </summary>
    public static class CrossNFCWriterFeature
    {
        static Lazy<INFCWriterFeature> implementation = new Lazy<INFCWriterFeature>(() => CreateNFCWriterFeature(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => implementation.Value == null ? false : true;

        /// <summary>
        /// Current plugin implementation to use
        /// </summary>
        public static INFCWriterFeature Current
        {
            get
            {
                INFCWriterFeature ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static INFCWriterFeature CreateNFCWriterFeature()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0
            return null;
#else
#pragma warning disable IDE0022 // Use expression body for methods
            return new NFCWriterFeatureImplementation();
#pragma warning restore IDE0022 // Use expression body for methods
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");

    }

    public enum TypeNameFormat
    {
        TNF_ABSOLUTE_URI = 3,
        TNF_EMPTY = 0,
        TNF_EXTERNAL_TYPE = 4,
        TNF_MIME_MEDIA = 2,
        TNF_UNCHANGED = 6,
        TNF_UNKNOWN = 5,
        TNF_WELL_KNOWN = 1
    };

    public enum RecordTypeDefinition
    {
        RTD_ALTERNATIVE_CARRIER,
        RTD_HANDOVER_CARRIER,
        RTD_HANDOVER_REQUEST,
        RTD_HANDOVER_SELECT,
        RTD_SMART_POSTER,
        RTD_TEXT,
        RTD_URI
    };

    public enum WriteResult
    {
        NULL,
        OK,
        TOOLARGE,
        READONLY,
        FORMATFAILED,
        NOTSUPPORTED,
        FAILED
    };
}
