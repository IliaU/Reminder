﻿
namespace Reminder
{
    partial class FProviderSetup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.txtBoxConnectionString = new System.Windows.Forms.TextBox();
            this.btnConfig = new System.Windows.Forms.Button();
            this.cmbBoxPrvTyp = new System.Windows.Forms.ComboBox();
            this.lblPrvTyp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(16, 48);
            this.lblConnectionString.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(152, 17);
            this.lblConnectionString.TabIndex = 14;
            this.lblConnectionString.Text = "Строка подключения:";
            // 
            // txtBoxConnectionString
            // 
            this.txtBoxConnectionString.Location = new System.Drawing.Point(172, 45);
            this.txtBoxConnectionString.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoxConnectionString.Multiline = true;
            this.txtBoxConnectionString.Name = "txtBoxConnectionString";
            this.txtBoxConnectionString.ReadOnly = true;
            this.txtBoxConnectionString.Size = new System.Drawing.Size(347, 61);
            this.txtBoxConnectionString.TabIndex = 13;
            // 
            // btnConfig
            // 
            this.btnConfig.Location = new System.Drawing.Point(420, 114);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(100, 28);
            this.btnConfig.TabIndex = 12;
            this.btnConfig.Text = "Изменить";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // cmbBoxPrvTyp
            // 
            this.cmbBoxPrvTyp.FormattingEnabled = true;
            this.cmbBoxPrvTyp.Location = new System.Drawing.Point(172, 11);
            this.cmbBoxPrvTyp.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxPrvTyp.Name = "cmbBoxPrvTyp";
            this.cmbBoxPrvTyp.Size = new System.Drawing.Size(347, 24);
            this.cmbBoxPrvTyp.TabIndex = 11;
            // 
            // lblPrvTyp
            // 
            this.lblPrvTyp.AutoSize = true;
            this.lblPrvTyp.Location = new System.Drawing.Point(16, 15);
            this.lblPrvTyp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPrvTyp.Name = "lblPrvTyp";
            this.lblPrvTyp.Size = new System.Drawing.Size(120, 17);
            this.lblPrvTyp.TabIndex = 10;
            this.lblPrvTyp.Text = "Тип провайдера:";
            // 
            // FProviderSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 153);
            this.Controls.Add(this.lblConnectionString);
            this.Controls.Add(this.txtBoxConnectionString);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.cmbBoxPrvTyp);
            this.Controls.Add(this.lblPrvTyp);
            this.Name = "FProviderSetup";
            this.Text = "Настройка подключения к базе";
            this.Load += new System.EventHandler(this.FProviderSetup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.TextBox txtBoxConnectionString;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.ComboBox cmbBoxPrvTyp;
        private System.Windows.Forms.Label lblPrvTyp;
    }
}