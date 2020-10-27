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
            if (AssetDatabase.LoadMainAssetAtPath(sourcePath) is Dialogue)
            {
                Debug.Log("renaming a Dialogue");
                //yes, we are renaming a Dialog SO

                //if renaming then the source and destination path will be the same...
                if (Path.GetDirectoryName(sourcePath) == Path.GetDirectoryName(destinationPath))
                {
                    //MOVE HAPPENING, On Dialogue SO, and its a rename, not a directory move
                    Dialogue dialogue = AssetDatabase.LoadMainAssetAtPath(sourcePath) as Dialogue;

                    dialogue.name = Path.GetFileNameWithoutExtension(destinationPath);
                    return AssetMoveResult.DidNotMove;
                }
            }
            //else we are moving to a different directory (and NOT renaming)
            //renaming any other asset so we dont do anything
            return AssetMoveResult.DidNotMove;
        }
    }










}
