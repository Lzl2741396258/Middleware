using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.Middleware.Properties;
using HslCommunication.Controls;

namespace Ersa.Mes.Middleware;

public class P_frmDataErsa : Form
{
	private CancellationTokenSource m_cts = new CancellationTokenSource();

	private IContainer components = null;

	private ToolStrip toolStrip1;

	public ToolStripButton m_tsbtnStart;

	public ToolStripButton m_tsbtnStop;

	private UserCurve m_uc1;

	private ObjectListView m_olvParameter;

	private OLVColumn olvColumn1;

	private OLVColumn olvColumn2;

	private OLVColumn olvColumn3;

	private OLVColumn olvColumn4;

	protected List<Struct_ProcessParameter> m_List { get; set; } = new List<Struct_ProcessParameter>();


	private List<float> m_u32Value { get; set; } = new List<float>();


	private int m_i32IndexErsaData { get; set; } = -1;


	private string m_strCurrentCurveName { get; set; }

	public P_frmDataErsa(List<Struct_ProcessParameter> i_List)
	{
		InitializeComponent();
		m_List = i_List;
		m_cts = new CancellationTokenSource();
		Task.Run(delegate
		{
			while (true)
			{
				m_olvParameter.SetObjects(m_List);
				Thread.Sleep(10000);
			}
		});
		Task.Run(delegate
		{
			while (true)
			{
				Sub_Load(m_List);
				Thread.Sleep(2000);
			}
		});
	}

	protected virtual void Sub_Load(List<Struct_ProcessParameter> list)
	{
		if (list != null && list.Count > 0)
		{
			m_List = list;
		}
	}

	private void m_olvParameter_SelectedIndexChanged(object sender, EventArgs e)
	{
		m_i32IndexErsaData = m_olvParameter.SelectedIndex;
	}

	private void m_tsbtnStart_Click(object sender, EventArgs e)
	{
		try
		{
			if (m_i32IndexErsaData < 0)
			{
				return;
			}
			m_u32Value.Clear();
			m_cts = new CancellationTokenSource();
			Task.Factory.StartNew(delegate
			{
				while (!m_cts.IsCancellationRequested)
				{
					int.TryParse(m_List[m_i32IndexErsaData].m_strValue, out var result);
					m_uc1.ValueMaxLeft = (int)((double)result * 1.2);
					m_uc1.ValueMaxRight = (int)((double)result * 1.2);
					m_u32Value.Add(result);
					Thread.Sleep(1000);
					m_strCurrentCurveName = m_List[m_i32IndexErsaData]?.m_strName;
					m_uc1.SetRightCurve(m_strCurrentCurveName, m_u32Value.ToArray(), Color.Blue);
				}
			}, m_cts.Token, TaskCreationOptions.None, TaskScheduler.Default);
		}
		catch (Exception)
		{
		}
	}

	private void m_tsbtnStop_Click(object sender, EventArgs e)
	{
		m_cts.Cancel();
		m_uc1.RemoveCurve(m_strCurrentCurveName);
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
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.m_uc1 = new HslCommunication.Controls.UserCurve();
		this.m_olvParameter = new BrightIdeasSoftware.ObjectListView();
		this.olvColumn1 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn2 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn3 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn4 = new BrightIdeasSoftware.OLVColumn();
		this.m_tsbtnStart = new System.Windows.Forms.ToolStripButton();
		this.m_tsbtnStop = new System.Windows.Forms.ToolStripButton();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.m_olvParameter).BeginInit();
		base.SuspendLayout();
		this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(38, 38);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.m_tsbtnStart, this.m_tsbtnStop });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip1.Size = new System.Drawing.Size(834, 62);
		this.toolStrip1.TabIndex = 16;
		this.toolStrip1.Text = "123";
		this.m_uc1.BackColor = System.Drawing.Color.Transparent;
		this.m_uc1.Dock = System.Windows.Forms.DockStyle.Top;
		this.m_uc1.IsAbscissaStrech = true;
		this.m_uc1.Location = new System.Drawing.Point(0, 62);
		this.m_uc1.Name = "m_uc1";
		this.m_uc1.Size = new System.Drawing.Size(834, 271);
		this.m_uc1.TabIndex = 17;
		this.m_uc1.ValueMaxLeft = 300f;
		this.m_uc1.ValueMaxRight = 300f;
		this.m_olvParameter.AllColumns.Add(this.olvColumn1);
		this.m_olvParameter.AllColumns.Add(this.olvColumn2);
		this.m_olvParameter.AllColumns.Add(this.olvColumn3);
		this.m_olvParameter.AllColumns.Add(this.olvColumn4);
		this.m_olvParameter.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
		this.m_olvParameter.CellEditUseWholeCell = false;
		this.m_olvParameter.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4] { this.olvColumn1, this.olvColumn2, this.olvColumn3, this.olvColumn4 });
		this.m_olvParameter.Cursor = System.Windows.Forms.Cursors.Default;
		this.m_olvParameter.Dock = System.Windows.Forms.DockStyle.Fill;
		this.m_olvParameter.EmptyListMsg = "Ersa realtime data is empty";
		this.m_olvParameter.FullRowSelect = true;
		this.m_olvParameter.GridLines = true;
		this.m_olvParameter.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
		this.m_olvParameter.HideSelection = false;
		this.m_olvParameter.Location = new System.Drawing.Point(0, 333);
		this.m_olvParameter.Name = "m_olvParameter";
		this.m_olvParameter.ShowGroups = false;
		this.m_olvParameter.ShowImagesOnSubItems = true;
		this.m_olvParameter.Size = new System.Drawing.Size(834, 428);
		this.m_olvParameter.TabIndex = 18;
		this.m_olvParameter.UseAlternatingBackColors = true;
		this.m_olvParameter.UseCompatibleStateImageBehavior = false;
		this.m_olvParameter.View = System.Windows.Forms.View.Details;
		this.m_olvParameter.SelectedIndexChanged += new System.EventHandler(m_olvParameter_SelectedIndexChanged);
		this.olvColumn1.AspectName = "m_strName";
		this.olvColumn1.Text = "Name";
		this.olvColumn1.Width = 180;
		this.olvColumn2.AspectName = "m_strValue";
		this.olvColumn2.Text = "Value";
		this.olvColumn2.Width = 150;
		this.olvColumn3.AspectName = "m_strUnit";
		this.olvColumn3.Text = "Unit";
		this.olvColumn3.Width = 150;
		this.olvColumn4.AspectName = "m_bytTrackNumber";
		this.olvColumn4.Text = "TrackNumber";
		this.olvColumn4.Width = 150;
		this.m_tsbtnStart.Image = Resources.PNG_Starten_48x48;
		this.m_tsbtnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.m_tsbtnStart.Name = "m_tsbtnStart";
		this.m_tsbtnStart.Size = new System.Drawing.Size(55, 59);
		this.m_tsbtnStart.Tag = "1_1";
		this.m_tsbtnStart.Text = "  Start  ";
		this.m_tsbtnStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
		this.m_tsbtnStart.Click += new System.EventHandler(m_tsbtnStart_Click);
		this.m_tsbtnStop.Font = new System.Drawing.Font("微软雅黑", 9f);
		this.m_tsbtnStop.Image = Resources.PNG_Stoppen_48x48;
		this.m_tsbtnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.m_tsbtnStop.Name = "m_tsbtnStop";
		this.m_tsbtnStop.Size = new System.Drawing.Size(55, 59);
		this.m_tsbtnStop.Text = "  Stop  ";
		this.m_tsbtnStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
		this.m_tsbtnStop.Click += new System.EventHandler(m_tsbtnStop_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(834, 761);
		base.Controls.Add(this.m_olvParameter);
		base.Controls.Add(this.m_uc1);
		base.Controls.Add(this.toolStrip1);
		base.Name = "P_frmDataErsa";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Ersasoft Realtime Data";
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.m_olvParameter).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
