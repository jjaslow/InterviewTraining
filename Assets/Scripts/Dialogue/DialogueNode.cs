using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;


namespace RPG.Dialogue
{
    //[System.Serializable]
    public class DialogueNode : ScriptableObject
    {
        //dont need uniqueID anymore...we can use the name of the scriptable object
        //public string uniqueID;

        [SerializeField]
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
#if UNITY_EDITOR
            set
            {
               Undo.RecordObject(this, "Update Dialogue Text");
               text = value;
               EditorUtility.SetDirty(this);
            }
#endif
        }





        [SerializeField]
        private List<string> children = new List<string>();

        public List<string> GetChildren()
        {
            return children;
        }

        public int GetChildrenCount()
        {
            return children.Count;
        }

#if UNITY_EDITOR
        public void AddChild(string node)
        {
            Undo.RecordObject(this, "Add Child Node");
            children.Add(node);
            EditorUtility.SetDirty(this);
        }
        public void RemoveChild(string node)
        {
            Undo.RecordObject(this, "Remove Child Node");
            children.Remove(node);
            EditorUtility.SetDirty(this);
        }
#endif






        [SerializeField] bool isPlayerSpeaking = false;
        [SerializeField] bool isAChoice = false;

        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }

        public bool IsAChoice()
        {
            return isAChoice;
        }

#if UNITY_EDITOR
        public void SetIsPlayerSpeaking(bool val)
        {
            isPlayerSpeaking = val;
            EditorUtility.SetDirty(this);
        }
        public void SetAChoice(bool val)
        {
            isAChoice = val;
            EditorUtility.SetDirty(this);
        }
#endif





        //position and size of the node in the editor
        [SerializeField]
        private Rect rectPosition = new Rect(10,10,200,125);
        public Rect RectPosition
        {
            get
            {
                return rectPosition;
            }
        }

#if UNITY_EDITOR
        public void SetRectPosition(Vector2 val)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rectPosition.position = val;
            EditorUtility.SetDirty(this);
        }
#endif



        [SerializeField]
        string onEnterAction;
        [SerializeField]
        string onExitAction;


        public string GetOnEnterAction()
        {
            return onEnterAction;
        }
        public string GetOnExitAction()
        {
            return onExitAction;
        }

        [SerializeField]
        int score = 0;
        [SerializeField]
        string description;


        public int GetScore()
        {
            return score;
        }
        public string GetDescription()
        {
            return description;
        }


    }

}
