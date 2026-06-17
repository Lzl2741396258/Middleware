using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Ersa.Mes.PLC.Model;

namespace Ersa.Mes.Middleware;

public class P_frmDataPlc : Form
{
	private int m_i32OlvSelectedIndex = 0;

	private int m_i32X = 0;

	private int m_i32Y = 0;

	private IContainer components = null;

	private GroupBox groupBox1;

	private Button m_btnSearch;

	private TextBox textBox1;

	public ObjectListView m_olvParameter;

	private OLVColumn olvColumn3;

	private OLVColumn olvColumn1;

	private OLVColumn olvColumn2;

	private Dictionary<string, List<Edc_PLCElement>> m_dic { get; }

	public P_frmDataPlc(Dictionary<string, List<Edc_PLCElement>> i_dic)
	{
		if (!base.DesignMode)
		{
			InitializeComponent();
		}
		m_dic = i_dic;
		Sub_Load();
		Sub_DataReflash();
	}

	private void Sub_Load()
	{
		if (m_dic == null || m_dic.Count <= 0)
		{
			return;
		}
		List<Edc_PLCElement> list = new List<Edc_PLCElement>();
		foreach (KeyValuePair<string, List<Edc_PLCElement>> item in m_dic)
		{
			list.AddRange(item.Value);
		}
		m_olvParameter.SetObjects(list);
	}

	protected virtual List<Edc_PLCElement> Fun_lstGetPlcElement()
	{
		return new List<Edc_PLCElement>();
	}

	private void Sub_DataReflash()
	{
		Task.Run(delegate
		{
			while (true)
			{
				Thread.Sleep(10000);
				try
				{
					List<Edc_PLCElement> list = Fun_lstGetPlcElement();
					Invoke((Action)delegate
					{
						if (list.Count > 0)
						{
							m_olvParameter.SetObjects(list);
							m_olvParameter.SelectedIndex = m_i32OlvSelectedIndex;
							m_olvParameter.EnsureModelVisible(list[m_i32OlvSelectedIndex]);
						}
					});
				}
				catch
				{
				}
			}
		});
	}

	private void m_olvParameter_SelectedIndexChanged(object sender, EventArgs e)
	{
		m_i32OlvSelectedIndex = m_olvParameter.SelectedIndex;
	}

	private void m_btnSearch_Click(object sender, EventArgs e)
	{
		IEnumerator test1 = m_olvParameter.Objects.GetEnumerator();
		while (test1.MoveNext())
		{
			Edc_PLCElement test2 = (Edc_PLCElement)test1.Current;
			if (test2.Pro_strAddress.Equals(textBox1.Text))
			{
				m_olvParameter.SelectedObject = test2;
				m_olvParameter.RefreshObject(test2);
				m_olvParameter.EnsureModelVisible(test2);
			}
		}
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
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.m_btnSearch = new System.Windows.Forms.Button();
		this.m_olvParameter = new BrightIdeasSoftware.ObjectListView();
		this.olvColumn3 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn1 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn2 = new BrightIdeasSoftware.OLVColumn();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.m_olvParameter).BeginInit();
		base.SuspendLayout();
		this.groupBox1.Controls.Add(this.m_btnSearch);
		this.groupBox1.Controls.Add(this.textBox1);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1225, 117);
		this.groupBox1.TabIndex = 3;
		this.groupBox1.TabStop = false;
		this.textBox1.Location = new System.Drawing.Point(45, 46);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(658, 28);
		this.textBox1.TabIndex = 0;
		this.m_btnSearch.Location = new System.Drawing.Point(726, 38);
		this.m_btnSearch.Name = "m_btnSearch";
		this.m_btnSearch.Size = new System.Drawing.Size(111, 41);
		this.m_btnSearch.TabIndex = 1;
		this.m_btnSearch.Text = "Search";
		this.m_btnSearch.UseVisualStyleBackColor = true;
		this.m_btnSearch.Click += new System.EventHandler(m_btnSearch_Click);
		this.m_olvParameter.AllColumns.Add(this.olvColumn3);
		this.m_olvParameter.AllColumns.Add(this.olvColumn1);
		this.m_olvParameter.AllColumns.Add(this.olvColumn2);
		this.m_olvParameter.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
		this.m_olvParameter.CellEditUseWholeCell = false;
		this.m_olvParameter.Columns.AddRange(new System.Windows.Forms.ColumnHeader[3] { this.olvColumn3, this.olvColumn1, this.olvColumn2 });
		this.m_olvParameter.Cursor = System.Windows.Forms.Cursors.Default;
		this.m_olvParameter.Dock = System.Windows.Forms.DockStyle.Fill;
		this.m_olvParameter.EmptyListMsg = "PLC data is empty";
		this.m_olvParameter.FullRowSelect = true;
		this.m_olvParameter.GridLines = true;
		this.m_olvParameter.HideSelection = false;
		this.m_olvParameter.Location = new System.Drawing.Point(0, 117);
		this.m_olvParameter.Margin = new System.Windows.Forms.Padding(4);
		this.m_olvParameter.Name = "m_olvParameter";
		this.m_olvParameter.Size = new System.Drawing.Size(1225, 891);
		this.m_olvParameter.TabIndex = 4;
		this.m_olvParameter.UseAlternatingBackColors = true;
		this.m_olvParameter.UseCompatibleStateImageBehavior = false;
		this.m_olvParameter.View = System.Windows.Forms.View.Details;
		this.m_olvParameter.SelectedIndexChanged += new System.EventHandler(m_olvParameter_SelectedIndexChanged);
		this.olvColumn3.AspectName = "Pro_strGroupName";
		this.olvColumn3.Text = "GroupName";
		this.olvColumn3.Width = 150;
		this.olvColumn1.AspectName = "Pro_strAddress";
		this.olvColumn1.Text = "Address";
		this.olvColumn1.Width = 334;
		this.olvColumn2.AspectName = "Pro_objValue";
		this.olvColumn2.Text = "Value";
		this.olvColumn2.Width = 150;
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 18f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1225, 1008);
		base.Controls.Add(this.m_olvParameter);
		base.Controls.Add(this.groupBox1);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "FrmDataPlcParent";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Pvi Data";
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.m_olvParameter).EndInit();
		base.ResumeLayout(false);
	}
}
