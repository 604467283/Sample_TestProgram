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
    internal class WCBN3602M_MB_FT2 : WCBN3602M_FT2
    {




        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================


        public WCBN3602M_MB_FT2(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
            list_str.Add("1.目前测试FW仅支持GD烧录IC");
            list_str.Add("2.(2025-04-25)变更客户FW: NIUV_v1.0.0Vrc1_A29040201A-S00104401A_supplier");
            list_str.Add("3.(2025-05-20)变更客户FW: NIUV_v1.0.0Vrc2_A29040201A-S00104401B_supplier");
            list_str.Add("4.(2025-07-14)变更客户FW: NIUV_v1.0.0Vrc3_full_flash_support_no_secureboot");
            list_str.Add("5.(2025-08-12)变更客户FW: NIUV_v1.0.0V_A29040201A-S00104402A_supplier");
            list_str.Add("6.(2025-11-07)将原先的RF测试和客户FW功能测试合站");
            list_str.Add("7.(2025-12-22)添加iperf测试");

            #endregion

            return list_str;
        }






    }
}
