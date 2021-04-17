//Create: Icarus
//ヾ(•ω•`)o
//2020-10-08 08:26
//Icarus.EditorFrame

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CabinIcarus.EditorFrame.Utils
{
    public static class UnityObjectEx
    {
        private static MonoScript[] _monoScripts;

        [InitializeOnLoadMethod]
        static void _init()
        {
            _monoScripts  = MonoImporter.GetAllRuntimeMonoScripts();
        }

        // public static void AddReplaceContextMenu(Type source,Type target,Action<Component> onReplace)
        // {
        //     var menuPath = $"CONTEXT/{source.Name}/Replace {target.Name}";
        //     Debug.LogError(menuPath);
        //     ProjectMenu.AddMenu(menuPath,100, _menu);
        //     EditorUtility.SelectMenuItemFunction
        // }

        public static T ReplaceComponent<T>(this Component self) where T : Component
        {
            return self.ReplaceComponent(typeof(T)) as T;    
        }
        
        public static Component ReplaceComponent(this Component self, Type type)
        {
            if (!typeof(Component).IsAssignableFrom(type) || self.GetType() == type)
            {
                return null;
            }

            var index = Undo.GetCurrentGroup();
                
            Undo.SetCurrentGroupName($"{self.GetType().Name} Replace To {type.Name}");
            
            var go = self.gameObject;

            if (PrefabUtility.IsPartOfPrefabInstance(go))
            {
                PrefabUtility.ApplyPrefabInstance(PrefabUtility.GetNearestPrefabInstanceRoot(go),InteractionMode.AutomatedAction);
                
                Undo.DestroyObjectImmediate(self);
                var c = Undo.AddComponent(go, type);
                Undo.CollapseUndoOperations(index + 1);
                
                var removeC = PrefabUtility.GetRemovedComponents(go);
            
                EditorUtility.CopySerializedManagedFieldsOnly(removeC.Last().assetComponent, c);

                return c;
            }

            var serobj = new SerializedObject(self);
            
            MonoScript newMono = null;
            
            foreach (var monoScript in _monoScripts)
            {
                if (monoScript == null)
                {
                    _init();

                    return ReplaceComponent(self, type);
                }
                
                if (monoScript.GetClass() == type)
                {
                    newMono = monoScript;
                    break;
                }
            }
         
            if (newMono == null)
            {
                return null;
            }
                
            var property = serobj.FindProperty("m_Script");
                
            property.objectReferenceValue = newMono;
            serobj.ApplyModifiedProperties();
            serobj.Update();
            
            return (Component) serobj.targetObject;
        }
    }
}