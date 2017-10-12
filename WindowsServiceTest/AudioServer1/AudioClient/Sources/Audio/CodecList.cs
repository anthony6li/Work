using System.Collections.Generic;
using AudioServer1.Sources.Audio.codecs;

namespace AudioServer1.Sources.Audio
{
    static class CodecList
    {
        private static List<INetworkChatCodec> _codecs;
        public static List<INetworkChatCodec> Codecs
        {
            get
            {
                if (_codecs != null)
                    return _codecs;
                _codecs = new List<INetworkChatCodec>
                              {
                                  new AcmALawChatCodec(),
                                  new G722ChatCodec(),
                                  new Gsm610ChatCodec(),
                                  new MicrosoftAdpcmChatCodec(),
                                  new MuLawChatCodec(),
                                  new UncompressedPcmChatCodec()
                              };

                return _codecs;
            }
        }
    }
}
