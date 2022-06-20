using System;

namespace TXA.Packer
{
    class DatEntry
    {
        public String m_FileName { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwSize { get; set; }
    }
}
