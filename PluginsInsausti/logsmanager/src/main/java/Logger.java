import android.util.Log;

public class Logger {
    public static final Logger _instance = new Logger();

    public static Logger GetInstance(){
        return _instance;
    }

    public void SendLog(String msg) {
        Log.d("L>", msg);
    }
}
