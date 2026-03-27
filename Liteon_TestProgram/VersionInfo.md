V01.00.04   202512051502
1:加密档案中增加检查本exe的md5值功能，使其执行档exe和加密ini绑定；

V01.00.03   202507241006
1:添加机种变更记录窗口，方便查看机种变更记录；
2.添加了ICT自动导入Flow的功能;


V01.00.02   202506111452
1:UIHandleHelper.ShowRunLog添计时功能，当ShowRunLog中参数ShowGridView: true，会计算和上一个ShowGridView: true的间隔时间，并显示在界面中；
2:添加WCBN3602M_FT1_Sample程式 V0.0.0.1  20250611
3:添加OQC_Base基类，WCBN813A_N5 WCBN814A_S6继承这个基类，这个基类主要实现一些通用的功能，如：iperf3传输和蓝牙的播放音乐和传输图片，以及Mes的上传；
4:添加ICT基类Base_ICT，主要实现通用的ICT功能，可以直接继承来实现多个机种的ICT测试；
5:实现机种的手动测试条码截取功能，可以重写虚函数Func_TestMac实现；
6:将网卡的操作写成DevconHelper类，方便调用；使用方法参考<开发示例.md>
7:添加WCBN814A_BA_ICT测试； V0.0.0.1  20250613
8:OQC基类添加虚属性str_MacPrefix，方便子类调用进行MAC前缀添加，比如23S；


V01.00.01   V0.0.0.1  202505151401  
1: 添加WCBN3534A_Fruitiness自动测试，功能主要为控制IQXel获取产品的Power


