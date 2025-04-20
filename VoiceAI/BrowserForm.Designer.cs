// Token: 0x02000002 RID: 2
public partial class BrowserForm : global::System.Windows.Forms.Form, global::CefSharp.IDisplayHandler
{
	// Token: 0x0600003B RID: 59 RVA: 0x00002BEB File Offset: 0x00000DEB
	protected override void Dispose(bool disposing)
	{
		if (disposing && this.components != null)
		{
			this.components.Dispose();
		}
		base.Dispose(disposing);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002C0C File Offset: 0x00000E0C
	private void InitializeComponent()
	{
		this.label1 = new global::System.Windows.Forms.Label();
		this.button1 = new global::System.Windows.Forms.Button();
		this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
		this.pictureBox2 = new global::System.Windows.Forms.PictureBox();
		this.pictureBox3 = new global::System.Windows.Forms.PictureBox();
		((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		((global::System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		((global::System.ComponentModel.ISupportInitialize)this.pictureBox3).BeginInit();
		base.SuspendLayout();
		this.label1.CausesValidation = false;
		this.label1.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = global::System.Drawing.SystemColors.Window;
		this.label1.Location = new global::System.Drawing.Point(37, 6);
		this.label1.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new global::System.Drawing.Size(120, 23);
		this.label1.TabIndex = 1;
		this.label1.Text = "voice.ai";
		this.button1.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right;
		this.button1.BackColor = global::System.Drawing.Color.Black;
		this.button1.BackgroundImage = global::VoiceAIGui.Properties.Resources.icon_close_27;
		this.button1.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
		this.button1.FlatAppearance.BorderSize = 0;
		this.button1.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
		this.button1.Location = new global::System.Drawing.Point(1150, 2);
		this.button1.Margin = new global::System.Windows.Forms.Padding(0);
		this.button1.Name = "button1";
		this.button1.Size = new global::System.Drawing.Size(31, 31);
		this.button1.TabIndex = 2;
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new global::System.EventHandler(this.button1_customCloseClick);
		this.pictureBox1.BackgroundImage = global::VoiceAIGui.Properties.Resources.icon_27;
		this.pictureBox1.BackgroundImageLayout = global::System.Windows.Forms.ImageLayout.Stretch;
		this.pictureBox1.ImageLocation = "";
		this.pictureBox1.InitialImage = global::VoiceAIGui.Properties.Resources.icon_64;
		this.pictureBox1.Location = new global::System.Drawing.Point(7, 4);
		this.pictureBox1.Margin = new global::System.Windows.Forms.Padding(0);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new global::System.Drawing.Size(25, 25);
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.pictureBox2.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
		this.pictureBox2.Location = new global::System.Drawing.Point(0, 0);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new global::System.Drawing.Size(1183, 34);
		this.pictureBox2.TabIndex = 0;
		this.pictureBox2.TabStop = false;
		this.pictureBox3.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
		this.pictureBox3.BackColor = global::System.Drawing.Color.Black;
		this.pictureBox3.Location = new global::System.Drawing.Point(0, 35);
		this.pictureBox3.Name = "pictureBox3";
		this.pictureBox3.Size = new global::System.Drawing.Size(1183, 1027);
		this.pictureBox3.TabIndex = 3;
		this.pictureBox3.TabStop = false;
		base.AutoScaleDimensions = new global::System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Dpi;
		base.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.BackColor = global::System.Drawing.Color.Black;
		base.ClientSize = new global::System.Drawing.Size(1184, 1061);
		base.Controls.Add(this.pictureBox3);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.pictureBox2);
		base.Margin = new global::System.Windows.Forms.Padding(4);
		this.MinimumSize = new global::System.Drawing.Size(1100, 700);
		base.Name = "BrowserForm";
		base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Show;
		base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
		((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		((global::System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		((global::System.ComponentModel.ISupportInitialize)this.pictureBox3).EndInit();
		base.ResumeLayout(false);
	}

	// Token: 0x0400000B RID: 11
	private global::System.ComponentModel.IContainer components;

	// Token: 0x0400000C RID: 12
	private global::System.Windows.Forms.PictureBox pictureBox1;

	// Token: 0x0400000D RID: 13
	private global::System.Windows.Forms.Button button1;

	// Token: 0x0400000E RID: 14
	private global::System.Windows.Forms.Label label1;

	// Token: 0x0400000F RID: 15
	private global::System.Windows.Forms.PictureBox pictureBox2;

	// Token: 0x04000010 RID: 16
	private global::System.Windows.Forms.PictureBox pictureBox3;
}
