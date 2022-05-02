using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeChangeMonitor
{
    class Core
    {
        public struct SectionInfo
        {
            public string sectionName;
            public UInt32 sectionSize;
            public UInt32 RVirtualAddress;
            public string sectionDiscributions;
        }
        public struct CodeInfo
        {
            public Int64 address;
            public string codeChange;
        }
        private const uint IMAGE_SCN_CNT_CODE = 0x00000020;
        private const uint IMAGE_SCN_CNT_INITIALIZED_DATA = 0x00000040;
        private const uint IMAGE_SCN_CNT_UNINITIALIZED_DATA = 0x00000080;
        private const uint IMAGE_SCN_MEM_DISCARDABLE = 0x02000000;
        private const uint IMAGE_SCN_MEM_NOT_CACHED = 0x04000000;
        private const uint IMAGE_SCN_MEM_NOT_PAGED = 0x08000000;
        private const uint IMAGE_SCN_MEM_SHARED = 0x10000000;
        private const uint IMAGE_SCN_MEM_EXECUTE = 0x20000000;
        private const uint IMAGE_SCN_MEM_READ = 0x40000000;
        private const uint IMAGE_SCN_MEM_WRITE = 0x80000000;

        public static SectionInfo[] GetSectionInfos(Process p)
        {
            byte[] buffer = new byte[0x800];
            buffer = Memory.ReadBytes(p.Id, p.MainModule.BaseAddress.ToInt64(), 0x800);
            if (buffer[0] == 0x4D && buffer[1] == 0x5A)//MZ
            {
                int NtHeaderOffset = BitConverter.ToInt32(buffer, 0x3C);//At location 0x3c, the stub has the file offset to the PE signature.
                int FileHeaderOffset = NtHeaderOffset + 0x4;//This signature is "PE\0\0",so+0x4
                if (BitConverter.ToUInt16(buffer, FileHeaderOffset)/*This is Machine Types*/!= 0x8664/* 0x8664 mean that the trage is a x64 processes*/)
                {
                    throw new Exception("非x64进程无法使用");
                }
                int numberOfSections = BitConverter.ToUInt16(buffer, FileHeaderOffset + 0x2);//Machine Type is only take 2 bytes length.
                SectionInfo[] retInfo = new SectionInfo[numberOfSections];
                int firstSectionsOffset = FileHeaderOffset + 0x14/*2 2 4 4 4 2 2 = 20 = 0x14*/+ 0xF0/*Optional Header size*/;
                for (int i = 0; i < numberOfSections; i++)
                {
                    retInfo[i].sectionName = Encoding.ASCII.GetString(buffer, firstSectionsOffset + i * 0x28, 8).TrimEnd('\0');
                    retInfo[i].sectionSize = BitConverter.ToUInt32(buffer, firstSectionsOffset + (i * 0x28) + 0x8);//VirtualSize 4bytes
                    retInfo[i].RVirtualAddress = BitConverter.ToUInt32(buffer, firstSectionsOffset + (i * 0x28) + 0xC);//VirtualAddress 4bytes
                    UInt32 tempCharacteristics = BitConverter.ToUInt32(buffer, firstSectionsOffset + (i * 0x28) + 0x24);//Characteristics
                    #region CharacteristicsCMP
                    if ((tempCharacteristics & IMAGE_SCN_CNT_CODE) == IMAGE_SCN_CNT_CODE)
                    {
                        retInfo[i].sectionDiscributions += "此节包含代码\r\n";
                    }
                    if ((tempCharacteristics & IMAGE_SCN_CNT_INITIALIZED_DATA) == IMAGE_SCN_CNT_INITIALIZED_DATA)
                    {
                        retInfo[i].sectionDiscributions += "此节包含初始化数据\r\n";
                    }
                    if ((tempCharacteristics & IMAGE_SCN_CNT_UNINITIALIZED_DATA) == IMAGE_SCN_CNT_UNINITIALIZED_DATA)
                    {
                        retInfo[i].sectionDiscributions += "此节包含未初始化数据\r\n";
                    }
                    if ((tempCharacteristics & IMAGE_SCN_MEM_DISCARDABLE) == IMAGE_SCN_MEM_DISCARDABLE)
                    {
                        retInfo[i].sectionDiscributions += "可以根据需要丢弃该部分\r\n";
                    }
                    if ((tempCharacteristics & IMAGE_SCN_MEM_NOT_CACHED) == IMAGE_SCN_MEM_NOT_CACHED)
                    {
                        retInfo[i].sectionDiscributions += "无法缓存此节\r\n";
                    }
                    if ((tempCharacteristics & IMAGE_SCN_MEM_NOT_PAGED) == IMAGE_SCN_MEM_NOT_PAGED)
                    {
                        retInfo[i].sectionDiscributions += "此节不可分页\r\n";
                    }
                    if ((tempCharacteristics & IMAGE_SCN_MEM_SHARED) == IMAGE_SCN_MEM_SHARED)
                    {
                        retInfo[i].sectionDiscributions += "此节的可以被分享在内存中\r\n";
                    }
                    if ((tempCharacteristics & IMAGE_SCN_MEM_EXECUTE) == IMAGE_SCN_MEM_EXECUTE)
                    {
                        retInfo[i].sectionDiscributions += "此节可作为代码执行\r\n";
                    }
                    if ((tempCharacteristics & IMAGE_SCN_MEM_READ) == IMAGE_SCN_MEM_READ)
                    {
                        retInfo[i].sectionDiscributions += "此节可以被读\r\n";
                    }
                    if ((tempCharacteristics & IMAGE_SCN_MEM_WRITE) == IMAGE_SCN_MEM_WRITE)
                    {
                        retInfo[i].sectionDiscributions += "此节可以被写\r\n";
                    }
                    #endregion
                }
                return retInfo;
            }
            else
            {
                throw new Exception("未找到MZ头");
            }
        }
        public static void GetOrgBytes(Process p, SectionInfo[] sectionInfos)
        {
            using (FileStream fs = new FileStream(Application.StartupPath + "\\org.file", FileMode.OpenOrCreate))
            {
                foreach (SectionInfo info in sectionInfos)
                {
                    byte[] buffer = new byte[info.sectionSize];
                    buffer = Memory.ReadBytes(p.Id, info.RVirtualAddress + p.MainModule.BaseAddress.ToInt64(), (int)info.sectionSize);
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Flush();
                }

            }

        }
        public static CodeInfo[] GetChange(Process p, SectionInfo[] sectionInfos)
        {
            List<CodeInfo> retInfo = new List<CodeInfo>();
            using (FileStream fs = new FileStream(Application.StartupPath + "\\org.file", FileMode.Open))
            {

                foreach (SectionInfo info in sectionInfos)
                {
                    byte[] buffer = new byte[info.sectionSize];
                    buffer = Memory.ReadBytes(p.Id, info.RVirtualAddress + p.MainModule.BaseAddress.ToInt64(), (int)info.sectionSize);
                    for (int i = 0; i < (int)info.sectionSize; i++)
                    {
                        byte readByte = BitConverter.GetBytes(fs.ReadByte())[0];
                        if (buffer[i] != readByte)
                        {
                            retInfo.Add(new CodeInfo
                            {
                                address = p.MainModule.BaseAddress.ToInt64() + info.RVirtualAddress + i,
                                codeChange = $"由0x{Convert.ToString(readByte, 16)}变为了0x{Convert.ToString(buffer[i], 16)}\r"
                            });
                        }
                    }
                }

            }
            return retInfo.ToArray();
        }
    }
}
