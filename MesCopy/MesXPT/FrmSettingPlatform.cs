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

    private StatusStrip statusStrip1;

    private System.Windows.Forms.Timer timer1;

    private SerialPort serialPort1;
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
            XPT_Data.m_byPass = true;
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
            XPT_Data.m_strProgram = m_Config.m_programFilePath;

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
        serialPort1.Close();
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
            this.MachineCode = new System.Windows.Forms.Label();
            this.m_MachineCode = new System.Windows.Forms.TextBox();
            this.cbByPass = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(38, 38);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_tsbtnSave,
            this.m_tsbtnExit});
            this.toolStrip1.Location = new System.Drawing.Point(6, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1384, 41);
            this.toolStrip1.TabIndex = 27;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // m_tsbtnSave
            // 
            this.m_tsbtnSave.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.m_tsbtnSave.Name = "m_tsbtnSave";
            this.m_tsbtnSave.Size = new System.Drawing.Size(72, 35);
            this.m_tsbtnSave.Text = "Save";
            this.m_tsbtnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // m_tsbtnExit
            // 
            this.m_tsbtnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_tsbtnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_tsbtnExit.Name = "m_tsbtnExit";
            this.m_tsbtnExit.Size = new System.Drawing.Size(66, 35);
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
            this.groupBox4.Location = new System.Drawing.Point(6, 44);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.groupBox4.Size = new System.Drawing.Size(1384, 522);
            this.groupBox4.TabIndex = 49;
            this.groupBox4.TabStop = false;
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
            // m_MachineCode
            // 
            this.m_MachineCode.Location = new System.Drawing.Point(304, 42);
            this.m_MachineCode.Margin = new System.Windows.Forms.Padding(2);
            this.m_MachineCode.Name = "m_MachineCode";
            this.m_MachineCode.Size = new System.Drawing.Size(404, 35);
            this.m_MachineCode.TabIndex = 3;
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
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private async Task<MoveInVerifyResponse> MoveInVerifyAsync(string messn)
    {
        try
        {
            var request = new MoveInVerifyRequest
            {
                CommandType = "BarcodeCheck",
                LocalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                Line = m_Config.m_Line,
                MachineCode = m_Config.m_MachineCode,
                Barcode = m_Config.m_Barcode,
                Lane = m_Config.m_Lane,
                Layer = m_Config.m_Layer
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
                    return new MoveInVerifyResponse
                    {
                        Message = $"请求超时（{timeout}ms）",
                        ValueReturn = "408"
                    };
                }

                // 读取响应内容
                string responseJson = await response.Content.ReadAsStringAsync();
                m_edcLogger.Info($"MoveInVerify Response: {responseJson}");

                var result = JsonConvert.DeserializeObject<MoveInVerifyResponse>(responseJson);

                return result;
            }
        }
        catch (HttpRequestException ex)
        {
            m_edcLogger.Error($"HTTP请求异常: {ex.Message}", ex, "MoveInVerifyAsync", 0);
            MessageBox.Show($"进站接口调用失败: {ex.Message}");

            return new MoveInVerifyResponse
            {
                Message = $"HTTP请求错误: {ex.Message}",
                ValueReturn = "500"
            };
        }
        catch (Exception ex)
        {
            m_edcLogger.Error($"MoveInVerify Error: {ex.Message}", ex, "MoveInVerifyAsync", 0);
            MessageBox.Show($"进站接口调用失败: {ex.Message}");

            return new MoveInVerifyResponse
            {
                Message = $"接口调用异常: {ex.Message}",
                ValueReturn = "500"
            };
        }
    }

    private async void btTest_Click(object sender, EventArgs e)
    {
        //// 测试用的序列号，您可以根据需要修改
        //string testSN = "211102600047901618XKN03AAH";

        //var response = await MoveInVerifyAsync(testSN);

        //if (response.IsSuccessStatusCode)
        //{
        //    MessageBox.Show($"进站校验成功!\nMessage: {response.Message}\n工单: {response.Data?.BelongWipOrderModel?.WipOrderNo}");
        //}
        //else
        //{
        //    MessageBox.Show($"进站校验失败!\nError: {response.Message}\nCode: {response.Result}");
        //}

        //int iTimtout = 50000;

        SendProgram("3608937XXX02A-NIO-AE_TOP");

    }

    private void SendProgram(string program)
    {
        #region 旧代码
        //try
        //{

        //    //   OnShowMessage(Enum_LogType.Info, "PROGRAM： " + program);

        //    //if (program.Equals(lastChangeProgram))
        //    //{
        //    //    m_edcLogger.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "和上一笔程序一样未换型，不生成新文件");
        //    //    OnShowMessage(Enum_LogType.Info, "PROGRAM： " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "和上一笔程序一样未换型，不生成新文件");
        //    //}

        //    string path = @"D:\Test\ERSA";

        //    // 获取当前日期
        //    DateTime currentDate = DateTime.Now;
        //    string fileName = $"rel0_{currentDate:yyyyMMddHHmmss}.csv"; // 例如：20230928_data.csv

        //    // 完整文件路径
        //    string filePath = Path.Combine(path, fileName);

        //    // 数据要写入的内容
        //    string[] headers = new string[]
        //    {
        //    "Program",
        //    "TrackNumber",
        //    "Side"
        //    };

        //    // 变量赋值
        //    string ChangeProgram = program;

        //    if (!string.IsNullOrEmpty(ChangeProgram))
        //    {
        //        // E2C-7375-B-01
        //        // EXCELTECH-01-TOP
        //        string[] prog = ChangeProgram.Split('-');
        //        string type = string.Empty;
        //        if (prog.Length > 2)
        //        {
        //            string source = prog[2];
        //            type = new string(source.Take(1).ToArray());
        //        }
        //        string trackNumber = "0";

        //        string side = type;

        //        // 创建或追加到文件
        //        using (StreamWriter writer = new StreamWriter(filePath, false))
        //        {
        //            // 写入表头
        //            writer.WriteLine(string.Join(",", headers));

        //            // 写入值
        //            writer.WriteLine($"{ChangeProgram},{trackNumber},{side}");
        //        }

        //        //lastChangeProgram = program;

        //        //OnShowMessage(Enum_LogType.Info, "处理 程序写入文件成功 ！");
        //    }
        //}
        //catch (Exception ex)
        //{

        //    //OnShowMessage(Enum_LogType.Info, "处理 程序异常： " + ex.Message);
        //}
        #endregion

        try
        {
            //string strProgram = XPT_Data.m_strProgram;

            //if (program != strProgram)
            //{
            //    string errorMsg = $"程序换型检测到不一致！当前程序：{program}，上一次程序：{strProgram}。流程已中断，禁止继续执行。";
            //    MessageBox.Show(errorMsg);
            //    return;
            //}

            MessageBox.Show("PROGRAM： " + program);


            //  m_edcLogger.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "程序名 ： " + program);
            //  OnShowMessage(Enum_LogType.Info, "PROGRAM： " + program + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "和上一笔程序一样未换型，不生成新文件");


            string path = m_Config.m_programFilePath;

            // 获取当前日期
            DateTime currentDate = DateTime.Now;
            string fileName = $"rel0_{currentDate:yyyyMMddHHmmss}.csv"; // 例如：20230928_data.csv

            // 完整文件路径
            string filePath = Path.Combine(path, fileName);

            MessageBox.Show("PROGRAM 路径： " + filePath);

            // 数据要写入的内容
            string[] headers = new string[]
            {
            "Program",
            "TrackNumber",
            "Side"
            };

            // 变量赋值
            string ChangeProgram = program;

            if (!string.IsNullOrEmpty(ChangeProgram))
            {
                //3608937XXX02A-NIO-AE_TOP ProductNo:3608937XXX02A-NIO  ProductVersion:AE  PCBSurfaceID:TOP
                //string[] prog = ChangeProgram.Split('-');
                //string type = string.Empty;
                //if (prog.Length > 2)
                //{
                //    string source = prog[2];
                //    type = new string(source.Take(1).ToArray());
                //}

                string type = string.Empty;

                //string source = m_Config.m_side;

                string source = "TOP";

                if (source.Equals("TOP", StringComparison.OrdinalIgnoreCase))
                {
                    type = "T";
                }
                else
                {
                    type = "B";
                }

                string trackNumber = "0";

                string side = type;

                // 创建或追加到文件
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    // 写入表头
                    writer.WriteLine(string.Join(",", headers));

                    // 写入值
                    writer.WriteLine($"{ChangeProgram},{trackNumber},{side}");
                }

                MessageBox.Show("处理 程序写入文件成功 ！");
            }
        }
        catch (Exception ex)
        {

            MessageBox.Show( "处理 程序异常： " + ex.Message);
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
