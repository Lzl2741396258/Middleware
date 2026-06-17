using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.Middleware.Helper;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.ConfigForm;

public class UclDevice : UserControl, Inf_ConfigForm
{
	private int m_i32IndexSerialport;

	private int m_i32IndexSocket;

	private IContainer components = null;

	private GroupBox m_gboxSerialPort;

	private ComboBox m_cbxStopBits1;

	private Label label5;

	private TextBox m_txtDataBits1;

	private Label label4;

	private ComboBox m_cbxParity1;

	private Label label3;

	private ComboBox m_cbxRate1;

	private Label label1;

	private ComboBox m_cbxPort1;

	private Label label13;

	private CheckBox m_chkDtrEnable1;

	private CheckBox m_chkRtsEnable1;

	private GroupBox m_gboxSocket;

	private TextBox m_txtIP;

	private TextBox m_txtPort;

	private Label label2;

	private Label label6;

	private Label label7;

	private ComboBox m_cbxSerialportIndex;

	private Label label8;

	private ComboBox m_cbxNetworktIndex;

	private Button m_btnMinusSerialport;

	private Button m_btnAddSerialport;

	private Button m_btnMinusNetwork;

	private Button m_btnAddNetwork;

	private GroupBox groupBox2;

	private TextBox m_txtCameraNoRead;

	private Label label12;

	private CheckBox m_chkServer;

	private Label label9;

	private ComboBox m_cbxTrack;

	private TextBox m_txtRemarkSp;

	private Label label10;

	private TextBox m_txtRemarkNp;

	private Label label11;

	public int m_i32FormID { get; set; } = 405;


	public string Pro_FormName => Enum_ConfigForm.Device.ToString();

	private Enum_SignalType m_enuSignalType { get; set; } = Enum_SignalType.Default;


	private Edc_Device m_edcInputDevice { get; set; }

	private List<Edc_ConfigSerialPort> m_lstSerialport { get; set; }

	private List<Edc_Socket> m_lstSocket { get; set; }

	public UclDevice(Edc_ConfigBase i_ConfigBase)
	{
		InitializeComponent();
		m_edcInputDevice = i_ConfigBase.m_clsDevice;
		if (m_edcInputDevice == null)
		{
			m_edcInputDevice = new Edc_Device
			{
				m_blnHearderEndActive = false,
				m_lstSerialPorts = new List<Edc_ConfigSerialPort>(),
				m_lstSockets = new List<Edc_Socket>(),
				m_lstProcess = new List<Edc_Process>(),
				m_strCameraNoRead = "NoRead"
			};
		}
		m_lstSerialport = m_edcInputDevice.m_lstSerialPorts;
		m_lstSocket = m_edcInputDevice.m_lstSockets;
	}

	private void UclInputDevice_Load(object sender, EventArgs e)
	{
		Sub_Initialize();
		m_txtCameraNoRead.Text = m_edcInputDevice.m_strCameraNoRead.ToString();
	}

	public void Sub_Initialize()
	{
		Sub_DefaultSerialport();
		Sub_DefaultSocket();
		Sub_InitializeIndex();
	}

	private void Sub_InitializeIndex()
	{
		int a_i32Count = m_lstSerialport.Count;
		Sub_BindIndex(a_i32Count, m_cbxSerialportIndex);
		m_cbxSerialportIndex.SelectedIndex = ((a_i32Count == 0) ? (-1) : 0);
		a_i32Count = m_lstSocket.Count;
		Sub_BindIndex(a_i32Count, m_cbxNetworktIndex);
		m_cbxNetworktIndex.SelectedIndex = ((a_i32Count == 0) ? (-1) : 0);
	}

	private void Sub_BindIndex(int i_i32Count, ComboBox i_edcCombobox)
	{
		i_edcCombobox.Items.Clear();
		if (i_i32Count != 0)
		{
			object[] a_i32Items = new object[i_i32Count];
			for (int i = 0; i < i_i32Count; i++)
			{
				a_i32Items[i] = i + 1;
			}
			i_edcCombobox.Items.AddRange(a_i32Items);
		}
	}

	public bool Fun_blnSave(Edc_ConfigBase i_Config, bool i_blnShowMessagebox = true)
	{
		try
		{
			if (m_i32IndexSerialport >= 0 && m_edcInputDevice.m_lstSerialPorts.Count != 0)
			{
				m_edcInputDevice.m_lstSerialPorts[m_i32IndexSerialport].m_blnActive = true;
				m_edcInputDevice.m_lstSerialPorts[m_i32IndexSerialport].m_strPortName = m_cbxPort1.Text;
				int a_i32Default = 0;
				int.TryParse(m_cbxRate1.Text, out a_i32Default);
				m_edcInputDevice.m_lstSerialPorts[m_i32IndexSerialport].m_i32Rate = a_i32Default;
				Parity a_edcParity = Parity.None;
				m_edcInputDevice.m_lstSerialPorts[m_i32IndexSerialport].m_edcParity = (Parity)(Enum.Parse(typeof(Parity), m_cbxParity1.Text) ?? ((object)a_edcParity));
				a_i32Default = 0;
				int.TryParse(m_txtDataBits1.Text, out a_i32Default);
				m_edcInputDevice.m_lstSerialPorts[m_i32IndexSerialport].m_i32DataBits = a_i32Default;
				StopBits a_edcStopBits = StopBits.One;
				m_edcInputDevice.m_lstSerialPorts[m_i32IndexSerialport].m_strStopBits = (StopBits)(Enum.Parse(typeof(StopBits), m_cbxStopBits1.Text) ?? ((object)a_edcStopBits));
				m_edcInputDevice.m_lstSerialPorts[m_i32IndexSerialport].m_blnDtrEnable = m_chkDtrEnable1.Checked;
				m_edcInputDevice.m_lstSerialPorts[m_i32IndexSerialport].m_blnRtsEnable = m_chkRtsEnable1.Checked;
				ushort a_uDefault = 1;
				ushort.TryParse(m_cbxTrack.Text, out a_uDefault);
				m_edcInputDevice.m_lstSerialPorts[m_i32IndexSerialport].m_i32TrackId = a_uDefault;
				m_edcInputDevice.m_lstSerialPorts[m_i32IndexSerialport].m_strRemark = m_txtRemarkSp.Text.Trim();
			}
			if (m_i32IndexSocket >= 0 && m_edcInputDevice.m_lstSockets.Count != 0)
			{
				m_edcInputDevice.m_lstSockets[m_i32IndexSocket].m_blnActive = true;
				m_edcInputDevice.m_lstSockets[m_i32IndexSocket].m_blnServer = m_chkServer.Checked;
				m_edcInputDevice.m_lstSockets[m_i32IndexSocket].m_strIP = m_txtIP.Text;
				m_edcInputDevice.m_lstSockets[m_i32IndexSocket].m_strRemark = m_txtRemarkNp.Text;
				int result = 0;
				int.TryParse(m_txtPort.Text, out result);
				m_edcInputDevice.m_lstSockets[m_i32IndexSocket].m_i32Port = result;
			}
			m_edcInputDevice.m_strCameraNoRead = m_txtCameraNoRead.Text;
			i_Config.m_clsDevice = m_edcInputDevice;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			return false;
		}
		return true;
	}

	private void m_cbxSerialportIndex_SelectedIndexChanged(object sender, EventArgs e)
	{
		m_i32IndexSerialport = m_cbxSerialportIndex.SelectedIndex;
		Edc_ConfigSerialPort a_edcSerialport = m_edcInputDevice.m_lstSerialPorts[m_i32IndexSerialport];
		if (m_i32IndexSerialport < 0)
		{
			Sub_DefaultSerialport();
			return;
		}
		m_cbxPort1.SelectedIndex = m_cbxPort1.FindString(a_edcSerialport.m_strPortName);
		m_cbxRate1.SelectedIndex = m_cbxRate1.FindString(a_edcSerialport.m_i32Rate.ToString());
		m_cbxParity1.SelectedIndex = m_cbxParity1.FindString(a_edcSerialport.m_edcParity.ToString());
		m_txtDataBits1.Text = a_edcSerialport.m_i32DataBits.ToString();
		m_cbxStopBits1.SelectedIndex = m_cbxStopBits1.FindString(a_edcSerialport.m_strStopBits.ToString());
		m_chkDtrEnable1.Checked = a_edcSerialport.m_blnDtrEnable;
		m_chkRtsEnable1.Checked = a_edcSerialport.m_blnRtsEnable;
		m_txtRemarkSp.Text = a_edcSerialport.m_strRemark;
	}

	private void m_cbxNetworktIndex_SelectedIndexChanged(object sender, EventArgs e)
	{
		m_i32IndexSocket = m_cbxNetworktIndex.SelectedIndex;
		Edc_Socket network = m_edcInputDevice.m_lstSockets[m_i32IndexSocket];
		if (m_i32IndexSocket < 0)
		{
			Sub_DefaultSocket();
			return;
		}
		m_txtIP.Text = network.m_strIP;
		m_txtPort.Text = network.m_i32Port.ToString();
		m_chkServer.Checked = network.m_blnServer;
		m_txtRemarkNp.Text = network.m_strRemark;
	}

	private void m_btnAddSerialport_Click(object sender, EventArgs e)
	{
		m_lstSerialport.Add(new Edc_ConfigSerialPort
		{
			m_i32Rate = 9600,
			m_edcParity = Parity.None,
			m_i32DataBits = 8,
			m_strStopBits = StopBits.One,
			m_blnDtrEnable = false,
			m_blnRtsEnable = false,
			m_i32TrackId = 1,
			m_blnActive = true
		});
		Sub_BindIndex(m_lstSerialport.Count, m_cbxSerialportIndex);
		Sub_DefaultSerialport();
		m_cbxSerialportIndex.SelectedIndex = m_lstSerialport.Count - 1;
	}

	private void m_btnMinusSerialport_Click(object sender, EventArgs e)
	{
		m_lstSerialport.RemoveAt(m_cbxSerialportIndex.SelectedIndex);
		Sub_BindIndex(m_lstSerialport.Count, m_cbxSerialportIndex);
		Sub_DefaultSerialport();
		m_cbxSerialportIndex.SelectedIndex = m_lstSerialport.Count - 1;
	}

	private void m_btnAddNetwork_Click(object sender, EventArgs e)
	{
		m_lstSocket.Add(new Edc_Socket
		{
			m_blnActive = true
		});
		Sub_BindIndex(m_lstSocket.Count, m_cbxNetworktIndex);
		Sub_DefaultSocket();
		m_cbxNetworktIndex.SelectedIndex = m_lstSocket.Count - 1;
	}

	private void m_btnMinusNetwork_Click(object sender, EventArgs e)
	{
		m_lstSocket.RemoveAt(m_cbxNetworktIndex.SelectedIndex);
		Sub_BindIndex(m_lstSocket.Count, m_cbxNetworktIndex);
		Sub_DefaultSocket();
		m_cbxNetworktIndex.SelectedIndex = m_lstSocket.Count - 1;
	}

	private void Sub_DefaultSerialport(string i_strPort = "", string i_strRate = "9600", Parity i_edcParity = Parity.None, string i_strDataBits = "8", StopBits i_edcStopBits = StopBits.One, bool i_blnDtrEnable = false, bool i_blnRtsEnable = false, string i_strRemark = "")
	{
		m_cbxPort1.Items.Clear();
		ComboBox.ObjectCollection items = m_cbxPort1.Items;
		object[] portNames = SerialPort.GetPortNames();
		items.AddRange(portNames);
		m_cbxPort1.Text = i_strPort;
		m_cbxRate1.Items.Clear();
		ComboBox.ObjectCollection items2 = m_cbxRate1.Items;
		portNames = SerialportHelper.Fun_strGetBaudRate();
		items2.AddRange(portNames);
		m_cbxRate1.SelectedIndex = m_cbxRate1.FindStringExact(i_strRate);
		m_cbxParity1.Items.Clear();
		ComboBox.ObjectCollection items3 = m_cbxParity1.Items;
		portNames = Enum.GetNames(typeof(Parity));
		items3.AddRange(portNames);
		m_cbxParity1.SelectedItem = i_edcParity;
		m_txtDataBits1.Text = i_strDataBits;
		m_cbxStopBits1.Items.Clear();
		ComboBox.ObjectCollection items4 = m_cbxStopBits1.Items;
		portNames = Enum.GetNames(typeof(StopBits));
		items4.AddRange(portNames);
		m_cbxStopBits1.SelectedItem = i_edcStopBits;
		m_cbxTrack.SelectedIndex = 0;
		m_chkDtrEnable1.Checked = i_blnDtrEnable;
		m_chkRtsEnable1.Checked = i_blnRtsEnable;
		m_txtRemarkSp.Text = i_strRemark;
	}

	private void Sub_DefaultSocket()
	{
		m_txtIP.Text = string.Empty;
		m_txtPort.Text = string.Empty;
		m_txtRemarkNp.Text = string.Empty;
		m_chkServer.Checked = false;
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
		this.m_gboxSerialPort = new System.Windows.Forms.GroupBox();
		this.m_txtRemarkSp = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.m_cbxTrack = new System.Windows.Forms.ComboBox();
		this.label9 = new System.Windows.Forms.Label();
		this.m_btnMinusSerialport = new System.Windows.Forms.Button();
		this.m_btnAddSerialport = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.m_cbxSerialportIndex = new System.Windows.Forms.ComboBox();
		this.m_chkRtsEnable1 = new System.Windows.Forms.CheckBox();
		this.m_chkDtrEnable1 = new System.Windows.Forms.CheckBox();
		this.m_cbxStopBits1 = new System.Windows.Forms.ComboBox();
		this.label5 = new System.Windows.Forms.Label();
		this.m_txtDataBits1 = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.m_cbxParity1 = new System.Windows.Forms.ComboBox();
		this.label3 = new System.Windows.Forms.Label();
		this.m_cbxRate1 = new System.Windows.Forms.ComboBox();
		this.label1 = new System.Windows.Forms.Label();
		this.m_cbxPort1 = new System.Windows.Forms.ComboBox();
		this.label13 = new System.Windows.Forms.Label();
		this.m_gboxSocket = new System.Windows.Forms.GroupBox();
		this.m_chkServer = new System.Windows.Forms.CheckBox();
		this.m_btnMinusNetwork = new System.Windows.Forms.Button();
		this.m_btnAddNetwork = new System.Windows.Forms.Button();
		this.label8 = new System.Windows.Forms.Label();
		this.m_cbxNetworktIndex = new System.Windows.Forms.ComboBox();
		this.m_txtIP = new System.Windows.Forms.TextBox();
		this.m_txtPort = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.m_txtCameraNoRead = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.m_txtRemarkNp = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.m_gboxSerialPort.SuspendLayout();
		this.m_gboxSocket.SuspendLayout();
		this.groupBox2.SuspendLayout();
		base.SuspendLayout();
		this.m_gboxSerialPort.Controls.Add(this.m_txtRemarkSp);
		this.m_gboxSerialPort.Controls.Add(this.label10);
		this.m_gboxSerialPort.Controls.Add(this.m_cbxTrack);
		this.m_gboxSerialPort.Controls.Add(this.label9);
		this.m_gboxSerialPort.Controls.Add(this.m_btnMinusSerialport);
		this.m_gboxSerialPort.Controls.Add(this.m_btnAddSerialport);
		this.m_gboxSerialPort.Controls.Add(this.label7);
		this.m_gboxSerialPort.Controls.Add(this.m_cbxSerialportIndex);
		this.m_gboxSerialPort.Controls.Add(this.m_chkRtsEnable1);
		this.m_gboxSerialPort.Controls.Add(this.m_chkDtrEnable1);
		this.m_gboxSerialPort.Controls.Add(this.m_cbxStopBits1);
		this.m_gboxSerialPort.Controls.Add(this.label5);
		this.m_gboxSerialPort.Controls.Add(this.m_txtDataBits1);
		this.m_gboxSerialPort.Controls.Add(this.label4);
		this.m_gboxSerialPort.Controls.Add(this.m_cbxParity1);
		this.m_gboxSerialPort.Controls.Add(this.label3);
		this.m_gboxSerialPort.Controls.Add(this.m_cbxRate1);
		this.m_gboxSerialPort.Controls.Add(this.label1);
		this.m_gboxSerialPort.Controls.Add(this.m_cbxPort1);
		this.m_gboxSerialPort.Controls.Add(this.label13);
		this.m_gboxSerialPort.Dock = System.Windows.Forms.DockStyle.Top;
		this.m_gboxSerialPort.Font = new System.Drawing.Font("微软雅黑", 9f);
		this.m_gboxSerialPort.Location = new System.Drawing.Point(5, 5);
		this.m_gboxSerialPort.Name = "m_gboxSerialPort";
		this.m_gboxSerialPort.Size = new System.Drawing.Size(679, 205);
		this.m_gboxSerialPort.TabIndex = 50;
		this.m_gboxSerialPort.TabStop = false;
		this.m_gboxSerialPort.Text = "SerialPort";
		this.m_txtRemarkSp.Location = new System.Drawing.Point(300, 160);
		this.m_txtRemarkSp.Margin = new System.Windows.Forms.Padding(2);
		this.m_txtRemarkSp.Name = "m_txtRemarkSp";
		this.m_txtRemarkSp.Size = new System.Drawing.Size(121, 23);
		this.m_txtRemarkSp.TabIndex = 22;
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(244, 163);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(53, 17);
		this.label10.TabIndex = 23;
		this.label10.Text = "Remark";
		this.m_cbxTrack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxTrack.FormattingEnabled = true;
		this.m_cbxTrack.Items.AddRange(new object[4] { "1", "2", "3", "4" });
		this.m_cbxTrack.Location = new System.Drawing.Point(79, 160);
		this.m_cbxTrack.Name = "m_cbxTrack";
		this.m_cbxTrack.Size = new System.Drawing.Size(121, 25);
		this.m_cbxTrack.TabIndex = 21;
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(33, 163);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(40, 17);
		this.label9.TabIndex = 20;
		this.label9.Text = "Track";
		this.m_btnMinusSerialport.Location = new System.Drawing.Point(236, 37);
		this.m_btnMinusSerialport.Name = "m_btnMinusSerialport";
		this.m_btnMinusSerialport.Size = new System.Drawing.Size(25, 25);
		this.m_btnMinusSerialport.TabIndex = 18;
		this.m_btnMinusSerialport.Text = "-";
		this.m_btnMinusSerialport.UseVisualStyleBackColor = true;
		this.m_btnMinusSerialport.Click += new System.EventHandler(m_btnMinusSerialport_Click);
		this.m_btnAddSerialport.Location = new System.Drawing.Point(205, 37);
		this.m_btnAddSerialport.Name = "m_btnAddSerialport";
		this.m_btnAddSerialport.Size = new System.Drawing.Size(25, 25);
		this.m_btnAddSerialport.TabIndex = 17;
		this.m_btnAddSerialport.Text = "+";
		this.m_btnAddSerialport.UseVisualStyleBackColor = true;
		this.m_btnAddSerialport.Click += new System.EventHandler(m_btnAddSerialport_Click);
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(32, 40);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(40, 17);
		this.label7.TabIndex = 16;
		this.label7.Text = "Index";
		this.m_cbxSerialportIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxSerialportIndex.FormattingEnabled = true;
		this.m_cbxSerialportIndex.Location = new System.Drawing.Point(78, 37);
		this.m_cbxSerialportIndex.Name = "m_cbxSerialportIndex";
		this.m_cbxSerialportIndex.Size = new System.Drawing.Size(121, 25);
		this.m_cbxSerialportIndex.TabIndex = 14;
		this.m_cbxSerialportIndex.SelectedIndexChanged += new System.EventHandler(m_cbxSerialportIndex_SelectedIndexChanged);
		this.m_chkRtsEnable1.AutoSize = true;
		this.m_chkRtsEnable1.Location = new System.Drawing.Point(558, 161);
		this.m_chkRtsEnable1.Name = "m_chkRtsEnable1";
		this.m_chkRtsEnable1.Size = new System.Drawing.Size(84, 21);
		this.m_chkRtsEnable1.TabIndex = 13;
		this.m_chkRtsEnable1.Text = "RtsEnable";
		this.m_chkRtsEnable1.UseVisualStyleBackColor = true;
		this.m_chkDtrEnable1.AutoSize = true;
		this.m_chkDtrEnable1.Location = new System.Drawing.Point(447, 161);
		this.m_chkDtrEnable1.Name = "m_chkDtrEnable1";
		this.m_chkDtrEnable1.Size = new System.Drawing.Size(88, 21);
		this.m_chkDtrEnable1.TabIndex = 12;
		this.m_chkDtrEnable1.Text = "DtrEnable ";
		this.m_chkDtrEnable1.UseVisualStyleBackColor = true;
		this.m_cbxStopBits1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxStopBits1.FormattingEnabled = true;
		this.m_cbxStopBits1.Location = new System.Drawing.Point(298, 113);
		this.m_cbxStopBits1.Name = "m_cbxStopBits1";
		this.m_cbxStopBits1.Size = new System.Drawing.Size(121, 25);
		this.m_cbxStopBits1.TabIndex = 11;
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(240, 115);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(56, 17);
		this.label5.TabIndex = 9;
		this.label5.Text = "StopBits";
		this.m_txtDataBits1.Location = new System.Drawing.Point(78, 112);
		this.m_txtDataBits1.Margin = new System.Windows.Forms.Padding(2);
		this.m_txtDataBits1.Name = "m_txtDataBits1";
		this.m_txtDataBits1.Size = new System.Drawing.Size(121, 23);
		this.m_txtDataBits1.TabIndex = 7;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(18, 115);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(56, 17);
		this.label4.TabIndex = 8;
		this.label4.Text = "DataBits";
		this.m_cbxParity1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxParity1.FormattingEnabled = true;
		this.m_cbxParity1.Location = new System.Drawing.Point(518, 82);
		this.m_cbxParity1.Name = "m_cbxParity1";
		this.m_cbxParity1.Size = new System.Drawing.Size(121, 25);
		this.m_cbxParity1.TabIndex = 7;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(472, 85);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(40, 17);
		this.label3.TabIndex = 6;
		this.label3.Text = "Parity";
		this.m_cbxRate1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxRate1.FormattingEnabled = true;
		this.m_cbxRate1.Location = new System.Drawing.Point(298, 82);
		this.m_cbxRate1.Name = "m_cbxRate1";
		this.m_cbxRate1.Size = new System.Drawing.Size(121, 25);
		this.m_cbxRate1.TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(262, 85);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(32, 17);
		this.label1.TabIndex = 2;
		this.label1.Text = "Port";
		this.m_cbxPort1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxPort1.FormattingEnabled = true;
		this.m_cbxPort1.Location = new System.Drawing.Point(78, 82);
		this.m_cbxPort1.Name = "m_cbxPort1";
		this.m_cbxPort1.Size = new System.Drawing.Size(121, 25);
		this.m_cbxPort1.TabIndex = 1;
		this.label13.AutoSize = true;
		this.label13.Location = new System.Drawing.Point(37, 85);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(35, 17);
		this.label13.TabIndex = 0;
		this.label13.Text = "Com";
		this.m_gboxSocket.Controls.Add(this.m_txtRemarkNp);
		this.m_gboxSocket.Controls.Add(this.label11);
		this.m_gboxSocket.Controls.Add(this.m_chkServer);
		this.m_gboxSocket.Controls.Add(this.m_btnMinusNetwork);
		this.m_gboxSocket.Controls.Add(this.m_btnAddNetwork);
		this.m_gboxSocket.Controls.Add(this.label8);
		this.m_gboxSocket.Controls.Add(this.m_cbxNetworktIndex);
		this.m_gboxSocket.Controls.Add(this.m_txtIP);
		this.m_gboxSocket.Controls.Add(this.m_txtPort);
		this.m_gboxSocket.Controls.Add(this.label2);
		this.m_gboxSocket.Controls.Add(this.label6);
		this.m_gboxSocket.Dock = System.Windows.Forms.DockStyle.Top;
		this.m_gboxSocket.Font = new System.Drawing.Font("微软雅黑", 9f);
		this.m_gboxSocket.Location = new System.Drawing.Point(5, 210);
		this.m_gboxSocket.Name = "m_gboxSocket";
		this.m_gboxSocket.Size = new System.Drawing.Size(679, 122);
		this.m_gboxSocket.TabIndex = 54;
		this.m_gboxSocket.TabStop = false;
		this.m_gboxSocket.Text = "NetworkPort";
		this.m_chkServer.AutoSize = true;
		this.m_chkServer.Location = new System.Drawing.Point(447, 39);
		this.m_chkServer.Name = "m_chkServer";
		this.m_chkServer.Size = new System.Drawing.Size(74, 21);
		this.m_chkServer.TabIndex = 22;
		this.m_chkServer.Text = "IsServer";
		this.m_chkServer.UseVisualStyleBackColor = true;
		this.m_btnMinusNetwork.Location = new System.Drawing.Point(236, 37);
		this.m_btnMinusNetwork.Name = "m_btnMinusNetwork";
		this.m_btnMinusNetwork.Size = new System.Drawing.Size(25, 25);
		this.m_btnMinusNetwork.TabIndex = 21;
		this.m_btnMinusNetwork.Text = "-";
		this.m_btnMinusNetwork.UseVisualStyleBackColor = true;
		this.m_btnMinusNetwork.Click += new System.EventHandler(m_btnMinusNetwork_Click);
		this.m_btnAddNetwork.Location = new System.Drawing.Point(205, 37);
		this.m_btnAddNetwork.Name = "m_btnAddNetwork";
		this.m_btnAddNetwork.Size = new System.Drawing.Size(25, 25);
		this.m_btnAddNetwork.TabIndex = 20;
		this.m_btnAddNetwork.Text = "+";
		this.m_btnAddNetwork.UseVisualStyleBackColor = true;
		this.m_btnAddNetwork.Click += new System.EventHandler(m_btnAddNetwork_Click);
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(32, 40);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(40, 17);
		this.label8.TabIndex = 19;
		this.label8.Text = "Index";
		this.m_cbxNetworktIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.m_cbxNetworktIndex.FormattingEnabled = true;
		this.m_cbxNetworktIndex.Location = new System.Drawing.Point(78, 37);
		this.m_cbxNetworktIndex.Name = "m_cbxNetworktIndex";
		this.m_cbxNetworktIndex.Size = new System.Drawing.Size(121, 25);
		this.m_cbxNetworktIndex.TabIndex = 17;
		this.m_cbxNetworktIndex.SelectedIndexChanged += new System.EventHandler(m_cbxNetworktIndex_SelectedIndexChanged);
		this.m_txtIP.Location = new System.Drawing.Point(78, 78);
		this.m_txtIP.Margin = new System.Windows.Forms.Padding(2);
		this.m_txtIP.Name = "m_txtIP";
		this.m_txtIP.Size = new System.Drawing.Size(121, 23);
		this.m_txtIP.TabIndex = 6;
		this.m_txtIP.Text = "127.0.0.1";
		this.m_txtPort.Location = new System.Drawing.Point(298, 77);
		this.m_txtPort.Margin = new System.Windows.Forms.Padding(2);
		this.m_txtPort.Name = "m_txtPort";
		this.m_txtPort.Size = new System.Drawing.Size(123, 23);
		this.m_txtPort.TabIndex = 5;
		this.m_txtPort.Text = "12121";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(263, 80);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(32, 17);
		this.label2.TabIndex = 2;
		this.label2.Text = "Port";
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(45, 80);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(23, 17);
		this.label6.TabIndex = 0;
		this.label6.Text = "Ip:";
		this.groupBox2.Controls.Add(this.m_txtCameraNoRead);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox2.Location = new System.Drawing.Point(5, 332);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(679, 100);
		this.groupBox2.TabIndex = 58;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Scanner Info";
		this.m_txtCameraNoRead.Location = new System.Drawing.Point(78, 45);
		this.m_txtCameraNoRead.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
		this.m_txtCameraNoRead.Name = "m_txtCameraNoRead";
		this.m_txtCameraNoRead.Size = new System.Drawing.Size(204, 23);
		this.m_txtCameraNoRead.TabIndex = 19;
		this.label12.AutoSize = true;
		this.label12.Location = new System.Drawing.Point(15, 48);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(56, 17);
		this.label12.TabIndex = 20;
		this.label12.Text = "NoRead";
		this.m_txtRemarkNp.Location = new System.Drawing.Point(518, 74);
		this.m_txtRemarkNp.Margin = new System.Windows.Forms.Padding(2);
		this.m_txtRemarkNp.Name = "m_txtRemarkNp";
		this.m_txtRemarkNp.Size = new System.Drawing.Size(121, 23);
		this.m_txtRemarkNp.TabIndex = 24;
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(462, 77);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(53, 17);
		this.label11.TabIndex = 25;
		this.label11.Text = "Remark";
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.m_gboxSocket);
		base.Controls.Add(this.m_gboxSerialPort);
		this.Font = new System.Drawing.Font("微软雅黑", 9f);
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.Name = "UclDevice";
		base.Padding = new System.Windows.Forms.Padding(5);
		base.Size = new System.Drawing.Size(689, 439);
		base.Load += new System.EventHandler(UclInputDevice_Load);
		this.m_gboxSerialPort.ResumeLayout(false);
		this.m_gboxSerialPort.PerformLayout();
		this.m_gboxSocket.ResumeLayout(false);
		this.m_gboxSocket.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		base.ResumeLayout(false);
	}
}
