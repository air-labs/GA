namespace Presenter
{
    partial class MainForm
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
            this.controls = new System.Windows.Forms.GroupBox();
            this.isCompleteView = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.stack = new System.Windows.Forms.TableLayoutPanel();
            this.view = new System.Windows.Forms.GroupBox();
            this.plot = new System.Windows.Forms.Panel();
            this.closeButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            this.dataBox = new System.Windows.Forms.TextBox();
            this.controls.SuspendLayout();
            this.view.SuspendLayout();
            this.SuspendLayout();
            // 
            // controls
            // 
            this.controls.Controls.Add(this.isCompleteView);
            this.controls.Controls.Add(this.comboBox1);
            this.controls.Controls.Add(this.stack);
            this.controls.Location = new System.Drawing.Point(590, 12);
            this.controls.Name = "controls";
            this.controls.Size = new System.Drawing.Size(233, 510);
            this.controls.TabIndex = 0;
            this.controls.TabStop = false;
            this.controls.Text = "Controls";
            // 
            // isCompleteView
            // 
            this.isCompleteView.AutoSize = true;
            this.isCompleteView.Location = new System.Drawing.Point(7, 460);
            this.isCompleteView.Name = "isCompleteView";
            this.isCompleteView.Size = new System.Drawing.Size(95, 17);
            this.isCompleteView.TabIndex = 3;
            this.isCompleteView.Text = "Complete view";
            this.isCompleteView.UseVisualStyleBackColor = true;
            this.isCompleteView.CheckedChanged += new System.EventHandler(this.IsCompleteViewCheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Success rate",
            "Iterations",
            "Length"});
            this.comboBox1.Location = new System.Drawing.Point(7, 483);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(220, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // stack
            // 
            this.stack.AutoScroll = true;
            this.stack.ColumnCount = 1;
            this.stack.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.stack.Location = new System.Drawing.Point(6, 19);
            this.stack.Name = "stack";
            this.stack.RowCount = 100;
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stack.Size = new System.Drawing.Size(221, 435);
            this.stack.TabIndex = 0;
            // 
            // view
            // 
            this.view.Controls.Add(this.plot);
            this.view.Location = new System.Drawing.Point(12, 12);
            this.view.Name = "view";
            this.view.Size = new System.Drawing.Size(572, 572);
            this.view.TabIndex = 1;
            this.view.TabStop = false;
            this.view.Text = "View";
            // 
            // plot
            // 
            this.plot.Location = new System.Drawing.Point(6, 19);
            this.plot.Name = "plot";
            this.plot.Size = new System.Drawing.Size(560, 547);
            this.plot.TabIndex = 0;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(590, 559);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(233, 25);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(590, 528);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(233, 25);
            this.openButton.TabIndex = 2;
            this.openButton.Text = "Open folder";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.OpenButtonClick);
            // 
            // dataBox
            // 
            this.dataBox.Location = new System.Drawing.Point(12, 590);
            this.dataBox.Multiline = true;
            this.dataBox.Name = "dataBox";
            this.dataBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.dataBox.Size = new System.Drawing.Size(811, 130);
            this.dataBox.TabIndex = 3;
            this.dataBox.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(835, 732);
            this.Controls.Add(this.dataBox);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.view);
            this.Controls.Add(this.controls);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Presentation";
            this.controls.ResumeLayout(false);
            this.controls.PerformLayout();
            this.view.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox controls;
        private System.Windows.Forms.GroupBox view;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Panel plot;
        private System.Windows.Forms.TableLayoutPanel stack;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox isCompleteView;
        private System.Windows.Forms.TextBox dataBox;
    }
}