using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liteon_TestProgram.TestFunc.ICT
{
    internal class TestICTResult
    {
        public string Mac { get; set; }   //Mac
        public string Type { get; set; }  //测试的类型，是压降还是阻抗
        public string Value { get; set; }  //测试的数值
        public string Uint { get; set; }     //测试的值的单位
        public string Range { get; set; }     //测试规则
        public string Result { get; set; }     //测试的结果
    }
}
