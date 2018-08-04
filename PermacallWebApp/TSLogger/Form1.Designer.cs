namespace TSLogger
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
            this.components = new System.ComponentModel.Container();
            this.bntLogNow = new System.Windows.Forms.Button();
            this.logTimer = new System.Windows.Forms.Timer(this.components);
            this.btnOrder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bntLogNow
            // 
            this.bntLogNow.Location = new System.Drawing.Point(49, 12);
            this.bntLogNow.Name = "bntLogNow";
            this.bntLogNow.Size = new System.Drawing.Size(75, 23);
            this.bntLogNow.TabIndex = 0;
            this.bntLogNow.Text = "Log Now";
            this.bntLogNow.UseVisualStyleBackColor = true;
            this.bntLogNow.Click += new System.EventHandler(this.bntLogNow_Click);
            // 
            // logTimer
            // 
            this.logTimer.Enabled = true;
            this.logTimer.Interval = 300000;
            this.logTimer.Tick += new System.EventHandler(this.logTimer_Tick);
            // 
            // btnOrder
            // 
            this.btnOrder.Location = new System.Drawing.Point(49, 41);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(75, 23);
            this.btnOrder.TabIndex = 1;
            this.btnOrder.Text = "Order Now";
            this.btnOrder.UseVisualStyleBackColor = true;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 70);
            this.Controls.Add(this.btnOrder);
            this.Controls.Add(this.bntLogNow);
            this.MaximumSize = new System.Drawing.Size(200, 109);
            this.MinimumSize = new System.Drawing.Size(200, 109);
            this.Name = "Form1";
            this.Text = "TSLogger";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bntLogNow;
        private System.Windows.Forms.Timer logTimer;
        private System.Windows.Forms.Button btnOrder;
    }
}

