using UnityEngine;

namespace UnityCourse2022
{
    /// <summary>
    /// スクリーンショット
    /// </summary>
    public class ScreenShot : MonoBehaviour
    {
        #region DEFINITION
        static string SCREENSHOT_PATH = "Assets";
        #endregion

        #region VARIABLE
        [SerializeField] KeyCode _screenShot = KeyCode.S;
        #endregion

        #region UNITY_EVENT
        void Update()
        {
            if (Input.GetKeyDown(_screenShot))
            {
                _capture();
            }
        }
        #endregion

        #region PUBLIC_METHODS
        #endregion

        #region PRIVATE_METHODS
        void _capture()
        {
            string date = System.DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string path = string.Format("{0}/{1}.png", SCREENSHOT_PATH, date);
            ScreenCapture.CaptureScreenshot(path);
            Debug.LogFormat("Screen shot: {0}", path);
        }
        #endregion
    }
}