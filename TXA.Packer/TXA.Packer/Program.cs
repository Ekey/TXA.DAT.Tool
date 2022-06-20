using System;

namespace TXA.Packer
{
    class Program
    {
        private static String m_Title = "Taisho x Alice DAT Packer";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2022 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    TXA.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Destination DAT file");
                Console.WriteLine("    m_Directory - Source directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    TXA.Packer E:\\Games\\Taisho x Alice\\bgm_new.dat D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_File = args[0];
            String m_Input = Utils.iCheckArgumentsPath(args[1]);

            DatPack.iDoIt(m_File, m_Input);
        }
    }
}
