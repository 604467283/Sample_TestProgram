using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Liteon_TestProgram.Utilities
{
    internal class StringTool
    {

        public static string RemoveColorEscapeSequences(string input)
            => Regex.Replace(input, @"\u001b\[\d{1,2}(;\d{1,2})?m", string.Empty);




        public static string Int64ToMacAddress(long macAddress)
        {
            string _macAddress = Convert.ToString(macAddress, 16);

            if (_macAddress.Length == 12)
            {
                return _macAddress;
            }

            int zeroNum = 12 - _macAddress.Length;

            for (int i = 0; i < zeroNum; i++)
            {
                _macAddress = _macAddress.Insert(0, "0");
            }

            return _macAddress;
        }

        public static string[] SplitStringIntoChunks(string input, int chunkSize)
        {
            int numOfChunks = (int)Math.Ceiling((double)input.Length / chunkSize);
            string[] chunks = new string[numOfChunks];
            for (int i = 0; i < numOfChunks; i++)
            {
                int startIndex = i * chunkSize;
                int length = Math.Min(chunkSize, input.Length - startIndex);
                chunks[i] = input.Substring(startIndex, length);
            }
            return chunks;
        }

        public static string FormattedMacAddress(string macAddress)
        {
            string formattedMacAddress = string.Join(":", SplitStringIntoChunks(macAddress, 2));
            return formattedMacAddress;
        }


        /// <summary>
        /// return a bool value that the Mac Address entered is Vaild or Not
        /// 判断字符串是否为合法的Mac地址(Not Formatted)
        /// </summary>
        /// <param name="_Mac"></param>
        /// <returns></returns>
        public static bool IsMacAddress_2(string _Mac)
        {
            if (_Mac.Length != 12)
                return false;
            return Regex.IsMatch(_Mac, "[A-Fa-f0-9]{12}");
        }


        public static string GetNextMacAddr(string MacAddr)
        {
            long MyMacData = Convert.ToInt64(MacAddr, 16);
            MyMacData++;
            string TempStr = Convert.ToString(MyMacData, 16);
            if (TempStr.Length == 12)
            {
                return TempStr.ToUpper();
            }
            else
            {
                int PutZeroNum = 12 - TempStr.Length;
                string ret = "";
                for (int n = 0; n < PutZeroNum; n++)
                    ret += "0";
                ret += TempStr;
                return ret.ToUpper();
            }
        }

        public static bool XorChecksum(string content, byte checksum)
        {
            return checksum == GetXorChecksum(content);
        }

        public static byte GetXorChecksum(string ascii)
        {
            if (ascii.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ascii));
            }

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(ascii);

            byte checksum = 0;
            foreach (byte b in bytes)
            {
                checksum ^= b;
            }
            return checksum;
        }
    }
}
