
namespace CodeChangeMonitor
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.checkSectionsBox = new System.Windows.Forms.CheckedListBox();
            this.logBox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(428, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 31);
            this.button1.TabIndex = 0;
            this.button1.Text = "获取节段信息";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkSectionsBox
            // 
            this.checkSectionsBox.FormattingEnabled = true;
            this.checkSectionsBox.HorizontalScrollbar = true;
            this.checkSectionsBox.Location = new System.Drawing.Point(428, 69);
            this.checkSectionsBox.Name = "checkSectionsBox";
            this.checkSectionsBox.ScrollAlwaysVisible = true;
            this.checkSectionsBox.Size = new System.Drawing.Size(211, 224);
            this.checkSectionsBox.TabIndex = 2;
            this.checkSectionsBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkSectionsBox_ItemCheck);
            // 
            // logBox
            // 
            this.logBox.AcceptsReturn = true;
            this.logBox.AcceptsTab = true;
            this.logBox.Location = new System.Drawing.Point(22, 69);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logBox.Size = new System.Drawing.Size(400, 224);
            this.logBox.TabIndex = 3;
            this.logBox.WordWrap = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(428, 299);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 28);
            this.button2.TabIndex = 4;
            this.button2.Text = "创建对比源";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(542, 299);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(97, 28);
            this.button3.TabIndex = 5;
            this.button3.Text = "对比";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(62, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(360, 25);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "请输入程序名字（不含.exe)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 357);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.checkSectionsBox);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckedListBox checkSectionsBox;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
    }
}

