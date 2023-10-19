
namespace EVCP.Simulation;
public class CarThreadClass
{
    private volatile bool run = false;
    private volatile int updateFrequencyMs;
    private volatile int threadWaitFluctuationMs;
    private volatile int carId;
    private Thread thread;
    private Object lockObject = new();
    public CarThreadClass(int updateFrequencyMs, int threadWaitFluctuationMs, int id)
    {
        this.updateFrequencyMs = updateFrequencyMs;
        this.threadWaitFluctuationMs = threadWaitFluctuationMs;
        this.carId = id;
        thread = new Thread(this.MainLoop);
    }

    public void startThread(){
        run = true;
        thread.Start();
    }

    public void stopThread(){
        run = false;
        Console.WriteLine($"Car: {carId} stopping...");
    }

    public void stopAndJoin(){
        run = false; 
        thread.Join();
        Console.WriteLine($"Car: {carId} STOPPED");
    }

    private void MainLoop(){
        Random r = new();
        int i = 1;
        while(run){
            Thread.Sleep(updateFrequencyMs+r.Next(0,threadWaitFluctuationMs));
            Console.WriteLine($"Hi I am car: {carId} and this is my {i++}. loop.");
        }

    }
}
