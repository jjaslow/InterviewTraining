using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            
            //Debug.Log("renaming a Dialogue 2");

            Dialogue dialogue = AssetDatabase.LoadMainAssetAtPath(sourcePath) as Dialogue;
            if (dialogue == null)
            {
                return AssetMoveResult.DidNotMove;
            }

            if (Path.GetDirectoryName(sourcePath) != Path.GetDirectoryName(destinationPath))
            {
                return AssetMoveResult.DidNotMove;
            }

            //Debug.Log("name to start: "+dialogue.name);
            dialogue.name = Path.GetFileNameWithoutExtension(destinationPath);

            //Debug.Log("then: "+dialogue.name + ", " + sourcePath + ", " + destinationPath);

            return AssetMoveResult.DidNotMove;
        }
    }
}