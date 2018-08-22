namespace smART.Integration.LeadsOnlineDesktop {
  partial class frmLeadsOnline {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.btnPost = new System.Windows.Forms.Button();
      this.btnClose = new System.Windows.Forms.Button();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // btnPost
      // 
      this.btnPost.Location = new System.Drawing.Point(57, 57);
      this.btnPost.Name = "btnPost";
      this.btnPost.Size = new System.Drawing.Size(258, 50);
      this.btnPost.TabIndex = 0;
      this.btnPost.Text = "POST";
      this.btnPost.UseVisualStyleBackColor = true;
      this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
      // 
      // btnClose
      // 
      this.btnClose.Location = new System.Drawing.Point(57, 145);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(258, 50);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(13, 214);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(384, 206);
      this.textBox1.TabIndex = 2;
      // 
      // frmLeadsOnline
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(409, 432);
      this.ControlBox = false;
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.btnPost);
      this.MaximizeBox = false;
      this.Name = "frmLeadsOnline";
      this.Text = "Leads Online";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnPost;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.TextBox textBox1;
  }
}

