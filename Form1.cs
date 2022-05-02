using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
namespace CodeChangeMonitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Core.SectionInfo> showSectionInfos = new List<Core.SectionInfo>();
        private void button1_Click(object sender, EventArgs e)
        {
            checkSectionsBox.Items.Clear();
            Core.SectionInfo[] sectionInfos = Core.GetSectionInfos(Process.GetProcessesByName(textBox1.Text)[0]);
            foreach (var item in sectionInfos)
            {
                if (item.sectionDiscributions.Contains("此节包含代码") || item.sectionDiscributions.Contains("此节可作为代码执行"))
                {
                    checkSectionsBox.Items.Add(item.sectionName);
                    showSectionInfos.Add(item);
                }

            }
            logBox.Text += "在选择框中选中来查看节段信息\r\n";
        }
        private void checkSectionsBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Unchecked)
            {
                logBox.Text += $"{showSectionInfos[e.Index].sectionName}段:\r大小:{showSectionInfos[e.Index].sectionSize}\r{showSectionInfos[e.Index].sectionDiscributions}\r";
            }
            else if (e.CurrentValue == CheckState.Checked)
            {
                logBox.Text = logBox.Text.Replace($"{showSectionInfos[e.Index].sectionName}段:\r大小:{showSectionInfos[e.Index].sectionSize}\r{showSectionInfos[e.Index].sectionDiscributions}\r", "");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            logBox.Text = "开始创建比对源文件\r\n";
            Task.Run(() =>
            {
                Core.GetOrgBytes(Process.GetProcessesByName(textBox1.Text)[0], showSectionInfos.ToArray());
                logBox.BeginInvoke(new Action(() => { logBox.Text += "创建成功!\r\n"; }));
            });

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                logBox.BeginInvoke(new Action(() => { logBox.Text += $"开始比对!\r\n"; }));
                Core.CodeInfo[] a = Core.GetChange(Process.GetProcessesByName(textBox1.Text)[0], showSectionInfos.ToArray());
                foreach (var b in a)
                {
                    logBox.BeginInvoke(new Action(() => { logBox.Text += $"0x{Convert.ToString(b.address, 16)}处:\r\n {b.codeChange}\r\n"; }));
                }
                if (a.Length == 0)
                {
                    logBox.BeginInvoke(new Action(() => { logBox.Text += $"无修改!\r\n"; }));
                }
            });
        }
    }
}
