# MyCor

### 开始
测试代码直接写在Program.cs里面了

### 对象池的运行
CoroutinePool对象必须持续执行MoveNext方法才能正常运行。

可以通过以下两种方式启动CoroutinePool

#### 1.同步方式

调用协程池的MoveWhile、MoveUntil或MoveUntilEmpty方法都可以以同步的方式运行协程池。

这种方式是阻塞式的，这些方法内部都是循环，满足指定条件才会结束循环。循环结束后，也意味着协程池不运行了。

#### 2.异步方式

调用协程池的MoveAsync方法可以以异步方式运行协程池。

这种方式是非阻塞式的，当主函数结束时或者调用StopMoveAsync方法时才能停止协程池的运行。