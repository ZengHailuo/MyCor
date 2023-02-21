using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
namespace MyCor;

/// <summary>
/// 协程池
/// </summary>
public class CoroutinePool
{
    /// <summary>
    /// 包含所有协程的链表
    /// </summary>
    LinkedList<Coroutine> coroutines = new LinkedList<Coroutine>();
    /// <summary>
    /// 是否正在线程里运行协程
    /// </summary>
    bool runInThread = false;



    /// <summary>
    /// 协程池内协程的数量
    /// </summary>
    public int Count => coroutines.Count;



    /// <summary>
    /// 创建协程
    /// </summary>
    /// <param name="enumerator">迭代器</param>
    /// <returns>协程</returns>
    public Coroutine Start(IEnumerator enumerator)
    {
        var cor = new Coroutine(enumerator);
        RegisterCoroutine(cor);
        return cor;
    }

    /// <summary>
    /// 停止协程
    /// </summary>
    /// <param name="coroutine">协程</param>
    public void Stop(Coroutine coroutine) => coroutine.Stop();

    /// <summary>
    /// 每个协程执行一步
    /// </summary>
    public void MoveNext()
    {
        lock (this)
        {
            var node = coroutines.First;
            while (node != null)
            {
                var next = node.Next;
                if (node.Value.IsDispose)
                    coroutines.Remove(node);
                else if (!node.Value.MoveNext())
                    node.Value.Stop();
                node = next;
            }
        }
    }

    /// <summary>
    /// 当条件满足时一直执行
    /// </summary>
    /// <param name="predicate">条件判断</param>
    public void MoveWhile(Func<bool> predicate)
    {
        while (predicate.Invoke()) MoveNext();
    }

    /// <summary>
    /// 当条件不满足时一直执行
    /// </summary>
    /// <param name="predicate">条件判断</param>
    public void MoveUntil(Func<bool> predicate) => MoveWhile(() => !predicate.Invoke());

    /// <summary>
    /// 当还存在可以执行的协程时一直执行
    /// </summary>
    public void MoveUntilEmpty() => MoveUntil(() => Count == 0);

    /// <summary>
    /// 异步运行协程
    /// </summary>
    public void MoveAsync()
    {
        if (!runInThread)
        {
            runInThread = true;
            Task.Run(() =>
            {
                while (runInThread)
                    MoveNext();
            });
        }
    }

    /// <summary>
    /// 取消异步运行协程
    /// </summary>
    public void StopMoveAsync()
    {
        if (runInThread)
            runInThread = false;
    }

    /// <summary>
    /// 清空协程池内的所有协程
    /// </summary>
    public void Clear() => coroutines.Clear();

    /// <summary>
    /// 手动注册协程到协程池
    /// </summary>
    /// <param name="coroutine">协程</param>
    public void RegisterCoroutine(Coroutine coroutine)
    {
        coroutines.AddLast(coroutine);
    }
}