using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.TreeViewExamples
{
	
	[CreateAssetMenu (fileName = "TreeDataAsset", menuName = "Tree Asset", order = 1)]
	public class MyTreeAsset : ScriptableObject
	{
		[SerializeField] List<MyTreeElement> m_TreeElements = new List<MyTreeElement> ();

		internal List<MyTreeElement> treeElements
		{
			get { return m_TreeElements; }
			set { m_TreeElements = value; }
		}

		void Awake ()
		{
			if (m_TreeElements.Count == 0)
				m_TreeElements = MyTreeElementGenerator.GenerateRandomTree(160);
		}
	}
}
