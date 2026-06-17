using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Definition;
using Ersa.Mes.Middleware.Helper;
using Ersa.Mes.Middleware.Interfaces;
using Ersa.Mes.Middleware.Properties;
using Ersa.Mes.PLC;

namespace Ersa.Mes.Middleware.ConfigForm;

public class UclAutoSettings : UserControl, Inf_ConfigForm
{
	public const string Pro_strFormName = "Ersasoft MES";

	private Inf_PLC Pro_edcSpsService;

	public string m_strInitializeMES = AppDomain.CurrentDomain.BaseDirectory + "\\Initialize\\Initialize_MES.xml";

	private const string Pro_strSoftName = "ERSASOFT";

	private const string mC_ErsaInterfaceDllName = "Ersa.Mes.Schnittstelle.dll";

	private const string mC_ErsaSchnittstelleDllNameBak = "Ersa.Mes.Schnittstelle_bak.dll";

	private const string mC_PathTracebility = "Traceability";

	private const string mC_PathErsaInterfaceDll00 = "0_ERSA GmbH_Basis";

	private const string mC_PathErsaInterfaceDll05 = "5_Handke_TransferAgent";

	private const string mC_FilenameTracebility05Config = "KonfigTrace05Handke.xml";

	private string Pro_strInitializeFullname = string.Empty;

	private string m_strFilenameOld;

	private string m_strFilenameNew;

	private string m_strFullname00Dll;

	private string m_strFullname05Dll;

	private string m_strFullnameTrace05Config;

	public List<Edc_olvAutoSetting> m_lstAutoSetting = new List<Edc_olvAutoSetting>();

	public TcpClient m_TcpClient = new TcpClient();

	private DateTime m_dtmBegin;

	private IContainer components = null;

	private Button m_btnStart;

	private Button m_btnRestore;

	private ObjectListView m_olvOperating;

	public int m_i32FormID { get; set; } = 408;


	public string Pro_FormName => "Ersasoft MES";

	private Edc_ConfigBase m_Config { get; set; }

	private Inf_Logger m_edclogger { get; }

	public UclAutoSettings(Edc_ConfigBase i_Config, Inf_Logger i_logger)
	{
		InitializeComponent();
		m_Config = i_Config;
		m_edclogger = i_logger;
		Sub_InitializeOlv();
		Sub_GetPath(m_Config.m_edcFilesPath.m_strPathErsasoft);
	}

	private void Sub_GetPath(string i_strPathErsasoft)
	{
		if (!string.IsNullOrEmpty(i_strPathErsasoft))
		{
			m_strFilenameOld = Path.Combine(m_Config.m_edcFilesPath.m_strPathErsasoft, "Ersa.Mes.Schnittstelle.dll");
			m_strFilenameNew = Path.Combine(m_Config.m_edcFilesPath.m_strPathErsasoft, "Ersa.Mes.Schnittstelle_bak.dll");
			m_strFullname00Dll = Path.Combine(m_Config.m_edcFilesPath.m_strPathErsasoft, "Traceability", "0_ERSA GmbH_Basis", "Ersa.Mes.Schnittstelle.dll");
			m_strFullname05Dll = Path.Combine(m_Config.m_edcFilesPath.m_strPathErsasoft, "Traceability", "5_Handke_TransferAgent", "Ersa.Mes.Schnittstelle.dll");
			m_strFullnameTrace05Config = Path.Combine(m_Config.m_edcFilesPath.m_strPathErsasoft, "Traceability", "KonfigTrace05Handke.xml");
		}
	}

	public void Sub_InitializeOlv()
	{
		try
		{
			m_lstAutoSetting.Add(new Edc_olvAutoSetting
			{
				m_i32Step = 1,
				m_strOperating = "Auto search Ersasoft path",
				m_i32Status = 0
			});
			m_lstAutoSetting.Add(new Edc_olvAutoSetting
			{
				m_i32Step = 2,
				m_strOperating = "Close Ersasoft...",
				m_i32Status = 0
			});
			m_lstAutoSetting.Add(new Edc_olvAutoSetting
			{
				m_i32Step = 3,
				m_strOperating = "Backup data...",
				m_i32Status = 0
			});
			m_lstAutoSetting.Add(new Edc_olvAutoSetting
			{
				m_i32Step = 4,
				m_strOperating = "Assemble the interface file...",
				m_i32Status = 0
			});
			m_lstAutoSetting.Add(new Edc_olvAutoSetting
			{
				m_i32Step = 5,
				m_strOperating = "Modify the configuration file...",
				m_i32Status = 0
			});
			m_lstAutoSetting.Add(new Edc_olvAutoSetting
			{
				m_i32Step = 6,
				m_strOperating = "Load the Initialize file...",
				m_i32Status = 0
			});
			m_lstAutoSetting.Add(new Edc_olvAutoSetting
			{
				m_i32Step = 7,
				m_strOperating = "Restart Ersasoft...",
				m_i32Status = 0
			});
			m_lstAutoSetting.Add(new Edc_olvAutoSetting
			{
				m_i32Step = 8,
				m_strOperating = "Change Ersasoft MES options...",
				m_i32Status = 0
			});
			m_lstAutoSetting.Add(new Edc_olvAutoSetting
			{
				m_i32Step = 9,
				m_strOperating = "Restart Ersasoft...",
				m_i32Status = 0
			});
			m_lstAutoSetting.Add(new Edc_olvAutoSetting
			{
				m_i32Step = 10,
				m_strOperating = "Completed...",
				m_i32Status = 0
			});
			Sub_edcCreateOlv(ref m_olvOperating);
			m_olvOperating.SetObjects(m_lstAutoSetting);
		}
		catch (Exception ex)
		{
			m_edclogger.Error("Middleware -> Load Intialize MES Error..." + ex.Message, null, "Sub_InitializeOlv", 129);
		}
	}

	private void Sub_edcCreateOlv(ref ObjectListView i_olv)
	{
		i_olv.Columns.Clear();
		i_olv.Columns.Add(new OLVColumn("Step", "m_i32Step")
		{
			Width = 50
		});
		i_olv.Columns.Add(new OLVColumn("Operating", "m_strOperating")
		{
			Width = 300
		});
		OLVColumn ch3 = new OLVColumn("Status", "m_i32Status")
		{
			Width = 150
		};
		i_olv.Columns.Add(ch3);
		i_olv.Columns.Add(new OLVColumn("DateTime", "m_strDatetime")
		{
			Width = 200
		});
		i_olv.SmallImageList = Fun_edcGetImageList();
		i_olv.LargeImageList = i_olv.SmallImageList;
		try
		{
			ch3.AspectGetter = (object row) => ((Edc_olvAutoSetting)row).m_i32Status;
			ch3.ImageGetter = (object row) => ((Edc_olvAutoSetting)row).m_i32Status switch
			{
				0 => "Null", 
				1 => "select", 
				2 => "delete", 
				_ => "Null", 
			};
		}
		catch (Exception ex)
		{
			m_edclogger.Error(ex.Message, null, "Sub_edcCreateOlv", 163);
		}
		i_olv.EmptyListMsg = "No Infomation Now...";
		i_olv.HeaderStyle = ColumnHeaderStyle.Nonclickable;
		i_olv.ShowImagesOnSubItems = true;
		i_olv.ShowGroups = false;
		i_olv.FullRowSelect = true;
		i_olv.Scrollable = true;
	}

	private ImageList Fun_edcGetImageList()
	{
		ImageList a_ImageList = new ImageList();
		a_ImageList.Images.Add("0", Resources.information);
		a_ImageList.Images.SetKeyName(0, "Null");
		a_ImageList.Images.Add("1", Resources.select);
		a_ImageList.Images.SetKeyName(1, "select");
		a_ImageList.Images.Add("2", Resources.delete);
		a_ImageList.Images.SetKeyName(2, "delete");
		return a_ImageList;
	}

	private async void m_btnStart_Click(object sender, EventArgs e)
	{
		try
		{
			m_dtmBegin = DateTime.Now;
			if ((!Fun_blnNeedAutoSearch() || Fun_blnStepAction(Fun_blnAutoSearchErsasoftPath, 1)) && Fun_blnStepAction(Fun_blnKillErsasfot, 2) && Fun_blnStepAction(Fun_blnBackupDll, 3) && Fun_blnStepAction(Fun_blnCopyDll05, 4) && Fun_blnStepAction(Fun_blnEditTcpip, 5) && Fun_blnStepAction(Fun_blnLoadInitializeFile, 6) && Fun_blnStepAction(() => new Process
			{
				StartInfo = 
				{
					FileName = Path.Combine(m_Config.m_edcFilesPath.m_strPathErsasoft, "ErsaSoft.exe")
				}
			}.Start(), 7) && Fun_blnStepAction(delegate
			{
				Thread.Sleep(15000);
				return true;
			}, 8))
			{
				bool test1 = await Fun_blnEditErsasoftMesConfig();
				if (Fun_blnStepAction(() => test1, 9) && Fun_blnStepAction(() => true, 10))
				{
				}
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			Pro_edcSpsService.Sub_DisConnect();
			Pro_edcSpsService.Dispose();
		}
	}

	public async Task Fun_Test()
	{
		NetworkStream a_Stream = m_TcpClient.GetStream();
		if (!a_Stream.CanRead || !a_Stream.DataAvailable)
		{
		}
	}

	private void m_btnRestore_Click(object sender, EventArgs e)
	{
		if (Fun_blnKillErsasfot() && Fun_blnCopyDll05() && Fun_blnEditXmlCommunication(i_blnStart: false))
		{
		}
	}

	private bool Fun_blnStepAction(Func<bool> fun, int i_i32StepId)
	{
		bool? result = fun?.Invoke();
		Edc_olvAutoSetting cell = m_lstAutoSetting.Where((Edc_olvAutoSetting s) => s.m_i32Step == i_i32StepId).FirstOrDefault();
		if (result.HasValue && result == true)
		{
			cell.m_i32Status = 1;
			cell.m_strDatetime = (DateTime.Now - m_dtmBegin).TotalSeconds.ToString("f4");
			m_dtmBegin = DateTime.Now;
			m_olvOperating.RefreshObject(cell);
		}
		if (result.HasValue && result == false)
		{
			cell.m_i32Status = 2;
			m_olvOperating.RefreshObject(cell);
			return false;
		}
		return true;
	}

	private bool Fun_blnNeedAutoSearch()
	{
		if (string.IsNullOrEmpty(m_Config.m_edcFilesPath.m_strPathErsasoft))
		{
			return true;
		}
		if (string.IsNullOrEmpty(DirectoryFilesHelper.Fun_strGetErsasoftSettingFilePath(m_Config.m_edcFilesPath.m_strPathErsasoft)))
		{
			return true;
		}
		return false;
	}

	private bool Fun_blnAutoSearchErsasoftPath()
	{
		string a_strPathErsasoft = DirectoryFilesHelper.Fun_strGetErsasoftPath();
		if (string.IsNullOrEmpty(a_strPathErsasoft))
		{
			return false;
		}
		m_Config.m_edcFilesPath.m_strPathErsasoft = a_strPathErsasoft;
		Sub_GetPath(m_Config.m_edcFilesPath.m_strPathErsasoft);
		string a_strPathErsasoftIni = DirectoryFilesHelper.Fun_strGetErsasoftSettingFilePath(a_strPathErsasoft);
		if (!string.IsNullOrEmpty(a_strPathErsasoftIni))
		{
			m_Config.m_edcFilesPath.m_strPathProtocol = Path.Combine(a_strPathErsasoft, Edc_IniHelper.Fun_strRead("Protocol", "Directory", "", a_strPathErsasoftIni));
			m_Config.m_edcFilesPath.m_strPathTrend = Path.Combine(a_strPathErsasoft, Edc_IniHelper.Fun_strRead("Trend", "Directory", "", a_strPathErsasoftIni));
			m_Config.m_edcFilesPath.m_strPathZtxt = Path.Combine(a_strPathErsasoft, "Backup", "z.txt");
			m_Config.m_edcFilesPath.m_strPathBibs = Path.Combine(a_strPathErsasoft, Edc_IniHelper.Fun_strRead("Setup", "LoetprgPfad", "", a_strPathErsasoftIni));
			string a_strPathInitialize = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Initialize");
			if (Directory.Exists(a_strPathInitialize))
			{
				string[] files = Directory.GetFiles(a_strPathInitialize);
				if (files.Length != 0)
				{
					m_Config.m_edcFilesPath.m_strPathInitialize = files[0];
					Pro_strInitializeFullname = files[0];
				}
			}
			m_Config.m_edcFilesPath.m_strPathData = Path.Combine(a_strPathErsasoft, "Data");
		}
		return true;
	}

	private bool Fun_blnKillErsasfot()
	{
		if (ProcessHelper.Fun_blnProcessContain("ERSASOFT") && MessageBox.Show("Please make sure Ersasoft is not in production!!!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
		{
			return ProcessHelper.Fun_blnProcessKill("ERSASOFT");
		}
		return true;
	}

	private bool Fun_blnBackupDll()
	{
		if (string.IsNullOrEmpty(m_Config.m_edcFilesPath.m_strPathErsasoft))
		{
			MessageBox.Show("Please check the path of Ersasoft...");
			return false;
		}
		try
		{
			if (!File.Exists(m_strFullname05Dll))
			{
				MessageBox.Show("MES interface file not found, please configure MES manually...");
				return false;
			}
			if (File.Exists(m_strFilenameNew))
			{
				File.Delete(m_strFilenameNew);
			}
			if (File.Exists(m_strFilenameOld))
			{
				File.Move(m_strFilenameOld, m_strFilenameNew);
			}
			return true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return false;
	}

	private bool Fun_blnRestoreDll()
	{
		try
		{
			if (!File.Exists(m_strFullname05Dll))
			{
				MessageBox.Show("MES interface file not found, please configure MES manually...");
				return false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return false;
	}

	private bool Fun_blnCopyDll05()
	{
		try
		{
			File.Copy(m_strFullname05Dll, m_strFilenameOld, overwrite: true);
			return true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return false;
	}

	private bool Fun_blnCopyDll00()
	{
		try
		{
			File.Copy(m_strFullname00Dll, m_strFilenameOld, overwrite: true);
			return true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return false;
	}

	private bool Fun_blnEditXmlCommunication(bool i_blnStart)
	{
		string a_strCommunicationOld = "<Kommunikationsweg>PerDateien</Kommunikationsweg>";
		string a_strCommunicationNew = "<Kommunikationsweg>PerTcpip</Kommunikationsweg>";
		if (!File.Exists(m_strFullnameTrace05Config))
		{
			return false;
		}
		string a_strContext = File.ReadAllText(m_strFullnameTrace05Config);
		if (i_blnStart)
		{
			if (a_strContext.Contains(a_strCommunicationOld))
			{
				a_strContext = a_strContext.Replace(a_strCommunicationOld, a_strCommunicationNew);
			}
		}
		else if (a_strContext.Contains(a_strCommunicationNew))
		{
			a_strContext = a_strContext.Replace(a_strCommunicationNew, a_strCommunicationOld);
		}
		using (StreamWriter sw = new StreamWriter(m_strFullnameTrace05Config))
		{
			sw.Write(a_strContext);
		}
		return true;
	}

	private bool Fun_blnEditTcpip()
	{
		return Fun_blnEditXmlCommunication(i_blnStart: true);
	}

	private bool Fun_blnEditData()
	{
		return Fun_blnEditXmlCommunication(i_blnStart: false);
	}

	private bool Fun_blnLoadInitializeFile()
	{
		try
		{
			Struct_Initialize initializePLC = Pro_strInitializeFullname.Fun_edcDeserializeByFilePath<Struct_Initialize>();
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	private async Task<bool> Fun_blnEditErsasoftMesConfig()
	{
		try
		{
			if (Pro_edcSpsService == null)
			{
				Pro_edcSpsService = new Edc_BrPlc(m_edclogger);
			}
			await Pro_edcSpsService.Fun_ConnectAsync(i_blnOnline: true, "127.0.0.1");
			await Pro_edcSpsService.Sub_WriteMesAddress("bde_data.udtPara_VP");
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			MessageBox.Show(ex.Message);
			return false;
		}
		return true;
	}

	public bool Fun_blnSave(Edc_ConfigBase i_Config, bool i_blnShowMessagebox = true)
	{
		i_Config = m_Config;
		i_blnShowMessagebox = false;
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
		this.m_btnStart = new System.Windows.Forms.Button();
		this.m_btnRestore = new System.Windows.Forms.Button();
		this.m_olvOperating = new BrightIdeasSoftware.ObjectListView();
		((System.ComponentModel.ISupportInitialize)this.m_olvOperating).BeginInit();
		base.SuspendLayout();
		this.m_btnStart.Location = new System.Drawing.Point(4, 4);
		this.m_btnStart.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnStart.Name = "m_btnStart";
		this.m_btnStart.Size = new System.Drawing.Size(168, 46);
		this.m_btnStart.TabIndex = 16;
		this.m_btnStart.Text = "Start";
		this.m_btnStart.UseVisualStyleBackColor = true;
		this.m_btnStart.Click += new System.EventHandler(m_btnStart_Click);
		this.m_btnRestore.Location = new System.Drawing.Point(181, 4);
		this.m_btnRestore.Margin = new System.Windows.Forms.Padding(4);
		this.m_btnRestore.Name = "m_btnRestore";
		this.m_btnRestore.Size = new System.Drawing.Size(168, 46);
		this.m_btnRestore.TabIndex = 17;
		this.m_btnRestore.Text = "Restore";
		this.m_btnRestore.UseVisualStyleBackColor = true;
		this.m_btnRestore.Click += new System.EventHandler(m_btnRestore_Click);
		this.m_olvOperating.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
		this.m_olvOperating.CellEditUseWholeCell = false;
		this.m_olvOperating.Cursor = System.Windows.Forms.Cursors.Default;
		this.m_olvOperating.FullRowSelect = true;
		this.m_olvOperating.HideSelection = false;
		this.m_olvOperating.Location = new System.Drawing.Point(4, 57);
		this.m_olvOperating.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.m_olvOperating.Name = "m_olvOperating";
		this.m_olvOperating.ShowGroups = false;
		this.m_olvOperating.ShowImagesOnSubItems = true;
		this.m_olvOperating.Size = new System.Drawing.Size(1010, 680);
		this.m_olvOperating.TabIndex = 18;
		this.m_olvOperating.UseCompatibleStateImageBehavior = false;
		this.m_olvOperating.View = System.Windows.Forms.View.Details;
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 18f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.m_olvOperating);
		base.Controls.Add(this.m_btnRestore);
		base.Controls.Add(this.m_btnStart);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "UclErsasoft";
		base.Size = new System.Drawing.Size(1018, 740);
		((System.ComponentModel.ISupportInitialize)this.m_olvOperating).EndInit();
		base.ResumeLayout(false);
	}
}
