﻿namespace VSS2Git
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnDump = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVss = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSSUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSSPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtExtractPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSSRoot = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.chkTest = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtReport = new System.Windows.Forms.TextBox();
            this.chkAutoProject = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtCmdLog = new System.Windows.Forms.TextBox();
            this.statLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDump
            // 
            this.btnDump.Location = new System.Drawing.Point(10, 318);
            this.btnDump.Name = "btnDump";
            this.btnDump.Size = new System.Drawing.Size(68, 24);
            this.btnDump.TabIndex = 12;
            this.btnDump.Text = "Dump";
            this.btnDump.UseVisualStyleBackColor = true;
            this.btnDump.Click += new System.EventHandler(this.btnDump_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "SS Database";
            // 
            // txtVss
            // 
            this.txtVss.Location = new System.Drawing.Point(120, 8);
            this.txtVss.Name = "txtVss";
            this.txtVss.Size = new System.Drawing.Size(232, 20);
            this.txtVss.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "SS User";
            // 
            // txtSSUser
            // 
            this.txtSSUser.Location = new System.Drawing.Point(120, 36);
            this.txtSSUser.Name = "txtSSUser";
            this.txtSSUser.Size = new System.Drawing.Size(232, 20);
            this.txtSSUser.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "SS Password";
            // 
            // txtSSPassword
            // 
            this.txtSSPassword.Location = new System.Drawing.Point(120, 64);
            this.txtSSPassword.Name = "txtSSPassword";
            this.txtSSPassword.Size = new System.Drawing.Size(232, 20);
            this.txtSSPassword.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Extract Path";
            // 
            // txtExtractPath
            // 
            this.txtExtractPath.Location = new System.Drawing.Point(120, 120);
            this.txtExtractPath.Name = "txtExtractPath";
            this.txtExtractPath.Size = new System.Drawing.Size(232, 20);
            this.txtExtractPath.TabIndex = 4;
            this.txtExtractPath.Text = "%TEMP%\\VSS2Git";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "SS Root";
            // 
            // txtSSRoot
            // 
            this.txtSSRoot.Location = new System.Drawing.Point(120, 92);
            this.txtSSRoot.Name = "txtSSRoot";
            this.txtSSRoot.Size = new System.Drawing.Size(232, 20);
            this.txtSSRoot.TabIndex = 3;
            this.txtSSRoot.Text = "$/";
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(11, 379);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStatus.Size = new System.Drawing.Size(796, 180);
            this.txtStatus.TabIndex = 11;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 352);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 26;
            // 
            // chkTest
            // 
            this.chkTest.AutoSize = true;
            this.chkTest.Location = new System.Drawing.Point(95, 323);
            this.chkTest.Name = "chkTest";
            this.chkTest.Size = new System.Drawing.Size(47, 17);
            this.chkTest.TabIndex = 13;
            this.chkTest.Text = "Test";
            this.chkTest.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(367, 11);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(41, 13);
            this.label16.TabIndex = 29;
            this.label16.Text = "Log file";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(452, 8);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(293, 20);
            this.txtLog.TabIndex = 5;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(367, 39);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(55, 13);
            this.label17.TabIndex = 30;
            this.label17.Text = "Report file";
            // 
            // txtReport
            // 
            this.txtReport.Location = new System.Drawing.Point(452, 36);
            this.txtReport.Name = "txtReport";
            this.txtReport.Size = new System.Drawing.Size(293, 20);
            this.txtReport.TabIndex = 31;
            // 
            // chkAutoProject
            // 
            this.chkAutoProject.AutoSize = true;
            this.chkAutoProject.Location = new System.Drawing.Point(165, 323);
            this.chkAutoProject.Name = "chkAutoProject";
            this.chkAutoProject.Size = new System.Drawing.Size(187, 17);
            this.chkAutoProject.TabIndex = 33;
            this.chkAutoProject.Text = "Auto Project/Repository Creation?";
            this.chkAutoProject.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(367, 68);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(75, 13);
            this.label19.TabIndex = 34;
            this.label19.Text = "Command Log";
            // 
            // txtCmdLog
            // 
            this.txtCmdLog.Location = new System.Drawing.Point(452, 65);
            this.txtCmdLog.Name = "txtCmdLog";
            this.txtCmdLog.Size = new System.Drawing.Size(293, 20);
            this.txtCmdLog.TabIndex = 35;
            // 
            // statLabel
            // 
            this.statLabel.AutoSize = true;
            this.statLabel.Location = new System.Drawing.Point(8, 294);
            this.statLabel.Name = "statLabel";
            this.statLabel.Size = new System.Drawing.Size(0, 13);
            this.statLabel.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 571);
            this.Controls.Add(this.txtCmdLog);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.chkAutoProject);
            this.Controls.Add(this.txtReport);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.chkTest);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.txtSSRoot);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtExtractPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSSPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSSUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.statLabel);
            this.Controls.Add(this.txtVss);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDump);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Dump VSS Database";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDump;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVss;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSSUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSSPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtExtractPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSSRoot;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chkTest;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtReport;
        private System.Windows.Forms.CheckBox chkAutoProject;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtCmdLog;
        private System.Windows.Forms.Label statLabel;
    }
}

