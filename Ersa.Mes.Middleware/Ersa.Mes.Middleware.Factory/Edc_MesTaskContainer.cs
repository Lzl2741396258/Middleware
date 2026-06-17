using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ersa.Mes.FileSystem.Model.Config.ConfigMiddleware;
using Ersa.Mes.FileSystem.Model.RequestResponse;
using Ersa.Mes.Logging;
using Ersa.Mes.Middleware.Interface;
using Ersa.Mes.Middleware.MesFunctionFolder;

namespace Ersa.Mes.Middleware.Factory;

public abstract class Edc_MesTaskContainer : Inf_MesTaskContainer, Inf_MesFunctionContainer
{
    public Inf_Logger m_edcLogger;

    protected Inf_MesTaskContainer m_edcMesTaskContainer { get; set; }

    private Edc_ConfigBase m_edcConfigBase { get; set; }

    public bool m_blnTaskStart { get; set; } = false;


    private IDictionary<Type, object> m_dicTasks { get; set; }

    public event Evt_ShowMessageEventHandler Evt_ShowMessage;

    public event Evt_ShowOeeEventHandler Evt_ShowOEE;

    public event Evt_ShowErsasoftEventHandler Evt_ShowErsasoft;

    public event Evt_ShowPlcEventHandler Evt_ShowPLC;

    protected void OnShowMessage(Enum_LogType i_enuLogType, string i_strMessage)
    {
        this.Evt_ShowMessage?.Invoke(i_enuLogType, i_strMessage);
    }

    protected void OnShowOEE(string i_strOeeCode, string i_strOeeText)
    {
        this.Evt_ShowOEE?.Invoke(i_strOeeCode, i_strOeeText);
    }

    protected void OnShowErsasoft(int i_i32Cycle)
    {
        this.Evt_ShowErsasoft?.Invoke(i_i32Cycle);
    }

    protected void OnShowPlc(int i_i32Cycle)
    {
        this.Evt_ShowPLC?.Invoke(i_i32Cycle);
    }

    public Edc_MesTaskContainer(Edc_ConfigBase i_edcConfigBase, Inf_Logger i_edcLogger)
    {
        m_edcConfigBase = i_edcConfigBase;
        m_edcLogger = i_edcLogger;
        m_dicTasks = new Dictionary<Type, object>();
    }

    public virtual void Sub_Initialize()
    {
        try
        {
            Sub_AddMesTask();
        }
        catch (Exception ex)
        {
            m_edcLogger.Error("Sub_AssmeblyMesTask error...Details:'" + ex.Message + "'", null, "Sub_Initialize", 110);
        }


        foreach (KeyValuePair<Type, object> dicTask in m_dicTasks)
        {
            Edc_MesTask mesTask = dicTask.Value as Edc_MesTask;
            mesTask.Evt_ShowMessage += this.Evt_ShowMessage;
            mesTask.Evt_ShowErsasoftCount += this.Evt_ShowErsasoft;
            mesTask.Evt_ShowPlcCount += this.Evt_ShowPLC;
            mesTask.Evt_ShowOEE += this.Evt_ShowOEE;
            mesTask.Sub_Invoke();
            OnShowMessage(Enum_LogType.Info, "Task '" + mesTask.Pro_edcMesTaskAttributes.Pro_strName + "' Start");
        }
        m_blnTaskStart = true;
    }

    public void Func_Test()
    {
        if (m_dicTasks.FirstOrDefault().Value is Edc_MesFunctionContainer container)
        {
            container.Func_Test();
        }
    }

    private void Sub_AddMesTask()
    {
        switch (m_edcConfigBase.m_clsBasicSettings.m_enuMachineType)
        {
            case Enum_MachineType.Selektiv:
                Sub_AddMesTaskSelective();
                break;
            case Enum_MachineType.Reflow:
                Sub_AddMesTaskReflow();
                break;
            case Enum_MachineType.Welle:
                Sub_AddMesTaskWave();
                break;
            case Enum_MachineType.EcoSelect2:
                Sub_AddMesTaskEcoSelect2();
                break;
            default:
                throw new Exception("Unrecognized machine types");
        }
    }

    protected virtual void Sub_AddMesTaskReflow()
    {
    }

    protected virtual void Sub_AddMesTaskSelective()
    {
    }

    protected virtual void Sub_AddMesTaskWave()
    {
    }

    protected virtual void Sub_AddMesTaskEcoSelect2()
    {
    }

    public void Sub_AddObject<T>(object i_objTask)
    {
        if (i_objTask != null && i_objTask is Edc_MesTask && ((Edc_MesTask)i_objTask).Pro_blnActive)
        {
            if (m_dicTasks.ContainsKey(typeof(T)))
            {
                Sub_RemoveObject<T>();
            }
            m_dicTasks.Add(typeof(T), i_objTask);
        }
    }

    public T Fun_edcGetObject<T>() where T : class
    {
        if (!m_dicTasks.TryGetValue(typeof(T), out var value))
        {
            return null;
        }
        return (T)value;
    }

    public void Sub_RemoveObject<T>()
    {
        m_dicTasks?.Remove(typeof(T));
    }

    public virtual void Sub_EndTask()
    {
        try
        {
            m_blnTaskStart = false;
            foreach (KeyValuePair<Type, object> dicTask in m_dicTasks)
            {
                Edc_MesTask mesTask = dicTask.Value as Edc_MesTask;
                mesTask.Sub_End();
            }
        }
        catch (Exception ex)
        {
            m_edcLogger.Error(MethodBase.GetCurrentMethod().Name + " error...Details:'" + ex.Message + "'", null, "Sub_EndTask", 225);
            throw;
        }
    }
}
