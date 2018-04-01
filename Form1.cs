using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

using ICSharpCode.SharpZipLib.Zip;
using System.Text.RegularExpressions;
namespace McMySkin
{

    public partial class Form1 : Form
    {

        public class Win32
        {
            public const Int32 AW_HOR_POSITIVE = 0x00000001;    // 从左到右打开窗口  
            public const Int32 AW_HOR_NEGATIVE = 0x00000002;    // 从右到左打开窗口  
            public const Int32 AW_VER_POSITIVE = 0x00000004;    // 从上到下打开窗口  
            public const Int32 AW_VER_NEGATIVE = 0x00000008;    // 从下到上打开窗口  
            public const Int32 AW_CENTER = 0x00000010;
            public const Int32 AW_HIDE = 0x00010000;        // 在窗体卸载时若想使用本函数就得加上此常量  
            public const Int32 AW_ACTIVATE = 0x00020000;    //在窗体通过本函数打开后，默认情况下会失去焦点，除非加上本常量  
            public const Int32 AW_SLIDE = 0x00040000;
            public const Int32 AW_BLEND = 0x00080000;       // 淡入淡出效果  
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool AnimateWindow(
            IntPtr hwnd, // handle to window      
            int dwTime, // duration of animation      
            int dwFlags // animation type      
            );
        }

            private const int WM_NCHITTEST = 0x84;
 	        private const int HTCLIENT = 0x1;
 	        private const int HTCAPTION = 0x2;
	        private const int WM_NCLBUTTONDBLCLK = 0xA3;//鼠标双击标题栏消息
	 
	        protected override void WndProc(ref Message m)
 	        {
 	            switch (m.Msg)
	            {
 	                case WM_NCHITTEST:
	                    base.WndProc(ref m);
	                    if ((int)m.Result == HTCLIENT)
	                        m.Result = (IntPtr)HTCAPTION;
 	                    return;
 	               case WM_NCLBUTTONDBLCLK:      
	                        break;
	               default:
 	                        base.WndProc(ref m);
	                        break;
 	            }
           
        }


        #region 窗体边框阴影效果变量申明

        const int CS_DropSHADOW = 0x20000;
        const int GCL_STYLE = (-26);
        //声明Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);

        #endregion

        public Form1()
        {
            InitializeComponent();
            SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW); //API函数加载，实现窗体边框阴影效果

             this.Height = 414;
            this.Width = 610;
            panel4.Top = 75;
            panel4.Left = 1;
            panel3.Top = 75;
            panel3.Left = 1;
            panel2.Top = 75;
            panel2.Left = 1;
            panel1.Top = 75;
            panel1.Left = 1;

            panel4.Visible = false;
            panel3.Visible = false;
            panel2.Visible = false;
            panel1.Visible = true;
            button4.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            Win32.AnimateWindow(this.Handle, 300, Win32.AW_BLEND);

            checkversion.RunWorkerAsync();

            if (File.Exists(Application.StartupPath + "\\.minecraft\\bin\\minecraft.jar") == true)
            {
                textBox1.Text = Application.StartupPath + "\\.minecraft";
            }
            else if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\") == true)
            {
                textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\";
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dia = MessageBox.Show("您真的要退出McMySkin MOD安装程序？", " ", MessageBoxButtons.OKCancel);

            if (dia == DialogResult.OK)
            {
                Win32.AnimateWindow(this.Handle, 300, Win32.AW_SLIDE | Win32.AW_HIDE | Win32.AW_BLEND);
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            folderBrowserDialog1.SelectedPath = "C:";   // 设置当前选择的路径
            folderBrowserDialog1.ShowNewFolderButton = false;   // 允许在对话框中包括一个新建目录的按钮
            folderBrowserDialog1.Description = "请选择.minecraft的目录";   // 设置对话ww框的说明信息

            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
        {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel3.Visible = false;
            panel2.Visible = false;
            panel1.Visible = true;;
            button4.Enabled = false;
            step.Text = "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
                #region 步骤执行代码
                if (step.Text == "1")
                {
                    if (File.Exists(textBox1.Text + "\\bin\\minecraft.jar") == true)
                    {
                        panel1.Visible = false;
                        panel2.Visible = true;
                        panel4.Visible = false;
                        button4.Enabled = true;
                        step.Text = "2";
                    }
                    else
                    {
                        MessageBox.Show("请输入一个合法并且存在的Minecraft文件目录，因为新版已更改为.minecraft文件夹");
                    }

                }
                else if (step.Text == "2")
                {
                    if (checkBox1.Checked == true)
                    {
                        if (comboBox1.SelectedIndex == -1)
                        {
                            MessageBox.Show("请选择一个Minecraft版本");
                            return;
                        }

                        panel1.Visible = false;
                        panel2.Visible = false;
                        panel3.Visible = true;
                        panel4.Visible = false;
                        button4.Enabled = false;
                        button2.Enabled = false;
                        step.Text = "3";
                        backgroundWorker1.RunWorkerAsync(comboBox1.Items[comboBox1.SelectedIndex]);
                    }
                    else
                    {
                        panel1.Visible = false;
                        panel2.Visible = false;
                        panel3.Visible = true;
                        panel4.Visible = false;
                        button4.Enabled = false;
                        button2.Enabled = false;
                        step.Text = "3";
                        backgroundWorker1.RunWorkerAsync();
                    }

                        
                }
                else if (step.Text == "4")
                    Application.Exit();
                #endregion

        }
        private byte[] FileContent(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            try
            {
                byte[] buffur = new byte[fs.Length];
                fs.Read(buffur, 0, (int)fs.Length);

                return buffur;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (fs != null)
                {

                    //关闭资源   
                    fs.Close();
                }
            }
        }

        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        private static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        //////////安装代码//////////
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ////////////////原版MC皮肤Hex定义/////////////////////////////////
            string Skinbr =   "2A687474703A2F2F736B696E732E6D696E6563726166742E6E65742F4D696E656372616674536B696E732F"; //原版皮肤Hex
            string Cloakbr = "2B687474703A2F2F736B696E732E6D696E6563726166742E6E65742F4D696E656372616674436C6F616B732F"; //原版披风Hex
            /////////////////////////////////////////////////////////////////////
            
            backgroundWorker1.ReportProgress(10); //更新进度：开始

            FastZip FZ = new FastZip();
            System.Net.WebClient WC = new System.Net.WebClient();  //两行为声明

            string JarFile = textBox1.Text + "\\bin\\minecraft.jar"; //定义MC JAR文件路径
            string tempFileName = System.IO.Path.GetTempPath(); //获取临时文件目录
            string tempWorkPath = tempFileName + "mms\\"; //定义临时工作路径
            string newPath = tempWorkPath;  //定义修改后的class存放路径默认为临时工作路径
            string Skinar, Cloakar; //定义MMS的皮肤披肩Hex

            File.Copy(JarFile, textBox1.Text + "\\bin\\minecraft_备份.jar", true); //进行备份
            if (Directory.Exists(tempWorkPath) == true) Directory.Delete(tempWorkPath, true);   //清理以前的临时文件

            backgroundWorker1.ReportProgress(30); //更新进度/////////////////////////////////////////
            FZ.ExtractZip(JarFile, tempWorkPath,"");  //解压Minecraft
            backgroundWorker1.ReportProgress(40); //更新进度/////////////////////////////////////////

            try //获取网络Hex开始
            {
                Skinar = WC.DownloadString("http://mcmyskin.com/mms_install/appinfo/skin");
                Cloakar = WC.DownloadString("http://mcmyskin.com/mms_install/appinfo/cloak");
            }
            catch
            {
                Skinar =    "23687474703a2f2f6d636d79736b696e2e636f6d2f736b696e732e7068703f6e616d653d"; //MMS皮肤
                Cloakar = "28687474703a2f2f6d636d79736b696e2e636f6d2f736b696e732e7068703f63617065266e616d653d"; //MMS披风
            } //获取网络Hex结束

            backgroundWorker1.ReportProgress(70); //更新进度

            /////////////////////////////准备与解压过程结束，开始遍历////////////////////////////////////////
            DirectoryInfo TheFolder = new DirectoryInfo(tempWorkPath);
            foreach (FileInfo NextFile in TheFolder.GetFiles("*.class")) //开始遍历minecraft.jar中的文件
            {
                String ApplyFile = tempWorkPath + NextFile.Name; //定义需要修改的文件路径
                String ApplyFileSave = ""; //定义修改后的文件保存路径

                if (checkBox2.Checked == true) //如果用户选择了自行安装，则开始：
                {
                    Directory.CreateDirectory(textBox1.Text + "\\McMySkin\\"); //如果选择用户要自行安装，则创建所需要的目录
                    newPath = textBox1.Text + "\\McMySkin\\"; //修改生效后的目录为...
                    ApplyFileSave = newPath + NextFile.Name; //修改生效后的文件保存为...
                }
                else 
                {
                    ApplyFileSave = ApplyFile; //如果没有选择则默认
                }

                Byte[] BA1 = FileContent(ApplyFile); //定义遍历文件的字节数组
                string BAsed = ToHexString(BA1); //定义字节数组转换后的Hex

                if (BAsed.IndexOf(Skinbr) > -1) //检测是否为皮肤相关文件
                {
                    String ChangedHex = BAsed.Replace(Skinbr, Skinar); //修改皮肤
                    ChangedHex = ChangedHex.Replace(Cloakbr, Cloakar); //修改披风
                    //停止修改，开始保存
                    Byte[] BAced = HexToByte(ChangedHex); //将Hex转换为字节数组
                    File.WriteAllBytes(ApplyFileSave, BAced); //将字节数组写入遍历的文件
                }
            }

            backgroundWorker1.ReportProgress(95); //更新进度
            /////////////////////////////遍历结束，开始扫尾、打包////////////////////////////////////////

             if (checkBox1.Checked == true) //-----安装高清皮肤支持-----//
            {
                string choosever = e.Argument.ToString(); //获取传递过来的参数（用户选择的版本号）
                string verinfo = WC.DownloadString("http://mcmyskin.com/mms_install/HD/" + choosever + ".name");//读取对应版本号对应的class名称
                string HDSavePath = newPath + verinfo; //高清皮肤class保存路径

                if (File.Exists(HDSavePath) == true) 
                {
                    File.Delete(HDSavePath);//删除默认的皮肤渲染class
                }

                WC.DownloadFile("http://mcmyskin.com/mms_install/HD/" + choosever, HDSavePath); //下载高清皮肤class
            } 

            if (checkBox2.Checked == false)
            {
                try{Directory.Delete(tempWorkPath + "\\META-INF", true);}catch{ }
                backgroundWorker1.ReportProgress(98); //更新进度
                FZ.CreateZip(JarFile, tempWorkPath, true, "");
                backgroundWorker1.ReportProgress(100); //更新进度
            }

        } 

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                label14.Text = "安装过程中有错误";
                label12.Text = "该安装程序无法正确安装McMySkin至您的Mod，请重新执行安装程序并且选择一个正确的Minecraft目录重新安装。";
            }
            //结束代码
            step.Text = "4";
            button2.Text = "完成(&C)";
            button3.Visible = false;
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = true;
            button2.Enabled = true;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void checkversion_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                System.Net.WebClient WC = new System.Net.WebClient();
                string appnewversion = WC.DownloadString("http://mcmyskin.com/mms_install/appinfo/newest");
                if (appnewversion == Application.ProductVersion)
                {

                }
                else
                {
                    DialogResult dia = MessageBox.Show("检测到安装程序有新版本" + appnewversion + "，是否需要到达官方网站下载？", "更新", MessageBoxButtons.OKCancel);

                    if (dia == DialogResult.OK)
                    {
                        System.Diagnostics.Process.Start("http://mcmyskin.com/download.php");
                    }
                }

                
            }
            catch
            {

            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://mcmyskin.com");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                button2.Enabled = false;
                    comboBox1.Enabled = true;
                    comboBox1.Text = "获取中...";
                    getmcver.RunWorkerAsync();
            }
            else
            {
                button2.Enabled = true;
                comboBox1.Items.Clear();
                comboBox1.Enabled = false;
            }
        }

        private void getmcver_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Net.WebClient WC = new System.Net.WebClient();
            string mcverlist = WC.DownloadString("http://mcmyskin.com/mms_install/HD/mcver.txt");
            e.Result = mcverlist;
        }

        private void getmcver_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button2.Enabled = true;
            comboBox1.Text = "(请选择)";
            string mcverlist = e.Result.ToString();
            string[] sArray = mcverlist.Split('j');
            foreach (string i in sArray) comboBox1.Items.Add(i.ToString());
            //comboBox1.Text = "\\n" + mcverlist;
            comboBox1.Enabled = true;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            AboutBox1 About = new AboutBox1();
            About.ShowDialog();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe",@textBox1.Text);
        }



        }

}
