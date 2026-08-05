using Ersa.Mes.Common;
using Ersa.Mes.FileSystem.Model;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Model;
using MesXPT.CommunicationService;
using MesXPT.Model;
using MesXPT.XPT_MesTask;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MesXPT;

public class FrmSettingPlatform : Form
{
    private bool m_openPortEnabled = true;

    public string m_strCurrentCode = "";

    private string m_recvPort = "";

    private IContainer components = null;

    private ToolStrip toolStrip1;

    private ToolStripButton m_tsbtnExit;

    private ToolStripButton m_tsbtnSave;

    private ToolStripButton m_tsbtnSerialConfig;

    private StatusStrip statusStrip1;

    private System.Windows.Forms.Timer timer1;

    private SerialPort serialPort1;

    // 串口连接状态指示灯（放在顶部 toolStrip，紧挨"串口配置"按钮）
    private Panel pnlSerialStatusLight;
    private Label lblSerialStatusText;
    private FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
    private TextBox tbErsaOnlineUrl;
    private Label lbErsaOnlineURL;
    private CheckBox cbErsaDownline;
    private Label lbErsaDownlineURL;
    private TextBox tbErsaDownlineUrl;
    private Label label6;
    private Button btTest;
    private GroupBox groupBox4;
    private TextBox m_MachineCode;
    private Label MachineCode;
    private CheckBox cbByPass;

    private XPT_Config m_Config { get; set; }

    private Inf_Logger m_edcLogger { get; set; }

    private Edc_XPTCommunicationService m_edcService { get; set; }

    private Edc_Account m_edcAccount { get; set; }

    private static readonly HttpClient _httpClient = new HttpClient();

    public FrmSettingPlatform(XPT_Config i_Config, Inf_Logger i_edcLogger, Edc_Account i_edcAccount)
    {
        InitializeComponent();
        // 复用全局共享串口（FrmMain 启动时已打开，避免重复创建）
        // 必须在 InitializeComponent 之后赋值，否则会被 designer 里的 new SerialPort 覆盖
        serialPort1 = XPT_Data.SharedSerialPort;

        m_Config = i_Config;
        m_edcLogger = i_edcLogger;
        m_edcAccount = i_edcAccount;
        m_edcService = new Edc_XPTCommunicationService(i_Config, i_edcLogger);
        // 初始化HttpClient默认设置
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private void FrmSettingTcpip_Load(object sender, EventArgs e)
    {
        // Set Add ComboBox for SerialPorts
        //ComboBox.ObjectCollection items = m_comPort.Items;
        //object[] portNames = SerialPort.GetPortNames();
      //  items.AddRange(portNames);

        // Timer
        timer1.Interval = 200;
        timer1.Start();

        // Set value for each textbox.
        m_MachineCode.Text = m_Config.m_RecipeName;
        tbErsaDownlineUrl.Text = m_Config.m_ersaDownLineUrl;

        if (bool.TryParse(m_Config.m_checkCompeletResult, out bool compeletresult))
        {
            cbErsaDownline.Checked = compeletresult;
        }
        else
        {
            cbErsaDownline.Checked = false;
        }
        if (bool.TryParse(m_Config.m_byPass, out bool byPass))
        {
            cbByPass.Checked = byPass;
            if (byPass)
            {
                XPT_Data.m_byPass = true;
            }
            else
            {
                XPT_Data.m_byPass = false;
            }
        }
        else
        {
            cbByPass.Checked = false;
            XPT_Data.m_byPass = false;
        }

        tbErsaOnlineUrl.Text = m_Config.m_ersaOnlineUrl;
        base.Size = new Size
        {
            Height = 400,
            Width = 750
        };

        // 订阅串口错误事件 + 初始化指示灯状态
        serialPort1.ErrorReceived += serialPort1_ErrorReceived;
        UpdateSerialPortStatus();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            m_Config.m_RecipeName = m_MachineCode.Text.Trim();
            m_Config.m_checkCompeletResult = cbErsaDownline.Checked.ToString();
            m_Config.m_ersaDownLineUrl = tbErsaDownlineUrl.Text.Trim();
            m_Config.m_ersaOnlineUrl = tbErsaOnlineUrl.Text.Trim();
            m_Config.m_byPass = cbByPass.Checked.ToString();
            if (m_Config.m_byPass == "True")
            {
                XPT_Data.m_byPass = true;
            }
            else
            {
                XPT_Data.m_byPass = false;
            }
            XPT_Data.m_strProgram = m_Config.m_programFilePath;

            // 串口配置由弹窗内已临时写入 m_Config，这里统一落地
            // m_Config.m_strcomPort / m_strBaudRate / m_strDataBits / m_strStopBits
            // m_Config.m_strParity / m_strPrefix / m_strSuffix / m_strTriggerChar

            XPT_Data.m_Config = m_Config;
            bool result = Edc_OperationConfig.Fun_WriteConfig<XPT_Config>(m_Config.m_edcFilesPath.m_strPathConfig, m_Config);
            MessageBox.Show($"Save Config File {result}...");

        }
        catch (Exception ex)
        {
            MessageBox.Show("Save Config File Error...Result'" + ex.Message + "'");
            m_edcLogger.Error("Save Config Error...Function'" + MethodBase.GetCurrentMethod().Name + "'...Message'" + ex.Message + "'", null, "btnSave_Click", 122);
        }
    }

    private void m_tsbtnExit_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void m_tsbtnSerialConfig_Click(object sender, EventArgs e)
    {
        try
        {
            using (FrmSerialPortConfig frm = new FrmSerialPortConfig(m_Config, serialPort1, m_edcLogger, UpdateSerialPortStatus))
            {
                frm.ShowDialog(this);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("打开串口配置失败: " + ex.Message);
        }
    }

    private void m_btnSelectProgram_Click(object sender, EventArgs e)
    {

        DialogResult dr = folderBrowserDialog.ShowDialog();

        if (dr.ToString() == "OK")
        {
            string filePath = folderBrowserDialog.SelectedPath;
            XPT_Data.m_strLibrary = filePath;
            string[] programFiles = Directory.GetFiles(filePath);
            if (programFiles.Length > 0)
            {
                // 路劲下最新的那个程序文本
                string programFile = programFiles[0];
                XPT_Data.m_strProgram = Path.GetFileName(programFile);
            }
        }

        //   XPT_Data.m_strProgram = m_txtProgram.Text.Trim();
        if (MessageBox.Show("Confirm select program " + XPT_Data.m_strLibrary + "\\" + XPT_Data.m_strProgram, "Confirm", MessageBoxButtons.OKCancel) == DialogResult.OK)
        {
            XPT_Data.m_blnActiveSelectProgram = true;
        }
    }

    /*    private void m_btnPopupDialog_Click(object sender, EventArgs e)
        {
            XPT_Data.m_strAlarmMessage = m_txtAlarmMessage.Text.Trim();
            XPT_Data.m_blnActivePopupDialog = true;
        }*/

    private void button1_Click(object sender, EventArgs e)
    {
        Edc_XPTCommunicationService edc_XPTCommunicationService = new Edc_XPTCommunicationService(m_Config, m_edcLogger);
    }

    private void timer1_Tick(object sender, EventArgs e)
    {

    }

    private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {

    }

    private void FrmSettingPlatform_FormClosed(object sender, FormClosedEventArgs e)
    {
        // 取消 ErrorReceived 订阅，避免多次打开 FrmSettingPlatform 后重复触发指示灯刷新
        if (serialPort1 != null)
        {
            serialPort1.ErrorReceived -= serialPort1_ErrorReceived;
        }
        // 共享串口由 FrmMain 生命周期管理，此处不再关闭
    }

    /// <summary>
    /// 窗体被激活时刷新一次指示灯（处理从串口配置弹窗返回等场景的兜底刷新）。
    /// </summary>
    private void FrmSettingPlatform_Activated(object sender, EventArgs e)
    {
        UpdateSerialPortStatus();
    }

    /// <summary>
    /// 串口底层错误（断线、溢出等）时刷新指示灯。
    /// </summary>
    private void serialPort1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new SerialErrorReceivedEventHandler(serialPort1_ErrorReceived), sender, e);
            return;
        }
        m_edcLogger?.Error("SerialPort ErrorReceived: " + e.EventType, null, "serialPort1_ErrorReceived", 0);
        UpdateSerialPortStatus();
    }

    /// <summary>
    /// 根据 serialPort1.IsOpen 刷新顶部指示灯与文本。
    /// </summary>
    public void UpdateSerialPortStatus()
    {
        try
        {
            bool isOpen = serialPort1 != null && serialPort1.IsOpen;
            if (isOpen)
            {
                pnlSerialStatusLight.BackColor = Color.Lime;
                lblSerialStatusText.Text = $"串口已连接 ({serialPort1.PortName} @ {serialPort1.BaudRate})";
                lblSerialStatusText.ForeColor = Color.LimeGreen;
            }
            else
            {
                pnlSerialStatusLight.BackColor = Color.DimGray;
                lblSerialStatusText.Text = "串口未连接";
                lblSerialStatusText.ForeColor = Color.DimGray;
            }
        }
        catch
        {
            pnlSerialStatusLight.BackColor = Color.DimGray;
            lblSerialStatusText.Text = "串口未连接";
            lblSerialStatusText.ForeColor = Color.DimGray;
        }
    }

    /// <summary>
    /// 指示灯 Paint：在 BackColor 基础上加一圈白色边框，让 LED 更立体、更显眼。
    /// </summary>
    private void pnlSerialStatusLight_Paint(object sender, PaintEventArgs e)
    {
        var panel = sender as Panel;
        if (panel == null) return;
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        using (var pen = new Pen(Color.White, 2))
        {
            // 留 1px 边距给白边，避免被 Region 裁掉
            e.Graphics.DrawEllipse(pen, 1, 1, panel.Width - 3, panel.Height - 3);
        }
    }

    private void label18_Click(object sender, EventArgs e)
    {
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
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.m_tsbtnSave = new System.Windows.Forms.ToolStripButton();
            this.m_tsbtnSerialConfig = new System.Windows.Forms.ToolStripButton();
            this.m_tsbtnExit = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbErsaOnlineURL = new System.Windows.Forms.Label();
            this.tbErsaOnlineUrl = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.cbErsaDownline = new System.Windows.Forms.CheckBox();
            this.lbErsaDownlineURL = new System.Windows.Forms.Label();
            this.tbErsaDownlineUrl = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btTest = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbByPass = new System.Windows.Forms.CheckBox();
            this.m_MachineCode = new System.Windows.Forms.TextBox();
            this.MachineCode = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(38, 38);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_tsbtnSave,
            this.m_tsbtnSerialConfig,
            this.m_tsbtnExit});
            this.toolStrip1.Location = new System.Drawing.Point(6, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1384, 50);
            this.toolStrip1.TabIndex = 27;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // m_tsbtnSave
            //
            this.m_tsbtnSave.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.m_tsbtnSave.Name = "m_tsbtnSave";
            this.m_tsbtnSave.Size = new System.Drawing.Size(72, 44);
            this.m_tsbtnSave.Text = "Save";
            this.m_tsbtnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // m_tsbtnSerialConfig
            //
            this.m_tsbtnSerialConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_tsbtnSerialConfig.Name = "m_tsbtnSerialConfig";
            this.m_tsbtnSerialConfig.Size = new System.Drawing.Size(96, 44);
            this.m_tsbtnSerialConfig.Text = "串口配置";
            this.m_tsbtnSerialConfig.Click += new System.EventHandler(this.m_tsbtnSerialConfig_Click);
            //
            // m_tsbtnExit
            // 
            this.m_tsbtnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_tsbtnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_tsbtnExit.Name = "m_tsbtnExit";
            this.m_tsbtnExit.Size = new System.Drawing.Size(66, 44);
            this.m_tsbtnExit.Text = "退出";
            this.m_tsbtnExit.ToolTipText = "保存退出";
            this.m_tsbtnExit.Click += new System.EventHandler(this.m_tsbtnExit_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(6, 1101);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(4, 0, 20, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1384, 22);
            this.statusStrip1.TabIndex = 28;
            this.statusStrip1.Text = "statusStrip1";
            //
            // pnlSerialStatusLight（圆形 LED 指示灯）
            //
            this.pnlSerialStatusLight = new System.Windows.Forms.Panel();
            this.pnlSerialStatusLight.Name = "pnlSerialStatusLight";
            this.pnlSerialStatusLight.Size = new System.Drawing.Size(24, 24);
            this.pnlSerialStatusLight.BackColor = System.Drawing.Color.DimGray;
            this.pnlSerialStatusLight.Margin = new System.Windows.Forms.Padding(8, 3, 2, 3);
            this.pnlSerialStatusLight.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSerialStatusLight_Paint);
            // 设置圆形 Region，让 Panel 显示为 LED 圆点
            {
                var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(0, 0, this.pnlSerialStatusLight.Width - 1, this.pnlSerialStatusLight.Height - 1);
                this.pnlSerialStatusLight.Region = new System.Drawing.Region(path);
            }
            //
            // lblSerialStatusText（指示灯旁的状态文字）
            //
            this.lblSerialStatusText = new System.Windows.Forms.Label();
            this.lblSerialStatusText.Name = "lblSerialStatusText";
            this.lblSerialStatusText.Text = "串口未连接";
            this.lblSerialStatusText.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblSerialStatusText.ForeColor = System.Drawing.Color.DimGray;
            this.lblSerialStatusText.AutoSize = true;
            this.lblSerialStatusText.Margin = new System.Windows.Forms.Padding(2, 6, 4, 3);
            // 把 LED + 文字作为 ToolStripControlHost 嵌入顶部工具栏
            this.toolStrip1.Items.Add(new System.Windows.Forms.ToolStripControlHost(this.pnlSerialStatusLight));
            this.toolStrip1.Items.Add(new System.Windows.Forms.ToolStripControlHost(this.lblSerialStatusText));
            // 
            // lbErsaOnlineURL
            // 
            this.lbErsaOnlineURL.AutoSize = true;
            this.lbErsaOnlineURL.Location = new System.Drawing.Point(630, 293);
            this.lbErsaOnlineURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbErsaOnlineURL.Name = "lbErsaOnlineURL";
            this.lbErsaOnlineURL.Size = new System.Drawing.Size(166, 24);
            this.lbErsaOnlineURL.TabIndex = 40;
            this.lbErsaOnlineURL.Text = "ChangeOverURL";
            // 
            // tbErsaOnlineUrl
            // 
            this.tbErsaOnlineUrl.Location = new System.Drawing.Point(817, 290);
            this.tbErsaOnlineUrl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbErsaOnlineUrl.Name = "tbErsaOnlineUrl";
            this.tbErsaOnlineUrl.Size = new System.Drawing.Size(402, 35);
            this.tbErsaOnlineUrl.TabIndex = 39;
            // 
            // cbErsaDownline
            // 
            this.cbErsaDownline.AutoSize = true;
            this.cbErsaDownline.Location = new System.Drawing.Point(140, 142);
            this.cbErsaDownline.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbErsaDownline.Name = "cbErsaDownline";
            this.cbErsaDownline.Size = new System.Drawing.Size(210, 28);
            this.cbErsaDownline.TabIndex = 35;
            this.cbErsaDownline.Text = "ErsaCheckPoint";
            this.cbErsaDownline.UseVisualStyleBackColor = true;
            // 
            // lbErsaDownlineURL
            // 
            this.lbErsaDownlineURL.AutoSize = true;
            this.lbErsaDownlineURL.Location = new System.Drawing.Point(580, 143);
            this.lbErsaDownlineURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbErsaDownlineURL.Name = "lbErsaDownlineURL";
            this.lbErsaDownlineURL.Size = new System.Drawing.Size(214, 24);
            this.lbErsaDownlineURL.TabIndex = 36;
            this.lbErsaDownlineURL.Text = "ErsaCheckPointURL";
            // 
            // tbErsaDownlineUrl
            // 
            this.tbErsaDownlineUrl.Location = new System.Drawing.Point(815, 134);
            this.tbErsaDownlineUrl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbErsaDownlineUrl.Name = "tbErsaDownlineUrl";
            this.tbErsaDownlineUrl.Size = new System.Drawing.Size(406, 35);
            this.tbErsaDownlineUrl.TabIndex = 37;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 50;
            // 
            // btTest
            // 
            this.btTest.Location = new System.Drawing.Point(145, 257);
            this.btTest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(112, 37);
            this.btTest.TabIndex = 49;
            this.btTest.Text = "Test";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbByPass);
            this.groupBox4.Controls.Add(this.btTest);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.tbErsaDownlineUrl);
            this.groupBox4.Controls.Add(this.lbErsaDownlineURL);
            this.groupBox4.Controls.Add(this.cbErsaDownline);
            this.groupBox4.Controls.Add(this.m_MachineCode);
            this.groupBox4.Controls.Add(this.MachineCode);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(6, 53);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.groupBox4.Size = new System.Drawing.Size(1384, 522);
            this.groupBox4.TabIndex = 49;
            this.groupBox4.TabStop = false;
            // 
            // cbByPass
            // 
            this.cbByPass.AutoSize = true;
            this.cbByPass.Location = new System.Drawing.Point(140, 193);
            this.cbByPass.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbByPass.Name = "cbByPass";
            this.cbByPass.Size = new System.Drawing.Size(114, 28);
            this.cbByPass.TabIndex = 51;
            this.cbByPass.Text = "ByPass";
            this.cbByPass.UseVisualStyleBackColor = true;
            // 
            // m_MachineCode
            // 
            this.m_MachineCode.Location = new System.Drawing.Point(304, 42);
            this.m_MachineCode.Margin = new System.Windows.Forms.Padding(2);
            this.m_MachineCode.Name = "m_MachineCode";
            this.m_MachineCode.Size = new System.Drawing.Size(404, 35);
            this.m_MachineCode.TabIndex = 3;
            // 
            // MachineCode
            // 
            this.MachineCode.AutoSize = true;
            this.MachineCode.Location = new System.Drawing.Point(141, 45);
            this.MachineCode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.MachineCode.Name = "MachineCode";
            this.MachineCode.Size = new System.Drawing.Size(142, 24);
            this.MachineCode.TabIndex = 2;
            this.MachineCode.Text = "MachineCode";
            this.MachineCode.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FrmSettingPlatform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1396, 1126);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FrmSettingPlatform";
            this.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Platform Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmSettingPlatform_FormClosed);
            this.Load += new System.EventHandler(this.FrmSettingTcpip_Load);
            this.Activated += new System.EventHandler(this.FrmSettingPlatform_Activated);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private async Task<Response> MoveInVerifyAsync(string messn)
    {
        try
        {
            var request = new ReadBarcode
            {
                Ib_Code = messn,
                Dev_Code = m_Config.m_RecipeName
            };

            string jsonRequest = JsonConvert.SerializeObject(request);
            m_edcLogger.Info($"MoveInVerify Request: {jsonRequest}");
            MessageBox.Show($"发送进站请求: {messn}");

            int timeout = 5000; // 5秒超时
            string url = m_Config.m_ersaOnlineUrl;

            // 使用HttpClient发送POST请求
            using (var cts = new System.Threading.CancellationTokenSource(timeout))
            {
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                try
                {
                    // 发送POST请求并等待响应
                    response = await _httpClient.PostAsync(url, content, cts.Token);
                }
                catch (OperationCanceledException)
                {
                    // 处理超时
                    return new Response
                    {
                        Message = $"请求超时（{timeout}ms）",
                        Code = "408"
                    };
                }

                // 读取响应内容
                string responseJson = await response.Content.ReadAsStringAsync();
                m_edcLogger.Info($"MoveInVerify Response: {responseJson}");

                var result = JsonConvert.DeserializeObject<Response>(responseJson);

                return result;
            }
        }
        catch (HttpRequestException ex)
        {
            m_edcLogger.Error($"HTTP请求异常: {ex.Message}", ex, "MoveInVerifyAsync", 0);
            MessageBox.Show($"进站接口调用失败: {ex.Message}");

            return new Response
            {
                Message = $"HTTP请求错误: {ex.Message}",
                Code = "500"
            };
        }
        catch (Exception ex)
        {
            m_edcLogger.Error($"MoveInVerify Error: {ex.Message}", ex, "MoveInVerifyAsync", 0);
            MessageBox.Show($"进站接口调用失败: {ex.Message}");

            return new Response
            {
                Message = $"接口调用异常: {ex.Message}",
                Code = "500"
            };
        }
    }

    private async void btTest_Click(object sender, EventArgs e)
    {
        // 测试用的序列号，您可以根据需要修改
        string testSN = "34264286427327JUYHGG";
        // 优先使用当前条码 m_strCurrentCode，为空时使用测试条码
        string barcode = string.IsNullOrEmpty(m_strCurrentCode) ? testSN : m_strCurrentCode;

        try
        {
            // 若共享串口未打开，尝试按 m_Config 当前配置打开
            if (XPT_Data.SharedSerialPort == null || !XPT_Data.SharedSerialPort.IsOpen)
            {
                if (!XPT_Data.OpenSharedSerialPort(m_Config, m_edcLogger, out string openErr))
                {
                    MessageBox.Show("自动打开串口失败: " + openErr, "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // 按配置的前缀/后缀拼接报文
            string prefix = m_Config?.m_strPrefix ?? string.Empty;
            string suffix = m_Config?.m_strSuffix ?? string.Empty;
            string payload = ResolveControlChars(prefix) + barcode + ResolveControlChars(suffix);

            // 串口发送
            XPT_Data.SharedSerialPort.Write(payload);
            m_edcLogger?.Info($"SerialPort Send: [{prefix}]{barcode}[{suffix}]", null);

            MessageBox.Show($"已通过 {XPT_Data.SharedSerialPort.PortName} @ {XPT_Data.SharedSerialPort.BaudRate} 发送：\r\n{barcode}",
                "发送成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            m_edcLogger?.Error("SerialPort Send Error: " + ex.Message, ex, "btTest_Click", 0);
            MessageBox.Show("串口发送失败: " + ex.Message, "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// 已废弃：保留仅为兼容外部可能的反射调用。实际打开逻辑请使用 XPT_Data.OpenSharedSerialPort。
    /// </summary>
    [Obsolete("请改用 XPT_Data.OpenSharedSerialPort")]
    private bool TryOpenSerialPort()
    {
        return XPT_Data.OpenSharedSerialPort(m_Config, m_edcLogger, out _);
    }

    /// <summary>
    /// 把形如 "STX (0x02)" / "CR (0x0D)" / "CRLF (0x0D 0x0A)" / "ETX (0x03)" / "无" 的描述转换为对应控制字符，
    /// 其它文本原样返回。空字符串直接返回空。
    /// </summary>
    private string ResolveControlChars(string token)
    {
        if (string.IsNullOrEmpty(token)) return string.Empty;
        string t = token.Trim();
        if (t.Equals("无", StringComparison.OrdinalIgnoreCase)) return string.Empty;

        switch (t.ToUpperInvariant())
        {
            case "STX (0X02)": return "\u0002";
            case "ETX (0X03)": return "\u0003";
            case "CR (0X0D)": return "\r";
            case "LF (0X0A)": return "\n";
            case "CRLF (0X0D 0X0A)": return "\r\n";
            default: return t;
        }
    }

    private XPT_ersaOnlineResponse GetResponse1(string message)
    {
        return JsonConvert.DeserializeObject<XPT_ersaOnlineResponse>(message);
    }
    private XPT_ersaDownlineResponse GetResponse(string message)
    {
       return JsonConvert.DeserializeObject<XPT_ersaDownlineResponse>(message);
    }
}
