
namespace Reminder
{
    partial class FStart
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FStart));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.TsmItemConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmItemConfigRepository = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelIcon = new System.Windows.Forms.ToolStripStatusLabel();
            this.tSSLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlTopRight = new System.Windows.Forms.Panel();
            this.PicStatRepOfline = new System.Windows.Forms.PictureBox();
            this.PicStatRepOnline = new System.Windows.Forms.PictureBox();
            this.pnlFill = new System.Windows.Forms.Panel();
            this.TsmItemConfigProviderObj = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmItemConfigProviderMon = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmItemConfigParam = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlTopRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicStatRepOfline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicStatRepOnline)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmItemConfig});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(628, 36);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // TsmItemConfig
            // 
            this.TsmItemConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmItemConfigRepository,
            this.TsmItemConfigProviderObj,
            this.TsmItemConfigProviderMon,
            this.TsmItemConfigParam});
            this.TsmItemConfig.Name = "TsmItemConfig";
            this.TsmItemConfig.Size = new System.Drawing.Size(97, 32);
            this.TsmItemConfig.Text = "Настройка";
            // 
            // TsmItemConfigRepository
            // 
            this.TsmItemConfigRepository.Name = "TsmItemConfigRepository";
            this.TsmItemConfigRepository.Size = new System.Drawing.Size(392, 26);
            this.TsmItemConfigRepository.Text = "Настройка репозитория";
            this.TsmItemConfigRepository.Click += new System.EventHandler(this.TsmItemConfigRepository_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelIcon,
            this.tSSLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 526);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(800, 26);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabelIcon
            // 
            this.toolStripStatusLabelIcon.Name = "toolStripStatusLabelIcon";
            this.toolStripStatusLabelIcon.Size = new System.Drawing.Size(13, 20);
            this.toolStripStatusLabelIcon.Text = " ";
            // 
            // tSSLabel
            // 
            this.tSSLabel.Name = "tSSLabel";
            this.tSSLabel.Size = new System.Drawing.Size(162, 20);
            this.tSSLabel.Text = "Загрузка приложения";
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.menuStrip1);
            this.pnlTop.Controls.Add(this.pnlTopRight);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(800, 36);
            this.pnlTop.TabIndex = 2;
            // 
            // pnlTopRight
            // 
            this.pnlTopRight.Controls.Add(this.PicStatRepOfline);
            this.pnlTopRight.Controls.Add(this.PicStatRepOnline);
            this.pnlTopRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlTopRight.Location = new System.Drawing.Point(628, 0);
            this.pnlTopRight.Name = "pnlTopRight";
            this.pnlTopRight.Size = new System.Drawing.Size(172, 36);
            this.pnlTopRight.TabIndex = 0;
            // 
            // PicStatRepOfline
            // 
            this.PicStatRepOfline.Dock = System.Windows.Forms.DockStyle.Left;
            this.PicStatRepOfline.Image = ((System.Drawing.Image)(resources.GetObject("PicStatRepOfline.Image")));
            this.PicStatRepOfline.Location = new System.Drawing.Point(44, 0);
            this.PicStatRepOfline.Name = "PicStatRepOfline";
            this.PicStatRepOfline.Size = new System.Drawing.Size(44, 36);
            this.PicStatRepOfline.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicStatRepOfline.TabIndex = 1;
            this.PicStatRepOfline.TabStop = false;
            this.PicStatRepOfline.Visible = false;
            // 
            // PicStatRepOnline
            // 
            this.PicStatRepOnline.Dock = System.Windows.Forms.DockStyle.Left;
            this.PicStatRepOnline.Image = ((System.Drawing.Image)(resources.GetObject("PicStatRepOnline.Image")));
            this.PicStatRepOnline.Location = new System.Drawing.Point(0, 0);
            this.PicStatRepOnline.Name = "PicStatRepOnline";
            this.PicStatRepOnline.Size = new System.Drawing.Size(44, 36);
            this.PicStatRepOnline.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicStatRepOnline.TabIndex = 2;
            this.PicStatRepOnline.TabStop = false;
            this.PicStatRepOnline.Visible = false;
            // 
            // pnlFill
            // 
            this.pnlFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFill.Location = new System.Drawing.Point(0, 36);
            this.pnlFill.Name = "pnlFill";
            this.pnlFill.Size = new System.Drawing.Size(800, 490);
            this.pnlFill.TabIndex = 3;
            // 
            // TsmItemConfigProviderObj
            // 
            this.TsmItemConfigProviderObj.Name = "TsmItemConfigProviderObj";
            this.TsmItemConfigProviderObj.Size = new System.Drawing.Size(392, 26);
            this.TsmItemConfigProviderObj.Text = "Настройка провайдера объектов";
            this.TsmItemConfigProviderObj.Click += new System.EventHandler(this.TsmItemConfigProviderObj_Click);
            // 
            // TsmItemConfigProviderMon
            // 
            this.TsmItemConfigProviderMon.Name = "TsmItemConfigProviderMon";
            this.TsmItemConfigProviderMon.Size = new System.Drawing.Size(392, 26);
            this.TsmItemConfigProviderMon.Text = "Настройка провайдера мониторинга";
            this.TsmItemConfigProviderMon.Click += new System.EventHandler(this.TsmItemConfigProviderMon_Click);
            // 
            // TsmItemConfigParam
            // 
            this.TsmItemConfigParam.Name = "TsmItemConfigParam";
            this.TsmItemConfigParam.Size = new System.Drawing.Size(392, 26);
            this.TsmItemConfigParam.Text = "Настройка других параметров программы";
            this.TsmItemConfigParam.Click += new System.EventHandler(this.TsmItemConfigParam_Click);
            // 
            // FStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 552);
            this.Controls.Add(this.pnlFill);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.statusStrip);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FStart";
            this.ShowInTaskbar = false;
            this.Text = "Напоминалка созданная Погодиным И.М.";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FStart_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlTopRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicStatRepOfline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicStatRepOnline)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tSSLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelIcon;
        private System.Windows.Forms.ToolStripMenuItem TsmItemConfig;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlTopRight;
        private System.Windows.Forms.Panel pnlFill;
        private System.Windows.Forms.ToolStripMenuItem TsmItemConfigRepository;
        private System.Windows.Forms.PictureBox PicStatRepOfline;
        private System.Windows.Forms.PictureBox PicStatRepOnline;
        private System.Windows.Forms.ToolStripMenuItem TsmItemConfigProviderObj;
        private System.Windows.Forms.ToolStripMenuItem TsmItemConfigProviderMon;
        private System.Windows.Forms.ToolStripMenuItem TsmItemConfigParam;
    }
}

