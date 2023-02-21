using System.Collections;
using System;
using MyCor;

public class Program
{
    static CoroutinePool pool = new CoroutinePool();

    public static void Main(String[] args)
    {
        // 开启协程
        var cor = pool.Start(MainCoroutine());
        pool.Start(TimerCoroutine());

        pool.MoveUntilEmpty(); // 死循环，一直执行直到所有协程都结束
    }

    static IEnumerator MainCoroutine()
    {
        for (int i = 0; i < 10; ++i)
        {
            Console.WriteLine($"Main {i}");
            if (i == 5)
                yield return pool.Start(SubCoroutine()); // 等待SubCoroutine结束后
            else
                yield return null;
        }
    }

    static IEnumerator SubCoroutine()
    {
        for (int i = 0; i < 10; ++i)
        {
            Console.WriteLine($"Sub {i}");
            yield return null;
        }
    }

    static IEnumerator TimerCoroutine()
    {
        for (int i = 0; i < 5; ++i)
        {
            Console.WriteLine($"Timer {i}");
            yield return new WaitForSeconds(1); // 等待1秒
        }
    }
}