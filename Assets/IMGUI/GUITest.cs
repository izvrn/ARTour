using UnityEngine;

public class GUITest_2 : MonoBehaviour
{
    private int _menuWidthPercent = 30;
    private int _menuHeightPercent = 80;
    
    private int _buttonWidthPercent = 90;
    private int _buttonSpacingPercent = 20;

    private int _buttonsCount = 5;

    public GUISkin Skin;
    
    private void OnGUI()
    {
        GUI.skin = Skin;
        
        var menuWidth = Screen.width * _menuWidthPercent * 0.01f;
        var menuHeight = Screen.height * _menuHeightPercent * 0.01f;
        
        var x = (Screen.width - menuWidth) * 0.5f;
        var y = (Screen.height - menuHeight) * 0.5f;
        
        GUI.BeginGroup (new Rect (x, y, menuWidth, menuHeight));
        
            /* GUI.Box(new Rect(x, y, menuWidth, menuHeight), "Loader Menu");

            var buttonWidth = menuWidth * _buttonWidthPercent * 0.01f;
            var buttonHeight = menuHeight / _buttonsCount * (1 - _buttonSpacingPercent * 0.01f);
            
            x = (Screen.width - buttonWidth) * 0.5f;
            var buttonSpacing = buttonHeight * _buttonSpacingPercent * 0.01f;

            for (int i = 0; i < _buttonsCount; i++)
            {
                if (GUI.Button(new Rect(x, y + buttonHeight * i + buttonSpacing * (i + 1), buttonWidth, buttonHeight),
                        "Level " + (i + 1)))
                {
                    Application.LoadLevel(i + 1);
                }
            } */
            
            GUILayout.Box("Loader Menu");
            for (int i = 0; i < _buttonsCount; i++)
            {
                if (GUILayout.Button("Level " + (i + 1)))
                {
                    Application.LoadLevel(i + 1);
                }
            } 
            
        GUI.EndGroup();
    }
}
