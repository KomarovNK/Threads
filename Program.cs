using System;
using System.Threading;

class SharedRes
{
    public static int Count;
    public static Mutex mtx = new Mutex();
}

class IncThread
{
    private int num;
    public Thread Thrd;

    public IncThread(string name, int n)
    {
        Thrd = new Thread(this.Run);
        num = n;
        Thrd.Name = name;
        Thrd.Start();
    }

    void Run()
    {
        Console.WriteLine(Thrd.Name + " ожидает мьютекс");
        SharedRes.mtx.WaitOne(); // Получаем мьютекс
        Console.WriteLine(Thrd.Name + " получает мьютекс");

        for (int i = 0; i < num; i++)
        {
            Thread.Sleep(500); // Имитация работы
            SharedRes.Count++;
            Console.WriteLine("в потоке {0}, Count={1}", Thrd.Name, SharedRes.Count);
        }

        Console.WriteLine(Thrd.Name + " освобождает мьютекс");
        SharedRes.mtx.ReleaseMutex(); // Освобождаем мьютекс
    }
}

class DecThread
{
    private int num;
    public Thread Thrd;

    public DecThread(string name, int n)
    {
        Thrd = new Thread(this.Run);
        num = n;
        Thrd.Name = name;
        Thrd.Start();
    }

    void Run()
    {
        Console.WriteLine(Thrd.Name + " ожидает мьютекс");
        SharedRes.mtx.WaitOne(); // Получаем мьютекс
        Console.WriteLine(Thrd.Name + " получает мьютекс");

        for (int i = 0; i < num; i++)
        {
            Thread.Sleep(500); // Имитация работы
            SharedRes.Count--;
            Console.WriteLine("в потоке {0}, Count={1}", Thrd.Name, SharedRes.Count);
        }

        Console.WriteLine(Thrd.Name + " освобождает мьютекс");
        SharedRes.mtx.ReleaseMutex(); // Освобождаем мьютекс
    }
}

class Program
{
    static void Main()
    {
        IncThread incThread = new IncThread("Инкрементирующий поток", 5);
        DecThread decThread = new DecThread("Декрементирующий поток", 5);

        incThread.Thrd.Join(); // Ожидаем завершения инкрементирующего потока
        decThread.Thrd.Join(); // Ожидаем завершения декрементирующего потока

        Console.WriteLine("Конечное значение Count: " + SharedRes.Count);
    }
}