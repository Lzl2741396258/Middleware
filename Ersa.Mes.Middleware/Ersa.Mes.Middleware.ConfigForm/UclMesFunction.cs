using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.ConfigForm;

public class UclMesFunction : UserControl, Inf_ConfigForm
{
	private IContainer components = null;

	private ObjectListView m_olvMesFunction;

	private OLVColumn olvColumn1;

	private OLVColumn olvColumn2;

	private OLVColumn olvColumn3;

	private OLVColumn olvColumn4;

	public int m_i32FormID { get; set; } = 403;


	public string Pro_FormName => "MesFunction";

	private List<Edc_ConfigMesFunction> m_lstConfigMesFunction { get; set; }

	public UclMesFunction(Edc_ConfigBase i_Config)
	{
		InitializeComponent();
		m_lstConfigMesFunction = i_Config.m_lstMesFunction;
		m_olvMesFunction.SetObjects(m_lstConfigMesFunction.ToList());
	}

	public bool Fun_blnSave(Edc_ConfigBase i_Config, bool i_blnShowMessagebox = true)
	{
		try
		{
			i_Config.m_lstMesFunction = (List<Edc_ConfigMesFunction>)m_olvMesFunction.Objects;
		}
		catch
		{
			return false;
		}
		return true;
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
		this.m_olvMesFunction = new BrightIdeasSoftware.ObjectListView();
		this.olvColumn1 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn2 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn3 = new BrightIdeasSoftware.OLVColumn();
		this.olvColumn4 = new BrightIdeasSoftware.OLVColumn();
		((System.ComponentModel.ISupportInitialize)this.m_olvMesFunction).BeginInit();
		base.SuspendLayout();
		this.m_olvMesFunction.AllColumns.Add(this.olvColumn1);
		this.m_olvMesFunction.AllColumns.Add(this.olvColumn2);
		this.m_olvMesFunction.AllColumns.Add(this.olvColumn3);
		this.m_olvMesFunction.AllColumns.Add(this.olvColumn4);
		this.m_olvMesFunction.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
		this.m_olvMesFunction.CellEditUseWholeCell = false;
		this.m_olvMesFunction.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4] { this.olvColumn1, this.olvColumn2, this.olvColumn3, this.olvColumn4 });
		this.m_olvMesFunction.Cursor = System.Windows.Forms.Cursors.Default;
		this.m_olvMesFunction.Dock = System.Windows.Forms.DockStyle.Fill;
		this.m_olvMesFunction.Font = new System.Drawing.Font("阿里巴巴普惠体 2.0 55 Regular", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.m_olvMesFunction.FullRowSelect = true;
		this.m_olvMesFunction.HideSelection = false;
		this.m_olvMesFunction.Location = new System.Drawing.Point(0, 0);
		this.m_olvMesFunction.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.m_olvMesFunction.Name = "m_olvMesFunction";
		this.m_olvMesFunction.ShowGroups = false;
		this.m_olvMesFunction.ShowImagesOnSubItems = true;
		this.m_olvMesFunction.Size = new System.Drawing.Size(859, 620);
		this.m_olvMesFunction.TabIndex = 4;
		this.m_olvMesFunction.UseCompatibleStateImageBehavior = false;
		this.m_olvMesFunction.View = System.Windows.Forms.View.Details;
		this.olvColumn1.AspectName = "Pro_strName";
		this.olvColumn1.Text = "Name";
		this.olvColumn1.Width = 150;
		this.olvColumn2.AspectName = "Pro_blnActivePlatform";
		this.olvColumn2.Text = "Active";
		this.olvColumn2.Width = 125;
		this.olvColumn3.AspectName = "Pro_blnActiveDatabase";
		this.olvColumn3.Text = "DatabaseActive";
		this.olvColumn3.Width = 133;
		this.olvColumn4.AspectName = "Pro_blnActiveLocalFile";
		this.olvColumn4.Text = "LocalFileActive";
		this.olvColumn4.Width = 158;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.m_olvMesFunction);
		base.Name = "UclMesFunction";
		base.Size = new System.Drawing.Size(859, 620);
		((System.ComponentModel.ISupportInitialize)this.m_olvMesFunction).EndInit();
		base.ResumeLayout(false);
	}
}
