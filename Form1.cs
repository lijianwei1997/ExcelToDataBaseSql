using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Threading;

namespace 连接数据库
{


    public partial class Form1 : Form
    {  //Provider=MySQLProv;Data Source=mydb;User Id=myUsername;Password=myPassword;
        //Data Source=WIN-20160915CDO\SQLEXPRESS;User ID=sa
        public static string mysql;//= "Database=mysql;Data Source=localhost;User Id=root;Password=rootroot;pooling=false;CharSet=utf8;port=3306";
        //allowbatch=True;server=root;user id=root;persistsecurityinfo=True;database=ajksdfjk
        public static string str;
        //= "Data Source=.\\SQLEXPRESS;User ID=sa;Password=123456;Database=master";
        List<User> list = new List<User>();
        List<User> listSuccess = new List<User>();
        List<User> listDefault = new List<User>();
    



        public Form1()
        {
            InitializeComponent();

        }



        private void Export_Click(object sender, EventArgs e)
        {
            if (list.Count <= 0)
            {
                MessageBox.Show("请导入数据");

            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "文本文件|*.txt";
                sfd.ShowDialog();
                string path = sfd.FileName;
                try
                {
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        string str = null;
                        for (int i = 0; i < list.Count; i++)
                        {

                            str = (list[i].Ip + "--" + list[i].USer + "--" + list[i].Password).ToString();
                            sw.WriteLine(str);
                        }
                    }
                    MessageBox.Show("保存成功");
                }

                catch
                { }
            }

        } //导出

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;

        }//窗体加载

        private bool CheckMySql(string mysql)
        {
            MySqlConnection ole = new MySqlConnection(mysql);
            try { ole.Open(); }
            catch { }
            if (ole.State == ConnectionState.Open)
            {
                return true;
            }
            else
            { return false; }
        }//检查mysql数据库连接状态

        private bool CheckSqlServer(string str)
        {
            SqlConnection ole = new SqlConnection(str);
            try { ole.Open(); }
            catch { }
            if (ole.State == ConnectionState.Open)
            {
                return true;

            }
            else
            {   return false; }

        }//检查SQLserver 数据库连接状态

        private void Import_Click(object sender, EventArgs e)//导入
        {
            list.Clear();
            int s = 0;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "文本文件|*.txt";
            ofd.InitialDirectory = @"D:\";
            ofd.ShowDialog();
            path.Text = ofd.FileName;//选中文件名

            try
            {
                using (StreamReader sr = new StreamReader(ofd.FileName))//数据读取
                {   
                    string[] cs = { "--", "——", "－" };
                    string str;
                    while (!sr.EndOfStream)
                    {
                        User us = new User();
                        str = sr.ReadLine();
                        string[] stu = str.Split(cs, StringSplitOptions.RemoveEmptyEntries);
                        us.Ip = stu[0].ToString();
                        us.USer = stu[1].ToString();
                        us.Password = stu[2].ToString();
                        list.Add(us);
                        DGview.Rows.Add((s+1).ToString(), us.Ip, us.USer, us.Password);
                        s++;
                        
                    }
                    MessageBox.Show("导入成功");
                    
                }
            }
            catch
            { }
        }

        private void CheckConnection_Click(object sender, EventArgs e)
        {

            //清空数据行
            //CheckPoint();
            //if (cbID.Text == "SQL数据库--1433端口")
            //{
            //    CheckSqlServer();
            //}
            //else if (cbID.Text == "MySql数据库--3306端口")
            //{
          
            //    CheckMySql();
            //}
            if (cbID.Text == "")
            {
                MessageBox.Show("请选择默认端口");
            }
            else {
                DGview.Rows.Clear();
                Thread th = new Thread(Database);
                th.IsBackground = true;
            th.Start();
            }



        }//测试连接

        private void Database()
        {
            int s = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (cbID.Text == "SQL数据库--1433端口")
                {
                    str = "Data Source=" + list[i].Ip + "\\SQLEXPRESS;User ID=" + list[i].USer + ";Password=" + list[i].Password + ";Database=master";
                    if (CheckSqlServer(str))
                    {
                        DGview.Rows.Add((s + 1).ToString(), list[i].Ip, list[i].USer, list[i].Password, "连接成功");
                        listSuccess.Add(list[i]);
                        s++;
                    }
                    else
                    {
                        DGview.Rows.Add((s + 1).ToString(), list[i].Ip, list[i].USer, list[i].Password, "连接失败");
                        listDefault.Add(list[i]);
                        s++;
                    }
                }
                else if (cbID.Text == "MySql数据库--3306端口")
                {
                    string h;
                    str = "Database=mysql;Data Source=" + list[i].Ip + ";User Id=" + list[i].USer + ";Password=" + list[i].Password + ";pooling=false;CharSet=utf8;port=" + (h = TBPort.Text == "" ? (3306).ToString() : TBPort.Text.ToString());
                    if (CheckMySql(str))
                    {
                        DGview.Rows.Add((s + 1).ToString(), list[i].Ip, list[i].USer, list[i].Password, "连接成功");
                        listSuccess.Add(list[i]);
                        s++;
                    }
                    else
                    {
                        DGview.Rows.Add((s + 1).ToString(), list[i].Ip, list[i].USer, list[i].Password, "连接失败");
                        listDefault.Add(list[i]);
                        s++;
                    }


                }



            }
        }

        private void CheckPoint()
        {
           
            if (cbID.Text == "")
            {
                MessageBox.Show("请选择连接数据库的类型");
            }
            else if (cbID.Text == "SQL数据库--1433端口")
            {
                if (TBPort.Text == "")
                {
                    str = "Data Source=.\\SQLEXPRESS;User ID=sa;Password=123456;Database=master";
                }
            }
            else if (cbID.Text == "MySql数据库--3306端口")
            {
                mysql = "Database = mysql; Data Source = localhost; User Id = root; Password = rootroot; pooling = false; CharSet = utf8; port = 3306";
            }
            else
            {
                MessageBox.Show("没有这个值，只能从中连接一个");
            }
        //检查本地默认连接端口
     }



        private void BuliderConnectionstrings()
        {



        }
    }
}
