using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System.Windows.Forms;
using System.Runtime.InteropServices;
using Common;

public class ClearMemory : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Click()
    {
        #region System.Windows.Form Implement:Off
        //DialogResult result = MessageBox.Show("你确定要删除所有存储信息吗？\n这会导致游戏数据清零", "确认信息", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
        //if (result == DialogResult.Yes)
        //{
        //    PlayerPrefs.DeleteAll();
        //}
        #endregion

        if(ExternalFunctionality.MessageBox(IntPtr.Zero, "\u4f60\u786e\u5b9a\u8981\u6e05\u7a7a\u6240\u6709\u5b58\u50a8\u5417", "\u64cd\u4f5c\u786e\u8ba4", 1) == 1)
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
