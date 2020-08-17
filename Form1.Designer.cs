namespace VSS2Git
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
            this.statLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSSUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSSPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtExtractPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSSRoot = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCommit = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAdd = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtUpdate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtOpen = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtClose = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.chkTest = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtCreate = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtReport = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.chkAutoProject = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnDump
            // 
            this.btnDump.Location = new System.Drawing.Point(651, 68);
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
            // statLabel
            // 
            this.statLabel.AutoSize = true;
            this.statLabel.Location = new System.Drawing.Point(8, 294);
            this.statLabel.Name = "statLabel";
            this.statLabel.Size = new System.Drawing.Size(0, 13);
            this.statLabel.TabIndex = 3;
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
            this.txtStatus.Location = new System.Drawing.Point(8, 341);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStatus.Size = new System.Drawing.Size(796, 180);
            this.txtStatus.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 262);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Commit All Changes";
            // 
            // txtCommit
            // 
            this.txtCommit.Location = new System.Drawing.Point(120, 258);
            this.txtCommit.Name = "txtCommit";
            this.txtCommit.Size = new System.Drawing.Size(500, 20);
            this.txtCommit.TabIndex = 10;
            this.txtCommit.Text = "git commit -a -m \"{1}\"";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 206);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Add New File";
            // 
            // txtAdd
            // 
            this.txtAdd.Location = new System.Drawing.Point(120, 202);
            this.txtAdd.Name = "txtAdd";
            this.txtAdd.Size = new System.Drawing.Size(500, 20);
            this.txtAdd.TabIndex = 8;
            this.txtAdd.Text = "git add \"{0}\"";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 234);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Update Existing File";
            // 
            // txtUpdate
            // 
            this.txtUpdate.Location = new System.Drawing.Point(120, 230);
            this.txtUpdate.Name = "txtUpdate";
            this.txtUpdate.Size = new System.Drawing.Size(500, 20);
            this.txtUpdate.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 178);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "SCM Open";
            // 
            // txtOpen
            // 
            this.txtOpen.Location = new System.Drawing.Point(120, 174);
            this.txtOpen.Name = "txtOpen";
            this.txtOpen.Size = new System.Drawing.Size(500, 20);
            this.txtOpen.TabIndex = 7;
            this.txtOpen.Text = "git init";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(632, 178);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "{0} - Extract path";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(632, 206);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "{0} - File name {1} - comment";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(632, 234);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(144, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "{0} - File name {1} - comment";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 290);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "SCM Close";
            // 
            // txtClose
            // 
            this.txtClose.Location = new System.Drawing.Point(120, 286);
            this.txtClose.Name = "txtClose";
            this.txtClose.Size = new System.Drawing.Size(500, 20);
            this.txtClose.TabIndex = 11;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(632, 262);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(150, 13);
            this.label14.TabIndex = 25;
            this.label14.Text = "{0} - Timestamp {1} - comment";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 318);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 26;
            // 
            // chkTest
            // 
            this.chkTest.AutoSize = true;
            this.chkTest.Location = new System.Drawing.Point(743, 72);
            this.chkTest.Name = "chkTest";
            this.chkTest.Size = new System.Drawing.Size(47, 17);
            this.chkTest.TabIndex = 13;
            this.chkTest.Text = "Test";
            this.chkTest.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 149);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 13);
            this.label15.TabIndex = 28;
            this.label15.Text = "SCM Create";
            // 
            // txtCreate
            // 
            this.txtCreate.Location = new System.Drawing.Point(120, 146);
            this.txtCreate.Name = "txtCreate";
            this.txtCreate.Size = new System.Drawing.Size(500, 20);
            this.txtCreate.TabIndex = 6;
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
            this.txtLog.Location = new System.Drawing.Point(426, 8);
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
            this.txtReport.Location = new System.Drawing.Point(426, 36);
            this.txtReport.Name = "txtReport";
            this.txtReport.Size = new System.Drawing.Size(293, 20);
            this.txtReport.TabIndex = 31;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(632, 149);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(165, 13);
            this.label18.TabIndex = 32;
            this.label18.Text = "{0} - Extract Path {1} - Timestamp";
            // 
            // chkAutoProject
            // 
            this.chkAutoProject.AutoSize = true;
            this.chkAutoProject.Location = new System.Drawing.Point(655, 103);
            this.chkAutoProject.Name = "chkAutoProject";
            this.chkAutoProject.Size = new System.Drawing.Size(187, 17);
            this.chkAutoProject.TabIndex = 33;
            this.chkAutoProject.Text = "Auto Project/Repository Creation?";
            this.chkAutoProject.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 545);
            this.Controls.Add(this.chkAutoProject);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.txtReport);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtCreate);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.chkTest);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtClose);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtOpen);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtUpdate);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtAdd);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtCommit);
            this.Controls.Add(this.label6);
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
        private System.Windows.Forms.Label statLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSSUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSSPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtExtractPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSSRoot;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCommit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAdd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtUpdate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtOpen;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtClose;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chkTest;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtCreate;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtReport;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox chkAutoProject;
    }
}

