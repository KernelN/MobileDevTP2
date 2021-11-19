using UnityEngine;

public class AndroidLoggerImplementation : UniversalLoggerImplementation
{
    AndroidJavaClass LoggerClass;
    AndroidJavaObject LoggerInstance;

    public AndroidLoggerImplementation(string packName, string className)
    {
        PACK_NAME = packName;
        LOGGER_CLASS_NAME = className;
    }
    internal override void Init()
    {
        LoggerClass = new AndroidJavaClass(PACK_NAME + "." + LOGGER_CLASS_NAME);
        LoggerInstance = LoggerClass.CallStatic<AndroidJavaObject>("GetInstance");
    }
    public override void SendLog(string msg)
    {
        if (LoggerInstance == null)
        {
            Init();
        }

        LoggerInstance.Call("SendLog", msg);
    }
}