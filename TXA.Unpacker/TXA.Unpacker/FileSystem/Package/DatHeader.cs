using System;

namespace TXA.Unpacker
{
    class DatHeader
    {
        public String m_Magic { get; set; } // GAMEDAT PAC2
        public Int32 dwTotalFiles { get; set; }
    }
}
