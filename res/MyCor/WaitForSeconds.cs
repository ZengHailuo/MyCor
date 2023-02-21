using System;
using System.Collections;
namespace MyCor;

public class WaitForSeconds : IEnumerator
{
    DateTime targetDateTime;
    double seconds;

    /// <summary>
    /// 等待指定时间
    /// </summary>
    /// <param name="seconds">时间（单位：秒）</param>
    public WaitForSeconds(double seconds)
    {
        this.seconds = seconds;
        Reset();
    }

    public object Current => (targetDateTime - DateTime.Now).TotalSeconds;

    public bool MoveNext() => (double)Current > 0;

    public void Reset()
    {
        targetDateTime = DateTime.Now.AddSeconds(seconds);
    }
}