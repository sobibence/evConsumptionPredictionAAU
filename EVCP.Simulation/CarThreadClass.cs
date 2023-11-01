
using EVCP.Domain.Models;

namespace EVCP.Simulation;
public class CarThreadClass
{
    private volatile bool run = false;
    private volatile int updateFrequencyMs;
    private volatile int threadWaitFluctuationMs;
    private volatile int carId;

    public int CarId{get{
        return carId;
    }}
    private Thread thread;
    private Object lockObject = new();

    Car car;

    private List<Edge> route;
    public CarThreadClass(int updateFrequencyMs, int threadWaitFluctuationMs, int id, List<Edge> initRoute)
    {
        this.updateFrequencyMs = updateFrequencyMs;
        this.threadWaitFluctuationMs = threadWaitFluctuationMs;
        carId = id;
        route = initRoute;
        thread = new Thread(MainLoop);
        car = new(initRoute);
    }

    public void startThread()
    {
        run = true;
        thread.Start();
    }

    public void stopThread()
    {
        run = false;
        Console.WriteLine($"Car: {carId} stopping...");
    }

    public void stopAndJoin()
    {
        run = false;
        thread.Join();
        Console.WriteLine($"Car: {carId} STOPPED");
    }

    private void MainLoop()
    {
        Random r = new();
        car = new(route);
        Console.WriteLine($"Hi I am car: {carId} and I started my main loop...");
        while (run)
        {
            int sleeptime = updateFrequencyMs + r.Next(0, threadWaitFluctuationMs);
            SimulationManager.Instance.getRouteFromCar(car.getNextCarStatus(sleeptime),this);
            Thread.Sleep(sleeptime);

        }

    }
}
