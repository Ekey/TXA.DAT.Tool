using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace TXA.Unpacker
{
    class DatUnpack
    {
        static List<DatEntry> m_EntryTable = new List<DatEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            using (FileStream TArchiveStream = File.OpenRead(m_Archive))
            {
                var m_Header = new DatHeader();

                m_Header.m_Magic = Encoding.ASCII.GetString(TArchiveStream.ReadBytes(12));
                m_Header.dwTotalFiles = TArchiveStream.ReadInt32();

                if (m_Header.m_Magic != "GAMEDAT PAC2")
                {
                    Utils.iSetError("[ERROR]: Invalid magic of DAT archive file");
                    return;
                }

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    String m_FileName = Encoding.ASCII.GetString(TArchiveStream.ReadBytes(32)).TrimEnd('\0');

                    var m_Entry = new DatEntry
                    {
                        m_FileName = m_FileName,
                        dwOffset = 0,
                        dwSize = 0,
                    };

                    m_EntryTable.Add(m_Entry);
                }

                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    UInt32 dwOffset = TArchiveStream.ReadUInt32();
                    Int32 dwSize = TArchiveStream.ReadInt32();

                    m_EntryTable[i].dwOffset = dwOffset;
                    m_EntryTable[i].dwSize = dwSize;
                }

                UInt32 dwBaseOffset = (UInt32)TArchiveStream.Position;

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FullPath = m_DstFolder + m_Entry.m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_Entry.m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TArchiveStream.Seek(m_Entry.dwOffset + dwBaseOffset, SeekOrigin.Begin);

                    var lpBuffer = TArchiveStream.ReadBytes(m_Entry.dwSize);

                    File.WriteAllBytes(m_FullPath, lpBuffer);
                }

                TArchiveStream.Dispose();
            }
        }
    }
}
