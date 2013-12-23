using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Thornado;
using AIRLab.Thornado.TypeFormats;
using AIRLab.Thornado.Modules;

namespace AIRLab.Thornado
{
    public class Formats
    {
        public static readonly IntFormat Int = new IntFormat();
        public static readonly DoubleFormat Double = new DoubleFormat();
        public static readonly IPAddressFormat IPAddress = new IPAddressFormat();
        public static readonly IPEndPointFormat IPEndPoint = new IPEndPointFormat();
        public static readonly BoolFormat Bool = new BoolFormat();
        public static readonly GuidFormat Guid = new GuidFormat();
        public static readonly FullDateTimeFormat FullDateTime = new FullDateTimeFormat();
    }
}
