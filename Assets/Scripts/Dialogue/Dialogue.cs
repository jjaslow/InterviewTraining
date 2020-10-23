using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace RPG.Dialogue
{
    //when we save the file (the Dialogue SO), we want a callback when the file is actually serialized (ie saved) so we can use it
    //so we implemenet ISerializationCallbackReceiver
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        List<DialogueNode> nodes = new List<DialogueNode>();
        [SerializeField]
        Vector2 newNodeOffset = new Vector2(225,0);

        //for child lookups
        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();



        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetNode(int i)
        {
            return nodes[i];
        }

        public int GetNumberOfNodes()
        {
            return nodes.Count;
        }



        //return list of child nodes for a specific parent node
        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            List<DialogueNode> result = new List<DialogueNode>();
            foreach (string child in parentNode.GetChildren())
            {
                if (nodeLookup.ContainsKey(child))
                    result.Add(nodeLookup[child]);
            }
            return result;
        }

        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
        {
            foreach(DialogueNode child in GetAllChildren(currentNode))
            {
                if (child.IsPlayerSpeaking())
                    yield return child;
            }
        }

        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode)
        {
            foreach (DialogueNode child in GetAllChildren(currentNode))
            {
                if (!child.IsPlayerSpeaking())
                    yield return child;
            }
        }



        //initialize the dialogue when first created (in the editor only)
#if UNITY_EDITOR
        private void Awake()
        {
            //if(nodes.Count == 0)
            //{
            //    CreateNewNode(null);
            //}
        }
#endif

        //function is called when the script is loaded or a value is changed in the Inspector
        //when a value is changed in the Inspector (for this dialogue)
        //we want to update the lookup table so we can lookup nodes by uniqueID
        private void OnValidate()
        {
            nodeLookup.Clear();

            foreach (DialogueNode node in nodes)
            {
                nodeLookup.Add(node.name, node);
            }
        }





        public bool isItAChild(DialogueNode parent, DialogueNode child)
        {
            return parent.GetChildren().Contains(child.name);
        }

        public void AddChild(DialogueNode parent, DialogueNode child)
        {
            parent.AddChild(child.name);
            OnValidate();
        }

        public void RemoveChild(DialogueNode parent, DialogueNode child)
        {
            parent.RemoveChild(child.name);
            OnValidate();
        }

#if UNITY_EDITOR
        public void CreateNewNode(DialogueNode parent)
        {
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                Undo.RecordObject(this, "Added New Dialogue Node");
            }

            //dont use the new keyword to create a new SO (as we do for a regular class), 
            //we call CreateInstance<T> to create an instance of a scriptable object
            //DialogueNode node = new DialogueNode();
            DialogueNode node = CreateInstance<DialogueNode>();

            node.name = System.Guid.NewGuid().ToString();


            nodes.Add(node);

            if(parent != null)
            {
                parent.AddChild(node.name);
                node.SetRectPosition(parent.RectPosition.position + newNodeOffset);
                if (!parent.IsPlayerSpeaking())
                    node.SetIsPlayerSpeaking(true);
            }
 

            //unity will actually combine both undos into 1 step
            Undo.RegisterCompleteObjectUndo(node, "Create New Dialogue Node");

            //moved to callback OnBeforeSerialize. Otherwise it may try to add a node to a new Dialogue before Dialogue is saved.
            //AssetDatabase.AddObjectToAsset(node, this);

            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");

            nodes.Remove(nodeToDelete);

            OnValidate();

            RemoveChildren(nodeToDelete);

            AssetDatabase.RemoveObjectFromAsset(nodeToDelete);

            //Destroys the object and records an undo operation so that it can be recreated.
            //need this b/c now the node is a SO (insead of a class) and needs to be manually removed
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        //if we remove a node, remove it from any child references that may exist in other nodes
        private void RemoveChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in nodes)
            {
                if (node.GetChildren().Contains(nodeToDelete.name))
                    node.RemoveChild(nodeToDelete.name);
            }
        }
#endif


        //when you are about to save a file to the hard drive
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                CreateNewNode(null);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in nodes)
                {
                    if(AssetDatabase.GetAssetPath(node) == "")
                        AssetDatabase.AddObjectToAsset(node, this);
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
        }



    }



}



