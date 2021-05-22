使用 [`友元`](https://docs.microsoft.com/zh-cn/dotnet/standard/assembly/friend) 来访问Unity的`internal`的API,需要`Unity 2018.1`以上。

在`Unity 2018.1`后官方添加了一些`友元`,可以让我们用来当作胶水来访问``API

具体的[列表](https://github.com/Unity-Technologies/UnityCsReference/blob/2018.1/Editor/Mono/AssemblyInfo/AssemblyInfo.cs)
`InternalAPIEditorBridge xxxx`的都是


本例子会创建一个范例,范例使用`Unity.InternalAPIEngineBridge.006`来进行演示