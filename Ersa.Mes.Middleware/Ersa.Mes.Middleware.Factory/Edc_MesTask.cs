using System;
using System.Threading;
using System.Threading.Tasks;
using Ersa.Mes.FileSystem.Interface;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Interfaces;

namespace Ersa.Mes.Middleware.Factory;

public abstract class Edc_MesTask : Inf_MesTask
{
	public CancellationTokenSource m_cts = new CancellationTokenSource();

	protected bool m_blnOnlyOnce = false;

	protected Inf_Logger m_edcLogger { get; set; }

	public Enum_MachineType Pro_enuMachineType { get; set; } = Enum_MachineType.Reflow;


	public int m_i32CycleCount { get; set; }

	public Inf_MesTaskAttributes Pro_edcMesTaskAttributes { get; set; }

	public bool Pro_blnActive { get; set; } = false;


	public string Pro__strTaskName { get; set; } = "";


	public int Pro_i32DelayTime { get; set; } = 0;


	public int Pro_i32Repetition { get; set; } = 1;


	public int Pro_i32Interval { get; set; } = 1000;


	public event Evt_ShowMessageEventHandler Evt_ShowMessage;

	public event Evt_ShowErsasoftEventHandler Evt_ShowErsasoftCount;

	public event Evt_ShowPlcEventHandler Evt_ShowPlcCount;

	public event Evt_ShowOeeEventHandler Evt_ShowOEE;

	public event Evt_ProgramSelectedEventHandler Evt_ProgramSelected;

	protected virtual void OnShowMessage(Enum_LogType i_enuLogType, string i_strMessage)
	{
		this.Evt_ShowMessage?.Invoke(i_enuLogType, i_strMessage);
	}

	protected virtual void OnShowErsasoftCount(int i_i32Cycle)
	{
		this.Evt_ShowErsasoftCount?.Invoke(i_i32Cycle);
	}

	protected virtual void OnShowPlcCount(int i_i32Cycle)
	{
		this.Evt_ShowPlcCount?.Invoke(i_i32Cycle);
	}

	protected virtual void OnShowOEE(string i_strOeeCode, string i_strOeeText, int i_i32PcbInMachine = 0)
	{
		this.Evt_ShowOEE?.Invoke(i_strOeeCode, i_strOeeText, i_i32PcbInMachine);
	}

	protected virtual void OnProgramSelected(string i_strXmlContent)
	{
		this.Evt_ProgramSelected?.Invoke(i_strXmlContent);
	}

	public Edc_MesTask(Inf_MesTaskAttributes i_edcMesTaskAttributes, Inf_Logger i_edcLogger)
	{
		m_edcLogger = i_edcLogger;
		if (i_edcMesTaskAttributes != null)
		{
			Pro_edcMesTaskAttributes = i_edcMesTaskAttributes;
			Pro_blnActive = i_edcMesTaskAttributes.Pro_blnActive;
			Pro__strTaskName = i_edcMesTaskAttributes.Pro_strName;
			Pro_i32Repetition = i_edcMesTaskAttributes.Pro_i32Repetition;
			Pro_i32Interval = i_edcMesTaskAttributes.Pro_i32Interval;
			Pro_i32DelayTime = i_edcMesTaskAttributes.Pro_i32DelayTime;
			m_edcLogger.Debug($"Assmebly MES Task  TaskName:{i_edcMesTaskAttributes.Pro_strName}  Repetition:{i_edcMesTaskAttributes.Pro_i32Repetition}  Interval:{i_edcMesTaskAttributes.Pro_i32Interval}  DelayTime:{i_edcMesTaskAttributes.Pro_i32DelayTime}", null, ".ctor", 139);
		}
	}

	public void Sub_Invoke()
	{
		m_i32CycleCount = 0;
		if (m_cts.IsCancellationRequested)
		{
			m_cts = new CancellationTokenSource();
		}
		if (Pro_blnActive)
		{
			Task.Factory.StartNew(delegate
			{
				Sub_StartTask();
			}, m_cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}
	}

	public void Sub_StartTask()
	{
		try
		{
			Thread.Sleep(Pro_i32DelayTime);
			Sub_Load();
			while (!m_cts.IsCancellationRequested)
			{
				if (!Pro__strTaskName.Equals("Mes", StringComparison.OrdinalIgnoreCase))
				{
					m_edcLogger.Debug("Begin Task:" + Pro__strTaskName, null, "Sub_StartTask", 194);
				}
				Sub_Act();
				if (!Pro__strTaskName.Equals("Mes", StringComparison.OrdinalIgnoreCase))
				{
					m_edcLogger.Debug("End Task:" + Pro__strTaskName, null, "Sub_StartTask", 199);
				}
				if (Pro_edcMesTaskAttributes.Pro_i32Repetition <= 0)
				{
					m_edcLogger.Debug("Task:" + Pro__strTaskName + " Repetition 0", null, "Sub_StartTask", 205);
					break;
				}
				Thread.Sleep(Pro_i32Interval);
				if (!Pro__strTaskName.Equals("Mes", StringComparison.OrdinalIgnoreCase))
				{
					Pro_edcMesTaskAttributes.Pro_i32Repetition--;
				}
				if (m_blnOnlyOnce)
				{
					break;
				}
			}
		}
		catch (Exception ex)
		{
			m_edcLogger.Error("Middleware -> Mes Task Sub_StartTask Error...Details:'" + ex.Message + "'", null, "Sub_StartTask", 221);
		}
	}

	public virtual void Sub_Load()
	{
	}

	public abstract Task Sub_Act();

	public virtual void Sub_End()
	{
		m_cts.Cancel();
	}
}
