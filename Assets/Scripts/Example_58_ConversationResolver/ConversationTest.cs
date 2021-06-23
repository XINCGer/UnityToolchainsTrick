#region 注 释
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 
 *
 */
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CZToolKit.ConversationResolver.Examples
{
    public class ConversationTest : MonoBehaviour
    {
        [TextArea(4, 30)]
        [InspectorName("已有关键字")]
        public string tip;

        [TextArea(4, 30)]
        public string conversationText;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
        public void Resolver()
        {
            GetComponent<ConversationManager>().BeginConversation(conversationText);
        }
#endif
    }
}
