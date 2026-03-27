using System;
using System.Runtime.InteropServices;

namespace Rockmong
{
    class io
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct IO_Init_TxStruct
        {
            public byte Pin;
            public byte Mode;
            public byte Pull;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IO_Init_RxStruct
        {
            public byte Ret;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IO_Read_TxStruct
        {
            public byte Pin;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IO_Read_RxStruct
        {
            public byte Ret;
            public byte PinState;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IO_Write_TxStruct
        {
            public byte Pin;
            public byte PinState;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IO_Write_RxStruct
        {
            public byte Ret;
        }

        //��ʼ�����Ź���ģʽ
        //SerialNumber: �豸���
        //Pin�����ű�š�0��P0. 1, P1...
        //Mode���������ģʽ��0�����롣1�������2����©
        //Pull�������������衣0���ޡ�1��ʹ���ڲ�������2��ʹ���ڲ�����
		//�������أ�0��������<0���쳣
        [DllImport("librockmong.dll")]
        public static extern int IO_InitPin(int SerialNumber, int Pin, int Mode, int Pull);

        //��ȡ����״̬
        //SerialNumber: �豸���
        //Pin�����ű�š�0��P0. 1, P1...
        //PinState����������״̬��0���͵�ƽ��1���ߵ�ƽ
		//�������أ�0��������<0���쳣
        [DllImport("librockmong.dll")]
        public static extern int IO_ReadPin(int SerialNumber, int Pin, ref int PinState);

        //�����������״̬
        //SerialNumber: �豸���
        //Pin�����ű�š�0��P0. 1, P1...
        //PinState������״̬��0���͵�ƽ��1���ߵ�ƽ
		//�������أ�0��������<0���쳣
        [DllImport("librockmong.dll")]
        public static extern int IO_WritePin(int SerialNumber, int Pin, int PinState);

         [DllImport("librockmong.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int IO_InitMultiPin(int SerialNumber, [In] IO_Init_TxStruct[] TxStruct, [Out] IO_Init_RxStruct[] RxStruct, int Number);

         [DllImport("librockmong.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int IO_ReadMultiPin(int SerialNumber, [In] IO_Read_TxStruct[] TxStruct, [Out] IO_Read_RxStruct[] RxStruct, int Number);

         [DllImport("librockmong.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int IO_WriteMultiPin(int SerialNumber, [In] IO_Write_TxStruct[] TxStruct, [Out] IO_Write_RxStruct[] RxStruct, int Number);
    }
}