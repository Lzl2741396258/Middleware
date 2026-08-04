using System;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using Ersa.Mes.FileSystem.Model;
using Ersa.Mes.Logging;
using MesXPT.Model;

namespace MesXPT;

public class FrmSerialPortConfig : Form
{
    private IContainer components = null;

    private XPT_Config m_Config { get; }

    private SerialPort m_serialPort { get; }

    private Inf_Logger m_edcLogger { get; }

    private TextBox tbComPort;
    private ComboBox cbBaudRate;
    private ComboBox cbDataBits;
    private ComboBox cbStopBits;
    private ComboBox cbParity;
    private ComboBox cbPrefix;
    private ComboBox cbSuffix;
    private TextBox tbTriggerChar;
    private Button btnOk;
    private Button btnCancel;

    private Label lblComPort;
    private Label lblBaudRate;
    private Label lblDataBits;
    private Label lblStopBits;
    private Label lblParity;
    private Label lblPrefix;
    private Label lblSuffix;
    private Label lblTriggerChar;

    public FrmSerialPortConfig(XPT_Config i_Config, SerialPort i_serialPort, Inf_Logger i_edcLogger)
    {
        m_Config = i_Config;
        m_serialPort = i_serialPort;
        m_edcLogger = i_edcLogger;
        InitializeComponent();
        LoadFromConfig();
    }

    private void LoadFromConfig()
    {
        tbComPort.Text = string.IsNullOrEmpty(m_Config.m_strcomPort) ? "COM 1" : m_Config.m_strcomPort;

        string baudRate = m_Config.m_strBaudRate;
        if (string.IsNullOrEmpty(baudRate)) baudRate = "9600";
        SetComboSelected(cbBaudRate, baudRate);

        string dataBits = m_Config.m_strDataBits;
        if (string.IsNullOrEmpty(dataBits)) dataBits = "8";
        SetComboSelected(cbDataBits, dataBits);

        string stopBits = m_Config.m_strStopBits;
        if (string.IsNullOrEmpty(stopBits)) stopBits = "1";
        SetComboSelected(cbStopBits, stopBits);

        string parity = m_Config.m_strParity;
        if (string.IsNullOrEmpty(parity)) parity = "None";
        SetComboSelected(cbParity, parity);

        SetComboSelected(cbPrefix, m_Config.m_strPrefix ?? "无");
        SetComboSelected(cbSuffix, m_Config.m_strSuffix ?? "CR (0x0D)");

        tbTriggerChar.Text = string.IsNullOrEmpty(m_Config.m_strTriggerChar) ? "a" : m_Config.m_strTriggerChar;
    }

    private void SetComboSelected(ComboBox cb, string value)
    {
        for (int i = 0; i < cb.Items.Count; i++)
        {
            if (string.Equals(cb.Items[i].ToString(), value, StringComparison.OrdinalIgnoreCase))
            {
                cb.SelectedIndex = i;
                return;
            }
        }
        cb.Text = value;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            // 临时保存到 XPT_Config 实体（不立即写文件，由主窗体 Save 统一落地）
            m_Config.m_strcomPort = tbComPort.Text.Trim();
            m_Config.m_strBaudRate = cbBaudRate.Text.Trim();
            m_Config.m_strDataBits = cbDataBits.Text.Trim();
            m_Config.m_strStopBits = cbStopBits.Text.Trim();
            m_Config.m_strParity = cbParity.Text.Trim();
            m_Config.m_strPrefix = cbPrefix.Text.Trim();
            m_Config.m_strSuffix = cbSuffix.Text.Trim();
            m_Config.m_strTriggerChar = tbTriggerChar.Text.Trim();

            // 复用现有 SerialPort 对象：应用参数后自动打开串口
            if (m_serialPort != null)
            {
                // 若串口已打开，先关闭以便更新参数
                if (m_serialPort.IsOpen)
                {
                    m_serialPort.Close();
                }

                // 先设置 PortName（打开前必须先设置）
                m_serialPort.PortName = m_Config.m_strcomPort;

                if (int.TryParse(m_Config.m_strBaudRate, out int baud))
                    m_serialPort.BaudRate = baud;
                if (int.TryParse(m_Config.m_strDataBits, out int dataBits))
                    m_serialPort.DataBits = dataBits;

                if (Enum.TryParse<StopBits>(m_Config.m_strStopBits == "1.5" ? "OnePointFive" : m_Config.m_strStopBits, out StopBits stopBits))
                    m_serialPort.StopBits = stopBits;

                if (Enum.TryParse<Parity>(m_Config.m_strParity, out Parity parity))
                    m_serialPort.Parity = parity;

                // 打开串口
                m_serialPort.Open();

                MessageBox.Show($"串口已打开：{m_serialPort.PortName} @ {m_serialPort.BaudRate}", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            m_edcLogger?.Error("SerialPort Config Apply Error: " + ex.Message, ex, "btnOk_Click", 0);
            MessageBox.Show("应用串口配置失败: " + ex.Message, "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            // 打开失败时让弹窗保持打开，方便用户修改
            DialogResult = DialogResult.None;
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
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
        this.tbComPort = new System.Windows.Forms.TextBox();
        this.cbBaudRate = new System.Windows.Forms.ComboBox();
        this.cbDataBits = new System.Windows.Forms.ComboBox();
        this.cbStopBits = new System.Windows.Forms.ComboBox();
        this.cbParity = new System.Windows.Forms.ComboBox();
        this.cbPrefix = new System.Windows.Forms.ComboBox();
        this.cbSuffix = new System.Windows.Forms.ComboBox();
        this.tbTriggerChar = new System.Windows.Forms.TextBox();
        this.btnOk = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.SuspendLayout();
        //
        // 表单
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(384, 421);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "FrmSerialPortConfig";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "串口配置";
        //
        // lblComPort
        //
        this.lblComPort = new System.Windows.Forms.Label();
        this.lblComPort.AutoSize = true;
        this.lblComPort.Location = new System.Drawing.Point(20, 25);
        this.lblComPort.Name = "lblComPort";
        this.lblComPort.Size = new System.Drawing.Size(80, 24);
        this.lblComPort.Text = "Com 口";
        this.lblComPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        //
        // tbComPort
        //
        this.tbComPort.Location = new System.Drawing.Point(110, 20);
        this.tbComPort.Name = "tbComPort";
        this.tbComPort.Size = new System.Drawing.Size(240, 28);
        this.tbComPort.Text = "COM 1";
        //
        // lblBaudRate
        //
        this.lblBaudRate = new System.Windows.Forms.Label();
        this.lblBaudRate.AutoSize = true;
        this.lblBaudRate.Location = new System.Drawing.Point(20, 65);
        this.lblBaudRate.Name = "lblBaudRate";
        this.lblBaudRate.Size = new System.Drawing.Size(80, 24);
        this.lblBaudRate.Text = "波特率";
        this.lblBaudRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        //
        // cbBaudRate
        //
        this.cbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbBaudRate.Items.AddRange(new object[] { "9600", "19200", "38400", "57600", "115200" });
        this.cbBaudRate.Location = new System.Drawing.Point(110, 60);
        this.cbBaudRate.Name = "cbBaudRate";
        this.cbBaudRate.Size = new System.Drawing.Size(240, 28);
        //
        // lblDataBits
        //
        this.lblDataBits = new System.Windows.Forms.Label();
        this.lblDataBits.AutoSize = true;
        this.lblDataBits.Location = new System.Drawing.Point(20, 105);
        this.lblDataBits.Name = "lblDataBits";
        this.lblDataBits.Size = new System.Drawing.Size(80, 24);
        this.lblDataBits.Text = "数据位";
        this.lblDataBits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        //
        // cbDataBits
        //
        this.cbDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbDataBits.Items.AddRange(new object[] { "5", "6", "7", "8" });
        this.cbDataBits.Location = new System.Drawing.Point(110, 100);
        this.cbDataBits.Name = "cbDataBits";
        this.cbDataBits.Size = new System.Drawing.Size(240, 28);
        //
        // lblStopBits
        //
        this.lblStopBits = new System.Windows.Forms.Label();
        this.lblStopBits.AutoSize = true;
        this.lblStopBits.Location = new System.Drawing.Point(20, 145);
        this.lblStopBits.Name = "lblStopBits";
        this.lblStopBits.Size = new System.Drawing.Size(80, 24);
        this.lblStopBits.Text = "停止位";
        this.lblStopBits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        //
        // cbStopBits
        //
        this.cbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbStopBits.Items.AddRange(new object[] { "1", "1.5", "2" });
        this.cbStopBits.Location = new System.Drawing.Point(110, 140);
        this.cbStopBits.Name = "cbStopBits";
        this.cbStopBits.Size = new System.Drawing.Size(240, 28);
        //
        // lblParity
        //
        this.lblParity = new System.Windows.Forms.Label();
        this.lblParity.AutoSize = true;
        this.lblParity.Location = new System.Drawing.Point(20, 185);
        this.lblParity.Name = "lblParity";
        this.lblParity.Size = new System.Drawing.Size(80, 24);
        this.lblParity.Text = "奇偶";
        this.lblParity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        //
        // cbParity
        //
        this.cbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbParity.Items.AddRange(new object[] { "None", "Odd", "Even", "Mark", "Space" });
        this.cbParity.Location = new System.Drawing.Point(110, 180);
        this.cbParity.Name = "cbParity";
        this.cbParity.Size = new System.Drawing.Size(240, 28);
        //
        // lblPrefix
        //
        this.lblPrefix = new System.Windows.Forms.Label();
        this.lblPrefix.AutoSize = true;
        this.lblPrefix.Location = new System.Drawing.Point(20, 225);
        this.lblPrefix.Name = "lblPrefix";
        this.lblPrefix.Size = new System.Drawing.Size(80, 24);
        this.lblPrefix.Text = "前缀";
        this.lblPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        //
        // cbPrefix
        //
        this.cbPrefix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
        this.cbPrefix.Items.AddRange(new object[] { "STX (0x02)", "无" });
        this.cbPrefix.Location = new System.Drawing.Point(110, 220);
        this.cbPrefix.Name = "cbPrefix";
        this.cbPrefix.Size = new System.Drawing.Size(240, 28);
        //
        // lblSuffix
        //
        this.lblSuffix = new System.Windows.Forms.Label();
        this.lblSuffix.AutoSize = true;
        this.lblSuffix.Location = new System.Drawing.Point(20, 265);
        this.lblSuffix.Name = "lblSuffix";
        this.lblSuffix.Size = new System.Drawing.Size(80, 24);
        this.lblSuffix.Text = "后缀";
        this.lblSuffix.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        //
        // cbSuffix
        //
        this.cbSuffix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
        this.cbSuffix.Items.AddRange(new object[] { "CR (0x0D)", "LF (0x0A)", "CRLF (0x0D 0x0A)", "ETX (0x03)", "无" });
        this.cbSuffix.Location = new System.Drawing.Point(110, 260);
        this.cbSuffix.Name = "cbSuffix";
        this.cbSuffix.Size = new System.Drawing.Size(240, 28);
        //
        // lblTriggerChar
        //
        this.lblTriggerChar = new System.Windows.Forms.Label();
        this.lblTriggerChar.AutoSize = true;
        this.lblTriggerChar.Location = new System.Drawing.Point(20, 305);
        this.lblTriggerChar.Name = "lblTriggerChar";
        this.lblTriggerChar.Size = new System.Drawing.Size(80, 24);
        this.lblTriggerChar.Text = "触发字符";
        this.lblTriggerChar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        //
        // tbTriggerChar
        //
        this.tbTriggerChar.Location = new System.Drawing.Point(110, 300);
        this.tbTriggerChar.Name = "tbTriggerChar";
        this.tbTriggerChar.Size = new System.Drawing.Size(240, 28);
        this.tbTriggerChar.Text = "a";
        //
        // btnOk
        //
        this.btnOk.Location = new System.Drawing.Point(120, 360);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(90, 32);
        this.btnOk.Text = "确定";
        this.btnOk.UseVisualStyleBackColor = true;
        this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
        //
        // btnCancel
        //
        this.btnCancel.Location = new System.Drawing.Point(230, 360);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(90, 32);
        this.btnCancel.Text = "取消";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
        //
        // FrmSerialPortConfig
        //
        this.Controls.Add(this.lblComPort);
        this.Controls.Add(this.tbComPort);
        this.Controls.Add(this.lblBaudRate);
        this.Controls.Add(this.cbBaudRate);
        this.Controls.Add(this.lblDataBits);
        this.Controls.Add(this.cbDataBits);
        this.Controls.Add(this.lblStopBits);
        this.Controls.Add(this.cbStopBits);
        this.Controls.Add(this.lblParity);
        this.Controls.Add(this.cbParity);
        this.Controls.Add(this.lblPrefix);
        this.Controls.Add(this.cbPrefix);
        this.Controls.Add(this.lblSuffix);
        this.Controls.Add(this.cbSuffix);
        this.Controls.Add(this.lblTriggerChar);
        this.Controls.Add(this.tbTriggerChar);
        this.Controls.Add(this.btnOk);
        this.Controls.Add(this.btnCancel);
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
