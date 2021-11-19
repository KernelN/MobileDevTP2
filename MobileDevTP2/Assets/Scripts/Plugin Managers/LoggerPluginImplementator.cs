public static class LoggerPluginImplementator
{
	const string PACK_NAME = "com.insausti.logsmanager";
    const string LOGGER_CLASS_NAME = "Logger";
    static UniversalLoggerImplementation platformAdapter;

	static void Init()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        platformAdapter = new AndroidLoggerImplementation(PACK_NAME, LOGGER_CLASS_NAME);
#endif
    }

    public static void SendLog(string msg)
    {
        if (platformAdapter == null)
        {
            Init();
            if (platformAdapter == null) return; //if init didn't work, abort
        }

        platformAdapter.SendLog(msg);
    }
}