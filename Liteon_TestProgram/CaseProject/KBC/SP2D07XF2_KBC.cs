using Liteon_TestProgram.Base;
using Liteon_TestProgram.Forms;
using Liteon_TestProgram.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using static Liteon_TestProgram.Forms.TestForm;
using Liteon_TestProgram.Utilities.IOHelpers.Extensions;
using Liteon_TestProgram.InstrumentControl;
using System.IO;
using Liteon_TestProgram.Utilities.IOHelpers;
using System.Reflection;
using FluentFTP.Helpers;
using System.Text.RegularExpressions;
using Liteon_TestProgram.TestFunc.ICT.LogDataCollection_V1;
using static Liteon_TestProgram.Base.Class_Variable;
using Renci.SshNet.Sftp;
using Liteon_TestProgram.Save_LogFile;
using ScottPlot.Colormaps;
using static SkiaSharp.SKImageFilter;


namespace Liteon_TestProgram.CaseProject
{
    internal class SP2D07XF2_KBC : SP2D07XF0_KBC
    {


        public override string str_ReadKBCVerExeName { get; set; } = "ReadKBCFW(555-000826-01r2).exe";
        public override string str_ReadKBCVerExeFolderName { get; set; } = "ReadKBCFW(555-000826-01r2)"; // 子类中重新赋值

        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================




        public SP2D07XF2_KBC(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
            list_str.Add("2.增加自动导入ICT Flow功能");
            list_str.Add("3.增加产品多版本检查功能");

            #endregion

            return list_str;
        }

       



    }
}
