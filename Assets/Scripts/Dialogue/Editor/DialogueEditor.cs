using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.PackageManager.UI;
using UnityEngine;


namespace RPG.Dialogue.Editor
{

    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        GUIStyle nodeStyle = null;
        GUIStyle playerNodeStyle = null;

        DialogueNode draggingNode = null;
        Vector2 dragOffset;
        Vector2 scrollPosition;
        [NonSerialized]
        Vector2 scrollStartPOS = Vector2.zero;

        //fields in EditorWindow class are all serialized automatically. 
        //Its possible that the value persists in the editor and repeats, so we un-serialize it
        [NonSerialized]
        DialogueNode creatingNode = null;
        [NonSerialized]
        DialogueNode deletingNode = null;
        [NonSerialized]
        DialogueNode linkingParentNode = null;
        [NonSerialized]
        DialogueNode linkingSecondNode = null;

        //OnOpenAsset callback Attribute is called when ANY asset is 2x clicked in editor
        //need to check the typeof the asset clicked and if it is a Dialogue then open the window
        //static because it runs for all windows
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (Selection.activeObject is Dialogue)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        //open a window (using a menu dropdown)
        //static because it runs for all windows
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }




        //register callback for Dialogue selection change. Also style the window when it is enabled
        private void OnEnable()
        {
            //callback when a different item is selected in the Editor
            Selection.selectionChanged += OnSelectionChanged;

            //and then run it to test if the selected item at open is a Dialogue
            OnSelectionChanged();

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            nodeStyle.normal.textColor = Color.white;

            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
            playerNodeStyle.normal.textColor = Color.white;
        }

        //need to check the type of the asset selected in the Editor and if it is a Dialogue then 
        //update selected and refresh the screen with the current Nodes
        private void OnSelectionChanged()
        {
            if(Selection.activeObject is Dialogue)
            {
                selectedDialogue = Selection.activeObject as Dialogue;

                //manually call OnGUI to redraw the inspector
                Repaint();
            }
        }




        //render the window contents
        private void OnGUI()
        {
            if (selectedDialogue==null)
            {
                //EditorGUILayout is the auto layout version EXTENSION of EditorGUI
                EditorGUILayout.LabelField("No Dialogue Selected");
            }
            else
            {
                //check for mouse events and then draw each node and connection
                ProcessEvents();

                //start scroll container. but BeginScrollView can only calculate contents for auto layout items inside it.
                //but our nodes are created without auto layout (have specific places and sizes)
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                //so we can set the size of the total contents below. we are establishing an area for auto layout to see.
                //This is hard coded as a large box.
                //TODO:: would be better to calculate based on the smallest and largest node positions???
                Rect canvas = GUILayoutUtility.GetRect(4000, 4000);
                //draw background
                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                //Texture2D is 50px x 50 px so the 3rd argument is the # of times to repeat (so 4000canvas/50px => 80 times)
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, new Rect(0,0,80,80));   

                //break out (and 2x loops) so that all curves get drawn before (and underneath) any of the nodes
                foreach (DialogueNode dn in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(dn);
                }
                foreach (DialogueNode dn in selectedDialogue.GetAllNodes())
                {
                    DrawNode(dn);
                }
                EditorGUILayout.EndScrollView();

                //do the node changes after the drawing foreach loops...
                //actually create or delete any requested nodes
                if (creatingNode != null)
                {
                    selectedDialogue.CreateNewNode(creatingNode);
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }

                //actually do linking and unlinking when both parent and child are chosen
                if(linkingParentNode != null && linkingSecondNode != null)
                {
                    if (linkingParentNode == linkingSecondNode) //pressed cancel
                    {
                        linkingParentNode = null;
                        linkingSecondNode = null;
                    }
                    else if (selectedDialogue.isItAChild(linkingParentNode, linkingSecondNode)) //pressed remove link
                    {
                        selectedDialogue.RemoveChild(linkingParentNode, linkingSecondNode);
                        linkingParentNode = null;
                        linkingSecondNode = null;
                    }
                    else  //pressed add link
                    {
                        selectedDialogue.AddChild(linkingParentNode, linkingSecondNode);
                        linkingParentNode = null;
                        linkingSecondNode = null;
                    }
                }
            } 
        }

 

        //check for mouse click and drag of nodes
        void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                //method to see if click point is inside a Node rect
                draggingNode = GetNodeAtPoint(Event.current.mousePosition);

                if(draggingNode != null)
                {
                    float x = draggingNode.RectPosition.x - Event.current.mousePosition.x;
                    float y = draggingNode.RectPosition.y - Event.current.mousePosition.y;
                    dragOffset = new Vector2(x, y);

                    //what is displayed in the Inspector
                    Selection.activeObject = draggingNode;
                }  
                else
                {
                    scrollStartPOS = Event.current.mousePosition;

                    //what is displayed in the Inspector
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if(Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                draggingNode.SetRectPosition(Event.current.mousePosition + dragOffset);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode == null)
            {
                scrollPosition += scrollStartPOS - Event.current.mousePosition;
                scrollStartPOS = Event.current.mousePosition;
                GUI.changed = true;
            }
            else if(Event.current.type == EventType.MouseUp)    // && draggingNode != null
            {
                draggingNode = null;
                scrollStartPOS = Vector2.zero;
            }
        }


        //draw each node
        private void DrawNode(DialogueNode dn)
        {
            GUIStyle style = nodeStyle;
            if (dn.IsPlayerSpeaking())
                style = playerNodeStyle;

            //create the block
            GUILayout.BeginArea(dn.RectPosition, style);

            //if change then it will run GUI.changed on EndChangeCheck
            EditorGUI.BeginChangeCheck();

            //EditorGUILayout.LabelField("Node:", EditorStyles.whiteLabel);
            //string newID = EditorGUILayout.TextField(dn.uniqueID);

            GUILayoutOption[] options =
            {
                GUILayout.Height(40),
            };
            EditorStyles.textField.wordWrap = true;
            string newText = EditorGUILayout.TextField(dn.Text, options);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("-"))
            {
                //we set the fact that there is a change in nodes here, but we cant change the list of node while
                //inside a foreach loop, so we can make the change after the loop, based on this fact (see above)
                deletingNode = dn;
            }

            DrawLinkButton(dn);

            if (GUILayout.Button("+"))
            {
                //we set the fact that there is a change in nodes here, but we cant change the list of node while
                //inside a foreach loop, so we can make the change after the loop, based on this fact (see above)
                creatingNode = dn;
            }
            GUILayout.EndHorizontal();

            //if there was a change in the editor then propogate the changes to the actual node
            if (EditorGUI.EndChangeCheck())
            {
                dn.Text = newText;
                //dn.uniqueID = newID;
            }

            GUILayout.EndArea();
        }

        private void DrawLinkButton(DialogueNode dn)
        {
            string linkButtonText;
            if (linkingParentNode == null)
                linkButtonText = "link";
            else if (linkingParentNode == dn)
                linkButtonText = "Cancel";
            else if (selectedDialogue.isItAChild(linkingParentNode, dn))
                linkButtonText = "unlink";
            else
                linkButtonText = "child";

            //we set the fact that there is a change in nodes here, but we cant change the list of node while
            //inside a foreach loop, so we can make the change after the loop, based on this fact (see above)
            if (GUILayout.Button(linkButtonText))
            {
                if (linkingParentNode == null)
                    linkingParentNode = dn;
                else
                    linkingSecondNode = dn;
            }
        }

        private void DrawConnections(DialogueNode dn)
        {
            Vector2 startPOS = dn.RectPosition.center + new Vector2(dn.RectPosition.xMax - dn.RectPosition.center.x, 0);

            foreach (DialogueNode child in selectedDialogue.GetAllChildren(dn))
            {
                Vector2 endPOS = child.RectPosition.center - new Vector2(child.RectPosition.center.x - child.RectPosition.xMin, 0);

                //adjust the curves of the lines to make them less harsh
                Vector2 controlPointOffset = endPOS - startPOS;
                controlPointOffset.y = 0;
                controlPointOffset.x *= .8f;

                Handles.DrawBezier(startPOS, endPOS, 
                    startPOS + controlPointOffset, endPOS- controlPointOffset, 
                    Color.white, null, 2f);
            }
        }


        //method to see if click point is inside a Node rect
        DialogueNode GetNodeAtPoint(Vector2 point)
        {
            //get mouse click position in canvas space
            point += scrollPosition;

            foreach(DialogueNode node in selectedDialogue.GetAllNodes().Reverse())
            {
                if (node.RectPosition.Contains(point))
                    return node;
            }
            return null;
        }





    }

}
