using Liteon_TestProgram.Utilities;
using System.Diagnostics;
using System.Text;
using Liteon_TestProgram.TestFunc.Litepoint.LogDataCollection_V2;
using static Liteon_TestProgram.Base.Class_Variable;
using Liteon_TestProgram.Utilities.HttpClient;
using ScottPlot.Statistics;
using Liteon_TestProgram.Utilities.WIFIHelper;
using Liteon_TestProgram.Utilities.IOHelpers;
using ScottPlot.Colormaps;
using ScottPlot;
using SimpleWifi;
using System.Web;
using Liteon_TestProgram.Utilities.IOHelpers.Extensions;
using OpenTK.Audio.OpenAL;
using Liteon_TestProgram.Utilities.PEM;
using Liteon_TestProgram.Forms;


namespace Liteon_TestProgram.CaseProject
{
    internal class WN3602M_3V3_Plus_SA_HOC_LabelCheck : WN3602M_3V3_Plus_SA_HOC_F_LabelCheck
    {
        /// <summary>
        /// 用于添加MAC的前缀，比如“23S”，默认是空
        /// </summary>
        protected override string str_MacPrefix { get; set; } = "";


        /// <summary>
        /// 用于添加MAC的后缀，比如“A”，默认是空
        /// </summary>
        protected override string str_MacSuffix { get; set; } = "A";

        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================


        public WN3602M_3V3_Plus_SA_HOC_LabelCheck(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
         : base(mf, tf, pc, str_CaseProject)
        {
            struct_TestVariable.bNeedCreateSFCFile = true;
            _CaseProjectName = str_CaseProjectName = str_CaseProject == GetType().Name ? str_CaseProject : GetType().Name;
            _testForm = tf;

            Type sfcFileType = typeof(SfcFile); // 如果你已经有了SfcFile的实例，可以用instance.GetType()        
            struct_TestVariable.str_LogDataCollectionType = sfcFileType.FullName; // 使用反射获取类型的完整名称（包括命名空间）

            List<string> tipslist_str = new List<string>();
            tipslist_str = CreateTestStringList();
            string[] array_tipsstr = tipslist_str.ToArray();
            ShowTipsMessage(str_CaseProjectName + "测试提要", array_tipsstr);

            List<string> changelist_str = new List<string>();
            changelist_str = CreateChangeStringList();
            string[] array_changestr = changelist_str.ToArray();
            ShowChangeListMessage($"  <{str_CaseProjectName} 变更记录>", array_changestr);
        }


        /// <summary>
        /// 机种变更记录,会显示到UI标签页上
        /// </summary>
        /// <returns></returns>
        public override List<string> CreateChangeStringList()
        {
            List<string> list_str = new List<string>();

            #region V0.0.0.1

            list_str.Add("V0.0.0.1");
            list_str.Add("1.修改电源供电方式为直流稳压电源，并程控.");

            #endregion

            return list_str;
        }


        /// <summary>
        /// 测试须知填写处,会显示到测试UI上
        /// </summary>
        /// <returns></returns>
        public override List<string> CreateTestStringList()
        {
            List<string> list_str = new List<string>();
            list_str.Add("1.此程式用于检查带线组壳的SA label check\r\n");
            list_str.Add("2.测试配置页中VISA名称1中选GWINSTEK_PSS2303\r\n");
            list_str.Add("3.测试配置页中VISA地址1中填写对应的VISA地址资源名称\r\n");
            list_str.Add("4.用到无线网卡名需要填到测试配置设定中的设备名称1中\r\n");
            list_str.Add("5.先插线，然后依次扫码, 点击测试\r\n");
            return list_str;
        }


        public override void Func_TestMac()
        {

            macBD_Relation = MacBD_Relation.Separate;
            sn_Relation = SN_Relation.SN;

            if (struct_NormalINI.stru_b_DebugMode)
            {
                struct_TestVariable.iMAC_OriginalLength = 13;
                struct_TestVariable.iMAC_ExtractStartPosition = 0;
                struct_TestVariable.iMACLength = 12;

                struct_TestVariable.iBD_OriginalLength = 13;
                struct_TestVariable.iBD_ExtractStartPosition = 1;
                struct_TestVariable.iBDLength = 12;

                struct_TestVariable.iSN_OriginalLength = 27;
                struct_TestVariable.iSN_ExtractStartPosition = 0;
                struct_TestVariable.iSNLength = 26;

            }
            else
            {
                struct_TestVariable.iMACLength = 12;
                struct_TestVariable.iBDLength = 12;
                struct_TestVariable.iSNLength = 26;
            }
        }



    }
}
