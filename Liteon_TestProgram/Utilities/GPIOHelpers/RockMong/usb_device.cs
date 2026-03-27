using System;
using System.Runtime.InteropServices;

namespace Rockmong
{
    class usb_device
    {
        //ɨ��USB�豸
        //����ֵ�������0�������ȡ���豸�ĸ������������0������δ�����豸�����С��0�������������
        [DllImport("librockmong.dll")]
        public static extern int UsbDevice_Scan(int[] SerialNumbers);
    }
}