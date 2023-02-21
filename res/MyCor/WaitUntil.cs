using System;
using System.Collections;
namespace MyCor;

public class WaitUntil : IEnumerator
{
    Func<bool> predicate;

    /// <summary>
    /// 等待直到满足条件
    /// </summary>
    /// <param name="predicate">条件判断</param>
    public WaitUntil(Func<bool> predicate)
    {
        this.predicate = predicate;
    }

    public object Current => predicate.Invoke();

    public bool MoveNext() => !(bool)Current;

    public void Reset() { }
}