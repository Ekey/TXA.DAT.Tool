using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace TXA.Packer
{
    class DatPack
    {
        static List<DatEntry> m_EntryTable = new List<DatEntry>();

        public static void iDoIt(String m_Archive, String m_SrcFolder)
        {
            var m_Header = new DatHeader();

            m_Header.m_Magic = "GAMEDAT PAC2";
            m_Header.dwTotalFiles = Directory.GetFiles(m_SrcFolder, "*.*", SearchOption.AllDirectories).Length;

            using (BinaryWriter TDatWriter = new BinaryWriter(File.Open(m_Archive, FileMode.Create)))
            {
                TDatWriter.Write(Encoding.ASCII.GetBytes(m_Header.m_Magic));
                TDatWriter.Write(m_Header.dwTotalFiles);

                Int32 dwReservedSize = m_Header.dwTotalFiles * 32 + m_Header.dwTotalFiles * 8;

                Byte[] lpReserved = new Byte[dwReservedSize];
                TDatWriter.Write(lpReserved);

                UInt32 dwBaseOffset = (UInt32)TDatWriter.BaseStream.Position;

                m_EntryTable.Clear();
                foreach (String m_File in Directory.GetFiles(m_SrcFolder, "*.*", SearchOption.AllDirectories))
                {
                    Utils.iSetInfo("[PACKING]: " + Path.GetFileName(m_File));

                    var lpBuffer = File.ReadAllBytes(m_File);

                    var m_Entry = new DatEntry
                    {
                        m_FileName = Path.GetFileName(m_File),
                        dwOffset = (UInt32)TDatWriter.BaseStream.Position - dwBaseOffset,
                        dwSize = lpBuffer.Length,
                    };

                    m_EntryTable.Add(m_Entry);

                    TDatWriter.Write(lpBuffer);
                }

                TDatWriter.Seek(16, SeekOrigin.Begin);

                foreach (var m_Entry in m_EntryTable)
                {
                    Byte[] lpFileName = new Byte[32];
                    var lpTemp = Encoding.ASCII.GetBytes(m_Entry.m_FileName);

                    Array.Copy(lpTemp, lpFileName, lpTemp.Length);

                    TDatWriter.Write(lpFileName);
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    TDatWriter.Write(m_Entry.dwOffset);
                    TDatWriter.Write(m_Entry.dwSize);
                }

                TDatWriter.Dispose();
            }
        }
    }
}
