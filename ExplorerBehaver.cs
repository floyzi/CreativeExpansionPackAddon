using UnityEngine;
using UnhollowerRuntimeLib;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace FraggleExpansion
{
    public class ExplorerBehaver : MonoBehaviour
    {

        public static ExplorerBehaver Instance;
        public float updateInterval = 1f;
        public float timer = 0.0f;

        float accum = 0.0f;
        int frames = 0;
        float timeleft;
        float fps;
        GUIStyle textStyle = new GUIStyle();

        
        void Start()
        {
            timer = 0;
            timeleft = updateInterval;
            textStyle.normal.textColor = Color.green;
            textStyle.fontSize = 15;
        }

        public void OnGUI()
        {

            if (FraggleExpansionData.ToggleFPSCounter)
            {
                GUI.Label(new Rect(10, 10, 100, 25), fps.ToString("F2") + " FPS", textStyle);
            }

            if (FraggleExpansionData.ToggleGUI == true)
            {
                if (FraggleExpansionData.AdaptiveFPSCounter)
                {
                    GUI.Label(new Rect(250, 10, 100, 25), fps.ToString("F2") + " FPS", textStyle);
                }
                GUI.Box(new Rect(10, 10, 230, 169), "");
                GUI.Label(new Rect(15, 15, 230, 125), "<b>CEP Addon Menu</b>");
                GuiInput.SlimeSpeedPercentage = GUI.TextField(new Rect(15, 40, 100, 20), GuiInput.SlimeSpeedPercentage, 9);
                GUI.Label(new Rect(120, 40, 300, 150), "Slime speed");
                GuiInput.SlimeHeightMIN = GUI.TextField(new Rect(15, 60, 100, 20), GuiInput.SlimeHeightMIN, 9);
                GUI.Label(new Rect(120, 60, 300, 150), "Slime Height Min.");
                GuiInput.SlimeHeightMAX = GUI.TextField(new Rect(15, 80, 100, 20), GuiInput.SlimeHeightMAX, 9);
                GUI.Label(new Rect(120, 80, 300, 150), "Slime Height Max.");
                if (GUI.Button(new Rect(15, 100, 100, 20), "Update values"))
                {
                    GameObject.Find("(singleton) LevelEditorOptionsSingleton");
                    var EditorSingleton = LevelEditorOptionsSingleton.Instance;
                    DataRisingSlime.SlimeSpeedPercentage = int.Parse(GuiInput.SlimeSpeedPercentage);
                    EditorSingleton.SlimeHeightMIN = int.Parse(GuiInput.SlimeHeightMIN);
                    EditorSingleton.SlimeHeightMAX = int.Parse(GuiInput.SlimeHeightMAX);
                }
                GUI.Label(new Rect(15, 120, 300, 150), "By floyzi102 on Twitter");
                GUI.Label(new Rect(200, 160, 230, 150), "<size=10>130823</size>");
                GUI.Label(new Rect(15, 135, 230, 125), "<size=10>press F1 to toggle ui, F2 to toggle cursor, F3 to toggle settings ui, END to open creator menu (only in FGC)</size>");


                if (FraggleExpansionData.ToggleSettingsGUI == true)
                {
                    GUI.Box(new Rect(10, 190, 230, 315), "");
                    GUI.Label(new Rect(15, 195, 230, 141), "<b>CEP Settings</b>");
                    FraggleExpansionData.AddVanillaObjects = GUI.Toggle(new Rect(15, 215, 180, 20), FraggleExpansionData.AddVanillaObjects, "<color=yellow>Add Original Theme Objects</color>");
                    FraggleExpansionData.AddRetroObjects = GUI.Toggle(new Rect(15, 235, 175, 20), FraggleExpansionData.AddRetroObjects, "<color=yellow>Add Retro Theme Objects</color>");
                    FraggleExpansionData.CanClipObjects = GUI.Toggle(new Rect(15, 255, 120, 20), FraggleExpansionData.CanClipObjects, "Let Objects Clip");
                    FraggleExpansionData.InsanePainterSize = GUI.Toggle(new Rect(15, 275, 140, 20), FraggleExpansionData.InsanePainterSize, "<color=yellow>Infinity Scaleable</color>");
                    FraggleExpansionData.BetaWalls = GUI.Toggle(new Rect(15, 295, 100, 20), FraggleExpansionData.BetaWalls, "Beta Walls");
                    FraggleExpansionData.LastPostion = GUI.Toggle(new Rect(15, 315, 150, 20), FraggleExpansionData.LastPostion, "Last Spawn Position");
                    FraggleExpansionData.UseMainSkinInExploreState = GUI.Toggle(new Rect(15, 335, 150, 20), FraggleExpansionData.UseMainSkinInExploreState, "Custom Skin");
                    FraggleExpansionData.BypassBounds = GUI.Toggle(new Rect(15, 355, 150, 20), FraggleExpansionData.BypassBounds, "<color=yellow>Bypass Bounds</color>");
                    FraggleExpansionData.RemoveRotation = GUI.Toggle(new Rect(15, 375, 150, 20), FraggleExpansionData.RemoveRotation, "<color=yellow>Remove Rotation Limit</color>");
                    FraggleExpansionData.AdaptiveFPSCounter = GUI.Toggle(new Rect(15, 395, 150, 20), FraggleExpansionData.AdaptiveFPSCounter, "FPS Counter");
                    GUI.Label(new Rect(15, 415, 230, 141), "<size=10>Options that colored by <color=yellow>yellow color</color> may require restart of FG creative</size>");
                    GUI.Label(new Rect(15, 445, 230, 141), "<size=10>If you want to turn something off by default you can do this in txt config that located it the mod folder</size>");

                }

            }
        }

        public static void Init()
        {
            ClassInjector.RegisterTypeInIl2Cpp<ExplorerBehaver>();
            var BehaverGameObject = new GameObject("Creative Expansion Pack | Behaviour");
            Instance = BehaverGameObject.AddComponent<ExplorerBehaver>();
            UnityEngine.Object.DontDestroyOnLoad(BehaverGameObject);
            SceneManager.sceneLoaded = ((ReferenceEquals(SceneManager.sceneLoaded, null)) ? new System.Action<Scene, LoadSceneMode>(OnSceneLoaded) : Il2CppSystem.Delegate.Combine(SceneManager.sceneLoaded, (UnityAction<Scene, LoadSceneMode>)new System.Action<Scene, LoadSceneMode>(OnSceneLoaded)).Cast<UnityAction<Scene, LoadSceneMode>>());

        }



        public static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            Main.Instance.OnSceneWasLoaded();
        }

        public void Update()
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;
            if (timeleft <= 0.0)
            {
                fps = (accum / frames);
                timeleft = updateInterval;
                accum = 0f;
                frames = 0;
            }
            Main.Instance.OnUpdate();
        }
    }
}
