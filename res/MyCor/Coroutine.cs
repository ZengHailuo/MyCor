using System.Collections;

namespace MyCor;

/// <summary>
/// 协程
/// </summary>
public class Coroutine
{
    /// <summary>
    /// 匿名协程池
    /// </summary>
    static CoroutinePool anonymousPool = new CoroutinePool();

    /// <summary>
    /// 静态构造函数
    /// </summary>
    static Coroutine()
    {
        anonymousPool.MoveAsync();
    }



    /// <summary>
    /// 迭代器
    /// </summary>
    IEnumerator enumerator;
    /// <summary>
    /// 要等待的协程
    /// </summary>
    Coroutine wait = null;



    /// <summary>
    /// 当前协程是否被销毁
    /// </summary>
    public bool IsDispose { get; private set; } = false;



    /// <summary>
    /// 创建协程
    /// </summary>
    /// <param name="enumerator">迭代器</param>
    public Coroutine(IEnumerator enumerator)
    {
        this.enumerator = enumerator;
    }

    /// <summary>
    /// 停止协程
    /// </summary>
    public void Stop()
    {
        IsDispose = true;
    }

    /// <summary>
    /// 协程执行一步
    /// </summary>
    /// <returns>协程能否继续执行</returns>
    public bool MoveNext()
    {
        var ret = false;

        if (!IsDispose && enumerator != null)
        {
            if (wait?.IsDispose == true)
                wait = null;

            if (wait == null)
            {
                if (ret = enumerator.MoveNext())
                {
                    if (enumerator.Current is IEnumerator)
                        wait = anonymousPool.Start((IEnumerator)enumerator.Current);
                    else if (enumerator.Current is Coroutine)
                        wait = (Coroutine)enumerator.Current;
                }
            }
            else
                ret = true;
        }

        return ret;
    }
}