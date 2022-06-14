using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class ShaderPreprocessor : IPreprocessShaders
{
    static ShaderKeyword[] s_uselessKeywords;

    public int callbackOrder
    {
        get { return 0; } // 可以指定多个处理器之间回调的顺序
    }

    static ShaderPreprocessor()
    {
        s_uselessKeywords = new ShaderKeyword[]
        {
            new ShaderKeyword("RED"),
        };
    }

    public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
    {
        Debug.Log($"Shader:{shader.name}要编译啦");
        // for (int i = data.Count - 1; i >= 0; --i)
        // {
        //     for (int j = 0; j < s_uselessKeywords.Length; ++j)
        //     {
        //         if (data[i].shaderKeywordSet.IsEnabled(s_uselessKeywords[j]))
        //         {
        //             data.RemoveAt(i);
        //             break;
        //         }
        //     }
        // }
    }
}
