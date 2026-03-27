using System;
using System.Runtime.InteropServices;

internal class SFC_CommFunc
{
    [DllImport("Sajet_CheckSFCI.dll")]
    public static extern int Check_SFCS(string Str);  //4;mac;mo

    [DllImport("Sajet_CheckSFCI.dll")]
    public static extern int Sajet_Destroy();

    [DllImport("Sajet_CheckSFCI.dll")]
    public static extern int Sajet_Initial(string str_Station, string str_IP);

    //[DllImport("SFC.dll")]
    //public static extern void SendToSFC();
}

