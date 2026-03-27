using Liteon_TestProgram.Base;
using Liteon_TestProgram.Forms;
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

namespace Liteon_TestProgram.CaseProject
{
    internal class WCBN3602M_SA_WH_FT1: WCBN3602M_SA_FT1
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


        public WCBN3602M_SA_WH_FT1(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
            list_str.Add("1.初版");
            list_str.Add("2.增加Uart J3-J4测试(20250925)");
            list_str.Add("3.(2026-03-13)添加上传客户FW版本到IT数据库，便于后面在线列印");
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
            list_str.Add("1.COMA请填测试Mini TB008小板的串口\r\n");
            list_str.Add("2.测试Mini TB008小板: 1O 1F控制给板子上下电\r\n");
            list_str.Add("3.测试Mini TB008小板: 2O 2F控制给J3 J4短路\r\n");
            list_str.Add("4.测试电脑需要使用到无线网卡和蓝牙适配器.\r\n");
            list_str.Add("5.用到无线网卡名需要填到设备名称1中.\r\n");
            list_str.Add("6.(2025-12-22)添加iperf测试");

            return list_str;
        }





    }
}
