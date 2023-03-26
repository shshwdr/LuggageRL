using Sinbad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TextInfo
{
    public string Name;
    public string Text;
}
public class TutorialManager : Singleton<TutorialManager>
{
    bool SkipTutorial = false;
    public Dictionary<string, bool> read = new Dictionary<string, bool>();
    public Dictionary<string, TextInfo> textDict = new Dictionary<string, TextInfo>();
    // Start is called before the first frame update
    void Start()
    {

        var itemInfos = CsvUtil.LoadObjects<TextInfo>("texts");
        foreach (var item in itemInfos)
        {
            textDict[item.Name] = item;
        }
        DialoguePopupManager.Instance.showDialogue(TutorialManager.Instance.getText("Stranded"));
        //DetailView.Instance.showTutorial("Stranded", TutorialManager.Instance.getUnreadText("Stranded"));
    }
    public TextInfo getTextInfo(string itemName)
    {
        if (textDict.ContainsKey(itemName))
        {
            return textDict[itemName];

        }
        Debug.LogError("no text " + itemName);
        return textDict["PickupSelection"];
    }
    public string getUnreadText(string itemName)
    {
        if (SkipTutorial)
        {

            return "";
        }
        if (read.ContainsKey(itemName))
        {
            return "";
        }
        return getText(itemName);
    }
    public string getText(string itemName)
    {
        var info = getTextInfo(itemName);
        read[itemName] = true;
        return info.Text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
