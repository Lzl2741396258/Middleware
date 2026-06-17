using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MesXPT;

public class FrmRegister : Form
{
	private IContainer components = null;

	private Button m_btnRegister;

	private TextBox m_txtKey;

	private Label label1;

	public string m_strKey { get; set; }

	public FrmRegister()
	{
		InitializeComponent();
	}

	private void m_btnRegister_Click(object sender, EventArgs e)
	{
		m_strKey = m_txtKey.Text.Trim();
		Hide();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.m_btnRegister = new System.Windows.Forms.Button();
		this.m_txtKey = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.m_btnRegister.Location = new System.Drawing.Point(154, 114);
		this.m_btnRegister.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.m_btnRegister.Name = "m_btnRegister";
		this.m_btnRegister.Size = new System.Drawing.Size(132, 52);
		this.m_btnRegister.TabIndex = 5;
		this.m_btnRegister.Text = "Register";
		this.m_btnRegister.UseVisualStyleBackColor = true;
		this.m_btnRegister.Click += new System.EventHandler(m_btnRegister_Click);
		this.m_txtKey.Location = new System.Drawing.Point(89, 54);
		this.m_txtKey.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.m_txtKey.Name = "m_txtKey";
		this.m_txtKey.Size = new System.Drawing.Size(329, 26);
		this.m_txtKey.TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(38, 58);
		this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(31, 16);
		this.label1.TabIndex = 3;
		this.label1.Text = "Key";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(485, 221);
		base.Controls.Add(this.m_btnRegister);
		base.Controls.Add(this.m_txtKey);
		base.Controls.Add(this.label1);
		this.Font = new System.Drawing.Font("宋体", 12f);
		base.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		base.Name = "FrmRegister";
		this.Text = "Key";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
