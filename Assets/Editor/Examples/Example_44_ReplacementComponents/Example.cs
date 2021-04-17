//Create: Icarus
//ヾ(•ω•`)o
//2021-04-17 10:34
//Assembly-CSharp-Editor

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CabinIcarus.EditorFrame.Utils
{
    public class Example
    {
        [MenuItem("CONTEXT/Image/Replace To Test Image")]
        static void _imageReplaceToTestImage(MenuCommand command)
        {
            ((Component) command.context).ReplaceComponent<TestImage>();
        } 
        
        [MenuItem("CONTEXT/TestImage/Replace To Image")]
        static void _testImageReplaceToImage(MenuCommand command)
        {
            ((Component) command.context).ReplaceComponent<Image>();
        } 
    }

    class TestImage:Image
    {
        
    }
}