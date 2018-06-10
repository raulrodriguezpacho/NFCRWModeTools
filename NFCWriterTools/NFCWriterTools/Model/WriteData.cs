using System;
using System.Collections.Generic;
using System.Text;

namespace NFCWriterTools.Model
{
    public class WriteData
    {
        public string Content { get; set; }
        public WriteDataType Type { get; set; }
    }

    public enum WriteDataType
    {
        AbsoluteUri,        
        Aplication,
        MimeCard,
        MimeJson,
        ExternalType
    };
}
