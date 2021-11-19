public abstract class UniversalLoggerImplementation
{
    internal string PACK_NAME;
    internal string LOGGER_CLASS_NAME;

    internal abstract void Init();
    public abstract void SendLog(string msg);
}